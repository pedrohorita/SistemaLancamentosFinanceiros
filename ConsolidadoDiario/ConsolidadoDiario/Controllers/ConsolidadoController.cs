using ConsolidadoDiario.Domain.Entidades;
using ConsolidadoDiario.Domain.Interfaces.Respositorio;
using Microsoft.AspNetCore.Mvc;

namespace ConsolidadoDiario.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ConsolidadoController(IConsolidadoDiarioRepositorio consolidadoDiarioRepositorio,
    IConsolidadoDiarioCategoriaRepositorio consolidadoDiarioCategoriaRepositorio) : ControllerBase
{
    private readonly IConsolidadoDiarioRepositorio _consolidadoDiarioRepositorio = consolidadoDiarioRepositorio;
    private readonly IConsolidadoDiarioCategoriaRepositorio _consolidadoDiarioCategoriaRepositorio = consolidadoDiarioCategoriaRepositorio;

    [HttpGet]
    [ProducesResponseType(typeof(Domain.Entidades.ConsolidadoDiario), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ObterConsolidadoDiarioPorDataAsync([FromQuery] DateOnly data, CancellationToken token)
    {
        var consolidado = await _consolidadoDiarioRepositorio.ObterConsolidadoDiarioPorDataAsync(data, token);
        if (consolidado == null)
        {
            return NoContent();
        }

        return Ok(consolidado);
    }

    [HttpGet("periodo")]
    [ProducesResponseType(typeof(IEnumerable<Domain.Entidades.ConsolidadoDiario>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ObterConsolidadosDiariosAsync([FromQuery] DateOnly dataInicio, [FromQuery] DateOnly dataFim, CancellationToken token)
    {
        var consolidado = await _consolidadoDiarioRepositorio.ObterConsolidadosDiariosAsync(dataInicio, dataFim, token);
        if (!consolidado.Any())
        {
            return NoContent();
        }
        return Ok(consolidado);
    }

    [HttpGet("categoria")]
    [ProducesResponseType(typeof(IEnumerable<ConsolidadoDiarioCategoria>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ObterConsolidadoDiarioCategoria([FromQuery] DateOnly data, CancellationToken token)
    {
        var consolidado = await _consolidadoDiarioCategoriaRepositorio.ObterConsolidadoDiarioCategoriaAsync(data, token);
        if (consolidado == null || !consolidado.Any())
        {
            return NoContent();
        }
        return Ok(consolidado);
    }

    [HttpGet("categoria/{categoriaId}")]
    [ProducesResponseType(typeof(ConsolidadoDiarioCategoria), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ObterConsolidadoPorCategoria([FromRoute] int categoriaId, [FromQuery] DateOnly data, CancellationToken token)
    {
        var consolidado = await _consolidadoDiarioCategoriaRepositorio.ObterConsolidadoPorCategoria(categoriaId, data, token);
        if (consolidado == null)
        {
            return NoContent();
        }
        return Ok(consolidado);
    }

    [HttpGet("categoria/{categoriaId}/periodo")]
    [ProducesResponseType(typeof(IEnumerable<ConsolidadoDiarioCategoria>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ObterConsolidadosDiariosPorCategoria([FromRoute] int categoriaId, [FromQuery] DateOnly dataInicio, [FromQuery] DateOnly dataFim, CancellationToken token)
    {
        var consolidado = await _consolidadoDiarioCategoriaRepositorio.ObterConsolidadosDiariosPorCategoriaAsync(categoriaId, dataInicio, dataFim, token);
        if (consolidado == null || !consolidado.Any())
        {
            return NoContent();
        }
        return Ok(consolidado);

    }

}