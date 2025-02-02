import {User} from "@/Interfaces";
import {
    Dialog,
    DialogContent,
    DialogFooter,
    DialogHeader,
    DialogTitle,
    DialogTrigger
} from "@/components/ui/dialog.tsx";
import {Button} from "@/components/ui/button.tsx";
import {Label} from "@/components/ui/label.tsx";
import {Input} from "@/components/ui/input.tsx";

interface Props {
    user: User
}
export default function UpdateUser({user}: Props) {
    return (
        <Dialog>
            <DialogTrigger asChild>
                <Button size="icon" variant="secondary" disabled={!!user.deletionRequestedAt}>
                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor"
                         className="bi bi-pencil-square" viewBox="0 0 16 16">
                        <path
                            d="M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z"/>
                        <path fill-rule="evenodd"
                              d="M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5z"/>
                    </svg>
                </Button>
            </DialogTrigger>
            <DialogContent className="sm:oidc-max-w-screen-md">
                <DialogHeader>
                    <DialogTitle className="oidc-capitalize">Edit {user.userName}</DialogTitle>
                </DialogHeader>
                <form method="post" action="/admin/user-management/update">
                    <span className="hidden"
                          dangerouslySetInnerHTML={{__html: window.__RequestVerificationToken}}></span>
                    <input type="hidden" name="id" value={user.id}/>
                    <div className="grid oidc-grid-cols-8 gap-4 py-4">
                        <div className="flex oidc-flex-col oidc-col-span-4 oidc-space-y-1">
                            <Label htmlFor="firstName">
                                First Name
                            </Label>
                            <Input id="firstName" name="firstName" defaultValue={user.firstName} className="oidc-w-full"/>
                        </div>
                        <div className="flex oidc-flex-col oidc-col-span-4 oidc-space-y-1">
                            <Label htmlFor="lastName">
                                Last Name
                            </Label>
                            <Input id="lastName" name="lastName" defaultValue={user.lastName} className="oidc-w-full"/>
                        </div>
                        <div className="flex oidc-flex-col oidc-col-span-8 oidc-space-y-1">
                            <Label htmlFor="username">
                                Username
                            </Label>
                            <Input id="username" name="username" defaultValue={user.userName} required/>
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