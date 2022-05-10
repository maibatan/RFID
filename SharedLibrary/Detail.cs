using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public class Detail
    {
        public int Id { get; set; }
        public int InventoryId { get; set; }
        public int AssetItemId { get; set; }
        public int ExpectedQuality { get; set; }
        public int PhysicalQuality { get; set; }
        public virtual AssetItem AssetItemNavigation { get; set; }
    }
}
