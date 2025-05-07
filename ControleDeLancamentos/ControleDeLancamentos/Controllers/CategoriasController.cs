using ControleDeLancamentos.Domain.Entidades;
using ControleDeLancamentos.Domain.Interfaces.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeLancamentos.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController(ICategoriaRepositorio categoriaRepositorio) : ControllerBase
    {
        private readonly ICategoriaRepositorio _categoriaRepositorio = categoriaRepositorio;

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Categoria>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> ObterCategoriasAsync(CancellationToken token = default)
        {
            var categorias = await _categoriaRepositorio.ObterTodosAsync(token);
            if (!categorias.Any())
                return NoContent();
            return Ok(categorias);
        }
    }
}
