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
using System.Text.RegularExpressions;

namespace NIP24
{
	#region interface

	/// <summary>
	/// Interfejs weryfikatora numeru REGON
	/// </summary>
	[Guid("EBA2D0C5-D7C9-4E18-AF01-C3F6DB64DA71")]
	[ComVisible(true)]
	public interface IREGON
    {
		/// <summary>
		/// Konwertuje podany numer REGON do postaci znormalizowanej
		/// </summary>
		/// <param name="regon">numer REGON w dowolnym formacie</param>
		/// <returns>znormalizowany numer REGON</returns>
		[DispId(1)]
		string Normalize(string regon);

		/// <summary>
		/// Sprawdza poprawność numeru REGON
		/// </summary>
		/// <param name="regon">numer REGON</param>
		/// <returns>true jeżeli podany numer jest prawidłowy</returns>
		[DispId(2)]
		bool IsValid(string regon);
    }

	#endregion

	#region implementation

	/// <summary>
	/// Weryfikator numeru REGON
	/// </summary>
	[Guid("A9A5DC62-60C8-4361-B7CC-D5AEE09FF287")]
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	public class REGON : IREGON
	{
		/// <summary>
		/// Konwertuje podany numer REGON do postaci znormalizowanej
		/// </summary>
		/// <param name="regon">numer REGON w dowolnym formacie</param>
		/// <returns>znormalizowany numer REGON</returns>
		[ComVisible(false)]
		public static string Normalize(string regon)
		{
			if (regon == null || regon.Length == 0)
			{
				return null;
			}

			regon = regon.Trim();

			Regex re = new Regex(@"^[0-9]{9,14}$");

			if (!re.IsMatch(regon))
			{
				return null;
			}

			if (regon.Length != 9 && regon.Length != 14)
			{
				return null;
			}

			return regon;
		}

		/// <summary>
		/// Konwertuje podany numer REGON do postaci znormalizowanej
		/// </summary>
		/// <param name="regon">numer REGON w dowolnym formacie</param>
		/// <returns>znormalizowany numer REGON</returns>
		string IREGON.Normalize(string regon)
		{
			return Normalize(regon);
		}

		/// <summary>
		/// Sprawdza poprawność numeru REGON
		/// </summary>
		/// <param name="regon">numer REGON</param>
		/// <returns>true jeżeli podany numer jest prawidłowy</returns>
		[ComVisible(false)]
		public static bool IsValid(string regon)
		{
			if ((regon = Normalize(regon)) == null)
			{
				return false;
			}

			if (regon.Length == 9)
			{
				return IsValidR9(regon);
			}
			else
			{
				if (!IsValidR9(regon.Substring(0, 9)))
				{
					return false;
				}

				return IsValidR14(regon);
			}
		}

		/// <summary>
		/// Sprawdza poprawność numeru REGON
		/// </summary>
		/// <param name="regon">numer REGON</param>
		/// <returns>true jeżeli podany numer jest prawidłowy</returns>
		bool IREGON.IsValid(string regon)
		{
			return IsValid(regon);
		}

		/// <summary>
		/// Sprawdza poprawność 9-cyfrowego numeru REGON
		/// </summary>
		/// <param name="regon">numer REGON</param>
		/// <returns>true jeżeli podany numer jest prawidłowy</returns>
		private static bool IsValidR9(string regon)
		{
			int[] w = {
				8, 9, 2, 3, 4, 5, 6, 7
			};

			int sum = 0;

			for (int i = 0; i < w.Length; i++)
			{
				sum += Convert.ToInt32("" + regon[i], 10) * w[i];
			}

			sum %= 11;

			if (sum == 10)
			{
				sum = 0;
			}

			if (sum != Convert.ToInt32("" + regon[8], 10))
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Sprawdza poprawność 14-cyfrowego numeru REGON
		/// </summary>
		/// <param name="regon">numer REGON</param>
		/// <returns>true jeżeli podany numer jest prawidłowy</returns>
		private static bool IsValidR14(string regon)
		{
			int[] w = {
				2, 4, 8, 5, 0, 9, 7, 3, 6, 1, 2, 4, 8
			};

			int sum = 0;

			for (int i = 0; i < w.Length; i++)
			{
				sum += Convert.ToInt32("" + regon[i], 10) * w[i];
			}

			sum %= 11;

			if (sum == 10)
			{
				sum = 0;
			}

			if (sum != Convert.ToInt32("" + regon[13], 10))
			{
				return false;
			}

			return true;
		}
	}

	#endregion
}
