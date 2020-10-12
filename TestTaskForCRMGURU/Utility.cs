using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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
                Console.WriteLine("Выберите действие \n Ввод странны - 1 \n Получение списка стран - 2.");

                var action = Convert.ToInt32(Console.ReadLine());

                if (action == 1)
                {
                    Console.WriteLine("Введите название страны.");

                    var country = Console.ReadLine();

                    PrintInfoAboutContry(country);

                    Console.WriteLine("Сохранить странну в базу данных? \n 1 - Да \n 2 - Нет.");

                    var answer = Convert.ToInt32(Console.ReadLine());

                    if (answer==1)
                    {
                        GetDataAndInsertCountry(country);
                    }
                }
                else if (action == 2)
                {
                    var db = new WorkWithDB();

                    foreach (var el in db.GetCountries())
                    {
                        Console.WriteLine($"{el}");
                    }
                }
                else
                {
                    throw new Exception("Выберите действие 1 или 2.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Для ввода доступны только числа (1,2)", ex);
            }
        }

        /// <summary>
        /// Получение ответа от API.
        /// </summary>
        /// <param name="country"> Страна</param>
        /// <returns> Коллекция объектов страны.</returns>
        private static List<InformationAboutContry> GetWebResponse(string country)
        {
            try
            {
                var url = "https://restcountries.eu/rest/v2/name/" + country;

                var webRequest = HttpWebRequest.Create(url);
                var webResponse = webRequest.GetResponse();

                var response = "";

                using (var streamReader = new StreamReader(webResponse.GetResponseStream()))
                {
                    response = streamReader.ReadToEnd();
                }

                var apiCountry = JsonConvert.DeserializeObject<List<InformationAboutContry>>(response);

                return apiCountry;
            }
            catch (Exception ex)
            {
                throw new Exception("Странна не найденна.", ex);
            }
        }

        /// <summary>
        /// Вывод информации о выбранной стране.
        /// </summary>
        /// <param name="country"> Страна.</param>
        private static void PrintInfoAboutContry(string country)
        {
            var data = GetWebResponse(country);

            foreach (var el in data)
            {
                Console.WriteLine(el.ToString());
            }
        }

        /// <summary>
        /// Получение информации о стране и вставка ее в бд.
        /// </summary>
        /// <param name="country"> Страна.</param>
        private static void GetDataAndInsertCountry(string country)
        {
            var data = GetWebResponse(country);

            foreach (var el in data)
            {
                var name = el.Name;
                var code = el.Alpha2Code;
                var capital = el.Capital;
                var region = el.Region;
                var population = el.Population;
                var area = el.Area;

                WorkWithDB workWithDB = new WorkWithDB();

                workWithDB.InsertCountry(name, capital, region, code, population, area);
            }

            Console.WriteLine("Страна успешно сохраненна.");
        }
    }
}
