namespace SowFoodProject.Application.Interfaces.IServices
{
    public interface IServiceManager
    {
        IAdminSowFoodCompanyService AdminSowFoodCompanyService { get; }
        ICloudinaryService CloudinaryService { get; }
        IRoleService RoleService { get; }
        ISowFoodCompanyCustomerService SowFoodCompanyCustomerService { get; }
        ISowFoodCompanyProductionItemService SowFoodCompanyProductionItemService { get; }
        ISowFoodCompanySalesRecordService SowFoodCompanySalesRecordService { get; }
        ISowFoodCompanyShelfItemService SowFoodCompanyShelfItemService { get; }
        ISowFoodCompanyStaffService SowFoodCompanyStaffService { get; }
        ISowFoodCompanyStaffAppraiserService SowFoodCompanyStaffAppraiserService { get; }
        ISowFoodCompanyStaffAttendanceService SowFoodCompanyStaffAttendanceService { get; }
        IDashBoardService DashBoardService { get; }
    }
}
