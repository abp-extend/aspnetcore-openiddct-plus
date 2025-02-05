import {PageInfo, Role} from "@/Interfaces";
import {Table, TableBody, TableCell, TableHead, TableHeader, TableRow} from "@/components/ui/table.tsx";

import {
    Pagination,
    PaginationContent,
    PaginationItem,
    PaginationNext,
    PaginationPrevious
} from "@/components/ui/pagination.tsx";
import {Alert, AlertDescription, AlertTitle} from "@/components/ui/alert.tsx";
import {AlertCircle} from "lucide-react";
import useError from "@/hooks/useError.ts";
import DeleteRole from "@/components/features/roles/DeleteRole.tsx";
import UpdateRole from "@/components/features/roles/UpdateRole.tsx";


interface Props {
    roles: Array<Role>;
    pageInfo: PageInfo;
    error?: string;
}

export default function RoleList(props: Props) {
    const {roles, pageInfo, error} = props;
    console.log(roles);
    const errorMessage = useError({error, autoHide: true});
    return (
        <div className="overflow-x-auto">
            {errorMessage && (
                <Alert variant="destructive">
                    <AlertCircle className="h-4 w-4"/>
                    <AlertTitle>Error</AlertTitle>
                    <AlertDescription>
                        {error}
                    </AlertDescription>
                </Alert>
            )}
            <Table className="mt-5">
                <TableHeader>
                    <TableRow className="oidc-text-lg">
                        <TableHead>Name</TableHead>
                        <TableHead>Assigned permissions</TableHead>
                        <TableHead>Action</TableHead>
                    </TableRow>
                </TableHeader>
                <TableBody>
                    {roles.map((role) => (
                        <TableRow key={role.roleId}>
                            <TableCell>{role.roleName}</TableCell>
                            <TableCell>Total permissions ({role.permissions.length})</TableCell>
                            <TableCell className="flex  oidc-space-x-2">
                                <UpdateRole role={role}/>
                                <DeleteRole role={role}/>
                            </TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
            {pageInfo.totalCount > 10 && (
                <div className="flex overflow-x-auto justify-center pt-5">
                    <Pagination>
                        <PaginationContent>
                            <PaginationItem>
                                <PaginationPrevious href="#"/>
                            </PaginationItem>
                            <span className="text-sm text-gray-700 dark:text-gray-400">
                                        Showing <span
                                className="font-semibold text-gray-900 dark:text-white">{pageInfo.currentPage}</span> to <span
                                className="font-semibold text-gray-900 dark:text-white">{pageInfo.pageSize}</span> of <span
                                className="font-semibold text-gray-900 dark:text-white">{pageInfo.totalCount}</span> Entries
                                    </span>
                            <PaginationItem>
                                <PaginationNext href="#"/>
                            </PaginationItem>
                        </PaginationContent>
                    </Pagination>
                </div>
            )}
        </div>
    )
}