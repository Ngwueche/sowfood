namespace SowFoodProject.Application.DTOs
{
    public class CreateStaffAttendanceDto
    {
        public string StaffId { get; set; }
        public DateTime LogonTime { get; set; }
        public DateTime LogoutTime { get; set; }
    }

    public class UpdateStaffAttendanceDto
    {
        public string AttendanceId { get; set; }
        public DateTime? ConfirmedTimeIn { get; set; }
        public bool IsConfirmed { get; set; }
        public string? ConfirmedByUserId { get; set; }
    }

    public class GetStaffAttendanceDto
    {
        public string AttendanceId { get; set; }
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public DateTime LogonTime { get; set; }
        public DateTime LogoutTime { get; set; }
        public DateTime? ConfirmedTimeIn { get; set; }
        public bool IsConfirmed { get; set; }
        public string? ConfirmedBy { get; set; }
        public DateTime DateLogged { get; set; }

    }
}
