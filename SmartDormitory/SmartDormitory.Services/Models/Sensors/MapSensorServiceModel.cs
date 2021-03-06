﻿using SmartDormitory.Data.Models;
using System;

namespace SmartDormitory.Services.Models.Sensors
{
    public class MapSensorServiceModel
    {
        public string Id { get; set; }

        public Coordinates Coordinates { get; set; }

        public string Name { get; set; }

        public bool IsPublic { get; set; }

        public string SensorType { get; set; }

        public DateTime CreatedOn { get; set; }

        public string UserId { get; set; }

        public float Value { get; set; }
    }
}
