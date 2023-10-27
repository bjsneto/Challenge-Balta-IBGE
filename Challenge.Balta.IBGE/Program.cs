using AutoMapper;
using Challenge.Balta.IBGE.Auth;
using Challenge.Balta.IBGE.DependencyInjection;
using Challenge.Balta.IBGE.Model;
using Challenge.Balta.IBGE.Swagger;
using Chanllenge.Balta.IBGE.Domain.Entities;
using Chanllenge.Balta.IBGE.Domain.Interfaces;
using FluentValidation;
using Swashbuckle.AspNetCore.Annotations;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureAuthenticationAndAuthorization(configuration);
builder.Services.AddControllers();
builder.Services.ConfigureDependencies(configuration);
builder.Services.AddLogging(builder => builder.AddConsole());

var app = builder.Build();

app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Challenge Balta IBGE");
});

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

#region Location CRUD
app.MapPost("/api/ibge/upload", async (IValidator<IFormFile> validator, IFormFile arquivo, ILocalityService _localityService, ILogger<Program> logger) =>
{
    try
    {
        var validationResult = await validator.ValidateAsync(arquivo);

        if (!validationResult.IsValid)
            return Results.ValidationProblem(validationResult.ToDictionary());

        var rowsInsert = await _localityService.ProcessExcelFileAsync(arquivo);

        return Results.Ok(new
        {
           Result = rowsInsert == 0 
           ? "No data was entered. All data has already been imported. No data was entered. All data has already been imported." 
           :  $"{rowsInsert} Localities saved successfully!"
        });
    }
    catch (Exception ex)
    {
        logger.LogError($"An exception occurred while processing the upload endpoint: {ex.Message}");
        return Results.BadRequest(ex.Message);
    }
})
.WithMetadata(new SwaggerOperationAttribute(
    "Excel File Upload",
    "Upload an Excel file containing information about states and cities. This endpoint allows you to import locality data from an Excel file."
))
.WithTags(new[] { "IBGE" })
.RequireAuthorization();

app.MapPut("/api/ibge/update", async (IValidator<IbgeModel> validator, IMapper mapper, ILocalityService _localityService, IbgeModel model, ILogger<Program> logger) =>
{
    try
    {
        var validationResult = await validator.ValidateAsync(model);

        if (!validationResult.IsValid)
            return Results.ValidationProblem(validationResult.ToDictionary());

        var ibge = mapper.Map<Ibge>(model);
        await _localityService.Update(ibge);
        return Results.Ok("Location updated successfully");
    }
    catch (Exception ex)
    {
        logger.LogError($"An exception occurred while processing the locality update endpoint: {ex.Message}");
        return Results.BadRequest(ex.Message);
    }
})
.WithMetadata(new SwaggerOperationAttribute(
    "Update Locale",
    "Updates a location. This endpoint allows you to modify an existing location's information. Provide the following properties in the request body: 'Id' (string), 'State' (string), 'City' (string)."
))
.WithTags(new[] { "IBGE" })
.RequireAuthorization();

app.MapDelete("/api/ibge/delete", async (IMapper mapper, ILocalityService _localityService, string code, ILogger<Program> logger) =>
{
    try
    {
        await _localityService.Delete(code);
        return Results.Ok("Successfully deleted location");
    }
    catch (Exception ex)
    {
        logger.LogError($"An exception occurred while processing the locality delete endpoint: {ex.Message}");
        return Results.BadRequest(ex.Message);
    }
})
.WithMetadata(new SwaggerOperationAttribute(
    "Delete a Location",
    "Removes a location by its IBGE code. This endpoint allows you to delete a location from the system based on its unique IBGE code."
))
.WithTags(new[] { "IBGE" })
.RequireAuthorization();

