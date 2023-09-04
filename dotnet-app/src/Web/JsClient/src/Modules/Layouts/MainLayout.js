import { Outlet } from 'react-router-dom';
import ThemeLayout from './ThemeLayout';

const MainLayout = () => {
    return (
        <ThemeLayout>
            <Outlet />
        </ThemeLayout>
    );
}

export default MainLayout;