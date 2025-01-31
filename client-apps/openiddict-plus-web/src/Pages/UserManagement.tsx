import { Head } from "@inertiajs/react";
import AdminLayout from "@/Layouts/AdminLayout";
import { PaginatedResponse, User } from "@/Interfaces";
import { Table, Button, Pagination } from "flowbite-react";
import { useState } from "react";

interface Props {
    userResponse: PaginatedResponse<User>;
}
export default function Index(props: Props) {
    const users = props.userResponse.data;
    console.log(props.userResponse);
    const [currentPage, setCurrentPage] = useState(props.userResponse.currentPage);

    const onPageChange = (page: number) => setCurrentPage(page);
   
    return (
        <>
            
            <Head>
                <title>Admin | User Management</title>
            </Head>
            <AdminLayout title="User Management">
                <div className="overflow-x-auto">
                    <Table>
                        <Table.Head>
                            <Table.HeadCell>First name</Table.HeadCell>
                            <Table.HeadCell>Username</Table.HeadCell>
                            <Table.HeadCell>Email</Table.HeadCell>
                            <Table.HeadCell>Email verified</Table.HeadCell>
                            <Table.HeadCell>Action</Table.HeadCell>
                        </Table.Head>
                        <Table.Body className="divide-y text-black divide-gray-200">
                            {users.map((user) => (
                                <Table.Row key={user.id}>
                                    <Table.Cell>{user.firstName}</Table.Cell>
                                    <Table.Cell>{user.userName}</Table.Cell>
                                    <Table.Cell>{user.email}</Table.Cell>
                                    <Table.Cell>{user.emailConfirmed ? "Yes": "No"}</Table.Cell>
                                    <Table.Cell>
                                        <span className="flex items-center space-x-2">
                                            <Button color="blue">Edit</Button>
                                            <Button color="failure">Delete</Button>
                                        </span>
                                    </Table.Cell>
                                </Table.Row>
                            ))}
                        </Table.Body>
                    </Table>
                    {props.userResponse.totalPages > 10 && (
                        <div className="flex overflow-x-auto justify-center pt-5">
                            <Pagination layout="table" currentPage={currentPage}
                                        totalPages={props.userResponse.totalPages} onPageChange={onPageChange} showIcons />
                        </div>
                    )}
                </div>
            </AdminLayout>
        </>
    );
}