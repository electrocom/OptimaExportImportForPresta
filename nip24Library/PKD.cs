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
	/// Interfejs elementu słownika z rejestru PKD (Polska Klasyfikacja Działalności)
	/// </summary>
	[Guid("679DCF2A-222A-4F34-B929-20F3753B60B4")]
	[ComVisible(true)]
	public interface IPKD
    {
		/// <summary>
		/// Kod PKD
		/// </summary>
		[DispId(1)]
		string Code { get; set; }

		/// <summary>
		/// Opis kodu PKD
		/// </summary>
		[DispId(2)]
		string Description { get; set; }

		/// <summary>
		/// Znacznik przeważającej działalności
		/// </summary>
		[DispId(3)]
		bool Primary { get; set; }

		[DispId(4)]
		string ToString();
    }

	#endregion

	#region implementation

	/// <summary>
	/// Element słownika z rejestru PKD (Polska Klasyfikacja Działalności)
	/// </summary>
	[Guid("B501C035-1F51-461D-87E4-6189E2AB9045")]
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	public class PKD : IPKD
	{
		/// <summary>
		/// Kod PKD
		/// </summary>
		public string Code { get; set; }

		/// <summary>
		/// Opis kodu PKD
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Znacznik przeważającej działalności
		/// </summary>
		public bool Primary { get; set; }

		/// <summary>
		/// Tworzy nowy, pusty obiekt
		/// </summary>
		public PKD()
		{
		}

		public override string ToString()
		{
			return "PKD: [primary = " + Primary
				+ ", code = " + Code
				+ ", description = " + Description
				+ "]";
		}
	}

	#endregion
}
