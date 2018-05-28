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
	/// Weryfikator numeru NIP
	/// </summary>
	[Guid("8CD86293-4132-4FF4-B290-BCBCB6260A7B")]
	[ComVisible(true)]
	public interface INIP
    {
		/// <summary>
		/// Konwertuje podany numer NIP do postaci znormalizowanej
		/// </summary>
		/// <param name="nip">numer NIP w dowolnym formacie</param>
		/// <returns>znormalizowany numer NIP</returns>
		[DispId(1)]
		string Normalize(string nip);

		/// <summary>
		/// Sprawdza poprawność numeru NIP
		/// </summary>
		/// <param name="nip">numer NIP</param>
		/// <returns>true jeżeli podany numer jest prawidłowy</returns>
		[DispId(2)]
		bool IsValid(string nip);
    }

	#endregion

	#region implementation

	/// <summary>
	/// Weryfikator numeru NIP
	/// </summary>
	[Guid("8A8B804D-11F2-4153-B4DF-4C86103659B0")]
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	public class NIP : INIP
	{
		/// <summary>
		/// Konwertuje podany numer NIP do postaci znormalizowanej
		/// </summary>
		/// <param name="nip">numer NIP w dowolnym formacie</param>
		/// <returns>znormalizowany numer NIP</returns>
		[ComVisible(false)]
		public static string Normalize(string nip)
		{
			if (nip == null || nip.Length == 0)
			{
				return null;
			}

			nip = nip.Replace("-", "");
			nip = nip.Trim();

			Regex re = new Regex(@"^[0-9]{10}$");

			if (!re.IsMatch(nip))
			{
				return null;
			}

			return nip;
		}

		/// <summary>
		/// Konwertuje podany numer NIP do postaci znormalizowanej
		/// </summary>
		/// <param name="nip">numer NIP w dowolnym formacie</param>
		/// <returns>znormalizowany numer NIP</returns>
		string INIP.Normalize(string nip)
		{
			return Normalize(nip);
		}

		/// <summary>
		/// Sprawdza poprawność numeru NIP
		/// </summary>
		/// <param name="nip">numer NIP</param>
		/// <returns>true jeżeli podany numer jest prawidłowy</returns>
		[ComVisible(false)]
		public static bool IsValid(string nip)
		{
			if ((nip = Normalize(nip)) == null)
			{
				return false;
			}

			int[] w = {
				6, 5, 7, 2, 3, 4, 5, 6, 7
			};

			int sum = 0;

			for (int i = 0; i < w.Length; i++)
			{
				sum += Convert.ToInt32("" + nip[i], 10) * w[i];
			}

			sum %= 11;

			if (sum != Convert.ToInt32("" + nip[9], 10))
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Sprawdza poprawność numeru NIP
		/// </summary>
		/// <param name="nip">numer NIP</param>
		/// <returns>true jeżeli podany numer jest prawidłowy</returns>
		bool INIP.IsValid(string nip)
		{
			return IsValid(nip);
		}
	}

	#endregion
}
