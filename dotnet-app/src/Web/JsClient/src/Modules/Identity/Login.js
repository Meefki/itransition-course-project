import React, { useState, useMemo, useEffect, useContext } from 'react';
import {
  MDBContainer,
  MDBTabs,
  MDBTabsItem,
  MDBTabsLink,
  MDBTabsContent,
  MDBTabsPane,
  MDBBtn,
  MDBIcon,
  MDBInput,
  MDBCheckbox,
  MDBSpinner
}
from 'mdb-react-ui-kit';
import CryptoJS from 'crypto-js';
import { IdentityService } from '../../Services/IdentityService';
import { useTranslation } from 'react-i18next';
import { UserManagerContext } from '../../Contexts/UserManagerContext';
import { useNavigate } from 'react-router-dom';

export function Login() {
    
    const ns = 'identity-login';
    const { t, i18n } = useTranslation(ns);

    const mgr = useContext(UserManagerContext);
    const navigate = useNavigate();

    const [justifyActive, setJustifyActive] = useState('login')
    // const [name, setName] = useState('');
    const [username, setUsername] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [rememberMe, setRememberMe] = useState(true);
    const [passHash, setPassHash] = useState('');

    const [error, setError] = useState('');

    const [loadingState, setLoadingState] = useState(false);
    const [pageLoadingStage, setPageLoadingStage] = useState(true);

    const identityService = new IdentityService();

    function handleJustifyClick(value) {
        if (value === justifyActive) {
            return;
        }

        // setName('');
        setUsername('');
        setEmail('');
        setPassword('');
        setRememberMe(true);
        setError('');
        setJustifyActive(value);
    };

    async function register(event) {
        event.preventDefault();
        setLoadingState(true);

        const credentials = {
            passHash: passHash,
            email: email,
            username: username,
        }
        const search = window.location.search;
        const data = await identityService.register(credentials, search);

        if (data && data.isOk) {
          window.location = data.redirectUrl;
        }
        else {
            setError(data?.error ?? 'Something went wrong');
            setLoadingState(false);
        }
    }

    async function login(event) {
        event.preventDefault();
        setLoadingState(true);

        const credentials = {
            passHash: passHash,
            email: email,
            rememberMe: rememberMe,
        }
        const search = window.location.search;
        const data = await identityService.login(credentials, search);

        if (data && data.isOk) {
            window.location = data.redirectUrl;
        }
        else {
            setError('Wrong email or password');
            setLoadingState(false);
        }
    }

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
        mgr.getUser()
            .then(user => {
                if (user)
                    navigate("/")
            })
    }, [])
    /* eslint-enable */

    return pageLoadingStage ? '' :
        <div style={{height: "100vh"}} className='d-flex align-items-center'>

            <MDBContainer className="p-3 my-5 d-flex flex-column col-12 col-sm-10 col-md-8 col-lg-6 col-xl-4">

                <MDBTabs pills justify className='mb-3 d-flex flex-row justify-content-between'>
                    <MDBTabsItem>
                        <MDBTabsLink onClick={() => handleJustifyClick('login')} active={justifyActive === 'login'}>
                            {t('login_tab')}
                        </MDBTabsLink>
                    </MDBTabsItem>
                    <MDBTabsItem>
                        <MDBTabsLink onClick={() => handleJustifyClick('register')} active={justifyActive === 'register'}>
                            {t('register_tab')}
                        </MDBTabsLink>
                    </MDBTabsItem>
                </MDBTabs>

                <MDBTabsContent>

                    <MDBTabsPane show={justifyActive === 'login'}>

                        <div className="text-center mb-3">
                            <p>{t('signin_label')}</p>
                            <div className='d-flex justify-content-between mx-auto' style={{ width: '40%' }}>
                                <MDBBtn tag='a' color='none' className='p-2' style={{ color: '#1266f1' }}>
                                    <MDBIcon fab icon='facebook-f' size="sm" />
                                </MDBBtn>

                                <MDBBtn tag='a' color='none' className='p-2' style={{ color: '#1266f1' }}>
                                    <MDBIcon fab icon='twitter' size="sm" />
                                </MDBBtn>

                                <MDBBtn tag='a' color='none' className='p-2' style={{ color: '#1266f1' }}>
                                    <MDBIcon fab icon='google' size="sm" />
                                </MDBBtn>

                                <MDBBtn tag='a' color='none' className='p-2' style={{ color: '#1266f1' }}>
                                    <MDBIcon fab icon='github' size="sm" />
                                </MDBBtn>
                            </div>

                            <p className="text-center mt-3">{t('or_label')}</p>
                        </div>

                        <form onSubmit={login}>
                            <MDBInput 
                                wrapperClass='mb-4' 
                                label={t('email_label')}
                                type='email' 
                                required
                                value={email}
                                onChange={(event) => setEmail(event.target.value) }/>
                            <MDBInput 
                                wrapperClass='mb-4' 
                                label={t('password_label')}
                                type='password'
                                required
                                value={password}
                                onChange={(event) => {
                                    const generatedPassHash = CryptoJS.SHA3(event.target.value, { outputLength: 256 }).toString(CryptoJS.enc.Hex)
                                    setPassword(event.target.value)
                                    setPassHash(generatedPassHash);
                                }}/>

                            <div className="d-flex justify-content-between mx-4 mb-4">
                                <MDBCheckbox  
                                    value=''
                                    checked={rememberMe}
                                    onChange={(event) => setRememberMe(event.target.checked)}
                                    label={t('remember_me')} />
                            </div>

                            {error ? <span className="text-danger w-100">{error}</span> : ''}

                            <MDBBtn disabled={loadingState} className="mb-4 w-100" type="submit">
                                {loadingState && <MDBSpinner className='me-2' size='sm'></MDBSpinner>}
                                <span>{t('signin_btn')}</span>
                            </MDBBtn>
                        </form>
                    </MDBTabsPane>

                    <MDBTabsPane show={justifyActive === 'register'}>

                        <div className="text-center mb-3">
                            <p>{t('signup_label')}</p>

                            <div className='d-flex justify-content-between mx-auto' style={{ width: '40%' }}>
                                <MDBBtn tag='a' color='none' className='p-2' style={{ color: '#1266f1' }}>
                                    <MDBIcon fab icon='facebook-f' size="sm" />
                                </MDBBtn>

                                <MDBBtn tag='a' color='none' className='p-2' style={{ color: '#1266f1' }}>
                                    <MDBIcon fab icon='twitter' size="sm" />
                                </MDBBtn>

                                <MDBBtn tag='a' color='none' className='p-2' style={{ color: '#1266f1' }}>
                                    <MDBIcon fab icon='google' size="sm" />
                                </MDBBtn>

                                <MDBBtn tag='a' color='none' className='p-2' style={{ color: '#1266f1' }}>
                                    <MDBIcon fab icon='github' size="sm" />
                                </MDBBtn>
                            </div>

                            <p className="text-center mt-3">{t('or_label')}</p>
                        </div>

                        <form onSubmit={register}>
                            {/* <MDBInput 
                                wrapperClass='mb-4' 
                                label='Name' 
                                type='text' 
                                value={name}
                                onChange={(event) => setName(event.target.value) }/> */}
                            <MDBInput 
                                wrapperClass='mb-4' 
                                label={t('username_label')}
                                type='text' 
                                required
                                value={username}
                                onChange={(event) => setUsername(event.target.value) }/>
                            <MDBInput 
                                wrapperClass='mb-4' 
                                label={t('email_label')}
                                type='email'
                                required
                                value={email}
                                onChange={(event) => setEmail(event.target.value) }/>
                            <MDBInput 
                                wrapperClass='mb-4' 
                                label={t('password_label')}
                                type='password' 
                                required
                                value={password}
                                onChange={(event) => {
                                    const generatedPassHash = CryptoJS.SHA3(event.target.value, { outputLength: 256 }).toString(CryptoJS.enc.Hex)
                                    setPassword(event.target.value)
                                    setPassHash(generatedPassHash);
                                }}/>

                            {error ? <span className="text-danger w-100">{error}</span> : ''}

                            <MDBBtn disabled={loadingState} className="mb-4 w-100" type="submit">
                                {loadingState && <MDBSpinner className='me-2' size='sm'></MDBSpinner>}
                                <span>{t('signup_btn')}</span>
                            </MDBBtn>
                        </form>
                    </MDBTabsPane>

                </MDBTabsContent>

            </MDBContainer>
        </div>
}