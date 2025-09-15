namespace AuctionService.Data;

using AuctionService.Entities;
using Microsoft.EntityFrameworkCore;

public class AuctionDbContext : DbContext
{
    public AuctionDbContext(DbContextOptions options) : base(options)
    {

    }
    public DbSet<Auction> Auction { get; set; }
}