import CreateRole from "@/components/features/roles/CreateRole.tsx";

declare global {
    interface Window {
        __RequestVerificationToken: string;
    }
}

import {Button} from "@/components/ui/button.tsx";
import {Input} from "@/components/ui/input.tsx";
import CreateUser from "@/components/features/users/CreateUser.tsx";

interface Props {
    children: React.ReactNode;
    title: string;
    createType: "user" | "role" | "permission";
}

export default function AdminLayout({children, title, createType}: Props) {
    return (

            <div className="flex flex-col justify-center items-center mx-auto max-w-screen-xl">
                <div className="w-full bg-white shadow-md rounded-lg p-6">
                    <h1 className="text-2xl font-medium pb-3">{title}</h1>
                    <div className="flex items-center justify-between pt-3 pb-5 oidc-border-b-4 oidc-border-primary">
                        <form>
                            <div className="relative flex w-full max-w-sm items-center ">
                                <Input type="search" id="search"
                                       className="block w-full p-4 ps-10 text-sm text-gray-900 border border-gray-300 rounded-lg bg-gray-50 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
                                       placeholder="Search..."/>

                                <Button type="submit" variant="ghost" size="icon">
                                    <svg className="w-4 h-4 text-gray-500 dark:text-gray-400" aria-hidden="true"
                                         xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 20 20">
                                        <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round"
                                              stroke-width="2" d="m19 19-4-4m0-7A7 7 0 1 1 1 8a7 7 0 0 1 14 0Z"/>
                                    </svg>
                                </Button>
                            </div>
                        </form>
                        {createType === "user" && <CreateUser />}
                        {createType === "role" && <CreateRole />}
                    </div>
                    <div>
                        {children}
                    </div>
                </div>
            </div>

    )

}