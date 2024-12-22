import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { map } from 'rxjs';
import { AccountService } from '../services/account.service';

export const AdminGuard: CanActivateFn = () => {
  const accountService = inject(AccountService);

  return accountService.currentUser$.pipe(
    map(user => {
      if (!user) {
        return false;
      }
      if (user.role.includes('Admin')) {
        return true;
      } else {
        return false;
      }
    })
  );
};