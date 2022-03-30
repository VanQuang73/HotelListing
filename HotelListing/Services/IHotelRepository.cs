using HotelListing.Models;
using System.Threading.Tasks;

namespace HotelListing.Services
{
    public interface IHotelRepository
    {
        Task<Repsonse> GetHotels(RequestParams requestParams);
        Task<Repsonse> GetHotel(int id);
        Task<Repsonse> CreateHotel(CreateHotelDTO hotelDTO);
        Task<Repsonse> UpdateHotel(int id, UpdateHotelDTO hotelDTO);
        Task<Repsonse> DeleteHotel(int id);

    }
}