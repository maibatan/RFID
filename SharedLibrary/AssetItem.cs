using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public class AssetItem
    {
    
        public int Id { get; set; }
        public string Code { get; set; }
        public string DisplayName { get; set; }
       
    }
}
