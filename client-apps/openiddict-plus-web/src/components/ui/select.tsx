import * as React from "react"
import * as SelectPrimitive from "@radix-ui/react-select"
import { Check, ChevronDown, ChevronUp } from "lucide-react"

import { cn } from "@/lib/utils"

const Select = SelectPrimitive.Root

const SelectGroup = SelectPrimitive.Group

const SelectValue = SelectPrimitive.Value

const SelectTrigger = React.forwardRef<
  React.ElementRef<typeof SelectPrimitive.Trigger>,
  React.ComponentPropsWithoutRef<typeof SelectPrimitive.Trigger>
>(({ className, children, ...props }, ref) => (
  <SelectPrimitive.Trigger
    ref={ref}
    className={cn(
      "oidc-flex oidc-h-9 oidc-w-full oidc-items-center oidc-justify-between oidc-whitespace-nowrap oidc-rounded-md oidc-border oidc-border-input oidc-bg-transparent oidc-px-3 oidc-py-2 oidc-text-sm oidc-shadow-sm oidc-ring-offset-background placeholder:oidc-text-muted-foreground focus:oidc-outline-none focus:oidc-ring-1 focus:oidc-ring-ring disabled:oidc-cursor-not-allowed disabled:oidc-opacity-50 [&>span]:oidc-line-clamp-1",
      className
    )}
    {...props}
  >
    {children}
    <SelectPrimitive.Icon asChild>
      <ChevronDown className="oidc-h-4 oidc-w-4 oidc-opacity-50" />
    </SelectPrimitive.Icon>
  </SelectPrimitive.Trigger>
))
SelectTrigger.displayName = SelectPrimitive.Trigger.displayName

const SelectScrollUpButton = React.forwardRef<
  React.ElementRef<typeof SelectPrimitive.ScrollUpButton>,
  React.ComponentPropsWithoutRef<typeof SelectPrimitive.ScrollUpButton>
>(({ className, ...props }, ref) => (
  <SelectPrimitive.ScrollUpButton
    ref={ref}
    className={cn(
      "oidc-flex oidc-cursor-default oidc-items-center oidc-justify-center oidc-py-1",
      className
    )}
    {...props}
  >
    <ChevronUp className="oidc-h-4 oidc-w-4" />
  </SelectPrimitive.ScrollUpButton>
))
SelectScrollUpButton.displayName = SelectPrimitive.ScrollUpButton.displayName

const SelectScrollDownButton = React.forwardRef<
  React.ElementRef<typeof SelectPrimitive.ScrollDownButton>,
  React.ComponentPropsWithoutRef<typeof SelectPrimitive.ScrollDownButton>
>(({ className, ...props }, ref) => (
  <SelectPrimitive.ScrollDownButton
    ref={ref}
    className={cn(
      "oidc-flex oidc-cursor-default oidc-items-center oidc-justify-center oidc-py-1",
      className
    )}
    {...props}
  >
    <ChevronDown className="oidc-h-4 oidc-w-4" />
  </SelectPrimitive.ScrollDownButton>
))
SelectScrollDownButton.displayName =
  SelectPrimitive.ScrollDownButton.displayName

const SelectContent = React.forwardRef<
  React.ElementRef<typeof SelectPrimitive.Content>,
  React.ComponentPropsWithoutRef<typeof SelectPrimitive.Content>
>(({ className, children, position = "popper", ...props }, ref) => (
  <SelectPrimitive.Portal>
    <SelectPrimitive.Content
      ref={ref}
      className={cn(
        "oidc-relative oidc-z-50 oidc-max-h-96 oidc-min-w-[8rem] oidc-overflow-hidden oidc-rounded-md oidc-border oidc-bg-popover oidc-text-popover-foreground oidc-shadow-md data-[state=open]:oidc-animate-in data-[state=closed]:oidc-animate-out data-[state=closed]:oidc-fade-out-0 data-[state=open]:oidc-fade-in-0 data-[state=closed]:oidc-zoom-out-95 data-[state=open]:oidc-zoom-in-95 data-[side=bottom]:oidc-slide-in-from-top-2 data-[side=left]:oidc-slide-in-from-right-2 data-[side=right]:oidc-slide-in-from-left-2 data-[side=top]:oidc-slide-in-from-bottom-2",
        position === "popper" &&
          "data-[side=bottom]:oidc-translate-y-1 data-[side=left]:oidc--translate-x-1 data-[side=right]:oidc-translate-x-1 data-[side=top]:oidc--translate-y-1",
        className
      )}
      position={position}
      {...props}
    >
      <SelectScrollUpButton />
      <SelectPrimitive.Viewport
        className={cn(
          "oidc-p-1",
          position === "popper" &&
            "oidc-h-[var(--radix-select-trigger-height)] oidc-w-full oidc-min-w-[var(--radix-select-trigger-width)]"
        )}
      >
        {children}
      </SelectPrimitive.Viewport>
      <SelectScrollDownButton />
    </SelectPrimitive.Content>
  </SelectPrimitive.Portal>
))
SelectContent.displayName = SelectPrimitive.Content.displayName

const SelectLabel = React.forwardRef<
  React.ElementRef<typeof SelectPrimitive.Label>,
  React.ComponentPropsWithoutRef<typeof SelectPrimitive.Label>
>(({ className, ...props }, ref) => (
  <SelectPrimitive.Label
    ref={ref}
    className={cn("oidc-px-2 oidc-py-1.5 oidc-text-sm oidc-font-semibold", className)}
    {...props}
  />
))
SelectLabel.displayName = SelectPrimitive.Label.displayName

const SelectItem = React.forwardRef<
  React.ElementRef<typeof SelectPrimitive.Item>,
  React.ComponentPropsWithoutRef<typeof SelectPrimitive.Item>
>(({ className, children, ...props }, ref) => (
  <SelectPrimitive.Item
    ref={ref}
    className={cn(
      "oidc-relative oidc-flex oidc-w-full oidc-cursor-default oidc-select-none oidc-items-center oidc-rounded-sm oidc-py-1.5 oidc-pl-2 oidc-pr-8 oidc-text-sm oidc-outline-none focus:oidc-bg-accent focus:oidc-text-accent-foreground data-[disabled]:oidc-pointer-events-none data-[disabled]:oidc-opacity-50",
      className
    )}
    {...props}
  >
    <span className="oidc-absolute oidc-right-2 oidc-flex oidc-h-3.5 oidc-w-3.5 oidc-items-center oidc-justify-center">
      <SelectPrimitive.ItemIndicator>
        <Check className="oidc-h-4 oidc-w-4" />
      </SelectPrimitive.ItemIndicator>
    </span>
    <SelectPrimitive.ItemText>{children}</SelectPrimitive.ItemText>
  </SelectPrimitive.Item>
))
SelectItem.displayName = SelectPrimitive.Item.displayName

const SelectSeparator = React.forwardRef<
  React.ElementRef<typeof SelectPrimitive.Separator>,
  React.ComponentPropsWithoutRef<typeof SelectPrimitive.Separator>
>(({ className, ...props }, ref) => (
  <SelectPrimitive.Separator
    ref={ref}
    className={cn("oidc--mx-1 oidc-my-1 oidc-h-px oidc-bg-muted", className)}
    {...props}
  />
))
SelectSeparator.displayName = SelectPrimitive.Separator.displayName

export {
  Select,
  SelectGroup,
  SelectValue,
  SelectTrigger,
  SelectContent,
  SelectLabel,
  SelectItem,
  SelectSeparator,
  SelectScrollUpButton,
  SelectScrollDownButton,
}
