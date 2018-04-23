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
using System.Configuration;
using System.Collections.Specialized;
using System.Data.SqlClient;
namespace OptimaExportImportForPresta
{
    class ComarchOptimaImportOrder
    {

        static IApplication Application = null;
        static ILogin Login = null;

        static protected bool LogowanieAutomatyczne()
        {
            Application = new CDNBase.Application();
            string Operator = Properties.Settings.Default.erpLogin;
            string Haslo = Properties.Settings.Default.erpPasswd;
            string Firma = Properties.Settings.Default.erpName;
            Environment.CurrentDirectory = Properties.Settings.Default.erpSrc;
            Application.LockApp(256, 5000, null, null, null, null);
            try
            {
                Login = Application.Login(Operator, Haslo, Firma, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
                return true;
            }
            catch
            {
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
        private void comarchOptimaImportOrderSTA(EventLog eventLog)
        {
            string akronim;

            try
            {
                string orderId = "";
                WebClient client = new WebClient();
                string prestaResponse = "";

                try
                {
                    NameValueCollection postData = new NameValueCollection() { { "action", "getXmlOrders" } };
                    prestaResponse = Encoding.UTF8.GetString(client.UploadValues(Properties.Settings.Default.ordersGate, postData));
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
                            Dictionary<string, List<XmlNode>> splitedOrder = new Dictionary<string, List<XmlNode>>();
                            orderId = orderXML["id"].InnerText;
                            XmlNode tmpNode = orderXML["associations"];
                            foreach (XmlNode orderXmlElement in tmpNode["order_rows"])
                            {
                                string TwrTwrId = orderXmlElement["TwrTwrId"].InnerText;
                                SqlConnection conn = new SqlConnection();
                                conn.ConnectionString =
                                                        "Data Source=" + Properties.Settings.Default.serverName + ";" +
                                                        "Initial Catalog=" + Properties.Settings.Default.dataBaseName + ";" +
                                                        "User id=" + Properties.Settings.Default.userName + ";" +
                                                        "Password=" + Properties.Settings.Default.secret + ";";
                                conn.Open();
                                SqlCommand getKntId = new SqlCommand("select twr_twrid, twr_kod, 7 from cdn.Towary where twr_twrid = " + TwrTwrId, conn);


                                SqlDataReader reader = getKntId.ExecuteReader();

                                while (reader.Read())
                                {
                                    if (!splitedOrder.ContainsKey(reader[0].ToString()))
                                        splitedOrder.Add(reader[0].ToString(), new List<XmlNode>());
                                    splitedOrder[reader[0].ToString()].Add(orderXmlElement);
                                }

                                reader.Close();
                                reader.Dispose();

                                conn.Close();


                            }


                            foreach (KeyValuePair<string, List<XmlNode>> orderContent in splitedOrder)
                            {
                                LogowanieAutomatyczne();
                                AdoSession Sesja = Login.CreateSession();


                                try {

                                    XmlNode xmlBilling = orderXML.SelectSingleNode("address_invoice")["address"];
                                    XmlNode xmlShipping = orderXML.SelectSingleNode("address_delivery")["address"];
                                    XmlNode xmlCustomer = orderXML.SelectSingleNode("customer");
                                    
                                    try
                                    {
                                       
                                        Kontrahent knt;
                                        XmlNode xmltmp = orderXML["address_invoice"]["address"];


                                        DefAtrybut defAtrybut = Sesja.CreateObject("CDN.DefAtrybuty").Item("DeA_Kod='B2BID'");
                                        string KnA_DeAId = "KnA_DeAId=" + defAtrybut.ID.ToString() + "";

                                        SqlConnection conn = new SqlConnection();
                                        conn.ConnectionString =
                                                                "Data Source=" + Properties.Settings.Default.serverName + ";" +
                                                                "Initial Catalog=" + Properties.Settings.Default.dataBaseName + ";" +
                                                                "User id=" + Properties.Settings.Default.userName + ";" +
                                                                "Password=" + Properties.Settings.Default.secret + ";";
                                        conn.Open();


                                        SqlCommand idKnt = new SqlCommand("SELECT    [KnA_PodmiotId]  ,[KnA_WartoscTxt]  FROM [CDN_Tomax].[CDN].[KntAtrybuty] where [KnA_DeAId] = '" + defAtrybut.ID.ToString() + "' AND [KnA_WartoscTxt] = '"+ xmlBilling["id"].InnerText+"'" , conn);


                                        SqlDataReader reader = idKnt.ExecuteReader();
                                        int podmiotId = 0;

                                        akronim = "B2B_";
                                        if (xmlBilling["company"].InnerText.Length > 0)
                                            akronim += xmlBilling["company"].InnerText.Replace(" ", "");
                                        akronim += xmlBilling["lastname"].InnerText + "_" + xmlBilling["id"].InnerText;

                                        if (reader.Read())
                                        {

                                            Int32.TryParse(reader["KnA_PodmiotId"].ToString(), out podmiotId);
                                            knt = Sesja.CreateObject("CDN.Kontrahenci").Item("Knt_kntid=" + podmiotId);
                                        }

                                        else
                                        { podmiotId = 0;

                                            try
                                            {
                                                knt = Sesja.CreateObject("CDN.Kontrahenci").Item("Knt_Kod='" + akronim + "'");
                                            }catch (Exception ex)
                                            {
                                                knt = null;
                                            }

                                            if (knt==null) {

                                                knt = Sesja.CreateObject("CDN.Kontrahenci").AddNew();

                                                knt.Akronim = akronim;
                                                knt.Rodzaj_Odbiorca = 1;
                                                knt.Rodzaj_Dostawca = 0;
                                                knt.Nazwa1 = xmlBilling["company"].InnerText;
                                                knt.Nazwa2 = xmlBilling["firstname"].InnerText + " " + xmlBilling["lastname"].InnerText;
                                                knt.Adres.Ulica = xmlBilling["address1"].InnerText;
                                                knt.Adres.NrDomu = xmlBilling["address2"].InnerText;
                                                knt.Adres.Miasto = xmlBilling["city"].InnerText;
                                                knt.Adres.KodPocztowy = xmlBilling["postcode"].InnerText;
                                                knt.Adres.Kraj = "Polska";

                                                if (xmlCustomer["email"].InnerText.Length > 2)
                                                    knt.Email = xmlCustomer["email"].InnerText;

                                                if (xmlBilling["phone"].InnerText.Length > 5)
                                                    knt.Telefon = xmlBilling["phone"].InnerText;

                                                if (xmlBilling["company"].InnerText.Length > 0)

                                                    knt.Nazwa1 = xmlBilling["company"].InnerText;

                                                if (xmlBilling["address1"].InnerText.Length > 0)
                                                {
                                                    knt.Adres.Ulica = xmlBilling["address1"].InnerText;

                                                }








                                                knt.Adres.Kraj = "Polska";

                                                knt.Nip = xmlBilling["vat_number"].InnerText;




                                                if (xmlBilling["postcode"].InnerText.Length > 0)
                                                    knt.Adres.KodPocztowy = xmlBilling["postcode"].InnerText;
                                                if (xmlBilling["city"].InnerText.Length > 0)
                                                    knt.Adres.Miasto = xmlBilling["city"].InnerText;


                                                // DokAtrybut dostawa = dok.Atrybuty.AddNew();
                                                // dostawa.Kod = "METODADOSTAWY";
                                                // dostawa.Wartosc = orderXML["Header"]["DeliveryMethod"].InnerText;
                                                //DokAtrybut platnosc = dok.Atrybuty.AddNew();

                                                KntAtrybut b2bId = knt.Atrybuty.AddNew();


                                                b2bId.DefAtrybut = defAtrybut;
                                                b2bId.ROSaveMode = 1;
                                                b2bId.Wartosc = xmlBilling["id"].InnerText;
                                                Sesja.Save();

                                                podmiotId = knt.ID;


                                            }
                                            

                                        }




                                        reader.Close();
                                        reader.Dispose();

                                        conn.Close();











                                        /*

                                     if (xmlBilling["vat_number"].InnerText.Length >= 10) {
                                         string Knt_nip = "Knt_Nip='" + xmlBilling["vat_number"].InnerText + "'";
                                         knt = Sesja.CreateObject("CDN.Kontrahenci").Item(Knt_nip);
                                     }
                                     else
                                     {
                                         knt = Sesja.CreateObject("CDN.Kontrahenci").Item("Knt_Kod='!NIEOKREŚLONY!'");
                                     }
                                 */









                                     

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
                                     //   dok.Odbiorca =
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
                                        dostawa.Wartosc = "TEST";
                                        //DokAtrybut platnosc = dok.Atrybuty.AddNew();
                                        //platnosc.Kod = "METODAPLATNOSCI";
                                        // platnosc.Wartosc = orderXML["Header"]["PaymentMethod"].InnerText;
                                        dok.NumerObcy = orderId;
                                        // dok.MagazynZrodlowyID = int.Parse(orderContent.Key);
                                        ICollection pozycje = dok.Elementy;
                                        foreach (XmlNode orderItem in orderContent.Value)
                                        {
                                            ElementHaMag pozycja = pozycje.AddNew();
                                            pozycja.TowarID = Convert.ToInt32(orderItem["TwrTwrId"].InnerText);
                                            var product_price = orderItem["product_price"].InnerText.Replace(".", ",");
                                            pozycja.CenaT = Convert.ToDecimal(product_price);
                                            pozycja.IloscJM = Convert.ToDouble(orderItem["product_quantity"].InnerText);
                                        }

                                        error = false;
                                        Sesja.Save();
                                    }
                                    catch (Exception exDokDef)
                                    {
                                        eventLog.WriteEntry("Error 2 on order import:" + Environment.NewLine + exDokDef.ToString(), EventLogEntryType.Error, 0);
                                    }
                                }
                                catch (Exception exPresta)
                                {
                                    error = true;
                                    //  eventLog.WriteEntry("Error on order import: " + orderXML["Header"]["OrderId"].InnerText + Environment.NewLine + "dzieki tej opcji wystapienie bledu importu zamowienia nie powinno zabijac optimy" + exMagento, EventLogEntryType.Error, 0);
                                    Wylogowanie();
                                }
                                Wylogowanie();
                            }

                        }
                        catch (Exception exPresta)
                        {
                            error = true;
                            //   eventLog.WriteEntry("Error 3 on order import: " + orderXML["Header"]["OrderId"].InnerText + Environment.NewLine + Environment.NewLine + exMagento.ToString(), EventLogEntryType.Error, 0);
                        }



                    }
                }


            }
            catch (Exception exception)
            {
                //eventLog.WriteEntry("Error 5 on order import:" + Environment.NewLine + ex.ToString(), EventLogEntryType.Error, 0);
            }



        }

        public void ComarchOptimaImportOrderStart(EventLog eventLog)
        {
            this.comarchOptimaImportOrderSTA(eventLog);
        }


    }
}
