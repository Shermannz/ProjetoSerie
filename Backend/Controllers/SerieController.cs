using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("/serie")]
    public class SerieController : ControllerBase
    {
        private SerieService _service;

        public SerieController(SerieService service) { 
        _service = service;
        }

        #region Pegar Series
        [HttpGet]
        public IActionResult PegarSeries(string? nome, string? genero, int pagina = 1, int tamanhoPagina = 10)
        {
            return Ok(_service.PegarSeries(nome, genero, pagina, tamanhoPagina));
        }
        #endregion

        #region Pegar Serie Por Id
        [HttpGet("{id:int}")]
        public IActionResult PegarSeriePorId(int id)
        {
            try
            {
                return Ok(_service.PegarSeriePorId(id));
            } catch (Exception ex) { 
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Inserir Serie
        [HttpPost]
        public async Task<IActionResult> InserirSerie([FromForm] Serie serie,[FromForm] IFormFile? file, [FromForm] List<int> categorias)
        {
            if (file == null || file?.Length == 0)
                return BadRequest("Invalido");
                
            try
                {
                await _service.InserirSerie(serie, file, categorias);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
                return Created("", "Inserido com Sucesso!");
        }

        #endregion

        #region Atualizar Serie
        [HttpPut("{id:int}")]
        public async Task<IActionResult> AtualizarSerie(int id, [FromForm] Serie serie, [FromForm] IFormFile? file, [FromForm] List<int> categorias)
        {
            await _service.AtualizarSerie(id, serie, file, categorias);

            return Ok("Atualizado com Sucesso!");
        }
        #endregion

        #region Excluir Serie
        [HttpDelete("{id:int}")]
        public IActionResult ExcluirSerie(int id)
        {
            try
            {
                _service.ExcluirSerie(id);
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }
        #endregion
        
        #region Atualizar Imagem

        [HttpPut("atualizar-imagem/{id:int}")]
        public async Task<IActionResult> AtualizarImagem(int id, [FromForm] IFormFile? file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Foto obrigatoria");
            try
            {
                await _service.AtualizarImagem(id, file);
                return Ok("Imagem Atualizada com Sucesso!");

            } catch (Exception ex) {
                return BadRequest(ex.Message);   
            }
        }
        #endregion
    }
}
