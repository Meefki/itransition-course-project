import React, { useContext, useEffect, useState, useMemo } from "react";
import { 
    MDBBtn,
    MDBDropdown,
    MDBDropdownMenu,
    MDBDropdownToggle,
    MDBIcon
} from 'mdb-react-ui-kit'
import { UserManagerContext } from '../../Contexts/UserManagerContext'
import Cookies from "js-cookie";
import { useTranslation } from "react-i18next";
import { UserInteraction } from "../../Services/UserInteraction";
import { useNavigate } from "react-router-dom";
import logol from '../../Assets/Img/logo-l-1.png'
import HrStyle from "../../Assets/Css/hr";

function Header() {
    const navigate = useNavigate();

    const [lastScrollTop, setLastScrollTop] = useState(0);
    const [scrollTop, setScrollTop] = useState(0);
    const [headerPos, setHeaderPos] = useState(0);

    const [pageLoadingStage, setPageLoadingStage] = useState(true);
    const ns = "header";
    const { t, i18n } = useTranslation(ns);

    const [isAuthorized, setIsAuthorized] = useState(null);
    const mgr = useContext(UserManagerContext);
    const userInteraction = useMemo(() => new UserInteraction(mgr), [mgr]);
    const [isActive, setIsAvtive] = useState(true);

    const imgSize = '50px';
    const languages = [
        { code: 'en', title: 'English', image_code: 'america'},
        { code: 'ru', title: 'Русский', image_code: 'russia'}
    ]

    const menuItems = () => {
        return [
            //{ tag: 'noAuth', value: <hr key="menu-item-3" className="dropdown-divider" /> },
            { tag: 'noAuth', value: [
                                        <span key="menu-item-4" className="dropdown-item">{t('language_menu_item')}</span>,
                                        <ul key="menu-item-5" className="dropdown-menu dropdown-submenu-left shadow-lg btn-group-vertical">
                                            {languages.map(({code, title, image_code}) => (
                                                <li key={code}>
                                                    <span 
                                                        className={`dropdown-item ${i18n.language === code ? 'disabled' : ''}`} 
                                                        tag={code} 
                                                        onClick={(e) => changeCountry(e.target.attributes['tag'].value)}>
                                                        <MDBIcon flag={image_code} className="mx-2" styles={i18n.language === code ? { opacity: '.5' } : {}}/>
                                                        {title}
                                                    </span>
                                                </li>
                                            ))}
                                        </ul>
                                    ]},
            { tag: 'auth', value: <hr key="menu-item-6" className="dropdown-divider" /> },
            { tag: 'auth', value: <button key="menu-item-7" className={`${isActive ? '' : 'disabled'} btn btn-link dropdown-item`} onClick={() => logout()}>{t('logout_btn')}</button> }    
        ]
    } 
    
    function changeCountry(code) {
        Cookies.set('i18next', code);
        localStorage.setItem('i18nextLng', code);
        i18n.changeLanguage(code);
    }

    async function login() {
        await userInteraction.login()
        setIsAvtive(false)
    }

    async function logout() {
        await userInteraction.logout()
        setIsAvtive(false)
    }

    useEffect(() => {
        userInteraction.isAuthorized().then((isAuth) => {
            setIsAuthorized(isAuth);
        });
    })

    /* eslint-disable */
    useMemo(() => {
        i18n.isInitialized &&
        !i18n.hasLoadedNamespace(ns) && 
            i18n.loadNamespaces(ns)
            .then(() => {
                setPageLoadingStage(false);
            });
        i18n.isInitialized &&
        i18n.hasLoadedNamespace(ns) &&
            setPageLoadingStage(false);
    }, [i18n.isInitialized]);

    useEffect(() => {
        const scrollDirection = scrollTop > lastScrollTop

        const header = document.getElementById('header');
        if (header?.style) {
            setHeaderPos(scrollDirection ? -header.clientHeight : 0);
        }

        setLastScrollTop(scrollTop <= 0 ? 0 : scrollTop);
    }, [scrollTop])

    useEffect(() => {
        window.addEventListener('scroll', handleOnScroll);
        mgr.clearStaleState();

        return () => {
            window.removeEventListener('scroll', handleOnScroll);
        }
    }, [])
    /* eslint-enable */

    const handleOnScroll = () => {
        setScrollTop(window.scrollY || document.documentElement.scrollTop);
    }

    return pageLoadingStage ? '' :
        <div id="header" className="header" style={{
            transform: `translateY(${headerPos ?? 0}px)`,
            position: 'sticky',
            top: '0px',
            zIndex: 1000,
            transition: 'transform .1s ease 0s',
        }}>
            <div className="d-flex flex-row py-2">
                <img src={logol} alt="" width={imgSize} height={imgSize} className="rounded-circle ms-3" onClick={() => navigate("/")}/>
                <nav className="w-100 d-flex align-items-center justify-content-between ms-4 me-4">
                    <ul className="m-0 p-0">
                    </ul>
                    
                    <MDBDropdown group>
                        <MDBDropdownToggle id='dd-toggle' split/>
                        {
                            isAuthorized ?
                                <MDBBtn onClick={() => navigate("/profile/me")}>{t('profile_nav_item')}</MDBBtn> :
                                <MDBBtn onClick={() => login()}>{t('login_btn')}</MDBBtn>
                        }
                        <MDBDropdownMenu className="shadow-lg">
                            {
                                menuItems().filter((item) => {
                                    return isAuthorized || item.tag !== 'auth'
                                }).map((item, index) => <li key={index}>{item.value}</li>)
                            }
                        </MDBDropdownMenu>
                    </MDBDropdown>
                </nav>
            </div>
            <div style={HrStyle.horizontalHrStyle}/>
        </div>
        
}

export default Header;