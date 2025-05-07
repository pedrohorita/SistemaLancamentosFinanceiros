import React, { useEffect, useState } from 'react';
import AddTransactionForm from '../components/InclusaoLancamentoForm';
import TransactionList from '../components/LancamentoLista';
import { Lancamento, LancamentoModel } from '../types/lancamento';
import { fetchTransactions } from '../services/api';
import EditTransactionForm from '../components/EdicaoLancamentoForm';

const TransactionsPage: React.FC = () => {
    const [lancamentos, setTransactions] = useState<Lancamento[]>([]);
    const [lancamentoEditando, setLancamentoEditando] = useState<Lancamento | null>(null);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [isAddModalOpen, setIsAddModalOpen] = useState(false);
    const openAddModal = () => setIsAddModalOpen(true);
    const closeAddModal = () => setIsAddModalOpen(false);

    useEffect(() => {
        const loadTransactions = async () => {
            const data = await fetchTransactions();
            setTransactions(data.lancamentos);
        };

        loadTransactions();
    }, []);

    const addTransaction = (lancamento: Lancamento) => {
        setTransactions([...lancamentos, lancamento]);
        closeAddModal();
    };

    const editTransaction = (updatedTransaction: Lancamento) => {
        console.log('Editando transação:', updatedTransaction);
        setTransactions(lancamentos.map(lancamento => 
            lancamento.id === updatedTransaction.id ? updatedTransaction : lancamento
        ));
    };

    const deleteTransaction = (id: string) => {
        setTransactions(lancamentos.filter(lancamento => lancamento.id !== id));
    };

    const openEditModal = (lancamento: Lancamento) => {
        setLancamentoEditando(lancamento);
        setIsModalOpen(true);
    };
    
    const closeEditModal = () => {
        setLancamentoEditando(null);
        setIsModalOpen(false);
    };

    return (
        <div>
            <h1>Lançamentos</h1>
            <button onClick={openAddModal}>Realizar Lançamento</button>

        {isAddModalOpen && (
            <div className="modal">
                <div className="modal-content">
                    <button className="close-button" onClick={closeAddModal}>X</button>
                    <AddTransactionForm 
                            onAddTransaction={(novoLancamento) => {
                                addTransaction(novoLancamento);
                                closeAddModal();
                            } } 
                            onCancel={closeAddModal}
                        />
                </div>
            </div>
        )}
            {isModalOpen && lancamentoEditando && (
            <div className="modal">
                <div className="modal-content">
                    <button className="close-button" onClick={closeEditModal}>X</button>
                    <EditTransactionForm 
                        lancamento={lancamentoEditando} 
                        onUpdate={(updatedLancamento) => {
                            const lancamento : Lancamento = {
                                ...lancamentoEditando,
                                ...updatedLancamento,
                            };
                            editTransaction(lancamento);
                            closeEditModal();
                        }}
                        onCancel={closeEditModal}
                    />
                </div>
            </div>
        )}

        <TransactionList 
            lancamentos={lancamentos} 
            onEdit={openEditModal} 
            onDelete={deleteTransaction} 
        />
    </div>
        
        
    );
};

export default TransactionsPage;