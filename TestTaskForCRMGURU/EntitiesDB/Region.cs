using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTaskForCRMGURU.EntitiesDB
{
    /// <summary>
    /// Регион.
    /// </summary>
    [Table(Name = "Region")]
    class Region
    {
        /// <summary>
        /// Первичный ключ.
        /// </summary>
        [PrimaryKey]
        [Column(Name = "Region_Id")]
        public Guid RegionId { get; set; }

        /// <summary>
        /// Название.
        /// </summary>
        [Column(Name = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Association(ThisKey = "RegionId", OtherKey = "RegionId")]
        public virtual Country Country { get; set; }
    }
}
