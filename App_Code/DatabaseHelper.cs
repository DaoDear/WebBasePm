using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Class1
/// </summary>
namespace WebBasePM
{
    public class DatabaseHelper
    {
        protected string connectionString = "Server=localhost;Uid=sa;PASSWORD=08102535;database=PM;Max Pool Size=400;Connect Timeout=600;";
        protected SqlConnection connectionObject;


        public DatabaseHelper()
        {
            connectionObject = new SqlConnection(connectionString);
        }
                

        public Object[] GetSingleQueryObject(string query) {
            Object[] rowObject;
            SqlCommand commandObject;
            SqlDataReader reader;

            connectionObject.Open();
            commandObject = new SqlCommand(query, connectionObject);
            reader = commandObject.ExecuteReader();
            if (reader.HasRows)
            {
                rowObject = new object[reader.FieldCount];
                while (reader.Read())
                {
                    for(int i = 0; i < reader.FieldCount; i++)
                    {
                        rowObject[i] = reader[i].ToString();
                    }
                }
            }
            else {
                rowObject = null;
            }
            reader.Close();
            connectionObject.Close();            

            return rowObject;
        }

        public void GetQueryInsertCommand(string query)
        {
            SqlCommand commandObject;

            connectionObject.Open();
            commandObject = new SqlCommand(query, connectionObject);
            commandObject.ExecuteNonQuery();
            connectionObject.Close();
        }

        public void InsertBackupDatabase(string projectCodeStr, string projectQuarterStr, string backupDB, string backupCF, string backupAL)
        {
            string projectCode = projectCodeStr;
            string projectQuarter = projectQuarterStr;
            SqlCommand commandObject;

            string environment = "INSERT INTO [PM].[dbo].[UserEnvironment]([projectCode],[projectQuarter],[header],[value]) VALUES ";
            environment += environment + "('" + projectCode + "','" + projectQuarter + "','" + backupCF + "','" + backupAL + "','" + backupDB + "'),";
            connectionObject.Open();
            commandObject = new SqlCommand(environment, connectionObject);
            commandObject.ExecuteNonQuery();
            connectionObject.Close();
        }

        public void InsertEnvironment(string projectCodeStr, string projectQuarterStr, List<object[]> list)
        {
            string projectCode = projectCodeStr;
            string projectQuarter = projectQuarterStr;
            SqlCommand commandObject;

            string environment = "INSERT INTO [PM].[dbo].[UserEnvironment]([projectCode],[projectQuarter],[header],[value]) VALUES ";
            for (int i = 0; i < list.Count; i++)
            {
                environment = environment + "('" + projectCode + "','" + projectQuarter + "','" + (list[i])[0] + "','" + (list[i])[1] + "'),";
            }

            environment = environment.Substring(0, environment.Length - 1);
            environment = environment + ";";
            connectionObject.Open();
            commandObject = new SqlCommand(environment, connectionObject);
            commandObject.ExecuteNonQuery();
            connectionObject.Close();
        }

        public void InsertPMConfiguration(object[] list) {
            SqlCommand commandObject;

            string pmConfiguration = "INSERT INTO [PM].[dbo].[PmInfo]([projectCode],[customerCompanyFull],[customerAbbv],[projectName],[quater],[pmstatus],[Reviewer],[Authun],[databaseName])";
            pmConfiguration = pmConfiguration + "VALUES('" + list[0] + "','" + list[1] + "','" + list[2] + "','" + list[3] + "','" + list[4] + "','In Progess','" + list[5] + "','" + list[6] + "','" + list[7] + "');";
            connectionObject.Open();
            commandObject = new SqlCommand(pmConfiguration, connectionObject);
            commandObject.ExecuteNonQuery();
            connectionObject.Close();
        }

        public void InsertHardware(string projectCodeStr, string projectQuarterStr, List<object> list)
        {
            string projectCode = projectCodeStr;
            string projectQuarter = projectQuarterStr;
            SqlCommand commandObject;

            string hardware = "INSERT INTO [PM].[dbo].[HardwareConfiguration]([projectCode],[projectQuarter],[node],[systemModel],[machineSerialNumber],[processorType],[processorImplementationMode],[processorVersion],[numOfProc],[procClockSpeed],[cpuType],[kernelType],[ipaddress],[subNetMask],[crontab]) VALUES";
            hardware = hardware + "('" + projectCode + "','" + projectQuarter + "','" + list[0] + "','" + list[1] + "','" + list[2] + "','" + list[3] + "','" + list[4] + "','" + list[5] + "','" + list[6] + "','" + list[7] + "','" + list[8] + "','" + list[9] + "','" + list[10] + "','" + list[11] + "','" + list[12] + "');";

            connectionObject.Open();
            commandObject = new SqlCommand(hardware, connectionObject);
            commandObject.ExecuteNonQuery();
            connectionObject.Close();

        }

