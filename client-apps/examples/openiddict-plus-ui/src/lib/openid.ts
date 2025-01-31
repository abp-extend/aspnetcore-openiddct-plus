import {IronSession, SessionOptions, getIronSession} from 'iron-session'
import {cookies} from 'next/headers'
import * as client from 'openid-client'
import ky from "ky";


export const clientConfig = {
    url: process.env.NEXT_AUTHORITY_API_URL,
    audience: process.env.NEXT_PUBLIC_API_URL,
    client_id: process.env.NEXT_PUBLIC_CLIENT_ID,
    scope: process.env.NEXT_PUBLIC_SCOPE,
    redirect_uri: `${process.env.NEXT_PUBLIC_APP_URL}/auth/oidc`,
    post_logout_redirect_uri: `${process.env.NEXT_PUBLIC_APP_URL}`,
    response_type: 'code',
    grant_type: 'authorization_code',
    post_login_route: `${process.env.NEXT_PUBLIC_APP_URL}/dashboard`,
    code_challenge_method: 'S256',
}

export interface SessionData {
    isLoggedIn: boolean
    access_token?: string
    code_verifier?: string
    state?: string
    refreshToken?: string
    expiresIn?: number
    userInfo?: {
        sub: string
        name: string
        email: string
        email_verified: boolean
    }
}

export const defaultSession: SessionData = {
    isLoggedIn: false,
    access_token: undefined,
    code_verifier: undefined,
    state: undefined,
    userInfo: undefined,

}

export const sessionOptions: SessionOptions = {
    password: process.env.SESSION_PASSWORD!,
    cookieName: 'web-session',
    cookieOptions: {
        // secure only works in `https` environments
        // if your localhost is not on `https`, then use: `secure: process.env.NODE_ENV === "production"`
        secure: true,
    },
    ttl: 3600 * 8, // 8 hours
}

export async function getSession(): Promise<IronSession<SessionData>> {
    const cookiesList = await cookies()
    const session = await getIronSession<SessionData>(cookiesList, sessionOptions)
    if (!session.isLoggedIn) {
        session.access_token = defaultSession.access_token
        session.expiresIn = defaultSession.expiresIn
        session.refreshToken = defaultSession.refreshToken
        session.userInfo = defaultSession.userInfo
    }
    return session
}


export async function getClientConfig() {
    console.log("App Environment: ", process.env);
    if (process.env.SERVER_ENV === "test") {
        return await client.discovery(new URL(clientConfig.url!), clientConfig.client_id!, {
            client_secret: process.env.CLIENT_SECRET,
        }, undefined, {
            [client.customFetch]: (...args) => ky(args[0], {
                ...args[1],
                hooks: {
                    beforeRequest: [
                        (request) => {
                            request.headers.set('X-API-KEY', `${process.env.X_API_KEY}`)
                        }
                    ]
                }
            })
        });
    }

    return await client.discovery(new URL(clientConfig.url!), clientConfig.client_id!, {
        client_secret: process.env.CLIENT_SECRET,
    });

}
