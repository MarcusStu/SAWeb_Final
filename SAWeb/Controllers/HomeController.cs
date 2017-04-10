using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using SystemAndersson.Anet.BI.Settings;
using SystemAndersson.Anet.Common.Contract;
using SystemAndersson.Anet.Common.Utility;
using SystemAndersson.Anet.Terminal.Client.Logic;

namespace SAWeb.Controllers
{
    public class HomeController : Controller
    {
        private ClientTimeTransList timeTransList = new ClientTimeTransList();
        private TerminalSettings settings = new TerminalSettings();
        private int updateInterval;
        private BitmapImage bi = null;
        private TTimeTransAndTimeSvcRec timeTrans;
        private int version;
        private ClientProductionOrderList openOrderList;
        private ClientProductionOrderHeadList orderHeadList;

        private ClientRfidList list;
        private ClientUnproductiveOrderList unproductiveOrderList;
        private ClientProductionOrderOperationList productionorderoperationlist;


        public bool ViewError;
        public bool ProgramStarted;
        private string infoXx;

        public string Info
        {
            get { return infoXx; }
            set
            {
                infoXx = value;
                //OnPropertyChanged(new PropertyChangedEventArgs("Info"));
            }
        }

        public string TerminalName
        {
            get
            {
                if (QTSysGlobal.ConfigRec != null)
                    return QTSysGlobal.ConfigRec.Name;
                else
                    return "";
            }
        }

        public ClientProductionOrderOperationList ProductionOrderOperationList
        {
            get { return productionorderoperationlist; }
        }

        public ClientProductionOrderList OpenOrderList

        {
            get { return openOrderList; }
        }

        private ClientProductionOrderList GetOpenProductionOrders()

        {
            QTSysGlobal.LogicAssemblyAPI.Production.GetProduction(ref openOrderList, 500, TAnetProductionType.Open, SvcRecGlobal.TimeTransHandlerSvcRec.ResourceId, out ViewError);

            return openOrderList;

        }

        public ClientProductionOrderHeadList OrderHeadList
        {
            get { return orderHeadList; }
        }
    



        public ClientProductionOrderHeadList GetOpenProductionOrderHeads()
        {
            orderHeadList = new ClientProductionOrderHeadList();
            QTSysGlobal.LogicAssemblyAPI.Production.GetProductionOrders(ref orderHeadList, 200, TAnetProductionType.Open, out ViewError);
            return orderHeadList;
        }

        public ClientProductionOrderOperationList GetOpenOperationOrderList()
        {
            productionorderoperationlist = new ClientProductionOrderOperationList();
            QTSysGlobal.LogicAssemblyAPI.Production.GetProductionOrderOperation(ref productionorderoperationlist, 200, 10077, out ViewError);
            return productionorderoperationlist;
        }


        private bool GetTerminalConfig()
        {
            bool result = false;
            QTSysGlobal.Log.WriteEntry(LogTypeLevel.DebugInformation, "Enter GetTerminalConfig", "", "", "");
            QTSysGlobal.ConfigRec = QTSysGlobal.LogicAssemblyAPI.Terminal.GetTerminalConfig(QTSysGlobal.TerminalKey, out ViewError);
            if (!ViewError)
            {
                if (QTSysGlobal.ConfigRec != null)
                {
                    try
                    {
                        QTSysGlobal.TerminalConfig = new TReadQTXmlConfig(QTSysGlobal.ConfigRec.Config);
                        QTSysGlobal.TerminalConfig.Read();
                        result = true;
                    }
                    catch
                    {
                        Info = "Fel i terminalens konfiguration (kolumn Config i databasen)";
                    }

                }
                else
                    Info = "Ingen configuration finns för denna terminal (terminalid saknas i service databasen)";
            }
            else
                Info = "Kan ej kommunicera med AnetCloudService [GetTerminalConfig]";
            QTSysGlobal.Log.WriteEntry(LogTypeLevel.DebugInformation, "Exit GetTerminalConfig", "", "", "");
            return result;
        }

