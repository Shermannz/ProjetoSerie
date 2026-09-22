using Backend.Models;
using Dapper;

namespace Backend.Repositories
{
    public class EpisodioRepository
    {
        private FactoryRepository _factory;
        public EpisodioRepository(FactoryRepository factory)
        {
            _factory = factory;
        }

        #region Pegar Episodios
        internal List<Episodio> PegarEpisodios()
        {
            using (var conn = _factory.GetConnection())
            {
                return conn.Query<Episodio>(@"
                    Select e.*, a.nome as Serie from Episodio e 
                    left join Serie a on a.id_serie = e.id_serie").ToList();
            }
        }
        #endregion

        #region Inserir Episodio
        internal void InserirEpisodio(Episodio episodio)
        {
            using (var conn = _factory.GetConnection())
            {
                conn.Open();
                using var transaction = conn.BeginTransaction();
                var sql = @"INSERT INTO Episodio (
                                nome,
                                video,
                                id_serie
                            )
                            VALUES (
                                @Nome,
                                @Video,
                                @IdSerie
                            );         
                            ";

                    conn.Execute(sql, episodio, transaction);
                transaction.Commit();
                conn.Close();
                }
            }
        #endregion

        #region Atualizar Episodio
        internal void AtualizarEpisodio(int idEpisodio, Episodio episodio)
        {
            using (var conn = _factory.GetConnection())
            {
                conn.Open();
                using var transaction = conn.BeginTransaction();

                var sqlSelect = @"SELECT * FROM Episodio WHERE id_episodio = @idEpisodio";

                var ep = conn.QueryFirstOrDefault<Episodio>(sqlSelect, new {idEpisodio});

                if (ep == null)
                    throw new Exception("Episodio não encontrado");

                var sql = @"
                        UPDATE Episodio
                        SET
                            nome = @Nome,
                            video = @Video,
                            id_serie = @IdSerie
                        WHERE id_episodio = @idEpisodio";

                conn.Execute(sql, new
                {
                    episodio.Nome,
                    episodio.Video,
                    episodio.IdSerie,
                    idEpisodio
                }, transaction);

                transaction.Commit();
                conn.Close();
            }
        }
        #endregion

        #region Excluir Episodio
        internal void ExcluirEpisodio(int idEpisodio)
        {
            using (var conn = _factory.GetConnection())
            {
                var sql = "delete from Episodio where id_episodio = @idEpisodio";
                var rows = conn.Execute(sql, new { idEpisodio });
                if (rows == 0)
                    throw new Exception("Episodio não encontrado");
            }
        }
        #endregion

        #region PegarEpisodioPorId
        internal Episodio PegarEpisodioPorId(int id)
        {
            using (var conn = _factory.GetConnection())
            {
                var episodio = conn.QueryFirstOrDefault<Episodio>(@"
                        Select e.*, a.nome as Serie from Episodio e
                        left join Serie a on a.id_serie = e.id_serie 
                        where id_episodio = @id", new { id });
                if (episodio == null)
                    throw new Exception("Episodio não encontrado");
                return episodio;
            }
        }
        #endregion
    }
}
