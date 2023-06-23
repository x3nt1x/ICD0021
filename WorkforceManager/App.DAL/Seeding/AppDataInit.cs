using App.Domain;
using App.Domain.Enums;
using App.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.Seeding;

public static class AppDataInit
{
    private static readonly Guid UserId = Guid.Parse("9c2ad71d-5e44-4d1c-9116-300bb291a48a");
    private static readonly Guid AdminId = Guid.Parse("bc7458ac-cbb0-4ecd-be79-d5abf19f8c77");
    private static readonly Guid ClientId = Guid.Parse("d4730035-a652-4ed6-896d-7229ddbe04f0");
    private static readonly Guid OrderId = Guid.Parse("c44d49e8-d011-4c17-ad16-e24cceefde90");
    private static readonly Guid AssignmentDesignId = Guid.Parse("11ac6686-2be6-442a-9d37-14ef6baa4190");
    private static readonly Guid AssignmentFunctionalityId = Guid.Parse("9017046e-7ec9-4c18-bae1-03eea97c0cb9");
    private static readonly Guid AssignmentSEOId = Guid.Parse("7ca55c99-70ea-4d28-a0c6-d60010bc55c0");

    public static void DropDatabase(AppDbContext context)
    {
        context.Database.EnsureDeleted();
    }

    public static void MigrateDatabase(AppDbContext context)
    {
        context.Database.Migrate();
    }

    public static void SeedIdentity(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        (Guid id, string email, string password) userData = (UserId, "user@app.xyz", "Foo.bar.2");
        (Guid id, string email, string password) adminData = (AdminId, "admin@app.com", "Foo.bar.1");

        var user = userManager.FindByEmailAsync(userData.email).Result;
        if (user == null)
        {
            user = new AppUser
            {
                Id = userData.id,
                Email = userData.email,
                UserName = userData.email,
                FirstName = "User",
                LastName = "App",
                EmailConfirmed = true
            };
            
            var result = userManager.CreateAsync(user, userData.password).Result;
            if (!result.Succeeded)
                throw new ApplicationException("Cannot seed identity User");
        }
        
        var admin = userManager.FindByEmailAsync(adminData.email).Result;
        if (admin == null)
        {
            admin = new AppUser
            {
                Id = adminData.id,
                Email = adminData.email,
                UserName = adminData.email,
                FirstName = "Admin",
                LastName = "App",
                EmailConfirmed = true
            };
            
            var result = userManager.CreateAsync(admin, adminData.password).Result;
            if (!result.Succeeded)
                throw new ApplicationException("Cannot seed identity Admin");
        }
    }

    public static void SeedAppData(AppDbContext context)
    {
        SeedClients(context);
        SeedOrders(context);
        SeedAssignments(context);
        SeedContacts(context);
        SeedComments(context);
        SeedWorkers(context);

        context.SaveChanges();
    }

    private static void SeedClients(AppDbContext context)
    {
        if (context.Clients.Any())
            return;

        context.Clients.Add(new Client
        {
            Id = ClientId,
            Name = "Xyz LTD",
            TotalOrders = 1,
            TotalTasks = 3,
            Contacts = new List<Contact>(),
            Orders = new List<Order>()
        });
    }
    
    private static void SeedContacts(AppDbContext context)
    {
        if (context.Contacts.Any())
            return;

        var contact = context.Contacts.Add(new Contact
        {
            Content = "xyz@mail.com",
            Type = EContactType.Email,
            ClientId = ClientId
        }).Entity;
        
        context.Clients.FirstOrDefault()?.Contacts?.Add(contact);
    }

    private static void SeedOrders(AppDbContext context)
    {
        if (context.Orders.Any())
            return;

        var order = context.Orders.Add(new Order
        {
            Id = OrderId,
            Name = "New Website",
            TotalTasks = 3,
            Start = new DateOnly(2023, 4, 14),
            ClientId = ClientId,
            Assignments = new List<Assignment>()
        }).Entity;
        
        context.Clients.FirstOrDefault()?.Orders?.Add(order);
    }

    private static void SeedAssignments(AppDbContext context)
    {
        if (context.Assignments.Any())
            return;

        context.Assignments.Add(new Assignment
        {
            Id = AssignmentDesignId,
            Title = "Design",
            Description = "Design for webpage",
            Priority = ETaskPriority.Medium,
            Status = ETaskStatus.Pending,
            DueDate = new DateOnly(2023, 4, 20),
            OrderId = OrderId,
            Comments = new List<Comment>(),
            Workers = new List<Worker>()
        });
        
        context.Assignments.Add(new Assignment
        {
            Id = AssignmentFunctionalityId,
            Title = "Functionality",
            Description = "Functionality for webpage",
            Priority = ETaskPriority.High,
            Status = ETaskStatus.Pending,
            DueDate = new DateOnly(2023, 3, 15),
            OrderId = OrderId,
            Comments = new List<Comment>(),
            Workers = new List<Worker>()
        });
        
        context.Assignments.Add(new Assignment
        {
            Id = AssignmentSEOId,
            Title = "SEO",
            Description = "SEO for webpage",
            Priority = ETaskPriority.Low,
            Status = ETaskStatus.Pending,
            DueDate = new DateOnly(2023, 6, 30),
            OrderId = OrderId,
            Comments = new List<Comment>(),
            Workers = new List<Worker>()
        });
    }
    
    private static void SeedComments(AppDbContext context)
    {
        if (context.Comments.Any())
            return;

        var comment = context.Comments.Add(new Comment
        {
            Content = "This is a comment",
            Date = new DateTime(2023, 4, 21, 15, 33, 43).ToUniversalTime(),
            AssignmentId = AssignmentDesignId,
            AppUserId = AdminId
        }).Entity;
        
        context.Assignments.FirstOrDefault()?.Comments?.Add(comment);
    }
    
    private static void SeedWorkers(AppDbContext context)
    {
        if (context.Workers.Any())
            return;

        var worker = context.Workers.Add(new Worker
        {
            AssignmentId = AssignmentDesignId,
            AppUserId = UserId
        }).Entity;
        
        var worker2 = context.Workers.Add(new Worker
        {
            AssignmentId = AssignmentFunctionalityId,
            AppUserId = UserId
        }).Entity;
        
        var worker3 = context.Workers.Add(new Worker
        {
            AssignmentId = AssignmentSEOId,
            AppUserId = UserId
        }).Entity;
        
        context.Assignments.FirstOrDefault(assignment => assignment.Id == AssignmentDesignId)?.Workers?.Add(worker);
        context.Assignments.FirstOrDefault(assignment => assignment.Id == AssignmentFunctionalityId)?.Workers?.Add(worker2);
        context.Assignments.FirstOrDefault(assignment => assignment.Id == AssignmentSEOId)?.Workers?.Add(worker3);
    }
}