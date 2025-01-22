using FIAP.TC.FASE01.APIContatos.Domain.Interfaces;
using MediatR;

namespace FIAP.TC.FASE01.APIContatos.Domain.Events;

public class ContatoAtualizadoEvent : IDomainEvent, INotification
{
    public Guid ContatoId { get; }
    public string Nome { get; }
    public string Email { get; }
    public string Telefone { get; }
    public DateTime DataOcorrencia { get; }

    public ContatoAtualizadoEvent(Guid contatoId, string nome, string email, string telefone)
    {
        ContatoId = contatoId;
        Nome = nome;
        Email = email;
        Telefone = telefone;
        DataOcorrencia = DateTime.UtcNow;
    }
}