using Microsoft.EntityFrameworkCore;

namespace Wheel.EntityFrameworkCore
{
    public class WheelDbContext : DbContext
    {
        public WheelDbContext(DbContextOptions<WheelDbContext> options) : base(options)
        {
        }
    }
}
