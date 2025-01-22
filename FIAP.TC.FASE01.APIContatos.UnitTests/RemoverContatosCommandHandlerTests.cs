using FIAP.TC.FASE01.APIContatos.Application.CommandHandlers;
using FIAP.TC.FASE01.APIContatos.Application.Commands;
using FIAP.TC.FASE01.APIContatos.Domain.Entities;
using FIAP.TC.FASE01.APIContatos.Domain.Events;
using FIAP.TC.FASE01.APIContatos.Domain.Interfaces.Repositories;
using MediatR;
using Moq;

namespace FIAP.TC.FASE01.APIContatos.UnitTests;

public class RemoverContatosCommandHandlerTests
{
    
    private readonly Mock<IContatoRepository> _contatoRepositoryMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly RemoverContatoCommandHandler _handler;

    public RemoverContatosCommandHandlerTests()
    {
        _contatoRepositoryMock = new Mock<IContatoRepository>();
        _mediatorMock = new Mock<IMediator>();
        _handler = new RemoverContatoCommandHandler(_contatoRepositoryMock.Object, _mediatorMock.Object);
    }

    [Fact]
    public async Task Handle_DeveRemoverContatoEPublicarEvento()
    {
        // Arrange
        var contatoId = Guid.NewGuid();
        var command = new RemoverContatoCommand(contatoId);

        var contatoExistente = new Contato("João", "123456789", "joao@email.com", "11");
        _contatoRepositoryMock.Setup(r => r.ObterPorIdAsync(contatoId)).ReturnsAsync(contatoExistente);
        _contatoRepositoryMock.Setup(r => r.RemoverAsync(contatoId)).Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _contatoRepositoryMock.Verify(r => r.RemoverAsync(contatoId), Times.Once);
        _mediatorMock.Verify(m => m.Publish(It.IsAny<ContatoRemovidoEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        Assert.Equal(Unit.Value, result);
    }

    [Fact]
    public async Task Handle_DeveLancarExcecao_SeContatoNaoExistir()
    {
        // Arrange
        var contatoId = Guid.NewGuid();
        var command = new RemoverContatoCommand(contatoId);

        _contatoRepositoryMock.Setup(r => r.ObterPorIdAsync(contatoId)).ReturnsAsync((Contato)null);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
    }
}
