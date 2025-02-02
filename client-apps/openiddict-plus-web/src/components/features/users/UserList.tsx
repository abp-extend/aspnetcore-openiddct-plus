import {PageInfo, User} from "@/Interfaces";
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
import DeleteUser from "@/components/features/users/DeleteUser.tsx";
import {cn} from "@/lib/utils.ts";
import UpdateUser from "@/components/features/users/UpdateUser.tsx";

interface Props {
    users: Array<User>;
    pageInfo: PageInfo;
    error?: string;
}

export default function UserList(props: Props) {
    const {users, pageInfo, error} = props;
    console.log(users);
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
                        <TableHead> First name</TableHead>
                        <TableHead>Username</TableHead>
                        <TableHead>Email</TableHead>
                        <TableHead>Email verified</TableHead>
                        <TableHead>Action</TableHead>
                    </TableRow>
                </TableHeader>
                <TableBody>
                    {users.map((user) => (
                        <TableRow key={user.id} className={cn({
                            "oidc-bg-amber-100 hover:oidc-bg-amber-100 hover:oidc-cursor-not-allowed": user.deletionRequestedAt,
                        })}
                                  title={user.deletionRequestedAt ? "Deletion requested and it will be deleted in 30 days." : ""}>
                            <TableCell>{user.firstName}</TableCell>
                            <TableCell>{user.userName}</TableCell>
                            <TableCell>{user.email}</TableCell>
                            <TableCell>{user.emailConfirmed ? "Yes" : "No"}</TableCell>
                            <TableCell className="flex  oidc-space-x-2">
                                <UpdateUser user={user}/>
                                <DeleteUser user={user}/>
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