import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../domain/services/auth.service';

@Component({
    selector: 'app-login',
    imports: [CommonModule, FormsModule, RouterLink],
    templateUrl: './login.html',
    styleUrl: './login.css'
})
export class Login {
    username = '';
    password = '';
    errorMessage = '';

    private authService = inject(AuthService);
    private router = inject(Router);

    login() {
        this.authService.login(this.username, this.password).subscribe({
            next: (user) => {
                this.authService.saveUser(user);
                this.router.navigate(['/products']);
            },
            error: () => {
                this.errorMessage = 'Username หรือ Password ไม่ถูกต้อง';
            }
        });
    }
}