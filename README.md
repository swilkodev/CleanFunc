[![Build Status](https://dev.azure.com/swilkodev/cleanarch/_apis/build/status/swilkodev.CleanFunc?branchName=master)](https://dev.azure.com/swilkodev/cleanarch/_build/latest?definitionId=1&branchName=master)

# CleanFunc
Clean architecture with azure functions. \**Work In Progress\**

### Getting Started

To run the function app, firstly run the **scripts/setup.ps1** file to create azure dependencies.

### Database Configuration

The sample is configured to use Entity Framework InMemory database by default. 

Alternatively if you want to use CosmosDB, you will need to update  **FunctionApp/local.settings.json** as follows: 

```json
  "UseInMemoryDatabase": false,
```

I will be adding support for SQL Server soon.

### Other Clean Architecture Repositories

 - https://github.com/jasontaylordev/CleanArchitecture
 - https://github.com/jasontaylordev/NorthwindTraders