        public void InsertCompareRequirement(string projectCodeStr, string projectQuarterStr, List<object> list)
        {
            string projectCode = projectCodeStr;
            string projectQuarter = projectQuarterStr;
            SqlCommand commandObject;

            string compareRequirement = "INSERT INTO [PM].[dbo].[CompareOracleRequirement]([projectCode],[projectQuarter],[osNode],[osType],[osInfo],[osMemSize],[osSwap],[osTmp],[osJava],[osKernel]) VALUES";
            compareRequirement = compareRequirement + "('" + projectCode + "','" + projectQuarter + "','" + list[0] + "','" + list[1] + "','" + list[2] + "','" + list[3] + "','" + list[4] + "','" + list[5] + "','" + list[6] + "','" + list[7] + "');";

            connectionObject.Open();
            commandObject = new SqlCommand(compareRequirement, connectionObject);
            commandObject.ExecuteNonQuery();
            connectionObject.Close();
        }

        public void InsertOsDiskSpace(string projectCodeStr, string projectQuarterStr, List<object[]> list)
        {
            string projectCode = projectCodeStr;
            string projectQuarter = projectQuarterStr;
            SqlCommand commandObject;

            string osDiskSpace = "INSERT INTO [PM].[dbo].[OSDiskSpace]([projectCode],[projectQuarter],[node],[fileSystem],[mbBlocks],[free],[percentUsed],[iUsed],[percentIUsed],[mountedOn])VALUES";
            for (int i = 0; i < list.Count; i++)
            {
                osDiskSpace = osDiskSpace + "('" + projectCode + "','" + projectQuarter + "','" + (list[i])[0] + "','" + (list[i])[1] + "','" + (list[i])[2] + "','" + (list[i])[3] + "','" + (list[i])[4] + "','" + (list[i])[5] + "','" + (list[i])[6] + "','" + (list[i])[7] + "'),";
            }

            osDiskSpace = osDiskSpace.Substring(0, osDiskSpace.Length - 1);
            osDiskSpace = osDiskSpace + ";";
            connectionObject.Open();
            commandObject = new SqlCommand(osDiskSpace, connectionObject);
            commandObject.ExecuteNonQuery();
            connectionObject.Close();
        }

        public void InsertChkServerMacSpec(string projectCodeStr, string projectQuarterStr, List<object> list) {
            string projectCode = projectCodeStr;
            string projectQuarter = projectQuarterStr;
            SqlCommand commandObject;

            string chkServerMacSpec = "INSERT INTO [PM].[dbo].[ChkServerMacSpec]([projectCode] ,[projectQuarter],[hostname],[ipAddress],[oracleOwner_login],[oracleOwner_homeDirectory],[oracleOwner_shell],[oracleGroup_firstGroup],[oracleGroup_secondGroup]) VALUES";
            chkServerMacSpec = chkServerMacSpec + "('" + projectCode + "','" + projectQuarter + "','" + list[0] + "','" + list[1] + "','" + list[2] + "','" + list[3] + "','" + list[4] + "','" + list[5] + "','" + list[6] + "');";
            
            connectionObject.Open();
            commandObject = new SqlCommand(chkServerMacSpec, connectionObject);
            commandObject.ExecuteNonQuery();
            connectionObject.Close();
        }

        public void InsertUndoSegmentsSize(string projectCodeStr, string projectQuarterStr, List<object[]> list)
        {
            string projectCode = projectCodeStr;
            string projectQuarter = projectQuarterStr;
            SqlCommand commandObject;

            string undoSegmentsSize = "INSERT INTO [PM].[dbo].[undoSegmentsSize]([projectCode],[projectQuarter],[amount],[segmentType],[size]) VALUES";
            for (int i = 0; i < list.Count; i++)
            {
                undoSegmentsSize = undoSegmentsSize + "('" + projectCode + "','" + projectQuarter + "','" + (list[i])[0] + "','" + (list[i])[1] + "','" + (list[i])[2] + "'),";
            }
            undoSegmentsSize = undoSegmentsSize.Substring(0, undoSegmentsSize.Length - 1);
            undoSegmentsSize = undoSegmentsSize + ";";
            connectionObject.Open();
            commandObject = new SqlCommand(undoSegmentsSize, connectionObject);
            commandObject.ExecuteNonQuery();
            connectionObject.Close();
        }

