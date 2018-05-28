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

namespace NIP24
{
	/// <summary>
	/// Przykładowy program testowy
	/// </summary>
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				// Utworzenie obiektu klienta usługi serwisu produkcyjnego
				// id – ciąg znaków reprezentujący identyfikator klucza API
				// key – ciąg znaków reprezentujący klucz API
				// NIP24Client nip24 = new NIP24Client("id", "key");

				// Utworzenie obiektu klienta usługi serwisu testowego
				NIP24Client nip24 = new NIP24Client();

				string nip = "7171642051";
                string nip_eu = "PL" + nip;

				// Sprawdzenie stanu konta
				AccountStatus account = nip24.GetAccountStatus();

				if (account != null)
				{
					Console.WriteLine(account);
				}
				else
				{
					Console.WriteLine("Błąd: " + nip24.LastError);
				}

				// Sprawdzenie statusu fimy
				bool active = nip24.IsActive(Number.NIP, nip);

				if (active)
				{
					Console.WriteLine("Firma prowadzi aktywną działalność");
				}
				else
				{
					if (String.IsNullOrEmpty(nip24.LastError))
					{
						Console.WriteLine("Firma zawiesiła lub zakończyła działalność");
					}
					else
					{
						Console.WriteLine("Błąd: " + nip24.LastError);
					}
				}

				// Sprawdzenie statusu firmy w rejestrze VAT
				VATStatus vat = nip24.GetVATStatus(Number.NIP, nip, true);

				if (vat != null)
				{
					Console.WriteLine("NIP: " + vat.NIP);
					Console.WriteLine("REGON: " + vat.REGON);
					Console.WriteLine("Nazwa firmy: " + vat.Name);
					Console.WriteLine("Status: " + vat.Status);
					Console.WriteLine("Wynik: " + vat.Result);
					Console.WriteLine("Data sprawdzenia: " + vat.Date);
					Console.WriteLine("Źródło: " + vat.Source);
				}
				else
				{
					Console.WriteLine("Błąd: " + nip24.LastError);
				}

				// Wywołanie metody zwracającej dane do faktury
				InvoiceData invoice = nip24.GetInvoiceData(Number.NIP, nip, false);

				if (invoice != null)
				{
                    Console.WriteLine("Nazwa: " + invoice.Name);
					Console.WriteLine("Adres : " + invoice.PostCode + " " + invoice.PostCity + " " + invoice.Street
						+ " " + invoice.StreetNumber + "/" + invoice.HouseNumber);
                    Console.WriteLine("NIP: " + invoice.NIP);
                }
                else
				{
					Console.WriteLine("Błąd: " + nip24.LastError);
				}

				// Wywołanie metody zwracającej szczegółowe dane firmy
				AllData all = nip24.GetAllData(Number.NIP, nip, false);

				if (all != null)
				{
					Console.WriteLine(all);
				}
				else
				{
					Console.WriteLine("Błąd: " + nip24.LastError);
				}

                // Wywołanie metody zwracającej dane z systemu VIES
                VIESData vies = nip24.GetVIESData(nip_eu);

                if (vies != null)
                {
                    Console.WriteLine(vies);
                }
                else
                {
                    Console.WriteLine("Błąd: " + nip24.LastError);
                }
			}
			catch (Exception e)
			{
				System.Diagnostics.Debug.WriteLine(e.StackTrace);
			}
		}
	}
}
