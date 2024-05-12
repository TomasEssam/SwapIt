using AutoMapper;
using SwapIt.BL.DTOs;
using SwapIt.BL.IServices;
using SwapIt.Data.Constants;
using SwapIt.Data.Entities;
using SwapIt.Data.IRepository;
using SwapIt.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.BL.Services
{
    public class UserBalanceService : IUserBalanceService
    {
        readonly IMapper _mapper;
        readonly IUserBalanceRepository _userBalanceRepository;
        readonly IPointsLoggerRepository _pointsLoggerRepository;
        public UserBalanceService(IMapper mapper, IUserBalanceRepository userBalanceRepository, IPointsLoggerRepository pointsLoggerRepository)
        {
            _mapper = mapper;
            _userBalanceRepository = userBalanceRepository;
            _pointsLoggerRepository = pointsLoggerRepository;
        }
        public async Task<UserBalanceDto> GetByIdAsync(int userBalanceId)
        {
            var model = await _userBalanceRepository.GetByIdAsync(userBalanceId);
            return _mapper.Map<UserBalanceDto>(model);
        }

        public async Task<bool> CreateAsync(UserBalanceDto dto)
        {
            var model = _mapper.Map<UserBalance>(dto);
            await _userBalanceRepository.AddAsync(model);
            return true;
        }

        public async Task<bool> DeleteAsync(int UserBalanceId)
        {
            await _userBalanceRepository.DeleteByIdAsync(UserBalanceId);
            return true;
        }


        public async Task<UserBalanceDto> UpdateAsync(UserBalanceDto dto)
        {
            var model = _mapper.Map<UserBalance>(dto);
            await _userBalanceRepository.UpdateAsync(model);
            return dto;
        }

        public async Task<bool> AddPointsAsync(UserBalanceDto dto, int points)
        {
            return await _userBalanceRepository.AddPointsAsync(_mapper.Map<UserBalance>(dto), points);
        }

        public async Task<bool> SubstractPointsAsync(UserBalanceDto dto, int points)
        {
            return await _userBalanceRepository.SubstractPointsAsync(_mapper.Map<UserBalance>(dto), points);
        }
    }
}
