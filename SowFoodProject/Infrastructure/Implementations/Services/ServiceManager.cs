using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using SowFoodProject.Application.Interfaces.IServices;
using SowFoodProject.Data;
using SowFoodProject.Infrastructure.Utilities;
using SowFoodProject.Models.Entities;

namespace SowFoodProject.Infrastructure.Implementations.Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly ApplicationDbContext _context;
        private readonly Lazy<IAdminSowFoodCompanyService> _adminSowFoodCompanyService;
        private readonly Lazy<ICloudinaryService> _cloudinaryService;
        private readonly Lazy<IRoleService> _roleService;
        private readonly Lazy<ISowFoodCompanyCustomerService> _sowFoodCompanyCustomerService;
        private readonly Lazy<ISowFoodCompanyProductionItemService> _sowFoodCompanyProductionItemService;
        private readonly Lazy<ISowFoodCompanySalesRecordService> _sowFoodCompanySalesRecordService;
        private readonly Lazy<ISowFoodCompanyShelfItemService> _sowFoodCompanyShelfItemService;
        private readonly Lazy<ISowFoodCompanyStaffService> _sowFoodCompalnyStaffService;
        private readonly Lazy<ISowFoodCompanyStaffAppraiserService> _sowFoodCompanyStaffAppraiserService;
        private readonly Lazy<ISowFoodCompanyStaffAttendanceService> _sowFoodCompanyStaffAttendanceService;
        private readonly Lazy<IDashBoardService> _dashBoardService;



        public ServiceManager(ApplicationDbContext context, IConfiguration configuration, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IHttpContextAccessor contextAccessor, IdGenerator idGenerator, IPasswordHasher<ApplicationUser> passwordHasher)
        {
            _context = context;
            _adminSowFoodCompanyService = new Lazy<IAdminSowFoodCompanyService>(() => new AdminSowFoodCompanyService(context!, userManager, roleManager, contextAccessor));
            _roleService = new Lazy<IRoleService>(() => new RoleService(roleManager!));
            _cloudinaryService = new Lazy<ICloudinaryService>(() => new CloudinaryService(configuration!));
            _sowFoodCompalnyStaffService = new Lazy<ISowFoodCompanyStaffService>(() => new SowFoodCompanyStaffService(context, userManager, roleManager, idGenerator));
            _sowFoodCompanyProductionItemService = new Lazy<ISowFoodCompanyProductionItemService>(() => new SowFoodCompanyProductionItemService(context, contextAccessor));
            _sowFoodCompanyCustomerService = new Lazy<ISowFoodCompanyCustomerService>(() => new SowFoodCompanyCustomerService(context, passwordHasher));
            _sowFoodCompanySalesRecordService = new Lazy<ISowFoodCompanySalesRecordService>(() => new SowFoodCompanySalesRecordService(context, contextAccessor));
            _sowFoodCompanyShelfItemService = new Lazy<ISowFoodCompanyShelfItemService>(() => new SowFoodCompanyShelfItemService(context, contextAccessor));
            _sowFoodCompanyStaffAppraiserService = new Lazy<ISowFoodCompanyStaffAppraiserService>(() => new SowFoodCompanyStaffAppraiserService(context));
            _sowFoodCompanyStaffAttendanceService = new Lazy<ISowFoodCompanyStaffAttendanceService>(() => new SowFoodCompanyStaffAttendanceService(context));
            _dashBoardService = new Lazy<IDashBoardService>(() => new DashBoardService(context, contextAccessor));

        }

        public IAdminSowFoodCompanyService AdminSowFoodCompanyService => _adminSowFoodCompanyService.Value;
        public ICloudinaryService CloudinaryService => _cloudinaryService.Value;

        public IRoleService RoleService => _roleService.Value;

        public ISowFoodCompanyStaffService SowFoodCompanyStaffService => _sowFoodCompalnyStaffService.Value;

        public ISowFoodCompanyProductionItemService SowFoodCompanyProductionItemService => _sowFoodCompanyProductionItemService.Value;

        public ISowFoodCompanyCustomerService SowFoodCompanyCustomerService => _sowFoodCompanyCustomerService.Value;

        public ISowFoodCompanySalesRecordService SowFoodCompanySalesRecordService => _sowFoodCompanySalesRecordService.Value;

        public ISowFoodCompanyShelfItemService SowFoodCompanyShelfItemService => _sowFoodCompanyShelfItemService.Value;

        public ISowFoodCompanyStaffAppraiserService SowFoodCompanyStaffAppraiserService => _sowFoodCompanyStaffAppraiserService.Value;

        public ISowFoodCompanyStaffAttendanceService SowFoodCompanyStaffAttendanceService => _sowFoodCompanyStaffAttendanceService.Value;
        public IDashBoardService DashBoardService => _dashBoardService.Value;
    }

}
