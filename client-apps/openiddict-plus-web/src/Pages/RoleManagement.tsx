import {PageInfo, PaginatedResponse, Role} from "@/Interfaces";
import {Head} from "@inertiajs/react";
import AdminLayout from "@/Layouts/AdminLayout.tsx";
import RoleList from "@/components/features/roles/RoleList.tsx";

interface Props {
    data: PaginatedResponse<Role>;
    error: string;
}
export default function RoleManagement(props: Props) {
console.log(props);
    const {items, currentPage, pageSize, hasPreviousPage, hasNextPage, totalCount} = props.data;
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
                <title>Admin | Role Management</title>
            </Head>
            <AdminLayout title="Role Management" createType="role">
                <RoleList roles={items} pageInfo={pagination} error={props.error} />
            </AdminLayout>
        </>
    );
}