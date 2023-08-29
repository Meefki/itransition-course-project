import React, { useState } from 'react';
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
  MDBCheckbox
}
from 'mdb-react-ui-kit';
import CryptoJS from 'crypto-js';

export function Login() {

    const [justifyActive, setJustifyActive] = useState('login')
    // const [name, setName] = useState('');
    const [username, setUsername] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [rememberMe, setRememberMe] = useState(true);
    const [passHash, setPassHash] = useState('');

    function handleJustifyClick(value) {
        if (value === justifyActive) {
            return;
        }

        // setName('');
        setUsername('');
        setEmail('');
        setPassword('');
        setRememberMe(true);
        setJustifyActive(value);
    };

    async function register(event) {
        event.preventDefault(); // just because of MDB
        const response = await fetch(process.env.REACT_APP_IDENTITY_AUTHENTICATE_URL + "/register" + window.location.search, {
            method: 'POST',
            headers: { 
                'Content-Type' : 'application/json'
            },
            credentials: 'include',
            body: JSON.stringify({
                // name,
                username,
                passwordHash: passHash,
                email,
            })
        })

        const data = await response.json();

        if (data && data.isOk) {
          window.location = data.redirectUrl;
        }
    }

    async function login(event) {
        event.preventDefault();
        const url = process.env.REACT_APP_IDENTITY_AUTHENTICATE_URL + "/login" + window.location.search;
        const response = await fetch(url, {
            method: 'POST',
            headers: { 
                'Content-Type' : 'application/json'
            },
            credentials: 'include',
            body: JSON.stringify({
                passwordHash: passHash,
                email,
                rememberMe,
            })
        })

        const data = await response.json();

        if (data && data.isOk) {
          window.location = data.redirectUrl;
        }
    }

    return (
        <div style={{height: "100vh"}} className='d-flex align-items-center'>
            <MDBContainer className="p-3 my-5 d-flex flex-column col-12 col-sm-10 col-md-8 col-lg-6 col-xl-4">

                <MDBTabs pills justify className='mb-3 d-flex flex-row justify-content-between'>
                    <MDBTabsItem>
                        <MDBTabsLink onClick={() => handleJustifyClick('login')} active={justifyActive === 'login'}>
                            Login
                        </MDBTabsLink>
                    </MDBTabsItem>
                    <MDBTabsItem>
                        <MDBTabsLink onClick={() => handleJustifyClick('register')} active={justifyActive === 'register'}>
                            Register
                        </MDBTabsLink>
                    </MDBTabsItem>
                </MDBTabs>

                <MDBTabsContent>

                    <MDBTabsPane show={justifyActive === 'login'}>

                        <div className="text-center mb-3">
                            <p>Sign in with:</p>

                            <div className='d-flex justify-content-between mx-auto' style={{ width: '40%' }}>
                                <MDBBtn tag='a' color='none' className='m-1' style={{ color: '#1266f1' }}>
                                    <MDBIcon fab icon='facebook-f' size="sm" />
                                </MDBBtn>

                                <MDBBtn tag='a' color='none' className='m-1' style={{ color: '#1266f1' }}>
                                    <MDBIcon fab icon='twitter' size="sm" />
                                </MDBBtn>

                                <MDBBtn tag='a' color='none' className='m-1' style={{ color: '#1266f1' }}>
                                    <MDBIcon fab icon='google' size="sm" />
                                </MDBBtn>

                                <MDBBtn tag='a' color='none' className='m-1' style={{ color: '#1266f1' }}>
                                    <MDBIcon fab icon='github' size="sm" />
                                </MDBBtn>
                            </div>

                            <p className="text-center mt-3">or:</p>
                        </div>

                        <form onSubmit={login}>
                            <MDBInput 
                                wrapperClass='mb-4' 
                                label='Email address'
                                type='email' 
                                required
                                value={email}
                                onChange={(event) => setEmail(event.target.value) }/>
                            <MDBInput 
                                wrapperClass='mb-4' 
                                label='Password' 
                                type='password' 
                                required
                                value={password}
                                onChange={(event) => {
                                    let passHash = CryptoJS.SHA3(event.target.value, { outputLength: 256 }).toString(CryptoJS.enc.Hex)
                                    setPassword(event.target.value)
                                    setPassHash(passHash);
                                }}/>

                            <div className="d-flex justify-content-between mx-4 mb-4">
                                <MDBCheckbox 
                                    name='flexCheck' 
                                    value=''
                                    checked={rememberMe}
                                    onChange={(event) => setRememberMe(event.target.checked)}
                                    id='flexCheckDefault' 
                                    label='Remember me' />
                            </div>

                            <MDBBtn className="mb-4 w-100" type="submit">Sign in</MDBBtn>
                        </form>
                    </MDBTabsPane>

                    <MDBTabsPane show={justifyActive === 'register'}>

                        <div className="text-center mb-3">
                            <p>Sign up with:</p>

                            <div className='d-flex justify-content-between mx-auto' style={{ width: '40%' }}>
                                <MDBBtn tag='a' color='none' className='m-1' style={{ color: '#1266f1' }}>
                                    <MDBIcon fab icon='facebook-f' size="sm" />
                                </MDBBtn>

                                <MDBBtn tag='a' color='none' className='m-1' style={{ color: '#1266f1' }}>
                                    <MDBIcon fab icon='twitter' size="sm" />
                                </MDBBtn>

                                <MDBBtn tag='a' color='none' className='m-1' style={{ color: '#1266f1' }}>
                                    <MDBIcon fab icon='google' size="sm" />
                                </MDBBtn>

                                <MDBBtn tag='a' color='none' className='m-1' style={{ color: '#1266f1' }}>
                                    <MDBIcon fab icon='github' size="sm" />
                                </MDBBtn>
                            </div>

                            <p className="text-center mt-3">or:</p>
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
                                label='Username' 
                                type='text' 
                                required
                                value={username}
                                onChange={(event) => setUsername(event.target.value) }/>
                            <MDBInput 
                                wrapperClass='mb-4' 
                                label='Email' 
                                type='email'
                                required
                                value={email}
                                onChange={(event) => setEmail(event.target.value) }/>
                            <MDBInput 
                                wrapperClass='mb-4' 
                                label='Password' 
                                type='password' 
                                required
                                value={password}
                                onChange={(event) => {
                                    let passHash = CryptoJS.SHA3(event.target.value, { outputLength: 256 }).toString(CryptoJS.enc.Hex)
                                    setPassword(event.target.value)
                                    setPassHash(passHash);
                                }}/>

                            <MDBBtn className="mb-4 w-100" type="submit">Sign up</MDBBtn>
                        </form>
                    </MDBTabsPane>

                </MDBTabsContent>

            </MDBContainer>
        </div>
    );
}