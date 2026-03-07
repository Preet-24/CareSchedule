// File: CareSchedule.Services.Implementation/SystemConfigService.cs
using System;
using System.Collections.Generic;
using System.Globalization;
using CareSchedule.DTOs;
using CareSchedule.Models;
using CareSchedule.Repositories.Interface;
using CareSchedule.Services.Interface;

namespace CareSchedule.Services.Implementation
{
    public class SystemConfigService : ISystemConfigService
    {
        private readonly ISystemConfigRepository _repo;

        public SystemConfigService(ISystemConfigRepository repo)
        {
            _repo = repo;
        }

        private static string Iso(DateTime utc)
        {
            return DateTime.SpecifyKind(utc, DateTimeKind.Utc)
                           .ToString("o", CultureInfo.InvariantCulture);
        }

        public List<SystemConfigDto> Search(SystemConfigSearchQuery q)
        {
            var page = q.Page <= 0 ? 1 : q.Page;
            var pageSize = q.PageSize <= 0 ? 25 : q.PageSize;
            var sortBy = string.IsNullOrWhiteSpace(q.SortBy) ? "updateddate" : q.SortBy;
            var sortDir = string.IsNullOrWhiteSpace(q.SortDir) ? "desc" : q.SortDir;

            var (items, _) = _repo.Search(q.Key, q.Scope, page, pageSize, sortBy, sortDir);

            var list = new List<SystemConfigDto>(items.Count);
            foreach (var c in items) list.Add(Map(c));
            return list;
        }

        public SystemConfigDto Get(int id)
        {
            var e = _repo.Get(id);
            if (e is null) throw new KeyNotFoundException("System config not found.");
            return Map(e);
        }

        public SystemConfigDto Create(SystemConfigCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Key))
                throw new ArgumentException("Key is required.");
            if (string.IsNullOrWhiteSpace(dto.Value))
                throw new ArgumentException("Value is required.");

            var e = new SystemConfig
            {
                Key = dto.Key.Trim(),
                Value = dto.Value.Trim(),
                Scope = string.IsNullOrWhiteSpace(dto.Scope) ? "Global" : dto.Scope.Trim(),
                UpdatedBy = dto.UpdatedBy,
                UpdatedDate = DateTime.UtcNow
            };

            e = _repo.Create(e);
            return Map(e);
        }

        public SystemConfigDto Update(int id, SystemConfigUpdateDto dto)
        {
            var e = _repo.Get(id);
            if (e is null) throw new KeyNotFoundException("System config not found.");

            if (dto.Value is not null) e.Value = dto.Value.Trim();
            if (!string.IsNullOrWhiteSpace(dto.Scope)) e.Scope = dto.Scope.Trim();
            if (dto.UpdatedBy.HasValue) e.UpdatedBy = dto.UpdatedBy;

            e.UpdatedDate = DateTime.UtcNow;
            _repo.Update(e);

            return Map(e);
        }

        public void Delete(int id)
        {
            _repo.Delete(id);
        }

        private static SystemConfigDto Map(SystemConfig c) => new SystemConfigDto
        {
            ConfigId = c.ConfigId,
            Key = c.Key,
            Value = c.Value,
            Scope = c.Scope,
            UpdatedBy = c.UpdatedBy,
            UpdatedDate = Iso(c.UpdatedDate)
        };
    }
}