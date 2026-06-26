
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UdemyCoreLayer.Configuration;
using UdemyCoreLayer.Entity;
using UdemyCoreLayer.Repositories;
using UdemyCoreLayer.Services;
using UdemyCoreLayer.UnitOfWork;
using UdemyDataLayer;
using UdemyDataLayer.Repositories;
using UdemyServiceLayer.Services;
using UdemySharedLibrary.Configurations;
using UdemySharedLibrary.Services;
using UdemySharedLibrary.Extensions;


JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();


builder.Services.UseCustomValidationResponse();

/*
 “ASP.NET Identity sistemini kur.
Kullanýcý tipi: UserApp
Rol tipi: IdentityRole
Verileri AppDbContext üzerinden DB’ye yaz.”
 
 “Ben ASP.NET Identity kullanacaðým.
User entity’m: UserApp
Role entity’m: IdentityRole”
 “Her kullanýcýnýn email’i benzersiz olsun. Ayný email ile 2 hesap açýlamasýn.”
Özel karakter zorunlu deðil,Rakam zorunlu deðil,Büyük harf zorunlu deðil,Minimum uzunluk: 3 karakter
.AddEntityFrameworkStores<AppDbContext>()
 “Identity tablolarýný AppDbContext üzerinden SQL Server’da tut.”Yani þu tablolar otomatik oluþur:
    AspNetUsers,AspNetRoles,AspNetUserRoles,AspNetUserClaims,AspNetUserLogins,AspNetRoleClaims,AspNetUserTokens
.AddDefaultTokenProviders()Þifre sýfýrlama linki ,Email onay linki altyapýsý hazýr olur.
AddDbContext ? “Veritabanýna nasýl baðlanacaðýmý kur.”
AddIdentity  ? “Kullanýcý, þifre, login, token altyapýsýný kur.”

 
 */
builder.Services.AddIdentity<UserApp, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 3;
}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();






/*
 Burasý “Gelen token’ý nasýl doðrulayacaðým?” ayarlarýnýn yapýldýðý yer.
  options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;Gelen her request’te: “Kullanýcý kim?” sorusu sorulunca
JWT Bearer ile kontrol et demek.
DefaultChallengeScheme:Yetkisiz biri gelirse (401 dönecekse)
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>“JWT geldiðinde, onu þu kurallara göre kontrol et.”
  var tokenOptions=builder.Configuration.GetSection("TokenOption").Get<CustomTokenOption>();Token üretirken kullandýðýn ayarlarla
Token doðrularken kullandýðýn ayarlar ayný olsun
 TokenValidationParameters Bu nesne þunu tanýmlar:“Gelen JWT token GEÇERLÝ mi? Hangi þartlarda kabul edeceðim?”
ValidIssuer = tokenOptions.Issuer,
ValidateIssuer = true,“Token’ýn içindeki Issuer deðeri appsettings’teki Issuer ile ayný olmalý”
ValidAudience = tokenOptions.Audience[0],Audience = Bu token kimin için üretildi?
 IssuerSigningKey = SignService.GetSymmetricSecurityKey(tokenOptions.SecurityKey),
ValidateIssuerSigningKey = true,“Token imzasýný hangi gizli anahtarla doðrulayacaðým?”Ayný SecurityKey ile Token’ýn imzasý çözülüyor..
ValidateLifetime = true,“Token’ýn süresi dolmuþ mu?Bitiþ zamaný geçmiþ mi?
”ClockSkew = TimeSpan.ZeroVarsayýlan olarak 5 dakikalýk bir tolerans süresi var.Bunu sýfýrlýyoruz.“Hiç tolerans istemiyorum
Süresi bittiði an token geçersiz olsun”
Bu bütün blok þunu yapýyor:

 “Uygulamama gelen HER request’te
Authorization: Bearer TOKEN baþlýðý varsa:”

Token’ý al

Ýmzasýný kontrol et (SecurityKey)

Issuer doðru mu bak

Audience doðru mu bak

Süresi dolmuþ mu bak

Hepsi doðruysa: Kullanýcý authenticated olur..
Eðer bu kod YOKSA:Token gönderilse bile Sistem onu okumaz
 */
