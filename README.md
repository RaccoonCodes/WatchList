# WatchList

## Overview

WatchList is a Web Project for user to keep track of their favorite series and movies. Currently, this project does not allow to create an account. So there are two Usernames and passwords that you may use to access their respective account (of course this only connects to your local database, so have fun messing around with it!). The users have been seeded when cloning and building this project. This Project allows you to add, edit, and delete series. There is also a "About site" page that has a short description of the website and a little about myself as well.  

The two Usernames & Passwords:

- User: Mapache

   Pass: 123
- User: Ivan

   Pass: 456

## Features
- User Authentication: Checks for valid usernames and passwords.
- Management: Users can add, remove, and update shows in their watchlist.
- Genre Filtering: Filter shows by genre when looking at your full list.
- Navigation bar: After login, you can use the nav bar to navigate to other pages
- Responsive Design: Responsive and works on desktop and mobile devices.

## How it was developed
- ASP.NET Core MVC for back-end
- HTML, CSS, Bootstrap 5 and javascript for front-end
- Entity Framework Core for database management.
- Microsoft SQL Server for database storage.

## Use
- Login with the usernames and passwrods provided.
- Home page after login shows last 10 series recently added
- Manage and add series
- Log out

## Installation
1) Clone the repository to your local machine.
2) Open the solution in Visual Studio.
3) Restore NuGet packages and build the solution.
4) In your appsettings.json, you may update to your connection strings you wish or you may leave it alone. Just as long you have Microsoft SQL Server in your machine.
5) you might need to run the database migration to create the needed tables. If so, run the command, "dotnet ef database update".
6) Build and Run Application

# Behind the Code Overview

The follwoing will explain some parts of the code in detail.

## Models
SeriesInfo.cs and UsersDBModels.cs are both Tables that are part of one database schema for Entity Framework. Both are then set and created through InfoDBContext.cs Each of the tables have validations and validation format through regular expression respectively on each property and methods.

### SeriesInfo
This table is the schema for series informations such as SeriesInfoID, UserID, Title, Season, Provider, and Genre.

```csharp
public class SeriesInfo
{
    [Key]
    public int SeriesInfoID {  get; set; }
    public int UserID { get; set; } //Foreign Key 
    public UsersDBModel? User { get; set; } //Navigaton, allow access related UsersDBModel objects.

    [Required(ErrorMessage ="Please enter Title")]
    public string TitleWatched { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter Season")]
    [RegularExpression(@"^\d+$",ErrorMessage ="Season must contain only whole numbers")]
    public string SeasonWatched { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter Provider")]
    public string ProviderWatched { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter the Genre of the series")]
    public string Genre { get; set; } = string.Empty;

}
```

- SeriesInfoID: An int property with the [Key] attribute. This property uniquely identifies each series entry.
- User: A navigation property representing the related UsersDBModel object. This allow for easy access to related objects in entity framework.
- UserID: An int property representing the foreign key. This property establishes a relationship between the SeriesInfo and UsersDBModel tables in the database. The Foreign key association is done in InfoDbContext.cs

### UserDBModel
This table is the structure of a user such as their username, password, and a collection of series they have watched.

```csharp
public class UsersDBModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserID { get; set; }
    public string UserName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public ICollection <SeriesInfo>? SeriesInfos { get; set; }
}
```

- public int UserID: An int property marked with the [Key] attribute. Additionally, it's decorated with the [DatabaseGenerated(DatabaseGeneratedOption.Identity)] attribute. This lets the database automatically generate values for this property for unique values.

### InfoDbContext

This serves, as the entry point for interacting with the database in the WatchList application and defines the database schema using entity configurations. InfoDbContext class inherits from DbContext, which is provided by Entity Framework Core for interacting with the database.

