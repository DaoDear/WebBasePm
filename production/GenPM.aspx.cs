using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using NUnit.Framework;

namespace WebBasePM
{
    public partial class ProductionGenerator : System.Web.UI.Page
    {
        protected DatabaseHelper dbHelper;
        bool globalChk = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            dbHelper = new DatabaseHelper();
        }

        // To open folder.
        protected void OpenFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            Thread thread = new Thread(() => folderBrowser.ShowDialog(new Form() { TopMost = true, WindowState = FormWindowState.Maximized }));
            thread.IsBackground = false;
            
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();

            string osPath = folderBrowser.SelectedPath;
            OSInput.Text = osPath;
            thread.Abort();            
        }

        /* Function that generate the os information it need OS Path. 
        *  Use this function as a test.
        */

        [TestCase("D:\\oracle")]
        public void osConfigGen(string osPath)
        {
            // Get the initial value for the project.
            string projectcode = projCodeInput.Text;
            string quarter = quarterInput.Text;
            string osTypeIn = OSType.Text;

            List<environment> environmentList = new List<ProductionGenerator.environment>();
            List<Hardware> hardwareList = new List<ProductionGenerator.Hardware>();
            List<DiskSpace> diskList = new List<ProductionGenerator.DiskSpace>();

            string hostname = "", 
                   ipAddress = "", 
                   oracleOwner_login = "", 
                   oracleOwner_homeDirectory = "", 
                   oracleOwner_shell = "", 
                   oracleGroup_firstGroup = "", 
                   oracleGroup_secondGroup = "",
                   osNode = "1", 
                   osType = osTypeIn, 
                   osInfo = "", 
                   osMemSize = "", 
                   osSwap = "", 
                   osTmp = "", 
                   osJava = "", 
                   osKernel = "",
                   systemModel = "", 
                   machineSerialNumber = "", 
                   processorType = "", 
                   processorImplementationMode = "", 
                   processorVersion = "", 
                   numOfProc = "", 
                   procClockSpeed = "", 
                   cpuType = "", 
                   kernelType = "", 
                   ipaddress = "", 
                   subNetMask = "", 
                   crontab = "";

            string timeStamp = GetTimestamp(DateTime.Now);
            string tempPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory.ToString(), "TEMP", "OS_" + timeStamp);

            //Create temp directory after inserted data to database, it must remove this folder.
            Directory.CreateDirectory(tempPath);

            string[] Files = Directory.GetFiles(osPath);
           
            List<FileInfo> osFilesList = new List<FileInfo>();
                      

            // Upload folder and files retrieve iteration.
            for (int i = 0; i < Files.Length; i++)
            {
                File.Copy(Files[i], Path.Combine(tempPath, Path.GetFileName(Files[i])));
            }
            string[] Files_Init = Directory.GetFiles(tempPath);
            // Add File into list of OS File.
            foreach (string file in Files_Init) {
                osFilesList.Add(new FileInfo(file));
            }
          
            //List file due to requirement .
            for (int i = 0; i < osFilesList.Count(); i++)
            {
                string fileName = osFilesList[i].Name;
                string filePath = osFilesList[i].FullName;

                // Trap user environment file.
                if (fileName.ToLower().Equals("user_environments.txt"))
                {
                    TextReader reader = File.OpenText(filePath);
                    string line;
                    string[] strtmps = null;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Trim().Length > 0)
                        {
                            strtmps = line.Trim().Split(new char[] { '=' }, 2);
                            environmentList.Add(new environment(strtmps[0].ToString(), strtmps[1].ToString()));
                        }
                    }
                }
                //Trap user id.
                else if (fileName.ToLower().Equals("user_id.txt"))
                {
                    TextReader reader = File.OpenText(filePath);
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] strtmps2 = line.Trim().Split(new char[] { '(', ')' });
                        string[] strtmps = new string[2];
                        strtmps[0] = "";
                        strtmps[1] = "";
                        if (strtmps2.Length > 3)
                        {
                            strtmps[0] = strtmps2[3];
                        }
                        if (strtmps2.Length > 5)
                        {
                            strtmps[1] = strtmps2[5];
                            for (int j = 7; j < strtmps2.Length; j += 2)
                            {
                                strtmps[1] += "," + strtmps2[j];
                                oracleGroup_firstGroup = strtmps[1].ToString();
                                oracleGroup_secondGroup = strtmps2[j].ToString();
                            }
                        }
                    }
                }
                // Trap crontab file.
                else if (fileName.ToLower().Equals("crontab.txt"))
                {
                    TextReader reader = File.OpenText(filePath);
                    string line;
                    string[] strtmps = null;
                    if ((line = reader.ReadToEnd()) != null && line.Trim().Length > 0)
                    {
                        strtmps = line.Trim().Split(new char[] { }, 1);
                    }
                    else
                    {
                        strtmps = new string[] { "-" };
                    }
                    crontab = strtmps[0].ToString();
                }
                // Trap hostname file.
                else if (fileName.ToLower().Equals("hostname.txt"))
                {
                    TextReader reader = File.OpenText(filePath);
                    string line;
                    object[] objtmps = null;
                    if ((line = reader.ReadLine()) != null)
                    {
                        objtmps = new object[2];
                        objtmps[0] = "Hostname";
                        objtmps[1] = line.Trim();
                        hostname = objtmps[1].ToString();
                    }

                }
                //Trap ip address.
                else if (fileName.ToLower().Equals("ip.txt"))
                {
                    TextReader reader = File.OpenText(filePath);
                    string line;
                    bool notFirst = false;
                    string ipTmp = null;
                    object[] objtmps = null;
                    string[] strtmps = null;
                    char[] deli1 = new char[] { ' ' };
                    while ((line = reader.ReadLine()) != null)
                    {
                        if ((strtmps = line.Trim().Split(deli1)).Length > 1 && strtmps[0].Equals("inet") && !strtmps[1].Equals("127.0.0.1"))
                        {
                            if (notFirst)
                            {
                                ipTmp += "," + strtmps[1];
                            }
                            else
                            {
                                ipTmp = strtmps[1];
                                notFirst = true;
                            }
                        }
                    }
                    objtmps = new object[2];
                    objtmps[0] = "IP Address";
                    objtmps[1] = ipTmp;
                    ipAddress = objtmps[1].ToString();
                }
                // Trap os information.
                else if (fileName.ToLower().Equals("os_info.txt"))
                {
                    TextReader reader = File.OpenText(filePath);
                    string line;
                    object[] objtmps = null;
                    if ((line = reader.ReadLine()) != null)
                    {
                        objtmps = new object[2];
                        objtmps[1] = line.Trim();
                        osInfo = objtmps[1].ToString();
                    }
                }
                // Trap disk information.
                else if (fileName.ToLower().Equals("disk.txt"))
                {
                    TextReader reader = File.OpenText(filePath);
                    string line;
                    object[] objtmps = null;
                    char[] deli1 = new char[] { ' ' };
                    if (reader.ReadLine() != null)
                    {
                        while (((line = reader.ReadLine()) != null) && (line.Trim().Length > 0))
                        {
                            objtmps = line.Split(deli1, 7, StringSplitOptions.RemoveEmptyEntries);
                            diskList.Add(new DiskSpace(objtmps[0].ToString(), objtmps[1].ToString(), objtmps[2].ToString(), objtmps[3].ToString(), objtmps[4].ToString(), objtmps[5].ToString(), objtmps[6].ToString()));
                        }
                    }
                }
                //Trap system information.
                else if (fileName.ToLower().Equals("system_info.txt"))
                {
                    TextReader reader = File.OpenText(filePath);
                    string line;
                    string[] strtmps = null;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Trim().Length > 0)
                        {
                            strtmps = line.Trim().Split(new char[] { ':' }, 2);
                            if (strtmps.Length > 1)
                            {
                                hardwareList.Add(new Hardware(strtmps[0].ToString(), strtmps[1].ToString()));
                            }                          
                        }
                    }
                    //This file is list like the xml file that it is needed to serach using this searchTextEnvironment function.
                    systemModel = searchTextEnvironment(hardwareList, "System Model");
                    machineSerialNumber = searchTextEnvironment(hardwareList, "Machine Serial Number"); 
                    processorType = searchTextEnvironment(hardwareList, "Processor Type");
                    processorImplementationMode = searchTextEnvironment(hardwareList, "Processor Implementation Mode");
                    processorVersion = searchTextEnvironment(hardwareList, "Processor Version");
                    numOfProc = searchTextEnvironment(hardwareList, "Number Of Processors");
                    procClockSpeed = searchTextEnvironment(hardwareList, "Processor Clock Speed");
                    cpuType = searchTextEnvironment(hardwareList, "CPU Type");
                    kernelType = searchTextEnvironment(hardwareList, "Kernel Type");
                    ipaddress = searchTextEnvironment(hardwareList, "IP Address");
                    subNetMask = searchTextEnvironment(hardwareList, "Sub Netmask");
                }
                // Trap swap information.
                else if (fileName.ToLower().Equals("swap.txt"))
                {
                    TextReader reader = File.OpenText(filePath);
                    string line;
                    string[] strtmps = null;
                    char[] deli1 = new char[] { ' ' };
                    object[] objtmps = null;
                    if (reader.ReadLine() != null)
                    {
                        if (((line = reader.ReadLine()) != null) && (line.Trim().Length > 0))
                        {
                            strtmps = line.Split(deli1, 9, StringSplitOptions.RemoveEmptyEntries);
                            if ((strtmps != null) && (strtmps.Length > 3) && (strtmps[3].Length > 0))
                            {
                                objtmps = new object[2];
                                objtmps[1] = "Paging Space Information\n   Total Paging Space : " + strtmps[3];
                                osSwap = objtmps[1].ToString();
                            }
                        }
                    }
                }
                // Trap temp space
                else if (fileName.ToLower().Equals("tmp_space.txt"))
                {
                    TextReader reader = File.OpenText(filePath);
                    string line;
                    string[] strtmps = null;
                    char[] deli1 = new char[] { ' ' };
                    object[] objtmps = null;
                    if (reader.ReadLine() != null)
                    {
                        if (((line = reader.ReadLine()) != null) && (line.Trim().Length > 0))
                        {
                            strtmps = line.Split(deli1, 7, StringSplitOptions.RemoveEmptyEntries);
                            if ((strtmps != null) && (strtmps.Length > 1) && (strtmps[1].Length > 0))
                            {
                                objtmps = new object[2];
                                objtmps[1] = "Tmp size: " + strtmps[1] + " Megabytes";
                                osTmp = objtmps[1].ToString();
                            }
                        }
                    }
                }
                // Trap java version
                else if (fileName.ToLower().Equals("java_version.txt")) {
                    TextReader reader = File.OpenText(filePath);
                    string line;
                    object[] objtmps;
                    if ((line = reader.ReadLine()) != null)
                        {
                            string strtmp = line.Trim();
                            while (((line = reader.ReadLine()) != null) && (line.Trim().Length > 0))
                            {
                                strtmp += "\n" + line.Trim();
                            }
                            objtmps = new object[2];                           
                            objtmps[1] = strtmp;
                            osJava = objtmps[1].ToString();
                        }
                }
            }
            // Search text fro environment information.
            oracleOwner_login = searchTextEnvironment(environmentList, "LOGNAME");
            oracleOwner_homeDirectory = searchTextEnvironment(environmentList, "HOME");
            oracleOwner_shell = searchTextEnvironment(environmentList, "SHELL");
            osMemSize = searchTextEnvironment(hardwareList, "Memory Size");

            // Database insert handle.
            // Check server machine spec.
            List<object> chkList = new List<object>();
            chkList.Add(hostname);
            chkList.Add(ipAddress);
            chkList.Add(oracleOwner_login);
            chkList.Add(oracleOwner_homeDirectory);
            chkList.Add(oracleOwner_shell);
            chkList.Add(oracleGroup_firstGroup);
            chkList.Add(oracleGroup_secondGroup);
           
            dbHelper.InsertChkServerMacSpec(projectcode, quarter, chkList);

            // Check disk space.
            List<object[]> diskSpaceList = new List<object[]>();
            for (int j = 0; j < diskList.Count; j++)
            {
                diskSpaceList.Add(new object[] {"1", diskList[j].getDiskObject()[0].ToString(), diskList[j].getDiskObject()[1].ToString(), diskList[j].getDiskObject()[2].ToString(), diskList[j].getDiskObject()[3].ToString(), diskList[j].getDiskObject()[4].ToString(), diskList[j].getDiskObject()[5].ToString(), diskList[j].getDiskObject()[6].ToString()});
            }
            dbHelper.InsertOsDiskSpace(projectcode, quarter, diskSpaceList);

            // Compare oracle requirement.
            osKernel = "AIXTHREAD_SCOPE=" + searchTextEnvironment(environmentList, "AIXTHREAD_SCOPE");
            List<object> compareList = new List<object>();
            compareList.Add(osNode);
            compareList.Add(osType);
            compareList.Add(osInfo);
            compareList.Add(osMemSize);
            compareList.Add(osSwap);
            compareList.Add(osTmp);
            compareList.Add(osJava);
            compareList.Add(osKernel);
            dbHelper.InsertCompareRequirement(projectcode, quarter, compareList);

            // Hardware Configuration.
            List<object> hardwareConfigList = new List<object>();
            hardwareConfigList.Add(osNode);
            hardwareConfigList.Add(systemModel);
            hardwareConfigList.Add(machineSerialNumber);
            hardwareConfigList.Add(processorType);
            hardwareConfigList.Add(processorImplementationMode);
            hardwareConfigList.Add(processorVersion);
            hardwareConfigList.Add(numOfProc);
            hardwareConfigList.Add(procClockSpeed);
            hardwareConfigList.Add(cpuType);
            hardwareConfigList.Add(kernelType);
            hardwareConfigList.Add(ipaddress);
            hardwareConfigList.Add(subNetMask);
            hardwareConfigList.Add(crontab);
            dbHelper.InsertHardware(projectcode, quarter, hardwareConfigList);

            // Environment configuration.
            List<object[]> environmentConfigList = new List<object[]>();
            for (int j = 0; j < environmentList.Count; j++) {
                environmentConfigList.Add(new object[] { environmentList[j].getHeader(), environmentList[j].getValue() });        
            }
            dbHelper.InsertEnvironment(projectcode, quarter, environmentConfigList);           

            try
            {
                Directory.Delete(tempPath, true);
            }
            catch (Exception ex) {
                Console.Write(ex);
            }
            Assert.True(true);

        }// End of OS Generator.

        // Function that set and get PM Information and insert to database.
        public void pmInfoConfig()
        {
            string projCode = projCodeInput.Text;
            string quarter = quarterInput.Text;
            string customerCompanyFull = custComFull.Text;
            string customerCompanyAbbv = custComAbbv.Text;
            string projectName = projInput.Text;
            string authun = docAut.Text;
            string docReviewed = docReview.Text;
            string databaseNamed = databaseName.Text;
            object[] pminfoObj = new object[] { projCode, customerCompanyFull, customerCompanyAbbv, projectName, quarter, docReviewed,authun, databaseNamed };

            object[] projectCodeObj = dbHelper.GetSingleQueryObject("SELECT * FROM [PM].[dbo].[PmInfo] WHERE [projectCode] = '" + projCode + "';");
           
            if (projectCodeObj != null)
            {
                dbHelper.InsertPMConfiguration(pminfoObj);
            }
            else
            {
                globalChk = true;
            }
            
        }// End of PM configuration
        
        // Function that set and get Database config file and insert to database.
        public void databaseConfigGen(string projectCode, string quarter)
        {
            string timeStamp = GetTimestamp(DateTime.Now);
            string tempPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory.ToString(), "TEMP", "DB_" + timeStamp);
            Directory.CreateDirectory(tempPath);
            string fileName = DBConfigUpload.FileName.ToString();
            string filePath = Path.Combine(tempPath, fileName);
            DBConfigUpload.PostedFile.SaveAs(Path.Combine(tempPath, fileName));

            string backupDatabaseFile = BackupDatabaseFile.FileName.ToString();
            string backupDatabaseFilePath = Path.Combine(tempPath, backupDatabaseFile);
            BackupDatabaseFile.PostedFile.SaveAs(Path.Combine(tempPath, backupDatabaseFile));

            string backupControlFile = BackupControlFile.FileName.ToString();
            string backupControlFilePath = Path.Combine(tempPath, backupControlFile);
            BackupControlFile.PostedFile.SaveAs(Path.Combine(tempPath, backupControlFile));

            string backupArcheiveFile = BackupArchieveFile.FileName.ToString();
            string backupArcheiveFilePath = Path.Combine(tempPath, backupArcheiveFile);
            BackupArchieveFile.PostedFile.SaveAs(Path.Combine(tempPath, backupArcheiveFile));
            string backupDBF, backupCFF, backupALF;

            using (TextReader reader = File.OpenText(backupDatabaseFilePath)) {                
                while ((backupDBF = reader.ReadLine()) != null)
                {
                    String.Concat(backupDBF, backupDBF);
                }
            }

            using (TextReader reader = File.OpenText(backupDatabaseFilePath))
            {
                while ((backupCFF = reader.ReadLine()) != null)
                {
                    String.Concat(backupCFF, backupCFF);
                }
            }

            using (TextReader reader = File.OpenText(backupArcheiveFilePath))
            {
                while ((backupALF = reader.ReadLine()) != null)
                {
                    String.Concat(backupALF, backupALF);
                }
            }
            dbHelper.InsertBackupDatabase(projectCode,quarter, backupDBF, backupCFF, backupALF);

            SetOfTableList tables = null;
            OracleInformation oracleInfo = new OracleInformation();

            using (TextReader logFile = File.OpenText(filePath))
            {
                tables = oracleInfo.readInputLog(logFile);
            }
                tableListWord tableWord;
                string binFolderPath = Server.MapPath("bin");
                string path = binFolderPath + "/Debug/config/4_1.txt";

            using (TextReader inFile = File.OpenText(path))
            {
                tableWord = oracleInfo.readOutputTable(inFile, tables);
                List<object[]> database4_1Obj = new List<object[]>();
                for (int k = 0; k < tableWord.getRowNumber(); k++)
                {
                    if (tableWord.getRow(k)[0].Equals("Temp tablespace size"))
                    {
                        object[] obj1 = (object[])tableWord.getRows()[k];
                        tableListWord obj2 = (tableListWord)obj1[1];
                        List<object[]> database4_1_1Obj = new List<object[]>();
                        for (int z = 0; z < obj2.getRows().Count; z++)
                        {
                            object[] subDetail = (object[])obj2.getRows()[z];
                            database4_1_1Obj.Add(new object[] { subDetail[0].ToString(), subDetail[1].ToString()});
                        }
                        dbHelper.InsertTempTableSize(projectCode, quarter, database4_1_1Obj);
                    }
                    else if (tableWord.getRow(k)[0].Equals("Tablespace size"))
                    {
                        object[] obj1 = (object[])tableWord.getRows()[k];
                        tableListWord obj2 = (tableListWord)obj1[1];
                        List<object[]> database4_1_2Obj = new List<object[]>();
                        for (int z = 0; z < obj2.getRows().Count; z++)
                        {
                            object[] subDetail = (object[])obj2.getRows()[z];
                            database4_1_2Obj.Add(new object[] { subDetail[0].ToString(), subDetail[1].ToString() });
                        }
                        dbHelper.InsertTempTableSize(projectCode, quarter, database4_1_2Obj);
                    }
                    else
                    {
                        database4_1Obj.Add(new object[] { tableWord.getRow(k)[0], tableWord.getRow(k)[1] });
                    }
                }
                dbHelper.InsertDatabaseConfiguration(projectCode, quarter, database4_1Obj);
            }

            path = binFolderPath + "/Debug/config/4_2.txt";
            using (TextReader inFile = File.OpenText(path))
            {
                tableWord = oracleInfo.readOutputTable(inFile, tables);
                List<object[]> database4_2Obj = new List<object[]>();
                for (int k = 0; k < tableWord.getRowNumber(); k++)
                {
                    database4_2Obj.Add(new object[] { tableWord.getRow(k)[0], tableWord.getRow(k)[1] });
                }
                dbHelper.InsertDatabaseParameter(projectCode, quarter, database4_2Obj);
            }
            /*
            path = binFolderPath + "/Debug/config/4_3.txt";
            using (TextReader inFile = File.OpenText(path))
            {
                tableWord = oracleInfo.readOutputTable(inFile, tables);
                string database4_3 = "INSERT INTO [PM].[dbo].[MajorSecurityInitailization]([projectCode],[projectQuarter],[header],[value])VALUES";
                for (int k = 0; k < tableWord.getRowNumber(); k++)
                {
                    database4_3 = database4_3 + "('" + projectCode + "','" + quarter + "','" + tableWord.getRow(k)[0] + "','" + tableWord.getRow(k)[1] + "'),";
                }
                database4_3 = database4_3.Substring(0, database4_3.Length - 1);
                database4_3 = database4_3 + ";";

                objConn.Open();
                //SqlCommand db4_3 = new SqlCommand(database4_3, objConn);
                //db4_3.ExecuteNonQuery();
                objConn.Close();

            }
            */
            tableList tableTmp = null;
            tableTmp = tables.getTableList("4_4@Database file@1");
            if (tableTmp != null)
            {
                List<object[]> databaseFileList = new List<object[]>();
                tableWord = new tableListWord(tableTmp);
                for (int k = 0; k < tableWord.getRowNumber(); k++)
                {
                    databaseFileList.Add(new object[] { tableWord.getRow(k)[0], tableWord.getRow(k)[1], tableWord.getRow(k)[2], tableWord.getRow(k)[3], tableWord.getRow(k)[4], tableWord.getRow(k)[5] });
                }
                dbHelper.InsertDatabaseFile(projectCode, quarter, databaseFileList);
            }

            tableTmp = null;
            tableTmp = tables.getTableList("4_5@Temp file@1");
            if (tableTmp != null)
            {
                List<object[]> tempFileList = new List<object[]>();
                tableWord = new tableListWord(tableTmp);
                for (int k = 0; k < tableWord.getRowNumber(); k++)
                {
                    tempFileList.Add(new object[] { tableWord.getRow(k)[0], tableWord.getRow(k)[1], tableWord.getRow(k)[2], tableWord.getRow(k)[3], tableWord.getRow(k)[4], tableWord.getRow(k)[5] });
                }
                dbHelper.InsertTempFile(projectCode, quarter, tempFileList);
            }

            tableTmp = null;
            tableTmp = tables.getTableList("4_6@Redo log file@1");
            if (tableTmp != null)
            {
                List<object[]> redoLogList = new List<object[]>();
                tableWord = new tableListWord(tableTmp);

                /// Convert B to MB (***warning: available only MB is string that represent integer)
                for (int k = 0; k < tableWord.getRowNumber(); k++)
                {
                    object[] rowTmp = tableWord.getRow(k);
                    if (rowTmp[2] is string)
                    {
                        rowTmp[2] = (float.Parse((string)(rowTmp[2])) / (1024 * 1024)).ToString();
                        redoLogList.Add(new object[] { tableWord.getRow(k)[0], tableWord.getRow(k)[1], rowTmp[2]});
                    }
                    else
                    {
                        redoLogList.Add(new object[] { tableWord.getRow(k)[0], tableWord.getRow(k)[1], tableWord.getRow(k)[2] });
                    }
                }
                dbHelper.InsertRedoLogFile(projectCode, quarter, redoLogList);
            }

            tableTmp = null;
            tableTmp = tables.getTableList("4_7@Controlfile@1");
            if (tableTmp != null)
            {
                List<object> controlFileList = new List<object>();
                tableWord = new tableListWord(tableTmp);
                for (int k = 0; k < tableWord.getRowNumber(); k++)
                {
                    controlFileList.Add(tableWord.getRow(k)[0]);
                }
                dbHelper.InsertControlFile(projectCode, quarter, controlFileList);
            }

            tableTmp = null;
            tableTmp = tables.getTableList("4_8@Jobs@1");
            if (tableTmp != null)
            {
                List<object[]> dailyList = new List<object[]>();
                int[] indexA = { 1024, 3, 1024 };
                tableWord = new tableListWord(tableTmp, indexA);
                for (int k = 0; k < tableWord.getRowNumber(); k++)
                {
                    dailyList.Add(new object[] { tableWord.getRow(k)[0], tableWord.getRow(k)[1], tableWord.getRow(k)[2] });
                }
                dbHelper.InsertDiaryWorksheet(projectCode, quarter, dailyList);
            }

            path = binFolderPath + "/Debug/config/4_9.txt";
            using (TextReader inFile = File.OpenText(path))
            {
                tableWord = oracleInfo.readOutputTable(inFile, tables);
                List<object[]> monthLyList = new List<object[]>();
                for (int k = 0; k < tableWord.getRowNumber(); k++)
                {
                    monthLyList.Add(new object[] { tableWord.getRow(k)[0], tableWord.getRow(k)[1], tableWord.getRow(k)[2] });
                }
                dbHelper.InsertMonthlyWorksheet(projectCode, quarter, monthLyList);
            }

            //////////////////////////////////////////////////////////    O5    ////////////////////////////////////////////////////////// 
            
            path = binFolderPath + "/Debug/config/5_1.txt";
            using (TextReader inFile = File.OpenText(path))
            {
                tableWord = oracleInfo.readOutputTable(inFile, tables);
                List<object[]> performanceReview = new List<object[]>();

                for (int k = 0; k < tableWord.getRowNumber(); k++)
                {
                    if (k == 0)
                    {
                        object[] obj1 = (object[])tableWord.getRows()[k];
                        tableListWord obj2 = (tableListWord)obj1[1];
                        List<object[]> hitRatioList = new List<object[]>();
                        for (int z = 0; z < obj2.getRows().Count; z++)
                        {
                            object[] subDetail = (object[])obj2.getRows()[z];
                            hitRatioList.Add(new object[] {subDetail[0].ToString(), subDetail[1].ToString()});
                        }
                        dbHelper.InsertHitRatio(projectCode, quarter, hitRatioList);
                    }
                    else if (k == 1)
                    {
                        object[] obj1 = (object[])tableWord.getRows()[k];
                        tableListWord obj2 = (tableListWord)obj1[1];
                        List<object[]> pinRatioList = new List<object[]>();
                        for (int z = 0; z < obj2.getRows().Count; z++)
                        {
                            object[] subDetail = (object[])obj2.getRows()[z];
                            pinRatioList.Add(new object[] {subDetail[0].ToString(), subDetail[1].ToString(), subDetail[2].ToString() });
                        }
                        dbHelper.InsertHitRatio(projectCode, quarter, pinRatioList);
                    }
                    else if (k == 14)
                    {
                        object[] obj1 = (object[])tableWord.getRows()[k];
                        tableListWord obj2 = (tableListWord)obj1[1];
                        List<object[]> undoList = new List<object[]>();
                        for (int z = 0; z < obj2.getRows().Count; z++)
                        {
                            object[] subDetail = (object[])obj2.getRows()[z];
                            undoList.Add(new object[] { subDetail[0].ToString(), subDetail[1].ToString(), subDetail[2].ToString() });
                        }
                        dbHelper.InsertUndoSegmentsSize(projectCode, quarter, undoList);
                    }
                    else
                    {
                        performanceReview.Add(new object[] { tableWord.getRow(k)[0], tableWord.getRow(k)[1] });
                    }
                }
                dbHelper.InsertPerformanceReview(projectCode, quarter, performanceReview);
            }

            // Table fress space.
            tableTmp = null;
            tableTmp = tables.getTableList("6_1@Table free space@1");
            if (tableTmp != null)
            {
                tableWord = new tableListWord(tableTmp);
                List<object[]> list = new List<object[]>();
                for (int k = 0; k < tableWord.getRowNumber(); k++)
                {
                    list.Add(new object[] { tableWord.getRow(k)[0], tableWord.getRow(k)[1], tableWord.getRow(k)[2], tableWord.getRow(k)[3], tableWord.getRow(k)[4] });
                }
                dbHelper.InsertTablespaceFreespace(projectCode, quarter, list);
            }

            // Temptable and tablespace
            tableTmp = null;
            tableTmp = tables.getTableList("7_1@default tbs/temp per user@1");
            if (tableTmp != null)
            {
                List<object[]> list = new List<object[]>();
                tableWord = new tableListWord(tableTmp);
                for (int k = 0; k < tableWord.getRowNumber(); k++)
                {
                    list.Add(new object[] { tableWord.getRow(k)[0], tableWord.getRow(k)[1], tableWord.getRow(k)[2]});
                }
                dbHelper.InsertTablespaceAndTempTablespace(projectCode, quarter, list);
            }

            // Database registration Insert.
            tableTmp = null;
            tableTmp = tables.getTableList("8_1@dba registry@1");
            if (tableTmp != null)
            {
                List<object[]> list = new List<object[]>();
                tableWord = new tableListWord(tableTmp);
                for (int k = 0; k < tableWord.getRowNumber(); k++)
                {
                    list.Add(new object[] { tableWord.getRow(k)[0], tableWord.getRow(k)[1], tableWord.getRow(k)[2], tableWord.getRow(k)[3] });
                }
                dbHelper.InsertDatabaseRegistry(projectCode, quarter, list);
            }

            // Growth Rate Insert.
            string currentAllocate, currentUsed, allocateGrowth, useGrowth;
            currentAllocate = currAlloc.Text;
            currentUsed = usedAlloc.Text;
            allocateGrowth = allocGrowth.Text;
            useGrowth = usedGrowth.Text;

            object[] growthRateList = new object[] { currentAllocate, currentUsed, allocateGrowth, useGrowth };
            dbHelper.InsertDatabaseGrowthRate(projectCode, quarter, growthRateList);



            //Delete Folder temp.
            Directory.Delete(tempPath,true);
        }

        // Searching funtion that need environment list and keyword it will return value
        public string searchTextEnvironment(List<environment> list, string keyword)
        {
            string result = "";
            for (int i = 0; i < list.Count(); i++)
            {
                if (list[i].getHeader().ToLower().Equals(keyword.ToLower()))
                {
                    result = list[i].getValue();
                    break;
                }
                else
                {
                    result = null;
                }
            }
            return result;
        }
        
        // Searching funtion that need Hardware list and keyword it will return value
        public string searchTextEnvironment(List<Hardware> list, string keyword)
        {
            string result = "";
            for (int i = 0; i < list.Count(); i++)
            {
                if (list[i].getHeader().ToLower().Equals(keyword.ToLower()))
                {
                    result = list[i].getValue();
                    break;
                }
                else
                {
                    result = null;
                }
            }
            return result;
        }

        // Environment Class.
        public class environment
        {
            public string header;
            public string value;
            public environment(string hearder, string value)
            {
                this.header = hearder;
                this.value = value;
            }
            public string getHeader()
            {
                return this.header;
            }
            public string getValue()
            {
                return this.value;
            }
        }

        // Hardware Class.
        public class Hardware
        {
            public string header;
            public string value;
            public Hardware(string hearder, string value)
            {
                this.header = hearder;
                this.value = value;
            }
            public string getHeader()
            {
                return this.header;
            }
            public string getValue()
            {
                return this.value;
            }
        }

        // Disk space Class.
        public class DiskSpace
        {
            public string fileSystem;
            public string mbBlocks;
            public string free;
            public string percentUsed;
            public string iUsed;
            public string percentIUsed;
            public string mountedOn;
            public object[] diskStroage;

            public DiskSpace(string fileSystem, string mbBlocks, string free, string percentUsed, string iUsed, string percentIUsed, string mountedOn)
            {
                this.fileSystem = fileSystem;
                this.mbBlocks = mbBlocks;
                this.free = free;
                this.percentUsed = percentUsed;
                this.iUsed = iUsed;
                this.percentIUsed = percentIUsed;
                this.mountedOn = mountedOn;
                diskStroage = new object[7];
                diskStroage[0] = fileSystem;
                diskStroage[1] = mbBlocks;
                diskStroage[2] = free;
                diskStroage[3] = percentUsed;
                diskStroage[4] = iUsed;
                diskStroage[5] = percentIUsed;
                diskStroage[6] = mountedOn;
            }
            public object[] getDiskObject() 
            {
                return this.diskStroage;
            }
        }
        
        // Function is part of generating Folder Name. 
        public static string GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }
        
        // Search information by project code. (Optional Choice)
        protected void srhProjCode_Click(object sender, EventArgs e)
        {
           
        }

        // Finish function will call stack of function work.
        protected void Finish_Click(object sender, EventArgs e)
        {
            string projectCode = projCodeInput.Text;
            string quarter = quarterInput.Text;

            string osPath = OSInput.Text;

            osConfigGen(osPath);
            projectCode = projCodeInput.Text;
            quarter = quarterInput.Text;
            databaseConfigGen(projectCode, quarter);  
            pmInfoConfig();
        }

        // Function that get person information and Insert to database.
        public void personInfoConfig()
        {

        }
            
        // Function that check conditation to trap the special symbol use in OSGenerator and DBGenerator.
        public string parseSpecialSymbol(string text)
        {
            string[] splits = text.Split(new string[] { "\\n" }, StringSplitOptions.None);
            string join = string.Join("\n", splits);

            splits = join.Split(new string[] { "\\t" }, StringSplitOptions.None);
            join = string.Join("\t", splits);

            return join;
        }

        // Function that rearrange number of text input line use in OSGenerator and DBGenerator.
        public string rearrangeNumber(string text)
        {
            int tmpInt;
            if (int.TryParse(text, out tmpInt))
            {
                return tmpInt.ToString();
            }
            return text;
        }

        // Function that check the center of special symbol.
        protected string getCenter(string text, int pass, char ch)
        {
            string tmp = text.Trim();
            int i, j;
            for (i = pass; i < tmp.Length && tmp[i] == ch; i++) ;
            for (j = tmp.Length - pass - 1; j > 0 && tmp[j] == ch; j--) ;
            return tmp.Substring(i, j - i + 1);     //fixed
        }

        // Function that trap condition and return type of starting point.
        private int checkCase(string text)
        {
            if (text.Length > 2)
            {
                string tmp = text.Substring(0, 3);
                if (tmp.Equals(">O<"))
                { //header
                    return 1;
                }
                else if (tmp.Equals("^o^"))
                { //begin table
                    return 2;
                }
                else if (tmp.Equals("*8*") || tmp.Equals("T^T"))
                { //end table
                    return 3;
                }
            }
            return 0;
        }
        
    } // End of class.
} // End of namespace.