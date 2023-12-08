using Dyno.Platform.ReferentialData.BusinessModel.Mapping;
using Dyno.Platform.ReferentialData.Nhibernate;
using Dyno.Platform.ReferentialData.WebApi;
using MicroserviceBase;
using Server.Kafka;
using WatchDog;
using Dyno.Platform.ReferentialData.DTO.Mapping;
using Dyno.Platform.ReferntialData.DataModel.UserData;
using Dyno.Platform.ReferntialData.DataModel.UserRole;
using NHibernate.AspNetCore.Identity;
using Dyno.Platform.ReferentialData.Business.IServices.IUserDataService;
using Dyno.Platform.ReferentialData.Business.Services.UserDataService;
using Dyno.Platform.ReferentialData.Business.IServices.IRoleDataService;
using Dyno.Platform.ReferentialData.Business.Services.RoleDataService;
using Dyno.Platform.ReferentialData.Business.IServices.IUserClaimService;
using Dyno.Platform.ReferentialData.Business.Services.ClaimService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Platform.Shared.EnvironmentVariable;
using Dyno.Platform.ReferentialData.Business.Services.Authentification;
using Dyno.Platform.ReferentialData.Business.IServices.IAuthentification;
using Dyno.Platform.ReferentialData.Business.IServices;
using Dyno.Platform.ReferentialData.Business.Services;
using Dyno.Platform.ReferentialData.Nhibernate.DefaultData;
using Dyno.Platform.ReferentialData.WebApi.Authorization;
using Dyno.Platform.ReferentialData.Business.Helper;
using Platform.Shared.HttpHelper;
using Dyno.Platform.Payment.DTO;
using Dyno.Platform.ReferentialData.Business.IServices.IQRCodeService;
using Dyno.Platform.ReferentialData.Business.Services.QRCodeService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Platform.Shared.Authorization;

var builder = WebApplication.CreateBuilder(args);

#region Kafka Config
KafkaConfig config = builder.Configuration.GetSection("Kafka_BootstrapServer").Get<KafkaConfig>();

/*builder.Services.AddSingleton(config);


builder.Services.AddSingleton<IProducer<HeartBeatMessage>>(producer => new Producer<HeartBeatMessage>(Topic.TOPIC_WATCHDOG_SEND_MESSAGE, config));
builder.Services.AddSingleton<IConsumer<HeartBeatMessage>>(consumer => new Consumer<HeartBeatMessage>(Topic.TOPIC_WATCHDOG_RECEIVE_MESSAGE, config));
builder.Services.AddHostedService<MicroserviceBaseWorker>();*/

#endregion




#region DataBase Config
DatabaseConfig? configdata = builder.Configuration.GetSection("ConnectionStrings").Get<DatabaseConfig>();
builder.Services.AddNHibernate(configdata?.Pgsqlconnection);
builder.Services.AddDefaultIdentity<UserEntity>()
    .AddRoles<RoleEntity>()
    .AddHibernateStores();
builder.Services.AddControllersWithViews();
#endregion

#region Authorization
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    options.ValidationInterval = TimeSpan.Zero;
});
#endregion

#region Auto Mapper
builder.Services.AddAutoMapper(typeof(MappingBMtoDM));
builder.Services.AddAutoMapper(typeof(MappingDTOtoBM));
#endregion


#region Business Service
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService,RoleService>();
builder.Services.AddScoped<IUserClaimService, UserClaimService>();
builder.Services.AddScoped<IRoleClaimService, RoleClaimService>();
builder.Services.AddScoped<IUserOtpService, UserOtpService>();
builder.Services.AddScoped<IUserTokenService, UserTokenService>();
builder.Services.AddScoped<IAuthentificationService, AuthentificationService>();


builder.Services.AddScoped<IPaymentHelper, PaymentHelper>();
builder.Services.AddScoped<IHelper<WalletDTO>, Helper<WalletDTO>>();
builder.Services.AddScoped<IHelper<QRCodeDTO>, Helper<QRCodeDTO>>();
builder.Services.AddScoped<IHelper<TransactionDTO>, Helper<TransactionDTO>>();
builder.Services.AddScoped<IQRCodeService, QRCodeService>();



builder.Services.AddScoped<IUrlHelper>(x =>
{
    var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
    var factory = x.GetRequiredService<IUrlHelperFactory>();
    return factory.GetUrlHelper(actionContext);

});
#endregion

#region Default Data Service
/*builder.Services.AddSingleton<DefaultRole>();

using (var scope = builder.Services.BuildServiceProvider().CreateScope())
{
    var roleInitializer = scope.ServiceProvider.GetRequiredService<DefaultRole>();
    roleInitializer.InitializeDefaultRoles();
}*/
#endregion

#region Authentification
//builder.Services.ConfigureJWT(builder.Configuration);
builder.Services.AddSwaggerDoc();
builder.Services.AddAuthentication().AddJwtBearer();
#endregion

// Add services to the container.

builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
