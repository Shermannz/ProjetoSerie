using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("/episodio")]
    public class EpisodioController : ControllerBase
    {
        private EpisodioService _service;

        public EpisodioController(EpisodioService service)
        {
            _service = service;
        }

        #region Pegar Episodios
        [HttpGet]
        public IActionResult PegarEpisodios()
        {
            return Ok(_service.PegarEpisodios());
        }
        #endregion

        #region Pegar Episodios Por Id
        [HttpGet("{id:int}")]
        public IActionResult PegarEpisodiosPorId(int id)
        {
            try
            {
                return Ok(_service.PegarEpisodiosPorId(id));
            }
            catch (Exception e) { 
                return BadRequest(e.Message);
            }
        }
        #endregion

        #region Inserir Episodio

        [HttpPost]
        public IActionResult InserirEpisodio(Episodio episodio)
        {
            try
            {
                _service.InserirEpisodio(episodio);
            } catch (Exception e) { 
                return BadRequest($"{e.Message}");
            }
            return Created("","Episodio criado com sucesso");
        }
        #endregion

        #region Atualizar Episodio
        [HttpPut("{idEpisodio:int}")]
        public IActionResult AtualizarEpisodio(int idEpisodio, [FromBody] Episodio episodio)
        {
            try
            {
                _service.AtualizarEpisodio(idEpisodio, episodio);
            } catch (Exception ex) { 
                return BadRequest(ex.Message);
            }
                return Ok();

        }
        #endregion

        #region Deletar Episodio
        [HttpDelete("{idEpisodio:int}")]
        public IActionResult DeletarEpisodio(int idEpisodio)
        {
            _service.ExcluirEpisodio(idEpisodio);

            return NoContent();
        }
        #endregion
    }
}
