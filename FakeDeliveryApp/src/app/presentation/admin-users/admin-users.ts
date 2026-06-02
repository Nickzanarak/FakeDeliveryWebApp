import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { UserService, UserResponse } from '../../domain/services/user.service';
import { AuthService } from '../../domain/services/auth.service';

@Component({
    selector: 'app-admin-users',
    imports: [CommonModule],
    templateUrl: './admin-users.html',
    styleUrl: './admin-users.css'
})
export class AdminUsers implements OnInit {
    users: UserResponse[] = [];
    currentUserId = 0;
    adminCount = 0;
    userCount = 0;

    private userService = inject(UserService);
    private authService = inject(AuthService);
    private router = inject(Router);

    ngOnInit() {
        this.currentUserId = this.authService.getUser()?.id ?? 0;
        this.loadUsers();
    }

    loadUsers() {
        this.userService.getAll().subscribe({
            next: (data) => {
                this.users = data;
                this.adminCount = data.filter(u => u.role === 'admin').length;
                this.userCount = data.filter(u => u.role === 'user').length;
            },
            error: (err) => console.error(err)
        });
    }

    changeRole(user: UserResponse, role: string) {
        this.userService.updateRole(user.id, role).subscribe({
            next: (updated) => {
                user.role = updated.role;
                this.adminCount = this.users.filter(u => u.role === 'admin').length;
                this.userCount = this.users.filter(u => u.role === 'user').length;
            },
            error: (err) => console.error(err)
        });
    }

    goBack() {
        this.router.navigate(['/products']);
    }
}