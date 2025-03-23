using ApplicationApi.Services;
using Domain.Filters;
using Domain.Interfaces;
using Infraestrutura.Context;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Infraestrutura.Repositories;
using Infraestrutura.PadraoUnitOfWork;
using Domain.Otimizacao;
using LojinhaApi.ApiExceptionMiddlaware;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Domain.Entities;
using Microsoft.OpenApi.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using LojinhaApi.RateLimit;
using Asp.Versioning;
using ApplicationApi.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

string mySqlConnection = builder.Configuration.GetConnectionString("ConexaoPadrao");

builder.Services.AddDbContext<LojinhaContext>(options =>
{
    options.UseMySql(mySqlConnection,ServerVersion.AutoDetect(mySqlConnection));
});




builder.Services.AddIdentity<ApplicationUser, IdentityRole>()    // substitui IdentityUser por ApplicationUser que herda de IdentityUser 
    .AddEntityFrameworkStores<LojinhaContext>()
    .AddDefaultTokenProviders();





builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ApiExceptionFilter));

}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
}).AddNewtonsoftJson();




builder.Services.AddRateLimiter(rateOptions =>
{
    rateOptions.AddFixedWindowLimiter("fixed", options =>
    {
        options.PermitLimit = 3;
        options.Window = TimeSpan.FromSeconds(10);
         options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 1;

    });
    rateOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});



var myRate = new RateLimitOptions();

builder.Configuration.GetSection(RateLimitOptions.MyRateLimit).Bind(myRate);


builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpcontext =>
                            RateLimitPartition.GetFixedWindowLimiter(
                                                partitionKey: httpcontext.User.Identity?.Name ??
                                                httpcontext.Request.Headers.Host.ToString(),
                            factory: partition => new FixedWindowRateLimiterOptions
                            {
                                AutoReplenishment = true,
                                PermitLimit = 5,
                                QueueLimit = 0,
                                Window = TimeSpan.FromSeconds(10)
                            }));
});






builder.Services.AddApiVersioning(o =>
{
    o.DefaultApiVersion = new ApiVersion(1, 0);
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.ReportApiVersions = true;
    o.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader("api-version"), new UrlSegmentApiVersionReader());
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV"; 
    options.SubstituteApiVersionInUrl = true;
});








builder.Services.AddAutoMapper(typeof(AutoMapperApplication));

builder.Services.AddScoped<ICategoriaCache, CategoriaCacheService>();
builder.Services.AddScoped<IProdutoCache, ProdutoCacheService>();

builder.Services.AddScoped<IUnitOfWork, PadraoUnitOfWork>();
builder.Services.AddScoped<LojinhaContext>();

builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();  
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IUserModelDTO, UserModelDTORepository>();
builder.Services.AddScoped<ITokenService,TokenServices>();
builder.Services.AddScoped<AuthServices>();
builder.Services.AddScoped<ProdutoServices>();
builder.Services.AddScoped<CategoriaServices>();
builder.Services.AddScoped<CategoriaCacheService>();
builder.Services.AddScoped<ProdutoCacheService>();





builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<Program>>();

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"])
        )
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            logger.LogError("Autenticação falhou: {Message}", context.Exception.Message);
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            logger.LogInformation("Token validado com sucesso para o usuário: {User}", context.Principal?.Identity?.Name);
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            logger.LogWarning("Desafio de autorização falhou. Detalhes: {Error}", context.ErrorDescription);
            return Task.CompletedTask;
        },
        OnMessageReceived = context =>
        {
            logger.LogInformation("Token recebido: {Token}", context.Token);
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddSwaggerGen(c =>
{
    //c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "APICatalogo",
        Version = "v1",
        Description = "Catalogo de produtos e categorias",
        TermsOfService = new Uri("https://arthur.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Arthur Guerra",
            Email = "arthurflavioguerra@gmail.com",
            Url = new Uri("https://www.arthur.net"),
        },
        License = new OpenApiLicense
        {
            Name = "usar sobre LICX",
            Url = new Uri("https://arthur.net/license"),
        }
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Insira 'Bearer' [espaço] e o token JWT",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey, 
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin","SuperAdmin"));
    options.AddPolicy("Super", policy => policy.RequireRole("SuperAdmin").RequireClaim("id","Cigarrim"));
    options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));

    options.AddPolicy("ExclusiveOnly", policy => policy.RequireAssertion(context => context.User.HasClaim(claim => claim.Type == "id" &&
        claim.Value == "cigarrim") ||
        context.User.IsInRole("SuperAdmin")));

});




builder.Services.AddCors(options =>
{
    options.AddPolicy("Politica1",
        policy =>
        {
            policy.WithOrigins("https://localhost:7239")
            .WithMethods("GET", "POST")
            .AllowAnyHeader();
        });

    options.AddPolicy("Politica2",
        policy =>
        {
            policy.WithOrigins("")      //https://apirequest.io
            .WithMethods("POST", "GET")
            .AllowAnyHeader();
        });
});


//builder.Services.AddCors(options =>
//{
//    options.AddDefaultPolicy(
//        policy =>
//        {
//            policy.WithOrigins("https://")
//            .WithMethods("GET", "POST")
//            .AllowAnyHeader();
//        });
//});




builder.Services.AddEndpointsApiExplorer();


//builder.Services.AddMemoryCache(options =>
//{
//    options.SizeLimit = 1024;
//});

builder.Services.AddMemoryCache();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.ConfigureExcepetionHandler();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    // habilita o middleware para servir o swagger 
//    // gerado como um endpoint JSON
//    app.UseSwagger();
//    //habilita o middleware de arquivos estaticos
//    app.UseSwaggerUI();
//    app.ConfigureExcepetionHandler();
//}





app.UseHttpsRedirection();

app.UseStaticFiles();  
app.UseRouting();


app.UseRateLimiter();



//app.UseCors(OrigensComAcessoPermitido);
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
