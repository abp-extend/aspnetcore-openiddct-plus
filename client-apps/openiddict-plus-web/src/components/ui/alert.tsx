import * as React from "react"
import { cva, type VariantProps } from "class-variance-authority"

import { cn } from "@/lib/utils"

const alertVariants = cva(
  "oidc-relative oidc-w-full oidc-rounded-lg oidc-border oidc-px-4 oidc-py-3 oidc-text-sm [&>svg+div]:oidc-translate-y-[-3px] [&>svg]:oidc-absolute [&>svg]:oidc-left-4 [&>svg]:oidc-top-4 [&>svg]:oidc-text-foreground [&>svg~*]:oidc-pl-7",
  {
    variants: {
      variant: {
        default: "oidc-bg-background oidc-text-foreground",
        destructive:
          "oidc-border-destructive/50 oidc-text-destructive dark:oidc-border-destructive [&>svg]:oidc-text-destructive",
      },
    },
    defaultVariants: {
      variant: "default",
    },
  }
)

const Alert = React.forwardRef<
  HTMLDivElement,
  React.HTMLAttributes<HTMLDivElement> & VariantProps<typeof alertVariants>
>(({ className, variant, ...props }, ref) => (
  <div
    ref={ref}
    role="alert"
    className={cn(alertVariants({ variant }), className)}
    {...props}
  />
))
Alert.displayName = "Alert"

const AlertTitle = React.forwardRef<
  HTMLParagraphElement,
  React.HTMLAttributes<HTMLHeadingElement>
>(({ className, ...props }, ref) => (
  <h5
    ref={ref}
    className={cn("oidc-mb-1 oidc-font-medium oidc-leading-none oidc-tracking-tight", className)}
    {...props}
  />
))
AlertTitle.displayName = "AlertTitle"

const AlertDescription = React.forwardRef<
  HTMLParagraphElement,
  React.HTMLAttributes<HTMLParagraphElement>
>(({ className, ...props }, ref) => (
  <div
    ref={ref}
    className={cn("oidc-text-sm [&_p]:oidc-leading-relaxed", className)}
    {...props}
  />
))
AlertDescription.displayName = "AlertDescription"

export { Alert, AlertTitle, AlertDescription }
