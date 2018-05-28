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
using System.Globalization;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace NIP24
{
	#region interface

	/// <summary>
	/// Interfejs klienta serwisu NIP24
	/// </summary>
	[Guid("80D40219-9D08-45C7-A255-44FC7A701578")]
	[ComVisible(true)]
	public interface INIP24Client
	{
		/// <summary>
		/// Ostatni komunikat błędu
		/// </summary>
		[DispId(1)]
		string LastError { get; set; }

		/// <summary>
		/// Adres URL serwisu
		/// </summary>
		[DispId(2)]
		string URL { get; set; }

		/// <summary>
		/// Identyfikator klucza klienta serwisu
		/// </summary>
		[DispId(3)]
		string Id { get; set; }

		/// <summary>
		/// Klucz klienta serwisu
		/// </summary>
		[DispId(4)]
		string Key { get; set; }

		/// <summary>
		/// Nazwa i wersja aplikacji korzystającej z klienta API
		/// </summary>
		[DispId(5)]
		string Application { get; set; }

		/// <summary>
		/// Sprawdzenie czy firma prowadzi aktywną działalność
		/// </summary>
		/// <param name="type">typ numeru identyfikującego firmę</param>
		/// <param name="number">numer określonego typu</param>
		/// <returns>true jeżeli firma prowadzi aktywną działalność, false jeżeli firma zakończyła działalność</returns>
		[DispId(8)]
		bool IsActive(Number type, string number);

		/// <summary>
		/// Sprawdzenie czy firma prowadzi aktywną działalność
		/// </summary>
		/// <param name="nip">numer NIP</param>
		/// <returns>true jeżeli firma prowadzi aktywną działalność, false jeżeli firma zakończyła działalność</returns>
		[DispId(9)]
		bool IsActive(string nip);

		/// <summary>
		/// Pobranie podstawowych danych firmy do faktury
		/// </summary>
		/// <param name="type">typ numeru identyfikującego firmę</param>
		/// <param name="number">numer określonego typu</param>
		/// <param name="force">jeżeli true zostanie wymuszone zapytanie do GUS w celu pobrania najświeższych danych (trwa dłużej)</param>
		/// <returns>dane firmy lub null w przypadku błędu</returns>
		[DispId(10)]
		InvoiceData GetInvoiceData(Number type, string number, bool force = false);

		/// <summary>
		/// Pobranie podstawowych danych firmy do faktury
		/// </summary>
		/// <param name="nip">numer NIP</param>
		/// <param name="force">jeżeli true zostanie wymuszone zapytanie do GUS w celu pobrania najświeższych danych (trwa dłużej)</param>
		/// <returns>dane firmy lub null w przypadku błędu</returns>
		[DispId(11)]
		InvoiceData GetInvoiceData(string nip, bool force = false);

		/// <summary>
		/// Pobranie szczegółowych danych firmy
		/// </summary>
		/// <param name="type">typ numeru identyfikującego firmę</param>
		/// <param name="number">numer określonego typu</param>
		/// <param name="force">jeżeli true zostanie wymuszone zapytanie do GUS w celu pobrania najświeższych danych (trwa dłużej)</param>
		/// <returns>dane firmy lub null w przypadku błędu</returns>
		[DispId(12)]
		AllData GetAllData(Number type, string number, bool force = false);

		/// <summary>
		/// Pobranie szczegółowych danych firmy
		/// </summary>
		/// <param name="nip">numer NIP</param>
		/// <param name="force">jeżeli true zostanie wymuszone zapytanie do GUS w celu pobrania najświeższych danych (trwa dłużej)</param>
		/// <returns>dane firmy lub null w przypadku błędu</returns>
		[DispId(13)]
		AllData GetAllData(string nip, bool force = false);

		/// <summary>
		/// Pobranie danych firmy z systemu VIES
		/// </summary>
		/// <param name="euvat">numer EU VAT ID</param>
		/// <returns>dane firmy lub null w przypadku błędu</returns>
		[DispId(14)]
		VIESData GetVIESData(string euvat);

		/// <summary>
		/// Sprawdzenie statusu firmy w rejestrze VAT
		/// </summary>
		/// <param name="type">typ numeru identyfikującego firmę</param>
		/// <param name="number">numer określonego typu</param>
		/// <param name="direct">true - zostanie wykonane zapytanie bezpośrednio do PPU MF,
		/// false - zapytanie do nip24.pl po dodatkowe dane, dzięki którym można pytać po REGON i KRS</param>
		/// <returns>status firmy lub null w przypadku błędu</returns>
		[DispId(15)]
		VATStatus GetVATStatus(Number type, string number, bool direct = false);

		/// <summary>
		/// Sprawdzenie statusu firmy w rejestrze VAT
		/// </summary>
		/// <param name="nip">numer NIP</param>
		/// <param name="direct">true - zostanie wykonane zapytanie bezpośrednio do PPU MF,
		/// false - zapytanie do nip24.pl po dodatkowe dane, dzięki którym można pytać po REGON i KRS</param>
		/// <returns>status firmy lub null w przypadku błędu</returns>
		[DispId(16)]
		VATStatus GetVATStatus(string nip, bool direct = true);

        /// <summary>
        /// Sprawdzenie bieżącego stanu konta użytkownika
        /// </summary>
        /// <returns>status konta lub null w przypadku błędu</returns>
        [DispId(17)]
        AccountStatus GetAccountStatus();
    }

    #endregion

    #region implementation

    /// <summary>
    /// Klient serwisu NIP24
    /// </summary>
    [Guid("44016EE2-134C-4CEF-9CF8-719A0B5B417C")]
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	public class NIP24Client : INIP24Client
	{
		public const string VERSION = "1.3.1";

		public const string PRODUCTION_URL = "https://www.nip24.pl/api";
		public const string TEST_URL = "https://www.nip24.pl/api-test";

		public const string TEST_ID = "test_id";
		public const string TEST_KEY = "test_key";

		/// <summary>
		/// Ostatni komunikat błędu
		/// </summary>
		public string LastError { get; set; }

		/// <summary>
		/// Adres URL serwisu
		/// </summary>
		public string URL { get; set; }

		/// <summary>
		/// Identyfikator klucza klienta serwisu
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// Klucz klienta serwisu
		/// </summary>
		public string Key { get; set; }

		/// <summary>
		/// Nazwa i wersja aplikacji korzystającej z klienta API
		/// </summary>
		public string Application { get; set; }

		private RandomNumberGenerator rng;

		/// <summary>
		/// Tworzy nowy obiekt klienta
		/// </summary>
		/// <param name="url">adres URL serwisu NIP24</param>
		/// <param name="id">identyfikator klucza klienta serwisu</param>
		/// <param name="key">klucz klienta serwisu</param>
		public NIP24Client(string url, string id, string key)
		{
			URL = url;
			Id = id;
			Key = key;

			LastError = string.Empty;

			rng = new RNGCryptoServiceProvider();
		}

		/// <summary>
		/// Tworzy nowy obiekt klienta
		/// </summary>
		/// <param name="url">adres URL serwisu NIP24</param>
		/// <param name="id">identyfikator klucza klienta serwisu</param>
		/// <param name="key">klucz klienta serwisu</param>
		public NIP24Client(Uri url, string id, string key) : this(url.ToString(), id, key)
		{
		}

		/// <summary>
		/// Tworzy nowy obiekt klienta serwisu produkcyjnego
		/// </summary>
		/// <param name="id">identyfikator klucza klienta serwisu</param>
		/// <param name="key">klucz klienta serwisu</param>
		public NIP24Client(string id, string key) : this(PRODUCTION_URL, id, key)
		{
		}

		/// <summary>
		/// Tworzy nowy obiekt klienta serwisu testowego
		/// </summary>
		public NIP24Client() : this(TEST_URL, TEST_ID, TEST_KEY)
		{
		}

		/// <summary>
		/// Ustawia alternatywny adres API serwisu
		/// </summary>
		/// <param name="url">adres URL serwisu NIP24</param>
		[Obsolete("Ta metoda będzie usunięta w kolejnej wersji API. Zamiast niej proszę użyć property URL", true)]
		public void setURL(Uri url)
		{
			URL = (url != null ? url.ToString() : null);
		}

		/// <summary>
		/// Ustawia alternatywny adres API serwisu
		/// </summary>
		/// <param name="url">adres URL serwisu NIP24</param>
		/// dep
		[Obsolete("Ta metoda będzie usunięta w kolejnej wersji API. Zamiast niej proszę użyć property URL", true)]
		public void setURL(string url)
		{
			URL = url;
		}

		/// <summary>
		/// Sprawdzenie czy firma prowadzi aktywną działalność
		/// </summary>
		/// <param name="type">typ numeru identyfikującego firmę</param>
		/// <param name="number">numer określonego typu</param>
		/// <returns>true jeżeli firma prowadzi aktywną działalność, false jeżeli firma zakończyła działalność</returns>
		public bool IsActive(Number type, string number)
		{
			try
			{
				// clear error
				LastError = string.Empty;

				// validate number and construct path
				string suffix = null;

				if ((suffix = GetPathSuffix(type, number)) == null)
				{
					return false;
				}

				// prepare url
				Uri url = new Uri(URL + "/check/firm/" + suffix);

				// prepare request
				XPathDocument doc = Get(url);

				if (doc == null)
				{
					LastError = "Nie udało się nawiązać połączenia z serwisem NIP24";
					return false;
				}

				// parse response
				string res = GetString(doc, "/result/error/code", null);

				if (res != null)
				{
					if (res.Equals("9"))
					{
						// not active
						LastError = string.Empty;
						return false;
					}
					else
					{
						LastError = GetString(doc, "/result/error/description", null);
						return false;
					}
				}

				// active
				return true;
			}
			catch (Exception e)
			{
				LastError = e.Message;
			}

			return false;
		}

		/// <summary>
		/// Sprawdzenie czy firma prowadzi aktywną działalność
		/// </summary>
		/// <param name="nip">numer NIP</param>
		/// <returns>true jeżeli firma prowadzi aktywną działalność, false jeżeli firma zakończyła działalność</returns>
		public bool IsActive(string nip)
		{
			return IsActive(Number.NIP, nip);
		}

		/// <summary>
		/// Pobranie podstawowych danych firmy do faktury
		/// </summary>
		/// <param name="type">typ numeru identyfikującego firmę</param>
		/// <param name="number">numer określonego typu</param>
		/// <param name="force">jeżeli true zostanie wymuszone zapytanie do GUS w celu pobrania najświeższych danych (trwa dłużej)</param>
		/// <returns>dane firmy lub null w przypadku błędu</returns>
		public InvoiceData GetInvoiceData(Number type, string number, bool force = false)
		{
			try
			{
				// clear error
				LastError = string.Empty;

                // validate number and construct path
                string suffix = null;

                if ((suffix = GetPathSuffix(type, number)) == null)
                {
                    return null;
                }

                // prepare url
                Uri url = new Uri(URL + (force ? "/getf" : "/get") + "/invoice/" + suffix);

				// prepare request
				XPathDocument doc = Get(url);

				if (doc == null)
				{
					LastError = "Nie udało się nawiązać połączenia z serwisem NIP24";
					return null;
				}

				// parse response
				string res = GetString(doc, "/result/error/code", null);

				if (res != null)
				{
					LastError = GetString(doc, "/result/error/description", null);
					return null;
				}

				InvoiceData id = new InvoiceData();

				id.UID = GetString(doc, "/result/firm/uid", null);

				id.NIP = GetString(doc, "/result/firm/nip", null);

                id.Name = GetString(doc, "/result/firm/name", null);
				id.FirstName = GetString(doc, "/result/firm/firstname", null);
				id.LastName = GetString(doc, "/result/firm/lastname", null);

				id.Street = GetString(doc, "/result/firm/street", null);
				id.StreetNumber = GetString(doc, "/result/firm/streetNumber", null);
				id.HouseNumber = GetString(doc, "/result/firm/houseNumber", null);
				id.City = GetString(doc, "/result/firm/city", null);
				id.PostCode = GetString(doc, "/result/firm/postCode", null);
				id.PostCity = GetString(doc, "/result/firm/postCity", null);

				id.Phone = GetString(doc, "/result/firm/phone", null);
				id.Email = GetString(doc, "/result/firm/email", null);
				id.WWW = GetString(doc, "/result/firm/www", null);

				return id;
			}
			catch (Exception e)
			{
				LastError = e.Message;
			}

			return null;
		}

        /// <summary>
        /// Pobranie podstawowych danych firmy do faktury
        /// </summary>
        /// <param name="nip">numer NIP</param>
        /// <param name="force">jeżeli true zostanie wymuszone zapytanie do GUS w celu pobrania najświeższych danych (trwa dłużej)</param>
        /// <returns>dane firmy lub null w przypadku błędu</returns>
        public InvoiceData GetInvoiceData(string nip, bool force = false)
        {
            return GetInvoiceData(Number.NIP, nip, force);
        }

        /// <summary>
        /// Pobranie szczegółowych danych firmy
        /// </summary>
        /// <param name="type">typ numeru identyfikującego firmę</param>
        /// <param name="number">numer określonego typu</param>
        /// <param name="force">jeżeli true zostanie wymuszone zapytanie do GUS w celu pobrania najświeższych danych (trwa dłużej)</param>
        /// <returns>dane firmy lub null w przypadku błędu</returns>
        public AllData GetAllData(Number type, string number, bool force = false)
		{
			try
			{
				// clear error
				LastError = string.Empty;

                // validate number and construct path
                string suffix = null;

                if ((suffix = GetPathSuffix(type, number)) == null)
                {
                    return null;
                }

                // prepare url
                Uri url = new Uri(URL + (force ? "/getf" : "/get") + "/all/" + suffix);

				// prepare request
				XPathDocument doc = Get(url);

				if (doc == null)
				{
					LastError = "Nie udało się nawiązać połączenia z serwisem NIP24";
					return null;
				}

				// parse response
				string res = GetString(doc, "/result/error/code", null);

				if (res != null)
				{
					LastError = GetString(doc, "/result/error/description", null);
					return null;
				}

				AllData ad = new AllData();

				ad.UID = GetString(doc, "/result/firm/uid", null);

				ad.NIP = GetString(doc, "/result/firm/nip", null);
				ad.Type = GetString(doc, "/result/firm/type", null);
				ad.NIP = GetString(doc, "/result/firm/nip", null);
                ad.REGON = GetString(doc, "/result/firm/regon", null);

				ad.Name = GetString(doc, "/result/firm/name", null);
				ad.ShortName = GetString(doc, "/result/firm/shortname", null);
				ad.FirstName = GetString(doc, "/result/firm/firstname", null);
				ad.SecondName = GetString(doc, "/result/firm/secondname", null);
				ad.LastName = GetString(doc, "/result/firm/lastname", null);

				ad.Street = GetString(doc, "/result/firm/street", null);
				ad.StreetCode = GetString(doc, "/result/firm/streetCode", null);
				ad.StreetNumber = GetString(doc, "/result/firm/streetNumber", null);
				ad.HouseNumber = GetString(doc, "/result/firm/houseNumber", null);
				ad.City = GetString(doc, "/result/firm/city", null);
				ad.CityCode = GetString(doc, "/result/firm/cityCode", null);
				ad.Community = GetString(doc, "/result/firm/community", null);
				ad.CommunityCode = GetString(doc, "/result/firm/communityCode", null);
				ad.County = GetString(doc, "/result/firm/county", null);
				ad.CountyCode = GetString(doc, "/result/firm/countyCode", null);
				ad.State = GetString(doc, "/result/firm/state", null);
				ad.StateCode = GetString(doc, "/result/firm/stateCode", null);
				ad.PostCode = GetString(doc, "/result/firm/postCode", null);
				ad.PostCity = GetString(doc, "/result/firm/postCity", null);

				ad.Phone = GetString(doc, "/result/firm/phone", null);
				ad.Email = GetString(doc, "/result/firm/email", null);
				ad.WWW = GetString(doc, "/result/firm/www", null);

				ad.CreationDate = GetDateTime(doc, "/result/firm/creationDate");
				ad.StartDate = GetDateTime(doc, "/result/firm/startDate");
				ad.RegistrationDate = GetDateTime(doc, "/result/firm/registrationDate");
				ad.HoldDate = GetDateTime(doc, "/result/firm/holdDate");
				ad.RenevalDate = GetDateTime(doc, "/result/firm/renevalDate");
				ad.LastUpdateDate = GetDateTime(doc, "/result/firm/lastUpdateDate");
				ad.EndDate = GetDateTime(doc, "/result/firm/endDate");

				ad.RegistryEntityCode = GetString(doc, "/result/firm/registryEntity/code", null);
				ad.RegistryEntityName = GetString(doc, "/result/firm/registryEntity/name", null);

				ad.RegistryCode = GetString(doc, "/result/firm/registry/code", null);
				ad.RegistryName = GetString(doc, "/result/firm/registry/name", null);

				ad.RecordCreationDate = GetDateTime(doc, "/result/firm/record/created");
				ad.RecordNumber = GetString(doc, "/result/firm/record/number", null);

				ad.BasicLegalFormCode = GetString(doc, "/result/firm/basicLegalForm/code", null);
				ad.BasicLegalFormName = GetString(doc, "/result/firm/basicLegalForm/name", null);

				ad.SpecificLegalFormCode = GetString(doc, "/result/firm/specificLegalForm/code", null);
				ad.SpecificLegalFormName = GetString(doc, "/result/firm/specificLegalForm/name", null);

				ad.OwnershipFormCode = GetString(doc, "/result/firm/ownershipForm/code", null);
				ad.OwnershipFormName = GetString(doc, "/result/firm/ownershipForm/name", null);

				for (int i = 1; ; i++)
				{
					string code = GetString(doc, "/result/firm/PKDs/PKD[" + i + "]/code", null);

					if (code == null)
					{
						break;
					}

					string descr = GetString(doc, "/result/firm/PKDs/PKD[" + i + "]/description", null);
					string pri = GetString(doc, "/result/firm/PKDs/PKD[" + i + "]/primary", "false");

					PKD pkd = new PKD();

					pkd.Code = code;
					pkd.Description = descr;
					pkd.Primary = pri.Equals("true");

					ad.PKD.Add(pkd);
				}

				return ad;
			}
			catch (Exception e)
			{
				LastError = e.Message;
			}

			return null;
		}

        /// <summary>
        /// Pobranie szczegółowych danych firmy
        /// </summary>
        /// <param name="nip">numer NIP</param>
        /// <param name="force">jeżeli true zostanie wymuszone zapytanie do GUS w celu pobrania najświeższych danych (trwa dłużej)</param>
        /// <returns>dane firmy lub null w przypadku błędu</returns>
        public AllData GetAllData(string nip, bool force = false)
        {
            return GetAllData(Number.NIP, nip, force);
        }

        /// <summary>
        /// Pobranie danych firmy z systemu VIES
        /// </summary>
        /// <param name="euvat">numer EU VAT ID</param>
        /// <returns>dane firmy lub null w przypadku błędu</returns>
        public VIESData GetVIESData(string euvat)
        {
            try
            {
                // clear error
                LastError = string.Empty;

                // validate number and construct path
                string suffix = null;

                if ((suffix = GetPathSuffix(Number.EUVAT, euvat)) == null)
                {
                    return null;
                }

                // prepare url
                Uri url = new Uri(URL + "/get/vies/" + suffix);

                // prepare request
                XPathDocument doc = Get(url);

                if (doc == null)
                {
                    LastError = "Nie udało się nawiązać połączenia z serwisem NIP24";
                    return null;
                }

				// parse response
				string res = GetString(doc, "/result/error/code", null);

				if (res != null)
				{
					LastError = GetString(doc, "/result/error/description", null);
					return null;
				}

				VIESData vies = new VIESData();

				vies.UID = GetString(doc, "/result/vies/uid", null);

				vies.CountryCode = GetString(doc, "/result/vies/countryCode", null);
                vies.VATNumber = GetString(doc, "/result/vies/vatNumber", null);

                vies.Valid = GetString(doc, "/result/vies/valid", "false").Equals("true");

                vies.TraderName = GetString(doc, "/result/vies/traderName", null);
                vies.TraderCompanyType = GetString(doc, "/result/vies/traderCompanyType", null);
                vies.TraderAddress = GetString(doc, "/result/vies/traderAddress", null);

                vies.ID = GetString(doc, "/result/vies/id", null);
                vies.Date = GetDateTime(doc, "/result/vies/date");
                vies.Source = GetString(doc, "/result/vies/source", null);

                return vies;
            }
            catch (Exception e)
            {
                LastError = e.Message;
            }

            return null;
		}

		/// <summary>
		/// Sprawdzenie statusu firmy w rejestrze VAT
		/// </summary>
		/// <param name="type">typ numeru identyfikującego firmę</param>
		/// <param name="number">numer określonego typu</param>
		/// <param name="direct">true - zostanie wykonane zapytanie bezpośrednio do PPU MF,
		/// false - zapytanie do nip24.pl po dodatkowe dane, dzięki którym można pytać po REGON i KRS</param>
		/// <returns>status firmy lub null w przypadku błędu</returns>
		public VATStatus GetVATStatus(Number type, string number, bool direct = false)
		{
			try
			{
				// clear error
				LastError = string.Empty;

				// validate number and construct path
				string suffix = null;

				if ((suffix = GetPathSuffix(type, number)) == null)
				{
					return null;
				}

				// prepare url
				Uri url = new Uri(URL + "/check/vat" + (direct ? "/direct/" : "/indirect/") + suffix);

				// prepare request
				XPathDocument doc = Get(url);

				if (doc == null)
				{
					LastError = "Nie udało się nawiązać połączenia z serwisem NIP24";
					return null;
				}

				// parse response
				string res = GetString(doc, "/result/error/code", null);

				if (res != null)
				{
					LastError = GetString(doc, "/result/error/description", null);
					return null;
				}

				VATStatus vat = new VATStatus();

                vat.UID = GetString(doc, "/result/vat/uid", null);

                vat.NIP = GetString(doc, "/result/vat/nip", null);
				vat.REGON = GetString(doc, "/result/vat/regon", null);
				vat.Name = GetString(doc, "/result/vat/name", null);

				vat.Status = int.Parse(GetString(doc, "/result/vat/status", "0"));
				vat.Result = GetString(doc, "/result/vat/result", null);

				vat.Date = GetDateTime(doc, "/result/vat/date");
				vat.Source = GetString(doc, "/result/vat/source", null);

				return vat;
			}
			catch (Exception e)
			{
				LastError = e.Message;
			}

			return null;
		}

		/// <summary>
		/// Sprawdzenie statusu firmy w rejestrze VAT
		/// </summary>
		/// <param name="nip">numer NIP</param>
		/// <param name="direct">true - zostanie wykonane zapytanie bezpośrednio do PPU MF,
		/// false - zapytanie do nip24.pl po dodatkowe dane, dzięki którym można pytać po REGON i KRS</param>
		/// <returns>status firmy lub null w przypadku błędu</returns>
		public VATStatus GetVATStatus(string nip, bool direct = true)
		{
			return GetVATStatus(Number.NIP, nip, direct);
		}

		/// <summary>
		/// Sprawdzenie bieżącego stanu konta użytkownika
		/// </summary>
		/// <returns>status konta lub null w przypadku błędu</returns>
		public AccountStatus GetAccountStatus()
		{
			try
			{
				// clear error
				LastError = string.Empty;

				// prepare url
				Uri url = new Uri(URL + "/check/account/status");

				// prepare request
				XPathDocument doc = Get(url);

				if (doc == null)
				{
					LastError = "Nie udało się nawiązać połączenia z serwisem NIP24";
					return null;
				}

				// parse response
				string res = GetString(doc, "/result/error/code", null);

				if (res != null)
				{
					LastError = GetString(doc, "/result/error/description", null);
					return null;
				}

				AccountStatus status = new AccountStatus();

				status.UID = GetString(doc, "/result/account/uid", null);
				status.BillingPlanName = GetString(doc, "/result/account/billingPlan/name", null);

				status.SubscriptionPrice = decimal.Parse(GetString(doc, "/result/account/billingPlan/subscriptionPrice", "0"), CultureInfo.InvariantCulture);
				status.ItemPrice = decimal.Parse(GetString(doc, "/result/account/billingPlan/itemPrice", "0"), CultureInfo.InvariantCulture);
				status.ItemPriceStatus = decimal.Parse(GetString(doc, "/result/account/billingPlan/itemPriceCheckStatus", "0"), CultureInfo.InvariantCulture);
				status.ItemPriceInvoice = decimal.Parse(GetString(doc, "/result/account/billingPlan/itemPriceInvoiceData", "0"), CultureInfo.InvariantCulture);
				status.ItemPriceAll = decimal.Parse(GetString(doc, "/result/account/billingPlan/itemPriceAllData", "0"), CultureInfo.InvariantCulture);

				status.Limit = int.Parse(GetString(doc, "/result/account/billingPlan/limit", "0"));
				status.RequestDelay = int.Parse(GetString(doc, "/result/account/billingPlan/requestDelay", "0"));
				status.DomainLimit = int.Parse(GetString(doc, "/result/account/billingPlan/domainLimit", "0"));

				status.OverPlanAllowed = GetString(doc, "/result/account/billingPlan/overplanAllowed", "false").Equals("true");
				status.TerytCodes = GetString(doc, "/result/account/billingPlan/terytCodes", "false").Equals("true");
				status.ExcelAddIn = GetString(doc, "/result/account/billingPlan/excelAddin", "false").Equals("true");
				status.Stats = GetString(doc, "/result/account/billingPlan/stats", "false").Equals("true");
				status.NIPMonitor = GetString(doc, "/result/account/billingPlan/nipMonitor", "false").Equals("true");

				status.SearchByNIP = GetString(doc, "/result/account/billingPlan/searchByNip", "false").Equals("true");
				status.SearchByREGON = GetString(doc, "/result/account/billingPlan/searchByRegon", "false").Equals("true");
				status.SearchByKRS = GetString(doc, "/result/account/billingPlan/searchByKrs", "false").Equals("true");

				status.FuncIsActive = GetString(doc, "/result/account/billingPlan/funcIsActive", "false").Equals("true");
				status.FuncGetInvoiceData = GetString(doc, "/result/account/billingPlan/funcGetInvoiceData", "false").Equals("true");
				status.FuncGetAllData = GetString(doc, "/result/account/billingPlan/funcGetAllData", "false").Equals("true");
				status.FuncGetVIESData = GetString(doc, "/result/account/billingPlan/funcGetVIESData", "false").Equals("true");
				status.FuncGetVATStatus = GetString(doc, "/result/account/billingPlan/funcGetVATStatus", "false").Equals("true");

				status.InvoiceDataCount = int.Parse(GetString(doc, "/result/account/requests/invoiceData", "0"));
				status.AllDataCount = int.Parse(GetString(doc, "/result/account/requests/allData", "0"));
				status.FirmStatusCount = int.Parse(GetString(doc, "/result/account/requests/firmStatus", "0"));
				status.VATStatusCount = int.Parse(GetString(doc, "/result/account/requests/vatStatus", "0"));
				status.VIESStatusCount = int.Parse(GetString(doc, "/result/account/requests/viesStatus", "0"));
				status.TotalCount = int.Parse(GetString(doc, "/result/account/requests/total", "0"));

				return status;
			}
			catch (Exception e)
			{
				LastError = e.Message;
			}

			return null;
		}

		/// <summary>
		/// Metoda HTTP GET
		/// </summary>
		/// <param name="url">adres URL</param>
		/// <returns>pobrana odpowiedź lub null</returns>
		private XPathDocument Get(Uri url)
		{
			XPathDocument doc = null;

			try
			{
				using (WebClient wc = new WebClient())
				{
					wc.UseDefaultCredentials = true;
					wc.Proxy = WebRequest.GetSystemWebProxy();

					wc.Headers.Set("Authorization", GetAuthHeader("GET", url));
					wc.Headers.Set("User-Agent", GetAgentHeader());

					byte[] b = wc.DownloadData(url);

					doc = new XPathDocument(new MemoryStream(b));
				}
			}
			catch (Exception e)
			{
				LastError = e.Message;
			}

			return doc;
		}

		/// <summary>
		/// Przygotowanie nagłówka z danymi o kliencie
		/// </summary>
		/// <returns>dane o kliencie</returns>
		private string GetAgentHeader()
		{
			return (string.IsNullOrEmpty(Application) ? "" : Application + " ") + "NIP24Client/" + VERSION + " .NET/" + Environment.Version;
		}

		/// <summary>
		/// Przygotowanie nagłówka z danymi do autoryzacji zapytania
		/// </summary>
		/// <param name="method">metoda HTTP</param>
		/// <param name="uri">docelowy adres URL</param>
		/// <returns>treść nagłówka HTTP Authorization</returns>
		private string GetAuthHeader(string method, Uri uri)
		{
			TimeSpan ts = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));

			string nonce = GetRandom(8);

			string str = string.Format("{0}\n{1}\n{2}\n{3}\n{4}\n{5}\n\n",
				Convert.ToInt32(ts.TotalSeconds),
				nonce,
				method,
				uri.PathAndQuery,
				uri.Host,
				uri.Port);

			string mac = GetMac(str);

			return string.Format("MAC id=\"{0}\", ts=\"{1}\", nonce=\"{2}\", mac=\"{3}\"", Id, Convert.ToInt32(ts.TotalSeconds), nonce, mac);
		}

		/// <summary>
		/// Zwraca losowy ciąg w postaci heksadecymalnej
		/// </summary>
		/// <param name="length">długość ciągu</param>
		/// <returns>losowy ciąg</returns>
		private string GetRandom(int length)
		{
			byte[] b = new byte[length / 2];

			rng.GetBytes(b);

			return BitConverter.ToString(b).Replace("-", "").ToLower();
		}

		/// <summary>
		/// Oblicza HMAC z podanego ciągu
		/// </summary>
		/// <param name="str">ciąg wejściowy</param>
		/// <returns>obliczony HMAC jako ciąg Base64 lub null</returns>
		private string GetMac(string str)
		{
			HashAlgorithm ha = new HMACSHA256(Encoding.ASCII.GetBytes(Key));

			return Convert.ToBase64String(ha.ComputeHash(Encoding.ASCII.GetBytes(str)));
		}

		/// <summary>
		/// Zwraca wartość węzła XML jako ciąg tekstowy
		/// </summary>
		/// <param name="doc">dokument XML</param>
		/// <param name="path">wyrażenie XPath wybierające wartość</param>
		/// <param name="def">wartość domyślna zwracana w przypadku braku wartości w XML</param>
		/// <returns>ciąg tekstowy</returns>
		private string GetString(XPathDocument doc, string path, string def)
		{
			try
			{
				XPathNavigator xpn = doc.CreateNavigator();

				string val = xpn.SelectSingleNode(path).Value;

				if (val != null)
				{
					return val;
				}
			}
			catch (Exception)
			{
			}

			return def;
		}

		/// <summary>
		/// Zwraca wartość węzła XML jako obiekt daty i czasu lokalnego
		/// </summary>
		/// <param name="doc">dokument XML</param>
		/// <param name="path">wyrażenie XPath wybierające wartość</param>
		/// <returns>data i czas lokalny lub null</returns>
		private DateTime? GetDateTime(XPathDocument doc, string path)
		{
			try
			{
				string val = GetString(doc, path, null);

				if (val != null)
				{
					return XmlConvert.ToDateTime(val, XmlDateTimeSerializationMode.Local);
				}
			}
			catch (Exception)
			{
			}

			return null;
		}

		/// <summary>
		/// Pobranie sufiksu ścieżki
		/// </summary>
		/// <param name="type">typ numeru identyfikującego firmę</param>
		/// <param name="number">numer określonego typu</param>
		/// <returns>fragment ścieżki lub null</returns>
		private string GetPathSuffix(Number type, string number)
        {
            string path = "";

            if (type.Equals(Number.NIP))
            {
                if (!NIP.IsValid(number))
                {
                    LastError = "Numer NIP jest nieprawidłowy";
                    return null;
                }

                path = "nip/" + NIP.Normalize(number);
            }
            else if (type.Equals(Number.REGON))
            {
                if (!REGON.IsValid(number))
                {
                    LastError = "Numer REGON jest nieprawidłowy";
                    return null;
                }

                path = "regon/" + REGON.Normalize(number);
            }
            else if (type.Equals(Number.KRS))
            {
                if (!KRS.IsValid(number))
                {
                    LastError = "Numer KRS jest nieprawidłowy";
                    return null;
                }

                path = "krs/" + KRS.Normalize(number);
            }
            else if (type.Equals(Number.EUVAT))
            {
                if (!EUVAT.IsValid(number))
                {
                    LastError = "Numer EU VAT ID jest nieprawidłowy";
                    return null;
                }

                path = "euvat/" + EUVAT.Normalize(number);
            }
            else
            {
                LastError = "Nieprawidłowy typ numeru";
                return null;
            }

            return path;
        }
    }

	#endregion
}