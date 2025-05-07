
export interface LancamentoPaginado {
    lancamentos: Lancamento[];
    totalPaginas: number;
    paginaAtual: number;
    totalRegistros: number;
    tamanhoPagina: number;
}

export interface Lancamento {
    id: string;
    valor: number;
    data: string;
    descricao: string;
    usuario: string;
    categoria: CategoriaLancamento;
    tipo: TipoLancamento;
}

export interface LancamentoModel {
    valor: number;
    descricao: string;
    usuario: string;
    categoriaId: number;
    tipoId: number;
}

export interface TipoLancamento {
    id: number;
    nome: string;
    descricao: string;
}

export interface CategoriaLancamento {
    id: number;
    nome: string;
    descricao: string;
}