import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { Product } from '../../models/product.model';

@Injectable({
    providedIn: 'root'
})
export class ProductService {
    constructor(private http: HttpClient) {}
    readonly baseURL = 'http://localhost:5231/api/Products';

    getAll() {
        return this.http.get<Product[]>(this.baseURL)
            .pipe(catchError(this.errorHandler));
    }

    getById(id: number) {
        return this.http.get<Product>(`${this.baseURL}/${id}`)
            .pipe(catchError(this.errorHandler));
    }

    create(product: Omit<Product, 'id' | 'imageUrl'> & { fileId?: number }) {
        return this.http.post<Product>(this.baseURL, product)
            .pipe(catchError(this.errorHandler));
    }

    update(id: number, product: Omit<Product, 'id' | 'imageUrl'> & { fileId?: number }) {
        return this.http.put<Product>(`${this.baseURL}/${id}`, product)
            .pipe(catchError(this.errorHandler));
    }

    delete(id: number) {
        return this.http.delete(`${this.baseURL}/${id}`)
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