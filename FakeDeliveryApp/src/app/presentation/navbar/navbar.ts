import { Component, OnInit, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../domain/services/auth.service';
import { CartService } from '../../domain/services/cart.service';

@Component({
    selector: 'app-navbar',
    imports: [CommonModule, RouterLink],
    templateUrl: './navbar.html',
    styleUrl: './navbar.css'
})
export class Navbar implements OnInit {
    isLoggedIn = false;
    isAdmin = false;
    cartCount = 0;

    private authService = inject(AuthService);
    private cartService = inject(CartService);
    private router = inject(Router);

    ngOnInit() {
        this.authService.user$.subscribe(user => {
            this.isLoggedIn = user !== null;
            this.isAdmin = user?.role === 'admin';
            if (user) {
                this.cartService.loadCart().subscribe();
            } else {
                this.cartService.resetCart();
            }
        });

        this.cartService.cart$.subscribe(cart => {
            this.cartCount = cart.items.reduce((sum, i) => sum + i.quantity, 0);
        });
    }

    logout() {
        this.authService.logout();
        this.router.navigate(['/login']);
    }

    goToLogin() {
        this.router.navigate(['/login']);
    }
}