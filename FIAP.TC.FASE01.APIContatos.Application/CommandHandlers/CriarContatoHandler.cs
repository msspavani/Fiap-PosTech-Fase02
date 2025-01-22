using FIAP.TC.FASE01.APIContatos.Application.Commands;
using FIAP.TC.FASE01.APIContatos.Domain.Entities;
using FIAP.TC.FASE01.APIContatos.Domain.Events;
using FIAP.TC.FASE01.APIContatos.Domain.Interfaces.Repositories;
using MediatR;

namespace FIAP.TC.FASE01.APIContatos.Application.CommandHandlers;

public class CriarContatoCommandHandler : IRequestHandler<CriarContatoCommand, Guid>
{
    private readonly IContatoRepository _contatoRepository;
    private readonly IMediator _mediator;

    public CriarContatoCommandHandler(IContatoRepository contatoRepository, IMediator mediator)
    {
        _contatoRepository = contatoRepository;
        _mediator = mediator;
    }

    public async Task<Guid> Handle(CriarContatoCommand request, CancellationToken cancellationToken)
    {
        // Cria a entidade Contato
        var contato = new Contato(request.Nome, request.Telefone, request.Email, request.Ddd);

        // Salva o contato no repositório
        await _contatoRepository.AdicionarAsync(contato);

        // Dispara o evento ContatoCriadoEvent
        var evento = new ContatoCriadoEvent(contato.Id, contato.Nome, contato.Email);
        await _mediator.Publish(evento, cancellationToken);

        return contato.Id;
    }
}