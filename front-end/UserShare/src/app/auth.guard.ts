import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { Store } from '@ngxs/store';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private store: Store, private router: Router){}
  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): boolean {
      const token = this.store.selectSnapshot(st => st.auth.token);
      if (token) {
        return true;
      } else {
        this.router.navigate(['/login']);
        return false;
      }
  }
}