app.MapGet("/api/ibge/search", async (IMapper mapper, ILocalityService _localityService, string search, ILogger<Program> logger) =>
{
    try
    {
        var result = await _localityService.Search(search);

        if (!result.Any())
            return Results.NotFound("No results found for the specified search.");

        var ibge = mapper.Map<List<IbgeModel>>(result);
        return Results.Ok(ibge);
    }
    catch (Exception ex)
    {
        logger.LogError($"An exception occurred while processing the locality search endpoint: {ex.Message}");
        return Results.BadRequest(ex.Message);
    }
})
.WithMetadata(new SwaggerOperationAttribute(
    "Search Location",
    "Search for locations by Municipality, State, or City code. This endpoint allows you to retrieve locations based on the provided search criteria. Use the 'search' parameter to specify the search term."
))
.WithTags(new[] { "IBGE" })
.RequireAuthorization();


app.MapPost("/api/ibge/create", async (IValidator<IbgeModel> validator, IMapper mapper, ILocalityService _localityService, IbgeModel model, ILogger<Program> logger) =>
{
    try
    {
        var validationResult = await validator.ValidateAsync(model);

        if (!validationResult.IsValid)
            return Results.ValidationProblem(validationResult.ToDictionary());

        var ibge = mapper.Map<Ibge>(model);
        await _localityService.Insert(ibge);

        return Results.Ok("Location created successfully");
    }
    catch (Exception ex)
    {
        logger.LogError($"An exception occurred while processing the locality create endpoint: {ex.Message}");
        return Results.BadRequest(ex.Message);
    }
})
.WithMetadata(new SwaggerOperationAttribute(
    "Create Locale",
    "Creates a new locality. This endpoint is used to add a new locality to the system. Provide the following properties in the request body: 'Id' (string), 'State' (string), 'City' (string)."
))
.WithTags(new[] { "IBGE" })
.RequireAuthorization();

#endregion

#region Login and User Creation
app.MapPost("/api/user/login", async (IValidator<LoginModel> validator, IJwtTokenService jwtTokenService, IAuthService authService, LoginModel model, ILogger<Program> logger) =>
{
    try
    {
        var validationResult = await validator.ValidateAsync(model);

        if (!validationResult.IsValid)
            return Results.ValidationProblem(validationResult.ToDictionary());

        var signInResult = await authService.LoginUserAsync(model.Email, model.Password);

        if (!signInResult.Succeeded)
            return Results.BadRequest("Invalid email or password.");

        var stringToken = jwtTokenService.GenerateToken(model.Email);
        return Results.Ok(new
        {
            Token = stringToken,
            Email = model.Email
        });
    }
    catch (Exception ex)
    {
        logger.LogError($"An exception occurred while processing the user login endpoint: {ex.Message}");
        return Results.BadRequest(ex.Message);
    }
})
.WithMetadata(new SwaggerOperationAttribute(
    "User Login",
    "Logs the user in and returns a JWT token if the login is successful. This endpoint is used for user authentication by providing valid email and password credentials."
))
.WithTags(new[] { "Authentication" })
.AllowAnonymous();

app.MapPost("/api/user/create", async (IValidator<CreateUserModel> validator, IAuthService authService, CreateUserModel model, ILogger<Program> logger) =>
{
    try
    {
        var validationResult = await validator.ValidateAsync(model);

        if (!validationResult.IsValid)
            return Results.ValidationProblem(validationResult.ToDictionary());

        var signInResult = await authService.RegisterUserAsync(model.Email, model.Password);

        if (signInResult.Succeeded)
            return Results.Ok("User created successfully");

        return Results.BadRequest(signInResult.Errors);
    }
    catch (Exception ex)
    {
        logger.LogError($"An exception occurred while processing the create user endpoint: {ex.Message}");
        return Results.BadRequest(ex.Message);
    }
})
.WithMetadata(new SwaggerOperationAttribute(
    "Create a user",
    "Allows you to create a new user in the application. This endpoint is used to register new users in the application by providing the required email and password information."
))
.WithTags(new[] { "Authentication" })
.AllowAnonymous();
#endregion

app.Run();
