## Adding Docker support to the MathApp
### Getting into Docker

1. Install Docker on your machine if not installed.
1. Install the VSCode Docker extension.
1. Create your Dockerfile
1. Make changes to your DB server to accommodate Docker
1. Build your container
1. Run your container

### Create your Dockerfile
1. Create a file called `Dockerfile` (no extension) in your project directory - same director as .csproj.
1. Add in the following into the Dockerfile by
[referring to my Dockerfile here](../Dockerfile). Ensure that you use the docs or GPT to understand it nicely. Go line by line.

### In SSMS, create a new SQL DB User
Since we will be using SQL auth (no windows auth in linux which is our container)
1. Under Server Properties --> Security --> Server Authentication --> Choose SQL Server and Windows
1. Restart SQL Server in SQL Server Configuration Manager
1. Add a new user with username and password (right click security --> New --> Login --> username is docker). Do not enforce policy. Under server roles, make this user a sysadmin. Note: we are in a dev environment so this is fine! 
1. Test your new user in cmd to see if its created correctly (no errors = all good)
    ```
    sqlcmd -S host.docker.internal,1433 -U docker -P {your password} -d Math_DB -N -C
    ```

### Add the new connection string
Since your container is not technically your machine (its a container), you need to update your SQL Server Setting and your connection string.
See this: https://medium.com/@vedkoditkar/connect-to-local-ms-sql-server-from-docker-container-9d2b3d33e5e9
I would make a new connection string environment variable called `Math_DB_Docker`.
It would look like this:
```
"Server=host.docker.internal,1433;Database=Math_DB;User Id=docker;password={your password};Encrypt=True;TrustServerCertificate=True;"
```
### Adding session fix
Since we use sessions, we need a place to store the keys within our container.
1. Add this using to Program.cs
    ```
    using Microsoft.AspNetCore.DataProtection;
    using System.IO;
    ```

1. Add this service in Program.cs:
    ```
    builder.Services.AddDataProtection()
        .PersistKeysToFileSystem(new DirectoryInfo("/keys"))
        .SetApplicationName("MathApp");

    ```
### Building your container image
Run this command in command prompt, which assumes you environment variables are setup:
```
docker build --build-arg FirebaseMathApp=%FirebaseMathApp% --build-arg Math_DB=%Math_DB_Docker% -t mathapp-image . 
```
Note: to test you can use ```echo %Math_DB_Docker%``` before running the command.

Troubleshooting: If your environment variables not pull into the container from Windows, try this command.
```
docker build --build-arg FirebaseMathApp=$env:FirebaseMathApp --build-arg Math_DB=$env:Math_DB_Docker -t mathapp-image `
```
Credit to: Devesh Gokul, Vivek Rajaram and Aveshan Pillay (thank you!)

### Running you container
Run your container:
1. Mount the keys volume
    ``` 
    docker volume create mathapp-keys
    ```
1. Run container
    ```
    docker run -d -v mathapp-keys:/keys -p 8085:8080 -p 8086:8081 --name mathapp mathapp-image
    ```

### Next steps
1. Dockerize your MathAPI (and the SQL DB)
1. Dockerize your MathAPIClient