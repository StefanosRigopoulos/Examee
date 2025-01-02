import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { AccountService } from '../services/account.service';

export const AuthGuard: CanActivateFn = () => {
  const accountService = inject(AccountService);

  if (accountService.currentUser()) {
    return true;
  } else {
    return false;
  }
};