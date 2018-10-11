import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from "@angular/common/http";
import { Store } from "@ngxs/store";
import { Observable } from "rxjs";
import { Injectable, Injector, Inject } from '@angular/core';

@Injectable()
export class CustomHttpInterceptor implements HttpInterceptor{
    constructor(private store: Store){}

    intercept(
        request: HttpRequest<any>,
        next: HttpHandler
    ): Observable<HttpEvent<any>> {

        const token = this.store.selectSnapshot(state => state.auth.token);
        const clonedRequest = request.clone({
            headers: request.headers.set('Authorization', 'Bearer ' + (token ? token : ''))
        });
        return next.handle(clonedRequest);
    }
}