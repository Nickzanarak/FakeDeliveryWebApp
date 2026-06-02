import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, tap, catchError, throwError } from 'rxjs';

export interface CartItem {
    id: number;
    productId: number;
    productName: string;
    price: number;
    imageUrl?: string;
    quantity: number;
    subtotal: number;
}

export interface CartResponse {
    id: number;
    items: CartItem[];
    totalPrice: number;
}

@Injectable({
    providedIn: 'root'
})
export class CartService {
    private http = inject(HttpClient);
    readonly baseURL = 'http://localhost:5231/api/Cart';

    private cartSubject = new BehaviorSubject<CartResponse>({ id: 0, items: [], totalPrice: 0 });
    cart$ = this.cartSubject.asObservable();

    loadCart() {
        return this.http.get<CartResponse>(this.baseURL).pipe(
            tap(cart => this.cartSubject.next(cart)),
            catchError(this.errorHandler)
        );
    }

    addToCart(productId: number, quantity: number = 1) {
        return this.http.post<CartResponse>(`${this.baseURL}/add`, { productId, quantity }).pipe(
            tap(cart => this.cartSubject.next(cart)),
            catchError(this.errorHandler)
        );
    }

    updateItem(cartItemId: number, quantity: number) {
        return this.http.put<CartResponse>(`${this.baseURL}/items/${cartItemId}`, { quantity }).pipe(
            tap(cart => this.cartSubject.next(cart)),
            catchError(this.errorHandler)
        );
    }

    removeItem(cartItemId: number) {
        return this.http.delete<CartResponse>(`${this.baseURL}/items/${cartItemId}`).pipe(
            tap(cart => this.cartSubject.next(cart)),
            catchError(this.errorHandler)
        );
    }

    clearCart() {
        return this.http.delete(`${this.baseURL}/clear`).pipe(
            tap(() => this.cartSubject.next({ id: 0, items: [], totalPrice: 0 })),
            catchError(this.errorHandler)
        );
    }

    resetCart() {
        this.cartSubject.next({ id: 0, items: [], totalPrice: 0 });
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