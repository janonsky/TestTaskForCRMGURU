using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTaskForCRMGURU.EntitiesDB
{
    /// <summary>
    /// Страна.
    /// </summary>
    [Table(Name = "Country")]
    class Country
    {

        /// <summary>
        /// Первичный ключ.
        /// </summary>
        [PrimaryKey]
        [Column(Name = "Country_Id")]
        public Guid CountryId { get; set; }

        /// <summary>
        /// Название 
        /// </summary>
        [Column(Name = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// Код.
        /// </summary>
        [Column(Name = "Code")]
        public string Code { get; set; }

        /// <summary>
        /// Вторичный ключ.
        /// </summary>
        [Column(Name = "Capital_Id")]
        public Guid CapitalId { get; set; }

        /// <summary>
        /// Площадь.
        /// </summary>
        [Column(Name = "Area")]
        public float Area { get; set; }

        /// <summary>
        /// Численность населения.
        /// </summary>
        [Column(Name = "Population")]
        public int Population { get; set; }

        /// <summary>
        /// Вторичный ключ.
        /// </summary>
        [Column(Name = "Region_Id")]
        public Guid RegionId { get; set; }

        /// <summary>
        /// Связь с таблицей City.
        /// </summary>
        [Association(ThisKey = "CapitalId", OtherKey = "CityId")]
        public IEnumerable<City> Cities { get; set; }

        /// <summary>
        /// Связь с таблицей Region.
        /// </summary>
        [Association(ThisKey = "RegionId", OtherKey = "RegionId")]
        public IEnumerable<Region> Regions { get; set; }

        /// <summary>
        /// Форматный вывод.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append(Name).
               Append(" ").
               Append(Code).
               Append(" ");

            if (Cities != null)
            {
                foreach (var el in Cities)
                {
                    stringBuilder.Append(el.Name).
                       Append(" ");
                }
            }

            stringBuilder.Append(Area).
               Append(" ").
               Append(Population).
               Append(" ");

            if (Regions != null)
            {
                foreach (var el in Regions)
                {
                    stringBuilder.Append(el.Name).
                       Append(" ");
                }
            }

            return stringBuilder.ToString();
        }
    }
}
