﻿using System;

namespace Api.Application.Models
{
    public class ChartWithEma
    {
        public long Time { get; set; }
        public double? EmaValue { get; set; }
        public DateTime DateTime { get; set; }
        public double Open { get; set; }
        public double Close { get; set; }
        public double Low { get; set; }
        public double High { get; set; }
        public string TypeAction { get; set; }
    }
}
