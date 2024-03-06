using AutoMapper;
using DemoSpiritsAPI.DTOs.HabitatDTOs;
using DemoSpiritsAPI.EntiryFramework.Contexts;
using DemoSpiritsAPI.Models;
using DemoSpiritsAPI.Servicies.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.AccessControl;

namespace DemoSpiritsAPI.Servicies
{
    public class HabitatService : IHabitatService
    {
        private MySQLContext _dbContext;
        private IMapper _mapper;
        public HabitatService(MySQLContext mySQLContext, IMapper mapper) {
            _dbContext = mySQLContext;
            _mapper = mapper;
        }
        public async Task Create(CreateHabitatDTO createHabitatDTO)
        {
            Habitat habitat = _mapper.Map<Habitat>(createHabitatDTO);
            habitat.LastUpdated = DateTime.Now;
            _dbContext.Habitats.Add(habitat);
            habitat.MarkerLocation.Habitat = habitat;

            _dbContext.SaveChanges();
        }

        public async Task Delete(int id)
        {
            //Implement soft delete
            var habitat = _dbContext.Habitats.Include(x=>x.Border).Where(x=>x.Id == id).FirstOrDefault();
            _dbContext.BorderPoints.RemoveRange(habitat.Border);
            _dbContext.Habitats.Remove(habitat);
            _dbContext.SaveChanges();
        }

        public GetHabitatDTO Get(int id)
        {
            Habitat habitat = _dbContext.Habitats
                .Include(h => h.MarkerLocation)
                .Include(h => h.Border)
                .Include(h => h.Spirits)
                .FirstOrDefault(h => h.Id == id);



            GetHabitatDTO getHabitatDTO = _mapper.Map<GetHabitatDTO>(habitat);
            
            getHabitatDTO.SpiritIds = habitat.Spirits.Select(x => x.Id).ToList();

            return getHabitatDTO;
        }

        public async Task Update(UpdateHabitatDTO updateHabitatDTO)
        {
            Habitat habitat = _mapper.Map<Habitat>(updateHabitatDTO);
            habitat.LastUpdated = DateTime.Now;
            _dbContext.Update(habitat);
            var border = _dbContext.BorderPoints.Where(x => x.Habitat.Id == updateHabitatDTO.Id);
            _dbContext.BorderPoints.RemoveRange(border);
            var marker = _dbContext.MarkerPoints.Where(x => x.Habitat.Id == updateHabitatDTO.Id).FirstOrDefault();
            if(marker != null)
            {
                _dbContext.MarkerPoints.Remove(marker);
            }
            var newMarker = _mapper.Map<MarkerPoint>(updateHabitatDTO.MarkerLocation);
            newMarker.Habitat = habitat;
            _dbContext.MarkerPoints.Add(newMarker);
            _dbContext.SaveChanges(); 
        }
        public List<GetHabitatDTO> GetAll()
        {
            var habitats = _dbContext.Habitats
                .Include(h => h.MarkerLocation)
                .Include(h=>h.Border)
                .Include(h=>h.Spirits)
                .ToList();
            List<GetHabitatDTO> getHabitatDTOs = habitats.Select(x => _mapper.Map<GetHabitatDTO>(x)).ToList();
            for(int i = 0;i < habitats.Count;i++)
            {
                getHabitatDTOs[i].SpiritIds = habitats[i].Spirits.Select(x => x.Id).ToList();
            }
            return getHabitatDTOs;
        }

    }
}
