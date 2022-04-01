using HotelListing.Models;
using System.Threading.Tasks;

namespace HotelListing.Services
{
    public interface ICountryRepository
    {
        Task<object> GetCountries(RequestParams requestParams);
        Task<object> GetCountry(int id);
        Task<object> CreateCountry(CreateCountryDTO countryDTO);
        Task<string> UpdateCountry(int id, UpdateCountryDTO countryDTO);
        Task<string> DeleteCountry(int id);
    }
}