        public void InsertPinRatio(string projectCodeStr, string projectQuarterStr, List<object[]> list)
        {
            string projectCode = projectCodeStr;
            string projectQuarter = projectQuarterStr;
            SqlCommand commandObject;

            string pinRatio = "INSERT INTO [PM].[dbo].[PinRatio]([projectCode],[projectQuarter],[execution],[cacheMisses],[sumPin]) VALUES";
            for (int i = 0; i < list.Count; i++)
            {
                pinRatio = pinRatio + "('" + projectCode + "','" + projectQuarter + "','" + (list[i])[0] + "','" + (list[i])[1] + "'),";
            }
            pinRatio = pinRatio.Substring(0, pinRatio.Length - 1);
            pinRatio = pinRatio + ";";
            connectionObject.Open();
            commandObject = new SqlCommand(pinRatio, connectionObject);
            commandObject.ExecuteNonQuery();
            connectionObject.Close();
        }

        public void InsertHitRatio(string projectCodeStr, string projectQuarterStr, List<object[]> list)
        {
            string projectCode = projectCodeStr;
            string projectQuarter = projectQuarterStr;
            SqlCommand commandObject;

            string hitRatio = "INSERT INTO [PM].[dbo].[getHitRatio]([projectCode],[projectQuarter],[nameSpace],[getHitRatio]) VALUES";
            for (int i = 0; i < list.Count; i++)
            {
                hitRatio = hitRatio + "('" + projectCode + "','" + projectQuarter + "','" + (list[i])[0] + "','" + (list[i])[1] + "'),";
            }
            hitRatio = hitRatio.Substring(0, hitRatio.Length - 1);
            hitRatio = hitRatio + ";";
            connectionObject.Open();
            commandObject = new SqlCommand(hitRatio, connectionObject);
            commandObject.ExecuteNonQuery();
            connectionObject.Close();
        }

        public void InsertPerformanceReview(string projectCodeStr, string projectQuarterStr, List<object[]> list)
        {
            string projectCode = projectCodeStr;
            string projectQuarter = projectQuarterStr;
            SqlCommand commandObject;

            string performanceReview = "INSERT INTO [PM].[dbo].[performanceReview]([projectCode],[projectQuarter],[header],[value]) VALUES";
            for (int i = 0; i < list.Count; i++)
            {
                performanceReview = performanceReview + "('" + projectCode + "','" + projectQuarter + "','" + (list[i])[0] + "','" + (list[i])[1] + "'),";
            }
            performanceReview = performanceReview.Substring(0, performanceReview.Length - 1);
            performanceReview = performanceReview + ";";
            connectionObject.Open();
            commandObject = new SqlCommand(performanceReview, connectionObject);
            commandObject.ExecuteNonQuery();
            connectionObject.Close();
        }

        public void InsertMonthlyWorksheet(string projectCodeStr, string projectQuarterStr, List<object[]> list)
        {
            string projectCode = projectCodeStr;
            string projectQuarter = projectQuarterStr;
            SqlCommand commandObject;

            string monthlyWorkSheet = "INSERT INTO [PM].[dbo].[MonthlyCalendarWorksheet]([projectCode],[projectQuarter],[dayProject],[descpOfHouse],[estimatedDur]) VALUES";
            for (int i = 0; i < list.Count; i++)
            {
                monthlyWorkSheet = monthlyWorkSheet + "('" + projectCode + "','" + projectQuarter + "','" + (list[i])[0] + "','" + (list[i])[1] + "','" + (list[i])[2] + "'),";
            }
            monthlyWorkSheet = monthlyWorkSheet.Substring(0, monthlyWorkSheet.Length - 1);
            monthlyWorkSheet = monthlyWorkSheet + ";";
            connectionObject.Open();
            commandObject = new SqlCommand(monthlyWorkSheet, connectionObject);
            commandObject.ExecuteNonQuery();
            connectionObject.Close();
        }

