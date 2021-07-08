using SerhatKaya.CheckList.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;

namespace SerhatKaya.CheckList.Context
{
    public class CheckListContext : DbContext
    {
        private readonly ILogger<CheckListContext> _logger;
        public CheckListContext(DbContextOptions<CheckListContext> options, ILogger<CheckListContext> logger) : base(options)
        {
            _logger = logger;
        }

        public DbSet<Entities.CheckList> CheckLists { get; set; }
        public DbSet<CheckListItem> CheckListItems { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TagItems> TagItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Logs> Logs { get; set; }


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
                try
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entry.Entity.CreatedUser = entry.Entity.CreatedUser ?? null;
                            entry.Entity.CreatedDateTime = DateTime.UtcNow;
                            break;
                    }

                    return await base.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError("Exception", ex);
                }

            var result = await base.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}