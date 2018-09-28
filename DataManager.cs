using System;
using System.Collections;
using OleDBDataManager;
using System.Data;
using System.Collections.Specialized;
using System.Configuration;
using System.Threading;
using LogDefault;


namespace eProcBackUp
{
    class DataManager
    {
        #region Class Variables
        private string connectStr = "";
        private LogManager lm = LogManager.GetInstance();
        private bool trace = false;
        private bool debug = false;
        private string sql = "";
        private string prefix = "";
        private string dateEntered = "";
        private DataSet dsResults = new DataSet();
        private ArrayList results = new ArrayList();
        private static DataManager dsm = null;
        private NameValueCollection ConfigData = null;
        protected ODMDataFactory ODMDataSetFactory = null;
        private const int STATUS_ID = 4;
        #region Parameters
        public string ConnectStr
        {
            set { connectStr = value; }
        }
        public bool Trace
        {
            set { trace = value; }
        }
        public bool Debug
        {
            set { debug = value; }
        }
        public DataSet DsResults
        {
            get { return dsResults; }
        }
        public ArrayList Results
        {
            get { return results; }
        }
        public string Prefix
        {
            set { prefix = value; }
        }
        public string DateEntered
        {
            set { dateEntered = value; }
        }       
        #endregion
        #endregion

        private DataManager()
        {
            if (trace) { lm.Write("TRACE:  DataSetManager/InitDataSetManager"); }
            lm.Debug = debug;
            ODMDataSetFactory = new ODMDataFactory();
            ConfigData = (NameValueCollection)ConfigurationSettings.GetConfig("appSettings");
            connectStr = ConfigData.Get("cnct_eProc");
        }

        public static DataManager GetInstance()
        {
            if (dsm == null)
            {
                CreateInstance();
            }
            return dsm;
        }

        private static void CreateInstance()
        {
            Mutex configMutex = new Mutex();
            configMutex.WaitOne();
            dsm = new DataManager();
            configMutex.ReleaseMutex();
        }

        public void GetControlList()
        {
            BuildSQL("controls");
            GetResults("controls");
        }
        public void GetDropList()
        {
            BuildSQL("dropList");
            GetResults("dropList");
        }

        private void GetResults(string function)
        {
            if (trace) { lm.Write("TRACE:  DataSetManager/GetResults"); }
            ODMRequest Request = new ODMRequest();
            Request.ConnectString = connectStr;
            Request.CommandType = CommandType.Text;
            Request.Command = "Execute ('" + sql + "')";

            if (trace)
                lm.Write("DataSetManager/GetResults:  " + Request.Command);
            try
            {
                if(function == "dropList")
                    results = ODMDataSetFactory.ExecuteDataReader(ref Request);
                else if(function == "controls")
                    dsResults = ODMDataSetFactory.ExecuteDataSetBuild(ref Request);
            }
            catch (Exception ex)
            {
                lm.Write("DataSetManager/GetResults:  " + ex.Message);
            }
        }

        private void BuildSQL(string task)
        {
            switch (task)
            {
                case "dropList":
                    sql = "select DISTINCT EntryPrefix " +
                          "from tblEntries " +
                          "where EntryStatusID = 4 " +
                          "AND DateEntered >  ''" + dateEntered + "'' ";
                    break;
                case "controls":
                    sql = "SELECT  EntryID,EntryNumber,SectionID,SectionOrder,Controls " +
                          "FROM[eProcessing].[dbo].tblEntries " +
                          "WHERE EntryStatusID = " + STATUS_ID + " " +
                          "AND DateEntered > ''" + dateEntered + "'' " +
                          "AND EntryPrefix = ''" + prefix + "'' " +
                          "order by EntryNumber, SectionID ";
                    break;
            }
             
        }

    }
}
