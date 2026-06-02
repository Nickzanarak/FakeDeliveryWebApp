import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ProductService } from '../../domain/services/product.service';
import { FileService } from '../../domain/services/file.service';
import { Product } from '../../models/product.model';

@Component({
    selector: 'app-manage-product',
    imports: [CommonModule, FormsModule],
    templateUrl: './manage-product.html',
    styleUrl: './manage-product.css'
})
export class ManageProduct implements OnInit {
    products: Product[] = [];
    selectedProduct: Product | null = null;
    editName = '';
    editDescription = '';
    editPrice = 0;
    errorMessage = '';
    selectedFile: File | null = null;
    previewUrl: string | null = null;

    private productService = inject(ProductService);
    private fileService = inject(FileService);
    private router = inject(Router);

    ngOnInit() {
        this.loadProducts();
    }

    loadProducts() {
        this.productService.getAll().subscribe({
            next: (data) => this.products = data,
            error: (err) => console.error(err)
        });
    }

    editProduct(product: Product) {
        this.selectedProduct = product;
        this.editName = product.name;
        this.editDescription = product.description ?? '';
        this.editPrice = product.price;
        this.errorMessage = '';
        this.selectedFile = null;
        this.previewUrl = null;
    }

    cancelEdit() {
        this.selectedProduct = null;
        this.errorMessage = '';
        this.selectedFile = null;
        this.previewUrl = null;
    }

    onFileSelected(event: any) {
        const file = event.target.files[0];
        if (!file) return;
        this.selectedFile = file;
        const reader = new FileReader();
        reader.onload = (e) => this.previewUrl = e.target?.result as string;
        reader.readAsDataURL(file);
    }

    saveEdit() {
        if (!this.editName || !this.editPrice) {
            this.errorMessage = 'กรุณากรอกชื่อสินค้าและราคา';
            return;
        }

        if (this.selectedFile) {
            this.fileService.upload(this.selectedFile).subscribe({
                next: (res) => this.updateProduct(res.id),
                error: () => this.errorMessage = 'อัพโหลดรูปไม่สำเร็จ'
            });
        } else {
            this.updateProduct(this.selectedProduct!.fileId ?? undefined);
        }
    }

    updateProduct(fileId?: number) {
        this.productService.update(this.selectedProduct!.id, {
            name: this.editName,
            description: this.editDescription,
            price: this.editPrice,
            fileId: fileId
        }).subscribe({
            next: (updated) => {
                const index = this.products.findIndex(p => p.id === updated.id);
                if (index !== -1) this.products[index] = updated;
                this.selectedProduct = null;
                this.previewUrl = null;
                this.selectedFile = null;
            },
            error: () => this.errorMessage = 'แก้ไขสินค้าไม่สำเร็จ'
        });
    }

    deleteProduct(product: Product) {
        if (!confirm(`ยืนยันการลบ "${product.name}"?`)) return;
        this.productService.delete(product.id).subscribe({
            next: () => {
                this.products = this.products.filter(p => p.id !== product.id);
            },
            error: () => alert('ลบสินค้าไม่สำเร็จ')
        });
    }

    goBack() {
        this.router.navigate(['/products']);
    }
}