using Backend.Models;
using Backend.Models.dto;
using CloudinaryDotNet;
using Dapper;
using Microsoft.Data.Sqlite;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Backend.Repositories
{
    public class SerieRepository
    {
        private FactoryRepository _factory;
        public SerieRepository(FactoryRepository factory)
        {
            _factory = factory;
        }

        #region Pegar Series
        internal ListaSerieDTO PegarSeries(string? nome, string? genero, int pagina, int tamanhoPagina)
        {
            using(var conn = _factory.GetConnection())
            {
                var registrosTotais = conn.ExecuteScalar<int>("select COUNT(*) from Serie");
                var paginaTotais = (int)Math.Ceiling((double)registrosTotais / tamanhoPagina);

                if (pagina > paginaTotais || pagina < 1)
                    throw new Exception("A pagina deve ser inferior ao total");

                var series = conn.Query<Serie>("select * from Serie limit @tamanho offset @offset", new {tamanho = tamanhoPagina, offset = (pagina - 1) * tamanhoPagina});
               
                return new ListaSerieDTO() { Series = series, PaginaTotais = paginaTotais, PaginaAtual = pagina};
            }
        }
        #endregion

        public IEnumerable<Serie> Teste(string? nome, string? genero)
        {
            using (var conn = _factory.GetConnection())
            {
                // Terminar a pesquisa por Categoria
                var sql = @"select distinct a.* from Serie a
                            left join SerieCategoria ac
                            on ac.id_serie = a.id_serie                
                            left join Categoria c on ac.id_categoria = c.id_categoria
                            where 1=1 ";
                if (!string.IsNullOrEmpty(nome))
                    sql += " and a.nome like @nome";

                if (!string.IsNullOrEmpty(genero))
                    sql += " and c.nome like @genero";

                var series = conn.Query<Serie>(sql, new { nome = $"%{nome}%", genero = $"%{genero}%" }).ToList();

                foreach (Serie serie in series)
                {
                    var sqlcategorias = @"
                        select c.nome from Categoria c
                            inner join SerieCategoria ac
                            on ac.id_categoria = c.id_categoria
                            inner join Serie a on ac.id_serie = a.id_serie
                            where a.id_serie = @idSerie";

                    var categorias = conn.Query<string>(sqlcategorias,
                            new
                            {
                                idSerie = serie.IdSerie
                            });
                    serie.Categorias = categorias.ToList();
                }

                return series;
            }
        }

        #region Inserir Serie
        internal void InserirSerie(Serie serie, List<int> categorias)
        {
            using (var conn = _factory.GetConnection())
            {
                conn.Open();
                using var transaction = conn.BeginTransaction();
                var sql = @"INSERT INTO Serie (
                                nome,
                                sinopse,
                                estudio,
                                ano,
                                lancamento,
                                foto
                            )
                            VALUES (
                                @Nome,
                                @Sinopse,
                                @Estudio,
                                @Ano,
                                @Lancamento,
                                @Foto
                            );         
                            SELECT last_insert_rowid();
                            ";

                var id = conn.ExecuteScalar<int>(sql, serie, transaction);

                var sqlCategoria = @"
                INSERT INTO SerieCategoria 
                    (id_serie, id_categoria) 
                    VALUES (@id, @categoria);
                ";

                foreach(var categoria in categorias)
                {
                    conn.Execute(sqlCategoria, new {id, categoria}, transaction);
                }
                transaction.Commit();
                conn.Close();
            }
        }
        #endregion
        
        #region Atualizar Serie
        internal void AtualizarSerie(int id, Serie serie, List<int> categorias)
        {
            using (var conn = _factory.GetConnection())
            {
                conn.Open();
                using var transaction = conn.BeginTransaction();
                var sql = $@"
                        UPDATE Serie
                        SET
                            nome = @Nome,
                            sinopse = @Sinopse,
                            {(!string.IsNullOrEmpty(serie.Foto) ? "foto = @Foto," : "")}
                            estudio = @Estudio,
                            ano = @Ano,
                            lancamento = @Lancamento
                        WHERE id_serie = @id";

                conn.Execute(sql, new
                {
                    serie.Nome,
                    serie.Sinopse,
                    serie.Foto,
                    serie.Estudio,
                    serie.Ano,
                    serie.Lancamento,
                    id
                }, transaction);
                
                var sqlDeletarCategoria = @"
                DELETE FROM SerieCategoria WHERE id_serie = @id;
                ";
                
                conn.Execute(sqlDeletarCategoria, new {id}, transaction);

                var sqlCategoria = @"
                INSERT INTO SerieCategoria 
                    (id_serie, id_categoria) 
                    VALUES (@id, @categoria);
                ";

                foreach (var categoria in categorias)
                {
                    conn.Execute(sqlCategoria, new { id, categoria }, transaction);
                }
                transaction.Commit();
                conn.Close();
            }
        }
        #endregion
        
        #region Excluir Serie
        internal void ExcluirSerie(int id)
        {
            using(var conn = _factory.GetConnection())
            {
                var sql = "delete from Serie where id_serie = @id";
                var rows = conn.Execute(sql, new {id});
                if (rows == 0)
                    throw new Exception("Serie não encontrado");
            }
        }
        #endregion

        #region Atualizar Imagem
        internal void AtualizarImagem(int id, string foto)
        {
            using(var conn = _factory.GetConnection())
            {
                var sql = @" UPDATE Serie
                        SET
                            foto = @foto
                        WHERE id_serie = @id;";
                conn.Execute(sql, new {foto, id});
            }
        }

        internal Serie PegarSeriePorId(int id)
        {
            using (var conn = _factory.GetConnection())
            {
                var sql = "SELECT * FROM Serie WHERE id_serie = @id";
         
                var serie = conn.QueryFirstOrDefault<Serie>(sql, new {id});
                if (serie == null)
                    throw new Exception("Serie não encontrado");

                var sqlcategorias = @"
                        select c.nome from Categoria c
                            inner join SerieCategoria ac
                            on ac.id_categoria = c.id_categoria
                            inner join Serie a on ac.id_serie = a.id_serie
                            where a.id_serie = @idSerie";

                var categorias = conn.Query<string>(sqlcategorias,
                        new
                        {
                            idSerie = serie.IdSerie
                        });
                serie.Categorias = categorias.ToList();

                var sqlepisodios = @"select e.nome from Episodio e
                            inner join Serie a on e.id_serie = a.id_serie
                            where a.id_serie = @idSerie";
                    var episodios = conn.Query<string>(sqlepisodios, new { idSerie = serie.IdSerie });
                    serie.Episodios = episodios.ToList();
                return serie;
            }
        }
        #endregion
    }
}
