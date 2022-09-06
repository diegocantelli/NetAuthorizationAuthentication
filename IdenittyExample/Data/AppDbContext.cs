using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdenittyExample.Data
{
    public class AppDbContext : IdentityDbContext //interface necessária para criar as tabelas do identity
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            :base(options)
        {

        }
    }
}
