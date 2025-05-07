using ControleDeLancamentos.Domain.Entidades;
using FluentValidation;

namespace ControleDeLancamentos.Domain.Validadores;

public class LancamentoValidador : AbstractValidator<Lancamento>
{
    public LancamentoValidador(bool ehCriacao)
    {
        if (ehCriacao)
        {
            RuleFor(l => l.Id)
                .NotEmpty().WithMessage("O ID é obrigatório.");
        }

        RuleFor(l => l.Descricao)
            .NotEmpty().WithMessage("A descrição é obrigatória.")
            .MaximumLength(255).WithMessage("A descrição deve ter no máximo 255 caracteres.");
        RuleFor(l => l.Valor)
            .NotEmpty().WithMessage("O valor é obrigatório.")
            .GreaterThan(0).WithMessage("O valor deve ser maior que zero.");
        RuleFor(l => l.Data)
            .NotEmpty().WithMessage("A data é obrigatória.");
        RuleFor(l => l.CategoriaId)
            .NotEmpty().WithMessage("A categoria é obrigatória.");
        RuleFor(l => l.TipoId)
            .NotEmpty().WithMessage("O tipo é obrigatório.");
        RuleFor(l => l.Usuario)
            .NotEmpty().WithMessage("O usuário é obrigatório.")
            .MaximumLength(100).WithMessage("O usuário deve ter no máximo 100 caracteres.");
    }
}
