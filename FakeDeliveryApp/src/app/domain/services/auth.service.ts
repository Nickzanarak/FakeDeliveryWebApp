import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { BehaviorSubject, catchError, tap, throwError } from 'rxjs';
import { User } from '../../models/user.model';

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    private http = inject(HttpClient);
    readonly baseURL = 'http://localhost:5231/api/Auth';

    private userSubject = new BehaviorSubject<User | null>(this.getUser());
    user$ = this.userSubject.asObservable();

    login(username: string, password: string) {
        return this.http.post<User>(`${this.baseURL}/login`, { username, password }).pipe(
            tap(user => {
                this.saveUser(user);
                this.userSubject.next(user);
            }),
            catchError(this.errorHandler)
        );
    }

    register(username: string, password: string, name: string, role: string = 'user') {
        return this.http.post<User>(`${this.baseURL}/register`, { username, password, name, role })
            .pipe(catchError(this.errorHandler));
    }

    saveUser(user: User) {
        localStorage.setItem('user', JSON.stringify(user));
    }

    getUser(): User | null {
        const user = localStorage.getItem('user');
        return user ? JSON.parse(user) : null;
    }

    getToken(): string | null {
        return this.getUser()?.token ?? null;
    }

    isLoggedIn(): boolean {
        return this.getUser() !== null;
    }

    isAdmin(): boolean {
        return this.getUser()?.role === 'admin';
    }

    logout() {
        localStorage.removeItem('user');
        this.userSubject.next(null);
    }

    errorHandler(error: any) {
        let errorMessage = '';
        if (error.error instanceof ErrorEvent) {
            errorMessage = error.error.message;
        } else {
            errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
        }
        return throwError(() => errorMessage);
    }
}