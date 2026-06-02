import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../domain/services/auth.service';

@Component({
    selector: 'app-register',
    imports: [CommonModule, FormsModule, RouterLink],
    templateUrl: './register.html',
    styleUrl: './register.css'
})
export class Register {
    username = '';
    name = '';
    password = '';
    confirmPassword = '';
    errorMessage = '';

    private authService = inject(AuthService);
    private router = inject(Router);

    register() {
        if (!this.username || !this.name || !this.password) {
            this.errorMessage = 'กรุณากรอกข้อมูลให้ครบ';
            return;
        }

        if (this.password !== this.confirmPassword) {
            this.errorMessage = 'รหัสผ่านไม่ตรงกัน';
            return;
        }

        this.authService.register(this.username, this.password, this.name,'user').subscribe({
            next: (user) => {
                this.authService.saveUser(user);
                this.router.navigate(['/products']);
            },
            error: () => {
                this.errorMessage = 'Username นี้มีอยู่แล้ว';
            }
        });
    }
}