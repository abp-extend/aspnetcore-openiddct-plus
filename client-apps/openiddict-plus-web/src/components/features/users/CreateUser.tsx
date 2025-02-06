import {Button} from "@/components/ui/button"
import {
    Dialog,
    DialogContent,
    DialogFooter,
    DialogHeader,
    DialogTitle,
    DialogTrigger,
} from "@/components/ui/dialog"
import {Input} from "@/components/ui/input"
import {Label} from "@/components/ui/label"
import usePermission from "@/hooks/usePermission.ts";

export default function CreateUser() {
    const { can, isAdminUser } = usePermission();
    const canCreatePolicy = can('create policy');
    const currentUserIsAdmin = isAdminUser();
    const canCreateUser = canCreatePolicy || currentUserIsAdmin;
    return (
        <Dialog>
            <DialogTrigger asChild>
                <Button disabled={!canCreateUser}>
                    <svg xmlns="http://www.w3.org/2000/svg" width="28" height="28" fill="currentColor"
                         className="bi bi-plus-lg" viewBox="0 0 16 16">
                        <path fill-rule="evenodd"
                              d="M8 2a.5.5 0 0 1 .5.5v5h5a.5.5 0 0 1 0 1h-5v5a.5.5 0 0 1-1 0v-5h-5a.5.5 0 0 1 0-1h5v-5A.5.5 0 0 1 8 2"/>
                    </svg>
                    Create
                </Button>
            </DialogTrigger>
            <DialogContent className="sm:oidc-max-w-screen-md">
                <DialogHeader>
                    <DialogTitle>Create new user</DialogTitle>
                </DialogHeader>
                <form method="post" action="/users/create">
                    <span className="hidden"
                          dangerouslySetInnerHTML={{__html: window.__RequestVerificationToken}}></span>
                    <div className="grid oidc-grid-cols-8 gap-4 py-4">
                        <div className="flex oidc-flex-col oidc-col-span-4 oidc-space-y-1">
                            <Label htmlFor="firstName">
                                First Name
                            </Label>
                            <Input id="firstName" name="firstName" className="oidc-w-full"/>
                        </div>
                        <div className="flex oidc-flex-col oidc-col-span-4 oidc-space-y-1">
                            <Label htmlFor="lastName">
                                Last Name
                            </Label>
                            <Input id="lastName" name="lastName" className="oidc-w-full"/>
                        </div>
                        <div className="flex oidc-flex-col oidc-col-span-8 oidc-space-y-1">
                            <Label htmlFor="username">
                                Username
                            </Label>
                            <Input id="username" name="username" required/>
                        </div>
                        <div className="flex oidc-flex-col oidc-col-span-8 oidc-space-y-1">
                            <Label htmlFor="email">
                                Email
                            </Label>
                            <Input id="email" name="email" required/>
                        </div>
                        <div className="flex oidc-flex-col oidc-col-span-8 oidc-space-y-1">
                            <Label htmlFor="password">
                                Password
                            </Label>
                            <Input id="password" name="password" required/>
                        </div>
                    </div>
                    <DialogFooter>
                        <Button type="submit">Submit</Button>
                    </DialogFooter>
                </form>
            </DialogContent>
        </Dialog>
    )
}