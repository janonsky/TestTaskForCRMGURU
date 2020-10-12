using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTaskForCRMGURU.EntitiesDB
{
    /// <summary>
    /// Города.
    /// </summary>
    [Table(Name = "City")]
    class City
    {
        /// <summary>
        /// Первичный ключ.
        /// </summary>
        [PrimaryKey]
        [Column(Name = "City_Id")]
        public Guid CityId { get; set; }

        /// <summary>
        /// Название.
        /// </summary>
        [Column(Name = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// Связь с таблицей Country.
        /// </summary>
        [Association(ThisKey = "CityId", OtherKey = "CityId")]
        public virtual Country Country { get; set; }
    }
}
