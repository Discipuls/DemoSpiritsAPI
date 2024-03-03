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
            _dbContext.Spirits.Add(spirit);
            _dbContext.SaveChanges();
            spirit.CardImageName = "CardImage_" + spirit.Id.ToString() + "_.png";
            _dbContext.SaveChanges();
            MemoryStream cardImageMS = new MemoryStream(spirit.CardImage);
            Image cardImage = Image.FromStream(cardImageMS);
            cardImage.Save(spirit.CardImageName, ImageFormat.Png);
        }

        public async Task Delete(int id)
        {
            var spirit = _dbContext.Spirits.Find(id);
            _dbContext.Spirits.Remove(spirit);
            _dbContext.SaveChanges();
        }

        public GetSpiritDTO Get(int id)
        {
            var spirit = _dbContext.Spirits.Find(id);
            spirit.CardImage = System.IO.File.ReadAllBytes(".\\Resources\\Images\\kukat.png");
            GetSpiritDTO getSpiritDTO = _mapper.Map<GetSpiritDTO>(spirit);
            return getSpiritDTO;
        }

        public List<GetSpiritDTO> GetAll()
        {
            var spirit = _dbContext.Spirits.ToList();
            List<GetSpiritDTO> getSpiritDTOs = spirit.Select(x => _mapper.Map<GetSpiritDTO>(x)).ToList();
            return getSpiritDTOs;
        }

        public async Task Update(UpdateSpiritDTO updateSpiritDTO)
        {
            Spirit spirit = _mapper.Map<Spirit>(updateSpiritDTO);
            _dbContext.Update(spirit);
            _dbContext.SaveChanges();
        }
    }
}
