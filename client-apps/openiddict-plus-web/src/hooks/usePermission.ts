import {useEffect, useState} from "react";
import {CurrentUser} from "@/Interfaces";

export default function usePermission() {
    const [userSetting, setUserSetting] = useState<CurrentUser | null>(null);
    useEffect(() => {
        async function fetchUserSettings() {
            const settings = sessionStorage.getItem('currentUser');
            if (settings) {
                setUserSetting(JSON.parse(settings).currentUser);
                return;
            }
            try {
                const response = await fetch('/api/settings');
                if (!response.ok) {
                    throw new Error('An error occurred while fetching user settings');
                }
                const data = await response.json() as {currentUser: CurrentUser};
                sessionStorage.setItem('currentUser', JSON.stringify(data));
                setUserSetting(data.currentUser);
            } catch (e: unknown) {
                if (e instanceof Error) {
                    console.error(e.message);
                }
            }
        }
        fetchUserSettings().catch(console.error);
    }, []);
    return {
        isAdminUser: () => !!userSetting?.roles?.find(r => r.name.toLowerCase() === 'admin')?.id,
        can: (permission: string) => {
            return !!userSetting?.permissions?.find(p => p.name.toLowerCase() === permission.toLowerCase())?.id;
        }
    }
}