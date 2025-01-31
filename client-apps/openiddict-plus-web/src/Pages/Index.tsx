import { Head } from "@inertiajs/react";

export default function Index(props: { name: string }) {
    return (
        <>
            <h1>Home</h1>
            <Head>
                <title>Your page title</title>
                <meta name="description" content="Your page description" />
            </Head>
            <p>Welcome to your Inertia app!</p>
            <div>name is {props.name}</div>
        </>
    );
}