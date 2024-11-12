using Blossom_BusinessObjects.Entities.Enums;
using Blossom_DAOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blossom_Services
{
    public class FlowerStatusUpdateTaskService : BackgroundService
    {
        private readonly FlowerService _flowerService;

        public FlowerStatusUpdateTaskService(FlowerService flowerService)
        {
            _flowerService = flowerService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // Adjust delay for the desired schedule

                var flowersToExpire = await _flowerService.GetExpiredFlowers();

                foreach (var flower in flowersToExpire)
                {
                    flower.Status = FlowerStatus.EXPIRED;
                    await _flowerService.UpdateFlower(flower);
                }
            }
        }
    }
}
