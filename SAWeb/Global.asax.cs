using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SystemAndersson.Anet.Common.Contract;
using SystemAndersson.Anet.Common.Utility;
using SystemAndersson.Anet.Terminal.Client.Logic;

namespace SAWeb
{

    public delegate void HandleErrorDelegate();

    public static class QTSysGlobal
    {
        public static ProgramInstanceInfo PgmInstanceInfo;
        public static TLogicAssemblyAPI LogicAssemblyAPI;
        public static Logger Log;
        //public static ViewFactoryBase ViewFactory;
        public static HandleErrorDelegate ErrorDelegate;
        public static Guid TerminalKey;
        public static TTerminalConfigSvcRec ConfigRec;
        public static TReadQTXmlConfig TerminalConfig;

        public static void HardInitialize()
        {
            // This method is called only once for the lifetime of the program.
            PgmInstanceInfo = new ProgramInstanceInfo("SystemAndersson", "AnetQT", new Guid("3126F35A-F3CB-44F1-B47B-A9BBAAF64410"));
        }

        public static bool SoftInitialize()
        {
            // This method can be called to reinitialize (restart) the program.
            PgmInstanceInfo.ReInitialize();
            QTSysGlobal.TerminalKey = Guid.Empty;
            QTSysGlobal.Log = new Logger(PgmInstanceInfo.ApplicationName, TLogfileLocation.LocalApplicationData);
            QTSysGlobal.Log.PrefferedLogStorage = LogStorage.LogFile;
            QTSysGlobal.Log.LogError = true;
            QTSysGlobal.Log.LogWarning = true;
            QTSysGlobal.Log.LogInformation = true;
            QTSysGlobal.Log.LogDebugInformation = true;
            QTSysGlobal.Log.WriteEntry(LogTypeLevel.DebugInformation, "Enter SoftInitialize", "", "", "");
            //if (QTSysGlobal.ViewFactory != null)
            //    QTSysGlobal.ViewFactory.CloseAll();
            //QTSysGlobal.ViewFactory = new ViewFactoryBase();

            // Check single instance
            if (!QTSysGlobal.PgmInstanceInfo.IsSingleInstance)
            {
                QTSysGlobal.Log.WriteEntry(LogTypeLevel.Error, "Programmet är redan igång. Endast en instans av programmet kan köras", "", "", "");
                return false;
            }


            // Read key.
            InstanceKey ik = new InstanceKey();
            if (!ik.ReadFile(@"C:\ProgramData\SystemAndersson\AnderssonProduktion\", out TerminalKey))
            {
                if (ik.ErrorCode == FileStreamErrorCode.StandardStreamRead)
                    Log.WriteEntry(LogTypeLevel.Error, "Terminal id fil saknas", "", "", "");
                else
                    Log.WriteEntry(LogTypeLevel.Error, "Fel i terminal id filen", "", "", "");
                return false;
            }
            ik = null;

            // Read config.
            ClientConfig config = new ClientConfig(QTSysGlobal.PgmInstanceInfo.CompanyName, QTSysGlobal.PgmInstanceInfo.ApplicationName, ConfigRegistryHive.LocalMachine);
            if (!config.Read())
            {
                Log.WriteEntry(LogTypeLevel.Error, "Kunde ej läsa konfigurationen", "", "", "");
                return false;
            }

            QTSysGlobal.Log.WriteEntry(LogTypeLevel.DebugInformation, "Exit SoftInitialize (before set logoptions)", "", "", "");
            // Reset Log options depending on configuration.
            QTSysGlobal.Log.PrefferedLogStorage = (LogStorage)config.LogOption.Storage;
            QTSysGlobal.Log.LogError = config.LogOption.Error;
            QTSysGlobal.Log.LogWarning = config.LogOption.Warning;
            QTSysGlobal.Log.LogInformation = config.LogOption.Information;
            QTSysGlobal.Log.LogDebugInformation = config.LogOption.DebugInformation;
            QTSysGlobal.LogicAssemblyAPI = new TLogicAssemblyAPI();
            QTSysGlobal.LogicAssemblyAPI.Initialize(QTSysGlobal.Log, config.HostAddress);
            SvcRecGlobal.CurrTimeTransRec = new ClientTimeTransRec();

            SvcRecGlobal.AdvancedTime = new AdvancedTimeQuestionClass();

            QTSysGlobal.Log.WriteEntry(LogTypeLevel.DebugInformation, "Exit SoftInitialize", "", "", "");
            PgmInstanceInfo.InitializedOk = true;
            return PgmInstanceInfo.InitializedOk;
        }

        public static void CleanUp()
        {
            PgmInstanceInfo = null;
            //QTSysGlobal.ViewFactory.CloseAll();
        }

    }

    public static class SvcRecGlobal
    {

        public static long LoginUser;
        public static bool LoginOk;
        public static bool ReadOutdeliveryOk;
        public static bool ReadTransonOk;
        public static bool ReadFreeOutdeliveryOk;
        public static bool ReadTransOffdoc;
        public static int OutdeliveryPackingSlipIndex;
        public static int FreeOutdeliveryPackingSlipIndex;
        public static int ManualIndeliveryIndex;
        public static int IndeliveryQuantity;
        public static string PackingSlipNo;
        public static bool ReadActManualOutDelivery;
        public static string SearchArtdesc;
        public static string SearchArtNo;
        public static long StaffId;
        public static bool SplitOnQuantity;
        public static bool SplitOnOrderRows;



        //public static bool Rfid = false;
        public static ClientRfidList RfidList;
        public static TimeSpan NewStartTime = new TimeSpan();
        public static ClientDocumentRec copyDoc;


        public static TTimeTransHandlerSvcRec[] TimeTransHandlerSvcRecList; // Used in the main screen for info on all users.
        public static TTimeTransHandlerSvcRec TimeTransHandlerSvcRec;
        public static ClientTimeTransRec CurrTimeTransRec;
        public static AdvancedTimeQuestionClass AdvancedTime;
        public static ClientCustomerRec CustomerRec;
        public static ClientReferenceRec ReferenceRec;
        public static ClientScheduleRec ScheduleRec;

        public static ClientArticleList CurrentArticleList;
        public static ClientDerogationList CurrentDerogationList;
        public static string Currentinstructions;

        public static ClientTimeTransRec TimeTransChange;

        public static bool SideJob = false;
        public static ClientMissedDaysList MissedDays = new ClientMissedDaysList();
    }





    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
