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
	/// Interfejs statusu firmy w rejestrze VAT
	/// </summary>
	[Guid("67A29CBA-B095-480E-9F5F-0A4BEAB83810")]
	[ComVisible(true)]
	public interface IVATStatus
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
		/// Numer REGON
		/// </summary>
		[DispId(3)]
		string REGON { get; set; }

		/// <summary>
		/// Nazwa firmy
		/// </summary>
		[DispId(4)]
		string Name { get; set; }

		/// <summary>
		/// Status jako jedna z wartości: NOT_REGISTERED, ACTIVE, EXEMPTED
		/// </summary>
		[DispId(5)]
		int Status { get; set; }

		/// <summary>
		/// Tekstowy opis statusu
		/// </summary>
		[DispId(6)]
		string Result { get; set; }

		/// <summary>
		/// Data sprawdzenia
		/// </summary>
		[DispId(7)]
		DateTime Date { get; set; }

		/// <summary>
		/// Źródło informacji o statusie
		/// </summary>
		[DispId(8)]
		string Source { get; set; }

		[DispId(9)]
		string ToString();
    }

	#endregion

	#region implementation

	/// <summary>
	/// Status firmy w rejestrze VAT
	/// </summary>
	[Guid("2D003F16-B168-4C6D-AFE7-9105F1A09B09")]
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	public class VATStatus : IVATStatus
	{
		public const int NOT_REGISTERED = 1;
		public const int ACTIVE = 2;
		public const int EXEMPTED = 3;

		/// <summary>
		/// Unikalny identyfikator odpowiedzi
		/// </summary>
		public string UID { get; set; }

		/// <summary>
		/// Numer NIP
		/// </summary>
		public string NIP { get; set; }

		/// <summary>
		/// Numer REGON
		/// </summary>
		public string REGON { get; set; }

		/// <summary>
		/// Nazwa firmy
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Status jako jedna z wartości: NOT_REGISTERED, ACTIVE, EXEMPTED
		/// </summary>
		public int Status { get; set; }

		/// <summary>
		/// Tekstowy opis statusu
		/// </summary>
		public string Result { get; set; }

		/// <summary>
		/// Data sprawdzenia
		/// </summary>
		[ComVisible(false)]
		public DateTime? Date { get; set; }

		/// <summary>
		/// Data sprawdzenia
		/// </summary>
		DateTime IVATStatus.Date
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
		public VATStatus()
		{
		}

		public override string ToString()
		{
			return "VATStatus: [uid = " + UID
				+ ", nip = " + NIP
				+ ", regon = " + REGON
				+ ", name = " + Name
				+ ", status = " + Status
				+ ", result = " + Result
				+ ", date = " + Date
				+ ", source = " + Source
				+ "]";
		}
	}

	#endregion
}
