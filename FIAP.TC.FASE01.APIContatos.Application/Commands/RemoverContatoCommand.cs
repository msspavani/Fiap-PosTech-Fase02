using MediatR;

namespace FIAP.TC.FASE01.APIContatos.Application.Commands;

public class RemoverContatoCommand : IRequest
{
    public Guid ContatoId { get; }

    public RemoverContatoCommand(Guid contatoId)
    {
        ContatoId = contatoId;
    }
}