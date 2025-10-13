using MapOfCafesNearUniversity.Models;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace MapOfCafesNearUniversity.ServiceContracts
{
    public interface ILeafletService
    {
        Task<List<Cafe>> GetCafes();
    }
}
