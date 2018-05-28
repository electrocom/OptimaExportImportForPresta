using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net;
using System.Xml;
using CDNTwrb1;
using CDNBase;
using CDNHlmn;
using CDNHeal;
using OP_KASBOLib;
using System.Configuration;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using NIP24;
namespace OptimaExportImportForPresta
{
    class ComarchOptimaImportOrder
    {
        static EventLog eventLog;
        static IApplication Application = null;
        static ILogin Login = null;
        static string connectionString;
        static XmlNode curOrderXML;
        public  ComarchOptimaImportOrder()
        {
            connectionString = "Data Source=" + Properties.Settings.Default.serverName + ";" +
                                                                "Initial Catalog=" + Properties.Settings.Default.dataBaseName + ";" +
                                                                "User id=" + Properties.Settings.Default.userName + ";" +
                                                                "Password=" + Properties.Settings.Default.secret + ";";
        }
        static protected bool LogowanieAutomatyczne(EventLog eventLog)
        {
            Application = new CDNBase.Application();
            string Operator = Properties.Settings.Default.erpLogin;
            string Haslo = Properties.Settings.Default.erpPasswd;
            string Firma = Properties.Settings.Default.erpName;
            Environment.CurrentDirectory = Properties.Settings.Default.erpSrc;
            Application.LockApp(256, 5000, null, null, null, null);
            object[] hPar = new object[] {
                         0,  0,   0,  0,   0,   1,  0,    0,   0,   0,   0,   0,   0,  0,   1,   0,  0 ,0};	// do jakich modułów będzie logowanie
           /* Kolejno:  KP, KH, KHP, ST, FA, MAG, PK, PKXL, CRM, ANL, DET, BIU, SRW, ODB, KB, KBP, HAP  
             */
            try
            {
               Login = Application.Login(Operator, Haslo, Firma, hPar[0], hPar[1], hPar[2], hPar[3], hPar[4], hPar[5], hPar[6], hPar[7], hPar[8], hPar[9], hPar[10], hPar[11], hPar[12], hPar[13], hPar[14], hPar[15], hPar[16], hPar[17]);
               // Login = Application.Login(Operator, Haslo, Firma);

                eventLog.WriteEntry("Pomyślnie zalogowano do Optimy log in Optima:" + Environment.NewLine, EventLogEntryType.Information, 0);
                return true;
            }
            catch (Exception ex)
            {
                eventLog.WriteEntry("Błąd Logowania do Optimy:" + Environment.NewLine + ex , EventLogEntryType.Error, 0);
                return false;
            }


        }

        protected static void Wylogowanie()
        {
            Login = null;
            Application.UnlockApp();
            Application = null;
        }


