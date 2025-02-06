export interface User {
    id: string;
    userName?: string;
    firstName?: string;
    lastName?: string;
    email: string;
    createdByAdmin: boolean;
    deletionRequestedAt?: string;
    createdAt?: string;
    updatedAt?: string;
    roles: Array<string>;
    emailConfirmed: boolean;
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

export interface RolePermission extends Permission {
    roleIds: Array<string>;
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


export interface UserSettings {
    id: string;
    userName: string
    email: string;
    createdByAdmin: boolean;
    roles: Array<{id: string; name: string}>;
    permissions: Array<{id: string; name: string, description?: string, roleId: string}>;
}

export interface CurrentUser extends UserSettings {
    isAuthenticated: boolean;
}