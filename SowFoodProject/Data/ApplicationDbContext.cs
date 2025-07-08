using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SowFoodProject.Models.Entities;
using SowFoodProject.Models.Entities.SowFoodLinkUp;

namespace SowFoodProject.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        #region DbSet Properties

        // Add Admin DbSet
        //public DbSet<Admin> Admins { get; set; }
        //public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        //public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<SowFoodCompanyStaff> SowFoodCompanyStaff { get; set; }
        public DbSet<SowFoodCompanyCustomer> SowFoodCompanyCustomers { get; set; }
        public DbSet<SowFoodCompanyProductionItem> SowFoodCompanyProductionItems { get; set; }
        public DbSet<SowFoodCompanySalesRecord> SowFoodCompanySalesRecords { get; set; }
        public DbSet<SowFoodCompanyShelfItem> SowFoodCompanyShelfItems { get; set; }
        public DbSet<SowFoodCompanyStaffAppraiser> SowFoodCompanyStaffAppraisers { get; set; }
        public DbSet<SowFoodCompanyStaffAttendance> SowFoodCompanyStaffAttendances { get; set; }
        public DbSet<SowFoodCompany> SowFoodCompanies { get; set; }
        //public DbSet<Transaction> Transactions { get; set; }

        #endregion


    }
}