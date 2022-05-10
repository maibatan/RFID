using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public class Instance
    {
        public int Id { get; set; }
        public string TagId { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsAvailable { get; set; }
        public int DepartmentId { get; set; }
        public int AssetItemId { get; set; }
        public virtual AssetItem AssetItemNavigation { get; set; }
        public virtual Department DepartmentNavigation { get; set; }
    }
}
