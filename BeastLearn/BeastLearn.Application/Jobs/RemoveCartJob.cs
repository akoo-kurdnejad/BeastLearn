using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeastLearn.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace BeastLearn.Application.Jobs
{
    [DisallowConcurrentExecution]
    public class RemoveCartJob:IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            var options = new DbContextOptionsBuilder<BestLearnContext>();
            options.UseSqlServer(
                "Data Source=AKOO-PC\\MSSQLSERVER2017; Initial Catalog=BestLearnDB; Integrated Security=True;MultipleActiveResultSets=True");

            using (BestLearnContext _context = new BestLearnContext(options.Options))
            {
                var orders = _context.Orders
                    .Where(o => !o.IsFinaly && o.CreateDate < DateTime.Now.AddHours(-48))
                    .ToList();
                foreach (var order in orders)
                {
                    var orderDetails = _context.OrderDetailses.Where(od => od.OrderId == order.OrderId).ToList();
                    foreach (var detail in orderDetails)
                    {
                        _context.OrderDetailses.Remove(detail);
                    }

                    _context.Orders.Remove(order);
                }
                _context.SaveChanges();

            }

            return Task.CompletedTask;
        }
    }
}
