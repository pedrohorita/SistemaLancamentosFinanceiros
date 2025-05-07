import React from 'react';
import { Lancamento } from '../types/lancamento';

interface TransactionItemProps {
    lancamento: Lancamento;
    onEdit: (lancamento: Lancamento) => void;
    onDelete: (id: string) => void;
}

const TransactionItem: React.FC<TransactionItemProps> = ({ lancamento, onEdit, onDelete }) => {
    return (
        <li>
            <span>{lancamento.descricao}</span>
            <span>{lancamento.valor}</span>
            <span>{lancamento.data}</span>
            <span>{lancamento.usuario}</span>
            <button onClick={() => onEdit(lancamento)}>Edit</button>
            <button onClick={() => onDelete(lancamento.id)}>Delete</button>
        </li>
    );
};

export default TransactionItem;