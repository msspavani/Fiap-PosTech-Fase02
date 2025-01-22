using FIAP.TC.FASE01.APIContatos.Domain.Entities;
using FIAP.TC.FASE01.APIContatos.Domain.Interfaces.Repositories;
using Moq;

namespace FIAP.TC.FASE01.APIContatos.UnitTests;

public class ContatoRepositoryTests
{
    private readonly Mock<IContatoRepository> _contatoRepositoryMock;

    public ContatoRepositoryTests()
    {
        _contatoRepositoryMock = new Mock<IContatoRepository>();
    }

    [Fact]
    public async Task AdicionarAsync_DeveSalvarContato()
    {
        // Arrange
        var contato = new Contato("Ana", "234567890", "ana@email.com", "12");

        // Act
        await _contatoRepositoryMock.Object.AdicionarAsync(contato);

        // Assert
        _contatoRepositoryMock.Verify(r => r.AdicionarAsync(contato), Times.Once);
    }

    [Fact]
    public async Task ObterPorIdAsync_DeveRetornarContato()
    {
        // Arrange
        var contatoId = Guid.NewGuid();
        var contato = new Contato("Carlos", "345678901", "carlos@email.com", "13");
        _contatoRepositoryMock.Setup(r => r.ObterPorIdAsync(contatoId)).ReturnsAsync(contato);

        // Act
        var result = await _contatoRepositoryMock.Object.ObterPorIdAsync(contatoId);

        // Assert
        Assert.Equal(contato, result);
    } 
}