```csharp
public class InfoDbContext:DbContext
{
    public InfoDbContext(DbContextOptions<InfoDbContext> options) : base(options) { }

    public DbSet<SeriesInfo> SeriesInfos =>Set<SeriesInfo>();

    public DbSet<UsersDBModel> UsersDBModels =>Set<UsersDBModel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SeriesInfo>()
            .HasOne(s => s.User) // Each SeriesInfo has one User
            .WithMany(u => u.SeriesInfos) // Each User can have many SeriesInfos
            .HasForeignKey(s => s.UserID); // The foreign key is UserID in SeriesInfo
    }
}
```

- SeriesInfos: A collection of SeriesInfo entities in the database. It's defined using the DbSet<T> class, which is a property of the DbContext class. This will map to a database table named "SeriesInfos".
- UsersDBModels: Similar to SeriesInfos, it is a collection of UsersDBModel entities in the database. It's also defined using the DbSet<T> class and will map to a table named "UsersDBModels"
- OnModelCreating: This method overrides the OnModelCreating method inherited from DbContext. It's called when the model for the context is being created.

In this method, entity configurations are specified using the ModelBuilder object: 
- The Entity<SeriesInfo>() method configures the SeriesInfo entity.
- The HasOne method specifies the relationship between SeriesInfo and UsersDBModel, indicating that each SeriesInfo has one User.
- The WithMany method specifies that each User can have many SeriesInfos.
- The HasForeignKey method sets the foreign key property in the SeriesInfo entity to UserID, linking it to the primary key of the UsersDBModel entity.

### IInfoRepository Interface

Reason I use Interface is that it promotes loose coupling, dependency inversion, and flexibility in the design of the application. It allows for easier testing, swapping of implementations, and overall better maintainability of the codebase.

```csharp
public interface IInfoRepository
{
    IQueryable<SeriesInfo> SeriesInfos { get; }

    void SaveInfo(SeriesInfo series);
    void CreateInfo(SeriesInfo series);
    void DeleteInfo(SeriesInfo series);

    SeriesInfo? GetSeriesInfoByID(int seriesInfoID, bool includeUser = false);

    SeriesInfo? GetSeriesInfoByAttributes(SeriesInfo seriesInfo);

    int GetLastSeriesInfoID();
    int GetTotalItemCount(int userID);
    
```
  
- SeriesInfos: An IQueryable property representing a collection of series information. This allows querying and accessing series data.
- GetSeriesInfoByID(seriesInfoID, includeUser): Retrieves a series information object by its ID. Optionally includes user information if specified.
- GetSeriesInfoByAttributes(seriesInfo): Retrieves a series information object based on specified attributes.

### EFInfoRepository Class
This class is responsible for providing access to series information stored in a database using Entity Framework Core. This is also tied with IInfoRepository Interface
```csharp
 public class EFInfoRepository : IInfoRepository
 {
     private InfoDbContext context;

     public EFInfoRepository(InfoDbContext context) {  
         this.context = context; 
     }

    public IQueryable<SeriesInfo> SeriesInfos => context.SeriesInfos;

     
     public void SaveInfo(SeriesInfo series) { 
         context.Update(series);
         context.SaveChanges();
     }

     public void DeleteInfo(SeriesInfo series) {  
         context.Remove(series); 
         context.SaveChanges();
     }

     public void CreateInfo(SeriesInfo series) {  
         context.Add(series);
         context.SaveChanges();
     }
     
     public int GetTotalItemCount(int userID)
     {
         return context.SeriesInfos.Count(s => s.UserID == userID);
     }

     public int GetLastSeriesInfoID()
     {
         int lastSeriesInfoID = context.SeriesInfos
             .OrderByDescending(s => s.SeriesInfoID).Select(s => s.SeriesInfoID).FirstOrDefault();
         return lastSeriesInfoID;        
     }

     public SeriesInfo? GetSeriesInfoByID(int seriesInfoID, bool includeUser = false)
     {
         if (includeUser)
         {
             return context.SeriesInfos
                 .Include(s => s.User)
                 .FirstOrDefault(s => s.SeriesInfoID == seriesInfoID);
         }
         else
         {
             return context.SeriesInfos
                 .FirstOrDefault(s => s.SeriesInfoID == seriesInfoID);
         }
     }

     public SeriesInfo? GetSeriesInfoByAttributes(SeriesInfo seriesInfo)
     {
         return context.SeriesInfos
        .FirstOrDefault(s =>
            s.TitleWatched == seriesInfo.TitleWatched &&
            s.SeasonWatched == seriesInfo.SeasonWatched &&
            s.ProviderWatched == seriesInfo.ProviderWatched &&
            s.Genre == seriesInfo.Genre &&
            s.UserID == seriesInfo.UserID);
     }

 }
    
```

