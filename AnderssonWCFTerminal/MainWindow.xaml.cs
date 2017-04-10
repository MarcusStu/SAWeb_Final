using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using SystemAndersson.Anet.BI.Settings;
using SystemAndersson.Anet.Common.Contract;
using SystemAndersson.Anet.Common.Utility;
using SystemAndersson.Anet.Terminal.Client.Logic;

namespace AnderssonWCFTerminal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private ClientTimeTransList timeTransList;
        private TerminalSettings settings = new TerminalSettings();
        private int updateInterval;
        private BitmapImage bi = null;
        private TTimeTransAndTimeSvcRec timeTrans;
        private int version;

        private ClientRfidList list;
        private ClientUnproductiveOrderList unproductiveOrderList;


        public bool ViewError;
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


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            list = new ClientRfidList();
            unproductiveOrderList = new ClientUnproductiveOrderList();
            //GetXamlDirectory();
          

            QTSysGlobal.HardInitialize();
            QTSysGlobal.SoftInitialize();
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
           unproductiveOrderList= GetUnproductiveOrders();
            Version = GetVersionFromLicenceKey();
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //updateInterval = QTSysGlobal.TerminalConfig.UpdateInterval;
            //timer.Interval = new TimeSpan(0, 0, updateInterval);
            //timer_Tick(this, null);
            ////timer.Start();
            //bool ViewError = false;

            //QTSysGlobal.LogicAssemblyAPI.Production.GetTerminalSettings(5, out ViewError);
            //if (ViewError)
            //{
            //    view.Dispatcher.Invoke(QTSysGlobal.ErrorDelegate, null);
            //}
            //UcInOutQuestion test = new UcInOutQuestion();

            //test.Visibility = Visibility.Visible;

            //QTSysGlobal.LogicAssemblyAPI.Production.GetRfidList(ref list, 200, out ViewError);
            //if (ViewError)
            //{
            //    view.Dispatcher.Invoke(QTSysGlobal.ErrorDelegate, null);
            //}
            //SvcRecGlobal.RfidList = list;
        }
    }
   
}
