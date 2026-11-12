using eAgenda.Infraestrutura.Compartilhado.Orm;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace eAgenda.Testes.e2e.Compartilhado;

public sealed class TestApplicationFactory : WebApplicationFactory<Program>
{
    public string? BaseUrl { get; private set; } = null;

    public async Task StartAsync()
    {
        StartupTestServer();

        await CleanupTestDatabaseAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
    }

    private void StartupTestServer()
    {
        UseKestrel(0);
        StartServer();

        IServer server = Services.GetRequiredService<IServer>();

        IServerAddressesFeature? serverAdressesFeature = server.Features.Get<IServerAddressesFeature>()
            ?? throw new InvalidOperationException("Não foi possível obter a URL do servidor");

        BaseUrl = serverAdressesFeature.Addresses.Single();
    }

    private async Task CleanupTestDatabaseAsync()
    {
        using var scope = Services.CreateScope();
        EAgendaDbContext dbContext = scope.ServiceProvider.GetRequiredService<EAgendaDbContext>();

        // O setup não possui usuário autenticado e precisa limpar os dados de todos os usuários.
        dbContext.Despesas.RemoveRange(dbContext.Despesas.IgnoreQueryFilters());
        dbContext.Categorias.RemoveRange(dbContext.Categorias.IgnoreQueryFilters());
        dbContext.ItensTarefa.RemoveRange(dbContext.ItensTarefa.IgnoreQueryFilters());
        dbContext.Tarefas.RemoveRange(dbContext.Tarefas.IgnoreQueryFilters());
        dbContext.Compromissos.RemoveRange(dbContext.Compromissos.IgnoreQueryFilters());
        dbContext.Contatos.RemoveRange(dbContext.Contatos.IgnoreQueryFilters());

        dbContext.Users.RemoveRange(dbContext.Users);

        await dbContext.SaveChangesAsync();
    }
}
