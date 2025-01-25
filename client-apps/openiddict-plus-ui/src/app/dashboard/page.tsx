import {getSession} from "@/lib/openid";

export default async function Dashboard() {
    const session = await getSession()
    return (
        <div>
            <h1>Dashboard</h1>
            <div>
            <h2>User Session</h2>
            <code>
                <pre>{JSON.stringify(session, null, 4)}</pre>
            </code>
            </div>

        </div>
    )
}