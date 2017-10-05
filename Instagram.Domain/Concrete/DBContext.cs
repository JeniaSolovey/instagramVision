using Instagram.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instagram.Domain.Concrete
{
    class DBContext : DbContext
    {
        public DbSet<Image> images { get; set; }
    }
}
