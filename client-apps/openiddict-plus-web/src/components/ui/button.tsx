import * as React from "react"
import { Slot } from "@radix-ui/react-slot"
import { cva, type VariantProps } from "class-variance-authority"

import { cn } from "@/lib/utils"

const buttonVariants = cva(
  "oidc-inline-flex oidc-items-center oidc-justify-center oidc-gap-2 oidc-whitespace-nowrap oidc-rounded-md oidc-text-sm oidc-font-medium oidc-transition-colors focus-visible:oidc-outline-none focus-visible:oidc-ring-1 focus-visible:oidc-ring-ring disabled:oidc-pointer-events-none disabled:oidc-opacity-50 [&_svg]:oidc-pointer-events-none [&_svg]:oidc-size-4 [&_svg]:oidc-shrink-0",
  {
    variants: {
      variant: {
        default:
          "oidc-bg-primary oidc-text-primary-foreground oidc-shadow hover:oidc-bg-primary/90",
        destructive:
          "oidc-bg-destructive oidc-text-destructive-foreground oidc-shadow-sm hover:oidc-bg-destructive/90",
        outline:
          "oidc-border oidc-border-input oidc-bg-background oidc-shadow-sm hover:oidc-bg-accent hover:oidc-text-accent-foreground",
        secondary:
          "oidc-bg-secondary oidc-text-secondary-foreground oidc-shadow-sm hover:oidc-bg-secondary/80",
        ghost: "hover:oidc-bg-accent hover:oidc-text-accent-foreground",
        link: "oidc-text-primary oidc-underline-offset-4 hover:oidc-underline",
      },
      size: {
        default: "oidc-h-9 oidc-px-4 oidc-py-2",
        sm: "oidc-h-8 oidc-rounded-md oidc-px-3 oidc-text-xs",
        lg: "oidc-h-10 oidc-rounded-md oidc-px-8",
        icon: "oidc-h-9 oidc-w-9",
      },
    },
    defaultVariants: {
      variant: "default",
      size: "default",
    },
  }
)

export interface ButtonProps
  extends React.ButtonHTMLAttributes<HTMLButtonElement>,
    VariantProps<typeof buttonVariants> {
  asChild?: boolean
}

const Button = React.forwardRef<HTMLButtonElement, ButtonProps>(
  ({ className, variant, size, asChild = false, ...props }, ref) => {
    const Comp = asChild ? Slot : "button"
    return (
      <Comp
        className={cn(buttonVariants({ variant, size, className }))}
        ref={ref}
        {...props}
      />
    )
  }
)
Button.displayName = "Button"

export { Button, buttonVariants }
