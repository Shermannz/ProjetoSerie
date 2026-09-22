using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("/categoria")]
    public class CategoriaController : ControllerBase
    {
        private CategoriaService _service;

        public CategoriaController(CategoriaService service)
        {
            _service = service;
        }

        #region Pegar Categorias
        [HttpGet]
        public IActionResult PegarCategorias(string? nome)
        {
            return Ok(_service.PegarCategorias(nome));
        }
        #endregion

        #region Inserir Categoria
        [HttpPost]
        public IActionResult InserirCategoria([FromBody]Categoria categoria)
        {
            _service.InserirCategoria(categoria);
            return Created("", "Usuario inserido com Sucesso");
        }
        #endregion

        #region Atualizar Categoria
        [HttpPut("{id:int}")]
        public IActionResult AtualizarCategoria(int id,[FromBody] Categoria categoria)
        {
            try
            {
                _service.AtualizarCategoria(id, categoria);
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
            return Ok();
        }
        #endregion

        #region Excluir Categoria
        [HttpDelete("{id:int}")]
        public IActionResult ExcluirCategoria(int id)
        {
            try
            {
                _service.ExcluirCategoria(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }
        #endregion
    }
}
