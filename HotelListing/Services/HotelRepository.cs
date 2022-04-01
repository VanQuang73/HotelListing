using AutoMapper;
using HotelListing.Controllers;
using HotelListing.Data;
using HotelListing.IRepository;
using HotelListing.Models;
using HotelListing.Properties;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelListing.Services
{
    public class HotelRepository : IHotelRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<HotelController> _logger;
        private readonly IMapper _mapper;
        public HotelRepository(IUnitOfWork unitOfWork, ILogger<HotelController> logger,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<object> CreateHotel(CreateHotelDTO hotelDTO)
        {
            var hotel = _mapper.Map<Hotel>(hotelDTO);
            await _unitOfWork.Hotels.Insert(hotel);
            await _unitOfWork.Save();
            return new
            {
                id = hotel.Id,
                hotel
            };
        }

        public async Task<string> DeleteHotel(int id)
        {
            var hotel = await _unitOfWork.Hotels.Get(q => q.Id == id);
            if (hotel == null)
            {
                _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteHotel)}");
                throw new BusinessException(Resource.NOT_DATA);
            }

            await _unitOfWork.Hotels.Delete(id);
            await _unitOfWork.Save();
            return Resource.DELETE_SUCCESS;
        }

        public async Task<object> GetHotel(int id)
        {
            var country = await _unitOfWork.Countries.Get(q => q.Id == id, include: q => q.Include(x => x.Hotels));
            var result = _mapper.Map<CountryDTO>(country);
            return new
            {
                result
            };
        }

        public async Task<object> GetHotels(RequestParams requestParams)
        {
            var hotels = await _unitOfWork.Hotels.GetPagedList(requestParams);
            var results = _mapper.Map<IList<HotelDTO>>(hotels);
            return new
            {
                results
            };
        }

        public async Task<string> UpdateHotel(int id, UpdateHotelDTO hotelDTO)
        {
            var hotel = await _unitOfWork.Hotels.Get(q => q.Id == id);
            if (hotel == null)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateHotel)}");
                throw new BusinessException(Resource.NOT_DATA);
            }

            _mapper.Map(hotelDTO, hotel);
            _unitOfWork.Hotels.Update(hotel);
            await _unitOfWork.Save();
            return Resource.UPDATE_SUCCESS;
        }
    }
}