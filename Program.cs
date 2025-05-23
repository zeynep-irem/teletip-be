using Google.Cloud.Firestore.V1;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Teletipbe.Services;
using Teletipbe.Hubs;
using Microsoft.AspNetCore.Builder;
using System.Net;

// Top-level statements

var builder = WebApplication.CreateBuilder(args);

FirebaseApp.Create(new AppOptions
{
    Credential = GoogleCredential.FromFile("ServiceAccount/teletipbe-firebase-adminsdk-e19td-4746d67c8f.json"),
});

// 1) Firebase kimlik do�rulama dosyas�
string path = "ServiceAccount/teletipbe-firebase-adminsdk-e19td-4746d67c8f.json";
Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

// 2) Firestore ve servislerin DI kayd�
builder.Services.AddSingleton<FirebaseService>();
builder.Services.AddSingleton(provider => FirestoreDb.Create("teletipbe"));
builder.Services.AddSingleton<AppointmentService>();
builder.Services.AddSingleton<MessageService>();
builder.Services.AddSingleton<ENabizService>();
builder.Services.AddHostedService<AppointmentNotificationService>();

builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 3) CORS politikas� (React 3000 portuna izin ver)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:3000",
                "https://localhost:3000",
                "http://192.168.1.103:3000",
                "https://192.168.1.103:3000"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.WebHost.ConfigureKestrel(opts =>
{
    opts.Listen(IPAddress.Any, 7231, lo => lo.UseHttps());
    opts.Listen(IPAddress.Any, 5200); // HTTP endpoint eklendi
    // Bu, dev-cert'i http.sys �zerindeki AnyIP'den sunar
});

var app = builder.Build();

// 4) Geli�tirme ortam� i�in Swagger ve detayl� hata sayfas�
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 5) HTTPS y�nlendirmesini en �stte yap�n

// 6) Routing ba�lat�n
app.UseRouting();

// 7) Tan�ml� CORS politikas�n� uygulay�n
app.UseCors(policy => policy
    .WithOrigins("http://localhost:3000", "https://localhost:3000", "http://192.168.1.103:3000", "https://192.168.1.103:3000")
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
);

app.UseHttpsRedirection();

// 8) (�ste�e ba�l�) WebSockets middleware
app.UseWebSockets();

app.UseAuthorization();

// 9) Controller ve SignalR Hub endpoint�leri
app.UseAuthorization();
app.MapControllers();   
app.MapHub<VideoCallHub>("/videoCallHub");

app.Run();
