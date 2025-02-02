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
    createdByAdmin: boolean;
    deletionRequestedAt?: string;
}

export interface PageInfo {
    currentPage: number;
    pageSize: number;
    totalCount: number;
    hasNextPage: boolean;
    hasPreviousPage: boolean;
}

export interface PaginatedResponse<T> extends PageInfo{
    items: T[];
}