# CarrotRent

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
Enter connection string to ConnectionStrings>DefaultConnection and jwt key to Jwt>Key sections in _appsettings.json_ file inside _API_ project.

In _API_ directory run following command:
```console
> dotnet ef database update
```
to create SQL database

```console
> dotnet run
```
to run application at _https://localhost:7044_ and _http://localhost:5265_

### Tests
In _UnitTests_ directory run following command:
```console
> dotnet test
```
to run all unit tests using xUnit library

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

## Screenshots
![map view](https://github.com/Damian0401/CarrotRent.App/blob/main/images/carrot-rent-map.png)
![create view](https://github.com/Damian0401/CarrotRent.App/blob/main/images/carrot-rent-create.png)
![details view](https://github.com/Damian0401/CarrotRent.App/blob/main/images/carrot-rent-details.png)
![department view](https://github.com/Damian0401/CarrotRent.App/blob/main/images/carrot-rent-department.png)
![vehicle view](https://github.com/Damian0401/CarrotRent.App/blob/main/images/carrot-rent-vehicle.png)
![vehicles view](https://github.com/Damian0401/CarrotRent.App/blob/main/images/carrot-rent-vehicles.png)
