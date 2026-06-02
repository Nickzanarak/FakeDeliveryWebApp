import { Component, OnInit, OnDestroy, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { OrderService } from '../../domain/services/order.service';
import { CartService, CartItem } from '../../domain/services/cart.service';
import { AuthService } from '../../domain/services/auth.service';
import { interval, Subscription } from 'rxjs';
import { switchMap } from 'rxjs/operators';

@Component({
    selector: 'app-cart',
    imports: [CommonModule],
    templateUrl: './cart.html',
    styleUrl: './cart.css'
})
export class Cart implements OnInit, OnDestroy {
    cartItems: CartItem[] = [];
    totalPrice = 0;
    private pollingSub?: Subscription;

    private orderService = inject(OrderService);
    private cartService = inject(CartService);
    private authService = inject(AuthService);
    private router = inject(Router);

    ngOnInit() {
        if (this.authService.isLoggedIn()) {
            this.cartService.loadCart().subscribe();

            // Polling ทุก 5 วินาที
            this.pollingSub = interval(5000).pipe(
                switchMap(() => this.cartService.loadCart())
            ).subscribe();
        }

        this.cartService.cart$.subscribe(cart => {
            this.cartItems = cart.items;
            this.totalPrice = cart.totalPrice;
        });
    }

    ngOnDestroy() {
        this.pollingSub?.unsubscribe();
    }

    increase(item: CartItem) {
        this.cartService.updateItem(item.id, item.quantity + 1).subscribe({
            error: (err) => console.error(err)
        });
    }

    decrease(item: CartItem) {
        if (item.quantity > 1) {
            this.cartService.updateItem(item.id, item.quantity - 1).subscribe({
                error: (err) => console.error(err)
            });
        } else {
            this.cartService.removeItem(item.id).subscribe({
                error: (err) => console.error(err)
            });
        }
    }

    checkout() {
        const items = this.cartItems.map(i => ({ productId: i.productId, quantity: i.quantity }));
        this.orderService.create(items).subscribe({
            next: () => {
                this.cartService.clearCart().subscribe();
                this.router.navigate(['/products']);
            },
            error: () => alert('เกิดข้อผิดพลาด กรุณาลองใหม่อีกครั้ง')
        });
    }

    goBack() {
        this.router.navigate(['/products']);
    }
}