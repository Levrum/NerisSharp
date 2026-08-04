using System.Collections.Generic;

namespace NerisSharp.Models.ElementModels
{
    public abstract class NerisPageSet
    {
        public int Page_Size { get; set; }
        public int Page_Count { get; set; }
        public int Page_Number { get; set; }
        public int Total_Count { get; set; }
    }

    public class EntityPageSet : NerisPageSet
    {
        public List<EntityModel> Entities { get; set; }
    }
}