    [STAThread]
        private void comarchOptimaImportOrderSTA()
        {
            string akronim;

            try
            {
                string orderId = "";
                string reference = "";
                WebClient client = new WebClient();
                string prestaResponse = "";

                try
                {
                    NameValueCollection postData = new NameValueCollection() { { "action", "getXmlOrders" } };
                    prestaResponse = Encoding.UTF8.GetString(client.UploadValues(getOrdersGate(), postData));
                }
                catch (Exception exPresta)
                {
                    eventLog.WriteEntry("Error 1 on order import:" + Environment.NewLine + exPresta.ToString(), EventLogEntryType.Error, 0);
                }

                XmlDocument ordersXML = new XmlDocument();
                ordersXML.LoadXml(prestaResponse);
                XmlElement ordersXMLroot = ordersXML.DocumentElement;

                if (ordersXMLroot.ChildNodes.Count > 0)
                {

                    foreach (XmlNode orderXML in ordersXMLroot.ChildNodes)
                    {
                        bool error = true;
                        try
                        {
                            eventLog.WriteEntry("Rozpoczynam import zamówień:" + Environment.NewLine , EventLogEntryType.Information, 0);

                            Dictionary<string, List<XmlNode>> splitedOrder = new Dictionary<string, List<XmlNode>>();
                            orderId = orderXML["id"].InnerText;
                            reference= orderXML["reference"].InnerText;
                            curOrderXML = orderXML;

                           if ( CzyZaimportowane(reference)) //Jeśłi zamówienie zaimportowane to przerywam.
                           continue;

                                XmlNode tmpNode = orderXML["associations"];
                   
                                LogowanieAutomatyczne(eventLog);
                                AdoSession Sesja = Login.CreateSession();


                                try {

                                    XmlNode xmlBilling = orderXML.SelectSingleNode("address_invoice")["address"];
                                    XmlNode xmlShipping = orderXML.SelectSingleNode("address_delivery")["address"];
                                    XmlNode xmlCustomer = orderXML.SelectSingleNode("customer");
                                    XmlNode xmlCarrier = orderXML.SelectSingleNode("carrier"); 
                                try
                                    {
                                       
                                        Kontrahent knt;
                                        XmlNode xmltmp = orderXML["address_invoice"]["address"];
                                        DefAtrybut defAtrybut = Sesja.CreateObject("CDN.DefAtrybuty").Item("DeA_Kod='B2BID'"); //Pobranie id atrybutu
                                        string KnA_DeAId = "KnA_DeAId=" + defAtrybut.ID.ToString() + "";
                                        SqlConnection conn = new SqlConnection();
                                        conn.ConnectionString = connectionString;
                                        conn.Open();
                                        SqlCommand idKnt = new SqlCommand("SELECT    [KnA_PodmiotId]  ,[KnA_WartoscTxt]  FROM [CDN].[KntAtrybuty] where [KnA_DeAId] = '" + defAtrybut.ID.ToString() + "' AND [KnA_WartoscTxt] = '"+ xmlBilling["id"].InnerText+"'" , conn);


                                        SqlDataReader reader = idKnt.ExecuteReader();
                                        int podmiotId = 0;


                                    try { 
                                        if (reader.Read())
                                        {   Int32.TryParse(reader["KnA_PodmiotId"].ToString(), out podmiotId);
                                            knt = Sesja.CreateObject("CDN.Kontrahenci").Item("Knt_kntid=" + podmiotId);
                                        }
                                        else
                                        {
                                            podmiotId = 0;

                                            try
                                            {
                                                knt = Sesja.CreateObject("CDN.Kontrahenci").Item("Knt_Kod='" + ZbudujAkronim(curOrderXML) + "'");
                                            }catch (Exception ex)
                                            {
                                                knt = null;
                                            }

                                            if (knt==null)
                                            {
                                                knt = Sesja.CreateObject("CDN.Kontrahenci").AddNew();
                                                knt.Akronim = ZbudujAkronim(curOrderXML); ;
                                                knt.Rodzaj_Odbiorca = 1;
                                                knt.Rodzaj_Dostawca = 0;
                                                PobierzDaneDoFaktury(xmlBilling,knt);
                                                KntAtrybut b2bId = knt.Atrybuty.AddNew();
                                                b2bId.DefAtrybut = defAtrybut;
                                                b2bId.ROSaveMode = 1;
                                                b2bId.Wartosc = xmlBilling["id"].InnerText;
                                                knt.Kategoria = Sesja.CreateObject("CDN.Kategorie").Item("Kat_KodOgolny='ALLEGRO MAJSTERKOWAN'");
                                                Sesja.Save();
                                                podmiotId = knt.ID;
                                             }
                                            

                                        }
                                    }
                                    catch (Exception exPresta)
                                    { 
                                       // eventLog.WriteEntry("Błąd przy tworzeniu Kontrahenta: " + orderId + Environment.NewLine + "" + exPresta, EventLogEntryType.Error, 0);
                                      
                                        throw ;
                                    }



                                        reader.Close();
                                        reader.Dispose();

                                        conn.Close();


                                        DokumentHaMag dok = Sesja.CreateObject("CDN.DokumentyHaMag").AddNew();
                                        dok.Rodzaj = 308000;
                                        dok.TypDokumentu = 308;
                                        

                                        var rNumerator = dok.Numerator;

                                        DefinicjaDokumentu dokDef = Sesja.CreateObject("CDN.DefinicjeDokumentow").Item("DDf_Symbol='RO'");
                                        rNumerator.DefinicjaDokumentu = dokDef;
                                        knt = Sesja.CreateObject("CDN.Kontrahenci").Item("Knt_kntid=" + knt.ID);
                                        dok.Podmiot = knt;
                                       
                                        dok.WalutaSymbol = "PLN";
                                    //  dok.OdbEmail = xmlShipping["Email"].InnerText;
                            
                                          dok.TypNB = 2; /* 1 - Licz od netto, 2 -licz od brutto*/
                                          dok.OdbTelefon = xmlShipping["phone"].InnerText;
                                        dok.OdbNazwa1 = xmlShipping["company"].InnerText;
                                        dok.OdbNazwa2 = xmlShipping["firstname"].InnerText + " " + xmlShipping["lastname"].InnerText;
                                        dok.OdbAdres.Ulica = xmlShipping["address1"].InnerText;
                                        dok.OdbAdres.NrDomu = xmlShipping["address2"].InnerText;
                                        //dok.OdbAdres.NrLokalu = xmlShipping["Street3"].InnerText;
                                        dok.OdbAdres.Miasto = xmlShipping["city"].InnerText;
                                        dok.OdbAdres.KodPocztowy = xmlShipping["postcode"].InnerText;
                                        dok.OdbAdres.Kraj = "Polska";
                                        //dok.OdbAdres.Wojewodztwo = xmlShipping["Region"].InnerText; 
                                       DokAtrybut dostawa = dok.Atrybuty.AddNew();
                                        dostawa.Kod = "METODADOSTAWY";
                                        dostawa.Wartosc = xmlCarrier["name"].InnerText;
                                        DokAtrybut platnosc = dok.Atrybuty.AddNew();
                                        platnosc.Kod = "METODAPLATNOSCI";
                                        platnosc.Wartosc = orderXML["payment"].InnerText;

                                    DokAtrybut b2bIdDok = dok.Atrybuty.AddNew();
                                    b2bIdDok.Kod = "B2BID";
                                    b2bIdDok.Wartosc = orderId;

                                    dok.NumerObcy = reference;
                                    // dok.MagazynZrodlowyID = int.Parse(orderContent.Key);

                                    Kategoria kategoria;
                                    if (orderXML["module"].InnerText == "allegro")
                                    {
                                        kategoria = Sesja.CreateObject("CDN.Kategorie").Item("Kat_KodOgolny='ALLEGRO MAJSTERKOWAN'");

                                    }
                                    else
                                    {
                                        kategoria = Sesja.CreateObject("CDN.Kategorie").Item("Kat_KodOgolny='MAJSTERKOWANIE.EU'");
                                      
                                    }
                                    dok.Kategoria = kategoria;






                                    CDNBase.ICollection FormyPlatnosci = (CDNBase.ICollection)(Sesja.CreateObject("CDN.FormyPlatnosci", null));
                                    if (orderXML["module"].InnerText== "ps_cashondelivery" || orderXML["payment"].InnerText.IndexOf("collect_on_delivery")!=-1) {
                                        dok.FormaPlatnosci = (OP_KASBOLib.FormaPlatnosci)FormyPlatnosci["FPl_FPlId=14"]; /*Pobranie IE*/
                                       
                                    }
                                    else {
                                        dok.FormaPlatnosci = (OP_KASBOLib.FormaPlatnosci)FormyPlatnosci["FPl_FPlId=13"];  /*Przedpłata IE*/
                                    }

                                    ICollection pozycje = dok.Elementy;
                                      
                                        foreach (XmlNode orderXmlElement in tmpNode["order_rows"])
                                        {
                                                ElementHaMag pozycja = pozycje.AddNew();
                                            int TwrTwrId;
                                                if (Int32.TryParse(orderXmlElement["TwrTwrId"].InnerText, out TwrTwrId))
                                                {
                                                pozycja.TowarID = TwrTwrId;
                                                 }
                                            else
                                            {
                                            pozycja.TowarID = Convert.ToInt32(Properties.Settings.Default.twrIdException);
                                            }

                                                var product_price = orderXmlElement["unit_price_tax_incl"].InnerText.Replace(".", ",");
                                                pozycja.CenaT = Convert.ToDecimal(product_price);
                                                pozycja.IloscJM = Convert.ToDouble(orderXmlElement["product_quantity"].InnerText);

                                        }

                                    ElementHaMag carrier = pozycje.AddNew();
                                    carrier.TowarID = Convert.ToInt32(Properties.Settings.Default.twrIdCarrier); ;
                                    var total_shipping = orderXML["total_shipping"].InnerText.Replace(".", ",");
                                    carrier.CenaT = Convert.ToDecimal(total_shipping);
                                    carrier.IloscJM = Convert.ToDouble(1);
                                    carrier.UstawNazweTowaru(  xmlCarrier["name"].InnerText);
                                    carrier.WartoscZakupu= Convert.ToDecimal(total_shipping);





                                    error = false;
                                        Sesja.Save();
                                    OznaczJakoPobrane(Convert.ToInt32(orderXML["id_optimaexportorders"].InnerText), Convert.ToInt32(orderXML["id"].InnerText)); 
                                    }
                                    catch (Exception exDokDef)
                                    {
                                        eventLog.WriteEntry("Error 2 on order import: " + orderId + Environment.NewLine + exDokDef.ToString(), EventLogEntryType.Error, 0);
                                    }
                                }
                                catch (Exception exPresta)
                                {
                                    error = true;
                                    eventLog.WriteEntry("Error on order import: " +orderId + Environment.NewLine + "dzieki tej opcji wystapienie bledu importu zamowienia nie powinno zabijac optimy" + exPresta, EventLogEntryType.Error, 0);
                                    Wylogowanie();
                                }
                                Wylogowanie();
                            

                        }
                        catch (Exception exPresta)
                        {
                            error = true;
                             eventLog.WriteEntry("Error 3 on order import: " + orderId + Environment.NewLine + Environment.NewLine + exPresta.ToString(), EventLogEntryType.Error, 0);
                        }



                    }
                }


            }
            catch (Exception exception)
            {
                eventLog.WriteEntry("Błąd ogólny:" + Environment.NewLine + exception.ToString(), EventLogEntryType.Error, 0);
            }



        }

