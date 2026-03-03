import { createContext, useContext, useState, useCallback } from 'react';

const AuthContext = createContext(null);

export function AuthProvider({ children }) {
    const [user, setUser] = useState(() => {
        const saved = localStorage.getItem('user');
        return saved ? JSON.parse(saved) : null;
    });

    const login = useCallback(async (email, password) => {
        const res = await fetch('/api/users/login', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            credentials: 'include',
            body: JSON.stringify({ email, password }),
        });
        if (!res.ok) throw new Error('Invalid credentials');
        const data = await res.json();
        const userData = { email };
        setUser(userData);
        localStorage.setItem('user', JSON.stringify(userData));
        return data;
    }, []);

    const register = useCallback(async ({ name, email, password }) => {
        const res = await fetch('/api/users', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ name, email, password, role: 'User' }),
        });
        if (!res.ok) throw new Error('Registration failed');
        return res.json();
    }, []);

    const logout = useCallback(async () => {
        await fetch('/api/users/logout', {
            method: 'POST',
            credentials: 'include',
        });
        setUser(null);
        localStorage.removeItem('user');
    }, []);

    return (
        <AuthContext.Provider value={{ user, login, register, logout }}>
            {children}
        </AuthContext.Provider>
    );
}

export function useAuth() {
    return useContext(AuthContext);
}
