using BugHunters.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace BugHunters.Api.Persistence;

public class BugHunterContext : DbContext
{
    public DbSet<Hunter> Hunters { get; set; }
    
}