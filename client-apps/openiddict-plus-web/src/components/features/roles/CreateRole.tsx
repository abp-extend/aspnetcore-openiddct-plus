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
import {Textarea} from "@/components/ui/textarea.tsx";


export default function CreateRole() {
    return (
        <Dialog>
            <DialogTrigger asChild>
                <Button>
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
                    <DialogTitle className="oidc-capitalize">Create new role</DialogTitle>
                </DialogHeader>
                <form method="post" action="/roles/create">
                    <span className="hidden"
                          dangerouslySetInnerHTML={{__html: window.__RequestVerificationToken}}></span>
                    <div className="grid oidc-grid-cols-8 gap-4 py-4">
                        <div className="flex oidc-flex-col oidc-col-span-8 oidc-space-y-1">
                            <Label htmlFor="name">
                                Role Name
                            </Label>
                            <Input id="name" name="name"  className="oidc-w-full"/>
                        </div>
                        <div className="flex oidc-flex-col oidc-col-span-8 oidc-space-y-1">
                            <Label htmlFor="description">
                                Description
                            </Label>
                            <Textarea id="description" name="description" className="oidc-w-full"/>
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
