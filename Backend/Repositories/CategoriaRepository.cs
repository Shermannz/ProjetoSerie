using Backend.Models;
using Dapper;
using Microsoft.Data.Sqlite;

namespace Backend.Repositories
{
    public class CategoriaRepository
    {
        private FactoryRepository _factory;

        public CategoriaRepository(FactoryRepository factory)
        {
            _factory = factory;
        }

        #region Pegar Categorias
        public IEnumerable<Categoria> PegarCategorias(string? nome)
        {
            using(var conn = _factory.GetConnection())
            {
                var sql = "SELECT * FROM Categoria WHERE 1=1";
                if (!string.IsNullOrEmpty(nome))
                    sql += " AND nome like @nome";
                return conn.Query<Categoria>(sql, new {nome = $"%{nome}%"});
            }
        }
        #endregion

        #region Inserir Categoria
        public void InserirCategoria(Categoria categoria)
        {
            using (var conn = _factory.GetConnection())
            {
                var sql = @"INSERT INTO Categoria (
                                nome
                            )
                            VALUES (
                                @Nome
                            );  ";
                conn.Execute(sql, categoria);
            }
        }
        #endregion

        #region Atualizar Categoria
        public void AtualizarCategoria(int id, Categoria categoria)
        {
            using (var conn = _factory.GetConnection())
            {
                var sql = $@"
                        UPDATE Categoria
                        SET
                            nome = @Nome
                        WHERE id_categoria = @id";
               var rows = conn.Execute(sql, new {id, categoria.Nome});
                if (rows == 0)
                    throw new Exception("Categoria não encontrada");
            }
        }
        #endregion

        #region Excluir Categoria
        internal void ExcluirCategoria(int id)
        {
            using (var conn = _factory.GetConnection())
            {
                var sql = "delete from Categoria where id_categoria = @id";
                var rows = conn.Execute(sql, new { id });
                if (rows == 0)
                    throw new Exception("Categoria não encontrada");
            }
        }
        #endregion
    }
}
