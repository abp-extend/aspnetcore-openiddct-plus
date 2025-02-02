import {useEffect, useState} from "react";
interface Props {
    error?: string;
    autoHide?: boolean;

}
export default function useError({error, autoHide}: Props) {
    const [message, setMessage] = useState<typeof error | null>(error);
    useEffect(() => {
        if(autoHide) {
            const id = setTimeout(() => {
                setMessage(null);
            }, 5000);
            return () => clearTimeout(id);
        }
    }, [autoHide]);


    return message;
}