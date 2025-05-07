import React, { useState } from 'react';
import '../styles/lancamentoLista.css';
import { Lancamento } from '../types/lancamento';
import { deleteTransaction } from '../services/api';

interface TransactionListProps {
    lancamentos: Lancamento[];
    onEdit: (lancamento: Lancamento) => void;
    onDelete: (id: string) => void;
}

const TransactionList: React.FC<TransactionListProps> = ({ lancamentos, onEdit, onDelete }) => {
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [selectedId, setSelectedId] = useState<string | null>(null);

    const openModal = (id: string) => {
        setSelectedId(id);
        setIsModalOpen(true);
    };

    const closeModal = () => {
        setSelectedId(null);
        setIsModalOpen(false);
    };

    const confirmDelete = () => {
        if (selectedId) {     
            deleteTransactionHandler(selectedId);
        }
        closeModal();
    };

    const deleteTransactionHandler = async (id: string) => {
        try {
            await deleteTransaction(id);
            onDelete(id);
        } catch (error) {
            console.error('Erro ao excluir transação:', error);
        }
    };

    const totalEntradas = lancamentos
        .filter((lancamento) => lancamento.tipo.id === 1)
        .reduce((acc, lancamento) => acc + lancamento.valor, 0);

    const totalSaidas = lancamentos
        .filter((lancamento) => lancamento.tipo.id === 2)
        .reduce((acc, lancamento) => acc + lancamento.valor, 0);

    const saldoTotal = totalEntradas - totalSaidas;

    return (
        <div className="transaction-list-container">
            <h2 className="transaction-list-title">Lista de Lançamentos</h2>
            {lancamentos.length === 0 ? (
                <p className="transaction-list-empty">Nenhum lançamento encontrado.</p>
            ) : (
                <table className="transaction-list-table">
                    <thead>
                        <tr>
                            <th>Data</th>
                            <th>Descrição</th>
                            <th>Categoria</th>
                            <th>Tipo</th>
                            <th>Valor</th>
                            <th>Ações</th>
                        </tr>
                    </thead>
                    <tbody>
    {lancamentos.map((lancamento) => (
        <tr
            key={lancamento.id}
            className={lancamento.tipo.id === 1 ? 'lancamento-entrada' : 'lancamento-saida'}
        >
            <td>{new Date(lancamento.data).toLocaleDateString('pt-BR')}</td>
            <td>{lancamento.descricao}</td>
            <td>{lancamento.categoria.nome}</td>
            <td>{lancamento.tipo.nome}</td>
            <td>R$ {lancamento.valor.toFixed(2)}</td>
            <td>
                <button
                    className="transaction-list-edit-button"
                    onClick={() => onEdit(lancamento)}
                >
                    Editar
                </button>
                <button
                    className="transaction-list-delete-button"
                    onClick={() => openModal(lancamento.id)}
                >
                    Excluir
                </button>
            </td>
        </tr>
    ))}
    {/* Linha de totais */}
    <tr className="totals-row">
        <td colSpan={2}><strong>Total Entradas:</strong> R$ {totalEntradas.toFixed(2)}</td>
        <td colSpan={2}><strong>Total Saídas:</strong> R$ {totalSaidas.toFixed(2)}</td>
        <td colSpan={2}>
            <strong
                style={{
                    color: saldoTotal < 0 ? 'red' : 'green',
                    fontWeight: 'bold',
                }}
            >
                Saldo Total: R$ {saldoTotal.toFixed(2)}
            </strong>
        </td>
    </tr>
</tbody>
                </table>
            )}

            {isModalOpen && (
                <div className="modal">
                    <div className='modal-content'>
                        <h2 className="modal-title">Confirmação de Exclusão</h2>
                        <p className="modal-message">
                            Tem certeza que deseja excluir este lançamento? Esta ação não pode ser desfeita.
                        </p>
                        <div className="modal-actions">
                            <button className="confirmation-cancel-button" onClick={closeModal}>
                                Cancelar
                            </button>
                            <button className="confirmation-delete-button" onClick={confirmDelete}>
                                Excluir
                            </button>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
};

export default TransactionList;