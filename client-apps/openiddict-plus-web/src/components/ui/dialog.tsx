import * as React from "react"
import * as DialogPrimitive from "@radix-ui/react-dialog"
import { X } from "lucide-react"

import { cn } from "@/lib/utils"

const Dialog = DialogPrimitive.Root

const DialogTrigger = DialogPrimitive.Trigger

const DialogPortal = DialogPrimitive.Portal

const DialogClose = DialogPrimitive.Close

const DialogOverlay = React.forwardRef<
  React.ElementRef<typeof DialogPrimitive.Overlay>,
  React.ComponentPropsWithoutRef<typeof DialogPrimitive.Overlay>
>(({ className, ...props }, ref) => (
  <DialogPrimitive.Overlay
    ref={ref}
    className={cn(
      "oidc-fixed oidc-inset-0 oidc-z-50 oidc-bg-black/80 oidc- data-[state=open]:oidc-animate-in data-[state=closed]:oidc-animate-out data-[state=closed]:oidc-fade-out-0 data-[state=open]:oidc-fade-in-0",
      className
    )}
    {...props}
  />
))
DialogOverlay.displayName = DialogPrimitive.Overlay.displayName

const DialogContent = React.forwardRef<
  React.ElementRef<typeof DialogPrimitive.Content>,
  React.ComponentPropsWithoutRef<typeof DialogPrimitive.Content>
>(({ className, children, ...props }, ref) => (
  <DialogPortal>
    <DialogOverlay />
    <DialogPrimitive.Content
      ref={ref}
      className={cn(
        "oidc-fixed oidc-left-[50%] oidc-top-[50%] oidc-z-50 oidc-grid oidc-w-full oidc-max-w-lg oidc-translate-x-[-50%] oidc-translate-y-[-50%] oidc-gap-4 oidc-border oidc-bg-background oidc-p-6 oidc-shadow-lg oidc-duration-200 data-[state=open]:oidc-animate-in data-[state=closed]:oidc-animate-out data-[state=closed]:oidc-fade-out-0 data-[state=open]:oidc-fade-in-0 data-[state=closed]:oidc-zoom-out-95 data-[state=open]:oidc-zoom-in-95 data-[state=closed]:oidc-slide-out-to-left-1/2 data-[state=closed]:oidc-slide-out-to-top-[48%] data-[state=open]:oidc-slide-in-from-left-1/2 data-[state=open]:oidc-slide-in-from-top-[48%] sm:oidc-rounded-lg",
        className
      )}
      {...props}
    >
      {children}
      <DialogPrimitive.Close className="oidc-absolute oidc-right-4 oidc-top-4 oidc-rounded-sm oidc-opacity-70 oidc-ring-offset-background oidc-transition-opacity hover:oidc-opacity-100 focus:oidc-outline-none focus:oidc-ring-2 focus:oidc-ring-ring focus:oidc-ring-offset-2 disabled:oidc-pointer-events-none data-[state=open]:oidc-bg-accent data-[state=open]:oidc-text-muted-foreground">
        <X className="oidc-h-4 oidc-w-4" />
        <span className="oidc-sr-only">Close</span>
      </DialogPrimitive.Close>
    </DialogPrimitive.Content>
  </DialogPortal>
))
DialogContent.displayName = DialogPrimitive.Content.displayName

const DialogHeader = ({
  className,
  ...props
}: React.HTMLAttributes<HTMLDivElement>) => (
  <div
    className={cn(
      "oidc-flex oidc-flex-col oidc-space-y-1.5 oidc-text-center sm:oidc-text-left",
      className
    )}
    {...props}
  />
)
DialogHeader.displayName = "DialogHeader"

const DialogFooter = ({
  className,
  ...props
}: React.HTMLAttributes<HTMLDivElement>) => (
  <div
    className={cn(
      "oidc-flex oidc-flex-col-reverse sm:oidc-flex-row sm:oidc-justify-end sm:oidc-space-x-2",
      className
    )}
    {...props}
  />
)
DialogFooter.displayName = "DialogFooter"

const DialogTitle = React.forwardRef<
  React.ElementRef<typeof DialogPrimitive.Title>,
  React.ComponentPropsWithoutRef<typeof DialogPrimitive.Title>
>(({ className, ...props }, ref) => (
  <DialogPrimitive.Title
    ref={ref}
    className={cn(
      "oidc-text-lg oidc-font-semibold oidc-leading-none oidc-tracking-tight",
      className
    )}
    {...props}
  />
))
DialogTitle.displayName = DialogPrimitive.Title.displayName

const DialogDescription = React.forwardRef<
  React.ElementRef<typeof DialogPrimitive.Description>,
  React.ComponentPropsWithoutRef<typeof DialogPrimitive.Description>
>(({ className, ...props }, ref) => (
  <DialogPrimitive.Description
    ref={ref}
    className={cn("oidc-text-sm oidc-text-muted-foreground", className)}
    {...props}
  />
))
DialogDescription.displayName = DialogPrimitive.Description.displayName

export {
  Dialog,
  DialogPortal,
  DialogOverlay,
  DialogTrigger,
  DialogClose,
  DialogContent,
  DialogHeader,
  DialogFooter,
  DialogTitle,
  DialogDescription,
}
