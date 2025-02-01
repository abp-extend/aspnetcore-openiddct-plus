export interface User {
    id: string;
    userName?: string;
    firstName?: string;
    lastName?: string;
    email: string;
    phoneNumber?: string;
    emailConfirmed: boolean;
    phoneNumberConfirmed?: boolean;
    twoFactorEnabled?: boolean;
    lockoutEnabled: boolean;
    accessFailedCount: number;
    lockoutEnd: string;
    created: string;
    updated: string;
}
export interface PaginatedResponse<T> { 
    data: T[];
    currentPage: number;
    pageSize: number;
    totalPages: number;
    hasNextPage: boolean;
    hasPreviousPage: boolean;
}