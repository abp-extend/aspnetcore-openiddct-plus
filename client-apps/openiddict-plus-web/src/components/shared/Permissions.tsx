import {Fragment, useEffect, useState} from "react";
import {PaginatedResponse, RolePermission} from "@/Interfaces";
import {Alert, AlertDescription, AlertTitle} from "@/components/ui/alert.tsx";
import {Checkbox} from "@/components/ui/checkbox.tsx";
import {AlertCircle} from "lucide-react";

interface Props {
    currentRoleId: string;
}

export default function Permissions({currentRoleId}: Props) {
    const [permissions, setPermissions] = useState<PaginatedResponse<RolePermission> | null>();
    const [error, setError] = useState<string | null>(null);
    const [abort] = useState(() => new AbortController());
    const [selectedPermissions, setSelectedPermissions] = useState<Array<string>>([]);


    const items = permissions?.items ?? [];
    const isPermissionSelected = (roleIds: Array<string>) => roleIds.includes(currentRoleId);
    const selectedPermissionNameAndIds = (id: string) => items.find(p => p.permissionId === id);
    useEffect(() => {
        async function fetchPermissions() {
            try {
                const response = await fetch('/api/permissions', {signal: abort.signal});
                if (!response.ok) {
                    throw new Error('An error occurred while fetching permissions');
                }
                const data = await response.json() as PaginatedResponse<RolePermission>;
                setPermissions(data);
            } catch (e: unknown) {
                if (e instanceof Error) {
                    setError(e.message);
                }
            }
        }

        fetchPermissions().catch(console.error);
        return () => abort.abort();
    }, [currentRoleId, abort]);

    useEffect(() => {
        if (permissions?.items) {
            setSelectedPermissions(permissions
                .items.filter((p) => isPermissionSelected(p.roleIds))
                .map((p) => p.permissionId));
        }
    }, [permissions?.items]);
    return (
        <div>
            <h1 className="oidc-font-semibold">Permissions</h1>
            {error && (
                <Alert variant="destructive" className="oidc-mb-3">
                    <AlertCircle className="oidc-h-4 oidc-w-4"/>
                    <AlertTitle>Error</AlertTitle>
                    <AlertDescription>{error}</AlertDescription>
                </Alert>
            )}
            {selectedPermissions.map((id, idx) => (
                <Fragment key={idx}>
                    <input type="hidden" name={`permissions[${idx}][name]`} value={selectedPermissionNameAndIds(id)?.name} />
                    <input type="hidden" name={`permissions[${idx}][permissionId]`} value={selectedPermissionNameAndIds(id)?.permissionId} />
                </Fragment>
            ))}
            {items.map((permission) => (
                <div key={permission.permissionId}>
                    <div className="oidc-flex oidc-items-center oidc-space-x-2">
                        <Checkbox id={permission.permissionId} className="oidc-my-2"
                                  defaultChecked={isPermissionSelected(permission.roleIds)}
                                  onCheckedChange={(checked) => {
                                      setSelectedPermissions((prev) => {
                                          if (checked) {
                                              return [...prev, permission.permissionId];
                                          }
                                          return prev.filter((id) => id !== permission.permissionId);
                                      });
                                  }}
                        />
                        <label
                            htmlFor={permission.permissionId}
                            className="oidc-text-sm oidc-font-medium oidc-leading-none oidc-peer-disabled:cursor-not-allowed oidc-peer-disabled:opacity-70"
                        >
                            {permission.name}
                        </label>
                    </div>
                </div>
            ))}

        </div>
    )
}