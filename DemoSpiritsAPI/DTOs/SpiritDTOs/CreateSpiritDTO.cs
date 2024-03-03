﻿using DemoSpiritsAPI.Models;

namespace DemoSpiritsAPI.DTOs.SpiritDTOs
{
    public class CreateSpiritDTO
    {
        public string Name { get; set;  }
        public string Description {  get; set; }
        public List<SpiritType> Classification { get; set; }
        public List<int> HabitatsIds { get; set; }

        public Byte[] CardImage {  get; set; }
        public Byte[] MarkerImage {  get; set; }
    }
}
