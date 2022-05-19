using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Entities;

namespace ShopOnline.Api.Data
{
    public class ShopOnlineDbContext: DbContext
    {
        
        private readonly Action<ShopOnlineDbContext, ModelBuilder> modelCustomizer;
        
        public ShopOnlineDbContext(DbContextOptions<ShopOnlineDbContext> options) : base(options)
        {

        }

        public ShopOnlineDbContext(DbContextOptions<ShopOnlineDbContext> options, Action<ShopOnlineDbContext, ModelBuilder> modelCustomizer = null):base(options)
        {            
            this.modelCustomizer = modelCustomizer;
        }        
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelCustomizer != null)
            {
                modelCustomizer(this, modelBuilder);
            } else
            {
                base.OnModelCreating(modelBuilder);
                DataHelper.SeedDatabase(modelBuilder);
            }
		}
		
		public DbSet<Cart> Carts { get; set; }
		public DbSet<CartItem> CartItems { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<ProductCategory> ProductCategories { get; set; }
		public DbSet<User> Users { get; set; }
	}
}
