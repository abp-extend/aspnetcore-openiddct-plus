# AspNetCore Openiddict Plus
A comprehensive solution for ASP.NET Core Identity with OpenIddict, featuring advanced user, role, and permission management. This repository provides a robust foundation for secure authentication and authorization in modern web applications. Perfect for developers looking to integrate OpenIddict with enhanced access control capabilities.

## Features
- **User Management**: Create, update, and delete users with ease.
- **Role Management**: Define roles and assign permissions to control access.
- **Permission Management**: Manage permissions and assign them to roles.
- **Authentication**: Secure your application with Openiddict and JWT tokens.
- **Authorization**: Control access to resources based on roles and permissions.
- **Logging**: Log user activity and system events for auditing purposes.
- **Admin Panel**: Manage users, roles, and permissions through a user-friendly interface.

## Getting Started
1. Clone the repository.
2. Open the solution in Rider or Your choice of an editor.
3. Run the migrations [initial-migration](initial-migration.ps1)
4. Run the application.
4. Navigate to `https://localhost:7006` to access the admin panel.

## NextJS Client
1. Go to the client-apps/openiddict-plus-ui folder
2. Run `npm install`
3. Update the .env file with the correct values for example:
```bash
NEXT_AUTHORITY_API_URL=https://localhost:7006
NEXT_PUBLIC_CLIENT_ID=nextjs-client
NEXT_PUBLIC_APP_URL=https://localhost:3000
NEXT_PUBLIC_SCOPE='openid profile email roles api'
SESSION_PASSWORD=your secrets...
NODE_TLS_REJECT_UNAUTHORIZED=0
SERVER_ENV=development
```
The client id information can be found in the database in the openiddict applications table or see the `ClientSeeder.cs`
file.
4. Run `npm run dev`
5. `https://localhost:3000/auth/login` to trigger the login flow


## openid configuration
https://localhost:7006/.well-known/openid-configuration


## Technologies
- **ASP.NET Core**: A cross-platform, high-performance framework for building modern, cloud-based, Internet-connected applications.
- **Openiddict**: An easy-to-use library for adding OpenID Connect and OAuth 2.0 support to your applications.
- **Entity Framework Core**: A lightweight, extensible, open-source, and cross-platform version of the popular Entity Framework data access technology.
- **TailwindCss**: A free and open-source CSS utility framework and highly customizable design.
- **PostgreSQL**: A powerful, open-source object-relational database system.

## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contributors

| ![Sajankumar Vijayan](https://github.com/sajanv88.png?size=50) | ![Anto Subash](https://github.com/antosubash.png?size=50) |
|:--------------------------------------------------------------:|:---------------------------------------------------------:|
|       [Sajankumar Vijayan](https://github.com/sajanv88)        |       [Anto Subash](https://github.com/antosubash)        |
