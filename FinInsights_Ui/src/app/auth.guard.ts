import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const authGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);

  // check token (or login flag)
  const token = localStorage.getItem('token');

  if (token) {
    return true; // ✅ allow
  } else {
    router.navigate(['/']); // 🔁 redirect to login
    return false; // ❌ block
  }
};