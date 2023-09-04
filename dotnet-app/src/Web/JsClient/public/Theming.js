function applyTheme() {
    const light = 'light';
    const dark = 'dark';

    const theme = JSON.parse(localStorage.getItem('theme')) ?? { 
        key: light, 
        cssPaths: { 
            main: 'css/mdb.min.css',
            header: 'css/header/header.css'
        }
    };
    document.getElementById('theme').setAttribute('href', theme.cssPaths.main);
    document.getElementById('headerTheme').setAttribute('href', theme.cssPaths.header);
}