using LightWeightPerformanceTesting.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LightWeightPerformanceTesting.Core.Interfaces
{
    public interface IAppDbContext: IDisposable
    {
        DbSet<StoredEvent> StoredEvents { get; set; }     
        DbSet<Snapshot> Snapshots { get; set; }
        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
