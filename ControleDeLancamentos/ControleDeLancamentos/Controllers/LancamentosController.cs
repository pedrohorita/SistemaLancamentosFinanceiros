using ControleDeLancamentos.Application.Funcionalidades.Lancamentos.Atualizar;
using ControleDeLancamentos.Application.Funcionalidades.Lancamentos.Excluir;
using ControleDeLancamentos.Application.Funcionalidades.Lancamentos.Inserir;
using ControleDeLancamentos.Application.Funcionalidades.Lancamentos.ObterPaginado;
using ControleDeLancamentos.Domain.Entidades;
using ControleDeLancamentos.Domain.Interfaces.Repositorios;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeLancamentos.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LancamentosController(IMediator mediator, ILancamentoRepositorio lancamentoRepositorio) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILancamentoRepositorio _lancamentoRepositorio = lancamentoRepositorio;


        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Lancamento>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> ObterLancamentosAsync(int pagina = 1, int quantidade = 100, CancellationToken token = default)
        {
            var resultado = await _mediator.Send(new ObterLancamentosPaginadosQuery(pagina, quantidade), token);
            if (!resultado.Sucesso)
            {
                Console.WriteLine(string.Join("; ", resultado.Erros));
                return NoContent();
            }

            return Ok(resultado.Dados);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Lancamento), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> ObterLancamentoPorIdAsync(Guid id, CancellationToken token)
        {
            var lancamento = await _lancamentoRepositorio.ObterPorIdAsync(id, token);
            if (lancamento == null)
                return NoContent();

            return Ok(lancamento);
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> InserirLancamentoAsync([FromBody] InserirLancamentoCommand comando, CancellationToken token)
        {
            var resultado = await _mediator.Send(comando, token);

            if(resultado.Sucesso)
                return Created(nameof(ObterLancamentoPorIdAsync), new { id = Guid.NewGuid() });

            return BadRequest(resultado.Erros);
        }


        [HttpPut("{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AtualizarLancamentoAsync(Guid id, [FromBody] AtualizarLancamentoCommand comando, CancellationToken token)
        {   
            var comandoAtualizacao = comando with { Id = id };
            var resultado = await _mediator.Send(comandoAtualizacao, token);

            if (resultado.Sucesso)
                return Ok();

            return BadRequest(resultado.Erros);
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Lancamento), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> ExcluirLancamentoAsync(Guid id, CancellationToken token)
        {
            var resultado = await _mediator.Send(new ExcluirLancamentoCommand(id), token);

            if (resultado.Sucesso)
                return Ok();

            return NoContent();
        }
    }
}
