# BugTrackerSample
Sample bug tracker technical task.

### Choices:
For the last few years my tech stack has been primarily WPF desktop applications and web services using Microsoft Orleans. These technologies would require too much setup and are overkill for the scope of the task.
Instead, I have used Blazor with Entity Framework. These are suited for the task, and I wanted to investigate them since they have come up in recent conversations.

### SQL Database Setup:
- Create a test database in the RDBMS of your choice
    - Addition migration required if using something other than Microsoft SQL Server
- Update DefaultConnectionString in .\AireBugTracker\appsettings.Development.json
- Ensure [Entity Framework CLI](https://learn.microsoft.com/en-us/ef/core/cli/dotnet) is installed
- Navigate to project in terminal
- If you are not using Microsoft SQL Server execute "dotnet ef migrations add 'mg-2'"
- Execute "dotnet ef database update" 

### Running:
- Run in debug to launch in browser
- AireBugTrackerTest project contains basic unit tests using controller methods

### Limitations
- No authentication
- Caching issue caused by EntityFramework tracking
- Delete user not available in UI due to an issue managing EFs many-to-one connection
