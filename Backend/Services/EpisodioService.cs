using Backend.Models;
using Backend.Repositories;
using CloudinaryDotNet;

namespace Backend.Services
{
    public class EpisodioService
    {

        private EpisodioRepository _repository;

        public EpisodioService(EpisodioRepository repository)
        {
            _repository = repository;
        }

        #region Pegar Episodios
        public List<Episodio> PegarEpisodios()
        {
            return _repository.PegarEpisodios();
        }
        #endregion

        #region Inserir Episodio
        internal void InserirEpisodio(Episodio episodio)
        {
            _repository.InserirEpisodio(episodio);
        }
        #endregion

        #region Atualizar Episodio
        internal void AtualizarEpisodio(int idEpisodio, Episodio episodio)
        {
            _repository.AtualizarEpisodio(idEpisodio, episodio);
        }
        #endregion

        #region Excluir Episodio
        internal void ExcluirEpisodio(int id)
        {
            _repository.ExcluirEpisodio(id);
        }

        internal Episodio PegarEpisodiosPorId(int id)
        {
            return _repository.PegarEpisodioPorId(id);
        }
        #endregion
    }
    
}