        public void InsertDiaryWorksheet(string projectCodeStr, string projectQuarterStr, List<object[]> list)
        {
            string projectCode = projectCodeStr;
            string projectQuarter = projectQuarterStr;
            SqlCommand commandObject;

            string dialyWorkSheet = "INSERT INTO [PM].[dbo].[DailyCalendarWorksheet]([projectCode],[projectQuarter],[timeDay],[descOfHouse],[estimatedDuration]) VALUES";
            for (int i = 0; i < list.Count; i++)
            {
                dialyWorkSheet = dialyWorkSheet + "('" + projectCode + "','" + projectQuarter + "','" + (list[i])[0] + "','" + (list[i])[1] + "','" + (list[i])[2] + "'),";
            }
            dialyWorkSheet = dialyWorkSheet.Substring(0, dialyWorkSheet.Length - 1);
            dialyWorkSheet = dialyWorkSheet + ";";
            connectionObject.Open();
            commandObject = new SqlCommand(dialyWorkSheet, connectionObject);
            commandObject.ExecuteNonQuery();
            connectionObject.Close();
        }

        public void InsertControlFile(string projectCodeStr, string projectQuarterStr, List<object> list)
        {
            string projectCode = projectCodeStr;
            string projectQuarter = projectQuarterStr;
            SqlCommand commandObject;

            string controlFile = "INSERT INTO [PM].[dbo].[ControlFile]([projectCode],[projectQuarter],[controlFileName]) VALUES";
            for (int i = 0; i < list.Count; i++)
            {
                controlFile = controlFile + "('" + projectCode + "','" + projectQuarter + "','" + list[i] + "'),";
            }
            controlFile = controlFile.Substring(0, controlFile.Length - 1);
            controlFile = controlFile + ";";
            connectionObject.Open();
            commandObject = new SqlCommand(controlFile, connectionObject);
            commandObject.ExecuteNonQuery();
            connectionObject.Close();
        }

        public void InsertRedoLogFile(string projectCodeStr, string projectQuarterStr, List<object[]> list)
        {
            string projectCode = projectCodeStr;
            string projectQuarter = projectQuarterStr;
            SqlCommand commandObject;

            string redoLogFile = "INSERT INTO [PM].[dbo].[RedoLogFile]([projectCode],[projectQuarter],[groupMember],[member],[size]) VALUES";
            for (int i = 0; i < list.Count; i++)
            {
                redoLogFile = redoLogFile + "('" + projectCode + "','" + projectQuarter + "','" + (list[i])[0] + "','" + (list[i])[1] + "','" + (list[i])[2] + "'),";
            }
            redoLogFile = redoLogFile.Substring(0, redoLogFile.Length - 1);
            redoLogFile = redoLogFile + ";";
            connectionObject.Open();
            commandObject = new SqlCommand(redoLogFile, connectionObject);
            commandObject.ExecuteNonQuery();
            connectionObject.Close();
        }

        public void InsertTempFile(string projectCodeStr, string projectQuarterStr, List<object[]> list)
        {
            string projectCode = projectCodeStr;
            string projectQuarter = projectQuarterStr;
            SqlCommand commandObject;

            string tempFile = "INSERT INTO [PM].[dbo].[TempFile]([projectCode],[projectQuarter],[tbsName],[fileName],[size],[max],[aut],[inc]) VALUES";
            for (int i = 0; i < list.Count; i++)
            {
                tempFile = tempFile + "('" + projectCode + "','" + projectQuarter + "','" + (list[i])[0] + "','" + (list[i])[1] + "','" + (list[i])[2] + "','" + (list[i])[3] + "','" + (list[i])[4] + "','" + (list[i])[5] + "'),";
            }
            tempFile = tempFile.Substring(0, tempFile.Length - 1);
            tempFile = tempFile + ";";
            connectionObject.Open();
            commandObject = new SqlCommand(tempFile, connectionObject);
            commandObject.ExecuteNonQuery();
            connectionObject.Close();
        }

