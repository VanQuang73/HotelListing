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
                message = Resource.CREATE_SUCCESS,
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
                    message = Resource.DELETE_FAIL,
                    developerMessage = "Dữ liệu không tồn tại."
                };
            }

            await _unitOfWork.Countries.Delete(id);
            await _unitOfWork.Save();
            return new Repsonse
            {
                statusCode = "200",
                message = Resource.DELETE_SUCCESS
            };
        }

        public async Task<Repsonse> GetCountries(RequestParams requestParams)
        {
            var countries = await _unitOfWork.Countries.GetPagedList(requestParams);
            var results = _mapper.Map<IList<CountryDTO>>(countries);
            return new Repsonse
            {
                statusCode = "200",
                message = Resource.GET_SUCCESS,
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
                message = Resource.GET_SUCCESS,
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
                    message = Resource.UPDATE_FAIL,
                    developerMessage = "Dữ liệu không tồn tại."
                };
            }

            _mapper.Map(countryDTO, country);
            _unitOfWork.Countries.Update(country);
            await _unitOfWork.Save();
            return new Repsonse
            {
                statusCode = "200",
                message = Resource.UPDATE_SUCCESS
            };
        }
    }
}