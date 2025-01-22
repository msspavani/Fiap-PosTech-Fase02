using FIAP.TC.FASE01.APIContatos.Application.Commands;
using FIAP.TC.FASE01.APIContatos.Domain.Events;
using FIAP.TC.FASE01.APIContatos.Domain.Interfaces.Repositories;
using MediatR;

namespace FIAP.TC.FASE01.APIContatos.Application.CommandHandlers;

public class AtualizarContatoCommandHandler : IRequestHandler<AtualizarContatoCommand>
{
    private readonly IContatoRepository _contatoRepository;
    private readonly IMediator _mediator;

    public AtualizarContatoCommandHandler(IContatoRepository contatoRepository, IMediator mediator)
    {
        _contatoRepository = contatoRepository;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(AtualizarContatoCommand request, CancellationToken cancellationToken)
    {
        // Carrega o contato do repositório
        var contato = await _contatoRepository.ObterPorIdAsync(request.ContatoId);
        if (contato == null)
            throw new Exception("Contato não encontrado.");

        // Atualiza as informações do contato
        contato.Atualizar(request.Nome, request.Telefone, request.Email, request.Ddd);

        // Salva as mudanças
        await _contatoRepository.AtualizarAsync(contato);

        // Dispara o evento ContatoAtualizadoEvent
        var evento = new ContatoAtualizadoEvent(contato.Id, contato.Nome, contato.Email, contato.Telefone);
        await _mediator.Publish(evento, cancellationToken);

        return Unit.Value;
    }
}