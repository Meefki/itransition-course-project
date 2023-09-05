import React, { useContext, useEffect, useMemo, useState } from "react";
import { useTranslation } from "react-i18next";
import { UserInteraction } from '../../Services/UserInteraction';
import { UserManagerContext } from '../../Contexts/UserManagerContext'
import { UserService } from "../../Services/UserService";

function UserProfile() {

    const mgr = useContext(UserManagerContext);
    const userInteraction = useMemo(() => new UserInteraction(mgr), [mgr]);
    const userService = useMemo(() => new UserService(), []);

    const ns = 'user-profile'
    const { t, i18n} = useTranslation(ns);
    const [transLoading, setTransLoading] = useState(true);

    const [user, setUser] = useState('');

    useEffect(() => {
        mgr.getUser()
            .then((user) => {
                if (user)
                    userService.getUser(user.profile.sub)
                        .then((userProfile) => {
                            setUser(userProfile);
                        })
            })
    }, [mgr])

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
        <div>User page</div>
    
}

export default UserProfile;