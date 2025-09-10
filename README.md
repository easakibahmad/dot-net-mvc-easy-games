### Users for login

username: owner, 
password: owner123

username: user
password: user123

### Run EasyGames (.NET 8)

1. Check .NET version

dotnet --version
You need 8.0.x. Example: 8.0.119.

2. Navigate to project folder

3. Restore dependencies: dotnet restore

4. Run the project: dotnet run

5. Open in browser: http://localhost:5008

Optional: If the default port is in use: dotnet run --urls "http://localhost:5009"