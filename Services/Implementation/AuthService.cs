using System;
using System.Collections.Generic;
using CareSchedule.Services.Interface;
using CareSchedule.DTOs;
using CareSchedule.Infrastructure;
using CareSchedule.Repositories.Interface;
using CareSchedule.Models;

namespace CareSchedule.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IAuditLogRepository _auditRepo;
        private readonly IUnitOfWork _uow;

        public AuthService(IUserRepository userRepo, IAuditLogRepository auditRepo, IUnitOfWork uow)
        {
            _userRepo = userRepo;
            _auditRepo = auditRepo;
            _uow = uow;
        }

        private static readonly string[] AllowedRoles = new[]
        {
            "Patient", "FrontDesk", "Provider", "Nurse", "Tech", "Operations", "Admin"
        };

        public LoginResponseDto Login(string email, string role)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required.");
            if (string.IsNullOrWhiteSpace(role))
                throw new ArgumentException("Role is required.");

            // Normalize inputs
            var normEmail = email.Trim();
            var normRole  = role.Trim();

            // Validate role vocabulary
            var matchedRole = AllowedRoles.FirstOrDefault(r => string.Equals(r, normRole, StringComparison.OrdinalIgnoreCase));
            if (matchedRole == null)
                throw new ArgumentException("Invalid role.");

            var user = _userRepo.GetByEmail(normEmail, matchedRole);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            if (string.Equals(user.Status, "Inactive", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("User is inactive.");
            if (string.Equals(user.Status, "Locked", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("User account is locked.");

            // Write AuditLog (append-only)
            var log = new AuditLog
            {
                UserId = user.UserId,
                Action = "Login",
                Resource = "User",
                Timestamp = DateTime.UtcNow,
                Metadata = "{\"message\":\"User logged in\"}"
            };
            _auditRepo.Create(log);
            _uow.SaveChanges();

            return new LoginResponseDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Role = user.Role,
                Email = user.Email,
                Status = user.Status
            };
        }

        public void Logout(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("Invalid userId.");

            var user = _userRepo.GetById(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            // Write AuditLog (append-only)
            var log = new AuditLog
            {
                UserId = user.UserId,
                Action = "Logout",
                Resource = "User",
                Timestamp = DateTime.UtcNow,
                Metadata = "{\"message\":\"User logged out\"}"
            };
            _auditRepo.Create(log);
            _uow.SaveChanges();
        }
    }
}