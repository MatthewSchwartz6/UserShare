export class AuthStateModel {
    token?: string;
    profileName?: string;
    guid?: string;
}
export class SignIn{
    static readonly type = '[Auth] SignIn';
    constructor(private profileName: string, private password: string){}
}
export class SignOut{
    static readonly type = '[Auth] SignOut';
}