        private bool CheckServiceState()
        {
            bool result = false;
            TServiceStatusSvcRec rec = new TServiceStatusSvcRec();
            //QTSysGlobal.Log.WriteEntry(LogTypeLevel.DebugInformation, "Enter CheckServiceState", "", "", "");
            rec = QTSysGlobal.LogicAssemblyAPI.Terminal.GetServiceStatus(out ViewError);
            //if (!ViewError)
            //{
            if (rec.DatabaseConnectionOk)
            {
                result = true;
            }
            else
                Info = "Fel i AnetCloudService, tjänsten kan ej koppla upp mot databasen";
            //}
            //else
            //    Info = "Kan ej kommunicera med AnetCloudService [CheckServiceState]";
            //QTSysGlobal.Log.WriteEntry(LogTypeLevel.DebugInformation, "Exit CheckServiceState", "", "", "");
            return result;
        }

        private bool CheckInitialization()
        {
            bool result = false;
            //QTSysGlobal.Log.WriteEntry(LogTypeLevel.DebugInformation, "Enter CheckInitialization", "", "", "");


            if (QTSysGlobal.PgmInstanceInfo.InitializedOk)
            {
                if (CheckServiceState())
                {
                    if (GetTerminalConfig())
                    {
                        QTSysGlobal.PgmInstanceInfo.ConfigReadOk = true;
                        Info = "Startup ok";
                        result = true;
                    }
                }
            }
            else
                Info = QTSysGlobal.Log.UserInfoMessage;
            //QTSysGlobal.Log.WriteEntry(LogTypeLevel.DebugInformation, "Exit CheckInitialization", "", "", "");
            return result;
        }

        public int Version
        {
            get { return version; }
            set
            {
                version = value;
                //OnPropertyChanged(new PropertyChangedEventArgs("Version"));
            }
        }

