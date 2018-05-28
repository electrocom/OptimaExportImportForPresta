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
using System.Text.RegularExpressions;

namespace NIP24
{
	#region interface

	/// <summary>
	/// Interfejs weryfikatora numeru EU VAT ID
	/// </summary>
	[Guid("155D7B28-4BA8-4F37-93D7-7AF0DAD2280A")]
	[ComVisible(true)]
	public interface IEUVAT
	{
		/// <summary>
		/// Konwertuje podany numer EU VAT do postaci znormalizowanej
		/// </summary>
		/// <param name="nip">numer EU VAT w dowolnym formacie</param>
		/// <returns>znormalizowany numer EU VAT</returns>
		[DispId(1)]
		string Normalize(string nip);

		/// <summary>
		/// Sprawdza poprawność numeru EU VAT
		/// </summary>
		/// <param name="nip">numer EU VAT</param>
		/// <returns>true jeżeli podany numer jest prawidłowy</returns>
		[DispId(2)]
		bool IsValid(string nip);
	}

	#endregion

	#region implementation

	/// <summary>
	/// Weryfikator numeru EU VAT ID
	/// </summary>
	[Guid("43749758-6453-47F1-AA33-5C923BA75462")]
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	public class EUVAT : IEUVAT
    {
		/// <summary>
		/// Konwertuje podany numer EU VAT do postaci znormalizowanej
		/// </summary>
		/// <param name="nip">numer EU VAT w dowolnym formacie</param>
		/// <returns>znormalizowany numer EU VAT</returns>
		[ComVisible(false)]
		public static string Normalize(string nip)
        {
            if (nip == null || nip.Length == 0)
            {
                return null;
            }

            nip = nip.Replace("-", "");
            nip = nip.Replace(" ", "");
            nip = nip.Trim();

            string cc = nip.Substring(0, 2).ToUpper();
            string num = nip.Substring(2).ToUpper();

            if (cc.Equals("AT"))
            {
                // 9 chars
                Regex re = new Regex(@"^[0-9A-Z]{9}$");

                if (!re.IsMatch(num))
                {
                    return null;
                }
            }
            else if (cc.Equals("BE"))
            {
                // 10 digits
                Regex re = new Regex(@"^[0-9]{10}$");

                if (!re.IsMatch(num))
                {
                    return null;
                }
            }
            else if (cc.Equals("BG"))
            {
                // 9 or 10 digits
                Regex re = new Regex(@"^[0-9]{9,10}$");

                if (!re.IsMatch(num))
                {
                    return null;
                }
            }
            else if (cc.Equals("CY"))
            {
                // 9 chars
                Regex re = new Regex(@"^[0-9A-Z]{9}$");

                if (!re.IsMatch(num))
                {
                    return null;
                }
            }
            else if (cc.Equals("CZ"))
            {
                // 8-10 digits
                Regex re = new Regex(@"^[0-9]{8,10}$");

                if (!re.IsMatch(num))
                {
                    return null;
                }
            }
            else if (cc.Equals("DE"))
            {
                // 9 digits
                Regex re = new Regex(@"^[0-9]{9}$");

                if (!re.IsMatch(num))
                {
                    return null;
                }
            }
            else if (cc.Equals("DK"))
            {
                // 8 digits
                Regex re = new Regex(@"^[0-9]{8}$");

                if (!re.IsMatch(num))
                {
                    return null;
                }
            }
            else if (cc.Equals("EE"))
            {
                // 9 digits
                Regex re = new Regex(@"^[0-9]{9}$");

                if (!re.IsMatch(num))
                {
                    return null;
                }
            }
            else if (cc.Equals("EL"))
            {
                // 9 digits
                Regex re = new Regex(@"^[0-9]{9}$");

                if (!re.IsMatch(num))
                {
                    return null;
                }
            }
            else if (cc.Equals("ES"))
            {
                // 9 chars
                Regex re = new Regex(@"^[0-9A-Z]{9}$");

                if (!re.IsMatch(num))
                {
                    return null;
                }
            }
            else if (cc.Equals("FI"))
            {
                // 8 digits
                Regex re = new Regex(@"^[0-9]{8}$");

                if (!re.IsMatch(num))
                {
                    return null;
                }
            }
            else if (cc.Equals("FR"))
            {
                // 11 chars
                Regex re = new Regex(@"^[0-9A-Z]{11}$");

                if (!re.IsMatch(num))
                {
                    return null;
                }
            }
            else if (cc.Equals("GB"))
            {
                // 5-12 chars
                Regex re = new Regex(@"^[0-9A-Z]{5,12}$");

                if (!re.IsMatch(num))
                {
                    return null;
                }
            }
            else if (cc.Equals("HR"))
            {
                // 11 digits
                Regex re = new Regex(@"^[0-9]{11}$");

                if (!re.IsMatch(num))
                {
                    return null;
                }
            }
            else if (cc.Equals("HU"))
            {
                // 8 digits
                Regex re = new Regex(@"^[0-9]{8}$");

                if (!re.IsMatch(num))
                {
                    return null;
                }
            }
            else if (cc.Equals("IE"))
            {
                // 8-9 chars
                Regex re = new Regex(@"^[0-9A-Z]{8,9}$");

                if (!re.IsMatch(num))
                {
                    return null;
                }
            }
            else if (cc.Equals("IT"))
            {
                // 11 digits
                Regex re = new Regex(@"^[0-9]{11}$");

                if (!re.IsMatch(num))
                {
                    return null;
                }
            }
            else if (cc.Equals("LT"))
            {
                // 9-12 digits
                Regex re = new Regex(@"^[0-9]{9,12}$");

                if (!re.IsMatch(num))
                {
                    return null;
                }
            }
            else if (cc.Equals("LU"))
            {
                // 8 digits
                Regex re = new Regex(@"^[0-9]{8}$");

                if (!re.IsMatch(num))
                {
                    return null;
                }
            }
            else if (cc.Equals("LV"))
            {
                // 11 digits
                Regex re = new Regex(@"^[0-9]{11}$");

                if (!re.IsMatch(num))
                {
                    return null;
                }
            }
            else if (cc.Equals("MT"))
            {
                // 8 digits
                Regex re = new Regex(@"^[0-9]{8}$");

                if (!re.IsMatch(num))
                {
                    return null;
                }
            }
            else if (cc.Equals("NL"))
            {
                // 12 chars
                Regex re = new Regex(@"^[0-9A-Z]{12}$");

                if (!re.IsMatch(num))
                {
                    return null;
                }
            }
            else if (cc.Equals("PL"))
            {
                // 10 digits
                Regex re = new Regex(@"^[0-9]{10}$");

                if (!re.IsMatch(num))
                {
                    return null;
                }
            }
            else if (cc.Equals("PT"))
            {
                // 9 digits
                Regex re = new Regex(@"^[0-9]{9}$");

                if (!re.IsMatch(num))
                {
                    return null;
                }
            }
            else if (cc.Equals("RO"))
            {
                // 2-10 digits
                Regex re = new Regex(@"^[0-9]{2,10}$");

                if (!re.IsMatch(num))
                {
                    return null;
                }
            }
            else if (cc.Equals("SE"))
            {
                // 12 digits
                Regex re = new Regex(@"^[0-9]{12}$");

                if (!re.IsMatch(num))
                {
                    return null;
                }
            }
            else if (cc.Equals("SI"))
            {
                // 8 digits
                Regex re = new Regex(@"^[0-9]{8}$");

                if (!re.IsMatch(num))
                {
                    return null;
                }
            }
            else if (cc.Equals("SK"))
            {
                // 10 digits
                Regex re = new Regex(@"^[0-9]{10}$");

                if (!re.IsMatch(num))
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

            return nip;
        }

		/// <summary>
		/// Konwertuje podany numer EU VAT do postaci znormalizowanej
		/// </summary>
		/// <param name="nip">numer EU VAT w dowolnym formacie</param>
		/// <returns>znormalizowany numer EU VAT</returns>
		string IEUVAT.Normalize(string nip)
		{
			return Normalize(nip);
		}

		/// <summary>
		/// Sprawdza poprawność numeru EU VAT
		/// </summary>
		/// <param name="nip">numer EU VAT</param>
		/// <returns>true jeżeli podany numer jest prawidłowy</returns>
		[ComVisible(false)]
		public static bool IsValid(string nip)
        {
            if ((nip = Normalize(nip)) == null)
            {
                return false;
            }

            string cc = nip.Substring(0, 2).ToUpper();
            string num = nip.Substring(2).ToUpper();

            if (cc.Equals("PL"))
            {
                return NIP.IsValid(num);
            }

            return true;
		}

		/// <summary>
		/// Sprawdza poprawność numeru EU VAT
		/// </summary>
		/// <param name="nip">numer EU VAT</param>
		/// <returns>true jeżeli podany numer jest prawidłowy</returns>
		bool IEUVAT.IsValid(string nip)
		{
			return IsValid(nip);
		}
	}

	#endregion
}
