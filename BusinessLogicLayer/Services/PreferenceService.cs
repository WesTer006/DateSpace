using AutoMapper;
using BusinessLogicLayer.Interfaces;
using Shared.DTOs;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;

namespace BusinessLogicLayer.Services
{
    public class PreferenceService : IPreferenceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PreferenceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PreferenceDto?> GetPreferencesAsync(string userId)
        {
            var preferencesRepo = _unitOfWork.GetRepository<Preference>();

            var prefs = await preferencesRepo.FindAsync(p => p.UserId == userId);
            var pref = prefs.FirstOrDefault();

            return pref == null ? null : _mapper.Map<PreferenceDto>(pref);
        }

        public async Task<PreferenceDto> AddPreferencesAsync(string userId, PreferenceDto preferenceDto)
        {
            var entity = _mapper.Map<Preference>(preferenceDto);
            entity.UserId = userId;

            var preferencesRepo = _unitOfWork.GetRepository<Preference>();
            await preferencesRepo.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<PreferenceDto>(entity);
        }

        public async Task<PreferenceDto> UpdatePreferencesAsync(string userId, PreferenceDto preferenceDto)
        {
            var preferencesRepo = _unitOfWork.GetRepository<Preference>();

            var prefs = await preferencesRepo.FindAsync(p => p.UserId == userId);
            var entity = prefs.FirstOrDefault();

            if (entity == null)
                throw new InvalidOperationException("Preferences not found for this user.");

            _mapper.Map(preferenceDto, entity);
            entity.UserId = userId;

            preferencesRepo.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<PreferenceDto>(entity);
        }

        public async Task<bool> DeletePreferencesAsync(string userId)
        {
            var preferencesRepo = _unitOfWork.GetRepository<Preference>();

            var prefs = await preferencesRepo.FindAsync(p => p.UserId == userId);
            var entity = prefs.FirstOrDefault();

            if (entity == null)
                return false;

            preferencesRepo.Remove(entity);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
