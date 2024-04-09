using SpiritsClassLibrary.DTOs.HabitatDTOs;

namespace DemoSpiritsAPI.Servicies.Interfaces
{
    public interface IHabitatService
    {
        public Task Create(CreateHabitatDTO createHabitatDTO);
        public Task Update(UpdateHabitatDTO updateHabitatDTO);
        public Task Delete(int id);
        public GetHabitatDTO Get(int id);
        public List<GetHabitatDTO> GetAll();
        public Task SetupTestData();

    }
}
