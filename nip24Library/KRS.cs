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
	/// Interfejs weryfikatora numeru KRS
	/// </summary>
	[Guid("F0B4C852-942A-4973-993F-A36C54382035")]
	[ComVisible(true)]
	public interface IKRS
    {
		/// <summary>
		/// Konwertuje podany numer KRS do postaci znormalizowanej
		/// </summary>
		/// <param name="krs">numer KRS w dowolnym formacie</param>
		/// <returns>znormalizowany numer KRS</returns>
		[DispId(1)]
		string Normalize(string krs);

		/// <summary>
		/// Sprawdza poprawność numeru KRS
		/// </summary>
		/// <param name="krs">numer KRS</param>
		/// <returns>true jeżeli podany numer jest prawidłowy</returns>
		[DispId(2)]
		bool IsValid(string krs);
    }

	#endregion

	#region implementation

	/// <summary>
	/// Weryfikator numeru KRS
	/// </summary>
	[Guid("25F3D24F-287C-4844-8003-FFC73D95B86E")]
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	public class KRS : IKRS
	{
		/// <summary>
		/// Konwertuje podany numer KRS do postaci znormalizowanej
		/// </summary>
		/// <param name="krs">numer KRS w dowolnym formacie</param>
		/// <returns>znormalizowany numer KRS</returns>
		[ComVisible(false)]
		public static string Normalize(string krs)
		{
			if (krs == null || krs.Length == 0)
			{
				return null;
			}

			krs = krs.Trim();
			krs = krs.PadLeft(10, '0');

			Regex re = new Regex(@"^[0-9]{10}$");

			if (!re.IsMatch(krs))
			{
				return null;
			}

			return krs;
		}

		/// <summary>
		/// Konwertuje podany numer KRS do postaci znormalizowanej
		/// </summary>
		/// <param name="krs">numer KRS w dowolnym formacie</param>
		/// <returns>znormalizowany numer KRS</returns>
		string IKRS.Normalize(string krs)
		{
			return Normalize(krs);
		}

		/// <summary>
		/// Sprawdza poprawność numeru KRS
		/// </summary>
		/// <param name="krs">numer KRS</param>
		/// <returns>true jeżeli podany numer jest prawidłowy</returns>
		[ComVisible(false)]
		public static bool IsValid(string krs)
		{
			if ((krs = Normalize(krs)) == null)
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Sprawdza poprawność numeru KRS
		/// </summary>
		/// <param name="krs">numer KRS</param>
		/// <returns>true jeżeli podany numer jest prawidłowy</returns>
		bool IKRS.IsValid(string krs)
		{
			return IsValid(krs);
		}
	}

	#endregion
}
