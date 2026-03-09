using CareSchedule.Repositories.Implementation;
using CareSchedule.Repositories.Interface;
using CareSchedule.Services.Implementation;
using CareSchedule.Services.Interface;
using CareSchedule.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using CareSchedule.API.Middleware;
using CareSchedule.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<CareScheduleContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


// Booking repos
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IAppointmentChangeRepository, AppointmentChangeRepository>();
builder.Services.AddScoped<IPublishedSlotBookingRepository, PublishedSlotBookingRepository>();
builder.Services.AddScoped<ICalendarEventRepository, CalendarEventRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IReminderScheduleRepository, ReminderScheduleRepository>();

// Booking service
builder.Services.AddScoped<IBookingService, BookingService>();


// Repositories
builder.Services.AddScoped<IAvailabilityTemplateRepository, AvailabilityTemplateRepository>();
builder.Services.AddScoped<IAvailabilityBlockRepository, AvailabilityBlockRepository>();
builder.Services.AddScoped<IPublishedSlotRepository, PublishedSlotRepository>();
builder.Services.AddScoped<ICalendarEventRepository, CalendarEventRepository>();

// Service
builder.Services.AddScoped<IAvailabilityService, AvailabilityService>();

builder.Services.AddScoped<ISiteRepository, SiteRepository>();
builder.Services.AddScoped<ISiteService, SiteService>();

builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IRoomService, RoomService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();
builder.Services.AddScoped<IAuditLogService, AuditLogService>();

builder.Services.AddScoped<ISystemConfigRepository, SystemConfigRepository>();
builder.Services.AddScoped<ISystemConfigService, SystemConfigService>();

builder.Services.AddScoped<IHolidayRepository, HolidayRepository>();
builder.Services.AddScoped<IHolidayService, HolidayService>();

builder.Services.AddScoped<IProviderRepository, ProviderRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IProviderServiceRepository, ProviderServiceRepository>();
builder.Services.AddScoped<IProviderMasterService, ProviderMasterService>();
builder.Services.AddScoped<IServiceMasterService, ServiceMasterService>();
builder.Services.AddScoped<IProviderServiceMappingService, ProviderServiceMappingService>();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