        public void PobierzDaneDoFakturyGus( Kontrahent knt)
        {
            string nip = knt.Nip;
            NIP24Client nip24 = new NIP24Client(Properties.Settings.Default.nip24Id, Properties.Settings.Default.nip24Key);
            AccountStatus account = nip24.GetAccountStatus();
            if (account != null)
            {
                eventLog.WriteEntry("Nip24 konto użytkownika: " + account + Environment.NewLine, EventLogEntryType.Information, 0);
            }
            else
            {
                eventLog.WriteEntry("Nip24 błąd: " + nip24.LastError + Environment.NewLine, EventLogEntryType.Information, 0);
            }


            // Sprawdzenie statusu fimy
            bool active = nip24.IsActive(Number.NIP, nip);

            if (active)
            {
                Console.WriteLine("Firma prowadzi aktywną działalność");
                eventLog.WriteEntry("Firma prowadzi aktywną działalność " + Environment.NewLine, EventLogEntryType.Information, 0);
            }
            else
            {
                if (String.IsNullOrEmpty(nip24.LastError))
                {
                    eventLog.WriteEntry("Firma zawiesiła lub zakończyła działalność " + Environment.NewLine, EventLogEntryType.Warning, 0);
                    //   Console.WriteLine("Firma zawiesiła lub zakończyła działalność");
                }
                else
                {
                    eventLog.WriteEntry("Błąd: " + nip24.LastError + Environment.NewLine, EventLogEntryType.Error, 0);
                    //Console.WriteLine("Błąd: " + nip24.LastError);
                }
            }


            InvoiceData invoice = nip24.GetInvoiceData(Number.NIP, nip, false);

            if (invoice != null)
            {
               // Console.WriteLine("Nazwa: " + invoice.Name);
                knt.Nazwa1 = invoice.Name;
                knt.Adres.KodPocztowy = invoice.PostCode;
                knt.Adres.Miasto = invoice.PostCity;
                knt.Adres.Ulica = invoice.Street;
                knt.Adres.NrDomu = invoice.StreetNumber;
                knt.Adres.NrLokalu = invoice.HouseNumber;
                            }
            else
            {
                // Console.WriteLine("Błąd: " + nip24.LastError);
                eventLog.WriteEntry("Błąd pobierania danych do faktury z GUS: " + nip24.LastError + Environment.NewLine, EventLogEntryType.Error, 0);
            }

           

        }

