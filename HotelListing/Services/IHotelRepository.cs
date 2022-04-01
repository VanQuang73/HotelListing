using HotelListing.Models;
using System.Threading.Tasks;

namespace HotelListing.Services
{
    public interface IHotelRepository
    {
        Task<object> GetHotels(RequestParams requestParams);
        Task<object> GetHotel(int id);
        Task<object> CreateHotel(CreateHotelDTO hotelDTO);
        Task<string> UpdateHotel(int id, UpdateHotelDTO hotelDTO);
        Task<string> DeleteHotel(int id);

    }
}