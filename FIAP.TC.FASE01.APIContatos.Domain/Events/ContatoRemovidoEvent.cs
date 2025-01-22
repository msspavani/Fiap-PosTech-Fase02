using FIAP.TC.FASE01.APIContatos.Domain.Interfaces;
using MediatR;

namespace FIAP.TC.FASE01.APIContatos.Domain.Events;

public class ContatoRemovidoEvent : IDomainEvent, INotification
{
    public Guid ContatoId { get; }
    public DateTime DataOcorrencia { get; }

    public ContatoRemovidoEvent(Guid contatoId)
    {
        ContatoId = contatoId;
        DataOcorrencia = DateTime.UtcNow;
    }
}