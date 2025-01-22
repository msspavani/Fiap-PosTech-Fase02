using System.Data;
using Dapper;
using FIAP.TC.FASE01.APIContatos.Domain.Entities;
using FIAP.TC.FASE01.APIContatos.Domain.Interfaces.Repositories;

namespace FIAP.TC.FASE01.APIContatos.Infrastrucure.Repositories;


    public class ContatoRepository : IContatoRepository
    {
        private readonly IDbConnection _dbConnection;

        public ContatoRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task AdicionarAsync(Contato contato)
        {
            var sql = "INSERT INTO Contatos (Id, Nome, Telefone, Email, Ddd) VALUES (@Id, @Nome, @Telefone, @Email, @Ddd)";
            await _dbConnection.ExecuteAsync(sql, contato);
        }

        public async Task AtualizarAsync(Contato contato)
        {
            var sql = "UPDATE Contatos SET Nome = @Nome, Telefone = @Telefone, Email = @Email, Ddd = @Ddd WHERE Id = @Id";
            await _dbConnection.ExecuteAsync(sql, contato);
        }

        public async Task RemoverAsync(Guid contatoId)
        {
            var sql = "DELETE FROM Contatos WHERE Id = @Id";
            await _dbConnection.ExecuteAsync(sql, new { Id = contatoId });
        }

        public async Task<Contato?> ObterPorIdAsync(Guid contatoId)
        {
            var sql = "SELECT * FROM Contatos WHERE Id = @Id";
            return await _dbConnection.QuerySingleOrDefaultAsync<Contato>(sql, new { Id = contatoId });
        }

        public async Task<IEnumerable<Contato>> ObterPorDddAsync(string ddd)
        {
            var sql = "SELECT * FROM Contatos WHERE Ddd = @Ddd";
            return await _dbConnection.QueryAsync<Contato>(sql, new { Ddd = ddd });
        }
    }
