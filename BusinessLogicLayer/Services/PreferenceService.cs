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
            // Получаем репозиторий для Preference с помощью дженерика
            var preferencesRepo = _unitOfWork.GetRepository<Preference>();

            // Ищем предпочтения по userId
            var prefs = await preferencesRepo.FindAsync(p => p.UserId == userId);
            var pref = prefs.FirstOrDefault();

            return pref == null ? null : _mapper.Map<PreferenceDto>(pref);
        }

        public async Task<PreferenceDto> AddPreferencesAsync(string userId, PreferenceDto preferenceDto)
        {
            var entity = _mapper.Map<Preference>(preferenceDto);
            entity.UserId = userId;

            // Получаем репозиторий для Preference с помощью дженерика и добавляем новую сущность
            var preferencesRepo = _unitOfWork.GetRepository<Preference>();
            await preferencesRepo.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<PreferenceDto>(entity);
        }

        public async Task<PreferenceDto> UpdatePreferencesAsync(string userId, PreferenceDto preferenceDto)
        {
            // Получаем репозиторий для Preference с помощью дженерика
            var preferencesRepo = _unitOfWork.GetRepository<Preference>();

            var prefs = await preferencesRepo.FindAsync(p => p.UserId == userId);
            var entity = prefs.FirstOrDefault();

            if (entity == null)
                throw new InvalidOperationException("Preferences not found for this user.");

            // Обновляем значения
            _mapper.Map(preferenceDto, entity);
            entity.UserId = userId; // Подстраховка, если в dto не передаётся

            preferencesRepo.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<PreferenceDto>(entity);
        }

        public async Task<bool> DeletePreferencesAsync(string userId)
        {
            // Получаем репозиторий для Preference с помощью дженерика
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
