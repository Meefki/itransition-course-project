import { MDBBtn, MDBIcon } from "mdb-react-ui-kit";

function ExternalSignButton({loading, login, scheme, icon}) {

    const loginFunc = async (scheme) => {
        if (loading)
            return;

        await login(scheme);
    }

    return(
        <MDBBtn 
            tag='a' 
            color='none'
            className='p-2' 
            style={{ color: '#1266f1' }}
            onClick={() => loginFunc(scheme)}
        >
            <MDBIcon fab icon={icon} size="sm" />
        </MDBBtn>
    )
}

export default ExternalSignButton;