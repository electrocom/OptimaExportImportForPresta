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

using System.Runtime.InteropServices;

namespace NIP24
{
	#region interface

	/// <summary>
	/// Interfejs danych firmy wymaganych do wystawienia faktury
	/// </summary>
	[Guid("03DF6A61-201B-47D7-A8D3-F4A192C1E557")]
	[ComVisible(true)]
	public interface IInvoiceData
	{
		/// <summary>
		/// Unikalny identyfikator odpowiedzi
		/// </summary>
		[DispId(1)]
		string UID { get; set; }

		/// <summary>
		/// Numer NIP
		/// </summary>
		[DispId(2)]
		string NIP { get; set; }

		/// <summary>
		/// Nazwa firmy
		/// </summary>
		[DispId(3)]
		string Name { get; set; }

		/// <summary>
		/// Imię osoby prowadzącej działalność gospodarczą
		/// </summary>
		[DispId(4)]
		string FirstName { get; set; }

		/// <summary>
		/// Nazwisko osoby prowadzącej działalność gospodarczą
		/// </summary>
		[DispId(5)]
		string LastName { get; set; }

		/// <summary>
		/// Nazwa ulicy
		/// </summary>
		[DispId(6)]
		string Street { get; set; }

		/// <summary>
		/// Numer budynku
		/// </summary>
		[DispId(7)]
		string StreetNumber { get; set; }

		/// <summary>
		/// Numer lokalu
		/// </summary>
		[DispId(8)]
		string HouseNumber { get; set; }

		/// <summary>
		/// Miejscowość
		/// </summary>
		[DispId(9)]
		string City { get; set; }

		/// <summary>
		/// Kod pocztowy
		/// </summary>
		[DispId(10)]
		string PostCode { get; set; }

		/// <summary>
		/// Miejscowość poczty
		/// </summary>
		[DispId(11)]
		string PostCity { get; set; }

		/// <summary>
		/// Telefon kontaktowy firmy
		/// </summary>
		[DispId(12)]
		string Phone { get; set; }

		/// <summary>
		/// Adres email firmy
		/// </summary>
		[DispId(13)]
		string Email { get; set; }

		/// <summary>
		/// Adres strony internetowej firmy
		/// </summary>
		[DispId(14)]
		string WWW { get; set; }

		[DispId(15)]
		string ToString();
	}

	#endregion

	#region implementation

	/// <summary>
	/// Dane firmy wymagane do wystawienia faktury
	/// </summary>
	[Guid("DB982324-1BF5-420E-BA9E-3FB248DDA89A")]
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	public class InvoiceData : IInvoiceData
	{
		/// <summary>
		/// Unikalny identyfikator odpowiedzi
		/// </summary>
		public string UID { get; set; }

		/// <summary>
		/// Numer NIP
		/// </summary>
		public string NIP { get; set; }

        /// <summary>
        /// Nazwa firmy
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Imię osoby prowadzącej działalność gospodarczą
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Nazwisko osoby prowadzącej działalność gospodarczą
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Nazwa ulicy
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// Numer budynku
        /// </summary>
        public string StreetNumber { get; set; }

        /// <summary>
        /// Numer lokalu
        /// </summary>
        public string HouseNumber { get; set; }

        /// <summary>
        /// Miejscowość
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Kod pocztowy
        /// </summary>
        public string PostCode { get; set; }

        /// <summary>
        /// Miejscowość poczty
        /// </summary>
        public string PostCity { get; set; }

        /// <summary>
        /// Telefon kontaktowy firmy
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Adres email firmy
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Adres strony internetowej firmy
        /// </summary>
        public string WWW { get; set; }

		/// <summary>
		/// Tworzy nowy, pusty obiekt
		/// </summary>
		public InvoiceData()
		{
		}

		public override string ToString()
        {
            return "InvoiceData: [uid = " + UID
				+ ", nip = " + NIP
                + ", name = " + Name
                + ", firstName = " + FirstName
                + ", lastName = " + LastName

                + ", street = " + Street
                + ", streetNumber = " + StreetNumber
                + ", houseNumber = " + HouseNumber
                + ", city = " + City
                + ", postCode = " + PostCode
                + ", postCity = " + PostCity

                + ", phone = " + Phone
                + ", email = " + Email
                + ", www = " + WWW
                + "]";
        }
    }

	#endregion
}