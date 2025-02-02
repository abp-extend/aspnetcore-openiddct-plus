import { Head } from "@inertiajs/react";
import AdminLayout from "@/Layouts/AdminLayout";
import {PageInfo, PaginatedResponse, User} from "@/Interfaces";

import UserList from "@/components/features/users/UserList.tsx";

interface Props {
    userResponse: PaginatedResponse<User>;
    error: string;
}
export default function Index(props: Props) {
    const {items, currentPage, pageSize, hasPreviousPage, hasNextPage, totalCount} = props.userResponse;
    const pagination = {
        currentPage,
        pageSize,
        totalCount,
        hasNextPage,
        hasPreviousPage
    } satisfies PageInfo;

    return (
        <>
            <Head>
                <title>Admin | User Management</title>
            </Head>
            <AdminLayout title="User Management" createType="user">
                <UserList users={items} pageInfo={pagination} error={props.error} />
            </AdminLayout>
        </>
    );
}