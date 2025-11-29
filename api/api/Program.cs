// using api.Data;
// using api.Services;
// using Microsoft.AspNetCore.Authentication.Google;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Configuration;
// using Microsoft.IdentityModel.Tokens;
// using System.Security.Claims;
// using System.Text;

// var builder = WebApplication.CreateBuilder(args);


// // cors
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowSpecificOrigin",
//         builder =>
//         {
//             builder.WithOrigins("http://localhost:8080")
//             .AllowAnyHeader()
//             .AllowAnyMethod()
//             .AllowCredentials();
//         });
// });
// // Add services to the container.
// builder.Services.AddDbContext<AppDbContext>(options=>
// options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// builder.Services.AddScoped<IVnPayService, VnPayService>();// C?u h�nh jwt


// builder.Services.AddAuthentication(options =>
// {
//     // X�c ??nh c? ch? x�c th?c
//     options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//     // X�c ??nh x? l� y�u c?u t�nh h?p l?
//     options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
// })
// .AddJwtBearer(options =>
// {
//     options.TokenValidationParameters = new TokenValidationParameters
//     {
//         ValidateIssuer = true,
//         ValidateAudience = true,
//         ValidateLifetime = true,
//         ValidateIssuerSigningKey = true,
//         ValidIssuer = builder.Configuration["Jwt:Issuer"],
//         ValidAudience = builder.Configuration["Jwt:Audience"],
//         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"])),
//         RoleClaimType = ClaimTypes.Role
//     };
// }
// );

// builder.Services.AddAuthorization(options =>
// {
//     options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
//     options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
// });


// builder.Services.AddControllers();
// // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

// var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// app.UseHttpsRedirection();

// app.UseRouting();
// app.UseCors("AllowSpecificOrigin");

// app.Use(async (context, next) =>
// {
//     // Custom middleware logic
//     await next.Invoke();
// });
// app.UseAuthentication();
// app.UseAuthorization();

// app.MapControllers();

// app.Run();
using api.Data;
using api.Services;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()    // Cho phép mọi domain
            .AllowAnyHeader()    // Cho phép mọi header
            .AllowAnyMethod();   // Cho phép GET, POST, PUT, DELETE,...
    });
});

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IVnPayService, VnPayService>();

// Cấu hình JWT (giữ nguyên)
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"])),
        RoleClaimType = ClaimTypes.Role
    };
});

// app.UseHttpsRedirection();  // Comment tạm cho dev, tránh redirect HTTP -> HTTPS
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
});

builder.Services.AddControllers();
// Swagger (giữ nguyên)
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();



var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

app.UseRouting();  // Thứ tự: Routing trước CORS



// Kích hoạt CORS
app.UseCors("AllowAll");

// Middleware custom (giữ nguyên)
//app.Use(async (context, next) =>
//{
//    await next.Invoke();
//});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
