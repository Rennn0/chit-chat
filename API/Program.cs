using API.Source;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDependencies(builder.Configuration);

WebApplication app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.DocumentTitle = "ChitChat API";
});

//}

app.UseRouting();
app.UseHttpsRedirection();
app.UseCors("All");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
