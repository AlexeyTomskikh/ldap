namespace ldap.Infrastructure
{
    using System.Data.Entity;
    using ldap.Models;

    public class LdapDbContext : DbContext
    {
        public LdapDbContext()
            : base("RolesContext")
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany(p => p.Roles)
                .WithMany(p => p.Users)
                .Map(p => { p.ToTable("UserOfRoles"); p.MapLeftKey("RoleId"); p.MapRightKey("UserId"); });
        }
    }
}