//“Benim uygulamamda kimlik doðrulama yöntemi olarak JWT Bearer kullanýlacak.”
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    var tokenOptions=builder.Configuration.GetSection("TokenOption").Get<CustomTokenOption>();
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidIssuer = tokenOptions.Issuer,
        ValidAudience = tokenOptions.Audience[0],
        IssuerSigningKey = SignService.GetSymmetricSecurityKey(tokenOptions.SecurityKey),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,

        NameClaimType = ClaimTypes.Name,
        RoleClaimType = ClaimTypes.Role
    };

});


/*
 
 appsettings.json içindeki ayarlarý C# class’larýna otomatik bind eder ve DI (Dependency Injection) container’a ekler.
 builder.Services.Configure<CustomTokenOption>(builder.Configuration.GetSection("TokenOption"));TokenOption bölümünü alýr,CustomTokenOption class’ýna map eder,DI container’a ekler
 builder.Services.Configure<List<Client>>(builder.Configuration.GetSection("Clients"));
 Bu iki satýr, appsettings.json’daki JWT ve Client ayarlarýný C# class’larýna baðlar ve onlarý servislerde güvenli ve temiz þekilde kullanmamýzý saðlar.
 */
builder.Services.Configure<CustomTokenOption>(builder.Configuration.GetSection("TokenOption"));
builder.Services.Configure<List<Client>>(builder.Configuration.GetSection("Clients"));
/*
 Bir sýnýf baþka sýnýfa ulaþmak için nesne üretiyor ve o nesne ile ulaþýyorlar, buna da new’leme diyoruz.
 DI Container þunu yapar:“Bir class baþka bir class’a ihtiyaç duyarsa ben onu senin yerine üretip vereyim.”
 Normalde bir sýnýf baþka bir sýnýfa ulaþmak için onu new’ler ve bu iþleme new’leme denir.DI’de ise bu new’leme iþini sýnýf deðil, DI Container yapar.
 AddScoped kullanýyoruz çünkü her HTTP request için tek bir DbContext, tek bir UnitOfWork ve tek bir Service üretmek istiyoruz; bu da veri tutarlýlýðý ve thread-safety saðlar.
 */

builder.Services.AddScoped<IAuthenticationService , AuthenticationService>();
builder.Services.AddScoped<IUserService , UserService>();
builder.Services.AddScoped<ITokenService , TokenService>();
//typeof kullanýyoruz çünkü burada somut bir servis deðil,generic bir tip þablonu kaydediyoruz;DI container da bunu her T için otomatik kapatýp (<Product>, <User> gibi) üretmek zorunda.
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
//T ve TDto tip parametreleri ile GenericService’i IServiceGeneric arayüzüne baðla...virgüllün analamý:“2 adet generic parametre alan IServiceGeneric interface’i”
/*
 <>  1 generic parametre

<,>  2 generic parametre

<,,>  3 generic parametre

<,,,>  4 generic parametre
 
 
 */
builder.Services.AddScoped(typeof(IServiceGeneric<,>), typeof(GenericService<,>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

/*
 “Ben EF Core kullanýyorum ve SQL Server’a baðlanacaðým.
DbContext’im AppDbContext.
Connection string’i appsettings.json’dan al.”
 “Biri AppDbContext isterse, onu üret ve ver.
Yaþam süresi: Scoped (request baþýna 1 tane).”
 “EF Core migration dosyalarýný UdemyDataLayer projesinde tut.”
 
 */

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"), sqlServerOptions =>
    {
        sqlServerOptions.MigrationsAssembly("UdemyDataLayer");
    });
});




builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHsts();
}
else
{
    app.UserCustomException();
}




//app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllers();


//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}")
//    .WithStaticAssets();


app.Run();