        public void InsertDatabaseFile(string projectCodeStr, string projectQuarterStr, List<object[]> list) {
            string projectCode = projectCodeStr;
            string projectQuarter = projectQuarterStr;
            SqlCommand commandObject;

            string databaseFile = "INSERT INTO [PM].[dbo].[DatabaseFile]([projectCode],[projectQuarter],[tbsName],[fileName],[size],[max],[aut],[inc])VALUES";
            for (int i = 0; i < list.Count; i++)
            {
                databaseFile = databaseFile + "('" + projectCode + "','" + projectQuarter + "','" + (list[i])[0] + "','" + (list[i])[1] + "','" + (list[i])[2] + "','" + (list[i])[3] + "','" + (list[i])[4] + "','" + (list[i])[5] + "'),";
            }
            databaseFile = databaseFile.Substring(0, databaseFile.Length - 1);
            databaseFile = databaseFile + ";";
            connectionObject.Open();
            commandObject = new SqlCommand(databaseFile, connectionObject);
            commandObject.ExecuteNonQuery();
            connectionObject.Close();
        }

        public void InsertDatabaseParameter(string projectCodeStr, string projectQuarterStr, List<object[]> list)
        {
            string projectCode = projectCodeStr;
            string projectQuarter = projectQuarterStr;
            SqlCommand commandObject;

            string databaseParameter = "INSERT INTO [PM].[dbo].[DatabaseParameter]([projectCode],[projectQuarter],[header],[value])VALUES";
            for (int i = 0; i < list.Count; i++)
            {
                databaseParameter = databaseParameter + "('" + projectCode + "','" + projectQuarter + "','" + (list[i])[0] + "','" + (list[i])[1] + "'),";
            }
            databaseParameter = databaseParameter.Substring(0, databaseParameter.Length - 1);
            databaseParameter = databaseParameter + ";";
            connectionObject.Open();
            commandObject = new SqlCommand(databaseParameter, connectionObject);
            commandObject.ExecuteNonQuery();
            connectionObject.Close();
        }

        public void InsertDatabaseConfiguration(string projectCodeStr, string projectQuarterStr, List<object[]> list) {
            string projectCode = projectCodeStr;
            string projectQuarter = projectQuarterStr;
            SqlCommand commandObject;

            string databaseConfiguration = "INSERT INTO [PM].[dbo].[DatabaseConfiguration]([projectCode],[projectQuarter],[header],[value])VALUES";
            for (int i = 0; i < list.Count; i++)
            {
                databaseConfiguration = databaseConfiguration + "('" + projectCode + "','" + projectQuarter + "','" + (list[i])[0] + "','" + (list[i])[1] + "'),";
            }
            databaseConfiguration = databaseConfiguration.Substring(0, databaseConfiguration.Length - 1);
            databaseConfiguration = databaseConfiguration + ";";
            connectionObject.Open();
            commandObject = new SqlCommand(databaseConfiguration, connectionObject);
            commandObject.ExecuteNonQuery();
            connectionObject.Close();
        }

        public void InsertTableSize(string projectCodeStr, string projectQuarterStr, List<object[]> list)
        {
            string projectCode = projectCodeStr;
            string projectQuarter = projectQuarterStr;
            SqlCommand commandObject;

            string tableSize = "INSERT INTO [PM].[dbo].[TablespaceName]([projectCode],[projectQuarter],[tablespaceName],[size])VALUES";
            for (int i = 0; i < list.Count; i++)
            {
                tableSize = tableSize + "('" + projectCode + "','" + projectQuarter + "','" + (list[i])[0] + "','" + (list[i])[1] + "'),";
            }
            tableSize = tableSize.Substring(0, tableSize.Length - 1);
            tableSize = tableSize + ";";
            connectionObject.Open();
            commandObject = new SqlCommand(tableSize, connectionObject);
            commandObject.ExecuteNonQuery();
            connectionObject.Close();
        }

        public void InsertTempTableSize(string projectCodeStr, string projectQuarterStr, List<object[]> list) {
            string projectCode = projectCodeStr;
            string projectQuarter = projectQuarterStr;
            SqlCommand commandObject;

            string tempTableSize = "INSERT INTO [PM].[dbo].[TempTableSize]([projectCode],[projectQuarter],[tempTableSpace],[size])VALUES";
            for (int i = 0; i < list.Count; i++)
            {
                tempTableSize = tempTableSize + "('" + projectCode + "','" + projectQuarter + "','" + (list[i])[0] + "','" + (list[i])[1] + "'),";
            }
            tempTableSize = tempTableSize.Substring(0, tempTableSize.Length - 1);
            tempTableSize = tempTableSize + ";";
            connectionObject.Open();
            commandObject = new SqlCommand(tempTableSize, connectionObject);
            commandObject.ExecuteNonQuery();
            connectionObject.Close();
        }

