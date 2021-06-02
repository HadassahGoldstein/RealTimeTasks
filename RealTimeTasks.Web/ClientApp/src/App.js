import React from 'react';
import { Route } from 'react-router';
import  Layout from './components/Layout';
import Home from './pages/Home';
import Signup from './pages/Signup';
import Login from './pages/Login';
import Logout from './pages/Logout';
import PrivateRoute from './PrivateRoute';
import { AuthContextComponent } from './AuthContext';



const App = () => {
  return (
    <AuthContextComponent>
      <Layout>
        <PrivateRoute exact path='/' component={Home} />
        <Route exact path='/Signup' component={Signup} />
        <Route exact path='/Login' component={Login} />
        <Route exact path='/Logout' component={Logout} />
      </Layout>
    </AuthContextComponent>
  )
}

export default App;
