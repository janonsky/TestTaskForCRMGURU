using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTaskForCRMGURU
{
    /// <summary>
    /// Информация о стране.
    /// </summary>
    class InformationAboutContry
    {
        /// <summary>
        /// Объект.
        /// </summary>
        /// <param name="name"> Название страны.</param>
        /// <param name="alpha2Code"> Код страны.</param>
        /// <param name="capital"> Название столицы.</param>
        /// <param name="region"> Название Регион.</param>
        /// <param name="population"> Численность населения.</param>
        /// <param name="area"> Площадь.</param>
        public InformationAboutContry(string name, string alpha2Code, string capital, string region, int population, float area)
        {
            Name = name;
            Alpha2Code = alpha2Code;
            Capital = capital;
            Region = region;
            Population = population;
            Area = area;
        }

        /// <summary>
        /// Название страны.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Код страны.
        /// </summary>
        public string Alpha2Code { get; set; }

        /// <summary>
        /// Столица.
        /// </summary>
        public string Capital { get; set; }

        /// <summary>
        /// Регион.
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// Численность населения.
        /// </summary>
        public int Population { get; set; }

        /// <summary>
        /// Площадь.
        /// </summary>
        public float Area { get; set; }

        /// <summary>
        /// Форматный вывод.
        /// </summary>
        /// <returns> Строку.</returns>
        public override string ToString()
        {
            return $"Название: {Name} Код: {Alpha2Code} Столица: {Capital} Регион: " +
                $"{Region} Численность населения: {Population} Площадь: {Area}";
        }
    }
}
