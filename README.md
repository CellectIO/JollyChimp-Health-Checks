# Hello! 👋 
### Welcome to JollyChimp Health Checks

---

# Feature Roadmap
 
### Replace Xabarils UI with Custom UI
- Replace built in Xabarils UI so that we have more control over its functionality (specifically the NAV bar)

### Auth
- Implement Auth on JollyChimp to make it a secure platform.

### Change Logs Page
- Implement a better (healthcheck, endpoint and webhook) deployment service (currently if changes are made to these services
  they are still sometimes considered "deployed" when they are not) With that said, with this new service we should create the
  Change logs screen, which will give the user a complete overview of everything that's still outstanding since last server restart.

### Logging
- Why does JollyChimp not already have logging implemented? good question

### Error Handling for badly Configured HealthChecks / Endpoint / Webhooks
- Add error handling abilities for when a badly configured entity throws an exception on startup to prevent the entire application
  from not starting up at all. This Error should then be associated to the entity on the UI for the user to correct.

### Test HealthChecks / Endpoints / Webhooks
- Add a test button to entities allowing users to test if the provided details work before saving the entity.

### JollyChimp Docker
- Make JollyChimp available as a docker image and optionaly host the image on docker.

---



### Migrations

```csharp
dotnet ef migrations add Init --project "JollyChimp.Core.Data" --startup-project "JollyChimp.UI" --context "JollyChimpContext" --verbose
```

```csharp
dotnet ef database update --project "JollyChimp.Core.Data" --startup-project "JollyChimp.UI" --context "JollyChimpContext"
```