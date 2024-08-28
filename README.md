# [Authentication System API]

An authentication system with a simple RESTful API.

## Introduction

This system goal is to make it easier and quicker to implement authentication and authorization in your application using a simple API.

## Some Features

- Account registration, sign in and sign out.
- Forgot password and reset password.
- Two factors authentication.
- Email confirmation.
- Users management.
- Roles management (authorization).

## Getting Started

To get started with this project, follow these steps:

### Prerequisites

- [.NET SDK 6.0 or later]
- [Sql Server]

### Installation

1. **Clone the Repository**

   ```bash
   git clone https://github.com/youssef-rafie212/Auth-System.git
   ```
2. **Navigate to the project directory**

3. **Restore Dependencies**
 
	```bash
	dotnet restore
	```
 
 ### Configuration

 **The application uses environment variables for configuration. Set the following environment variables as needed:**

 - AUTHSYS_DEV_DB_DEFAULT_URL = "Your DB connection string"
 - AUTHSYS_DEV_JWT_SECRET = "Your JWT secret key"
 - AUTHSYS_DEV_JWT_ISSUER = "Your JWT trusted issuer"

 ### Running The Application

 **Navigate to the Authentication-System folder and run:**
  ```bash
	dotnet run
  ```

  ### API Documentation

  Link : https://documenter.getpostman.com/view/29387971/2sAXjJ6Yk3

  ## License

  This project is licensed under the MIT License.
  