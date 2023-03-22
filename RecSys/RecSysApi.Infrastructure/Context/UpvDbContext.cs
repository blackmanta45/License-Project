using Microsoft.EntityFrameworkCore;
using RecSysApi.Domain.Models;

namespace RecSysApi.Infrastructure.Context;

public class UpvDbContext : DbContext
{
    public UpvDbContext(DbContextOptions<UpvDbContext> options) : base(options)
    {
    }

    public DbSet<Video> Videos { get; set; }
    public DbSet<Query> Queries { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Update> Updates { get; set; }
    public DbSet<Feedback> Feedbacks { get; set; }
}