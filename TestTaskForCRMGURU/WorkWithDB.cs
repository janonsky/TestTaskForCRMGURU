using LinqToDB;
using LinqToDB.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TestTaskForCRMGURU.EntitiesDB;

namespace TestTaskForCRMGURU
{
    /// <summary>
    /// Работа с базой данных.
    /// </summary>
    class WorkWithDB
    {
        /// <summary>
        /// Создает объект.
        /// </summary>
        public WorkWithDB()
        {
        }

        /// <summary>
        /// Возвращает список стран.
        /// </summary>
        /// <returns> страны.</returns>
        public IEnumerable<Country> GetCountries()
        {
            Configuration.Linq.AllowMultipleQuery = true;

            try
            {
                using (var data = new ConnectToDb())
                {
                    var countries = data.Country.
                        LoadWith(x => x.Regions).
                        LoadWith(y => y.Cities);

                    return countries.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("При выводе стран произошла ошибка.", ex);
            }

        }

        /// <summary>
        /// Вставка Странны в бд.
        /// </summary>
        /// <param name="name"> Название страны.</param>
        /// <param name="capital"> Название столицы.</param>
        /// <param name="region"> Название региона.</param>
        /// <param name="code"> Код региона.</param>
        /// <param name="popultion"> Численность населения.</param>
        /// <param name="area"> Площадь.</param>
        public void InsertCountry(string name, string capital, string region, string code, int popultion, float area)
        {
            var capitalGuid = CheckDuplicateCapital(capital);
            var regionGuid = CheckDuplicateRegion(region);
            var countryGuid = CheckDuplicateCountry(code);

            using (var data = new ConnectToDb())
            {
                try
                {
                    if (capitalGuid.Item2)
                    {
                        data.City.
                        Value(x => x.CityId, capitalGuid.Item1).
                        Value(x => x.Name, capital).
                        Insert();
                    }

                    if (regionGuid.Item2)
                    {
                        data.Region.
                        Value(x => x.RegionId, regionGuid.Item1).
                        Value(x => x.Name, region).
                        Insert();
                    }

                    if (countryGuid.Item2)
                    {
                        data.Country.
                        Value(x => x.CountryId, countryGuid.Item1).
                        Value(x => x.CapitalId, capitalGuid.Item1).
                        Value(x => x.Name, name).
                        Value(x => x.RegionId, regionGuid.Item1).
                        Value(x => x.Code, code).
                        Value(x => x.Population, popultion).
                        Value(x => x.Area, area).
                        Insert();
                    }
                    else
                    {
                        UpdateCountry(regionGuid.Item1, capitalGuid.Item1, countryGuid.Item1, name, capital, region, code, popultion, area);
                    }
                }
                catch (Exception e)
                {

                    throw new Exception("При вставке произошла ошибка.", e);
                }
            }
        }

        private void  UpdateCountry(Guid regionGuid,Guid capitalGuid, Guid countryId,string name, 
            string capital, string region, string code, int popultion, float area)
        {
            using (var data = new ConnectToDb())
            {
                data.Country.
                    Where(x => x.Code == code).
                    Set(x => x.CountryId, countryId).
                    Set(x => x.Name, name).
                    Set(x => x.Code, code).
                    Set(x => x.CapitalId, capitalGuid).
                    Set(x => x.Area, area).
                    Set(x => x.Population, popultion).
                    Set(x => x.RegionId, regionGuid).
                    Update();
            }
        }

        private IEnumerable<City> GetCapital()
        {
            using (var data = new ConnectToDb())
            {
                var country = data.City.
                    ToList();
                return country;
            }
        }

        private IEnumerable<Region> GetRegion()
        {
            using (var data = new ConnectToDb())
            {
                var region = data.Region.
                    ToList();
                return region;
            }
        }

        private IEnumerable<Country> GetCountry()
        {
            using (var data = new ConnectToDb())
            {
                var country = data.Country.
                    ToList();
                return country;
            }
        }

        private (Guid, bool) CheckDuplicateCapital(string str)
        {
            foreach (var el in GetCapital())
            {
                if (el.Name.Equals(str))
                {
                    return (el.CityId, false);
                }
            }

            return (Guid.NewGuid(), true);
        }

        private (Guid, bool) CheckDuplicateRegion(string str)
        {
            foreach (var el in GetRegion())
            {
                if (el.Name.Equals(str))
                {
                    return (el.RegionId, false);
                }
            }
            return (Guid.NewGuid(), true);
        }

        private (Guid, bool) CheckDuplicateCountry(string str)
        {
            foreach (var el in GetCountry())
            {
                if (el.Code.Equals(str))

                {
                    return (el.CountryId, false);
                }
            }
            return (Guid.NewGuid(), true);
        }
    }
}
