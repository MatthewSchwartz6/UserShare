import { AuthStateModel, SignIn, SignOut } from "./AuthStateModel";
import { State, Selector, Action, StateContext } from "@ngxs/store";
import { AuthService } from "../Services/auth.service";
import { tap } from "rxjs/operators";
import { LoginRoutingModule } from "../Components/login/login-routing.module";

@State<AuthStateModel>({
    name: 'auth'
})

export class AuthState {
    @Selector()
    static token(state: AuthStateModel){
        return state.token;
    }

    @Selector()
    static userDetails(state: AuthStateModel) {
        return {
            profileName: state.profileName,
            guid: state.guid
        };
    }

    // @Selector()
    // static refreshToken(state: AuthStateModel){
    //     return state.refreshToken;
    // }
    constructor(private authService: AuthService){}

    @Action(SignIn)
    SignIn(
        {patchState}: StateContext<AuthStateModel>, 
        {profileName, password}: SignIn ) {
        return this.authService.SignIn(profileName,password).pipe(
            tap(result => {
                patchState({
                    token: result.token,
                    profileName: result.profileName,
                    guid: result.guid
                });
            })
        );
    }

    @Action(SignOut)
    SignOut(
        {setState, getState}: StateContext<AuthStateModel>){
        const { token } = getState();
        
        return this.authService.Signout().pipe(
            tap(_ => {
                setState({});
            })
        );
    }
    

}