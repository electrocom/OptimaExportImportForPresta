/**
 * Copyright 2015-2017 NETCAT (www.netcat.pl)
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 * @author NETCAT <firma@netcat.pl>
 * @copyright 2015-2017 NETCAT (www.netcat.pl)
 * @license http://www.apache.org/licenses/LICENSE-2.0
 */

using System;
using System.Runtime.InteropServices;

namespace NIP24
{
	#region interface

	/// <summary>
	/// Interfejs danych firmy z systemu VIES
	/// </summary>
	[Guid("4CAB1F48-52C8-4F9A-B447-FAC754743597")]
	[ComVisible(true)]
	public interface IVIESData
    {
		/// <summary>
		/// Unikalny identyfikator odpowiedzi
		/// </summary>
		[DispId(1)]
		string UID { get; set; }

		/// <summary>
		/// Kod kraju
		/// </summary>
		[DispId(2)]
		string CountryCode { get; set; }

		/// <summary>
		/// Numer VAT ID
		/// </summary>
		[DispId(3)]
		string VATNumber { get; set; }

		/// <summary>
		/// Flaga aktywności firmy
		/// </summary>
		[DispId(4)]
		bool Valid { get; set; }

		/// <summary>
		/// Nazwa firmy
		/// </summary>
		[DispId(5)]
		string TraderName { get; set; }

		/// <summary>
		/// Typ firmy
		/// </summary>
		[DispId(6)]
		string TraderCompanyType { get; set; }

		/// <summary>
		/// Adres siedziby
		/// </summary>
		[DispId(7)]
		string TraderAddress { get; set; }

        /// <summary>
        /// Identyfikator sprawdzenia z systemu VIES
        /// </summary>
        [DispId(8)]
        string ID { get; set; }

        /// <summary>
        /// Data sprawdzenia
        /// </summary>
        [DispId(9)]
        DateTime Date { get; set; }

        /// <summary>
        /// Źródło informacji o statusie
        /// </summary>
        [DispId(10)]
        string Source { get; set; }

        [DispId(11)]
		string ToString();
    }

	#endregion

	#region implementation

	/// <summary>
	/// Dane firmy z systemu VIES
	/// </summary>
	[Guid("6F3DEB5D-846E-4F9A-B2D6-433CE0F2FC2E")]
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	public class VIESData : IVIESData
	{
		/// <summary>
		/// Unikalny identyfikator odpowiedzi
		/// </summary>
		public string UID { get; set; }

		/// <summary>
		/// Kod kraju
		/// </summary>
		public string CountryCode { get; set; }

		/// <summary>
		/// Numer VAT ID
		/// </summary>
		public string VATNumber { get; set; }

		/// <summary>
		/// Flaga aktywności firmy
		/// </summary>
		public bool Valid { get; set; }

		/// <summary>
		/// Nazwa firmy
		/// </summary>
		public string TraderName { get; set; }

		/// <summary>
		/// Typ firmy
		/// </summary>
		public string TraderCompanyType { get; set; }

		/// <summary>
		/// Adres siedziby
		/// </summary>
		public string TraderAddress { get; set; }

        /// <summary>
        /// Identyfikator sprawdzenia z systemu VIES
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Data sprawdzenia
        /// </summary>
        [ComVisible(false)]
        public DateTime? Date { get; set; }

        /// <summary>
        /// Data sprawdzenia
        /// </summary>
        DateTime IVIESData.Date
        {
            get { return Date ?? DateTime.MinValue; }
            set { Date = value; }
        }

        /// <summary>
        /// Źródło informacji o statusie
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Tworzy nowy, pusty obiekt
        /// </summary>
        public VIESData()
		{
		}

		public override string ToString()
		{
			return "VIESData: [uid = " + UID
				+ ", countryCode = " + CountryCode
				+ ", vatNumber = " + VATNumber
				+ ", valid = " + Valid
				+ ", traderName = " + TraderName
				+ ", traderCompanyType = " + TraderCompanyType
				+ ", traderAddress = " + TraderAddress
                + ", id = " + ID
                + ", date = " + Date
                + ", source = " + Source
                + "]";
		}
	}

	#endregion
}
