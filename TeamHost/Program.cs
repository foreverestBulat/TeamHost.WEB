using WebTeamHost.Persistance.Extensions;
using WebTeamHost.App.Extensions;
using WebTeamHost.Infrastructure.Extensions;
using WebTeamHost.SignalR.Hubs;
using TeamHost.Localizer.Extensions;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationLayer();
builder.Services.AddInfrastructureLayer();
builder.Services.AddPersistenceLayer(builder.Configuration);
builder.Services.AddSignalR();
builder.Services.AddLocalizer();

builder.Services.AddControllersWithViews()
    .AddViewLocalization();


var app = builder.Build();

app.UseRequestLocalization();
app.UseRouting();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "MyArea",
    pattern: "{area=Home}/{controller=Home}/{action=Index}/{id?}");

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<ChatHub>("/Chats");

app.Run();
