using LinqToDB.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TestTaskForCRMGURU.EntitiesDB;

namespace TestTaskForCRMGURU
{
    /// <summary>
	/// Вызов реализованных методов.    
	/// </summary>
    class Utility
    {
        static void Main(string[] args)
        {
            GetChoosedAction();

            Console.ReadKey();
        }

        /// <summary>
        /// Диалог с пользователем и выбор им действий.
        /// </summary>
        private static void GetChoosedAction()
        {
            try
            {
                Console.WriteLine("Выберите действие \n Ввод страны - y \n Получение списка стран - нажмите любую клавишу.");

                var action = Console.ReadLine();

                if (action.Equals("y"))
                {
                    var country = GetInputContry();

                    PrintInfoAboutContry(country);

                    Console.WriteLine("Сохранить страну в базу данных? \n y - Да \n Для завершение программы нажмите любую клавишу");

                    var answer = Console.ReadLine();

                    if (answer.Equals("y"))
                    {
                        SaveCountryToDB(country);
                    }
                    else
                    {
                        Environment.Exit(0);
                    }
                }
                else
                {
                    GetListCountries();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Возвращает название страны.
        /// </summary>
        /// <returns></returns>
        private static string GetInputContry()
        {
            Console.WriteLine("Введите название страны.");
            var country = Console.ReadLine();

            return country;
        }

        /// <summary>
        /// Вовзвращает список стран.
        /// </summary>
        private static void GetListCountries()
        {
            var data = new CountryHelper();

            foreach (var country in data.GetCountries())
            {
                Console.WriteLine($"{country}");
            }
        }

        /// <summary>
        /// Сохранение страны в бд.
        /// </summary>
        /// <param name="country"> Название страны.</param>
        private static void SaveCountryToDB(string country)
        {
            InsertCountry(GetDataCountry(country));
        }

        /// <summary>
        /// Получение ответа от API.
        /// </summary>
        /// <param name="countryName"> Название страны. </param>
        /// <returns> Коллекция объектов страны.</returns>
        private static List<InformationAboutContry> GetWebResponse(string countryName)
        {
            try
            {
                var url = "https://restcountries.eu/rest/v2/name/" + countryName;
                var webRequest = HttpWebRequest.Create(url);
                var webResponse = webRequest.GetResponse();
                var response = "";

                using (var streamReader = new StreamReader(webResponse.GetResponseStream()))
                {
                    response = streamReader.ReadToEnd();
                }

                var country = JsonConvert.DeserializeObject<List<InformationAboutContry>>(response);
                return country;
            }
            catch (Exception exception)
            {
                throw new Exception("Страна не найденна.", exception);
            }
        }

        /// <summary>
        /// Вывод информации о выбранной стране.
        /// </summary>
        /// <param name="country"> Название страны.</param>
        private static void PrintInfoAboutContry(string country)
        {
            var data = GetWebResponse(country);

            foreach (var el in data)
            {
                Console.WriteLine(el.ToString());
            }
        }

        /// <summary>
        /// Вставка странны в бд.
        /// </summary>
        /// <param name="country"> Название страны.</param>
        private static void InsertCountry(InformationAboutContry country)
        {
            try
            {
                var workWithDB = new CountryHelper();
                workWithDB.InsertCountry(country);

                Console.WriteLine("Страна успешно сохраненна.");
            }
            catch (Exception exception)
            {
                throw new Exception("При сохранении страны произошла ошибка.", exception);
            }
        }

        /// <summary>
        /// Получение информации о стране.
        /// </summary>
        /// <param name="country"> Страна.</param>
        /// <returns> Страна.</returns>
        private static InformationAboutContry GetDataCountry(string country)
        {
            var data = GetWebResponse(country);

            foreach (var element in data)
            {
                var name = element.Name;
                var code = element.Alpha2Code;
                var capital = element.Capital;
                var region = element.Region;
                var population = element.Population;
                var area = element.Area;

                return new InformationAboutContry(name, code, capital, region, population, area);
            }
            return null;
        }
    }
}
