using HRTool.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HRTool.Startup))]
namespace HRTool
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            //createRolesandUsers();
        }


        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                        
            if (!roleManager.RoleExists("SuperAdmin"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Id = null;
                role.Name = "SuperAdmin";
                roleManager.Create(role);

                var user = new ApplicationUser();
                //user.UserName = "giorgio.lacagnina@energymanpower.com";
                user.UserName = "hitesh@inheritx.com";
                string userPWD = "inx@!123";

                var chkUser = UserManager.Create(user, userPWD);
                
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "SuperAdmin");
                }
            }                       
        }
    }
}
