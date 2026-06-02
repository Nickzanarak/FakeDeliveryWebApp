export interface OrderItem {
    productId: number;
    productName: string;
    price: number;
    quantity: number;
    subtotal: number;
}

export interface Order {
    id: number;
    status: string;
    createDate: string;
    items: OrderItem[];
    totalPrice: number;
}