using DemoSpiritsAPI.EntiryFramework.Contexts;
using DemoSpiritsAPI.Models;
using DemoSpiritsAPI.Servicies.Interfaces;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using AutoMapper;
using DemoSpiritsAPI.DTOs.SpiritDTOs;
using Microsoft.EntityFrameworkCore;

namespace DemoSpiritsAPI.Servicies
{
    public class SpiritService : ISpiritService
    {
        private MySQLContext _dbContext;
        private IMapper _mapper;
        public SpiritService(MySQLContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task Create(CreateSpiritDTO createSpiritDTO)
        {
            Spirit spirit = _mapper.Map<Spirit>(createSpiritDTO);

         
            spirit.LastUpdated = DateTime.Now;
            _dbContext.Spirits.Add(spirit);
            _dbContext.SaveChanges();
            spirit.CardImageName = "CardImage_" + spirit.Id.ToString() + "_.png";
            spirit.MarkerImageName = "MarkerImage_" + spirit.Id.ToString() + "_.png";

            var relatedHabitats = _dbContext.Habitats.Where(h => createSpiritDTO.HabitatsIds.Contains(h.Id)).ToList();

            spirit.Habitats = relatedHabitats;
            _dbContext.Spirits.Update(spirit);
            _dbContext.SaveChanges();
            try
            {
                MemoryStream cardImageMS = new MemoryStream(spirit.CardImage);
                Image cardImage = Image.FromStream(cardImageMS);
                cardImage.Save(".\\Resources\\Images\\" + spirit.CardImageName, ImageFormat.Png);
            }catch (Exception ex)
            {
                _dbContext.Spirits.Remove(spirit);
                _dbContext.SaveChanges();
                throw new Exception("Problems with card Image");
            }

            try
            {
                MemoryStream markerImageMS = new MemoryStream(spirit.MarkerImage);
                Image markerImage = Image.FromStream(markerImageMS);
                markerImage.Save(".\\Resources\\Images\\" + spirit.MarkerImageName, ImageFormat.Png);
            }
            catch (Exception ex)
            {
                _dbContext.Spirits.Remove(spirit);
                _dbContext.SaveChanges();
                throw new Exception("Problems with marker Image");
            }



        }

        public async Task Delete(int id)
        {
            //TODO implement soft delete
            var spirit = _dbContext.Spirits.Find(id);
            if (spirit == null)
            {
                throw new Exception($"Spirit entity with id {id} not found");
            }
            try
            {
                File.Delete(spirit.CardImageName);
                File.Delete(spirit.MarkerImageName);
            }catch(Exception ex)
            {
                throw new Exception("Problems with deleting image resources");
            }

            System.IO.File.Delete(spirit.CardImageName);
            _dbContext.Spirits.Remove(spirit);
            _dbContext.SaveChanges();
        }

        public GetSpiritDTO Get(int id) 
        {
            var spirit = _dbContext.Spirits.Include(s => s.Habitats).FirstOrDefault(s => s.Id == id);
            if(spirit == null)
            {
                throw new Exception($"Spirit entity with id {id} not found");
            }
            spirit.CardImage = System.IO.File.ReadAllBytes(".\\Resources\\Images\\" + spirit.CardImageName); 
            spirit.MarkerImage = System.IO.File.ReadAllBytes(".\\Resources\\Images\\" + spirit.MarkerImageName); 
            GetSpiritDTO getSpiritDTO = _mapper.Map<GetSpiritDTO>(spirit);

            getSpiritDTO.HabitatsIds = spirit.Habitats.Select(h => h.Id).ToList();

            return getSpiritDTO;
        }

        public List<GetSpiritBasicsDTO> GetAll()
        {
            var spirits = _dbContext.Spirits.Include(s => s.Habitats).ToList();
            List<GetSpiritBasicsDTO> getSpiritDTOs = spirits.Select(x => _mapper.Map<GetSpiritBasicsDTO>(x)).ToList();

            for(int i = 0;i < spirits.Count; i++)
            {
                getSpiritDTOs[i].HabitatsIds = spirits[i].Habitats.Select(h => h.Id).ToList();
            }

            return getSpiritDTOs;
        }

        public async Task Update(UpdateSpiritDTO updateSpiritDTO)
        {
            var spiritExists = _dbContext.Spirits.Any(s => s.Id == updateSpiritDTO.Id);
            if (spiritExists == false)
            {
                throw new Exception($"Spirit entity with id {updateSpiritDTO.Id} not found");
            }
            
            var spirit = _mapper.Map<Spirit>(updateSpiritDTO);
            spirit.LastUpdated = DateTime.Now;
            spirit.CardImageName = "CardImage_" + spirit.Id.ToString() + "_.png";
            spirit.MarkerImageName = "MarkerImage_" + spirit.Id.ToString() + "_.png";
            _dbContext.Update(spirit);

            var relatedHabitats = _dbContext.Habitats
                .Where(h => h.Spirits.Contains(spirit))
                .Include(h => h.Spirits)
                .ToList();

            foreach(var h in spirit.Habitats)
            {
                h.Spirits.Remove(spirit);
            }
            _dbContext.SaveChanges();
            relatedHabitats = _dbContext.Habitats
                .Where(h => updateSpiritDTO.HabitatsIds.Contains(h.Id))
                .Include(h => h.Spirits)
                .ToList();
            spirit.Habitats = relatedHabitats;

            _dbContext.SaveChanges();
        }
    }
}
