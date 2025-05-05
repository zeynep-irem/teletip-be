using Microsoft.AspNetCore.SignalR;
using Teletipbe.Hubs;
using Teletipbe.Models;
using System.Globalization;
using Microsoft.Extensions.Logging;

namespace Teletipbe.Services
{
    public class AppointmentNotificationService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHubContext<VideoCallHub> _hubContext;

        public AppointmentNotificationService(IServiceProvider serviceProvider, IHubContext<VideoCallHub> hubContext)
        {
            _serviceProvider = serviceProvider;
            _hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var appointmentService = scope.ServiceProvider.GetRequiredService<AppointmentService>();
                    var appointments = await appointmentService.GetAllAppointmentsAsync();
                    var now = DateTime.Now;

                    Console.WriteLine($"[NotificationService] Şu anki zaman: {now}");

                    foreach (var appt in appointments)
                    {
                        Console.WriteLine($"[NotificationService] Randevu ID: {appt.Id}, Tarih: {appt.Date}, Saat: {appt.Time}, Durum: {appt.Status}");

                        if (DateTime.TryParseExact($"{appt.Date} {appt.Time}", "yyyy-MM-dd HH:mm",
                            CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime appointmentDateTime))
                        {
                            Console.WriteLine($"[NotificationService] Hesaplanan randevu zamanı: {appointmentDateTime}");

                            if (now >= appointmentDateTime && appt.Status == "Pending")
                            {
                                if (string.IsNullOrWhiteSpace(appt.Id))
                                {
                                    Console.WriteLine($"[NotificationService] Randevu ID boş! Tarih: {appt.Date}, Saat: {appt.Time}, Hasta: {appt.PatientId}");
                                    continue;
                                }

                                Console.WriteLine($"[NotificationService] Bildirim gönderiliyor: {appt.Id}");
                                await _hubContext.Clients.All.SendAsync("StartVideoCall", appt.Id);

                                // Durumu güncelle
                                appt.Status = "InCall";
                                await appointmentService.UpdateAppointmentAsync(appt.Id, appt);
                            }
                        }
                        else
                        {
                            Console.WriteLine("[NotificationService] Randevu tarih/saat formatı geçersiz.");
                        }
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }
    }
}
