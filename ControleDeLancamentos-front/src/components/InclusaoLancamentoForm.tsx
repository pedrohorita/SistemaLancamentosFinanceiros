import React, { useState, useEffect } from 'react';
import { Lancamento, TipoLancamento, CategoriaLancamento } from '../types/lancamento';
import { fetchTipos, fetchCategorias, addTransaction } from '../services/api';
import '../styles/inclusaoLancamentoForm.css'; // Importa o CSS para estilização

interface AddTransactionFormProps {
    onAddTransaction: (lancamento: Lancamento) => void;
    onCancel: () => void;
}

const AddTransactionForm: React.FC<AddTransactionFormProps> = ({ onAddTransaction, onCancel }) => {
    const [valor, setValor] = useState('');
    const [descricao, setDescricao] = useState('');
    const [tipoId, setTipoId] = useState(0);
    const [categoriaId, setCategoriaId] = useState(0);
    const [tipos, setTipos] = useState<TipoLancamento[]>([]);
    const [categorias, setCategorias] = useState<CategoriaLancamento[]>([]);
    const [error, setError] = useState('');

    useEffect(() => {
        const loadTipos = async () => {
            try {
                const tiposData = await fetchTipos();
                setTipos(tiposData);
            } catch (error) {
                console.error('Erro ao buscar tipos:', error);
            }
        };

        const loadCategorias = async () => {
            try {
                const categoriasData = await fetchCategorias();
                setCategorias(categoriasData);
            } catch (error) {
                console.error('Erro ao buscar categorias:', error);
            }
        };

        loadTipos();
        loadCategorias();
    }, []);

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        if (!valor || !descricao || tipoId === 0 || categoriaId === 0) {
            setError('Todos os campos são obrigatórios.');
            return;
        }

        const novoLancamento: Lancamento = {
            id: '',
            valor: parseFloat(valor),
            descricao,
            data: new Date().toISOString(),
            usuario: 'user', // Substitua pelo usuário real
            tipo: tipos.find((t) => t.id === tipoId) || { id: 0, nome: '', descricao: '' },
            categoria: categorias.find((c) => c.id === categoriaId) || { id: 0, nome: '', descricao: '' },
        };

        const lancamentoModel = {
            valor: parseFloat(valor),
            descricao,
            usuario: 'user', // Substitua pelo usuário real
            categoriaId,
            tipoId,
        };
        addTransaction(lancamentoModel)
            .then((response) => {
                novoLancamento.id = response; 
                console.log('Lançamento adicionado com sucesso:', novoLancamento.id);
                
                onAddTransaction(novoLancamento);
            }
            )
            .catch((error) => {
                console.error('Erro ao adicionar lançamento:', error);
                setError('Erro ao adicionar lançamento. Tente novamente.');
            }
            );
            

        setValor('');
        setDescricao('');
        setTipoId(0);
        setCategoriaId(0);
        setError('');
    };

    return (
        <form className="add-transaction-form" onSubmit={handleSubmit}>
            <h2>Realizar Lançamento</h2>
            {error && <p className="error-message">{error}</p>}
            <div className="form-group">
                <label>Valor:</label>
                <input
                    type="number"
                    value={valor}
                    onChange={(e) => setValor(e.target.value)}
                    placeholder="Digite o valor"
                    required
                />
            </div>
            <div className="form-group">
                <label>Descrição:</label>
                <input
                    type="text"
                    value={descricao}
                    onChange={(e) => setDescricao(e.target.value)}
                    placeholder="Digite a descrição"
                    required
                />
            </div>
            <div className="form-group">
                <label>Tipo:</label>
                <select
                    value={tipoId}
                    onChange={(e) => setTipoId(Number(e.target.value))}
                    required
                >
                    <option value={0}>Selecione um tipo</option>
                    {tipos.map((tipo) => (
                        <option key={tipo.id} value={tipo.id}>
                            {tipo.nome}
                        </option>
                    ))}
                </select>
            </div>
            <div className="form-group">
                <label>Categoria:</label>
                <select
                    value={categoriaId}
                    onChange={(e) => setCategoriaId(Number(e.target.value))}
                    required
                >
                    <option value={0}>Selecione uma categoria</option>
                    {categorias.map((categoria) => (
                        <option key={categoria.id} value={categoria.id}>
                            {categoria.nome}
                        </option>
                    ))}
                </select>
            </div>
            <div className="form-actions">
                <button type="submit" className="save-button">Salvar</button>
                <button type="button" className="cancel-button" onClick={onCancel}>
                    Cancelar
                </button>
            </div>
        </form>
    );
};

export default AddTransactionForm;