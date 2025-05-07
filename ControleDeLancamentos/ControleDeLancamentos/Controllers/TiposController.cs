using ControleDeLancamentos.Domain.Entidades;
using ControleDeLancamentos.Domain.Interfaces.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeLancamentos.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TiposController(ITipoRepositorio tipoRepositorio) : ControllerBase
    {
        private readonly ITipoRepositorio _tipoRepositorio = tipoRepositorio;

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Tipo>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> ObterTiposAsync(CancellationToken token = default)
        {
            var tipos = await _tipoRepositorio.ObterTodosAsync(token);
            if (!tipos.Any())
                return NoContent();
            return Ok(tipos);
        }
    }
}
