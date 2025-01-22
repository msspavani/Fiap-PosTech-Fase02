using FIAP.TC.FASE01.APIContatos.Application.CommandHandlers;
using FIAP.TC.FASE01.APIContatos.Application.Commands;
using FIAP.TC.FASE01.APIContatos.Domain.Entities;
using FIAP.TC.FASE01.APIContatos.Domain.Events;
using FIAP.TC.FASE01.APIContatos.Domain.Interfaces.Repositories;
using MediatR;
using Moq;

namespace FIAP.TC.FASE01.APIContatos.UnitTests;

public class AtualizarContatosCommandHandlerTests
{
     private readonly Mock<IContatoRepository> _contatoRepositoryMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly AtualizarContatoCommandHandler _handler;

        public AtualizarContatosCommandHandlerTests()
        {
            _contatoRepositoryMock = new Mock<IContatoRepository>();
            _mediatorMock = new Mock<IMediator>();
            _handler = new AtualizarContatoCommandHandler(_contatoRepositoryMock.Object, _mediatorMock.Object);
        }

        [Fact]
        public async Task Handle_DeveAtualizarContatoEPublicarEvento()
        {
            // Arrange
            var contatoId = Guid.NewGuid();
            var command = new AtualizarContatoCommand(contatoId, "Maria", "987654321", "maria@email.com", "21");

            var contatoExistente = new Contato("João", "123456789", "joao@email.com", "11");
            _contatoRepositoryMock.Setup(r => r.ObterPorIdAsync(contatoId)).ReturnsAsync(contatoExistente);
            _contatoRepositoryMock.Setup(r => r.AtualizarAsync(It.IsAny<Contato>())).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _contatoRepositoryMock.Verify(r => r.AtualizarAsync(It.IsAny<Contato>()), Times.Once);
            _mediatorMock.Verify(m => m.Publish(It.IsAny<ContatoAtualizadoEvent>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.Equal(Unit.Value, result);
        }

        [Fact]
        public async Task Handle_DeveLancarExcecao_SeContatoNaoExistir()
        {
            // Arrange
            var contatoId = Guid.NewGuid();
            var command = new AtualizarContatoCommand(contatoId, "Maria", "987654321", "maria@email.com", "21");

            _contatoRepositoryMock.Setup(r => r.ObterPorIdAsync(contatoId)).ReturnsAsync((Contato)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
        }
    
}