using System;
using System.Collections.Specialized;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Collections;
using System.Windows.Forms;
using DocumentFormat.OpenXml.Spreadsheet;
using SpreadsheetLight;
using LogDefault;

namespace eProcBackUp
{
    public partial class Form1 : Form
    {
        #region Class Variables
        private NameValueCollection ConfigData = (NameValueCollection)ConfigurationSettings.GetConfig("appSettings");
        DateTimeUtilities dtu = new DateTimeUtilities();
        DataManager dm = DataManager.GetInstance();
        LogManager lm = LogManager.GetInstance();
        SLDocument sld = new SLDocument();
        private ArrayList ColHeaders = new ArrayList();
        private ArrayList RowData = new ArrayList();
        private SLStyle stylHeader;
        private SLStyle stylHeader2;
        private SLStyle stylNormal;
        private string formNmbr = "";
        private string currentFormNmbr = "";
        private string selectedPrefix = "";
        private string currentPrefix = "";
        private string section = "";
        private string sectName = "";
        private string fullName = "";
        private string modName = "";
        private string status = "";
        private string attachmentPath = "";
        private bool hdrsComplete = false;
        private int entryNumber = 0;
        private int currentEntryNum = 0;
        private int sectionID = 0;
        private int currentSectionID = 0;
        private int entryID = 0;        
        private int sectionOrder = 0;
        private int rowNo = 1;
        private string controls = "";
        private bool printFormNumber = false;
        private bool useHeader1 = false;
        #endregion

        public Form1()
        {
            InitializeComponent();
            attachmentPath = ConfigData.Get("attachmentPath");
            lm.LogFile = ConfigData.Get("logFile");
            lm.LogFilePath = ConfigData.Get("logFilePath");
            dm.Debug = Convert.ToBoolean(ConfigData.Get("debug"));
            dm.Trace = Convert.ToBoolean(ConfigData.Get("trace"));
            InitForm();
        }

        private void InitForm()
        {
            stylHeader = sld.CreateStyle();
            stylHeader.Fill.SetPattern(PatternValues.Solid, SLThemeColorIndexValues.Accent4Color,System.Drawing.Color.White);
            stylHeader.Font.FontColor = System.Drawing.Color.White;
            stylHeader2 = sld.CreateStyle();
            stylHeader2.Fill.SetPattern(PatternValues.Solid, SLThemeColorIndexValues.Accent2Color, System.Drawing.Color.White);
            stylHeader2.Font.FontColor = System.Drawing.Color.White;
            stylNormal = sld.CreateStyle();
            stylNormal.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.White, System.Drawing.Color.White);
            stylNormal.Font.FontColor = System.Drawing.Color.Black;
            stylNormal.SetTopBorder(BorderStyleValues.Thin, System.Drawing.Color.LightGray);
            stylNormal.SetBottomBorder(BorderStyleValues.Thin, System.Drawing.Color.LightGray);
            LoadDropList();
        }

        private void LoadDropList()
        {
            dm.DateEntered = DateTime.Now.ToShortDateString();
            dm.GetDropList();
            foreach (string prefix in dm.Results)
            {
                cboxPrefix.Items.Add(prefix);
            }
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            string extension = "";
            useHeader1 = false;
            hdrsComplete = false;
            currentEntryNum = 0;
            selectedPrefix = cboxPrefix.Text.Trim();
            if (selectedPrefix.Length > 0)
            {
                foreach (string prefix in dm.Results)
                {
                    dm.Prefix = selectedPrefix;
                    dm.GetControlList();
                    DSetBreakDown();
                    break;
                }
                sld.AutoFitColumn(1, 50);
                extension = "eProcessing" + dtu.DateTimeCoded() + ".xlsx";
                sld.SaveAs(attachmentPath + extension);
                sld = new SLDocument();
                rowNo = 1;
                hdrsComplete = false;
                Pause();
            }
        }

        private void Pause()
        {
            btnGo.Text = "running...";
            btnGo.BackColor = System.Drawing.Color.LightYellow;
            tmrPause.Start();                       
        }

        private void DSetBreakDown()
        {
            foreach (DataRow drow in dm.DsResults.Tables[0].Rows)
            {// EntryID,EntryNumber,SectionID,SectionOrder,Control
                entryID = Convert.ToInt32(drow[0]);
                entryNumber = Convert.ToInt32(drow[1]);
                sectionID = Convert.ToInt32(drow[2]);
                sectionOrder = Convert.ToInt32(drow[3]);
                controls = drow[4].ToString(); //this field gets parsed below
                sectName = drow[5].ToString();
                fullName = drow[6].ToString();                            //ADDED 10/24
                modName = ChangeNullOrEmpty(drow[7].ToString(), fullName); //ADDED 10/24
                status = drow[8].ToString();                              //ADDED 10/24
                if (sectionID != currentSectionID)
                {
                    currentSectionID = sectionID;
                    hdrsComplete = false;
                    if (rowNo > 1)
                    {
                        rowNo++;
                        rowNo++;
                    }
                }
                ParseControls(controls);                   
            }
        }

        private string ChangeNullOrEmpty(string strToCheck)
        {
            return ChangeNullOrEmpty(strToCheck, "");
        }

        private string ChangeNullOrEmpty(string strToCheck, string strReplacement)
        {
            string rtnString = strToCheck == null ? strReplacement : strToCheck;
            return rtnString.Length == 0 ? strReplacement : rtnString;
        }

