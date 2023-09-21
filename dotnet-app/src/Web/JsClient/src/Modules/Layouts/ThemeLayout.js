import { Outlet } from 'react-router-dom';
import React, { useState } from 'react';
import { MDBSwitch, MDBIcon } from 'mdb-react-ui-kit';

const ThemeLayout = () => {
    const light = 'light';
    const dark = 'dark';
    const themes = {
        light: { 
            key: light, 
            cssPaths: {
                main: 'css/mdb.min.css', 
                custom: 'css/custom/light.css'
            }, 
            icon: 'sun'
        },
        dark: { 
            key: dark, 
            cssPaths: {
                main: 'css/mdb.dark.min.css',
                custom: 'css/custom/dark.css'
            }, 
            icon: 'moon'
        }
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
        document.getElementById('theme').setAttribute('href', theme.cssPaths.main);
        document.getElementById('custom').setAttribute('href', theme.cssPaths.custom);
        localStorage.setItem('theme', JSON.stringify(theme));
        window.dispatchEvent(new Event('theme'));
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

            <div className='bg position-fixed bottom-0 end-0 m-3'>
                <div className='d-flex flex-column justify-content-center align-items-center border rounded p-2'>
                    <MDBIcon icon={theme.icon} className='mb-3' size='2x' title={theme.key}/>
                    <MDBSwitch title='Change theme here!' checked={theme.key === dark} onChange={(e) => toggleButton(e.target.checked)}/>
                </div>
            </div>
        </div>
    );
}

export default ThemeLayout;