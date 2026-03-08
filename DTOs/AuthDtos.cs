namespace CareSchedule.DTOs
{
    public class LoginRequestDto
    {
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty; // Patient, FrontDesk, Provider, Nurse, Tech, Operations, Admin
    }

    public class LoginResponseDto
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty; // Patient, FrontDesk, Provider, Nurse, Tech, Operations, Admin
        public string Email { get; set; } = string.Empty;
        public string Status { get; set; } = "Active";   // Active, Inactive, Locked
    }

    public class LogoutRequestDto
    {
        public int UserId { get; set; }
    }
}