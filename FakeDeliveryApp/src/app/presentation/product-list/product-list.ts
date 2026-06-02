import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ProductService } from '../../domain/services/product.service';
import { AuthService } from '../../domain/services/auth.service';
import { CartService } from '../../domain/services/cart.service';
import { Product } from '../../models/product.model';

@Component({
    selector: 'app-product-list',
    imports: [CommonModule, FormsModule],
    templateUrl: './product-list.html',
    styleUrl: './product-list.css'
})
export class ProductList implements OnInit {
    products: Product[] = [];
    filteredProducts: Product[] = [];
    isAdmin = false;
    searchText = '';

    private productService = inject(ProductService);
    private authService = inject(AuthService);
    private cartService = inject(CartService);
    private router = inject(Router);

    ngOnInit() {
        this.isAdmin = this.authService.isAdmin();
        this.loadProducts();
        if (this.authService.isLoggedIn()) {
            this.cartService.loadCart().subscribe();
        }
    }

    loadProducts() {
        this.productService.getAll().subscribe({
            next: (data) => {
                this.products = data;
                this.filteredProducts = data;
            },
            error: (err) => console.error(err)
        });
    }

    ngDoCheck() {
        this.filteredProducts = this.products.filter(p =>
            p.name.toLowerCase().includes(this.searchText.toLowerCase()) ||
            (p.description ?? '').toLowerCase().includes(this.searchText.toLowerCase())
        );
    }

    addToCart(product: Product) {
        this.cartService.addToCart(product.id).subscribe({
            error: (err) => console.error(err)
        });
    }

    goToAddProduct() {
        this.router.navigate(['/add-product']);
    }

    goToManageProduct() {
        this.router.navigate(['/manage-product']);
    }
}