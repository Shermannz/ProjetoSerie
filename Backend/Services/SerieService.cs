using Backend.Models;
using Backend.Models.dto;
using Backend.Repositories;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Services
{
    public class SerieService
    {

        private SerieRepository _repository;
        private Cloudinary _cloudinary;

        public SerieService(SerieRepository repository, Cloudinary cloudinary)
        {
            _repository = repository;
            _cloudinary = cloudinary;

        }

        #region Pegar Series
        public ListaSerieDTO PegarSeries(string? nome, string? genero, int pagina, int tamanhoPagina)
        {
            return _repository.PegarSeries(nome, genero, pagina, tamanhoPagina);
        }
        #endregion

        #region Pegar Serie Por Id
        internal Serie PegarSeriePorId(int id)
        {
            return _repository.PegarSeriePorId(id);
        }
        #endregion

        #region Inserir Serie
        internal async Task InserirSerie(Serie serie, IFormFile? file, List<int> categorias)
        {
            serie.Foto = await InserirImagem(file);
            _repository.InserirSerie(serie, categorias);
        }
        #endregion

        #region Atualizar Serie
        internal async Task AtualizarSerie(int id, Serie serie, IFormFile? file, List<int> categorias)
        {
             serie.Foto = await InserirImagem(file);

             _repository.AtualizarSerie(id, serie, categorias);
        }
        #endregion

        #region Excluir Serie
        internal void ExcluirSerie(int id)
        {
            _repository.ExcluirSerie(id);
        }
        #endregion
        
        #region Inserir Imagem / Metodo a parte
        private async Task<string> InserirImagem(IFormFile? imagem)
        {
            if (imagem == null || imagem?.Length == 0)
                return "";
            
            using (var stream = imagem?.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(imagem?.FileName, stream)
                };


                var uploadResult = await _cloudinary.UploadAsync(uploadParams);


                if (uploadResult == null)
                    throw new Exception("Erro ao passar URL");
                return uploadResult.SecureUrl.ToString();
            }
        }
        #endregion
        
        #region Atualizar Imagem
        internal async Task AtualizarImagem(int id, IFormFile? file)
        {
            string foto = await InserirImagem(file);
            _repository.AtualizarImagem(id, foto);
        }


        #endregion

    }
}
