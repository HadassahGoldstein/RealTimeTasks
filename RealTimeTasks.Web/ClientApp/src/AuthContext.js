import React, { useState, useEffect, createContext, useContext } from 'react';
import axios from 'axios';

const AuthContext = createContext();

const AuthContextComponent = ({children}) => {
    const [user, setUser] = useState(null);
    const [bookmarkContent, setBookmarkContent] = useState({});

    useEffect(() => {
        const getUser = async () => {
            const { data } = await axios.get('/api/account/getcurrentuser');
            setUser(data);
        }
        getUser();
    }, []);

    const logout = () => setUser(null);

    return (
        <AuthContext.Provider value={{ user, logout, setUser, bookmarkContent, setBookmarkContent }}>
            {children}
        </AuthContext.Provider>
    )
}

const useAuthContext = () => useContext(AuthContext);

export { AuthContextComponent, useAuthContext};