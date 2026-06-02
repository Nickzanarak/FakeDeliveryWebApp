import { Routes } from '@angular/router';
import { Login } from './presentation/login/login';
import { ProductList } from './presentation/product-list/product-list';
import { AddProduct } from './presentation/add-product/add-product';
import { Cart } from './presentation/cart/cart';
import { authGuard } from './configuration/auth.guard';
import { adminGuard } from './configuration/admin.guard';
import { Register } from './presentation/register/register';
import { AdminUsers } from './presentation/admin-users/admin-users';
import { ManageProduct } from './presentation/manage-product/manage-product';

export const routes: Routes = [
    { path: '', redirectTo: 'products', pathMatch: 'full' },
    { path: 'login', component: Login },
    { path: 'register', component: Register },
    { path: 'products', component: ProductList },
    { path: 'add-product', component: AddProduct, canActivate: [authGuard, adminGuard] },
    { path: 'manage-product', component: ManageProduct, canActivate: [authGuard, adminGuard] },
    { path: 'cart', component: Cart, canActivate: [authGuard] },
    { path: 'admin-users', component: AdminUsers, canActivate: [authGuard, adminGuard] },
    { path: '**', redirectTo: 'products' }
];