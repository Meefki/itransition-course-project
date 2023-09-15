import React, { useContext, useEffect, useMemo, useState } from "react";
import { useTranslation } from "react-i18next";
import { useNavigate, useParams } from "react-router-dom";
import HrStyle from '../../../Assets/Css/hr';
import { UserManagerContext } from "../../../Contexts/UserManagerContext";
import { UserService } from "../../../Services/UserService";
import { FilterOptionsContext } from "../../../Contexts/FilterOptionsContext";

function UserInfo({ owner = false }) {
    const ns = "user-profile";
    const { t, i18n } = useTranslation(ns);
    const [pageLoadingStage, setPageLoadingStage] = useState(true);
    
    const {id} = useParams();
    const mgr = useContext(UserManagerContext);
    const userService = useMemo(() => new UserService(), []);
    const [userInfo, setUserInfo] = useState({});
    const [isFirstRender, setIsFirstRender] = useState(true);
    const navigate = useNavigate();

    const { setFilterOptions, setValid } = useContext(FilterOptionsContext);

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
        // setValid(false);
        if (owner) {
            mgr.getUser()
            .then((user) => {
                if (user) {
                    userService.getUser(user.profile.sub)
                    .then((userInfo) => {
                        if (userInfo) {
                            setUserInfo(userInfo);
                        }
                    })
                }
            })
        } else {
            if (id) {
                mgr.getUser()
                    .then((user) => { 
                        user && user.profile?.sub.toLowerCase() === id.toLowerCase() && navigate("/profile/me");
                    })
                userService.getUser(id)
                    .then((userInfo) => {
                        if (userInfo) {
                            setUserInfo(userInfo);
                        }
                    })
            }
        }

        setIsFirstRender(false);
    }, [id]);

    useEffect(() => {
        if (isFirstRender) {
            setValid(false);
            return;
        }

        setFilterOptions(current => {
            let filters = Array.isArray(current) ? [...current] : [];
            const obj = filters?.find((o, i) => {
                if (o.name === 'authorUserId') {
                    filters[i] = { name: 'authorUserId', value: userInfo?.id }
                    return true;
                }
                return false;
            })
            if (!obj) {
                filters?.push({ name: 'authorUserId', value: userInfo?.id });
            }
            if (userInfo?.id)
                setValid(true);
            return filters;
        });
    }, [userInfo]);

    useEffect(() => {

        return () => {
            setFilterOptions(current => { 
                let filters =  [...current.filter(f => f.name !== 'authorUserId')]
                return filters;
            });
        }
    }, [])
    /* eslint-enable */    

    return pageLoadingStage ? '' :
        <div className="mx-4 mb-4 d-flex flex-column">
            <h5>{t('user_profile_user_info')}</h5>
            <div className="w-100 d-flex flex-row justify-content-between">
                <span>{t('user_profile_user_info_username')}</span>
                <span>{userInfo.userName}</span>
            </div>
            <div className="w-100 d-flex flex-row justify-content-between">
                <span>{t('user_profile_user_info_name')}</span>
                <span>{userInfo?.name ?? ''}</span>
            </div>
            <div className="w-100 d-flex flex-row justify-content-between">
                <span>{t('user_profile_user_info_email')}</span>
                <span>{userInfo.email}</span>
            </div>
            <div className="w-100 d-flex flex-row justify-content-between">
                <span>{t('user_profile_user_info_role')}</span>
                <span>{userInfo?.role ?? ''}</span>
            </div>
            <div style={HrStyle.horizontalHrStyle}/>
        </div>
}

export default UserInfo;