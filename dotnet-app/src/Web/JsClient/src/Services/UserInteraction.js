export class UserInteraction {
    constructor(userManager) {
        this.mgr = userManager;
    }

    login = async () => {
        this.mgr.signinRedirect()

        this.mgr.getUser().then(function (user) {
            if (user)
              return true
        })

        return false
    }

    logout = async () => {
        this.mgr.signoutRedirect();

        this.mgr.getUser().then(function (user) {
            if (user)
              return true
        })

        return false
    }

    isAuthorized = async () => {
        const user = await this.mgr.getUser();
        if (user)
            return true;

        return false;
    }
}