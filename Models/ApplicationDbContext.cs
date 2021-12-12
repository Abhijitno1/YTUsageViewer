using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace YTUsageViewer.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            //https://stackoverflow.com/questions/9230053/stop-entity-framework-from-modifying-database
            //Preventing the accidental creation of database
            Database.SetInitializer<ApplicationDbContext>(null);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Playlist> Playlists { get; set; }

        public System.Data.Entity.DbSet<YTUsageViewer.Models.Channel> Channels { get; set; }

        public System.Data.Entity.DbSet<YTUsageViewer.Models.Video> Videos { get; set; }

        //public System.Data.Entity.DbSet<YTUsageViewer.Models.ApplicationUser> ApplicationUsers { get; set; }
        //public System.Data.Entity.DbSet<YTUsageViewer.Models.RoleViewModel> RoleViewModels { get; set; }
    }
}