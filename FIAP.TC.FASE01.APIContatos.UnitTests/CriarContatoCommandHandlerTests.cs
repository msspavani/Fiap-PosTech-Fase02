using FIAP.TC.FASE01.APIContatos.Application.CommandHandlers;
using FIAP.TC.FASE01.APIContatos.Application.Commands;
using FIAP.TC.FASE01.APIContatos.Domain.Entities;
using FIAP.TC.FASE01.APIContatos.Domain.Events;
using FIAP.TC.FASE01.APIContatos.Domain.Interfaces.Repositories;
using MediatR;
using Moq;

namespace FIAP.TC.FASE01.APIContatos.UnitTests;

public class CriarContatoCommandHandlerTests
{
    private readonly Mock<IContatoRepository> _contatoRepositoryMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly CriarContatoCommandHandler _handler;

    public CriarContatoCommandHandlerTests()
    {
        _contatoRepositoryMock = new Mock<IContatoRepository>();
        _mediatorMock = new Mock<IMediator>();
        _handler = new CriarContatoCommandHandler(_contatoRepositoryMock.Object, _mediatorMock.Object);
    }

    [Fact]
    public async Task Handle_DeveSalvarContatoEPublicarEvento()
    {
        // Arrange
        var command = new CriarContatoCommand("João", "123456789", "joao@email.com", "11");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _contatoRepositoryMock.Verify(r => r.AdicionarAsync(It.IsAny<Contato>()), Times.Once);
        _mediatorMock.Verify(m => m.Publish(It.IsAny<ContatoCriadoEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        Assert.IsType<Guid>(result);
    }
}