# IO-project

## General info
Application to manage the vehicle rental network.

## Technologies
-  Backend
    - .Net 6.0 MinimalAPI
    - EntityFrameworkCore
    - AspNetCore.Identity
    - Microsoft SQL Server
    - XUnit
    - Moq

- Frontend
    - React
    - TypeScript
    - ChakraUI
    - Axios

## Usage

### Backend
Enter connection string and jwt key in _appsettings.json_ file inside _API_ project.

In _API_ directory run following commands:
```console
> dotnet ef database update
```
to create SQL database

```console
> dotnet run
```
to run application at _https://localhost:7044_ and _http://localhost:5265_

### Frontend
In _client-app_ directory run following commands:
```console
> npm install
```
to install dependences 
```console
> npm start
```
to run application at _http://localhost:3000_