1) Dependency Injection: The class constructor takes an instance of InfoDbContext as a parameter, allowing it to interact with the database context.

2) CRUD Operations: It provides methods to perform CRUD (Create, Read, Update, Delete) operations on series information:
- SaveInfo: Updates an existing series information.
- DeleteInfo: Deletes a series information.
- CreateInfo: Creates a new series information.

3) Query Operations:
- GetTotalItemCount: Retrieves the total count of series items for a specific user.
- GetLastSeriesInfoID: Retrieves the ID of the last series information added to the database.
- GetSeriesInfoByID: Retrieves series information by its ID, optionally including related user information.
- GetSeriesInfoByAttributes: Retrieves series information by its attributes such as title, season, provider, genre, and user ID.
- Entity Framework Integration: The class leverages Entity Framework Core's features such as LINQ queries and change tracking to interact with the database.

### PagingInfo
This Focuses on pagination information used to manage and display paginated data in the application's user interface.
```csharp
 public class PagingInfo
 {
     public int TotalItems { get; set; }
     public int ItemsPerPage { get; set; }
     public int CurrentPage {  get; set; }
     public int TotalPage => (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage); 
 }
```

- TotalPage: Determines the total number of pages needed to display all items based on the total items and items per page.
  
The TotalPage property calculates the total number of pages required to display all available items. It uses the formula (TotalItems / ItemsPerPage), rounding up to the nearest whole number using Math.Ceiling.

## Views
I will only mention the Partial views and one of the view called "FullList.cshtml"
### Partial Views 

##### _NavigationPartial
This a partial view for a Navigationbar to be reused in other Views without needing to code it again. Easy maintainability and following DRY priniciple. 

```csharp
<nav class="navbar navbar-expand-sm bg-dark">
    <div class="container-fluid">

        <span class="navbar-text">Welcome @Model?.LoginModel.LoginName !</span>

        <ul class="navbar-nav mx-auto ">
            <li class="nav-item hovereffect">
                <a class="nav-link text-white-50" asp-action="WatchList">Home</a>
            </li>
            <li class="nav-item hovereffect">
                <a class="nav-link text-white-50" asp-action="FilterByGenre">Full List</a>
            </li>
            <li class="nav-item hovereffect">
                <a class="nav-link text-white-50" asp-action="Add">Add Series</a>
            </li>
            <li class="nav-item hovereffect">
                <a class="nav-link text-white-50" asp-action="AboutSite">About site</a>
            </li>
        </ul>
        <a class="btn btn-danger" asp-action="Logout">Log Out</a>
    </div>
</nav>
```

This navigation bar contains Home, Full List, Add Series, About Site, and Logout. With each of the item linked respectively to another view via asp-action tags.

#### _WatchListTable
This partial view provides a user-friendly interface for viewing and interacting with a list of series information, allowing users to edit or delete series entries as needed

```csharp
@model IEnumerable<WatchList.Models.SeriesInfo>

<table class="table table-dark table-hover my-2">
    <thead>
        <tr>
            <th>Title</th>
            <th>Season</th>
            <th>Provider</th>
            <th>Genre</th>
            <th></th>
        </tr>
    </thead>

    <tbody>
        @foreach (var info in Model)
        {
            <tr>
                <td>@info.TitleWatched</td>
                <td>@info.SeasonWatched</td>
                <td>@info.ProviderWatched</td>
                <td>@info.Genre</td>
                <td>
                    <a class="btn btn-outline-info my-2" asp-action="Edit" asp-route-infoID="@info.SeriesInfoID">Edit</a>
                    <a class="btn btn-outline-danger" asp-action="Delete" asp-route-infoID="@info.SeriesInfoID">Delete</a>
                </td>
            </tr>
        }
    </tbody>
</table>
```
- Model Declaration: Strongly typed to accept an enumerable collection of SeriesInfo objects.
- Attribute: The "asp-action" attribute specifies the controller action to be invoked when an action button is clicked, and the "asp-route-infoID" attribute passes the SeriesInfoID to the controller action for identification when choosing Edit or Delete. 

