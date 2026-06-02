import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, throwError } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class FileService {
    constructor(private http: HttpClient) {}
    readonly baseURL = 'http://localhost:5231/api/Files';

    upload(file: File) {
        const formData = new FormData();
        formData.append('file', file);
        return this.http.post<{ id: number }>(`${this.baseURL}/upload`, formData)
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