using SpiritsClassLibrary.DTOs.SpiritDTOs;

namespace DemoSpiritsAPI.Servicies.Interfaces
{
    public interface ISpiritService
    {
        public Task Create(CreateSpiritDTO createSpiritDTO);
        public Task Update(UpdateSpiritDTO updateSpiritDTO);
        public Task Delete(int id);
        public GetSpiritDTO Get(int id);
        public List<GetSpiritBasicsDTO> GetAll();

        public Task SetupTestData();

    }
}