        private void ParseControls(string controls)
        {
            // 200|DeptName~OR PROSTHESIS`200|cc~7021`200|Deliver-to~EA209`200|
            //Sect#|label~label text`end of field marker|kabel~ etc.
            int indx = 0;
            int hdrColCount = 0;
            string extension = "";
            string[] firstPass = controls.Split("|".ToCharArray());
            string[] field;
            string fieldOfOne = "";
            section = firstPass[0];
            foreach (string item in firstPass)
            {
                if (item != section)
                {
                    field = item.Split("~".ToCharArray());
                    if (!hdrsComplete)
                    {
                        //useHeader1 = !useHeader1;
                        SaveHeaders(field[0]);
                    }
                    //the last field in the row doesn't have the ~[sedtion ID] terminator so this next line doesn't srip it off.
                    fieldOfOne = field[1].EndsWith(sectionID.ToString()) ? field[1].Remove(field[1].Length - (sectionID.ToString().Length + 1)) : field[1];
                    RowData.Add(fieldOfOne);
                }
            }
            if (!hdrsComplete)
            {
               // useHeader1 = !useHeader1;
                hdrColCount = ColHeaders.Count;
                SetColHeaders();
            }
            try
            {
                SetRowData(hdrColCount);
                RowData.Clear();
            }
            catch (Exception ex)
            {
                lm.Write("form1/ParseControls:  Exception  " + ex.Message);
            }
        }  

        private void SaveHeaders(string colHdr)
        {
            if (!ColHeaders.Contains(colHdr) )
            {
                ColHeaders.Add(colHdr);                
            }
        }
      
        private void SetColHeaders()
        {
            int colNo = 3;      //ADDED 10/24 - changed from colNo = 1 to colNo = 3
            string extension = "";
            try
            {                //set the col headers
                if (entryNumber != currentEntryNum)
                {
                    useHeader1 = !useHeader1;  //change the header color
                }
                foreach (string colName in ColHeaders)
                {
                    sld.SetCellValue(rowNo, ++colNo, colName);
                }
                if (useHeader1)
                    sld.SetRowStyle(rowNo, stylHeader);
                else
                    sld.SetRowStyle(rowNo, stylHeader2);
                hdrsComplete = true;
                ColHeaders.Clear();
            }
            catch (IndexOutOfRangeException ex)
            {
                lm.Write("OutputManager/FormatAttachment:  IOOR Exception  " + ex.Message);
            }
            catch (Exception ex)
            {
                lm.Write("OutputManager/FormatAttachment:  Exception  " + ex.Message);
            }
        }

        private void SetRowData(int colCount)
        {
            int colNo = 0;
            int startCol = 4;
            rowNo++;
            //if (sectionID == 201)
            //    colNo = 0;
            try
            {
                foreach (string item in RowData)
                {                    
                   // if (colNo + 1 > colCount + 1)
                    if(colNo + 1 > colCount + startCol)
                        colNo = 0;
                    if (entryNumber != currentEntryNum)
                    {
                        formNmbr = selectedPrefix + entryNumber.ToString();
                        currentEntryNum = entryNumber;
                        printFormNumber = true;
                    }
                    if (colNo == 0)
                    {
                        sld.SetCellStyle(rowNo, colNo + 1, stylNormal);
                        sld.SetCellValue(rowNo, colNo + 1, sectName);

                        sld.SetCellValue(rowNo - 1, colNo + 2, "Modified By");   //ADDED 10/24
                        sld.SetCellValue(rowNo, colNo + 2, modName);            //ADDED 10/24

                        sld.SetCellValue(rowNo - 1, colNo + 3, "Status");       //ADDED 10/24
                        sld.SetCellValue(rowNo, colNo + 3, status);             //ADDED 10/24

                        sld.SetCellStyle(rowNo - 1, colNo + 1, stylNormal);
                        if (printFormNumber)
                        { //put FormNumber into col1
                            sld.SetCellValue(rowNo - 1, ++colNo, formNmbr);
                            //colNo = 3;                                          //ADDED 10/24
                        }
                        //else colNo = 3;    
                        colNo = 3;
                        if (sld.GetCurrentWorksheetName().Equals("Sheet1"))
                            sld.RenameWorksheet("Sheet1", selectedPrefix);
                        printFormNumber = false;

                        sld.SetCellValue(rowNo, ++colNo, item);
                    }
                    else
                        colNo++;

                    sld.SetCellValue(rowNo, colNo, item);
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                lm.Write("OutputManager/FormatAttachment:  IOOR Exception  " + ex.Message);
            }
            catch (Exception ex)
            {
                lm.Write("OutputManager/FormatAttachment:  Exception  " + ex.Message);
            }
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void lnkOutputPath_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process process = new Process();
            process.StartInfo.FileName = attachmentPath;
            process.Start();
        }

        private void tmrPause_Tick(object sender, EventArgs e)
        {
            btnGo.Text = "Go";
            btnGo.BackColor = System.Drawing.Color.PaleGreen;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Select an Entry Prefix from the drop list" + Environment.NewLine +
                            "and click on the GO button. If the list is" + Environment.NewLine +
                            "empty then there haven't been any eProcessing." + Environment.NewLine +
                            "forms submitted yet." + Environment.NewLine +
                            "Follow the link at the bottom of the form to get to the output file.", 
                            "Using eProcessingBackUp",MessageBoxButtons.OK);
        }
    }
}

//SELECT EntryNumber, SectionOrder, Controls
//                          FROM[eProcessing].[dbo].tblEntries
//WHERE EntryStatusID = 4
//                          AND DateEntered > '9/14/2018'
//                          AND EntryPrefix = 'LABITM'--'IMPDAYOF'
