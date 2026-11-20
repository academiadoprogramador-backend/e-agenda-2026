using eAgenda.Dominio.Modulos.Contatos;

namespace eAgenda.Testes.Unidade.Contatos;

[TestClass]
public sealed class ContatoTests
{
    [TestMethod]
    public void GetHashCode_ContatosIguaisComIdsDiferentes_DeveRetornarMesmoHash()
    {
        Contato contato = new("Maria", "maria@example.com", "(49) 99999-9999", "Analista", "Empresa");
        Contato outroContato = new("Maria", "maria@example.com", "(49) 99999-9999", "Analista", "Empresa");
        contato.Id = Guid.NewGuid();
        outroContato.Id = Guid.NewGuid();
        contato.UsuarioId = Guid.NewGuid();
        outroContato.UsuarioId = Guid.NewGuid();

        Assert.IsTrue(contato.Equals(outroContato));
        Assert.AreEqual(contato.GetHashCode(), outroContato.GetHashCode());
        Assert.HasCount(1, new HashSet<Contato> { contato, outroContato });
    }

    [TestMethod]
    public void GetHashCode_ContatosIguaisComCamposOpcionaisNulos_DeveRetornarMesmoHash()
    {
        Contato contato = new("Maria", "maria@example.com", "(49) 99999-9999", null, null);
        Contato outroContato = new("Maria", "maria@example.com", "(49) 99999-9999", null, null);

        Assert.IsTrue(contato.Equals(outroContato));
        Assert.AreEqual(contato.GetHashCode(), outroContato.GetHashCode());
        Assert.HasCount(1, new HashSet<Contato> { contato, outroContato });
    }
}
