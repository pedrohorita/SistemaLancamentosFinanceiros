import axios from 'axios';
import { LancamentoPaginado, LancamentoModel, TipoLancamento, CategoriaLancamento } from '../types/lancamento';
import { Consolidado, ConsolidadoPorCategoria } from '../types/consolidado';

const API_URL = process.env.REACT_APP_API_LANCAMENTOS_URL;
const API_URL_CONSOLIDADOS = process.env.REACT_APP_API_CONSOLIDADOS_URL;

const API_URL_CONSOLIDADO = `${API_URL_CONSOLIDADOS}/consolidado`;
const API_URL_LANCAMENTOS = `${API_URL}/lancamentos`;
const API_URL_CATEGORIAS = `${API_URL}/categorias`;
const API_URL_TIPOS = `${API_URL}/tipos`;

export const fetchTransactions = async (): Promise<LancamentoPaginado> => {
    const response = await axios.get<LancamentoPaginado>(API_URL_LANCAMENTOS);
    if (response.status === 204) {
        return {
            lancamentos: [],
            totalPaginas: 0,
            paginaAtual: 0,
            totalRegistros: 0,
            tamanhoPagina: 0,
        };
    }
    if (response.status !== 200) {
        throw new Error('Erro ao buscar lançamentos');
    }
    return response.data;
};

export const addTransaction = async (transaction: LancamentoModel): Promise<string> => {
    const response = await axios.post<{id:string}>(API_URL_LANCAMENTOS, transaction);
    return response.data.id;
};

export const editTransaction = async (id: string, lancamento: LancamentoModel): Promise<void> => {
    await axios.put(`${API_URL_LANCAMENTOS}/${id}`, lancamento);
};

export const deleteTransaction = async (id: string): Promise<void> => {
    await axios.delete(`${API_URL_LANCAMENTOS}/${id}`);
};

export const fetchCategorias = async (): Promise<CategoriaLancamento[]> => {
    const response = await axios.get<CategoriaLancamento[]>(API_URL_CATEGORIAS);
    return response.data;
};

export const fetchTipos = async (): Promise<TipoLancamento[]> => {
    const response = await axios.get<TipoLancamento[]>(API_URL_TIPOS);
    return response.data;
};

export const fetchConsolidadoPorData = async (data: string): Promise<any> => {
    const response = await axios.get<Consolidado>(`${API_URL_CONSOLIDADO}?data=${data}`);
    return response.data;
};

export const fetchConsolidadosPeriodo = async (dataInicio: string, dataFim: string): Promise<any> => {
    const response = await axios.get<Consolidado[]>(`${API_URL_CONSOLIDADO}/periodo?dataInicio=${dataInicio}&dataFim=${dataFim}`);
    return response.data;
};

export const fetchConsolidadosPorCategoria = async (data: string): Promise<any> => {
    const response = await axios.get<ConsolidadoPorCategoria[]>(`${API_URL_CONSOLIDADO}/categoria?data=${data}`);
    return response.data;
};

export const fetchConsolidadosPorCategoriaPeriodo = async (dataInicio: string, dataFim: string): Promise<any> => {
    const response = await axios.get<ConsolidadoPorCategoria[]>(`${API_URL_CONSOLIDADO}/categoria/periodo?dataInicio=${dataInicio}&dataFim=${dataFim}`);
    return response.data;
}

