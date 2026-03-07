using System.Globalization;
using CareSchedule.DTOs;
using CareSchedule.Models;
using CareSchedule.Repositories.Interface;
using CareSchedule.Services.Interface;

namespace CareSchedule.Services.Implementation
{
    public class HolidayService : IHolidayService
    {
        private readonly IHolidayRepository _repo;

        public HolidayService(IHolidayRepository repo)
        {
            _repo = repo;
        }

        public List<HolidayDto> SearchHoliday(HolidaySearchQuery query)
        {
            var page = query.Page <= 0 ? 1 : query.Page;
            var pageSize = query.PageSize <= 0 ? 25 : query.PageSize;

            DateOnly? date = ParseDate(query.Date);
            DateOnly? from = ParseDate(query.From);
            DateOnly? to   = ParseDate(query.To);

            var (items, _) = _repo.Search(
                siteId:   query.SiteId,
                date:     date,
                from:     from,
                to:       to,
                status:   query.Status,
                page:     page,
                pageSize: pageSize,
                sortBy:   query.SortBy,
                sortDir:  query.SortDir
            );

            return items.Select(Map).ToList();
        }

        public HolidayDto GetHoliday(int id)
        {
            var entity = _repo.Get(id);
            if (entity is null) throw new KeyNotFoundException("Holiday not found.");
            return Map(entity);
        }

        public HolidayDto GetHolidayByDate(int siteId, string date)
        {
            var d = ParseDate(date) ?? throw new ArgumentException("Invalid date. Use yyyy-MM-dd.");
            var entity = _repo.GetByDate(siteId, d);
            if (entity is null) throw new KeyNotFoundException("Holiday not found.");
            return Map(entity);
        }

        public HolidayDto CreateHoliday(HolidayCreateDto dto)
        {
            var d = ParseDate(dto.Date) ?? throw new ArgumentException("Invalid date. Use yyyy-MM-dd.");

            var entity = new Holiday
            {
                SiteId = dto.SiteId,
                Date = d,
                Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim(),
                Status = "Active"
            };

            entity = _repo.Create(entity);
            return Map(entity);
        }

        public HolidayDto UpdateHoliday(int id, HolidayUpdateDto dto)
        {
            var entity = _repo.Get(id);
            if (entity is null) throw new KeyNotFoundException("Holiday not found.");

            if (dto.SiteId.HasValue) entity.SiteId = dto.SiteId.Value;

            if (!string.IsNullOrWhiteSpace(dto.Date))
            {
                var parsed = ParseDate(dto.Date);
                if (parsed is null) throw new ArgumentException("Invalid date. Use yyyy-MM-dd.");
                entity.Date = parsed.Value; // ✅ fix the CS0266 you saw
            }

            if (dto.Description is not null)
                entity.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();

            if (!string.IsNullOrWhiteSpace(dto.Status))
                entity.Status = dto.Status.Trim();

            _repo.Update(entity);
            return Map(entity);
        }

        public void DeactivateHoliday(int id)
        {
            var entity = _repo.Get(id);
            if (entity is null) throw new KeyNotFoundException("Holiday not found.");

            if (!string.Equals(entity.Status, "Inactive", StringComparison.Ordinal))
            {
                entity.Status = "Inactive";
                _repo.Update(entity);
            }
        }

        public void ActivateHoliday(int id)
        {
            var entity = _repo.Get(id);
            if (entity is null) throw new KeyNotFoundException("Holiday not found.");

            if (!string.Equals(entity.Status, "Active", StringComparison.Ordinal))
            {
                entity.Status = "Active";
                _repo.Update(entity);
            }
        }

        // --- helpers ---

        private static HolidayDto Map(Holiday h) => new()
        {
            HolidayId = h.HolidayId,
            SiteId = h.SiteId,
            Date = h.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            Description = h.Description,
            Status = h.Status
        };

        private static DateOnly? ParseDate(string? date)
        {
            if (string.IsNullOrWhiteSpace(date)) return null;
            if (DateOnly.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                                       DateTimeStyles.None, out var d))
            {
                return d;
            }
            return null;
        }
    }
}