namespace eAgenda.Dominio.Compartilhado;

public sealed class ErroValidacao(string campo, string mensagem)
{
    public string Campo { get; set; } = campo;
    public string Mensagem { get; set; } = mensagem;
}

public abstract class EntidadeBase<T>
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public abstract List<ErroValidacao> Validar();
    public abstract void Atualizar(T entidadeAtualizada);
}
