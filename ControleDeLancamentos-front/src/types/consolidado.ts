import { CategoriaLancamento } from "./lancamento";

export interface Consolidado {
    id: string;
    data: string;
    totalReceitas: number;
    totalDespesas: number;
    saldo: number;    
}

export interface ConsolidadoPorCategoria {
    id: string;
    data: string;
    categoria: CategoriaLancamento;
    totalReceitas: number;
    totalDespesas: number;
    saldo: number;    
}