﻿using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Interfaces; // Import your Activity service
using Microsoft.Extensions.DependencyInjection;

namespace Core.BackendService
{
    public class ActivityStatusUpdater : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public ActivityStatusUpdater(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var activityEnrollmentService = scope.ServiceProvider.GetRequiredService<IActivityEnrollmentService>();
                    await activityEnrollmentService.UpdateActivityStatusAsync();

                    var activityService = scope.ServiceProvider.GetRequiredService<IActivityService>();
                    await activityService.UpdateActivityStatusAsync();
                }

                await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken); // Run every 10 minutes
            }
        }
    }
}
