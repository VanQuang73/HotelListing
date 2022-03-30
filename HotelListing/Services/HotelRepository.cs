﻿using AutoMapper;
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
        public async Task<Repsonse> CreateHotel(CreateHotelDTO hotelDTO)
        {
            var hotel = _mapper.Map<Hotel>(hotelDTO);
            await _unitOfWork.Hotels.Insert(hotel);
            await _unitOfWork.Save();
            return new Repsonse
            {
                statusCode = "200",
<<<<<<< HEAD
                message = Resource.CREATE_SUCCESS,
=======
                message = Resources.CREATE_SUCCESS,
>>>>>>> 881f55d69d73c13c17f841d8655250a445ed83b5
                data = new
                {
                    id = hotel.Id,
                    hotel
                }
            };
        }

        public async Task<Repsonse> DeleteHotel(int id)
        {
            var hotel = await _unitOfWork.Hotels.Get(q => q.Id == id);
            if (hotel == null)
            {
                _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteHotel)}");
                return new Repsonse
                {
                    statusCode = "400",
<<<<<<< HEAD
                    message = Resource.CREATE_FAIL,
=======
                    message = Resources.DELETE_FAIL,
>>>>>>> 881f55d69d73c13c17f841d8655250a445ed83b5
                    developerMessage = "Không tồn tại dữ liệu."
                };
            }

            await _unitOfWork.Hotels.Delete(id);
            await _unitOfWork.Save();
            return new Repsonse
            {
                statusCode = "200",
<<<<<<< HEAD
                message = Resource.DELETE_SUCCESS
=======
                message = Resources.DELETE_SUCCESS
>>>>>>> 881f55d69d73c13c17f841d8655250a445ed83b5
            };
        }

        public async Task<Repsonse> GetHotel(int id)
        {
            var country = await _unitOfWork.Countries.Get(q => q.Id == id, include: q => q.Include(x => x.Hotels));
            var result = _mapper.Map<CountryDTO>(country);
            return new Repsonse
            {
                statusCode = "200",
<<<<<<< HEAD
                message = Resource.GET_SUCCESS,
=======
                message = Resources.GET_SUCCESS,
>>>>>>> 881f55d69d73c13c17f841d8655250a445ed83b5
                developerMessage = new
                {
                    id = id
                },
                data = result
            };
        }

        public async Task<Repsonse> GetHotels(RequestParams requestParams)
        {
            var hotels = await _unitOfWork.Hotels.GetPagedList(requestParams);
            var results = _mapper.Map<IList<HotelDTO>>(hotels);
            return new Repsonse
            {
                statusCode = "200",
<<<<<<< HEAD
                message = Resource.GET_SUCCESS,
=======
                message = Resources.GET_SUCCESS,
>>>>>>> 881f55d69d73c13c17f841d8655250a445ed83b5
                developerMessage = requestParams,
                data = results
            };
        }

        public async Task<Repsonse> UpdateHotel(int id, UpdateHotelDTO hotelDTO)
        {
            var hotel = await _unitOfWork.Hotels.Get(q => q.Id == id);
            if (hotel == null)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateHotel)}");
                return new Repsonse
                {
                    statusCode = "400",
<<<<<<< HEAD
                    message = Resource.UPDATE_FAIL,
=======
                    message = Resources.UPDATE_FAIL,
>>>>>>> 881f55d69d73c13c17f841d8655250a445ed83b5
                    developerMessage = "Dữ kiệu không tồn tại."
                };
            }

            _mapper.Map(hotelDTO, hotel);
            _unitOfWork.Hotels.Update(hotel);
            await _unitOfWork.Save();
            return new Repsonse
            {
                statusCode = "200",
<<<<<<< HEAD
                message = Resource.UPDATE_SUCCESS
            };
        }
    }
}
=======
                message = Resources.UPDATE_SUCCESS
            };
        }
    }
}
>>>>>>> 881f55d69d73c13c17f841d8655250a445ed83b5
