import React, { useState } from 'react';
import '../styles/paginaInicial.css';
import TransactionsPage from './LancamentoPage';
import ConsolidadoDiarioPage from './ConsolidadoDiarioPage';

interface Consolidado {
    data: string;
    total: number;
}

const HomePage: React.FC = () => {
    const [activeContent, setActiveContent] = useState<'lancamentos' | 'consolidado' | null>(null);
    const [consolidado, setConsolidado] = useState<Consolidado[]>([]);

    const handleShowHome = () => {
        setActiveContent(null); // Define o estado como null para exibir o conteúdo inicial
    };

    const handleShowLancamentos = () => {
        setActiveContent('lancamentos');
    };

    const handleShowConsolidado = async () => {
        setActiveContent('consolidado');
        try {
            const response = await fetch('/api/consolidado'); // Substitua pela URL correta da API
            const data = await response.json();
            setConsolidado(data);
        } catch (error) {
            console.error('Erro ao carregar consolidado diário:', error);
        }
    };

    return (
        <div className="home-page">
            <div className="sidebar">
                <h2 onClick={handleShowHome} style={{ cursor: 'pointer' }}>
                    App de Lançamentos
                </h2>
                <ul>
                    <li>
                        <button
                            onClick={handleShowLancamentos}
                            className={activeContent === 'lancamentos' ? 'active' : ''}
                        >
                            Lançamentos
                        </button>
                    </li>
                    <li>
                        <button
                            onClick={handleShowConsolidado}
                            className={activeContent === 'consolidado' ? 'active' : ''}
                        >
                            Consolidado Diário
                        </button>
                    </li>
                </ul>
            </div>
            <div className="content">
                {activeContent === null && (
                    <>
                        <h1>Bem-vindo ao App de Lançamentos</h1>
                        <p>Este aplicativo permite que você gerencie seus lançamentos financeiros com facilidade.</p>
                    </>
                )}
                {activeContent === 'lancamentos' && <TransactionsPage />}
                {activeContent === 'consolidado' && <ConsolidadoDiarioPage/>}
            </div>
        </div>
    );
};

export default HomePage;