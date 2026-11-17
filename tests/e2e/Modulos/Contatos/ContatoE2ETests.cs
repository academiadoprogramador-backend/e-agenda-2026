using eAgenda.Testes.e2e.Compartilhado;
using Microsoft.Playwright;

namespace eAgenda.Testes.e2e.Modulos.Contatos;

[TestClass]
public sealed class ContatoE2ETests : E2ETestsBase
{
    [TestMethod]
    public async Task Deve_Cadastrar_Contato_Corretamente()
    {
        // Arranjo
        await CadastrarUsuarioAsync();

        await page.GotoAsync($"{baseUrl}/Contato/Cadastrar");

        // Ação
        await page.GetByLabel("Nome").FillAsync("João Silva");
        await page.GetByLabel("E-mail").FillAsync("joao.silva@academiadoprogramador.net");
        await page.GetByLabel("Telefone").FillAsync("(48) 99999-8888");
        await page.GetByLabel("Cargo").FillAsync("Testador");
        await page.GetByLabel("Empresa").FillAsync("Academia do Programador");

        await page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();

        // Após redirecionamento do cadastro para listagem
        ILocator card = page.Locator(".card").Filter(new() { HasText = "João Silva" });

        // Asserção
        await Assertions.Expect(card).ToBeVisibleAsync();
        await Assertions.Expect(card.Locator(".card-title")).ToHaveTextAsync("João Silva");
        await Assertions.Expect(card.GetByText("joao.silva@academiadoprogramador.net", new() { Exact = true })).ToBeVisibleAsync();
        await Assertions.Expect(card.GetByText("(48) 99999-8888", new() { Exact = true })).ToBeVisibleAsync();
        await Assertions.Expect(card.GetByText("Testador", new() { Exact = true })).ToBeVisibleAsync();
        await Assertions.Expect(card.GetByText("Academia do Programador", new() { Exact = true })).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task Deve_Editar_Contato_Corretamente()
    {
        // Arranjo
        await CadastrarUsuarioAsync();

        await page.GotoAsync($"{baseUrl}/Contato/Cadastrar");

        await page.GetByLabel("Nome").FillAsync("João Silva");
        await page.GetByLabel("E-mail").FillAsync("joao.silva@academiadoprogramador.net");
        await page.GetByLabel("Telefone").FillAsync("(48) 99999-8888");
        await page.GetByLabel("Cargo").FillAsync("Testador");
        await page.GetByLabel("Empresa").FillAsync("Academia do Programador");

        await page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();

        // Após redirecionamento do cadastro para listagem
        ILocator card = page.Locator(".card").Filter(new() { HasText = "João Silva" });

        await card.GetByRole(AriaRole.Link, new() { Name = "Editar", Exact = true }).ClickAsync();

        // Ação
        await page.GetByLabel("Nome").FillAsync("Maria Souza");
        await page.GetByLabel("E-mail").FillAsync("maria.souza@academiadoprogramador.net");
        await page.GetByLabel("Telefone").FillAsync("(48) 98888-9999");
        await page.GetByLabel("Cargo").FillAsync("Desenvolvedora");
        await page.GetByLabel("Empresa").FillAsync("Academia do Programador");

        await page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();

        ILocator cardEditado = page.Locator(".card").Filter(new() { HasText = "Maria Souza" });

        // Asserção
        await Assertions.Expect(cardEditado).ToBeVisibleAsync();
        await Assertions.Expect(cardEditado.Locator(".card-title")).ToHaveTextAsync("Maria Souza");
        await Assertions.Expect(cardEditado.GetByText("maria.souza@academiadoprogramador.net", new() { Exact = true })).ToBeVisibleAsync();
        await Assertions.Expect(cardEditado.GetByText("(48) 98888-9999", new() { Exact = true })).ToBeVisibleAsync();
        await Assertions.Expect(cardEditado.GetByText("Desenvolvedora", new() { Exact = true })).ToBeVisibleAsync();
        await Assertions.Expect(cardEditado.GetByText("Academia do Programador", new() { Exact = true })).ToBeVisibleAsync();
        await Assertions.Expect(card).ToHaveCountAsync(0);
    }

    [TestMethod]
    public async Task Deve_Excluir_Contato_Corretamente()
    {
        // Arranjo
        await CadastrarUsuarioAsync();

        await page.GotoAsync($"{baseUrl}/Contato/Cadastrar");

        await page.GetByLabel("Nome").FillAsync("João Silva");
        await page.GetByLabel("E-mail").FillAsync("joao.silva@academiadoprogramador.net");
        await page.GetByLabel("Telefone").FillAsync("(48) 99999-8888");
        await page.GetByLabel("Cargo").FillAsync("Testador");
        await page.GetByLabel("Empresa").FillAsync("Academia do Programador");

        await page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();

        // Após redirecionamento do cadastro para listagem
        ILocator card = page.Locator(".card").Filter(new() { HasText = "João Silva" });

        await card.GetByRole(AriaRole.Link, new() { Name = "Excluir", Exact = true }).ClickAsync();

        // Ação
        await page.GetByRole(AriaRole.Button, new() { Name = "Confirmar", Exact = true }).ClickAsync();

        // Asserção
        await Assertions.Expect(card).ToHaveCountAsync(0);
        await Assertions.Expect(page.GetByRole(AriaRole.Alert))
            .ToHaveTextAsync("Nenhum contato cadastrado.");
    }
}