        public  void ZbudujDaneDoFaktury(XmlNode xmlBilling, Kontrahent knt)
        {
            //Ustalanie nazwy kontahenta
            if (xmlBilling["company"].InnerText.Length > 2)
            {
                knt.Nazwa1 = xmlBilling["company"].InnerText;
                knt.Nazwa2 = xmlBilling["firstname"].InnerText + " " + xmlBilling["lastname"].InnerText;

            }
            else
            {
                knt.Nazwa1 = xmlBilling["firstname"].InnerText + " " + xmlBilling["lastname"].InnerText;
            }

            knt.Adres.Ulica = xmlBilling["address1"].InnerText;
            knt.Adres.NrDomu = xmlBilling["address2"].InnerText;
            knt.Adres.Miasto = xmlBilling["city"].InnerText;
            knt.Adres.KodPocztowy = xmlBilling["postcode"].InnerText;
            knt.Adres.Kraj = "Polska";

            //  if (xmlCustomer["email"].InnerText.Length > 2)
            //  knt.Email = xmlCustomer["email"].InnerText;

            if (xmlBilling["phone"].InnerText.Length > 5)
                knt.Telefon = xmlBilling["phone"].InnerText;

            if (xmlBilling["company"].InnerText.Length > 0)
                knt.Nazwa1 = xmlBilling["company"].InnerText;

            if (xmlBilling["address1"].InnerText.Length > 0)
            {
                knt.Adres.Ulica = xmlBilling["address1"].InnerText;
            }

            knt.Adres.Kraj = "Polska";
            if (xmlBilling["postcode"].InnerText.Length > 0)
                knt.Adres.KodPocztowy = xmlBilling["postcode"].InnerText;
            if (xmlBilling["city"].InnerText.Length > 0)
                knt.Adres.Miasto = xmlBilling["city"].InnerText;

        }
        public void PobierzDaneDoFaktury(XmlNode xmlBilling, Kontrahent knt)
        {
            var nip = NIPClean(xmlBilling["vat_number"].InnerText);

            if (xmlBilling["company"].InnerText.Length > 2)
            {
                if (NIPValidate(nip))
                {
                    knt.Nip = nip;
                    PobierzDaneDoFakturyGus(knt);
                }
                else
                {
                    eventLog.WriteEntry("Błędny nip firmy: " + xmlBilling["vat_number"].InnerText + Environment.NewLine, EventLogEntryType.Warning, 0);
                    ZbudujDaneDoFaktury(xmlBilling, knt);
                }
            }
            else
            {
                ZbudujDaneDoFaktury(xmlBilling, knt);
            }
        }
        public string ZbudujAkronim(XmlNode orderXML)
        {
            string akronim="";
            XmlNode xmlBilling = orderXML.SelectSingleNode("address_invoice")["address"];        
            XmlNode xmlCustomer = orderXML.SelectSingleNode("customer");
         
            string allegro = xmlCustomer["note"].InnerText.Split(':')[1].Trim();

            akronim = "B2B_" + xmlBilling["id"].InnerText+"_"+ allegro + "_";
            if (xmlBilling["company"].InnerText.Length > 0)
                akronim += xmlBilling["company"].InnerText.Replace(" ", "");

            akronim += xmlBilling["lastname"].InnerText;
          

            return akronim;
        }

