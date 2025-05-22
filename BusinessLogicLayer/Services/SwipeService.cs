using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Interfaces;
using Shared.DTOs;

namespace BusinessLogicLayer.Services
{
	public class SwipeService : ISwipeService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IUserRepository _userRepository;
		private readonly ISwipeRepository _swipeRepository;

		public SwipeService(IUnitOfWork unitOfWork, IMapper mapper, IUserRepository userRepository, ISwipeRepository swipeRepository)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_userRepository = userRepository;
			_swipeRepository = swipeRepository;
		}
		public async Task<SwipeResultDto> SwipeAsync(string swiperId, string targetId)
		{
			var target = await _userRepository.FindByIdAsync(targetId);
			var swiper = await _userRepository.FindByIdAsync(swiperId);

			if (swiper == null || target == null)
			{
				return new SwipeResultDto
				{
					Success = false,
					IsMatch = false
				};
			}

			var reverseSwipe = await _swipeRepository.GetReverseSwipeAsync(swiperId, targetId);

			bool isMatch = false;
			if (reverseSwipe != null)
			{
				await _swipeRepository.SetTargetAgreeAsync(swiperId, targetId);
				isMatch = true;
			}
			else
			{
				await _swipeRepository.CreateSwipeAsync(swiper, target);
			}

			await _unitOfWork.SaveChangesAsync();

			return new SwipeResultDto
			{
				Success = true,
				IsMatch = isMatch
			};
		}
	}
}
