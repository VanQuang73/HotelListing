using HotelListing.Models;
using System.Threading.Tasks;

namespace HotelListing.Services
{
    public interface ICountryRepository
    {
        Task<Repsonse> GetCountries(RequestParams requestParams);
        Task<Repsonse> GetCountry(int id);
        Task<Repsonse> CreateCountry(CreateCountryDTO countryDTO);
        Task<Repsonse> UpdateCountry(int id, UpdateCountryDTO countryDTO);
        Task<Repsonse> DeleteCountry(int id);
    }
<<<<<<< HEAD
}
=======
}
>>>>>>> 881f55d69d73c13c17f841d8655250a445ed83b5
