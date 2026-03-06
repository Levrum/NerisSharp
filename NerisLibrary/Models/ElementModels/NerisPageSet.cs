using System;
using System.Collections.Generic;
using System.Text;

namespace NerisLibrary.Models.ElementModels
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
