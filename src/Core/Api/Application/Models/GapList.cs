using System.Collections.Generic;

namespace Api.Application.Models
{
    public class GapList
    {
        public List<string> TopGapSymbolEu { get; set; }
        public List<string> BottomGapSymbolEu { get; set; }
        public List<string> TopGapSymbolUs { get; set; }
        public List<string> BottomGapSymbolUs { get; set; }
    }
}
