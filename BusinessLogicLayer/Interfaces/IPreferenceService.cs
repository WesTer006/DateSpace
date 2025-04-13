using Shared.DTOs;

namespace BusinessLogicLayer.Interfaces
{
    public interface IPreferenceService
    {
        Task<PreferenceDto?> GetPreferencesAsync(string userId);
        Task<PreferenceDto> AddPreferencesAsync(string userId, PreferenceDto preferenceDto);
        Task<PreferenceDto> UpdatePreferencesAsync(string userId, PreferenceDto preferenceDto);
        Task<bool> DeletePreferencesAsync(string userId);
    }

}
