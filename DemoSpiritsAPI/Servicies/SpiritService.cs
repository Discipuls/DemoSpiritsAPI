using DemoSpiritsAPI.EntiryFramework.Contexts;
using DemoSpiritsAPI.Models;
using DemoSpiritsAPI.Servicies.Interfaces;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using AutoMapper;
using DemoSpiritsAPI.DTOs.SpiritDTOs;
using Microsoft.EntityFrameworkCore;
using DemoSpiritsAPI.DTOs.HabitatDTOs;
using DemoSpiritsAPI.DTOs.GeoPointDTOs;

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
        {/*
            var CreateHabitatsDTOs = new List<CreateHabitatDTO>();
            
            CreateHabitatsDTOs.Add(new CreateHabitatDTO {
                Border =             {new CreateGeoPointDTO{Latitude=26.2005218,Longitude=55.0035572},new CreateGeoPointDTO{Latitude=26.0576995,Longitude=54.9499634},new CreateGeoPointDTO{Latitude=25.8874114,Longitude=54.934187},new CreateGeoPointDTO{Latitude=25.7006438,Longitude=54.8045861},new CreateGeoPointDTO{Latitude=25.739096,Longitude=54.5218216},new CreateGeoPointDTO{Latitude=25.5907805,Longitude=54.4100823},new CreateGeoPointDTO{Latitude=25.5468352,Longitude=54.3044485},new CreateGeoPointDTO{Latitude=25.7336028,Longitude=54.2755921},new CreateGeoPointDTO{Latitude=25.7775481,Longitude=54.2113944},new CreateGeoPointDTO{Latitude=25.6566985,Longitude=54.127788},new CreateGeoPointDTO{Latitude=25.5028899,Longitude=54.1760431},new CreateGeoPointDTO{Latitude=25.5907805,Longitude=54.2563433},new CreateGeoPointDTO{Latitude=25.4644378,Longitude=54.2884197},new CreateGeoPointDTO{Latitude=25.2282317,Longitude=54.249925},new CreateGeoPointDTO{Latitude=25.1643192,Longitude=54.1959284},new CreateGeoPointDTO{Latitude=25.0956547,Longitude=54.144479},new CreateGeoPointDTO{Latitude=24.9775517,Longitude=54.1460878},new CreateGeoPointDTO{Latitude=24.9628808,Longitude=54.1698986},new CreateGeoPointDTO{Latitude=24.9120691,Longitude=54.1602507},new CreateGeoPointDTO{Latitude=24.8379114,Longitude=54.1449701},new CreateGeoPointDTO{Latitude=24.7706201,Longitude=54.1107696},new CreateGeoPointDTO{Latitude=24.8454645,Longitude=54.0285731},new CreateGeoPointDTO{Latitude=24.7026422,Longitude=53.9631834},new CreateGeoPointDTO{Latitude=24.6930292,Longitude=54.0164716},new CreateGeoPointDTO{Latitude=24.513517,Longitude=53.9572525},new CreateGeoPointDTO{Latitude=24.4448525,Longitude=53.8925601},new CreateGeoPointDTO{Latitude=24.1894203,Longitude=53.9524041},new CreateGeoPointDTO{Latitude=23.9422279,Longitude=53.9184491},new CreateGeoPointDTO{Latitude=23.9120155,Longitude=53.965332},new CreateGeoPointDTO{Latitude=23.7994057,Longitude=53.8974155},new CreateGeoPointDTO{Latitude=23.6626759,Longitude=53.9349089},new CreateGeoPointDTO{Latitude=23.5143605,Longitude=53.9478423},new CreateGeoPointDTO{Latitude=23.5473195,Longitude=53.7729031},new CreateGeoPointDTO{Latitude=23.6352101,Longitude=53.5613569},new CreateGeoPointDTO{Latitude=23.8384572,Longitude=53.2502609},new CreateGeoPointDTO{Latitude=23.8902083,Longitude=53.0736536},new CreateGeoPointDTO{Latitude=23.9259138,Longitude=52.8619208},new CreateGeoPointDTO{Latitude=23.9478865,Longitude=52.7756079},new CreateGeoPointDTO{Latitude=24.1345506,Longitude=52.805573},new CreateGeoPointDTO{Latitude=24.2348009,Longitude=52.7582268},new CreateGeoPointDTO{Latitude=24.3885904,Longitude=52.771795},new CreateGeoPointDTO{Latitude=24.50944,Longitude=52.7485256},new CreateGeoPointDTO{Latitude=24.5808512,Longitude=52.7626549},new CreateGeoPointDTO{Latitude=24.5712381,Longitude=52.8771825},new CreateGeoPointDTO{Latitude=24.6591287,Longitude=52.9111506},new CreateGeoPointDTO{Latitude=24.641276,Longitude=52.9517118},new CreateGeoPointDTO{Latitude=24.7236734,Longitude=52.9748725},new CreateGeoPointDTO{Latitude=24.7621256,Longitude=52.941782},new CreateGeoPointDTO{Latitude=24.877482,Longitude=52.9566758},new CreateGeoPointDTO{Latitude=25.0010782,Longitude=52.9268829},new CreateGeoPointDTO{Latitude=25.0312906,Longitude=52.8904415},new CreateGeoPointDTO{Latitude=25.0642496,Longitude=52.9277108},new CreateGeoPointDTO{Latitude=25.0697428,Longitude=52.8340626},new CreateGeoPointDTO{Latitude=25.1507669,Longitude=52.8589447},new CreateGeoPointDTO{Latitude=25.2084451,Longitude=52.8440171},new CreateGeoPointDTO{Latitude=25.235911,Longitude=52.8771825},new CreateGeoPointDTO{Latitude=25.278483,Longitude=52.8813263},new CreateGeoPointDTO{Latitude=25.3334146,Longitude=52.8315736},new CreateGeoPointDTO{Latitude=25.363627,Longitude=52.8647484},new CreateGeoPointDTO{Latitude=25.3196817,Longitude=52.8746959},new CreateGeoPointDTO{Latitude=25.3169351,Longitude=52.8937556},new CreateGeoPointDTO{Latitude=25.3498941,Longitude=52.9210875},new CreateGeoPointDTO{Latitude=25.3169351,Longitude=52.9376438},new CreateGeoPointDTO{Latitude=25.4089456,Longitude=52.9450922},new CreateGeoPointDTO{Latitude=25.4597574,Longitude=53.0071115},new CreateGeoPointDTO{Latitude=25.5325418,Longitude=53.0236349},new CreateGeoPointDTO{Latitude=25.5929666,Longitude=53.0607896},new CreateGeoPointDTO{Latitude=25.6010904,Longitude=53.1684804},new CreateGeoPointDTO{Latitude=25.6230631,Longitude=53.3082028},new CreateGeoPointDTO{Latitude=25.7109537,Longitude=53.3852654},new CreateGeoPointDTO{Latitude=25.9032144,Longitude=53.4032812},new CreateGeoPointDTO{Latitude=26.1970987,Longitude=53.3656031},new CreateGeoPointDTO{Latitude=26.3701334,Longitude=53.3574078},new CreateGeoPointDTO{Latitude=26.5404215,Longitude=53.3934554},new CreateGeoPointDTO{Latitude=26.5733804,Longitude=53.4883437},new CreateGeoPointDTO{Latitude=26.4662637,Longitude=53.6107301},new CreateGeoPointDTO{Latitude=26.2959757,Longitude=53.7165121},new CreateGeoPointDTO{Latitude=26.2311377,Longitude=53.8641505},new CreateGeoPointDTO{Latitude=26.364347,Longitude=53.8941035},new CreateGeoPointDTO{Latitude=26.37396,Longitude=53.9280783},new CreateGeoPointDTO{Latitude=26.4247718,Longitude=53.9547534},new CreateGeoPointDTO{Latitude=26.4014258,Longitude=54.0137013},new CreateGeoPointDTO{Latitude=26.3258948,Longitude=53.985449},new CreateGeoPointDTO{Latitude=26.2943091,Longitude=53.9676806},new CreateGeoPointDTO{Latitude=26.3286414,Longitude=54.0548346},new CreateGeoPointDTO{Latitude=26.2187781,Longitude=54.0338698},new CreateGeoPointDTO{Latitude=26.1652198,Longitude=54.0185427},new CreateGeoPointDTO{Latitude=26.1336341,Longitude=54.0951219},new CreateGeoPointDTO{Latitude=26.1336341,Longitude=54.1305424},new CreateGeoPointDTO{Latitude=26.1363807,Longitude=54.1611084},new CreateGeoPointDTO{Latitude=26.0745826,Longitude=54.1410017},new CreateGeoPointDTO{Latitude=26.1089148,Longitude=54.1055902},new CreateGeoPointDTO{Latitude=26.0196509,Longitude=54.1353701},new CreateGeoPointDTO{Latitude=26.0181769,Longitude=54.1592728},new CreateGeoPointDTO{Latitude=26.0580023,Longitude=54.21231},new CreateGeoPointDTO{Latitude=26.0786017,Longitude=54.1821836},new CreateGeoPointDTO{Latitude=26.1026343,Longitude=54.1845945},new CreateGeoPointDTO{Latitude=26.1795386,Longitude=54.2251572},new CreateGeoPointDTO{Latitude=26.2433966,Longitude=54.2255586},new CreateGeoPointDTO{Latitude=26.306568,Longitude=54.2030735},new CreateGeoPointDTO{Latitude=26.3477667,Longitude=54.21231},new CreateGeoPointDTO{Latitude=26.4269197,Longitude=54.2799514},new CreateGeoPointDTO{Latitude=26.494211,Longitude=54.3152138},new CreateGeoPointDTO{Latitude=26.5257967,Longitude=54.3736508},new CreateGeoPointDTO{Latitude=26.6150606,Longitude=54.421619},new CreateGeoPointDTO{Latitude=26.6535127,Longitude=54.5070232},new CreateGeoPointDTO{Latitude=26.6601572,Longitude=54.5951466},new CreateGeoPointDTO{Latitude=26.6903696,Longitude=54.6508017},new CreateGeoPointDTO{Latitude=26.5983591,Longitude=54.6452396},new CreateGeoPointDTO{Latitude=26.5406809,Longitude=54.7166941},new CreateGeoPointDTO{Latitude=26.4549323,Longitude=54.8217607},new CreateGeoPointDTO{Latitude=26.4192268,Longitude=54.8320448},new CreateGeoPointDTO{Latitude=26.368415,Longitude=54.8225519},new CreateGeoPointDTO{Latitude=26.3450691,Longitude=54.8533956},new CreateGeoPointDTO{Latitude=26.3653523,Longitude=54.8851165},new CreateGeoPointDTO{Latitude=26.3083607,Longitude=54.923805},new CreateGeoPointDTO{Latitude=26.2630421,Longitude=54.9470791},new CreateGeoPointDTO{Latitude=26.243816,Longitude=54.9857081},new CreateGeoPointDTO{Latitude=26.212917,Longitude=55.0002829},new CreateGeoPointDTO{Latitude=26.2005218,Longitude=55.0035572}},
                MarkerLocation = 
        });


            var CreateSpiritsDTOs = new List<CreateSpiritDTO>();

            CreateSpiritsDTOs.Add(new CreateSpiritDTO {
                Name = "Зазоўка",
                Description = "Зазоўкі завабліваюць мужчын у гушчар сваёй прыгажосцю ды галіз­ной. Толькі даўгія валасы прыкрываюць самыя далікатныя часткі цела – што, вядома, пры­ваблівае мужчын яшчэ больш. Зазоўкі клічуць мужчын на імя, абяцаюць незвычай­ныя адчуванні. Хтосьці трапляе ў багну ці ў пастку, бо крочыць за прыгажуняй, не разбіраючы шляху. Хтосьці атрымлі­\r\nвае жаданае.\r\nТолькі пасля любошчаў Зазоўкі\r\nмужчына вяртаецца дадому не зусім сабой. Ён ужо не будзе жыць, як ра­ ней. З часам зноў сыдзе ў лес на по­ шукі чароўнай красуні. І болей ужо не вернецца.\r\n",
                Classification = { SpiritType.Forest },
                CardImage = System.IO.File.ReadAllBytes(".\\Resources\\Images\\TestData" + "zazoukat.png"),
                MarkerImage = System.IO.File.ReadAllBytes(".\\Resources\\Images\\TestData" + "zazouka.png"),
                HabitatsIds = { 0}
            });
                
                (27.056924, 53.967102,
                ,
                , "zazouka.png",
                new List<SpiritsClassification> { SpiritsClassification.forest },
                new List<string> { "Ракаў" }));

            Spirits.Add(new Spirit(27.090737, 53.151117,
                "Аднарог",
                "Апошні аднарог у Еўропе быў упаляваны менавіта ў Беларусі. Упаляваць аднарога можна толькі прывабіў­шы яго спевам юнай нявінніцы.\r\nРог аднарога, як вядома, мог лекаваць раны ды хваробы, ратаваць ад атруты.\r\n"
                , "adnaroh.png",
                new List<SpiritsClassification> { SpiritsClassification.forest },
                new List<string> { "Копыль " }));

            Spirits.Add(new Spirit(27.097120, 53.154631,
                "Азярніцы",
                "АЗЯРНіЦЫ, міфічныя насельніцы Чорнага возера, што знаходзіцца у лесе, недалёка ад вёскі Брусы Мядзельскага р-на. Уяуляліся у выглядзе маладых жанчын з доугімі зеленаватымі валасамі, цемнай скураю і ступнямі ў выглядзе плаўнікоў.\r\nНа дотык вельмі халодныя і замест крыві ў іх нібыта вада. Апранутыя ў  сукні, сплеценыя з багавіння. Выходвячы на бераг, размауляюць на незразумелай, як быццам птушынай,\r\nмове і спяваюць падобна да салоўкаў.\r\nЧалавек, які ўбачыць азярніц, не павінен выдаць сябе, каб не быць зацягнутым імі ў  багну. Гэта награжае і тым, хто купаецца ў  возеры. Азярніцы сваіх ахвар не адпускаюць.\r\n"
                , "aziarnizy.png",
                new List<SpiritsClassification> { SpiritsClassification.water },
                new List<string> { "веска Брусы, Копыльскі раен" }));

            Spirits.Add(new Spirit(26.907148, 54.855610,
                "Апівень",
                "АПІВЕНЬ, нячысцік, які чапляецца да людей, схіляючы іх да пʼянства.\r\nПрысутнічае на усіх бяседах, сочыць за тым, хто колькі выпівае, падбухторвае не надта пітушчых выпіць паболей. Калі яму гэта не ўдаецца, падсыпае ў чарку нейкага зелля, пасля чаго чалавек робіцца зусім пʼяны. Апівень любіць забаўляцца з пʼянымі, дражніць іх і скідвае пад стол. Пабачыць апіўня можна толькі на добрым падпітку.\r\nЯго выгляд спалучае ў сабе антрапа - зааморфныя рысы. Ен з‘яўляецца невялічкай істотаю, парослай цёмнай, рэдкай поўсцю. Галава па форме нагадвала чалавечую, толькі са свіным рылам, а там, дзе павінны быць бровы — тырчаць маленькія, як у маладога бычка, рожкі. Ёсць у апіўня  хвосцік, закручаны, нібы ў парсючка, ножкі з капытцамі. Ходзіць проста або ракам.\r\nСцвярджаюць, што няма на свеце п'яніцы, які б здолелі перапіць апіўня. Сам жа нячысцік, колькі б ні выпіў, надта пʼяным не будзе. Пра чалавека, які шмат п'е, але не п'янее, гавораць, што ён п'е, як апівень.\r\n"
                , "apivien.png",
                new List<SpiritsClassification> { SpiritsClassification.home },
                new List<string> { "Мядзельскі раён" }));

            Spirits.Add(new Spirit(24.667601, 53.685770,
                "Лесавік",
                "У беларускай міфалогіі дух - увасабленне лесу як часткі прасторы, патэнцыйна чужой чалавеку. Кожны лясны масіў мае сваяго гаспадара - Лесавіка, які апякуецца ўсімі звярамі і птушкамі(ратуе іх ад пажару, паляўнічых і г.д.)"
                , "liesavik.png",
                new List<SpiritsClassification> { SpiritsClassification.forest },
                new List<string> { "Гродзенская вобласць" }));*/
        }
    }
}
