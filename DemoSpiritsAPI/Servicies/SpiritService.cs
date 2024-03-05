using DemoSpiritsAPI.EntiryFramework.Contexts;
using DemoSpiritsAPI.Models;
using DemoSpiritsAPI.Servicies.Interfaces;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using AutoMapper;
using DemoSpiritsAPI.DTOs.SpiritDTOs;

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

        public GetSpiritDTO Get(int id) // TODO Get spirits ids for DTO
        {
            var spirit = _dbContext.Spirits.Find(id);
            if(spirit == null)
            {
                throw new Exception($"Spirit entity with id {id} not found");
            }
            spirit.CardImage = System.IO.File.ReadAllBytes(".\\Resources\\Images\\" + spirit.CardImageName); 
            spirit.MarkerImage = System.IO.File.ReadAllBytes(".\\Resources\\Images\\" + spirit.MarkerImageName); 
            GetSpiritDTO getSpiritDTO = _mapper.Map<GetSpiritDTO>(spirit);
            return getSpiritDTO;
        }

        public List<GetSpiritBasicsDTO> GetAll() //TODO Get habitats ids for DTO
        {
            var spirit = _dbContext.Spirits.ToList();
            List<GetSpiritBasicsDTO> getSpiritDTOs = spirit.Select(x => _mapper.Map<GetSpiritBasicsDTO>(x)).ToList();
            return getSpiritDTOs;
        }

        public async Task Update(UpdateSpiritDTO updateSpiritDTO)
        {
            var tryGetSpirit = _dbContext.Spirits.Find(updateSpiritDTO.Id);
            if (tryGetSpirit == null)
            {
                throw new Exception($"Spirit entity with id {updateSpiritDTO.Id} not found");
            }

            Spirit spirit = _mapper.Map<Spirit>(updateSpiritDTO);
            spirit.LastUpdated = DateTime.Now;
            _dbContext.Update(spirit);
            _dbContext.SaveChanges();
        }
    }
}
