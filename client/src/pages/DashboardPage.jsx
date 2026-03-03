import { useAuth } from '../context/AuthContext';
import { useNavigate } from 'react-router-dom';
import '../styles/dashboard.css';

export default function DashboardPage() {
    const { user, logout } = useAuth();
    const navigate = useNavigate();

    const handleLogout = async () => {
        await logout();
        navigate('/login');
    };

    return (
        <div className="dashboard-wrapper">
            <header className="dashboard-header">
                <div className="dashboard-brand">
                    <span className="brand-icon">⚡</span>
                    <span className="brand-name">Tapp</span>
                </div>
                <div className="header-right">
                    <span className="user-badge">{user?.email}</span>
                    <button className="btn-logout" onClick={handleLogout}>
                        Sign out
                    </button>
                </div>
            </header>

            <main className="dashboard-main">
                <div className="welcome-card">
                    <h1 className="welcome-title">You're in! 🎉</h1>
                    <p className="welcome-sub">
                        Signed in as <strong>{user?.email}</strong>
                    </p>
                    <p className="welcome-hint">
                        Start by exploring the API or building out your app features.
                    </p>
                </div>

                <div className="stats-grid">
                    <div className="stat-card">
                        <div className="stat-icon">📋</div>
                        <div className="stat-label">Tasks API</div>
                        <code className="stat-url">GET /api/tasks</code>
                    </div>
                    <div className="stat-card">
                        <div className="stat-icon">👤</div>
                        <div className="stat-label">Users API</div>
                        <code className="stat-url">GET /api/users</code>
                    </div>
                    <div className="stat-card">
                        <div className="stat-icon">🔐</div>
                        <div className="stat-label">Auth</div>
                        <code className="stat-url">Cookie JWT</code>
                    </div>
                </div>
            </main>
        </div>
    );
}
