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
    class CountryHelper
    {
        /// <summary>
        /// Создает объект.
        /// </summary>
        public CountryHelper()
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
                using (var data = new ConnectToDataBase())
                {
                    var countries = data.Country.
                        LoadWith(x => x.Regions).
                        LoadWith(y => y.Cities);

                    return countries.ToList();
                }
            }
            catch (Exception exception)
            {
                throw new Exception("При выводе стран произошла ошибка.", exception);
            }

        }

        /// <summary>
        /// Вставка странны в бд.
        /// </summary>
        /// <param name="informationAboutContry"> Информация о стране.</param>
        public void InsertCountry(InformationAboutContry informationAboutContry)
        {
            var capitalGuid = CheckDuplicateCapital(informationAboutContry.Capital);
            var regionGuid = CheckDuplicateRegion(informationAboutContry.Region);
            var countryGuid = CheckDuplicateCountry(informationAboutContry.Alpha2Code);

            using (var data = new ConnectToDataBase())
            {
                try
                {
                    if (capitalGuid.Item2)
                    {
                        data.City.
                        Value(x => x.CityId, capitalGuid.Item1).
                        Value(x => x.Name, informationAboutContry.Capital).
                        Insert();
                    }

                    if (regionGuid.Item2)
                    {
                        data.Region.
                        Value(x => x.RegionId, regionGuid.Item1).
                        Value(x => x.Name, informationAboutContry.Region).
                        Insert();
                    }

                    if (countryGuid.Item2)
                    {
                        data.Country.
                        Value(x => x.CountryId, countryGuid.Item1).
                        Value(x => x.CapitalId, capitalGuid.Item1).
                        Value(x => x.Name, informationAboutContry.Name).
                        Value(x => x.RegionId, regionGuid.Item1).
                        Value(x => x.Code, informationAboutContry.Alpha2Code).
                        Value(x => x.Population, informationAboutContry.Population).
                        Value(x => x.Area, informationAboutContry.Area).
                        Insert();
                    }
                    else
                    {
                        UpdateCountry(regionGuid.Item1, capitalGuid.Item1, countryGuid.Item1, informationAboutContry.Name,
                            informationAboutContry.Capital, informationAboutContry.Region, informationAboutContry.Alpha2Code,
                            informationAboutContry.Population, informationAboutContry.Area);
                    }
                }
                catch (Exception exception)
                {
                    throw new Exception("При вставке произошла ошибка.", exception);
                }
            }
        }

        private void UpdateCountry(Guid regionGuid, Guid capitalGuid, Guid countryId, string name,
            string capital, string region, string code, int popultion, float area)
        {
            using (var data = new ConnectToDataBase())
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
            using (var data = new ConnectToDataBase())
            {
                var country = data.City.
                    ToList();
                return country;
            }
        }

        private IEnumerable<Region> GetRegion()
        {
            using (var data = new ConnectToDataBase())
            {
                var region = data.Region.
                    ToList();
                return region;
            }
        }

        private IEnumerable<Country> GetCountry()
        {
            using (var data = new ConnectToDataBase())
            {
                var country = data.Country.
                    ToList();
                return country;
            }
        }

        private (Guid, bool) CheckDuplicateCapital(string capital)
        {
            foreach (var element in GetCapital())
            {
                if (element.Name.Equals(capital))
                {
                    return (element.CityId, false);
                }
            }

            return (Guid.NewGuid(), true);
        }

        private (Guid, bool) CheckDuplicateRegion(string region)
        {
            foreach (var element in GetRegion())
            {
                if (element.Name.Equals(region))
                {
                    return (element.RegionId, false);
                }
            }
            return (Guid.NewGuid(), true);
        }

        private (Guid, bool) CheckDuplicateCountry(string country)
        {
            foreach (var el in GetCountry())
            {
                if (el.Code.Equals(country))
                {
                    return (el.CountryId, false);
                }
            }
            return (Guid.NewGuid(), true);
        }
    }
}