        public string NIPClean(string nip)
        {
            return Regex.Replace(nip, @"[^\d]", "");
        }
        public bool CzyZaimportowane(string reference)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = connectionString;
            conn.Open();

            SqlCommand nrObcy = new SqlCommand("SELECT  [TrN_TrNID]    FROM [CDN].[TraNag] where [TrN_NumerObcy] ='"+ reference+"'", conn);
            SqlDataReader reader = nrObcy.ExecuteReader();
           

            if (reader.Read())
            {
                return true;
            }

            return false;
        }

        public bool OznaczJakoPobrane(int id_optimaexportorders,int id_order)
        {
            WebClient client = new WebClient();
            client.UseDefaultCredentials = true;
            client.Credentials = new NetworkCredential(getapiKey(), "");
            string prestaResponse = "";
            var xml = "<prestashop><optimaexportorder><id>" + id_optimaexportorders.ToString()+ "</id><id_order>687</id_order><export>1</export><id_optimaexportorders>" + id_optimaexportorders.ToString() + "</id_optimaexportorders></optimaexportorder></prestashop>";

            try
            {
                NameValueCollection postData = new NameValueCollection() { { "data", xml } };
                prestaResponse = Encoding.UTF8.GetString(client.UploadData(getApiUrl() + "optimaexportorders/" + id_optimaexportorders.ToString(), "PUT", Encoding.ASCII.GetBytes(xml)));
            }
            catch (Exception exPresta)
            {
             //   eventLog.WriteEntry("Error 1 on order import:" + Environment.NewLine + exPresta.ToString(), EventLogEntryType.Error, 0);
            }

            return true;
        }

