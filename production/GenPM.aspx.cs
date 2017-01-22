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
        string strConnString = "Server=localhost;Uid=sa;PASSWORD=08102535;database=PM;Max Pool Size=400;Connect Timeout=600;";
        SqlConnection objConn;
        SqlCommand objCmd;
        bool globalChk = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            
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
            string ChkServerMacSpecStr = "INSERT INTO [PM].[dbo].[ChkServerMacSpec]([projectCode] ,[projectQuarter],[hostname],[ipAddress],[oracleOwner_login],[oracleOwner_homeDirectory],[oracleOwner_shell],[oracleGroup_firstGroup],[oracleGroup_secondGroup]) VALUES";
            ChkServerMacSpecStr = ChkServerMacSpecStr + "('"+ projectcode + "', '"+ quarter +"' ,'" + hostname + "','" + ipAddress + "','" + oracleOwner_login + "','" + oracleOwner_homeDirectory + "','" + oracleOwner_shell + "','" + oracleGroup_firstGroup + "','" + oracleGroup_secondGroup + "')";
            objConn = new SqlConnection(strConnString);
            objConn.Open();
            SqlCommand ChkServerMacSpecCmd = new SqlCommand(ChkServerMacSpecStr, objConn);
            ChkServerMacSpecCmd.ExecuteNonQuery();
            
            // Check disk space.
            string diskSpaceStr = "INSERT INTO [PM].[dbo].[OSDiskSpace]([projectCode],[projectQuarter],[node],[fileSystem],[mbBlocks],[free],[percentUsed],[iUsed],[percentIUsed],[mountedOn])VALUES";
            for (int j = 0; j < diskList.Count; j++)
            {
                diskSpaceStr = diskSpaceStr + "('"+projectcode+"','"+quarter+"',1,'" + diskList[j].getDiskObject()[0].ToString() + "','" + diskList[j].getDiskObject()[1].ToString() + "','" + diskList[j].getDiskObject()[2].ToString() + "','" + diskList[j].getDiskObject()[3].ToString() + "','" + diskList[j].getDiskObject()[4].ToString() + "','" + diskList[j].getDiskObject()[5].ToString() + "','" + diskList[j].getDiskObject()[6].ToString() + "')";
                if (j != diskList.Count)
                {
                    diskSpaceStr = diskSpaceStr + ",";
                }
            }
            diskSpaceStr = diskSpaceStr.Substring(0, diskSpaceStr.Length - 1);
            diskSpaceStr = diskSpaceStr + ";";
            SqlCommand diskSpaceCmd = new SqlCommand(diskSpaceStr, objConn);
            diskSpaceCmd.ExecuteNonQuery();

            // Compare oracle requirement.
            osKernel = "AIXTHREAD_SCOPE=" + searchTextEnvironment(environmentList, "AIXTHREAD_SCOPE");
            string compareRequirementStr = "INSERT INTO [PM].[dbo].[CompareOracleRequirement]([projectCode],[projectQuarter],[osNode],[osType],[osInfo],[osMemSize],[osSwap],[osTmp],[osJava],[osKernel])";
            compareRequirementStr = compareRequirementStr + "VALUES('"+ projectcode +"','"+ quarter +"',"+ osNode +",'"+ osType +"','"+ osInfo +"','"+ osMemSize +"','"+ osSwap +"','"+ osTmp +"','"+ osJava +"','"+ osKernel +"')";
            SqlCommand compareRequirementCmd = new SqlCommand(compareRequirementStr, objConn);
            compareRequirementCmd.ExecuteNonQuery();

            // Hardware Configuration.
            string hardwareStr = "INSERT INTO [PM].[dbo].[HardwareConfiguration]([projectCode],[projectQuarter],[node],[systemModel],[machineSerialNumber],[processorType],[processorImplementationMode],[processorVersion],[numOfProc],[procClockSpeed],[cpuType],[kernelType],[ipaddress],[subNetMask],[crontab])VALUES";
            hardwareStr = hardwareStr + "('"+projectcode+"','"+quarter+"', "+ osNode +" , '"+systemModel+"','"+machineSerialNumber+"','"+processorType+"','"+processorImplementationMode+"','"+processorVersion+"','"+numOfProc+"','"+procClockSpeed+"','"+cpuType+"','"+kernelType+"','"+ipaddress+"','"+subNetMask+"','"+ crontab + "')";
            SqlCommand hardwareCmd = new SqlCommand(hardwareStr, objConn);
            hardwareCmd.ExecuteNonQuery();

            // Environment configuration.
            string environmentStr = "INSERT INTO [PM].[dbo].[UserEnvironment]([projectCode],[projectQuarter],[header],[value]) VALUES ";
            for (int j = 0; j < environmentList.Count; j++) {
                environmentStr = environmentStr + "('"+ projectcode +"','"+ quarter +"','"+ environmentList[j].getHeader() +"','"+ environmentList[j].getValue() +"'),";             
            }
            environmentStr = environmentStr.Substring(0, environmentStr.Length - 1);
            environmentStr = environmentStr + ";";
            SqlCommand environmentCmd = new SqlCommand(environmentStr, objConn);
            environmentCmd.ExecuteNonQuery();

            // Close object connection.
            objConn.Close();
            foreach (string file in Files_Init)
            {
                //File.Delete(file);
            }
            //Directory.Delete(tempPath);

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

            string projectCodeQuery = "SELECT * FROM [PM].[dbo].[PmInfo] WHERE [projectCode] = '" + projCode + "';";
            objConn = new SqlConnection(strConnString);
            SqlDataReader reader;
            objCmd = new SqlCommand(projectCodeQuery, objConn);
            objConn.Open();
            reader = objCmd.ExecuteReader();
            if (!reader.HasRows)
            {
                string intStr = "INSERT INTO [PM].[dbo].[PmInfo]([projectCode],[customerCompanyFull],[customerAbbv],[projectName],[quater],[pmstatus],[Reviewer],[Authun],[databaseName])";
                intStr = intStr + "VALUES('" + projCode + "','" + customerCompanyFull + "','" + customerCompanyAbbv + "','" + projectName + "','" + quarter + "','In Progess','" + docReviewed + "','" + authun + "','" + databaseNamed + "');";
                reader.Close();
                SqlCommand infoInsert = new SqlCommand(intStr, objConn);
                infoInsert.ExecuteReader();

            }
            else
            {
                globalChk = true;
            }

            objConn.Close();
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
            
            SetOfTableList tables = null;

            OracleInformation oracleInfo = new OracleInformation();
            objConn = new SqlConnection(strConnString);

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
                string database4_1 = "INSERT INTO [PM].[dbo].[DatabaseConfiguration]([projectCode],[projectQuarter],[header],[value])VALUES";
                for (int k = 0; k < tableWord.getRowNumber(); k++)
                {
                    if (tableWord.getRow(k)[0].Equals("Temp tablespace size"))
                    {
                        object[] obj1 = (object[])tableWord.getRows()[k];
                        tableListWord obj2 = (tableListWord)obj1[1];
                        string database4_1_1 = "INSERT INTO [PM].[dbo].[TempTableSize]([projectCode],[projectQuarter],[tempTableSpace],[size])VALUES";
                        for (int z = 0; z < obj2.getRows().Count; z++)
                        {
                            object[] subDetail = (object[])obj2.getRows()[z];
                            database4_1_1 = database4_1_1 + "('" + projectCode + "','" + quarter + "','" + subDetail[0].ToString() + "','" + subDetail[1].ToString() + "'),";
                        }
                        database4_1_1 = database4_1_1.Substring(0, database4_1_1.Length - 1);
                        database4_1_1 = database4_1_1 + ";";
                        objConn.Open();
                        SqlCommand db4_1_1 = new SqlCommand(database4_1_1, objConn);
                        db4_1_1.ExecuteNonQuery();
                        objConn.Close();
                    }
                    else if (tableWord.getRow(k)[0].Equals("Tablespace size"))
                    {
                        object[] obj1 = (object[])tableWord.getRows()[k];
                        tableListWord obj2 = (tableListWord)obj1[1];
                        string database4_1_2 = "INSERT INTO [PM].[dbo].[TablespaceName]([projectCode],[projectQuarter],[tablespaceName],[size])VALUES";
                        for (int z = 0; z < obj2.getRows().Count; z++)
                        {
                            object[] subDetail = (object[])obj2.getRows()[z];
                            database4_1_2 = database4_1_2 + "('" + projectCode + "','" + quarter + "','" + subDetail[0].ToString() + "','" + subDetail[1].ToString() + "'),";
                        }
                        database4_1_2 = database4_1_2.Substring(0, database4_1_2.Length - 1);
                        database4_1_2 = database4_1_2 + ";";
                        objConn.Open();
                        SqlCommand db4_1_2 = new SqlCommand(database4_1_2, objConn);
                        db4_1_2.ExecuteNonQuery();
                        objConn.Close();
                    }
                    else
                    {
                        database4_1 = database4_1 + "('" + projectCode + "','" + quarter + "','" + tableWord.getRow(k)[0] + "','" + tableWord.getRow(k)[1] + "'),";
                    }
                }
                database4_1 = database4_1.Substring(0, database4_1.Length - 1);
                database4_1 = database4_1 + ";";

                objConn.Open();
                SqlCommand db4_1 = new SqlCommand(database4_1, objConn);
                db4_1.ExecuteNonQuery();
                objConn.Close();
            }

            path = binFolderPath + "/Debug/config/4_2.txt";
            using (TextReader inFile = File.OpenText(path))
            {
                tableWord = oracleInfo.readOutputTable(inFile, tables);
                string database4_2 = "INSERT INTO [PM].[dbo].[DatabaseParameter]([projectCode],[projectQuarter],[header],[value])VALUES";

                for (int k = 0; k < tableWord.getRowNumber(); k++)
                {
                    database4_2 = database4_2 + "('" + projectCode + "','" + quarter + "','" + tableWord.getRow(k)[0] + "','" + tableWord.getRow(k)[1] + "'),";
                }
                database4_2 = database4_2.Substring(0, database4_2.Length - 1);
                database4_2 = database4_2 + ";";

                objConn.Open();
                SqlCommand db4_2 = new SqlCommand(database4_2, objConn);
                db4_2.ExecuteNonQuery();
                objConn.Close();
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
                string database4_4 = "INSERT INTO [PM].[dbo].[DatabaseFile]([projectCode],[projectQuarter],[tbsName],[fileName],[size],[max],[aut],[inc])VALUES";
                tableWord = new tableListWord(tableTmp);
                for (int k = 0; k < tableWord.getRowNumber(); k++)
                {
                    database4_4 = database4_4 + "('" + projectCode + "','" + quarter + "','" + tableWord.getRow(k)[0] + "','" + tableWord.getRow(k)[1] + "','" + tableWord.getRow(k)[2] + "','" + tableWord.getRow(k)[3] + "','" + tableWord.getRow(k)[4] + "','" + tableWord.getRow(k)[5] + "'),";
                }

                database4_4 = database4_4.Substring(0, database4_4.Length - 1);
                database4_4 = database4_4 + ";";
                objConn.Open();
                SqlCommand db4_4 = new SqlCommand(database4_4, objConn);
                db4_4.ExecuteNonQuery();
                objConn.Close();
            }

            tableTmp = null;
            tableTmp = tables.getTableList("4_5@Temp file@1");
            if (tableTmp != null)
            {
                string database4_5 = "INSERT INTO [PM].[dbo].[TempFile]([projectCode],[projectQuarter],[tbsName],[fileName],[size],[max],[aut],[inc]) VALUES";
                tableWord = new tableListWord(tableTmp);
                for (int k = 0; k < tableWord.getRowNumber(); k++)
                {
                    database4_5 = database4_5 + "('" + projectCode + "','" + quarter + "','" + tableWord.getRow(k)[0] + "','" + tableWord.getRow(k)[1] + "','" + tableWord.getRow(k)[2] + "','" + tableWord.getRow(k)[3] + "','" + tableWord.getRow(k)[4] + "','" + tableWord.getRow(k)[5] + "'),";
                }
                database4_5 = database4_5.Substring(0, database4_5.Length - 1);
                database4_5 = database4_5 + ";";
                objConn.Open();
                SqlCommand db4_5 = new SqlCommand(database4_5, objConn);
                db4_5.ExecuteNonQuery();
                objConn.Close();
            }

            tableTmp = null;
            tableTmp = tables.getTableList("4_6@Redo log file@1");
            if (tableTmp != null)
            {
                string database4_6 = "INSERT INTO [PM].[dbo].[RedoLogFile]([projectCode],[projectQuarter],[groupMember],[member],[size]) VALUES";
                tableWord = new tableListWord(tableTmp);

                /// Convert B to MB (***warning: available only MB is string that represent integer)
                for (int k = 0; k < tableWord.getRowNumber(); k++)
                {
                    object[] rowTmp = tableWord.getRow(k);
                    if (rowTmp[2] is string)
                    {
                        rowTmp[2] = (float.Parse((string)(rowTmp[2])) / (1024 * 1024)).ToString();
                        database4_6 = database4_6 + "('" + projectCode + "','" + quarter + "','" + tableWord.getRow(k)[0] + "','" + tableWord.getRow(k)[1] + "','" + rowTmp[2] + "'),";
                    }
                    else
                    {
                        database4_6 = database4_6 + "('" + projectCode + "','" + quarter + "','" + tableWord.getRow(k)[0] + "','" + tableWord.getRow(k)[1] + "','" + tableWord.getRow(k)[2] + "'),";
                    }
                }
                database4_6 = database4_6.Substring(0, database4_6.Length - 1);
                database4_6 = database4_6 + ";";
                objConn.Open();
                SqlCommand db4_6 = new SqlCommand(database4_6, objConn);
                db4_6.ExecuteNonQuery();
                objConn.Close();
            }

            tableTmp = null;
            tableTmp = tables.getTableList("4_7@Controlfile@1");
            if (tableTmp != null)
            {
                string database4_7 = "INSERT INTO [PM].[dbo].[ControlFile]([projectCode],[projectQuarter],[controlFileName]) VALUES";
                tableWord = new tableListWord(tableTmp);
                for (int k = 0; k < tableWord.getRowNumber(); k++)
                {
                    database4_7 = database4_7 + "('" + projectCode + "','" + quarter + "','" + tableWord.getRow(k)[0] + "'),";
                }
                database4_7 = database4_7.Substring(0, database4_7.Length - 1);
                database4_7 = database4_7 + ";";
                objConn.Open();
                SqlCommand db4_7 = new SqlCommand(database4_7, objConn);
                db4_7.ExecuteNonQuery();
                objConn.Close();
            }

            tableTmp = null;
            tableTmp = tables.getTableList("4_8@Jobs@1");
            if (tableTmp != null)
            {
                string database4_8 = "INSERT INTO [PM].[dbo].[DailyCalendarWorksheet]([projectCode],[projectQuarter],[timeDay],[descOfHouse],[estimatedDuration]) VALUES";

                int[] indexA = { 1024, 3, 1024 };
                tableWord = new tableListWord(tableTmp, indexA);
                for (int k = 0; k < tableWord.getRowNumber(); k++)
                {
                    database4_8 = database4_8 + "('" + projectCode + "','" + quarter + "','" + tableWord.getRow(k)[0] + "','" + tableWord.getRow(k)[1] + "','" + tableWord.getRow(k)[2] + "'),";
                }
                database4_8 = database4_8.Substring(0, database4_8.Length - 1);
                database4_8 = database4_8 + ";";
                objConn.Open();
                SqlCommand db4_8 = new SqlCommand(database4_8, objConn);
                db4_8.ExecuteNonQuery();
                objConn.Close();
            }

            path = binFolderPath + "/Debug/config/4_9.txt";
            using (TextReader inFile = File.OpenText(path))
            {
                tableWord = oracleInfo.readOutputTable(inFile, tables);
                string database4_9 = "INSERT INTO [PM].[dbo].[MonthlyCalendarWorksheet]([projectCode],[projectQuarter],[dayProject],[descpOfHouse],[estimatedDur]) VALUES";

                for (int k = 0; k < tableWord.getRowNumber(); k++)
                {
                    database4_9 = database4_9 + "('" + projectCode + "','" + quarter + "','" + tableWord.getRow(k)[0] + "','" + tableWord.getRow(k)[1] + "','" + tableWord.getRow(k)[2] + "'),";
                }
                database4_9 = database4_9.Substring(0, database4_9.Length - 1);
                database4_9 = database4_9 + ";";

                objConn.Open();
                SqlCommand db4_9 = new SqlCommand(database4_9, objConn);
                db4_9.ExecuteNonQuery();
                objConn.Close();
            }

            //////////////////////////////////////////////////////////    O5    ////////////////////////////////////////////////////////// 
            
            path = binFolderPath + "/Debug/config/5_1.txt";
            using (TextReader inFile = File.OpenText(path))
            {
                tableWord = oracleInfo.readOutputTable(inFile, tables);
                string database5_1 = "INSERT INTO [PM].[dbo].[performanceReview]([projectCode],[projectQuarter],[header],[value]) VALUES";

                for (int k = 0; k < tableWord.getRowNumber(); k++)
                {
                    if (k == 0)
                    {
                        object[] obj1 = (object[])tableWord.getRows()[k];
                        tableListWord obj2 = (tableListWord)obj1[1];
                        string database5_1_1 = "INSERT INTO [PM].[dbo].[getHitRatio]([projectCode],[projectQuarter],[nameSpace],[getHitRatio]) VALUES";
                        for (int z = 0; z < obj2.getRows().Count; z++)
                        {
                            object[] subDetail = (object[])obj2.getRows()[z];
                            database5_1_1 = database5_1_1 + "('" + projectCode + "','" + quarter + "','" + subDetail[0].ToString() + "','" + subDetail[1].ToString() + "'),";
                        }

                        database5_1_1 = database5_1_1.Substring(0, database5_1_1.Length - 1);
                        database5_1_1 = database5_1_1 + ";";
                        objConn.Open();
                        SqlCommand db5_1_1 = new SqlCommand(database5_1_1, objConn);
                        db5_1_1.ExecuteNonQuery();
                        objConn.Close();
                    }
                    else if (k == 1)
                    {
                        object[] obj1 = (object[])tableWord.getRows()[k];
                        tableListWord obj2 = (tableListWord)obj1[1];
                        string database5_1_2 = "INSERT INTO [PM].[dbo].[PinRatio]([projectCode],[projectQuarter],[execution],[cacheMisses],[sumPin]) VALUES";
                        for (int z = 0; z < obj2.getRows().Count; z++)
                        {
                            object[] subDetail = (object[])obj2.getRows()[z];
                            database5_1_2 = database5_1_2 + "('" + projectCode + "','" + quarter + "','" + subDetail[0].ToString() + "','" + subDetail[1].ToString() + "','" + subDetail[1].ToString() + "'),";

                        }
                        database5_1_2 = database5_1_2.Substring(0, database5_1_2.Length - 1);
                        database5_1_2 = database5_1_2 + ";";
                        objConn.Open();
                        SqlCommand db5_1_2 = new SqlCommand(database5_1_2, objConn);
                        db5_1_2.ExecuteNonQuery();
                        objConn.Close();
                    }
                    else if (k == 14)
                    {
                        object[] obj1 = (object[])tableWord.getRows()[k];
                        tableListWord obj2 = (tableListWord)obj1[1];
                        string database5_1_3 = "INSERT INTO [PM].[dbo].[undoSegmentsSize]([projectCode],[projectQuarter],[amount],[segmentType],[size])  VALUES";
                        for (int z = 0; z < obj2.getRows().Count; z++)
                        {
                            object[] subDetail = (object[])obj2.getRows()[z];
                            database5_1_3 = database5_1_3 + "('" + projectCode + "','" + quarter + "','" + subDetail[0].ToString() + "','" + subDetail[1].ToString() + "','" + subDetail[2].ToString() + "'),";
                        }
                        database5_1_3 = database5_1_3.Substring(0, database5_1_3.Length - 1);
                        database5_1_3 = database5_1_3 + ";";
                        objConn.Open();
                        SqlCommand db5_1_3 = new SqlCommand(database5_1_3, objConn);
                        db5_1_3.ExecuteNonQuery();
                        objConn.Close();
                    }
                    else
                    {
                        database5_1 = database5_1 + "('" + projectCode + "','" + quarter + "','" + tableWord.getRow(k)[0] + "','" + tableWord.getRow(k)[1] + "'),";
                    }
                }
                database5_1 = database5_1.Substring(0, database5_1.Length - 1);
                database5_1 = database5_1 + ";";

                objConn.Open();
                SqlCommand db5_1 = new SqlCommand(database5_1, objConn);
                db5_1.ExecuteNonQuery();
                objConn.Close();
            }

            //////////////////////////////////////////////////////////    O6    ////////////////////////////////////////////////////////// 
            tableTmp = null;
            tableTmp = tables.getTableList("6_1@Table free space@1");
            if (tableTmp != null)
            {
                string database6_1 = "INSERT INTO [PM].[dbo].[TablespaceFreespace]([projectCode],[projectQuarter],[tablespaceName],[maxBlocks],[countBlock],[sumFreeBlock],[pct_free]) VALUES";
                tableWord = new tableListWord(tableTmp);
                for (int k = 0; k < tableWord.getRowNumber(); k++)
                {
                    database6_1 = database6_1 + "('" + projectCode + "','" + quarter + "','" + tableWord.getRow(k)[0] + "','" + tableWord.getRow(k)[1] + "','" + tableWord.getRow(k)[2] + "','" + tableWord.getRow(k)[3] + "','" + tableWord.getRow(k)[4] + "'),";
                }

                database6_1 = database6_1.Substring(0, database6_1.Length - 1);
                database6_1 = database6_1 + ";";
                objConn.Open();
                SqlCommand db6_1 = new SqlCommand(database6_1, objConn);
                db6_1.ExecuteNonQuery();
                objConn.Close();
            }

            //////////////////////////////////////////////////////////    O7    ////////////////////////////////////////////////////////// 

            tableTmp = null;
            tableTmp = tables.getTableList("7_1@default tbs/temp per user@1");
            if (tableTmp != null)
            {
                string database7_1 = "INSERT INTO [PM].[dbo].[TablespaceAndTempTablespace]([projectCode],[projectQuarter],[userName],[defaultTablespace],[temporaryTablespace]) VALUES";
                tableWord = new tableListWord(tableTmp);
                for (int k = 0; k < tableWord.getRowNumber(); k++)
                {
                    database7_1 = database7_1 + "('" + projectCode + "','" + quarter + "','" + tableWord.getRow(k)[0] + "','" + tableWord.getRow(k)[1] + "','" + tableWord.getRow(k)[2] + "'),";
                }
                database7_1 = database7_1.Substring(0, database7_1.Length - 1);
                database7_1 = database7_1 + ";";
                objConn.Open();
                SqlCommand db7_1 = new SqlCommand(database7_1, objConn);
                db7_1.ExecuteNonQuery();
                objConn.Close();
            }

            //////////////////////////////////////////////////////////    O8    ////////////////////////////////////////////////////////// 

            tableTmp = null;
            tableTmp = tables.getTableList("8_1@dba registry@1");
            if (tableTmp != null)
            {
                string database8_1 = "INSERT INTO [PM].[dbo].[DatabaseRegistry]([projectCode],[projectQuarter],[compId],[version],[status],[lastModified]) VALUES";
                tableWord = new tableListWord(tableTmp);
                for (int k = 0; k < tableWord.getRowNumber(); k++)
                {
                    database8_1 = database8_1 + "('" + projectCode + "','" + quarter + "','" + tableWord.getRow(k)[0] + "','" + tableWord.getRow(k)[1] + "','" + tableWord.getRow(k)[2] + "','" + tableWord.getRow(k)[3] + "'),";
                }
                database8_1 = database8_1.Substring(0, database8_1.Length - 1);
                database8_1 = database8_1 + ";";
                objConn.Open();
                SqlCommand db8_1 = new SqlCommand(database8_1, objConn);
                db8_1.ExecuteNonQuery();
                objConn.Close();
            }
            //Delete Folder temp.
            Directory.Delete(tempPath);
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
            string projCode = projCodeInput.Text;
            custComFull.Text = "";
            custComAbbv.Text = "";
            string projectCodeQuery = "SELECT * FROM [PM].[dbo].[PmInfo] WHERE [projectCode] = '" + projCode + "';";
            objConn = new SqlConnection(strConnString);
            SqlDataReader reader;
            objCmd = new SqlCommand(projectCodeQuery, objConn);
            objConn.Open();
            reader = objCmd.ExecuteReader();
            if (reader.HasRows)
            {
                globalChk = true;
                while (reader.Read())
                {
                    if (reader["projectCode"].ToString() == projCode)
                    {
                        custComFull.Text = reader["customerCompanyFull"].ToString();
                        custComAbbv.Text = reader["customerAbbv"].ToString();
                    }
                    else
                    {
                        custComFull.Text = "None";
                        custComAbbv.Text = "None";
                    }
                }

                CustFName.Text = "";
                CustLName.Text = "";
                CustEmail.Text = "";
                CustPhone.Text = "";
                SaleFName.Text = "";
                SaleLName.Text = "";
                SaleEmail.Text = "";
                SalePhone.Text = "";
                EGFName.Text = "";
                EGLName.Text = "";
                EGEmail.Text = "";
                EGPhone.Text = "";


                string personInFoQuery = "SELECT Personinfo.*, PmInfo_PersonInfo.* FROM Personinfo JOIN PmInfo_PersonInfo ON Personinfo.personID = PmInfo_PersonInfo.personinfo_personID";
                objConn = new SqlConnection(strConnString);
                SqlDataReader readerPerson;
                objCmd = new SqlCommand(personInFoQuery, objConn);
                readerPerson = objCmd.ExecuteReader();
                if (readerPerson.HasRows)
                {
                    while (readerPerson.Read())
                    {
                        if (readerPerson["personType"].ToString().Equals("customer"))
                        {
                            CustFName.Text = readerPerson["personName"].ToString();
                            CustLName.Text = readerPerson["personLastName"].ToString();
                            CustEmail.Text = readerPerson["personPhone"].ToString();
                            CustPhone.Text = readerPerson["customerCompanyFull"].ToString();
                        }
                        else if (readerPerson["personType"].ToString().Equals("sale"))
                        {
                            SaleFName.Text = readerPerson["personName"].ToString();
                            SaleLName.Text = readerPerson["personLastName"].ToString();
                            SaleEmail.Text = readerPerson["personPhone"].ToString();
                            SalePhone.Text = readerPerson["customerCompanyFull"].ToString();
                        }
                        else if (readerPerson["personType"].ToString().Equals("engineer"))
                        {
                            EGFName.Text = readerPerson["personName"].ToString();
                            EGLName.Text = readerPerson["personLastName"].ToString();
                            EGEmail.Text = readerPerson["personPhone"].ToString();
                            EGPhone.Text = readerPerson["customerCompanyFull"].ToString();
                        }
                    }
                }
            }
            else
            {
                globalChk = false;
            }


            reader.Close();
            objConn.Close();
        }

        // Finish function will call stack of function work.
        protected void Finish_Click(object sender, EventArgs e)
        {
            string projectCode = projCodeInput.Text;
            string quarter = quarterInput.Text;

            string osPath = OSInput.Text;

            osConfigGen(osPath);
            //projectCode = projCodeInput.Text;
            //quarter = quarterInput.Text;
            //databaseConfigGen(projectCode, quarter);  
            //pmInfoConfig();
        }

        // Function that get person information and Insert to database.
        public void personInfoConfig()
        {
            string projCode = projCodeInput.Text;
            string quarter = quarterInput.Text;
            string cusName = CustFName.Text;
            string cusLName = CustLName.Text;
            string cusEmail = CustEmail.Text;
            string cusPhone = CustPhone.Text;
            string saleName = SaleFName.Text;
            string saleLName = SaleLName.Text;
            string saleEmail = SaleEmail.Text;
            string salePhone = SalePhone.Text;
            string enName = EGFName.Text;
            string enLName = EGLName.Text;
            string enEmail = EGEmail.Text;
            string enPhone = EGPhone.Text;

            string infPerson = "INSERT INTO [PM].[dbo].[Personinfo]([projectCode],[projectQuarter],[personName],[personLastName],[personEmail],[personPhone],[personType]) VALUES";
            infPerson = infPerson + "('" + projCode + "'+'" + quarter + "'+'" + cusName + "','" + cusLName + "','" + cusEmail + "','" + cusPhone + "','customer'),";
            infPerson = infPerson + "('" + projCode + "'+'" + quarter + "'+'" + saleName + "','" + saleLName + "','" + saleEmail + "','" + salePhone + "','sale'),";
            infPerson = infPerson + "('" + projCode + "'+'" + quarter + "'+'" + enName + "','" + enLName + "','" + enEmail + "','" + enPhone + "','engineer'),";
            objConn.Open();
            SqlCommand infoInsert = new SqlCommand(infPerson, objConn);
            infoInsert.ExecuteNonQuery();
            objConn.Close();
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