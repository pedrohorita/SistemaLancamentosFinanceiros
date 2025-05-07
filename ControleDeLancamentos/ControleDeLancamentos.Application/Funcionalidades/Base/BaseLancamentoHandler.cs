using ControleDeLancamentos.Application.Funcionalidades.Lancamentos.Inserir;
using ControleDeLancamentos.Domain.Entidades;
using ControleDeLancamentos.Domain.Notificador;

namespace ControleDeLancamentos.Application.Funcionalidades.Base;

public class BaseLancamentoHandler(INotificador notificador) 
    : BaseHandler(notificador)
{
    protected static Lancamento ObterLancamentoEntidade(BaseLancamentoCommand command) =>
        new(
            command.Descricao,
            command.Valor,
            command.CategoriaId,
            command.TipoId,
            command.Usuario);
}
