using eAgenda.Dominio.Modulos.Tarefas;
using eAgenda.Infraestrutura.Modulos.Tarefas;

namespace eAgenda.Testes.Integracao.Tarefas;

[TestClass]
public sealed partial class RepositorioTarefaEmOrmTests : RepositorioEmOrmTestsBase
{
    [TestMethod]
    public void Cadastrar_DevePersistir_TarefaEItensNoBanco()
    {
        // Arranjo
        Guid tarefaId = Guid.CreateVersion7();

        Tarefa tarefa = new Tarefa("Preparar Apresentação", PrioridadeTarefa.Alta)
        {
            Id = tarefaId,
            UsuarioId = usuarioId,
            DataCriacao = new DateOnly(2026, 11, 03)
        };

        tarefa.AdicionarItem(new ItemTarefa(tarefa, "Revisar conteúdo") { UsuarioId = usuarioId });
        tarefa.AdicionarItem(new ItemTarefa(tarefa, "Montar slides") { UsuarioId = usuarioId, Concluido = true });

        RepositorioTarefaEmOrm repositorioTarefa = new RepositorioTarefaEmOrm(dbContext);

        // Ação
        repositorioTarefa.Cadastrar(tarefa);
        dbContext.ChangeTracker.Clear();

        Tarefa? tarefaSelecionada = repositorioTarefa.SelecionarPorId(tarefaId);

        // Asserção
        Assert.IsNotNull(tarefaSelecionada);
        Assert.AreEqual("Preparar Apresentação", tarefaSelecionada.Titulo);
        Assert.AreEqual(PrioridadeTarefa.Alta, tarefaSelecionada.Prioridade);
        Assert.AreEqual(new DateOnly(2026, 11, 03), tarefaSelecionada.DataCriacao);
        Assert.AreEqual(50, tarefaSelecionada.PercentualConcluido);
        Assert.HasCount(2, tarefaSelecionada.Itens);

        CollectionAssert.AreEqual(
            new[] { "Montar slides", "Revisar conteúdo" },
            tarefaSelecionada.Itens.Select(i => i.Titulo).ToArray()
        );
    }

    [TestMethod]
    public void Editar_DeveAtualizar_TarefaEItensNoBanco()
    {
        // Arranjo

        // Ação

        // Asserção
    }

    [TestMethod]
    public void Excluir_DeveApagar_TarefaEItensNoBanco()
    {
        // Arranjo

        // Ação

        // Asserção
    }
}
