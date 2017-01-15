using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class production_pm_info : System.Web.UI.Page
{
    SqlConnection objConn;
    SqlCommand objCmd;
    string projectCoded = "", projectQuarter = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        string strConnString = "Server=localhost;Uid=sa;PASSWORD=08102535;database=PM;Max Pool Size=400;Connect Timeout=600;";
        objConn = new SqlConnection(strConnString);
        objConn.Open();
        string os = "";
        projectCoded = Request.QueryString["project"];
        projectQuarter = Request.QueryString["quarter"];
        

        string chkServerMacSpec = "SELECT * FROM ChkServerMacSpec WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter  + "';"; 
        SqlDataReader reader;
        objCmd = new SqlCommand(chkServerMacSpec, objConn);
        reader = objCmd.ExecuteReader();
        while (reader.Read())
        {
           
                hostname.Text = reader["hostname"].ToString();
                hostname2.Text = reader["hostname"].ToString();
                hostname3.Text = reader["hostname"].ToString();
                hostname4.Text = reader["hostname"].ToString();
                hostname5.Text = reader["hostname"].ToString();
                hostname6.Text = reader["hostname"].ToString();
                ipAddress.Text = reader["ipAddress"].ToString();
                login.Text = reader["oracleOwner_login"].ToString();
                homeDirectory.Text = reader["oracleOwner_homeDirectory"].ToString();
                shell.Text = reader["oracleOwner_shell"].ToString();
                oracleFirstGroup.Text = reader["oracleGroup_firstGroup"].ToString();
                oracleSecondGroup.Text = reader["oracleGroup_secondGroup"].ToString();
        }
        reader.Close();
        reader = null;


        string compareOracleReq = "SELECT * FROM CompareOracleRequirement WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';";
        SqlDataReader reader2;
        objCmd = new SqlCommand(compareOracleReq, objConn);
        reader2 = objCmd.ExecuteReader();
        while (reader2.Read())
        {
            os = reader2["osType"].ToString();
            osInfo.Text = reader2["osInfo"].ToString();
            ram.Text = reader2["osMemSize"].ToString();
            swap.Text = reader2["osSwap"].ToString();
            tmp.Text = reader2["osTmp"].ToString();
            java.Text = reader2["osJava"].ToString();
            kernel.Text = reader2["osKernel"].ToString();
        }
        reader2.Close();
        osType.Text = os;
        osType1.Text = os;
        osType2.Text = os;
        osType3.Text = os;
        osType4.Text = os;

        string diskSpace = "SELECT * FROM OSDiskSpace WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';";
        SqlDataReader reader3;
        objCmd = new SqlCommand(diskSpace, objConn);
        reader3 = objCmd.ExecuteReader();
       
        while (reader3.Read())
        {
            TableRow tRow = new TableRow();
            TableCell fileSystem = new TableCell();
            fileSystem.Text = reader3["fileSystem"].ToString();
            TableCell mbBlocks = new TableCell();
            mbBlocks.Text = reader3["mbBlocks"].ToString();
            TableCell free = new TableCell();
            free.Text = reader3["free"].ToString();
            TableCell percentUsed = new TableCell();
            percentUsed.Text = reader3["percentUsed"].ToString();
            TableCell iUsed = new TableCell();
            iUsed.Text = reader3["iUsed"].ToString();
            TableCell percentIUsed = new TableCell();
            percentIUsed.Text = reader3["percentIUsed"].ToString();
            TableCell mountedOn = new TableCell();
            mountedOn.Text = reader3["mountedOn"].ToString();
            tRow.Cells.Add(fileSystem);
            tRow.Cells.Add(mbBlocks);
            tRow.Cells.Add(free);
            tRow.Cells.Add(percentUsed);
            tRow.Cells.Add(iUsed);
            tRow.Cells.Add(percentIUsed);
            tRow.Cells.Add(mountedOn);
            diskTable.Rows.Add(tRow);

        }
        reader3.Close();


        string userEnv = "SELECT * FROM UserEnvironment WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';";
        SqlDataReader reader4;
        objCmd = new SqlCommand(userEnv, objConn);
        reader4 = objCmd.ExecuteReader();

        while (reader4.Read())
        {
            TableRow tRow = new TableRow();
            TableCell parameter = new TableCell();
            parameter.Text = reader4["header"].ToString();
            TableCell valueEnv = new TableCell();
            valueEnv.Text = reader4["value"].ToString();
            TableCell free = new TableCell();
           
            tRow.Cells.Add(parameter);
            tRow.Cells.Add(valueEnv);
            envTable.Rows.Add(tRow);

        }
        reader4.Close();

        string hardwareConfig = "SELECT * FROM HardwareConfiguration WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';";
        SqlDataReader reader5;
        objCmd = new SqlCommand(hardwareConfig, objConn);
        reader5 = objCmd.ExecuteReader();

        while (reader5.Read())
        {
            systemModel.Text = reader5["systemModel"].ToString();
            machineSerialNumber.Text = reader5["machineSerialNumber"].ToString();
            processorType.Text = reader5["processorType"].ToString();
            processorImplementationMode.Text = reader5["processorImplementationMode"].ToString();
            processorVersion.Text = reader5["processorVersion"].ToString();
            numOfProc.Text = reader5["numOfProc"].ToString();
            cpuType.Text = reader5["cpuType"].ToString();
            kernelType.Text = reader5["kernelType"].ToString();
            ipaddresses.Text = reader5["ipaddress"].ToString();
            subNetMask.Text = reader5["subNetMask"].ToString();
            crontab.Text = reader5["crontab"].ToString();
        }
        reader5.Close();


        /* Datbase Parameter */
        string databaseConfig = "SELECT * FROM DatabaseConfiguration WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';";
        SqlDataReader datbaseConfigReader;
        objCmd = new SqlCommand(databaseConfig, objConn);
        datbaseConfigReader = objCmd.ExecuteReader();
        while (datbaseConfigReader.Read())
        {
            TableRow tRow = new TableRow();
            TableCell parameter = new TableCell();
            parameter.Text = datbaseConfigReader["header"].ToString();
            TableCell valueEnv = new TableCell();
            valueEnv.Text = datbaseConfigReader["value"].ToString();
            TableCell free = new TableCell();

            tRow.Cells.Add(parameter);
            tRow.Cells.Add(valueEnv);
            databaseConfigTable.Rows.Add(tRow);

        }
        datbaseConfigReader.Close();


        /* Temp table space table */
        string tempTabSize = "SELECT * FROM TempTableSize WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';";
        SqlDataReader tempTabSizeReader;
        objCmd = new SqlCommand(tempTabSize, objConn);
        tempTabSizeReader = objCmd.ExecuteReader();


        Table subTableTempsize = new Table();
        TableRow mainRowTempsize = new TableRow();
        TableCell mainHeaderTempSize = new TableCell();
        mainHeaderTempSize.Text = "Temp tablespace size";
        TableCell mainValueTempSize = new TableCell();
        mainValueTempSize.Controls.Add(subTableTempsize);
        mainRowTempsize.Cells.Add(mainHeaderTempSize);
        mainRowTempsize.Cells.Add(mainValueTempSize);
        databaseConfigTable.Rows.AddAt(5, mainRowTempsize);

        
        subTableTempsize.ID = "subTempTable";
        subTableTempsize.CssClass = "table table-striped table-bordered";

        TableRow tRowHead = new TableRow();
        TableHeaderCell head1 = new TableHeaderCell();
        head1.Text = "Temporary tablespace name";
        TableHeaderCell head2 = new TableHeaderCell();
        head2.Text = "Size(Mb)";
        tRowHead.Cells.Add(head1);
        tRowHead.Cells.Add(head2);
        subTableTempsize.Rows.Add(tRowHead);

        while (tempTabSizeReader.Read())
        {
            TableRow tRow = new TableRow();
            TableCell tempTableSpaceName = new TableCell();
            tempTableSpaceName.Text = tempTabSizeReader["tempTableSpace"].ToString();
            TableCell tempSize = new TableCell();
            tempSize.Text = tempTabSizeReader["size"].ToString();
            TableCell free = new TableCell();
            
            
            tRow.Cells.Add(tempTableSpaceName);
            tRow.Cells.Add(tempSize);
            subTableTempsize.Rows.Add(tRow);


        }
        tempTabSizeReader.Close();


        /* Table Space Name*/

        string tabspaceName = "SELECT * FROM TablespaceName WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';";
        SqlDataReader tabspaceNameReader;
        objCmd = new SqlCommand(tabspaceName, objConn);
        tabspaceNameReader = objCmd.ExecuteReader();

        Table subTableTableSpaceName = new Table();

        TableRow mainRowTable = new TableRow();

        TableCell mainHeaderTableSpaceName = new TableCell();
        mainHeaderTableSpaceName.Text = "Tablespace size";

        TableCell mainValueTableSpaceName = new TableCell();
        mainValueTableSpaceName.Controls.Add(subTableTableSpaceName);

        mainRowTable.Cells.Add(mainHeaderTableSpaceName);
        mainRowTable.Cells.Add(mainValueTableSpaceName);

        databaseConfigTable.Rows.AddAt(6, mainRowTable);


        subTableTableSpaceName.ID = "subTableName";
        subTableTableSpaceName.CssClass = "table table-striped table-bordered";


        TableRow tRowHeadTableSpaceName = new TableRow();

        TableHeaderCell temphead1TableSpaceName = new TableHeaderCell();
        temphead1TableSpaceName.Text = "Tablespace name";

        TableHeaderCell temphead2TableSpaceName = new TableHeaderCell();
        temphead2TableSpaceName.Text = "Size(Mb)";

        tRowHeadTableSpaceName.Cells.Add(temphead1TableSpaceName);
        tRowHeadTableSpaceName.Cells.Add(temphead2TableSpaceName);
        subTableTableSpaceName.Rows.Add(tRowHeadTableSpaceName);

        while (tabspaceNameReader.Read())
        {
            TableRow tRow = new TableRow();
            TableCell tempTableSpaceName = new TableCell();
            tempTableSpaceName.Text = tabspaceNameReader["tablespaceName"].ToString();
            TableCell tempSize = new TableCell();
            tempSize.Text = tabspaceNameReader["size"].ToString();
            TableCell free = new TableCell();


            tRow.Cells.Add(tempTableSpaceName);
            tRow.Cells.Add(tempSize);
            subTableTableSpaceName.Rows.Add(tRow);
        }
        tabspaceNameReader.Close();

        /* Database Parameter*/
        string databaseParameterStr = "SELECT * FROM DatabaseParameter WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';";
        SqlDataReader databaseParameterReader;
        objCmd = new SqlCommand(databaseParameterStr, objConn);
        databaseParameterReader = objCmd.ExecuteReader();

        while (databaseParameterReader.Read())
        {
            TableRow tRow = new TableRow();
            TableCell parameter = new TableCell();
            parameter.Text = databaseParameterReader["header"].ToString();
            TableCell valueParam = new TableCell();
            valueParam.Text = databaseParameterReader["value"].ToString();
            TableCell free = new TableCell();

            tRow.Cells.Add(parameter);
            tRow.Cells.Add(valueParam);
            databaseParameter.Rows.Add(tRow);
            
        }
        databaseParameterReader.Close();


        /* Major Security Initailization Parameters*/
        string majorSecureStr = "SELECT * FROM DatabaseParameter WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "'";
        majorSecureStr = majorSecureStr + " AND ( header = 'O7_DICTIONARY_ACCESSIBILITY' OR header = 'audit_trail' OR header = 'remote_login_passwordfile' OR header = 'remote_os_authent') ;";
        SqlDataReader majorSecureReader;
        objCmd = new SqlCommand(majorSecureStr, objConn);
        majorSecureReader = objCmd.ExecuteReader();

        while (majorSecureReader.Read())
        {
            TableRow tRow = new TableRow();
            TableCell parameter = new TableCell();
            parameter.Text = majorSecureReader["header"].ToString();
            TableCell valueParam = new TableCell();
            valueParam.Text = majorSecureReader["value"].ToString();
            TableCell free = new TableCell();

            tRow.Cells.Add(parameter);
            tRow.Cells.Add(valueParam);
            majorSecure.Rows.Add(tRow);

        }
        majorSecureReader.Close();


        /* 4.4 Database File */
        string databaseFileStr = "SELECT * FROM DatabaseFile WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';";
        SqlDataReader databaseFileStrReader;
        objCmd = new SqlCommand(databaseFileStr, objConn);
        databaseFileStrReader = objCmd.ExecuteReader();

        while (databaseFileStrReader.Read())
        {
            TableRow tRow = new TableRow();

            TableCell val1 = new TableCell();
            val1.Text = databaseFileStrReader["tbsName"].ToString();
            TableCell val2 = new TableCell();
            val2.Text = databaseFileStrReader["fileName"].ToString();
            TableCell val3 = new TableCell();
            val3.Text = databaseFileStrReader["size"].ToString();
            TableCell val4 = new TableCell();
            val4.Text = databaseFileStrReader["max"].ToString();
            TableCell val5 = new TableCell();
            val5.Text = databaseFileStrReader["aut"].ToString();
            TableCell val6 = new TableCell();
            val6.Text = databaseFileStrReader["inc"].ToString();


            tRow.Cells.Add(val1);
            tRow.Cells.Add(val2);
            tRow.Cells.Add(val3);
            tRow.Cells.Add(val4);
            tRow.Cells.Add(val5);
            tRow.Cells.Add(val6);
            databaseFileTable.Rows.Add(tRow);

        }
        databaseFileStrReader.Close();

        /* 4.5 Temporary File */
        string tempFileStr = "SELECT * FROM TempFile WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';";
        SqlDataReader tempFileStrReader;
        objCmd = new SqlCommand(databaseFileStr, objConn);
        tempFileStrReader = objCmd.ExecuteReader();

        while (tempFileStrReader.Read())
        {
            TableRow tRow = new TableRow();

            TableCell val1 = new TableCell();
            val1.Text = tempFileStrReader["tbsName"].ToString();
            TableCell val2 = new TableCell();
            val2.Text = tempFileStrReader["fileName"].ToString();
            TableCell val3 = new TableCell();
            val3.Text = tempFileStrReader["size"].ToString();
            TableCell val4 = new TableCell();
            val4.Text = tempFileStrReader["max"].ToString();
            TableCell val5 = new TableCell();
            val5.Text = tempFileStrReader["aut"].ToString();
            TableCell val6 = new TableCell();
            val6.Text = tempFileStrReader["inc"].ToString();


            tRow.Cells.Add(val1);
            tRow.Cells.Add(val2);
            tRow.Cells.Add(val3);
            tRow.Cells.Add(val4);
            tRow.Cells.Add(val5);
            tRow.Cells.Add(val6);
            temporaryTable.Rows.Add(tRow);

        }
        tempFileStrReader.Close();

        /* 4.5 Redo Log File */
        string redoLogFileStr = "SELECT * FROM RedoLogFile WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';";
        SqlDataReader redoLogFileStrReader;
        objCmd = new SqlCommand(redoLogFileStr, objConn);
        redoLogFileStrReader = objCmd.ExecuteReader();

        while (redoLogFileStrReader.Read())
        {
            TableRow tRow = new TableRow();

            TableCell val1 = new TableCell();
            val1.Text = redoLogFileStrReader["groupMember"].ToString();
            TableCell val2 = new TableCell();
            val2.Text = redoLogFileStrReader["member"].ToString();
            TableCell val3 = new TableCell();
            val3.Text = redoLogFileStrReader["size"].ToString();
            


            tRow.Cells.Add(val1);
            tRow.Cells.Add(val2);
            tRow.Cells.Add(val3);
            redoLogFileTable.Rows.Add(tRow);

        }
        redoLogFileStrReader.Close();


        /* 4.7 Control File */
        string controlFileStr = "SELECT * FROM ControlFile WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';";
        SqlDataReader controlFileStrReader;
        objCmd = new SqlCommand(controlFileStr, objConn);
        controlFileStrReader = objCmd.ExecuteReader();

        while (controlFileStrReader.Read())
        {
            TableRow tRow = new TableRow();

            TableCell val1 = new TableCell();
            val1.Text = controlFileStrReader["controlFileName"].ToString();
           
            tRow.Cells.Add(val1);
            controlFileTable.Rows.Add(tRow);

        }
        controlFileStrReader.Close();

        /* 4.8 Daily Calendar */
        string dayCalstr = "SELECT * FROM DailyCalendarWorksheet WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';";
        SqlDataReader dayCalReader;
        objCmd = new SqlCommand(dayCalstr, objConn);
        dayCalReader = objCmd.ExecuteReader();

        while (dayCalReader.Read())
        {
            TableRow tRow = new TableRow();

            TableCell val1 = new TableCell();
            val1.Text = dayCalReader["timeDay"].ToString();
            TableCell val2 = new TableCell();
            val2.Text = dayCalReader["descOfHouse"].ToString();
            TableCell val3 = new TableCell();
            val3.Text = dayCalReader["estimatedDuration"].ToString();

            tRow.Cells.Add(val1);
            tRow.Cells.Add(val2);
            tRow.Cells.Add(val3);
            dayCalendar.Rows.Add(tRow);
        }
        dayCalReader.Close();

        /* 4.9 Monthly Calendar */
        string monthCalstr = "SELECT * FROM MonthlyCalendarWorksheet WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';";
        SqlDataReader monthCalReader;
        objCmd = new SqlCommand(monthCalstr, objConn);
        monthCalReader = objCmd.ExecuteReader();

        while (monthCalReader.Read())
        {
            TableRow tRow = new TableRow();

            TableCell val1 = new TableCell();
            val1.Text = monthCalReader["dayProject"].ToString();
            TableCell val2 = new TableCell();
            val2.Text = monthCalReader["descpOfHouse"].ToString();
            TableCell val3 = new TableCell();
            val3.Text = monthCalReader["estimatedDur"].ToString();

            tRow.Cells.Add(val1);
            tRow.Cells.Add(val2);
            tRow.Cells.Add(val3);
            monthCalendar.Rows.Add(tRow);
        }
        monthCalReader.Close();

        /* Perfomance Review */
        string perfRevice = "SELECT * FROM performanceReview WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';";
        SqlDataReader perfReviewReader;
        objCmd = new SqlCommand(perfRevice, objConn);
        perfReviewReader = objCmd.ExecuteReader();

        while (perfReviewReader.Read())
        {
            TableRow tRow = new TableRow();
            TableCell parameter = new TableCell();
            parameter.Text = perfReviewReader["header"].ToString();
            TableCell valueEnv = new TableCell();
            valueEnv.Text = perfReviewReader["value"].ToString();
            TableCell free = new TableCell();

            tRow.Cells.Add(parameter);
            tRow.Cells.Add(valueEnv);
            perfReview.Rows.Add(tRow);

        }
        perfReviewReader.Close();

        Table hitRatio = new Table();

        TableRow hitRatioHeader = new TableRow();

        TableCell hitCell1 = new TableCell();
        hitCell1.Text = "1. What is the gethitratio of the librarycache ?";
        TableCell hitCell2 = new TableCell();
        hitCell2.Controls.Add(hitRatio);

        hitRatio.CssClass = "table table-striped table-bordered";
        hitRatioHeader.Cells.Add(hitCell1);
        hitRatioHeader.Cells.Add(hitCell2);
        perfReview.Rows.AddAt(1, hitRatioHeader);

        string getHitRatio = "SELECT * FROM getHitRatio WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';";
        SqlDataReader getHitRatioReader;
        objCmd = new SqlCommand(getHitRatio, objConn);
        getHitRatioReader = objCmd.ExecuteReader();

        TableRow tRow1 = new TableRow();
        TableHeaderCell parameter1 = new TableHeaderCell();
        parameter1.Text = "Name Space";
        TableHeaderCell value1 = new TableHeaderCell();
        value1.Text = "Get Hit Ratio";

        tRow1.Cells.Add(parameter1);
        tRow1.Cells.Add(value1);
        hitRatio.Rows.Add(tRow1);

        while (getHitRatioReader.Read())
        {
            TableRow tRow = new TableRow();
            TableCell parameter = new TableCell();
            parameter.Text = getHitRatioReader["nameSpace"].ToString();
            TableCell valueEnv = new TableCell();
            valueEnv.Text = getHitRatioReader["getHitRatio"].ToString();
            TableCell free = new TableCell();

            tRow.Cells.Add(parameter);
            tRow.Cells.Add(valueEnv);
            hitRatio.Rows.Add(tRow);

        }
        getHitRatioReader.Close();

        /* 2 Pin Ratio */

        Table pinRatio = new Table();

        TableRow pinRatioHeader = new TableRow();

        TableCell pinCell1 = new TableCell();
        pinCell1.Text = "2. What is the PIN / RELOAD ratio within the librarycache?";
        TableCell pinCell2 = new TableCell();
        pinCell2.Controls.Add(pinRatio);

        pinRatio.CssClass = "table table-striped table-bordered";
        pinRatioHeader.Cells.Add(pinCell1);
        pinRatioHeader.Cells.Add(pinCell2);
        perfReview.Rows.AddAt(2, pinRatioHeader);

        string getPinRatio = "SELECT * FROM PinRatio WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';";
        SqlDataReader getPinRatioReader;
        objCmd = new SqlCommand(getPinRatio, objConn);
        getPinRatioReader = objCmd.ExecuteReader();

        TableRow tRow2 = new TableRow();
        TableHeaderCell parameter2 = new TableHeaderCell();
        parameter2.Text = "Execution";
        TableHeaderCell value2 = new TableHeaderCell();
        value2.Text = "Cache Misses";
        TableHeaderCell value3 = new TableHeaderCell();
        value3.Text = "Sum";

        tRow2.Cells.Add(parameter2);
        tRow2.Cells.Add(value2);
        tRow2.Cells.Add(value3);
        pinRatio.Rows.Add(tRow2);

        while (getPinRatioReader.Read())
        {
            TableRow tRow = new TableRow();
            TableCell value4 = new TableCell();
            value4.Text = getPinRatioReader["execution"].ToString();
            TableCell value5 = new TableCell();
            value5.Text = getPinRatioReader["cacheMisses"].ToString();
            TableCell value6 = new TableCell();
            value6.Text = getPinRatioReader["sumPin"].ToString();

            tRow.Cells.Add(value4);
            tRow.Cells.Add(value5);
            tRow.Cells.Add(value6);
            pinRatio.Rows.Add(tRow);

        }
        getPinRatioReader.Close();


        /* 3 Undo Segment */

        Table segmentTable = new Table();

        TableRow segmentTableHeader = new TableRow();

        TableCell segCell1 = new TableCell();
        segCell1.Text = "Number and size of Undo Segments?";
        TableCell segCell2 = new TableCell();
        segCell2.Controls.Add(segmentTable);

        segmentTable.CssClass = "table table-striped table-bordered";
        segmentTableHeader.Cells.Add(segCell1);
        segmentTableHeader.Cells.Add(segCell2);
        perfReview.Rows.AddAt(15, segmentTableHeader);

        string gesegmentTable = "SELECT * FROM undoSegmentsSize WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';";
        SqlDataReader gesegmentTableReader;
        objCmd = new SqlCommand(gesegmentTable, objConn);
        gesegmentTableReader = objCmd.ExecuteReader();

        TableRow tRow3 = new TableRow();
        TableHeaderCell value7 = new TableHeaderCell();
        value7.Text = "Amount";
        TableHeaderCell value8 = new TableHeaderCell();
        value8.Text = "Segment Type";
        TableHeaderCell value9 = new TableHeaderCell();
        value9.Text = "Size (Mb)";

        tRow3.Cells.Add(value7);
        tRow3.Cells.Add(value8);
        tRow3.Cells.Add(value9);
        segmentTable.Rows.Add(tRow3);

        while (gesegmentTableReader.Read())
        {
            TableRow tRow = new TableRow();
            TableCell value4 = new TableCell();
            value4.Text = gesegmentTableReader["amount"].ToString();
            TableCell value5 = new TableCell();
            value5.Text = gesegmentTableReader["segmentType"].ToString();
            TableCell value6 = new TableCell();
            value6.Text = gesegmentTableReader["size"].ToString();

            tRow.Cells.Add(value4);
            tRow.Cells.Add(value5);
            tRow.Cells.Add(value6);
            segmentTable.Rows.Add(tRow);

        }
        gesegmentTableReader.Close();

        /* 6.1 Tablespace Free Space */
        string tbsFreeStr = "SELECT * FROM TablespaceFreespace WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';";
        SqlDataReader tbsFreeStrReader;
        objCmd = new SqlCommand(tbsFreeStr, objConn);
        tbsFreeStrReader = objCmd.ExecuteReader();

        while (tbsFreeStrReader.Read())
        {
            TableRow tRow = new TableRow();

            TableCell val1 = new TableCell();
            val1.Text = tbsFreeStrReader["tablespaceName"].ToString();
            TableCell val2 = new TableCell();
            val2.Text = tbsFreeStrReader["maxBlocks"].ToString();
            TableCell val3 = new TableCell();
            val3.Text = tbsFreeStrReader["countBlock"].ToString();
            TableCell val4 = new TableCell();
            val4.Text = tbsFreeStrReader["sumFreeBlock"].ToString();
            TableCell val5 = new TableCell();
            val5.Text = tbsFreeStrReader["pct_free"].ToString();

            tRow.Cells.Add(val1);
            tRow.Cells.Add(val2);
            tRow.Cells.Add(val3);
            tRow.Cells.Add(val4);
            tRow.Cells.Add(val5);
            freespaceTable.Rows.Add(tRow);

        }
        tbsFreeStrReader.Close();

        /* 7.1 Default Tablespace Free Space */
        string defTbsFreeStr = "SELECT * FROM TablespaceAndTempTablespace WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';";
        SqlDataReader defTbsFreeStrReader;
        objCmd = new SqlCommand(defTbsFreeStr, objConn);
        defTbsFreeStrReader = objCmd.ExecuteReader();

        while (defTbsFreeStrReader.Read())
        {
            TableRow tRow = new TableRow();

            TableCell val1 = new TableCell();
            val1.Text = defTbsFreeStrReader["userName"].ToString();
            TableCell val2 = new TableCell();
            val2.Text = defTbsFreeStrReader["defaultTablespace"].ToString();
            TableCell val3 = new TableCell();
            val3.Text = defTbsFreeStrReader["temporaryTablespace"].ToString();

            tRow.Cells.Add(val1);
            tRow.Cells.Add(val2);
            tRow.Cells.Add(val3);
            defAndTemp.Rows.Add(tRow);

        }
        defTbsFreeStrReader.Close();

        /* 8.1 Database Registry */
        string dbRegStr = "SELECT * FROM DatabaseRegistry WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';";
        SqlDataReader dbRegStrReader;
        objCmd = new SqlCommand(dbRegStr, objConn);
        dbRegStrReader = objCmd.ExecuteReader();

        while (dbRegStrReader.Read())
        {
            TableRow tRow = new TableRow();

            TableCell val1 = new TableCell();
            val1.Text = dbRegStrReader["compId"].ToString();
            TableCell val2 = new TableCell();
            val2.Text = dbRegStrReader["version"].ToString();
            TableCell val3 = new TableCell();
            val3.Text = dbRegStrReader["status"].ToString();
            TableCell val4 = new TableCell();
            val4.Text = dbRegStrReader["lastModified"].ToString();

            tRow.Cells.Add(val1);
            tRow.Cells.Add(val2);
            tRow.Cells.Add(val3);
            tRow.Cells.Add(val4);
            databaseRegistry.Rows.Add(tRow);

        }
        dbRegStrReader.Close();

    }
}