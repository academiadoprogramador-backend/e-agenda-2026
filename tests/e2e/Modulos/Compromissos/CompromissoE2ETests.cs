using eAgenda.Testes.e2e.Compartilhado;
using Microsoft.Playwright;

namespace eAgenda.Testes.e2e.Modulos.Compromissos;

[TestClass]
public sealed class CompromissoE2ETests : E2ETestsBase
{
    [TestMethod]
    public async Task Deve_Cadastrar_Compromisso_Corretamente()
    {
        // Arranjo
        await AutenticarUsuario();

        await page.GotoAsync($"{baseUrl}/Compromisso/Cadastrar");

        // Ação
        await page.GetByLabel("Assunto").FillAsync("Reunião com cliente");
        await page.GetByLabel("Data de Ocorrência").FillAsync("2026-12-20");
        await page.GetByLabel("Hora de Início").FillAsync("09:00");
        await page.GetByLabel("Hora de Término").FillAsync("10:30");
        await page.GetByLabel("Tipo de Compromisso").SelectOptionAsync("Presencial");
        await page.GetByLabel("Local").FillAsync("Escritório Central");

        await page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();

        ILocator card = page.Locator(".card").Filter(new() { HasText = "Reunião com cliente" });

        // Asserção
        await Assertions.Expect(card).ToBeVisibleAsync();
        await Assertions.Expect(card.Locator(".card-title")).ToHaveTextAsync("Reunião com cliente");
        await Assertions.Expect(card.GetByText("Presencial", new() { Exact = true })).ToBeVisibleAsync();
        await Assertions.Expect(card.GetByText("20/12/2026", new() { Exact = true })).ToBeVisibleAsync();
        await Assertions.Expect(card.GetByText("09:00 - 10:30", new() { Exact = true })).ToBeVisibleAsync();
        await Assertions.Expect(card.GetByText("Escritório Central", new() { Exact = true })).ToBeVisibleAsync();
        await Assertions.Expect(card.GetByText("Não informado", new() { Exact = true })).ToHaveCountAsync(2);
    }

    [TestMethod]
    public async Task Deve_Editar_Compromisso_Corretamente()
    {
        // Arranjo
        await AutenticarUsuario();

        await page.GotoAsync($"{baseUrl}/Compromisso/Cadastrar");

        await page.GetByLabel("Assunto").FillAsync("Reunião com cliente");
        await page.GetByLabel("Data de Ocorrência").FillAsync("2026-12-20");
        await page.GetByLabel("Hora de Início").FillAsync("09:00");
        await page.GetByLabel("Hora de Término").FillAsync("10:30");
        await page.GetByLabel("Tipo de Compromisso").SelectOptionAsync("Presencial");
        await page.GetByLabel("Local").FillAsync("Escritório Central");
        await page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();

        ILocator card = page.Locator(".card").Filter(new() { HasText = "Reunião com cliente" });

        await card.GetByRole(AriaRole.Link, new() { Name = "Editar", Exact = true }).ClickAsync();

        // Ação
        await page.GetByLabel("Assunto").FillAsync("Planejamento trimestral");
        await page.GetByLabel("Data de Ocorrência").FillAsync("2027-01-15");
        await page.GetByLabel("Hora de Início").FillAsync("14:00");
        await page.GetByLabel("Hora de Término").FillAsync("15:30");
        await page.GetByLabel("Tipo de Compromisso").SelectOptionAsync("Remoto");
        await page.GetByLabel("Local").FillAsync(string.Empty);
        await page.GetByLabel("Link").FillAsync("https://meet.example.com/planejamento");

        await page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();

        await page.GotoAsync($"{baseUrl}/Compromisso/Listar");

        ILocator cardEditado = page.Locator(".card").Filter(new() { HasText = "Planejamento trimestral" });

        // Asserção
        await Assertions.Expect(cardEditado).ToHaveCountAsync(1);
        await Assertions.Expect(cardEditado.Locator(".card-title")).ToHaveTextAsync("Planejamento trimestral");
        await Assertions.Expect(cardEditado.GetByText("Remoto", new() { Exact = true })).ToBeVisibleAsync();
        await Assertions.Expect(cardEditado.GetByText("15/01/2027", new() { Exact = true })).ToBeVisibleAsync();
        await Assertions.Expect(cardEditado.GetByText("14:00 - 15:30", new() { Exact = true })).ToBeVisibleAsync();
        await Assertions.Expect(cardEditado.GetByText("https://meet.example.com/planejamento", new() { Exact = true })).ToBeVisibleAsync();
        await Assertions.Expect(cardEditado.GetByText("Não informado", new() { Exact = true })).ToHaveCountAsync(2);
        await Assertions.Expect(card).ToHaveCountAsync(0);
    }

    [TestMethod]
    public async Task Deve_Excluir_Compromisso_Corretamente()
    {
        // Arranjo
        await AutenticarUsuario();

        await page.GotoAsync($"{baseUrl}/Compromisso/Cadastrar");

        await page.GetByLabel("Assunto").FillAsync("Reunião com cliente");
        await page.GetByLabel("Data de Ocorrência").FillAsync("2026-12-20");
        await page.GetByLabel("Hora de Início").FillAsync("09:00");
        await page.GetByLabel("Hora de Término").FillAsync("10:30");
        await page.GetByLabel("Tipo de Compromisso").SelectOptionAsync("Presencial");
        await page.GetByLabel("Local").FillAsync("Escritório Central");
        await page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();

        ILocator card = page.Locator(".card").Filter(new() { HasText = "Reunião com cliente" });

        await Assertions.Expect(card).ToBeVisibleAsync();

        // Ação
        await card.GetByRole(AriaRole.Link, new() { Name = "Excluir", Exact = true }).ClickAsync();
        await page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();

        await page.GotoAsync($"{baseUrl}/Compromisso/Listar");

        // Asserção
        await Assertions.Expect(card).ToHaveCountAsync(0);
        await Assertions.Expect(page.GetByRole(AriaRole.Alert))
            .ToHaveTextAsync("Nenhum compromisso cadastrado.");
    }
}
