import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ProductService } from '../../domain/services/product.service';
import { FileService } from '../../domain/services/file.service';

@Component({
    selector: 'app-add-product',
    imports: [CommonModule, FormsModule],
    templateUrl: './add-product.html',
    styleUrl: './add-product.css'
})
export class AddProduct {
    name = '';
    description = '';
    price = 0;
    selectedFile: File | null = null;
    previewUrl: string | null = null;
    errorMessage = '';

    private productService = inject(ProductService);
    private fileService = inject(FileService);
    private router = inject(Router);

    onFileSelected(event: any) {
        const file = event.target.files[0];
        if (!file) return;
        this.selectedFile = file;
        const reader = new FileReader();
        reader.onload = (e) => this.previewUrl = e.target?.result as string;
        reader.readAsDataURL(file);
    }

    submit() {
        if (!this.name || !this.price) {
            this.errorMessage = 'กรุณากรอกชื่อสินค้าและราคา';
            return;
        }

        if (this.selectedFile) {
            this.fileService.upload(this.selectedFile).subscribe({
                next: (res) => this.createProduct(res.id),
                error: () => this.errorMessage = 'อัพโหลดรูปไม่สำเร็จ'
            });
        } else {
            this.createProduct(undefined);
        }
    }

    goBack() {
        this.router.navigate(['/products']);
    }

    createProduct(fileId?: number) {
        this.productService.create({
            name: this.name,
            description: this.description,
            price: this.price,
            fileId
        }).subscribe({
            next: () => this.router.navigate(['/products']),
            error: () => this.errorMessage = 'สร้างสินค้าไม่สำเร็จ'
        });
    }
}