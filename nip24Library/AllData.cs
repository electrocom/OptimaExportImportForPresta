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
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace NIP24
{
	#region interface

	/// <summary>
	/// Interfejs pełnych danych firmy 
	/// </summary>
	[Guid("F2C462BF-6C2C-44E2-95AB-2D52B50450C8")]
	[ComVisible(true)]
	public interface IAllData
	{
		#region members of IInvoiceData

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

		#endregion

		/// <summary>
		/// Typ firmy
		/// </summary>
		[DispId(50)]
		string Type { get; set; }

		/// <summary>
		/// Numer REGON
		/// </summary>
		[DispId(51)]
		string REGON { get; set; }

		/// <summary>
		/// Krótka nazwa
		/// </summary>
		[DispId(52)]
		string ShortName { get; set; }

		/// <summary>
		/// Drugie imię
		/// </summary>
		[DispId(53)]
		string SecondName { get; set; }

		/// <summary>
		/// Kod TERYT ulicy
		/// </summary>
		[DispId(54)]
		string StreetCode { get; set; }

		/// <summary>
		/// Kod TERYT miejscowości
		/// </summary>
		[DispId(55)]
		string CityCode { get; set; }

		/// <summary>
		/// Gmina
		/// </summary>
		[DispId(56)]
		string Community { get; set; }

		/// <summary>
		/// Kod TERYT gminy
		/// </summary>
		[DispId(57)]
		string CommunityCode { get; set; }

		/// <summary>
		/// Powiat
		/// </summary>
		[DispId(58)]
		string County { get; set; }

		/// <summary>
		/// Kod TERYT powiatu
		/// </summary>
		[DispId(59)]
		string CountyCode { get; set; }

		/// <summary>
		/// Województwo
		/// </summary>
		[DispId(60)]
		string State { get; set; }

		/// <summary>
		/// Kod TERYT województwa
		/// </summary>
		[DispId(61)]
		string StateCode { get; set; }

		/// <summary>
		/// Data powstania firmy
		/// </summary>
		[DispId(62)]
		DateTime CreationDate { get; set; }

		/// <summary>
		/// Data rozpoczęcia działalności
		/// </summary>
		[DispId(63)]
		DateTime StartDate { get; set; }

		/// <summary>
		/// Data wpisu do rejestru REGON
		/// </summary>
		[DispId(64)]
		DateTime RegistrationDate { get; set; }

		/// <summary>
		/// Data zawieszenia działalności
		/// </summary>
		[DispId(65)]
		DateTime HoldDate { get; set; }

		/// <summary>
		/// Data wznowienia działalności
		/// </summary>
		[DispId(66)]
		DateTime RenevalDate { get; set; }

		/// <summary>
		/// Data zaistnienia ostatniej zmiany we wpisie
		/// </summary>
		[DispId(67)]
		DateTime LastUpdateDate { get; set; }

		/// <summary>
		/// Data zakończenia działalności
		/// </summary>
		[DispId(68)]
		DateTime EndDate { get; set; }

		/// <summary>
		/// Kod organu rejestrowego
		/// </summary>
		[DispId(69)]
		string RegistryEntityCode { get; set; }

		/// <summary>
		/// Nazwa organu rejestrowego
		/// </summary>
		[DispId(70)]
		string RegistryEntityName { get; set; }

		/// <summary>
		/// Kod rodzaju rejestru ewidencji
		/// </summary>
		[DispId(71)]
		string RegistryCode { get; set; }

		/// <summary>
		/// Nazwa rodzaju rejestru ewidencji
		/// </summary>
		[DispId(72)]
		string RegistryName { get; set; }

		/// <summary>
		/// Data wpisu do rejestru ewidencji
		/// </summary>
		[DispId(73)]
		DateTime RecordCreationDate { get; set; }

		/// <summary>
		/// Numer w rejestrze ewidencji
		/// </summary>
		[DispId(74)]
		string RecordNumber { get; set; }

		/// <summary>
		/// Kod podstawowej formy prawnej
		/// </summary>
		[DispId(75)]
		string BasicLegalFormCode { get; set; }

		/// <summary>
		/// Nazwa podstawowej formy prawnej
		/// </summary>
		[DispId(76)]
		string BasicLegalFormName { get; set; }

		/// <summary>
		/// Kod szczególnej formy prawnej
		/// </summary>
		[DispId(77)]
		string SpecificLegalFormCode { get; set; }

		/// <summary>
		/// Nazwa szczególnej formy prawnej
		/// </summary>
		[DispId(78)]
		string SpecificLegalFormName { get; set; }

		/// <summary>
		/// Kod formy własności
		/// </summary>
		[DispId(79)]
		string OwnershipFormCode { get; set; }

		/// <summary>
		/// Nazwa formy własności
		/// </summary>
		[DispId(80)]
		string OwnershipFormName { get; set; }

		/// <summary>
		/// Lista kodów PKD
		/// </summary>
		[DispId(81)]
		PKD[] PKD { get; set; }

		[DispId(82)]
		string ToString();
	}

	#endregion

	#region implementation

	/// <summary>
	/// Pełne dane firmy 
	/// </summary>
	[Guid("B33B7921-18BA-4552-BE1D-BE3B8B0D4FCB")]
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	public class AllData : InvoiceData, IAllData
	{
        /// <summary>
        /// Typ firmy
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Numer REGON
        /// </summary>
        public string REGON { get; set; }

        /// <summary>
        /// Krótka nazwa
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Drugie imię
        /// </summary>
        public string SecondName { get; set; }

		/// <summary>
		/// Kod TERYT ulicy
		/// </summary>
		public string StreetCode { get; set; }

		/// <summary>
		/// Kod TERYT miejscowości
		/// </summary>
		public string CityCode { get; set; }

		/// <summary>
		/// Gmina
		/// </summary>
		public string Community { get; set; }

		/// <summary>
		/// Kod TERYT gminy
		/// </summary>
		public string CommunityCode { get; set; }

		/// <summary>
		/// Powiat
		/// </summary>
		public string County { get; set; }

		/// <summary>
		/// Kod TERYT powiatu
		/// </summary>
		public string CountyCode { get; set; }

		/// <summary>
		/// Województwo
		/// </summary>
		public string State { get; set; }

		/// <summary>
		/// Kod TERYT województwa
		/// </summary>
		public string StateCode { get; set; }

		/// <summary>
		/// Data powstania firmy
		/// </summary>
		[ComVisible(false)]
		public DateTime? CreationDate { get; set; }

		/// <summary>
		/// Data powstania firmy
		/// </summary>
		DateTime IAllData.CreationDate
		{
			get { return CreationDate ?? DateTime.MinValue; }
			set { CreationDate = value; }
		}

		/// <summary>
		/// Data rozpoczęcia działalności
		/// </summary>
		[ComVisible(false)]
		public DateTime? StartDate { get; set; }

		/// <summary>
		/// Data rozpoczęcia działalności
		/// </summary>
		DateTime IAllData.StartDate
		{
			get { return StartDate ?? DateTime.MinValue; }
			set { StartDate = value; }
		}

		/// <summary>
		/// Data wpisu do rejestru REGON
		/// </summary>
		[ComVisible(false)]
		public DateTime? RegistrationDate { get; set; }

		/// <summary>
		/// Data wpisu do rejestru REGON
		/// </summary>
		DateTime IAllData.RegistrationDate
		{
			get { return RegistrationDate ?? DateTime.MinValue; }
			set { RegistrationDate = value; }
		}

		/// <summary>
		/// Data zawieszenia działalności
		/// </summary>
		[ComVisible(false)]
		public DateTime? HoldDate { get; set; }

		/// <summary>
		/// Data zawieszenia działalności
		/// </summary>
		DateTime IAllData.HoldDate
		{
			get { return HoldDate ?? DateTime.MinValue; }
			set { HoldDate = value; }
		}

		/// <summary>
		/// Data wznowienia działalności
		/// </summary>
		[ComVisible(false)]
		public DateTime? RenevalDate { get; set; }

		/// <summary>
		/// Data wznowienia działalności
		/// </summary>
		DateTime IAllData.RenevalDate
		{
			get { return RenevalDate ?? DateTime.MinValue; }
			set { RenevalDate = value; }
		}

		/// <summary>
		/// Data zaistnienia ostatniej zmiany we wpisie
		/// </summary>
		[ComVisible(false)]
		public DateTime? LastUpdateDate { get; set; }

		/// <summary>
		/// Data zaistnienia ostatniej zmiany we wpisie
		/// </summary>
		DateTime IAllData.LastUpdateDate
		{
			get { return LastUpdateDate ?? DateTime.MinValue; }
			set { LastUpdateDate = value; }
		}

		/// <summary>
		/// Data zakończenia działalności
		/// </summary>
		[ComVisible(false)]
		public DateTime? EndDate { get; set; }

		/// <summary>
		/// Data zakończenia działalności
		/// </summary>
		DateTime IAllData.EndDate
		{
			get { return EndDate ?? DateTime.MinValue; }
			set { EndDate = value; }
		}

		/// <summary>
		/// Kod organu rejestrowego
		/// </summary>
		public string RegistryEntityCode { get; set; }

        /// <summary>
        /// Nazwa organu rejestrowego
        /// </summary>
        public string RegistryEntityName { get; set; }

        /// <summary>
        /// Kod rodzaju rejestru ewidencji
        /// </summary>
        public string RegistryCode { get; set; }

        /// <summary>
        /// Nazwa rodzaju rejestru ewidencji
        /// </summary>
        public string RegistryName { get; set; }

		/// <summary>
		/// Data wpisu do rejestru ewidencji
		/// </summary>
		[ComVisible(false)]
		public DateTime? RecordCreationDate { get; set; }

		/// <summary>
		/// Data wpisu do rejestru ewidencji
		/// </summary>
		DateTime IAllData.RecordCreationDate
		{
			get { return RecordCreationDate ?? DateTime.MinValue; }
			set { RecordCreationDate = value; }
		}

		/// <summary>
		/// Numer w rejestrze ewidencji
		/// </summary>
		public string RecordNumber { get; set; }

        /// <summary>
        /// Kod podstawowej formy prawnej
        /// </summary>
        public string BasicLegalFormCode { get; set; }

        /// <summary>
        /// Nazwa podstawowej formy prawnej
        /// </summary>
        public string BasicLegalFormName { get; set; }

        /// <summary>
        /// Kod szczególnej formy prawnej
        /// </summary>
        public string SpecificLegalFormCode { get; set; }

        /// <summary>
        /// Nazwa szczególnej formy prawnej
        /// </summary>
        public string SpecificLegalFormName { get; set; }

        /// <summary>
        /// Kod formy własności
        /// </summary>
        public string OwnershipFormCode { get; set; }

        /// <summary>
        /// Nazwa formy własności
        /// </summary>
        public string OwnershipFormName { get; set; }

		/// <summary>
		/// Lista kodów PKD
		/// </summary>
		[ComVisible(false)]
		public List<PKD> PKD { get; set; }

		/// <summary>
		/// Lista kodów PKD
		/// </summary>
		PKD[] IAllData.PKD
		{
			get { return PKD.ToArray(); }
			set { PKD = new List<PKD>(value); }
		}

		/// <summary>
		/// Tworzy nowy, pusty obiekt
		/// </summary>
		public AllData()
		{
			PKD = new List<PKD>();
		}

		public override string ToString()
        {
            return "AllData: [nip = " + NIP
                + ", regon = " + REGON
                + ", type = " + Type

                + ", name = " + Name
                + ", shortName = " + ShortName
                + ", firstName = " + FirstName
                + ", secondName = " + SecondName
                + ", lastName = " + LastName

                + ", street = " + Street
                + ", streetCode = " + StreetCode
                + ", streetNumber = " + StreetNumber
                + ", houseNumber = " + HouseNumber
                + ", city = " + City
                + ", cityCode = " + CityCode
                + ", community = " + Community
                + ", communityCode = " + CommunityCode
                + ", county = " + County
                + ", countyCode = " + CountyCode
                + ", state = " + State
                + ", stateCode = " + StateCode
                + ", postCode = " + PostCode
                + ", postCity = " + PostCity

                + ", phone = " + Phone
                + ", email = " + Email
                + ", www = " + WWW

                + ", creationDate = " + CreationDate
                + ", startDate = " + StartDate
                + ", registrationDate = " + RegistrationDate
                + ", holdDate = " + HoldDate
                + ", renevalDate = " + RenevalDate
                + ", lastUpdateDate = " + LastUpdateDate
                + ", endDate = " + EndDate

                + ", registryEntityCode = " + RegistryEntityCode
                + ", registryEntityName = " + RegistryEntityName

                + ", registryCode = " + RegistryCode
                + ", registryName = " + RegistryName

                + ", recordCreationDate = " + RecordCreationDate
                + ", recordNumber = " + RecordNumber

                + ", basicLegalFormCode = " + BasicLegalFormCode
                + ", basicLegalFormName = " + BasicLegalFormName

                + ", specificLegalFormCode = " + SpecificLegalFormCode
                + ", specificLegalFormName = " + SpecificLegalFormName

                + ", ownershipFormCode = " + OwnershipFormCode
                + ", ownershipFormName = " + OwnershipFormName

                + ", pkd = " + GetPKDString()
                + "]";
        }

        /// <summary>
        /// Lista PKD jako ciąg
        /// </summary>
        /// <returns>lista PKD</returns>
        private string GetPKDString()
        {
            string s = "";

            foreach (PKD pkd in PKD)
            {
                if (s.Length > 0)
                {
                    s += ", ";
                }

                s += PKD.ToString();
            }

            return s;
        }
    }

	#endregion
}
