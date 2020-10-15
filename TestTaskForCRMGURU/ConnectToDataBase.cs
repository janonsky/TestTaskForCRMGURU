using LinqToDB;
using LinqToDB.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTaskForCRMGURU.EntitiesDB;

namespace TestTaskForCRMGURU
{
    /// <summary>
	/// Подключение к базе данных.
	/// </summary>
    class ConnectToDataBase : DataConnection
    {
		/// <summary>
		/// Создает объект.
		/// </summary>
		public ConnectToDataBase() : base("Country") { }

		/// <summary>
		/// Возвращает таблицу Region.
		/// </summary>
		public ITable<Region> Region => GetTable<Region>();

		/// <summary>
		/// Возвращает таблицу City.
		/// </summary>
		public ITable<City> City => GetTable<City>();

		/// <summary>
		/// Возвращает таблицу Country.
		/// </summary>
		public ITable<Country> Country => GetTable<Country>();

	}
}