         public bool NIPValidate(string NIPValidate)
        {
            const byte lenght = 10;

            ulong nip = ulong.MinValue;
            byte[] digits;
            byte[] weights = new byte[] { 6, 5, 7, 2, 3, 4, 5, 6, 7 };

            if (NIPValidate.Length.Equals(lenght).Equals(false)) return false;

            if (ulong.TryParse(NIPValidate, out nip).Equals(false)) return false;
            else
            {
                string sNIP = NIPValidate.ToString();
                digits = new byte[lenght];

                for (int i = 0; i < lenght; i++)
                {
                    if (byte.TryParse(sNIP[i].ToString(), out digits[i]).Equals(false)) return false;
                }

                int checksum = 0;

                for (int i = 0; i < lenght - 1; i++)
                {
                    checksum += digits[i] * weights[i];
                }

                return (checksum % 11 % 10).Equals(digits[digits.Length - 1]);
            }

        }

        public void ComarchOptimaImportOrderStart()
        {
            // Create the source, if it does not already exist.
            if (!EventLog.SourceExists("IntegracjaB2B"))
            {
                //An event log source should not be created and immediately used.
                //There is a latency time to enable the source, it should be created
                //prior to executing the application that uses the source.
                //Execute this sample a second time to use the new source.
                EventLog.CreateEventSource("IntegracjaB2B", "IntegracjaB2Blog");

            }


       eventLog = new EventLog();
            eventLog.Source = "IntegracjaB2B";

            this.comarchOptimaImportOrderSTA();
        }

        public string getOrdersGate()
        {
           if (getDevMode())
           return Properties.Settings.Default.ordersGateDev;
           else
           return Properties.Settings.Default.ordersGate;
           
        }

        public string getApiUrl()
        {
            if (getDevMode())
                return Properties.Settings.Default.apiUrlDev;
            else
                return Properties.Settings.Default.apiUrl;

        }

        public string getapiKey()
        {
            if (getDevMode())
                return Properties.Settings.Default.apiKeyDev;
            else
                return Properties.Settings.Default.apiKeyDev;
        }

        public bool getDevMode()
        {
            if (Properties.Settings.Default.devMode)
                return true;
            else
                return false;
        }

    }
}
