using FIAP.TC.FASE01.APIContatos.Application.Commands;
using FIAP.TC.FASE01.APIContatos.Domain.Entities;
using FIAP.TC.FASE01.APIContatos.Domain.Interfaces.Repositories;
using FIAP.TC.FASE01.APIContatos.WebAPI.Controllers.V1;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FIAP.TC.FASE01.APIContatos.UnitTests;

public class ContatosControllerTests
{
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IContatoRepository> _contatoRepositoryMock;
        private readonly ContatosController _controller;

        public ContatosControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _contatoRepositoryMock = new Mock<IContatoRepository>();
            _controller = new ContatosController(_mediatorMock.Object, _contatoRepositoryMock.Object);
        }

        [Fact]
        public async Task CriarContato_DeveRetornarCreatedAtAction()
        {
            // Arrange
            var command = new CriarContatoCommand("João", "123456789", "joao@email.com", "11");
            _mediatorMock.Setup(m => m.Send(command, default)).ReturnsAsync(Guid.NewGuid());

            // Act
            var result = await _controller.CriarContato(command);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(201, createdResult.StatusCode);
        }

        [Fact]
        public async Task ObterContatoPorId_DeveRetornarOkComContato()
        {
            // Arrange
            var contatoId = Guid.NewGuid();
            var contato = new Contato("João", "123456789", "joao@email.com", "11");
            _contatoRepositoryMock.Setup(r => r.ObterPorIdAsync(contatoId)).ReturnsAsync(contato);

            // Act
            var result = await _controller.ObterContatoPorId(contatoId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(contato, okResult.Value);
        }

        [Fact]
        public async Task AtualizarContato_DeveRetornarNoContent()
        {
            // Arrange
            var command = new AtualizarContatoCommand(Guid.NewGuid(), "Maria", "987654321", "maria@email.com", "21");
            _mediatorMock.Setup(m => m.Send(command, default)).ReturnsAsync(Unit.Value);

            // Act
            var result = await _controller.AtualizarContato(command.ContatoId, command);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task RemoverContato_DeveRetornarNoContent()
        {
            // Arrange
            var contatoId = Guid.NewGuid();
            _mediatorMock.Setup(m => m.Send(It.IsAny<RemoverContatoCommand>(), default)).ReturnsAsync(Unit.Value);

            // Act
            var result = await _controller.RemoverContato(contatoId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
}