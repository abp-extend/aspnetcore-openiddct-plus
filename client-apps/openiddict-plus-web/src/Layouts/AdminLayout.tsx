import { Button } from "flowbite-react";
interface Props {
    children: React.ReactNode;
    title: string;
}
export default function AdminLayout({children, title}: Props) {
    return (
        <div className="flex flex-col justify-center items-center mx-auto max-w-screen-xl">
            <div className="w-full bg-white shadow-md rounded-lg p-6">
                <h1 className="text-2xl font-medium pb-3">{title}</h1>
                <div className="flex items-center justify-between pt-3 pb-5">
                    <form>
                        <label htmlFor="search"
                               className="mb-2 text-sm font-medium text-gray-900 sr-only dark:text-white">Search</label>
                        <div className="relative">
                            <div className="absolute inset-y-0 start-0 flex items-center ps-3 pointer-events-none">
                                <svg className="w-4 h-4 text-gray-500 dark:text-gray-400" aria-hidden="true"
                                     xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 20 20">
                                    <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round"
                                          stroke-width="2" d="m19 19-4-4m0-7A7 7 0 1 1 1 8a7 7 0 0 1 14 0Z"/>
                                </svg>
                            </div>
                            <input type="search" id="search"
                                   className="block w-full p-4 ps-10 text-sm text-gray-900 border border-gray-300 rounded-lg bg-gray-50 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
                                   placeholder="Search..."/>
                            <button type="submit"
                                    className="text-white absolute end-2.5 bottom-2.5 bg-gray-700 hover:bg-gray-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-4 py-2 dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800">Search
                            </button>
                        </div>
                    </form>
                    <Button color="blue" className="flex items-center space-x-4">
                        <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" fill="currentColor"
                             className="bi bi-plus-lg" viewBox="0 0 16 16">
                            <path fill-rule="evenodd"
                                  d="M8 2a.5.5 0 0 1 .5.5v5h5a.5.5 0 0 1 0 1h-5v5a.5.5 0 0 1-1 0v-5h-5a.5.5 0 0 1 0-1h5v-5A.5.5 0 0 1 8 2"/>
                        </svg>
                        <span>Create</span>
                    </Button>
                </div>
                <div>
                    {children}
                </div>
            </div>
        </div>
    )

}