        public void InsertTablespaceFreespace(string projectCodeStr, string projectQuarterStr, List<object[]> list) {
            string projectCode = projectCodeStr;
            string projectQuarter = projectQuarterStr;
            SqlCommand commandObject;

            string tablespaceFreespace = "INSERT INTO [PM].[dbo].[TablespaceFreespace]([projectCode],[projectQuarter],[tablespaceName],[maxBlocks],[countBlock],[sumFreeBlock],[pct_free]) VALUES";
            for (int i = 0; i < list.Count(); i++)
            {
                tablespaceFreespace = tablespaceFreespace + "('" + projectCode + "','" + projectQuarter + "','" + (list[i])[0] + "','" + (list[i])[1] + "','" + (list[i])[2] + "','" + (list[i])[3] + "','" + (list[i])[4] + "'),";
            }
            tablespaceFreespace = tablespaceFreespace.Substring(0, tablespaceFreespace.Length - 1);
            tablespaceFreespace = tablespaceFreespace + ";";
            connectionObject.Open();
            commandObject = new SqlCommand(tablespaceFreespace, connectionObject);
            commandObject.ExecuteNonQuery();
            connectionObject.Close();
        }

        public void InsertTablespaceAndTempTablespace(string projectCodeStr, string projectQuarterStr, List<object[]> list) {
            string projectCode = projectCodeStr;
            string projectQuarter = projectQuarterStr;
            SqlCommand commandObject;

            string tablespaceAndTempTablespace = "INSERT INTO [PM].[dbo].[TablespaceAndTempTablespace]([projectCode],[projectQuarter],[userName],[defaultTablespace],[temporaryTablespace]) VALUES";
            for (int i = 0; i < list.Count(); i++)
            {
                tablespaceAndTempTablespace = tablespaceAndTempTablespace + "('" + projectCode + "','" + projectQuarter + "','" + (list[i])[0] + "','" + (list[i])[1] + "','" + (list[i])[2] + "'),";
            }
            tablespaceAndTempTablespace = tablespaceAndTempTablespace.Substring(0, tablespaceAndTempTablespace.Length - 1);
            tablespaceAndTempTablespace = tablespaceAndTempTablespace + ";";
            connectionObject.Open();
            commandObject = new SqlCommand(tablespaceAndTempTablespace, connectionObject);
            commandObject.ExecuteNonQuery();
            connectionObject.Close();
        }

        public void InsertDatabaseRegistry(string projectCodeStr,string projectQuarterStr, List<object[]> list) {
            string projectCode = projectCodeStr;
            string projectQuarter = projectQuarterStr;
            SqlCommand commandObject;

            string databaseRegistration = "INSERT INTO [PM].[dbo].[DatabaseRegistry]([projectCode],[projectQuarter],[compId],[version],[status],[lastModified]) VALUES"; 
            for (int i = 0;i < list.Count(); i++)
            {
                databaseRegistration = databaseRegistration + "('" + projectCode + "','" + projectQuarter + "','" + (list[i])[0] + "','" + (list[i])[1] + "','" + (list[i])[2] + "','" + (list[i])[3] + "'),";
            }
            databaseRegistration = databaseRegistration.Substring(0, databaseRegistration.Length - 1);
            databaseRegistration = databaseRegistration + ";";
            connectionObject.Open();
            commandObject = new SqlCommand(databaseRegistration, connectionObject);
            commandObject.ExecuteNonQuery();
            connectionObject.Close();
        }

        public void InsertDatabaseGrowthRate(string projectCodeStr, string projectQuarterStr, object[] list)
        {
            string projectCode = projectCodeStr;
            string projectQuarter = projectQuarterStr;
            SqlCommand commandObject;

            string databaseGrowthRate = "INSERT INTO [PM].[dbo].[DBGrowthRate]([projectCode],[projectQuarter],[allocatedSpace],[usedSpace],[growthDay],[growthMonth]) VALUES";
            databaseGrowthRate = databaseGrowthRate + "('" + projectCode + "','" + projectQuarter + "','" + list[0] + "','" + list[1] + "','" + list[2] + "','" + list[3] + "');";
            connectionObject.Open();
            commandObject = new SqlCommand(databaseGrowthRate, connectionObject);
            commandObject.ExecuteNonQuery();
            connectionObject.Close();
        }
    }
}