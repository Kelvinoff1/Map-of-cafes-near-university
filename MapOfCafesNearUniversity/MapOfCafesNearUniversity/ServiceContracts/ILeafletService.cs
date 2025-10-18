using MapOfCafesNearUniversity.Models;

namespace MapOfCafesNearUniversity.ServiceContracts
{
    public interface ILeafletService
    {
        Task<List<Cafe>> GetCafes();
    }
}
