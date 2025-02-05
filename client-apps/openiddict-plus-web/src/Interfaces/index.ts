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
    createdAt?: string;
    updatedAt?: string;
}

export interface Role {
    roleId: string;
    roleName: string;
    createdAt?: string;
    updatedAt?: string;
    permissions: Array<Permission>;
}

export interface Permission {
    permissionId: string;
    name: string;
    description?: string;
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