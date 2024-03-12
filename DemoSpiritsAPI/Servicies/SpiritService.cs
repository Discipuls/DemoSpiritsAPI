using DemoSpiritsAPI.EntiryFramework.Contexts;
using SpiritsClassLibrary.Models;
using DemoSpiritsAPI.Servicies.Interfaces;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using AutoMapper;
using SpiritsClassLibrary.DTOs.SpiritDTOs;
using Microsoft.EntityFrameworkCore;
using SpiritsClassLibrary.DTOs.HabitatDTOs;
using SpiritsClassLibrary.DTOs.GeoPointDTOs;

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
                File.Delete(".\\Resources\\Images\\" + spirit.CardImageName);
                File.Delete(".\\Resources\\Images\\" + spirit.MarkerImageName);
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
            var spirit = _dbContext.Spirits
                .Include(s => s.Habitats)
                .Include(s => s.MarkerLocation)
                .FirstOrDefault(s => s.Id == id);
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
            var spirits = _dbContext.Spirits
                .Include(s => s.Habitats)
                .Include(s => s.MarkerLocation)
                .ToList();
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


            var oldCardImage = System.IO.File.ReadAllBytes(".\\Resources\\Images\\" + spirit.CardImageName);
            var oldMarkerImage = System.IO.File.ReadAllBytes(".\\Resources\\Images\\" + spirit.MarkerImageName);

            try
            {
                MemoryStream cardImageMS = new MemoryStream(spirit.CardImage);
                Image cardImage = Image.FromStream(cardImageMS);
                File.Delete(".\\Resources\\Images\\" + spirit.CardImageName);
                cardImage.Save(".\\Resources\\Images\\" + spirit.CardImageName, ImageFormat.Png);
            }
            catch (Exception ex)
            {
                MemoryStream cardImageMS = new MemoryStream(oldCardImage);
                Image cardImage = Image.FromStream(cardImageMS);
                File.Delete(".\\Resources\\Images\\" + spirit.CardImageName);
                cardImage.Save(".\\Resources\\Images\\" + spirit.CardImageName, ImageFormat.Png);
            }

            try
            {
                MemoryStream markerImageMS = new MemoryStream(spirit.MarkerImage);
                Image markerImage = Image.FromStream(markerImageMS);
                File.Delete(".\\Resources\\Images\\" + spirit.MarkerImageName);
                markerImage.Save(".\\Resources\\Images\\" + spirit.MarkerImageName, ImageFormat.Png);
            }
            catch (Exception ex)
            {

                MemoryStream markerImageMS = new MemoryStream(oldMarkerImage);
                Image markerImage = Image.FromStream(markerImageMS);
                File.Delete(".\\Resources\\Images\\" + spirit.MarkerImageName);
                markerImage.Save(".\\Resources\\Images\\" + spirit.MarkerImageName, ImageFormat.Png);
            }

            _dbContext.Update(spirit);


            var marker = _dbContext.MarkerPoints.Where(x => x.spirit.Id == updateSpiritDTO.Id).FirstOrDefault();
            if (marker != null)
            {
                _dbContext.MarkerPoints.Remove(marker);
            }
            var newMarker = _mapper.Map<MarkerPoint>(updateSpiritDTO.MarkerLocation);
            newMarker.spirit = spirit;
            _dbContext.MarkerPoints.Add(newMarker);



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

        public async Task SetupTestData()
        {
            var CreateSpiritsDTOs = new List<CreateSpiritDTO>();

            CreateSpiritsDTOs.Add(new CreateSpiritDTO {
                Name = "Зазоўка",
                Description = "Зазоўкі завабліваюць мужчын у гушчар сваёй прыгажосцю ды галіз­ной. Толькі даўгія валасы прыкрываюць самыя далікатныя часткі цела – што, вядома, пры­ваблівае мужчын яшчэ больш. Зазоўкі клічуць мужчын на імя, абяцаюць незвычай­ныя адчуванні. Хтосьці трапляе ў багну ці ў пастку, бо крочыць за прыгажуняй, не разбіраючы шляху. Хтосьці атрымлі­\r\nвае жаданае.\r\nТолькі пасля любошчаў Зазоўкі\r\nмужчына вяртаецца дадому не зусім сабой. Ён ужо не будзе жыць, як ра­ ней. З часам зноў сыдзе ў лес на по­ шукі чароўнай красуні. І болей ужо не вернецца.\r\n",
                Classification = new List<SpiritType> { SpiritType.Forest },
                CardImage = System.IO.File.ReadAllBytes(".\\Resources\\Images\\TestData\\" + "zazoukat.png"),
                MarkerImage = System.IO.File.ReadAllBytes(".\\Resources\\Images\\TestData\\" + "zazouka.png"),
                HabitatsIds = new List<int>{ _dbContext.Habitats.Where(h => h.Name == "Polygon1").First().Id },
                MarkerLocation = new CreateGeoPointDTO { Latitude = 27.056924 , Longitude= 53.967102 }
            });

            CreateSpiritsDTOs.Add(new CreateSpiritDTO
            {
                Name = "Аднарог",
                Description = "Апошні аднарог у Еўропе быў упаляваны менавіта ў Беларусі. Упаляваць аднарога можна толькі прывабіў­шы яго спевам юнай нявінніцы.\r\nРог аднарога, як вядома, мог лекаваць раны ды хваробы, ратаваць ад атруты.\r\n",
                Classification = new List<SpiritType> { SpiritType.Forest },
                CardImage = System.IO.File.ReadAllBytes(".\\Resources\\Images\\TestData\\" + "adnaroht.png"),
                MarkerImage = System.IO.File.ReadAllBytes(".\\Resources\\Images\\TestData\\" + "adnaroh.png"),
                HabitatsIds = new List<int> { _dbContext.Habitats.Where(h => h.Name == "Polygon2").First().Id },
                MarkerLocation = new CreateGeoPointDTO { Latitude = 27.090737, Longitude = 53.151117 }
            });


            CreateSpiritsDTOs.Add(new CreateSpiritDTO
            {
                Name = "Азярніцы",
                Description = "АЗЯРНіЦЫ, міфічныя насельніцы Чорнага возера, што знаходзіцца у лесе, недалёка ад вёскі Брусы Мядзельскага р-на. Уяуляліся у выглядзе маладых жанчын з доугімі зеленаватымі валасамі, цемнай скураю і ступнямі ў выглядзе плаўнікоў.\r\nНа дотык вельмі халодныя і замест крыві ў іх нібыта вада. Апранутыя ў  сукні, сплеценыя з багавіння. Выходвячы на бераг, размауляюць на незразумелай, як быццам птушынай,\r\nмове і спяваюць падобна да салоўкаў.\r\nЧалавек, які ўбачыць азярніц, не павінен выдаць сябе, каб не быць зацягнутым імі ў  багну. Гэта награжае і тым, хто купаецца ў  возеры. Азярніцы сваіх ахвар не адпускаюць.\r\n",
                Classification = new List<SpiritType> { SpiritType.Water },
                CardImage = System.IO.File.ReadAllBytes(".\\Resources\\Images\\TestData\\" + "aziarnizyt.png"),
                MarkerImage = System.IO.File.ReadAllBytes(".\\Resources\\Images\\TestData\\" + "aziarnizy.png"),
                HabitatsIds = new List<int> { _dbContext.Habitats.Where(h => h.Name == "Polygon3").First().Id },
                MarkerLocation = new CreateGeoPointDTO { Latitude = 27.097120, Longitude = 53.154631 }
            });


            CreateSpiritsDTOs.Add(new CreateSpiritDTO
            {
                Name = "Апівень",
                Description = "АПІВЕНЬ, нячысцік, які чапляецца да людей, схіляючы іх да пʼянства.\r\nПрысутнічае на усіх бяседах, сочыць за тым, хто колькі выпівае, падбухторвае не надта пітушчых выпіць паболей. Калі яму гэта не ўдаецца, падсыпае ў чарку нейкага зелля, пасля чаго чалавек робіцца зусім пʼяны. Апівень любіць забаўляцца з пʼянымі, дражніць іх і скідвае пад стол. Пабачыць апіўня можна толькі на добрым падпітку.\r\nЯго выгляд спалучае ў сабе антрапа - зааморфныя рысы. Ен з‘яўляецца невялічкай істотаю, парослай цёмнай, рэдкай поўсцю. Галава па форме нагадвала чалавечую, толькі са свіным рылам, а там, дзе павінны быць бровы — тырчаць маленькія, як у маладога бычка, рожкі. Ёсць у апіўня  хвосцік, закручаны, нібы ў парсючка, ножкі з капытцамі. Ходзіць проста або ракам.\r\nСцвярджаюць, што няма на свеце п'яніцы, які б здолелі перапіць апіўня. Сам жа нячысцік, колькі б ні выпіў, надта пʼяным не будзе. Пра чалавека, які шмат п'е, але не п'янее, гавораць, што ён п'е, як апівень.\r\n",
                Classification = new List<SpiritType> { SpiritType.Home },
                CardImage = System.IO.File.ReadAllBytes(".\\Resources\\Images\\TestData\\" + "apivient.png"),
                MarkerImage = System.IO.File.ReadAllBytes(".\\Resources\\Images\\TestData\\" + "apivien.png"),
                HabitatsIds = new List<int> { _dbContext.Habitats.Where(h => h.Name == "Polygon1").First().Id, _dbContext.Habitats.Where(h => h.Name == "Polygon3").First().Id },
                MarkerLocation = new CreateGeoPointDTO { Latitude = 26.907148, Longitude = 54.855610 }
            });

            CreateSpiritsDTOs.Add(new CreateSpiritDTO
            {
                Name = "Лесавік",
                Description = "У беларускай міфалогіі дух - увасабленне лесу як часткі прасторы, патэнцыйна чужой чалавеку. Кожны лясны масіў мае сваяго гаспадара - Лесавіка, які апякуецца ўсімі звярамі і птушкамі(ратуе іх ад пажару, паляўнічых і г.д.)",
                Classification = new List<SpiritType> { SpiritType.Forest },
                CardImage = System.IO.File.ReadAllBytes(".\\Resources\\Images\\TestData\\" + "liesavikt.png"),
                MarkerImage = System.IO.File.ReadAllBytes(".\\Resources\\Images\\TestData\\" + "liesavik.png"),
                HabitatsIds = new List<int> { _dbContext.Habitats.Where(h => h.Name == "Polygon1").First().Id, _dbContext.Habitats.Where(h => h.Name == "Polygon3").First().Id },
                MarkerLocation = new CreateGeoPointDTO { Latitude = 26.907148, Longitude = 54.855610 }
            });

            foreach (var spirit in CreateSpiritsDTOs)
            {
                Create(spirit);
            }
                
        }
    }
}
