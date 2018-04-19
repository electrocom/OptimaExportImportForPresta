using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using CDNTwrb1;
using CDNBase;
using CDNHlmn;
using CDNHeal;

namespace magentoSynchroService.API
{
    class comarchOptimaImportOrder
    {
        static IApplication Application = null;
        static ILogin Login = null;

        static protected bool LogowanieAutomatyczne()
        {
            Application = new CDNBase.Application();
            string Operator = ConfigurationManager.AppSettings["erpLogin"];
            string Haslo = ConfigurationManager.AppSettings["erpPasswd"];
            string Firma = ConfigurationManager.AppSettings["erpName"];
            Environment.CurrentDirectory = ConfigurationManager.AppSettings["erpSrc"];
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
            try
            {
                string orderId = "";
                WebClient client = new WebClient();
                string magentoResponse = "";
                try
                {
                    NameValueCollection postData = new NameValueCollection() { { "action", "getXmlOrders" } };
                    magentoResponse = Encoding.UTF8.GetString(client.UploadValues(ConfigurationManager.AppSettings["magentoGate"], postData));
                }
                catch (Exception exMagento)
                {
                    eventLog.WriteEntry("Error 1 on order import:" + Environment.NewLine + exMagento.ToString(), EventLogEntryType.Error, 0);
                }
                XmlDocument ordersXML = new XmlDocument();
                ordersXML.LoadXml(magentoResponse);
                XmlElement ordersXMLroot = ordersXML.DocumentElement;
                if (ordersXMLroot.ChildNodes.Count > 0)
                {
                    foreach (XmlNode orderXML in ordersXMLroot.ChildNodes)
                    {
                        bool error = true;
                        try
                        {

                            Dictionary<string, List<XmlNode>> splitedOrder = new Dictionary<string, List<XmlNode>>();
                            foreach (XmlNode orderXmlElement in orderXML["Items"])
                            {
                                using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["erpConnString"]))
                                {
                                    conn.Open();
                                    SqlCommand getKntId = new SqlCommand("select twr_twrid, twr_kod, isnull(Mag_MagId, 7) from cdn.Towary left outer join cdn.TwrAtrybuty on Twr_TwrId = TwA_TwrId and TwA_DeAId = 34 left outer join cdn.Magazyny on TwA_WartoscTxt like Mag_Symbol where twr_twrid = " + orderXmlElement.Attributes["ErpId"].InnerText, conn);
                                    SqlDataReader reader = getKntId.ExecuteReader();
                                    while (reader.Read())
                                    {
                                        if (!splitedOrder.ContainsKey(reader[2].ToString()))
                                            splitedOrder.Add(reader[2].ToString(), new List<XmlNode>());
                                        splitedOrder[reader[2].ToString()].Add(orderXmlElement);
                                    }
                                    reader.Close();
                                    reader.Dispose();
                                    conn.Close();
                                }
                            }
                            orderId = orderXML["Header"]["OrderId"].InnerText;
                            foreach (KeyValuePair<string, List<XmlNode>> orderContent in splitedOrder)
                            {
                                LogowanieAutomatyczne();
                                AdoSession Sesja = Login.CreateSession();
                                try
                                {
                                    XmlNode xmlBilling = orderXML.SelectSingleNode("Address[@Type='Billing']");
                                    XmlNode xmlShipping = orderXML.SelectSingleNode("Address[@Type='Shipping']");
                                    DokumentHaMag dok = Sesja.CreateObject("CDN.DokumentyHaMag").AddNew();
                                    dok.Rodzaj = 308000;
                                    dok.TypDokumentu = 308;
                                    var rNumerator = dok.Numerator;
                                    try
                                    {
                                        DefinicjaDokumentu dokDef = Sesja.CreateObject("CDN.DefinicjeDokumentow").Item("DDf_Symbol='RO'");
                                        rNumerator.DefinicjaDokumentu = dokDef;
                                    }
                                    catch (Exception exDokDef)
                                    {
                                        eventLog.WriteEntry("Error 2 on order import:" + Environment.NewLine + exDokDef.ToString(), EventLogEntryType.Error, 0);
                                    }
                                    Kontrahent knt;
                                    if (orderXML["ClientData"] != null)
                                        if (orderXML["ClientData"]["ErpId"] != null)
                                            knt = Sesja.CreateObject("CDN.Kontrahenci").Item("Knt_kntid=" + orderXML["ClientData"]["ErpId"].InnerText);
                                        else
                                            knt = Sesja.CreateObject("CDN.Kontrahenci").Item("Knt_Kod='!NIEOKREŚLONY!'");
                                    else
                                        knt = Sesja.CreateObject("CDN.Kontrahenci").Item("Knt_Kod='!NIEOKREŚLONY!'");
                                    dok.Podmiot = knt;
                                    dok.WalutaSymbol = orderXML["Header"]["Currency"].InnerText;
                                    dok.OdbEmail = xmlShipping["Email"].InnerText;
                                    dok.OdbTelefon = xmlShipping["Phone"].InnerText;
                                    dok.OdbNazwa1 = xmlShipping["Company"].InnerText;
                                    dok.OdbNazwa2 = xmlShipping["FirstName"].InnerText + " " + xmlShipping["LastName"].InnerText;
                                    dok.OdbAdres.Ulica = xmlShipping["Street1"].InnerText;
                                    dok.OdbAdres.NrDomu = xmlShipping["Street2"].InnerText;
                                    dok.OdbAdres.NrLokalu = xmlShipping["Street3"].InnerText;
                                    dok.OdbAdres.Miasto = xmlShipping["City"].InnerText;
                                    dok.OdbAdres.KodPocztowy = xmlShipping["Zip"].InnerText;
                                    dok.OdbAdres.Kraj = xmlShipping["Country"].InnerText;
                                    dok.OdbAdres.Wojewodztwo = xmlShipping["Region"].InnerText;
                                    DokAtrybut dostawa = dok.Atrybuty.AddNew();
                                    dostawa.Kod = "METODADOSTAWY";
                                    dostawa.Wartosc = orderXML["Header"]["DeliveryMethod"].InnerText;
                                    DokAtrybut platnosc = dok.Atrybuty.AddNew();
                                    platnosc.Kod = "METODAPLATNOSCI";
                                    platnosc.Wartosc = orderXML["Header"]["PaymentMethod"].InnerText;
                                    dok.NumerObcy = orderXML["Header"]["OrderId"].InnerText;
                                    dok.MagazynZrodlowyID = int.Parse(orderContent.Key);
                                    ICollection pozycje = dok.Elementy;
                                    foreach (XmlNode orderItem in orderContent.Value)
                                    {
                                        ElementHaMag pozycja = pozycje.AddNew();
                                        pozycja.TowarID = Convert.ToInt32(orderItem.Attributes["ErpId"].InnerText);
                                        pozycja.CenaT = Convert.ToDecimal(orderItem["PriceN"].InnerText);
                                        pozycja.IloscJM = Convert.ToDouble(orderItem["Quantity"].InnerText);
                                    }
                                    error = false;
                                    Sesja.Save();
                                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["erpConnString"]))
                                    {
                                        conn.Open();
                                        SqlCommand insCmd = new SqlCommand(@"INSERT INTO [dbo].[synchroData] ([sd_action] ,[sd_oTyp] ,[sd_oNumer] ,[sd_changeDT] ,[sd_changedDT]) VALUES (1 ,308 ,@order ,getdate() ,getdate())", conn);
                                        insCmd.Parameters.AddWithValue("@order", dok.ID);
                                        insCmd.ExecuteNonQuery();
                                    }
                                }
                                catch (Exception exMagento)
                                {
                                    error = true;
                                    eventLog.WriteEntry("Error on order import: " + orderXML["Header"]["OrderId"].InnerText + Environment.NewLine + "dzieki tej opcji wystapienie bledu importu zamowienia nie powinno zabijac optimy" + exMagento, EventLogEntryType.Error, 0);
                                }
                                Wylogowanie();

                            }
                        }
                        catch (Exception exMagento)
                        {
                            error = true;
                            eventLog.WriteEntry("Error 3 on order import: " + orderXML["Header"]["OrderId"].InnerText + Environment.NewLine + Environment.NewLine + exMagento.ToString(), EventLogEntryType.Error, 0);
                        }
                        if (error == false)
                            try
                            {
                                NameValueCollection postData = new NameValueCollection() {
                                    { "action", "setOrderConfirm" },
                                    { "incrementID", orderId}
                                };
                                magentoResponse = Encoding.UTF8.GetString(client.UploadValues(ConfigurationManager.AppSettings["magentoGate"], postData));
                                if (magentoResponse.StartsWith("Result=OK"))
                                {
                                    eventLog.WriteEntry("Magento Order " + orderId + " imported.", EventLogEntryType.Information, 11);
                                }
                                eventLog.WriteEntry("Magento Order " + orderId + " imported", EventLogEntryType.Information, 0);
                            }
                            catch (Exception exMagento)
                            {
                                eventLog.WriteEntry("Error on order import: " + orderId + Environment.NewLine + exMagento.ToString(), EventLogEntryType.Error, 0);
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                eventLog.WriteEntry("Error 5 on order import:" + Environment.NewLine + ex.ToString(), EventLogEntryType.Error, 0);
            }
        }

        public comarchOptimaImportOrder(EventLog eventLog)
        {
            this.comarchOptimaImportOrderSTA(eventLog);
        }
    }
}
