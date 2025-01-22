using FIAP.TC.FASE01.APIContatos.Domain.Entities;

namespace FIAP.TC.FASE01.APIContatos.Domain.Interfaces.Repositories;

public interface IContatoRepository
{
    Task AdicionarAsync(Contato contato);                 
    Task AtualizarAsync(Contato contato);                 
    Task RemoverAsync(Guid contatoId);                    
    Task<Contato?> ObterPorIdAsync(Guid contatoId);       
    Task<IEnumerable<Contato>> ObterPorDddAsync(string ddd);
}