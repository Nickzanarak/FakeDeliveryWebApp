import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';

export interface UserResponse {
    id: number;
    username: string;
    name: string;
    role: string;
}

@Injectable({
    providedIn: 'root'
})
export class UserService {
    private http = inject(HttpClient);
    readonly baseURL = 'http://localhost:5231/api/Users';

    getAll() {
        return this.http.get<UserResponse[]>(this.baseURL)
            .pipe(catchError(this.errorHandler));
    }

    updateRole(id: number, role: string) {
        return this.http.put<UserResponse>(`${this.baseURL}/${id}/role`, { role })
            .pipe(catchError(this.errorHandler));
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