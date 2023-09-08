function applyTheme() {
    const light = 'light';
    const dark = 'dark';

    const theme = JSON.parse(localStorage.getItem('theme')) ?? { 
        key: light, 
        cssPaths: { 
            main: 'css/mdb.min.css',
            custom: 'css/custom/light.css'
        }
    };
    document.getElementById('theme').setAttribute('href', theme.cssPaths.main);
    document.getElementById('custom').setAttribute('href', theme.cssPaths.custom);
}