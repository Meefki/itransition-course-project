import { Outlet } from 'react-router-dom';
import React, { useState } from 'react';
import { MDBSwitch, MDBIcon } from 'mdb-react-ui-kit';

const ThemeLayout = () => {
    const light = 'light';
    const dark = 'dark';
    const themes = {
        light: { key: light, cssPath: 'css/mdb.min.css', icon: 'sun' },
        dark: { key: dark, cssPath: 'css/mdb.dark.min.css', icon: 'moon' }
    };

    const loadTheme = () => {
        const preTheme = JSON.parse(localStorage.getItem('theme'));
        if (preTheme && preTheme.key) {
            return themes[preTheme.key];
        }

        return themes[light];
    }
    const [theme, setTheme] = useState(loadTheme);

    const changeTheme = (theme) => {
        document.getElementById('theme').setAttribute('href', theme.cssPath);
        localStorage.setItem('theme', JSON.stringify(theme));
    }

    const toggleButton = (isChecked) => {
        if (isChecked) {
            setTheme(themes[dark]);
            changeTheme(themes[dark]);
            return;
        }

        changeTheme(themes[light]);
        setTheme(themes[light]);
    }

    return (
        <div>
            <Outlet />

            <div className='position-absolute bottom-0 end-0 m-3'>
                <div className='d-flex flex-column justify-content-center align-items-center border rounded p-2'>
                    <MDBIcon icon={theme.icon} className='mb-3' size='2x'/>
                    <MDBSwitch title='Change theme here!' checked={theme.key === dark} onChange={(e) => toggleButton(e.target.checked)}/>
                </div>
            </div>
        </div>
    );
}

export default ThemeLayout;