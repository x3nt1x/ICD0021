using App.BLL;
using App.Contracts.BLL;
using App.DAL;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Tests.Unit;

public class ClientServiceTests
{
    private readonly IAppBll _appBll;
    private readonly AppDbContext _ctx;

    // arrange - common
    public ClientServiceTests()
    {
        // set up db context for testing - using InMemory db engine
        var optionBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionBuilder.EnableSensitiveDataLogging();
        
        // provide new random database name here
        optionBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        
        _ctx = new AppDbContext(optionBuilder.Options);
        _ctx.Database.EnsureDeleted();
        _ctx.Database.EnsureCreated();
        _ctx.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        _ctx.ChangeTracker.AutoDetectChangesEnabled = false;
       
        var dalMapperConfig = GetDalMapperConfiguration();
        var bllMapperConfig = GetBllMapperConfiguration();

        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

        _appBll = new AppBll(new AppUnitOfWork(_ctx, new Mapper(dalMapperConfig)), new Mapper(bllMapperConfig));
    }

    [Fact]
    public async Task Action_Test__Add()
    {
        // arrange
        var client = new App.BLL.DTO.Client
        {
            Name = "Test_Client",
            TotalOrders = 2,
            TotalTasks = 5
        };

        // act
        var newClient = _appBll.Clients.Add(client);
        await _appBll.SaveChangesAsync();

        // assert
        var response = _ctx.Clients.FirstOrDefault(e => e.Id == newClient.Id);

        Assert.NotNull(response);
        Assert.Equal(newClient.Id, response.Id);
        Assert.Equal(client.Name, response.Name);
        Assert.Equal(client.TotalOrders, response.TotalOrders);
        Assert.Equal(client.TotalTasks, response.TotalTasks);
    }

    [Fact]
    public async Task Action_Test__Update()
    {
        // arrange
        var client = _ctx.Clients.Add(new App.Domain.Client
        {
            Name = "Test_Client",
            TotalOrders = 2,
            TotalTasks = 5
        });

        var updateClient = new App.BLL.DTO.Client
        {
            Id = client.Entity.Id,
            Name = "Update_Test_Client",
            TotalOrders = 3,
            TotalTasks = 8
        };

        await _ctx.SaveChangesAsync();
        client.State = EntityState.Detached;

        // act
        var newClient = _appBll.Clients.Update(updateClient);
        await _appBll.SaveChangesAsync();

        // assert
        var response = _ctx.Clients.FirstOrDefault(e => e.Id == client.Entity.Id);

        Assert.NotNull(response);
        Assert.Equal(updateClient.Id, response.Id);
        Assert.Equal(updateClient.Name, response.Name);
        Assert.Equal(updateClient.TotalOrders, response.TotalOrders);
        Assert.Equal(updateClient.TotalTasks, response.TotalTasks);

        Assert.Equal(newClient.Id, response.Id);
        Assert.Equal(newClient.Name, response.Name);
        Assert.Equal(newClient.TotalOrders, response.TotalOrders);
        Assert.Equal(newClient.TotalTasks, response.TotalTasks);
    }

    [Fact]
    public async Task Action_Test__Remove()
    {
        // arrange
        var client = _ctx.Clients.Add(new App.Domain.Client
        {
            Name = "Test_Client",
            TotalOrders = 2,
            TotalTasks = 5
        });

        await _ctx.SaveChangesAsync();
        client.State = EntityState.Detached;

        // act
        _appBll.Clients.Remove(client.Entity.Id);
        await _appBll.SaveChangesAsync();

        // assert
        Assert.Null(_ctx.Clients.FirstOrDefault(e => e.Id == client.Entity.Id));
    }

    [Fact]
    public async Task Action_Test__FirstOrDefault()
    {
        // arrange
        var client = _ctx.Clients.Add(new App.Domain.Client()
        {
            Name = "Test_Client",
            TotalOrders = 2,
            TotalTasks = 5
        });

        await _ctx.SaveChangesAsync();

        // act
        var response = await _appBll.Clients.FirstOrDefaultAsync(client.Entity.Id);

        // assert
        Assert.NotNull(response);
        Assert.Equal(client.Entity.Id, response.Id);
    }

    [Fact]
    public async Task Action_Test__GetAll()
    {
        var client = _ctx.Clients.Add(new App.Domain.Client
        {
            Name = "Test_Client",
            TotalOrders = 2,
            TotalTasks = 5
        });

        var client2 = _ctx.Clients.Add(new App.Domain.Client
        {
            Name = "Test_Client_2",
            TotalOrders = 6,
            TotalTasks = 8
        });

        await _ctx.SaveChangesAsync();

        // ACT
        var response = await _appBll.Clients.AllAsync();

        // Assert
        var clients = response.ToList();
        Assert.NotNull(clients.FirstOrDefault(e => e.Id == client.Entity.Id));
        Assert.NotNull(clients.FirstOrDefault(e => e.Id == client2.Entity.Id));
    }

    private static MapperConfiguration GetBllMapperConfiguration()
    {
        return new MapperConfiguration(config =>
        {
            config.CreateMap<App.BLL.DTO.Assignment, App.DAL.DTO.Assignment>().ReverseMap();
            config.CreateMap<App.BLL.DTO.Client, App.DAL.DTO.Client>().ReverseMap();
            config.CreateMap<App.BLL.DTO.Comment, App.DAL.DTO.Comment>().ReverseMap();
            config.CreateMap<App.BLL.DTO.Contact, App.DAL.DTO.Contact>().ReverseMap();
            config.CreateMap<App.BLL.DTO.Order, App.DAL.DTO.Order>().ReverseMap();
            config.CreateMap<App.BLL.DTO.Worker, App.DAL.DTO.Worker>().ReverseMap();
        });
    }

    private static MapperConfiguration GetDalMapperConfiguration()
    {
        return new MapperConfiguration(config =>
        {
            config.CreateMap<App.DAL.DTO.Assignment, App.Domain.Assignment>().ReverseMap();
            config.CreateMap<App.DAL.DTO.Client, App.Domain.Client>().ReverseMap();
            config.CreateMap<App.DAL.DTO.Comment, App.Domain.Comment>().ReverseMap();
            config.CreateMap<App.DAL.DTO.Contact, App.Domain.Contact>().ReverseMap();
            config.CreateMap<App.DAL.DTO.Order, App.Domain.Order>().ReverseMap();
            config.CreateMap<App.DAL.DTO.Worker, App.Domain.Worker>().ReverseMap();
        });
    }
}