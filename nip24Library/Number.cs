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
	/// <summary>
	/// Typy numerów identyfikujących firmę
	/// </summary>
	[Guid("0F6089E4-9C66-47DE-83FF-51FD37277F21")]
	[ComVisible(true)]
	public enum Number
    {
        NIP = 1,
        REGON,
        KRS,
        EUVAT
    }
}
