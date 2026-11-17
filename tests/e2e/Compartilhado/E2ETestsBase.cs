using Microsoft.Playwright;

namespace eAgenda.Testes.e2e.Compartilhado;

public abstract class E2ETestsBase
{
    private TestApplicationFactory applicationFactory = null!;
    private IPlaywright playwright = null!;
    private IBrowser browser = null!;
    private IBrowserContext browserContext = null!;
    protected IPage page = null!;
    protected string? baseUrl = null;

    // Em CI o navegador roda em modo headless; localmente abre a interface para facilitar a depuração.
    // Use PLAYWRIGHT_HEADLESS=true/false para forçar o modo desejado.
    private static bool ModoHeadless
    {
        get
        {
            string? configuracao = Environment.GetEnvironmentVariable("PLAYWRIGHT_HEADLESS");

            if (bool.TryParse(configuracao, out bool headless))
                return headless;

            return string.Equals(Environment.GetEnvironmentVariable("CI"), "true", StringComparison.OrdinalIgnoreCase);
        }
    }

    [TestInitialize]
    public async Task InicializarTeste()
    {
        applicationFactory = new TestApplicationFactory();

        await applicationFactory.StartAsync();
        baseUrl = applicationFactory.BaseUrl;

        playwright = await Playwright.CreateAsync();
        browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = ModoHeadless
        });
        browserContext = await browser.NewContextAsync();
        page = await browserContext.NewPageAsync();
    }

    [TestCleanup]
    public async Task FinalizarTeste()
    {
        await browser.DisposeAsync();
        playwright.Dispose();

        await applicationFactory.DisposeAsync();
    }

    protected async Task AutenticarUsuario()
    {
        await page.GotoAsync($"{baseUrl}/Autenticacao/Cadastro");

        await page.GetByLabel("E-mail").FillAsync("tiago@academiadoprogramador.net");
        await page.GetByLabel("Senha", new() { Exact = true }).FillAsync("Teste@123");
        await page.GetByLabel("Confirmar Senha", new() { Exact = true }).FillAsync("Teste@123");
        await page.GetByRole(AriaRole.Button, new() { Name = "Cadastrar" }).ClickAsync();

        await page.GotoAsync($"{baseUrl}/Autenticacao/Login");

        await page.GetByLabel("E-mail").FillAsync("tiago@academiadoprogramador.net");
        await page.GetByLabel("Senha").FillAsync("Teste@123");
        await page.GetByRole(AriaRole.Button, new() { Name = "Entrar" }).ClickAsync();
    }
}
