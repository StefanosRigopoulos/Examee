import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { AccountService } from '../services/account.service';
import { map } from 'rxjs/operators';

export const AuthGuard: CanActivateFn = () => {
  const accountService = inject(AccountService);

  return accountService.currentUser$.pipe(
    map(user => {
      if (user) {
        return true;
      } else {
        return false;
      }
    })
  );
};