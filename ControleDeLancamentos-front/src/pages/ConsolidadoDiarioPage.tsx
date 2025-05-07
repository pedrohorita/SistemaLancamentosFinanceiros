import React, { useState } from 'react';
import { fetchConsolidadosPeriodo, fetchConsolidadosPorCategoria } from '../services/api';
import '../styles/consolidadoDiarioPage.css';
import { Consolidado, ConsolidadoPorCategoria } from '../types/consolidado';

const ConsolidadoDiarioPage: React.FC = () => {
    const [dataInicio, setDataInicio] = useState('');
    const [dataFim, setDataFim] = useState('');
    const [consolidados, setConsolidados] = useState<Consolidado[]>([]);
    const [expandedRow, setExpandedRow] = useState<string | null>(null);
    const [categoriasPorLinha, setCategoriasPorLinha] = useState<ConsolidadoPorCategoria[] | null>(null);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');

    const handleFetchConsolidados = async () => {
        if (!dataInicio || !dataFim) {
            setError('Por favor, preencha as datas de início e fim.');
            return;
        }

        setLoading(true);
        setError('');
        try {
            const data = await fetchConsolidadosPeriodo(dataInicio, dataFim);
            setConsolidados(data);
            setExpandedRow(null); // Reseta a linha expandida ao buscar novos dados
        } catch (err) {
            setError('Erro ao buscar o relatório de consolidado diário.');
        } finally {
            setLoading(false);
        }
    };

    const handleRowClick = async (data: string) => {
        if (expandedRow === data) {
            // Fecha a sublista se a mesma linha for clicada novamente
            setExpandedRow(null);
            setCategoriasPorLinha(null);
            return;
        }

        setLoading(true);
        setError('');
        try {
            const categorias = await fetchConsolidadosPorCategoria(data);
            setExpandedRow(data);
            setCategoriasPorLinha(categorias);
        } catch (err) {
            setError('Erro ao buscar o consolidado por categoria.');
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="daily-consolidated-report-container">
            <h1 className="title">Relatório de Consolidado Diário</h1>
            <div className="filters">
                <label>
                    Data Início:
                    <input
                        type="date"
                        value={dataInicio}
                        onChange={(e) => setDataInicio(e.target.value)}
                    />
                </label>
                <label>
                    Data Fim:
                    <input
                        type="date"
                        value={dataFim}
                        onChange={(e) => setDataFim(e.target.value)}
                    />
                </label>
                <button onClick={handleFetchConsolidados} disabled={loading}>
                    {loading ? 'Carregando...' : 'Buscar'}
                </button>
            </div>
            {error && <p className="error-message">{error}</p>}
            {loading ? (
                <div className="loading-spinner">Carregando...</div>
            ) : consolidados.length > 0 ? (
                <ConsolidadoTable
                    consolidados={consolidados}
                    expandedRow={expandedRow}
                    categoriasPorLinha={categoriasPorLinha}
                    onRowClick={handleRowClick}
                />
            ) : (
                !error && <p className="empty-message">Nenhum consolidado encontrado.</p>
            )}
        </div>
    );
};

const ConsolidadoTable: React.FC<{
    consolidados: Consolidado[];
    expandedRow: string | null;
    categoriasPorLinha: ConsolidadoPorCategoria[] | null;
    onRowClick: (data: string) => void;
}> = ({ consolidados, expandedRow, categoriasPorLinha, onRowClick }) => {
    const totalReceitas = consolidados.reduce((acc, item) => acc + item.totalReceitas, 0);
    const totalDespesas = consolidados.reduce((acc, item) => acc + item.totalDespesas, 0);
    const totalSaldo = consolidados.reduce((acc, item) => acc + item.saldo, 0);

    const formatLocalDate = (dateString: string): string => {
        const [year, month, day] = dateString.split('-');
        return `${day}/${month}/${year}`;
    };

    return (
        <table className="consolidated-table">
            <thead>
                <tr>
                    <th>Data</th>
                    <th>Receitas</th>
                    <th>Despesas</th>
                    <th>Saldo</th>
                </tr>
            </thead>
            <tbody>
                {consolidados.map((item, index) => (
                    <React.Fragment key={index}>
                        <tr
                            onClick={() => onRowClick(item.data)}
                            className={item.saldo > 0 ? 'positive-balance' : item.saldo < 0 ? 'negative-balance' : ''}
                        >
                            <td>{formatLocalDate(item.data)}</td>
                            <td>R$ {item.totalReceitas.toFixed(2)}</td>
                            <td>R$ {item.totalDespesas.toFixed(2)}</td>
                            <td>R$ {item.saldo.toFixed(2)}</td>
                        </tr>
                        {expandedRow === item.data && categoriasPorLinha && (
                            <tr>
                                <td colSpan={4}>
                                    <ConsolidadoCategoriaTable categorias={categoriasPorLinha} />
                                </td>
                            </tr>
                        )}
                    </React.Fragment>
                ))}
                <tr className="totals-row">
                    <td><strong>Total</strong></td>
                    <td><strong>R$ {totalReceitas.toFixed(2)}</strong></td>
                    <td><strong>R$ {totalDespesas.toFixed(2)}</strong></td>
                    <td className={totalSaldo > 0 ? 'positive-balance' : totalSaldo < 0 ? 'negative-balance' : ''}><strong>R$ {totalSaldo.toFixed(2)}</strong></td>
                </tr>
            </tbody>
        </table>
    );
};

const ConsolidadoCategoriaTable: React.FC<{ categorias: ConsolidadoPorCategoria[] }> = ({ categorias }) => (
    <table className="consolidated-category-table">
        <thead>
            <tr>
                <th>Categoria</th>
                <th>Receitas</th>
                <th>Despesas</th>
                <th>Saldo</th>
            </tr>
        </thead>
        <tbody>
            {categorias.map((categoria, index) => (
                <tr key={index}>
                    <td>{categoria.categoria.nome}</td>
                    <td>R$ {categoria.totalReceitas.toFixed(2)}</td>
                    <td>R$ {categoria.totalDespesas.toFixed(2)}</td>
                    <td>R$ {categoria.saldo.toFixed(2)}</td>
                </tr>
            ))}
        </tbody>
    </table>
);

export default ConsolidadoDiarioPage;