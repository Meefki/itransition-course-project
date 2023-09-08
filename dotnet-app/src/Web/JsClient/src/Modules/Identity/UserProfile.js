import React, { useContext, useEffect, useMemo, useState } from "react";
import { useTranslation } from "react-i18next";
import { UserManagerContext } from '../../Contexts/UserManagerContext'
import { UserService } from "../../Services/UserService";
import { useParams } from "react-router-dom";
import { ReviewingService } from "../../Services/ReviewingService";
import ReviewList from "../Reviews/ReviewList";

function UserProfile() {

    const {id} = useParams();
    const [userId, setUserId] = useState(id);

    const mgr = useContext(UserManagerContext);
    const userService = useMemo(() => new UserService(), []);
    const reviewingService = useMemo(() => new ReviewingService(), []);

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
                    setUserId(user.profile.sub);
                }
            })
        else {
            userService.getUser(id)
                .then((userProfile) => {
                    setUser(userProfile);
                });
            setUserId(id);
        }
    }, [mgr, id, userService])

    useEffect(() => {
        
    }, [user])

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
            <div className="card d-flex flex-column mt-5">
                <div className="card-body d-flex flex-column">
                    <h3>{t('user-info')}</h3>
                    <span>{'id: ' + user.id}</span>
                    <span>{t('username') + ' ' + user.userName}</span>
                    <span>{t('email') + ' ' + user.email}</span>
                </div>
                <div className="ms-4">
                    <h3>{t('reviews')}</h3>
                    <div>Filters</div>
                    <ReviewList filters={[{ name: "authorUserid", value: user.id}]}/>
                </div>
            </div>
        </div>
    
}

export default UserProfile;