using Dyno.Platform.Payment.Business.IServices;
using Dyno.Platform.Payment.Business.Services;
using Dyno.Platform.Payment.BusinessModel.Mapping;
using Dyno.Platform.Payment.DTO.Mapping;
using Dyno.Platform.Payment.Nhibernate;
using Dyno.Platform.Payment.WebAPI;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region DataBase Config
DatabaseConfig? configdata = builder.Configuration.GetSection("ConnectionStrings").Get<DatabaseConfig>();
builder.Services.AddNHibernate(configdata?.Pgsqlconnection);
builder.Services.AddControllersWithViews();
#endregion

#region Auto Mapper
builder.Services.AddAutoMapper(typeof(MapperDTO_BM));
builder.Services.AddAutoMapper(typeof(MapperBM_DM));
#endregion

#region Business Service
builder.Services.AddScoped<IQRCodeService, QRCodeService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IWalletService, WalletService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
