import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { Order } from '../../models/order.model';

@Injectable({
    providedIn: 'root'
})
export class OrderService {
    constructor(private http: HttpClient) {}
    readonly baseURL = 'http://localhost:5231/api/Orders';

    create(items: { productId: number, quantity: number }[]) {
        return this.http.post<Order>(this.baseURL, { items })
            .pipe(catchError(this.errorHandler));
    }

    getMyOrders() {
        return this.http.get<Order[]>(`${this.baseURL}/my-orders`)
            .pipe(catchError(this.errorHandler));
    }

    getAll() {
        return this.http.get<Order[]>(this.baseURL)
            .pipe(catchError(this.errorHandler));
    }

    updateStatus(id: number, status: string) {
        return this.http.put<Order>(`${this.baseURL}/${id}/status`, { status })
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