        public static LicenceKey ReadLicenceKey(LicenceKey settings, string path, string filename)
        {
            //string filename = "StartUpSettings.xml";
            try
            {
                XmlSerializer x = new XmlSerializer(typeof(LicenceKey));
                StreamReader reader = new StreamReader(path + filename);
                settings = (LicenceKey)x.Deserialize(reader);
                reader.Dispose();
                return settings;
            }
            catch
            {
                return settings;
            }
        }
        public int GetVersionFromLicenceKey()
        {
            int version = 0;
            LicenceKey license = new LicenceKey();

            license = ReadLicenceKey(license, @"C:\ProgramData\SystemAndersson\AnderssonProduktion\", "LicenceKey.xml");

            if (license.Key != Guid.Empty)
            {
                Guid ProgGuid = license.Key;
                switch (ProgGuid.ToString())
                {
                    case "330cea46-b763-437f-9760-6c93a17a5a19": //20
                        {
                            version = 1;
                            break;
                        }
                    case "f96f6d17-f572-40e0-82a0-e7e35300f359": //40
                        {
                            version = 2;
                            break;
                        }
                    case "bdfd5f09-5de7-440e-a7f8-faaf4606550b": //60
                        {
                            version = 3;
                            break;
                        }
                    case "cb391dec-5579-4e7c-9853-f2f595490dc0"://60i
                        {
                            version = 3;
                            break;
                        }
                    case "e324962c-ae9c-4bdd-9651-e204dd9540ca"://20i
                        {
                            version = 1;
                            break;
                        }
                    case "c7b501bb-2966-4550-a7b1-9e1d61f39e46"://40i
                        {
                            version = 2;
                            break;
                        }
                }
            }
            return version;

        }

        private ClientUnproductiveOrderList GetUnproductiveOrders()
        {
            QTSysGlobal.LogicAssemblyAPI.Production.GetUnproductiveOrders(ref unproductiveOrderList, 200, out ViewError);
            if (ViewError)
            {
                //view.Dispatcher.Invoke(QTSysGlobal.ErrorDelegate, null);
                return null;
            }
            return unproductiveOrderList;
        }

        // GET: Home
        public ActionResult Index()
        {
            // Load Terminal Loadup/Startup Method here
            // On Page Load! Or similar..
            list = new ClientRfidList();
            unproductiveOrderList = new ClientUnproductiveOrderList();
            //GetXamlDirectory();

            // Check bool, if program already started
            if (ProgramStarted != true)
            {
                QTSysGlobal.HardInitialize();
                QTSysGlobal.SoftInitialize();
                ProgramStarted = true;
            }

            if (CheckInitialization())
            {
                //mainWindowCommandEnabled = true;
                //systemShutDownCommandEnabled = false;
                Info = "Startar MainWindowView";
                //MainWindow(null);
            }
            else
            {
                //timer.Start();
                //mainWindowCommandEnabled = false;
                //systemShutDownCommandEnabled = true;
            }
            //timer.Start();
            bool ViewError = false;

            QTSysGlobal.LogicAssemblyAPI.Production.GetTerminalSettings(5, out ViewError);
            //if (ViewError)
            //{
            //    view.Dispatcher.Invoke(QTSysGlobal.ErrorDelegate, null);
            //}
            //UcInOutQuestion test = new UcInOutQuestion();


            //GetTerminalConfig();
            //updateInterval = QTSysGlobal.TerminalConfig.UpdateInterval;
            unproductiveOrderList = GetUnproductiveOrders();
            Version = GetVersionFromLicenceKey();

            QTSysGlobal.LogicAssemblyAPI.TimeTrans.GetTerminalUserInfo(ref SvcRecGlobal.TimeTransHandlerSvcRecList, timeTransList, QTSysGlobal.ConfigRec.RowId, out ViewError);

            ViewBag.UserList = timeTransList;
            ViewBag.unproductiveOrderList = unproductiveOrderList;
            return View(timeTransList);
        }

        public ClientTimeTransList TimeTransList
        {
            get { return timeTransList; }
        }

        public ActionResult Main()
        {
            return View();
        }

        public ActionResult Information()
        {
            return View();
        }

        public ActionResult Stamping()
        {

            return View();
        }

        #region Partials för "Information".
        // Information Partial Views - Right Div
        [HttpGet]
        public PartialViewResult Nya()
        {
            return PartialView("~/Views/Home/Partials/Information/_Right_Nya.cshtml");
        }

        [HttpGet]
        public PartialViewResult Lasta()
        {
            return PartialView("~/Views/Home/Partials/Information/_Right_Lasta.cshtml");
        }

        [HttpGet]
        public PartialViewResult Skickat()
        {
            return PartialView("~/Views/Home/Partials/Information/_Right_Skickat.cshtml");
        }

        // Information Partial Views - Left Div
        [HttpGet]
        public PartialViewResult Inkorg()
        {
            return PartialView("~/Views/Home/Partials/Information/_Left_Inkorg.cshtml");
        }

        [HttpGet]
        public PartialViewResult Skicka()
        {
            return PartialView("~/Views/Home/Partials/Information/_Left_Skicka.cshtml");
        }

        [HttpGet]
        public PartialViewResult Tidbank()
        {
            return PartialView("~/Views/Home/Partials/Information/_Left_Tidbank.cshtml");
        }

        [HttpGet]
        public PartialViewResult Senastestamp()
        {
            return PartialView("~/Views/Home/Partials/Information/_Left_Senastestamp.cshtml");
        }

        [HttpGet]
        public PartialViewResult Tidrapport()
        {
            return PartialView("~/Views/Home/Partials/Information/_Left_Tidrapport.cshtml");
        }

        [HttpGet]
        public PartialViewResult Skicka_Administration()
        {
            return PartialView("~/Views/Home/Partials/Information/_Right_Administration.cshtml");
        }

        [HttpGet]
        public PartialViewResult Skicka_Produktion()
        {
            return PartialView("~/Views/Home/Partials/Information/_Right_Produktion.cshtml");
        }
        #endregion

        #region Partials för "Stamping".
        //Partials för 'Inne'.
        //Skriv in information till SQL i 'ActionResult'. 'PartialViewResult' är enbart för att kalla fram sidan.
        public PartialViewResult InneInfoPartial()
        {
            return PartialView("~/Views/Home/Partials/Stamping/Inne/_InfoPartial.cshtml");
        }

        public ActionResult InneInfo()
        {
            return PartialView();
        }

        public PartialViewResult InneDokumentPartial()
        {
            return PartialView("~/Views/Home/Partials/Stamping/Inne/_DokumentPartial.cshtml");
        }

        public ActionResult InneDokument()
        {
            return PartialView();
        }

        public PartialViewResult InneKommentarPartial()
        {
            return PartialView("~/Views/Home/Partials/Stamping/Inne/_KommentarPartial.cshtml");
        }

        public ActionResult InneKommentar()
        {
            return PartialView();
        }

        public PartialViewResult InneArtiklarPartial()
        {
            return PartialView("~/Views/Home/Partials/Stamping/Inne/_ArtiklarPartial.cshtml");
        }

        public ActionResult InneArtiklar()
        {
            return PartialView();
        }

        public PartialViewResult InneAvvikelsePartial()
        {
            return PartialView("~/Views/Home/Partials/Stamping/Inne/_AvvikelsePartial.cshtml");
        }

        public ActionResult InneAvvikelse()
        {
            return PartialView();
        }

        //Partials för "Ute".
        public PartialViewResult UteInfoPartial()
        {
            return PartialView("~/Views/Home/Partials/Stamping/Ute/_InfoPartial.cshtml");
        }

        public ActionResult UteInfo()
        {
            return PartialView();
        }

        public PartialViewResult UteOperationerPartial()
        {
            return PartialView("~/Views/Home/Partials/Stamping/Ute/_OperationerPartial.cshtml");
        }

        public ActionResult UteOperationer()
        {

            // Load Terminal Loadup/Startup Method here
            // On Page Load! Or similar..
            list = new ClientRfidList();
            unproductiveOrderList = new ClientUnproductiveOrderList();
            //GetXamlDirectory();

            // Check bool, if program already started
            if (ProgramStarted != true)
            {
                QTSysGlobal.HardInitialize();
                QTSysGlobal.SoftInitialize();
                ProgramStarted = true;
            }

            if (CheckInitialization())
            {
                //mainWindowCommandEnabled = true;
                //systemShutDownCommandEnabled = false;
                Info = "Startar MainWindowView";
                //MainWindow(null);
            }
            else
            {
                //timer.Start();
                //mainWindowCommandEnabled = false;
                //systemShutDownCommandEnabled = true;
            }
            //timer.Start();
            bool ViewError = false;

            QTSysGlobal.LogicAssemblyAPI.Production.GetTerminalSettings(5, out ViewError);

            //GetTerminalConfig(); 
            //updateInterval = QTSysGlobal.TerminalConfig.UpdateInterval;
            orderHeadList = GetOpenProductionOrderHeads();
            productionorderoperationlist = GetOpenOperationOrderList();
            Version = GetVersionFromLicenceKey();

            var loopUteOperationerOrderHead = orderHeadList;
            var loopUteOperationerOrderList = productionorderoperationlist;
            //var JsonToReturn = new[] { FirstThing = loopUteOperationerOrderHead, SecondThing = loopUteOperationerOrderList };
            return Json(new { loopUteOperationerOrderHead, loopUteOperationerOrderList }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult UteTillverkningsorderPartial()
        {
            return PartialView("~/Views/Home/Partials/Stamping/Ute/_TillverkningsorderPartial.cshtml");
        }

        public ActionResult UteTillverkningsorder()
        {
            // Load Terminal Loadup/Startup Method here
            // On Page Load! Or similar..
            list = new ClientRfidList();
            unproductiveOrderList = new ClientUnproductiveOrderList();
            //GetXamlDirectory();

            // Check bool, if program already started
            if (ProgramStarted != true)
            {
                QTSysGlobal.HardInitialize();
                QTSysGlobal.SoftInitialize();
                ProgramStarted = true;
            }
            //QTSysGlobal.HardInitialize();
            //QTSysGlobal.SoftInitialize();
            if (CheckInitialization())
            {
                //mainWindowCommandEnabled = true;
                //systemShutDownCommandEnabled = false;
                Info = "Startar MainWindowView";
                //MainWindow(null);
            }
            else
            {
                //timer.Start();
                //mainWindowCommandEnabled = false;
                //systemShutDownCommandEnabled = true;
            }
            //timer.Start();
            bool ViewError = false;

            QTSysGlobal.LogicAssemblyAPI.Production.GetTerminalSettings(5, out ViewError);

            //GetTerminalConfig(); 
            //updateInterval = QTSysGlobal.TerminalConfig.UpdateInterval;
            orderHeadList = GetOpenProductionOrderHeads();
            productionorderoperationlist = GetOpenOperationOrderList();
            Version = GetVersionFromLicenceKey();

            var loopUteTillverkningsorderOrderHead = orderHeadList;
            var loopUteTillverkningsorderOrderList = productionorderoperationlist;
            //var JsonToReturn = new[] { FirstThing = loopUteOperationerOrderHead, SecondThing = loopUteOperationerOrderList };
            return Json(new { loopUteTillverkningsorderOrderHead, loopUteTillverkningsorderOrderList }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult UteOrderPersonPartial()
        {
            return PartialView("~/Views/Home/Partials/Stamping/Ute/_OrderPersonPartial.cshtml");
        }

        public ActionResult UteOrderPerson()
        {
            // Load Terminal Loadup/Startup Method here
            // On Page Load! Or similar..
            list = new ClientRfidList();
            unproductiveOrderList = new ClientUnproductiveOrderList();
            //GetXamlDirectory();


            // Check bool, if program already started
            if (ProgramStarted != true)
            {
                QTSysGlobal.HardInitialize();
                QTSysGlobal.SoftInitialize();
                ProgramStarted = true;
            }
            if (CheckInitialization())
            {
                //mainWindowCommandEnabled = true;
                //systemShutDownCommandEnabled = false;
                Info = "Startar MainWindowView";
                //MainWindow(null);
            }
            else
            {
                //timer.Start();
                //mainWindowCommandEnabled = false;
                //systemShutDownCommandEnabled = true;
            }
            //timer.Start();
            bool ViewError = false;

            QTSysGlobal.LogicAssemblyAPI.Production.GetTerminalSettings(5, out ViewError);

            //GetTerminalConfig(); 
            //updateInterval = QTSysGlobal.TerminalConfig.UpdateInterval;
            orderHeadList = GetOpenProductionOrderHeads();
            productionorderoperationlist = GetOpenOperationOrderList();
            Version = GetVersionFromLicenceKey();

            var loopUteOrderPersonOrderHead = orderHeadList;
            var loopUteOrderPersonOrderList = productionorderoperationlist;
            //var JsonToReturn = new[] { FirstThing = loopUteOperationerOrderHead, SecondThing = loopUteOperationerOrderList };
            return Json(new { loopUteOrderPersonOrderHead, loopUteOrderPersonOrderList }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult UteSnabborderPartial()
        {
            return PartialView("~/Views/Home/Partials/Stamping/Ute/_SnabborderPartial.cshtml");
        }

        public ActionResult UteSnabborder()
        {
            // Load Terminal Loadup/Startup Method here
            // On Page Load! Or similar..
            list = new ClientRfidList();
            unproductiveOrderList = new ClientUnproductiveOrderList();
            //GetXamlDirectory();


            // Check bool, if program already started
            if (ProgramStarted != true)
            {
                QTSysGlobal.HardInitialize();
                QTSysGlobal.SoftInitialize();
                ProgramStarted = true;
            }
            if (CheckInitialization())
            {
                //mainWindowCommandEnabled = true;
                //systemShutDownCommandEnabled = false;
                Info = "Startar MainWindowView";
                //MainWindow(null);
            }
            else
            {
                //timer.Start();
                //mainWindowCommandEnabled = false;
                //systemShutDownCommandEnabled = true;
            }
            //timer.Start();
            bool ViewError = false;

            QTSysGlobal.LogicAssemblyAPI.Production.GetTerminalSettings(5, out ViewError);

            //GetTerminalConfig(); 
            //updateInterval = QTSysGlobal.TerminalConfig.UpdateInterval;
            orderHeadList = GetOpenProductionOrderHeads();
            productionorderoperationlist = GetOpenOperationOrderList();
            Version = GetVersionFromLicenceKey();

            var loopUteSnabborderOrderHead = orderHeadList;
            var loopUteSnabborderOrderList = productionorderoperationlist;
            //var JsonToReturn = new[] { FirstThing = loopUteOperationerOrderHead, SecondThing = loopUteOperationerOrderList };
            return Json(new { loopUteSnabborderOrderHead, loopUteSnabborderOrderList }, JsonRequestBehavior.AllowGet);

        }

        public PartialViewResult UteImproduktivaPartial()
        {
            return PartialView("~/Views/Home/Partials/Stamping/Ute/_ImproduktivaPartial.cshtml");
        }

        public ActionResult UteImproduktiva()
        {

            // Load Terminal Loadup/Startup Method here
            // On Page Load! Or similar..
            list = new ClientRfidList();
            unproductiveOrderList = new ClientUnproductiveOrderList();
            //GetXamlDirectory();


            // Check bool, if program already started
            if (ProgramStarted != true)
            {
                QTSysGlobal.HardInitialize();
                QTSysGlobal.SoftInitialize();
                ProgramStarted = true;
            }
            if (CheckInitialization())
            {
                //mainWindowCommandEnabled = true;
                //systemShutDownCommandEnabled = false;
                Info = "Startar MainWindowView";
                //MainWindow(null);
            }
            else
            {
                //timer.Start();
                //mainWindowCommandEnabled = false;
                //systemShutDownCommandEnabled = true;
            }
            //timer.Start();
            bool ViewError = false;

            QTSysGlobal.LogicAssemblyAPI.Production.GetTerminalSettings(5, out ViewError);
            //if (ViewError)
            //{
            //    view.Dispatcher.Invoke(QTSysGlobal.ErrorDelegate, null);
            //}
            //UcInOutQuestion test = new UcInOutQuestion();


            //GetTerminalConfig();
            //updateInterval = QTSysGlobal.TerminalConfig.UpdateInterval;
            unproductiveOrderList = GetUnproductiveOrders();
            Version = GetVersionFromLicenceKey();

            var loopUteImproduktivaOrderList = unproductiveOrderList;
            return Json(new { loopUteImproduktivaOrderList }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult UteInstruktionerPartial()
        {
            return PartialView("~/Views/Home/Partials/Stamping/Ute/_InstruktionerPartial.cshtml");
        }

        public ActionResult UteInstruktioner()
        {
            return PartialView();
        }

        public PartialViewResult UteDokumentPartial()
        {
            return PartialView("~/Views/Home/Partials/Stamping/Ute/_DokumentPartial.cshtml");
        }

        public ActionResult UteDokument()
        {
            return PartialView();
        }

        //Partials för vänster-sidan.
        public PartialViewResult JobbPartial()
        {
            return PartialView("~/Views/Home/Partials/Stamping/_JobbPartial.cshtml");
        }

        public ActionResult Jobb()
        {
            return PartialView();
        }

        public PartialViewResult MaterialPartial()
        {
            return PartialView("~/Views/Home/Partials/Stamping/_MaterialPartial.cshtml");
        }

        public ActionResult Material()
        {
            // Load Terminal Loadup/Startup Method here
            // On Page Load! Or similar..
            list = new ClientRfidList();
            unproductiveOrderList = new ClientUnproductiveOrderList();
            //GetXamlDirectory();


            // Check bool, if program already started
            if (ProgramStarted != true)
            {
                QTSysGlobal.HardInitialize();
                QTSysGlobal.SoftInitialize();
                ProgramStarted = true;
            }
            if (CheckInitialization())
            {
                //mainWindowCommandEnabled = true;
                //systemShutDownCommandEnabled = false;
                Info = "Startar MainWindowView";
                //MainWindow(null);
            }
            else
            {
                //timer.Start();
                //mainWindowCommandEnabled = false;
                //systemShutDownCommandEnabled = true;
            }
            //timer.Start();
            bool ViewError = false;

            QTSysGlobal.LogicAssemblyAPI.Production.GetTerminalSettings(5, out ViewError);

            //GetTerminalConfig(); 
            //updateInterval = QTSysGlobal.TerminalConfig.UpdateInterval;
            orderHeadList = GetOpenProductionOrderHeads();
            productionorderoperationlist = GetOpenOperationOrderList();
            Version = GetVersionFromLicenceKey();

            var loopMaterialOrderHead = orderHeadList;
            var loopMaterialOrderList = productionorderoperationlist;
            //var JsonToReturn = new[] { FirstThing = loopUteOperationerOrderHead, SecondThing = loopUteOperationerOrderList };
            return Json(new { loopMaterialOrderHead, loopMaterialOrderList }, JsonRequestBehavior.AllowGet);

        }
        #endregion
    }
}