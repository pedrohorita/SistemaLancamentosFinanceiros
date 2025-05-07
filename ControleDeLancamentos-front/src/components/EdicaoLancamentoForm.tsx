import React, { useState, useEffect } from 'react';
import { Lancamento, TipoLancamento, CategoriaLancamento, LancamentoModel } from '../types/lancamento';
import { fetchTipos, fetchCategorias, editTransaction } from '../services/api';
import '../styles/edicaoLancamentoForm.css'; // Importa o CSS para estilização

interface EditTransactionFormProps {
    lancamento: Lancamento;
    onUpdate: (lancamentoAtualizado: Lancamento) => void;
    onCancel: () => void;
}

const EditTransactionForm: React.FC<EditTransactionFormProps> = ({ lancamento, onUpdate, onCancel }) => {
    const [valor, setValor] = useState(lancamento.valor.toString());
    const [descricao, setDescricao] = useState(lancamento.descricao);
    const [tipoId, setTipoId] = useState(lancamento.tipo.id);
    const [categoriaId, setCategoriaId] = useState(lancamento.categoria.id);
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

        const lancamentoAtualizado: Lancamento = {
            ...lancamento,
            valor: parseFloat(valor),
            descricao,
            tipo: tipos.find((t) => t.id === tipoId) || lancamento.tipo,
            categoria: categorias.find((c) => c.id === categoriaId) || lancamento.categoria,
        };

        const lancamentoModel: LancamentoModel = {
            valor: parseFloat(valor),
            descricao,
            usuario: "user",
            categoriaId: categoriaId,
            tipoId: tipoId,
        };
        editTransaction(lancamento.id, lancamentoModel)
            .then(() => {
                console.log('Lançamento editado com sucesso!');
                onUpdate(lancamentoAtualizado);
            }
            )
            .catch((error) => {
                console.error('Erro ao editar lançamento:', error);
                setError('Erro ao editar lançamento. Tente novamente.');
            }
            );
        
        setError('');
    };

    return (
        <form className="edit-transaction-form" onSubmit={handleSubmit}>
            <h2>Editar Lançamento</h2>
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

export default EditTransactionForm;