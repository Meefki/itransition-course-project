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

function Header() {
    const navigate = useNavigate();

    const [pageLoadingStage, setPageLoadingStage] = useState(true);
    const ns = "header";
    const { t, i18n } = useTranslation(ns);

    const [isAuthorized, setIsAuthorized] = useState(null);
    const mgr = useContext(UserManagerContext);
    const userInteraction = useMemo(() => new UserInteraction(mgr), [mgr]);
    const [isActive, setIsAvtive] = useState(true);

    const imgSize = '50px';
    const languages = [
        { code: 'en', title: 'English', country_code: 'america'},
        { code: 'ru', title: 'Русский', country_code: 'russia'}
    ]

    const menuItems = () => {
        return [
            { tag: 'noAuth', value: <span key="menu-item-1" className="dropdown-item">Action 2</span> },
            { tag: 'noAuth', value: <span key="menu-item-2" className="dropdown-item">Action 1</span> },
            { tag: 'noAuth', value: <hr key="menu-item-3" className="dropdown-divider" /> },
            { tag: 'noAuth', value: [
                                        <span key="menu-item-4" className="dropdown-item">{t('language_menu_item')}</span>,
                                        <ul key="menu-item-5" className="dropdown-menu dropdown-submenu-left shadow-lg btn-group-vertical">
                                            {languages.map(({code, title, country_code}) => (
                                                <li key={code}>
                                                    <span 
                                                        className={`dropdown-item ${i18n.language === code ? 'disabled' : ''}`} 
                                                        tag={code} 
                                                        onClick={(e) => changeCountry(e.target.attributes['tag'].value)}>
                                                        <MDBIcon flag={country_code} className="mx-2" styles={i18n.language === code ? { opacity: '.5' } : {}}/>
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
    /* eslint-enable */

    return pageLoadingStage ? '' :
        <div id="header" className="header pt-2 pb-2 border-bottom-2 shadow">
            <div className="d-flex flex-row">
                <img 
                    style={{width: imgSize, height: imgSize}}
                    alt=""
                    src="https://placehold.co/50x50"
                    className="rounded-circle ms-3"
                    onClick={() => navigate("/")}/>
                <nav className="w-100 d-flex align-items-center justify-content-between ms-4 me-4">
                    <ul className="m-0 p-0">
                        <li className="d-inline me-5">{t('categories_nav_item')}</li>
                        <li className="d-inline me-5">{t('categories_nav_item')}</li>
                        <li className="d-inline me-5">{t('categories_nav_item')}</li>
                        <li className="d-inline me-5">{t('categories_nav_item')}</li>
                    </ul>
                    
                    <MDBDropdown dropleft group>
                        <MDBDropdownToggle id='dd-toggle' split/>
                        {
                            isAuthorized ?
                                <MDBBtn onClick={() => navigate("/profile")}>{t('profile_nav_item')}</MDBBtn> :
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
        </div>
}

export default Header;