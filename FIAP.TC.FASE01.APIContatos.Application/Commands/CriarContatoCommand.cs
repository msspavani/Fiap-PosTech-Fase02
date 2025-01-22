using MediatR;

namespace FIAP.TC.FASE01.APIContatos.Application.Commands;

public class CriarContatoCommand : IRequest<Guid>
{
    public string Nome { get; }
    public string Telefone { get; }
    public string Email { get; }
    public string Ddd { get; }

    public CriarContatoCommand(string nome, string telefone, string email, string ddd)
    {
        Nome = nome;
        Telefone = telefone;
        Email = email;
        Ddd = ddd;
    }
}