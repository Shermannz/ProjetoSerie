using Backend.Models;
using Backend.Repositories;

namespace Backend.Services
{
    public class CategoriaService
    {
        private CategoriaRepository _repository;

        public CategoriaService(CategoriaRepository repository)
        {
            _repository = repository;
        }

        #region Pegar Categorias
        public IEnumerable<Categoria> PegarCategorias(string? nome)
        {
            return _repository.PegarCategorias(nome);
        }
        #endregion

        #region Inserir Categoria
        public void InserirCategoria(Categoria categoria)
        {
            _repository.InserirCategoria(categoria);
        }
        #endregion

        #region Atualizar Categoria
        public void AtualizarCategoria(int id, Categoria categoria)
        {
            _repository.AtualizarCategoria(id, categoria);
        }
        #endregion

        #region Excluir Categoria
        internal void ExcluirCategoria(int id)
        {
            _repository.ExcluirCategoria(id);
        }
        #endregion
    }
}
