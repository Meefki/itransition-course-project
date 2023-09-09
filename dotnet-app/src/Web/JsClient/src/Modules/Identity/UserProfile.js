import React, { useContext, useEffect, useMemo, useState } from "react";
import { useTranslation } from "react-i18next";
import { UserManagerContext } from '../../Contexts/UserManagerContext'
import { UserService } from "../../Services/UserService";
import { useParams } from "react-router-dom";
import ReviewList from "../Reviews/ReviewList";

function UserProfile() {

    const {id} = useParams();

    const mgr = useContext(UserManagerContext);
    const userService = useMemo(() => new UserService(), []);

    const ns = 'user-profile'
    const { t, i18n} = useTranslation(ns);
    const [transLoading, setTransLoading] = useState(true);

    const [user, setUser] = useState({});

    useEffect(() => {
        if (!id)
        mgr.getUser()
            .then((user) => {
                if (user) {
                    userService.getUser(user.profile.sub)
                        .then((userProfile) => {
                            setUser(userProfile);
                        });
                }
            })
        else {
            userService.getUser(id)
                .then((userProfile) => {
                    setUser(userProfile);
                });
        }
    }, [mgr, id, userService])

    /* eslint-disable */
    useMemo(() => {
        i18n.isInitialized &&
        !i18n.hasLoadedNamespace(ns) &&
            i18n.loadNamespaces(ns)
            .then(() => {
                setTransLoading(false);
            });
        i18n.isInitialized &&
        i18n.hasLoadedNamespace(ns) &&
        setTransLoading(false);
    }, [i18n.isInitialized]);
    /* eslint-enable */

    return transLoading ? '' :
        <div className="container">
            <div className="mt-5 d-flex flex-column flex-md-row">
                <div className="col-12 col-md-9 col-xl-10">
                    <div className="card me-0 me-md-3">
                        <div className="card-body d-flex flex-column">
                            <div className="card-title">
                                <h3>{t('user-info')}</h3>
                            </div>
                            <span>{'id: ' + user.id}</span>
                            <span>{t('username') + ' ' + user.userName}</span>
                            <span>{t('email') + ' ' + user.email}</span>
                        </div>
                    </div>
                </div>
                <div className="card mt-3 mt-md-0 col-12 col-md-3 col-xl-2">
                    <div className="card-body d-flex flex-column align-items-start align-items-md-end">
                        <div className="card-title">
                            <h3>{t('actions')}</h3>
                        </div>
                        <span>{t('create_action')}</span>
                        <span>{t('edit_action')}</span>
                        <span>{t('delete_action')}</span>
                    </div>
                </div>
            </div>
            <div className="card mt-3">
                <div className="card-body">    
                    <div className="card-title">
                        <h3>{t('reviews')}</h3>
                    </div>
                    {user.id && <ReviewList sort={null} isSelection={true} filters={{authorUserId: user.id}}/>}
                </div>
            </div>
        </div>
    
}

export default UserProfile;