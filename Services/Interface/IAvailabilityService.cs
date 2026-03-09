using System.Collections.Generic;
using CareSchedule.DTOs;

namespace CareSchedule.Services.Interface
{
    public interface IAvailabilityService
    {
        // --------- Templates ---------
        int CreateTemplate(CreateAvailabilityTemplateRequestDto dto);
        void UpdateTemplate(UpdateAvailabilityTemplateRequestDto dto);
        IEnumerable<AvailabilityTemplateResponseDto> ListTemplates(int providerId, int siteId);

        // --------- Blocks ---------
        int CreateBlock(CreateAvailabilityBlockRequestDto dto);
        void RemoveBlock(int blockId);
        IEnumerable<AvailabilityBlockResponseDto> ListBlocks(int providerId, int siteId, string? date);

        // --------- Slots (Read-only) ---------
        IEnumerable<SlotResponseDto> GetOpenSlots(SlotSearchRequestDto dto);

        // --------- Slot generation (internal trigger for MVP) ---------
        GenerateSlotsResponseDto GenerateSlots(GenerateSlotsRequestDto dto);
    }
}