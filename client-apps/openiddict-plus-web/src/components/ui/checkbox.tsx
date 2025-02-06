import * as React from "react"
import * as CheckboxPrimitive from "@radix-ui/react-checkbox"
import { Check } from "lucide-react"

import { cn } from "@/lib/utils"

const Checkbox = React.forwardRef<
  React.ElementRef<typeof CheckboxPrimitive.Root>,
  React.ComponentPropsWithoutRef<typeof CheckboxPrimitive.Root>
>(({ className, ...props }, ref) => (
  <CheckboxPrimitive.Root
    ref={ref}
    className={cn(
      "oidc-peer oidc-h-4 oidc-w-4 oidc-shrink-0 oidc-rounded-sm oidc-border oidc-border-primary oidc-shadow focus-visible:oidc-outline-none focus-visible:oidc-ring-1 focus-visible:oidc-ring-ring disabled:oidc-cursor-not-allowed disabled:oidc-opacity-50 data-[state=checked]:oidc-bg-primary data-[state=checked]:oidc-text-primary-foreground",
      className
    )}
    {...props}
  >
    <CheckboxPrimitive.Indicator
      className={cn("oidc-flex oidc-items-center oidc-justify-center oidc-text-current")}
    >
      <Check className="oidc-h-4 oidc-w-4" />
    </CheckboxPrimitive.Indicator>
  </CheckboxPrimitive.Root>
))
Checkbox.displayName = CheckboxPrimitive.Root.displayName

export { Checkbox }