#### FullList
This view provides a user-friendly interface for users to filter and view their list of series, with support for pagination to navigate through large lists.

```csharp
@model FinalViewModel
@using Microsoft.AspNetCore.Http

@{
    Layout = null;
}

<!DOCTYPE html>
<html>
<head>
    <title>Your List</title>
    <link rel="stylesheet" href="/lib/bootstrap/dist/css/bootstrap.css" />
    <link rel="stylesheet" href="~/css/Styles.css" />
</head>
<body>

    <partial name="_NavigationPartial" model="Model" />

    <div class="container-fluid">
        <div class="row p-2">
            <div class="col-3">
                <div class="d-grid gap-1">
                    <h3 class="text-center">Filter by</h3>
                    <form method="get" asp-action="FilterByGenre">
                        <button type="submit"
                                class="btn @(Model.SelectedGenre == "All" ? "btn-success active"
                                : "btn-primary") btn-length my-1" name="genre" value="All">
                            All
                        </button>
                        @if (Model != null && Model.InfoRepository != null)
                        {
                            @foreach (var genre in Model.InfoRepository.SeriesInfos
                           .Where(s => s.UserID == Model.LoginModel.UserID)
                           .Select(s => s.Genre)
                           .Distinct())
                            {
                                <button type="submit"
                                        class="btn @(Model.SelectedGenre == genre ? "btn-success active"
                                        : "btn-primary") btn-length my-1" name="genre" value="@genre">
                                    @genre
                                </button>
                            }
                        }

                    </form>
                </div>
            </div>

            <div class="col">
                <h3 class="text-center">Current List</h3>
                @await Html.PartialAsync("_WatchListTablePartial", Model?.FilteredSeries ?? Enumerable.Empty<SeriesInfo>())
                <div class="pagination">
                    <ul class="pagination justify-content-center">
                        @for (int x = 1; x <= Model?.PagingInfo.TotalPage; x++)
                        {
                            <li class="@(x == Model.PagingInfo.CurrentPage ? "active" : "") page-item">
                                <a class="page-link" href="@Url.Action("FilterByGenre", new { listPage = x, genre = Model.SelectedGenre})">@x</a>
                            </li>
                        }
                    </ul>
                </div>
            </div>

        </div>
    </div>

</body>
</html>
```
- Model Declaration: The view is strongly typed to accept a FinalViewModel object.
- Navigation Partial: The _NavigationPartial partial view is rendered, passing the Model object.
- Filter Section: A section on the left side allows users to filter the list by genre. It includes buttons for each genre, with an option to filter by "All." Genre buttons are generated based on available genres in the user's list.
- Pagination: Below the list, pagination links are displayed to navigate through multiple pages of the list. Pagination links are generated dynamically based on the total number of pages (TotalPage) in the PagingInfo object.

## Controllers

### AccountController
This file has long lines of codes, please refer to AccountController.cs to view the full code. I will only display parts of codes below. 

```csharp
 public AccountController(UserServices userServices,IInfoRepository infoRepository)
 {
     _userServices = userServices;
     _infoRepository = infoRepository;
 }
```
- Dependencies Injection: The controller is injected with instances of UserServices and IInfoRepository, which are used for user authentication and accessing series information, respectively.
```csharp
[HttpPost]
public IActionResult Login(LoginModel loginModel)
{
    if (!ModelState.IsValid)
        return View(loginModel);

    if (!ValidateCredentials(ref loginModel))
    {
        ModelState.AddModelError("","Invalid Username or Password");
        return View(loginModel);
    }
   
    _loginModel = loginModel;

 return RedirectToAction("WatchList");

}
```
