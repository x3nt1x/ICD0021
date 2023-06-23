# Generate Database Migration

~~~bash
# install or update
dotnet tool install --global dotnet-ef
dotnet tool update --global dotnet-ef

# create migration
dotnet ef migrations add Initial --project App.DAL --startup-project WebApp --context AppDbContext 

# apply migration
dotnet ef database update --project App.DAL --startup-project WebApp --context AppDbContext 

# delete database
dotnet ef database drop --project App.DAL --startup-project WebApp
~~~