using FIAP.TC.FASE01.APIContatos.Application.Commands;
using FIAP.TC.FASE01.APIContatos.Domain.Events;
using FIAP.TC.FASE01.APIContatos.Domain.Interfaces.Repositories;
using MediatR;

namespace FIAP.TC.FASE01.APIContatos.Application.CommandHandlers;


public class RemoverContatoCommandHandler : IRequestHandler<RemoverContatoCommand>
{
    private readonly IContatoRepository _contatoRepository;
    private readonly IMediator _mediator;

    public RemoverContatoCommandHandler(IContatoRepository contatoRepository, IMediator mediator)
    {
        _contatoRepository = contatoRepository;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(RemoverContatoCommand request, CancellationToken cancellationToken)
    {
        // Carrega o contato do repositório
        var contato = await _contatoRepository.ObterPorIdAsync(request.ContatoId);
        if (contato == null)
            throw new Exception("Contato não encontrado.");

        // Remove o contato
        await _contatoRepository.RemoverAsync(request.ContatoId);

        // Dispara o evento ContatoRemovidoEvent
        var evento = new ContatoRemovidoEvent(request.ContatoId);
        await _mediator.Publish(evento, cancellationToken);
        
        return Unit.Value;
        
    }
}