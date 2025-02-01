import { Head } from "@inertiajs/react";
import AdminLayout from "@/Layouts/AdminLayout";
import { PaginatedResponse, User } from "@/Interfaces";
import {Table, TableBody, TableCell, TableHead, TableHeader, TableRow} from "@/components/ui/table.tsx";
import {Button} from "@/components/ui/button.tsx";
import {
    Pagination,
    PaginationContent,
    PaginationItem,
    PaginationNext,
    PaginationPrevious
} from "@/components/ui/pagination.tsx";

interface Props {
    userResponse: PaginatedResponse<User>;
}
export default function Index(props: Props) {
    const users = props.userResponse.data;
    console.log(props.userResponse);

   
    return (
        <>
            <Head>
                <title>Admin | User Management</title>
            </Head>
            <AdminLayout title="User Management">
                <div className="overflow-x-auto">
                    <Table>
                        <TableHeader>
                            <TableRow>
                                <TableHead> First name</TableHead>
                                <TableHead>Username</TableHead>
                                <TableHead>Email</TableHead>
                                <TableHead>Email verified</TableHead>
                                <TableHead>Action</TableHead>
                            </TableRow>
                        </TableHeader>
                        <TableBody>
                            {users.map((user) => (
                                <TableRow key={user.id}>
                                    <TableCell>{user.firstName}</TableCell>
                                    <TableCell>{user.userName}</TableCell>
                                    <TableCell>{user.email}</TableCell>
                                    <TableCell>{user.emailConfirmed ? "Yes": "No"}</TableCell>
                                    <TableCell>
                                        <span className="flex items-center space-x-2">
                                            <Button>Edit</Button>
                                            <Button variant="destructive">Delete</Button>
                                        </span>
                                    </TableCell>
                                </TableRow>
                            ))}
                        </TableBody>
                    </Table>
                    {props.userResponse.totalPages > 10 && (
                        <div className="flex overflow-x-auto justify-center pt-5">
                            <Pagination>
                                <PaginationContent>
                                    <PaginationItem>
                                        <PaginationPrevious href="#"/>
                                    </PaginationItem>
                                    <span className="text-sm text-gray-700 dark:text-gray-400">
                                        Showing <span className="font-semibold text-gray-900 dark:text-white">{props.userResponse.currentPage}</span> to <span
                                        className="font-semibold text-gray-900 dark:text-white">{props.userResponse.pageSize}</span> of <span
                                        className="font-semibold text-gray-900 dark:text-white">{props.userResponse.totalPages}</span> Entries
                                    </span>
                                    <PaginationItem>
                                        <PaginationNext href="#"/>
                                    </PaginationItem>
                                </PaginationContent>
                            </Pagination>
                        </div>
                    )}
                </div>
            </AdminLayout>
        </>
    );
}