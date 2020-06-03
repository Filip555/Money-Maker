using System.Collections.Generic;

namespace Api.Application.Models
{
    public class ChartView
    {
        public IEnumerable<dynamic> Series { get; set; }
    }
}
