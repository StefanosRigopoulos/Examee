import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { AccountService } from '../services/account.service';

export const AdminGuard: CanActivateFn = () => {
  const accountService = inject(AccountService);

  if (accountService.role().includes('Admin')) {
    return true;
  } else {
    return false;
  }
};