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
        public async Task<object> CreateCountry(CreateCountryDTO countryDTO)
        {
            var country = _mapper.Map<Country>(countryDTO);
            await _unitOfWork.Countries.Insert(country);
            await _unitOfWork.Save();
            return new
            {
                id = country.Id,
                country
            };
        }

        public async Task<string> DeleteCountry(int id)
        {
            var country = await _unitOfWork.Countries.Get(q => q.Id == id);
            if (country == null || id < 1)
            {
                _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteCountry)}");
                throw new BusinessException(Resource.NOT_DATA);
            }

            await _unitOfWork.Countries.Delete(id);
            await _unitOfWork.Save();
            return Resource.DELETE_SUCCESS;
        }

        public async Task<object> GetCountries(RequestParams requestParams)
        {
            var countries = await _unitOfWork.Countries.GetPagedList(requestParams);
            var results = _mapper.Map<IList<CountryDTO>>(countries);
            return new
            {
                results
            };
        }

        public async Task<object> GetCountry(int id)
        {
            var country = await _unitOfWork.Countries.Get(q => q.Id == id, include: q => q.Include(x => x.Hotels));
            var result = _mapper.Map<CountryDTO>(country);
            return new
            {
                result
            };
        }

        public async Task<string> UpdateCountry(int id, UpdateCountryDTO countryDTO)
        {
            var country = await _unitOfWork.Countries.Get(q => q.Id == id);
            if (country == null)
            {
                throw new BusinessException(Resource.NOT_DATA);
            }

            _mapper.Map(countryDTO, country);
            _unitOfWork.Countries.Update(country);
            await _unitOfWork.Save();
            return Resource.UPDATE_SUCCESS;
        }
    }
}