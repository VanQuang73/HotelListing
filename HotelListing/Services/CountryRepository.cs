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
    public class CountryRepository : ICountryRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CountryController> _logger;
        private readonly IMapper _mapper;
        public CountryRepository(IUnitOfWork unitOfWork, ILogger<CountryController> logger,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<Repsonse> CreateCountry(CreateCountryDTO countryDTO)
        {
            var country = _mapper.Map<Country>(countryDTO);
            await _unitOfWork.Countries.Insert(country);
            await _unitOfWork.Save();
            return new Repsonse
            {
                statusCode = "201",
<<<<<<< HEAD
                message = Resource.CREATE_SUCCESS,
=======
                message = Resources.CREATE_SUCCESS,
>>>>>>> 881f55d69d73c13c17f841d8655250a445ed83b5
                data = new
                {
                    id = country.Id,
                    country
                }
            };
        }

        public async Task<Repsonse> DeleteCountry(int id)
        {
            var country = await _unitOfWork.Countries.Get(q => q.Id == id);
            if (country == null || id < 1)
            {
                _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteCountry)}");
                return new Repsonse
                {
                    statusCode = "400",
<<<<<<< HEAD
                    message = Resource.DELETE_FAIL,
=======
                    message = Resources.DELETE_FAIL,
>>>>>>> 881f55d69d73c13c17f841d8655250a445ed83b5
                    developerMessage = "Dữ liệu không tồn tại."
                };
            }

            await _unitOfWork.Countries.Delete(id);
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

        public async Task<Repsonse> GetCountries(RequestParams requestParams)
        {
            var countries = await _unitOfWork.Countries.GetPagedList(requestParams);
            var results = _mapper.Map<IList<CountryDTO>>(countries);
            return new Repsonse
            {
                statusCode = "200",
<<<<<<< HEAD
                message = Resource.GET_SUCCESS,
=======
                message = Resources.GET_SUCCESS,
>>>>>>> 881f55d69d73c13c17f841d8655250a445ed83b5
                data = results
            };
        }

        public async Task<Repsonse> GetCountry(int id)
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
                data = result
            };
        }

        public async Task<Repsonse> UpdateCountry(int id, UpdateCountryDTO countryDTO)
        {
            var country = await _unitOfWork.Countries.Get(q => q.Id == id);
            if (country == null)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateCountry)}");
                return new Repsonse
                {
                    statusCode = "400",
<<<<<<< HEAD
                    message = Resource.UPDATE_FAIL,
=======
                    message = Resources.UPDATE_FAIL,
>>>>>>> 881f55d69d73c13c17f841d8655250a445ed83b5
                    developerMessage = "Dữ liệu không tồn tại."
                };
            }

            _mapper.Map(countryDTO, country);
            _unitOfWork.Countries.Update(country);
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
