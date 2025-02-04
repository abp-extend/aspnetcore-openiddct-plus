import * as React from "react"

import { cn } from "@/lib/utils"

const Textarea = React.forwardRef<
  HTMLTextAreaElement,
  React.ComponentProps<"textarea">
>(({ className, ...props }, ref) => {
  return (
    <textarea
      className={cn(
        "oidc-flex oidc-min-h-[60px] oidc-w-full oidc-rounded-md oidc-border oidc-border-input oidc-bg-transparent oidc-px-3 oidc-py-2 oidc-text-base oidc-shadow-sm placeholder:oidc-text-muted-foreground focus-visible:oidc-outline-none focus-visible:oidc-ring-1 focus-visible:oidc-ring-ring disabled:oidc-cursor-not-allowed disabled:oidc-opacity-50 md:oidc-text-sm",
        className
      )}
      ref={ref}
      {...props}
    />
  )
})
Textarea.displayName = "Textarea"

export { Textarea }
