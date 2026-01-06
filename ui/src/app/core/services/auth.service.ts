import { computed, Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { Router } from '@angular/router';

interface AuthState {
  token: string | null;
  role: string | null;
}

@Injectable({ providedIn: 'root' })
export class AuthService {

  private baseUrl = 'https://localhost:7200/api/Auth';

  // ✅ EXISTING SUBJECT (KEEP)
  private roleSubject = new BehaviorSubject<string | null>(null);

  // ✅ EXISTING SIGNAL STATE (KEEP)
  private authState = signal<AuthState>({
    token: localStorage.getItem('token'),
    role: localStorage.getItem('role')
  });

  // ✅ EXISTING COMPUTED (KEEP)
  isAuthenticated = computed(() => !!this.authState().token);
  isAdmin = computed(() => this.authState().role === 'Admin');

  constructor(
    private http: HttpClient,
    private router: Router
  ) {
    // 🔒 Restore on refresh (KEEP + SYNC)
    const token = localStorage.getItem('token');
    const role = localStorage.getItem('role');

    if (token && role) {
      this.authState.set({ token, role });
      this.roleSubject.next(role);
    }
  }

  // ================= AUTH API =================

  login(payload: { Username: string; Password: string }) {
    return this.http.post<any>(`${this.baseUrl}/login`, payload);
  }

  register(payload: any): Observable<string> {
    return this.http.post<string>(
      `${this.baseUrl}/register`,
      payload,
      { responseType: 'text' as 'json' }
    );
  }

  // ================= STATE SET =================

  setAuth(token: string, role: string) {
    localStorage.setItem('token', token);
    localStorage.setItem('role', role);

    // 🔥 UPDATE BOTH STORES
    this.authState.set({ token, role });
    this.roleSubject.next(role);
  }

  // ================= NEW HELPERS (ADDED) =================

  /** Used by guards */
  isLoggedIn(): boolean {
    return !!this.authState().token;
  }

  /** Used by guards */
  hasRole(role: string): boolean {
    return this.authState().role === role;
  }

  /** Optional: current role */
  getRole(): string | null {
    return this.authState().role;
  }

  /** Optional: current token */
  getToken(): string | null {
    return this.authState().token;
  }

  /** 🔥 Role-based redirect after login */
  redirectByRole(role: string) {
    switch (role) {
      case 'Admin':
        this.router.navigate(['/admin/dashboard']);
        break;
      case 'Customer':
        this.router.navigate(['/customer/dashboard']);
        break;
      case 'Agent':
        this.router.navigate(['/agent/dashboard']);
        break;
      case 'ClaimsOfficer':
        this.router.navigate(['/claims/dashboard']);
        break;
      case 'HospitalProvider':
        this.router.navigate(['/hospital/dashboard']);
        break;
      default:
        this.router.navigate(['/auth/login']);
    }
  }

  // ================= ADMIN FUNCTIONS (KEEP) =================

  getAllUsers() {
    return this.http.get<any[]>(`${this.baseUrl}/users`);
  }

  approveUser(userId: string) {
    return this.http.put(`${this.baseUrl}/users/${userId}/approve`, {});
  }

  assignRole(userId: string, role: string) {
    return this.http.put(
      `${this.baseUrl}/users/${userId}/roles`,
      [role]
    );
  }

  // ================= LOGOUT (EXTENDED) =================

  logout() {
    localStorage.clear();
    this.roleSubject.next(null);
    this.authState.set({ token: null, role: null });

    // 🔒 Safe redirect
    this.router.navigate(['/auth/login']);
  }
}
