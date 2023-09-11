import React, { useContext, useEffect, useMemo, useState } from "react";
import { useTranslation } from "react-i18next";
import { useNavigate, useParams } from "react-router-dom";
import HrStyle from '../../../Assets/Css/hr';
import { UserManagerContext } from "../../../Contexts/UserManagerContext";
import { UserService } from "../../../Services/UserService";

function UserInfo({ owner = false }) {
    const ns = "user-profile";
    const { t, i18n } = useTranslation(ns);
    const [pageLoadingStage, setPageLoadingStage] = useState(true);
    
    const {id} = useParams();
    const mgr = useContext(UserManagerContext);
    const userService = useMemo(() => new UserService(), []);
    const [userInfo, setUserInfo] = useState({});
    const navigate = useNavigate();

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
        setUserInfo({});
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
                userService.getUser(id)
                    .then((userInfo) => {
                        if (userInfo) {
                            setUserInfo(userInfo);
                        }
                    })
            }
        }
    }, [id]);
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