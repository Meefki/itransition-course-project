import { Outlet } from 'react-router-dom';
import ThemeLayout from './ThemeLayout';
import Header from '../Shared/Header'

const MainLayout = () => {
    return (
        <div style={{height: '100vh'}}>
            <Header />
            <ThemeLayout>
                <Outlet />
            </ThemeLayout>
        </div>
    );
}

export default MainLayout;