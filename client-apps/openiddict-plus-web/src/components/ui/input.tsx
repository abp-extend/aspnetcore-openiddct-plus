import * as React from "react"

import { cn } from "@/lib/utils"

const Input = React.forwardRef<HTMLInputElement, React.ComponentProps<"input">>(
  ({ className, type, ...props }, ref) => {
    return (
      <input
        type={type}
        className={cn(
          "oidc-flex oidc-h-9 oidc-w-full oidc-rounded-md oidc-border oidc-border-input oidc-bg-transparent oidc-px-3 oidc-py-1 oidc-text-base oidc-shadow-sm oidc-transition-colors file:oidc-border-0 file:oidc-bg-transparent file:oidc-text-sm file:oidc-font-medium file:oidc-text-foreground placeholder:oidc-text-muted-foreground focus-visible:oidc-outline-none focus-visible:oidc-ring-1 focus-visible:oidc-ring-ring disabled:oidc-cursor-not-allowed disabled:oidc-opacity-50 md:oidc-text-sm",
          className
        )}
        ref={ref}
        {...props}
      />
    )
  }
)
Input.displayName = "Input"

export { Input }
