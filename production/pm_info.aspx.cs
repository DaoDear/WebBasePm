using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using WebBasePM;

public partial class production_pm_info : System.Web.UI.Page
{   
    string projectCoded = "", projectQuarter = "";
    protected DatabaseHelper dbHelper;

    protected void Page_Load(object sender, EventArgs e)
    {
        dbHelper = new DatabaseHelper();       
        string os = "";
        projectCoded = Request.QueryString["project"];
        projectQuarter = Request.QueryString["quarter"];

        /*  Check server    */
        object[] chkServerObj = dbHelper.GetSingleQueryObject("SELECT * FROM ChkServerMacSpec WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");

        hostname.Text = chkServerObj[2].ToString();
        hostname2.Text = chkServerObj[2].ToString();
        hostname3.Text = chkServerObj[2].ToString();
        hostname4.Text = chkServerObj[2].ToString();
        hostname5.Text = chkServerObj[2].ToString();
        hostname6.Text = chkServerObj[2].ToString();

        ipAddress.Text = chkServerObj[3].ToString();
        login.Text = chkServerObj[4].ToString();
        homeDirectory.Text = chkServerObj[5].ToString();
        shell.Text = chkServerObj[6].ToString();
        oracleFirstGroup.Text = chkServerObj[7].ToString();
        oracleSecondGroup.Text = chkServerObj[8].ToString(); 
        
        /*  Oracle Requirement  */
        object[] oracleReqObj = dbHelper.GetSingleQueryObject("SELECT * FROM CompareOracleRequirement WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
       
        os = oracleReqObj[3].ToString();
        osInfo.Text = oracleReqObj[4].ToString();
        ram.Text = oracleReqObj[5].ToString();
        swap.Text = oracleReqObj[6].ToString();
        tmp.Text = oracleReqObj[7].ToString();
        java.Text = oracleReqObj[8].ToString();
        kernel.Text = oracleReqObj[9].ToString();
        
        osType.Text = os;
        osType1.Text = os;
        osType2.Text = os;
        osType3.Text = os;
        osType4.Text = os;
       
        List<object[]> diskSpaceObj = dbHelper.GetMultiQueryObject("SELECT * FROM OSDiskSpace WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
        for (int row = 0; row < diskSpaceObj.Count(); row++)
        {
            TableRow tRow = new TableRow();
            TableCell fileSystem = new TableCell();
            TableCell mbBlocks = new TableCell();
            TableCell free = new TableCell();
            TableCell percentUsed = new TableCell();
            TableCell iUsed = new TableCell();
            TableCell percentIUsed = new TableCell();
            TableCell mountedOn = new TableCell();

            fileSystem.Text = (diskSpaceObj[row])[3].ToString();
            mbBlocks.Text = (diskSpaceObj[row])[4].ToString();
            free.Text = (diskSpaceObj[row])[5].ToString();
            percentUsed.Text = (diskSpaceObj[row])[6].ToString();
            iUsed.Text = (diskSpaceObj[row])[7].ToString();
            percentIUsed.Text = (diskSpaceObj[row])[8].ToString();
            mountedOn.Text = (diskSpaceObj[row])[9].ToString();

            tRow.Cells.Add(fileSystem);
            tRow.Cells.Add(mbBlocks);
            tRow.Cells.Add(free);
            tRow.Cells.Add(percentUsed);
            tRow.Cells.Add(iUsed);
            tRow.Cells.Add(percentIUsed);
            tRow.Cells.Add(mountedOn);
            diskTable.Rows.Add(tRow);
        }


        List<object[]> userEnvObj = new List<object[]>();
        userEnvObj = dbHelper.GetMultiQueryObject("SELECT * FROM UserEnvironment WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
        
        for(int row = 0; row < userEnvObj.Count(); row++) { 
            TableRow tRow = new TableRow();
            TableCell parameter = new TableCell();            
            TableCell valueEnv = new TableCell();
            parameter.Text = (userEnvObj[row])[2].ToString();
            valueEnv.Text = (userEnvObj[row])[3].ToString();

            tRow.Cells.Add(parameter);
            tRow.Cells.Add(valueEnv);
            envTable.Rows.Add(tRow);
        }

        /*  Hardware Configuration  */
        List<object[]> hardwareObj = dbHelper.GetMultiQueryObject("SELECT * FROM HardwareConfiguration WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
        for(int row = 0; row < hardwareObj.Count(); row++)
        {
            systemModel.Text = (hardwareObj[row])[3].ToString();
            machineSerialNumber.Text = (hardwareObj[row])[4].ToString();
            processorType.Text = (hardwareObj[row])[5].ToString();
            processorImplementationMode.Text = (hardwareObj[row])[6].ToString();
            processorVersion.Text = (hardwareObj[row])[7].ToString();
            numOfProc.Text = (hardwareObj[row])[8].ToString();
            cpuType.Text = (hardwareObj[row])[9].ToString();
            kernelType.Text = (hardwareObj[row])[10].ToString();
            ipaddresses.Text = (hardwareObj[row])[11].ToString();
            subNetMask.Text = (hardwareObj[row])[12].ToString();
            crontab.Text = (hardwareObj[row])[13].ToString();
        }

        /* Datbase Configuration */
        List<object[]> databaseConfigObj = dbHelper.GetMultiQueryObject("SELECT * FROM DatabaseConfiguration WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
        for(int row = 0; row < databaseConfigObj.Count(); row++)
        {
            TableRow tRow = new TableRow();
            TableCell parameter = new TableCell();            
            TableCell valueEnv = new TableCell();            
            TableCell free = new TableCell();

            parameter.Text = (databaseConfigObj[row])[2].ToString();
            valueEnv.Text = (databaseConfigObj[row])[3].ToString();

            tRow.Cells.Add(parameter);
            tRow.Cells.Add(valueEnv);
            databaseConfigTable.Rows.Add(tRow);
        }


        /* Temp table space table */       
        Table subTableTempsize = new Table();
        TableRow mainRowTempsize = new TableRow();
        TableCell mainHeaderTempSize = new TableCell();        
        TableCell mainValueTempSize = new TableCell();
        mainHeaderTempSize.Text = "Temp tablespace size";
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

        List<object[]> tempTableSizeObj = dbHelper.GetMultiQueryObject("SELECT * FROM TempTableSize WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
        if (tempTableSizeObj != null)
        {
            for (int row = 0; row < tempTableSizeObj.Count(); row++)
            {
                TableRow tRow = new TableRow();
                TableCell tempTableSpaceName = new TableCell();
                TableCell tempSize = new TableCell();
                TableCell free = new TableCell();

                tempTableSpaceName.Text = (tempTableSizeObj[row])[2].ToString();
                tempSize.Text = (tempTableSizeObj[row])[3].ToString();

                tRow.Cells.Add(tempTableSpaceName);
                tRow.Cells.Add(tempSize);
                subTableTempsize.Rows.Add(tRow);
            }
        }

        /* Table Space Name */
        Table subTableTableSpaceName = new Table();
        TableRow mainRowTable = new TableRow();
        TableCell mainHeaderTableSpaceName = new TableCell();
        TableCell mainValueTableSpaceName = new TableCell();

        mainHeaderTableSpaceName.Text = "Tablespace size";
        mainValueTableSpaceName.Controls.Add(subTableTableSpaceName);
        mainRowTable.Cells.Add(mainHeaderTableSpaceName);
        mainRowTable.Cells.Add(mainValueTableSpaceName);
        databaseConfigTable.Rows.AddAt(6, mainRowTable);
        subTableTableSpaceName.ID = "subTableName";
        subTableTableSpaceName.CssClass = "table table-striped table-bordered";

        TableRow tRowHeadTableSpaceName = new TableRow();
        TableHeaderCell temphead1TableSpaceName = new TableHeaderCell();
        TableHeaderCell temphead2TableSpaceName = new TableHeaderCell();
        temphead1TableSpaceName.Text = "Tablespace name";
        temphead2TableSpaceName.Text = "Size(Mb)";

        tRowHeadTableSpaceName.Cells.Add(temphead1TableSpaceName);
        tRowHeadTableSpaceName.Cells.Add(temphead2TableSpaceName);
        subTableTableSpaceName.Rows.Add(tRowHeadTableSpaceName);

        List<object[]> tempTableSpaceObj = dbHelper.GetMultiQueryObject("SELECT * FROM TablespaceName WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
        if (tempTableSpaceObj != null)
        {
            for (int row = 0; row < tempTableSpaceObj.Count(); row++)
            {
                TableRow tRow = new TableRow();
                TableCell tempTableSpaceName = new TableCell();
                TableCell tempSize = new TableCell();

                tempTableSpaceName.Text = (tempTableSpaceObj[row])[2].ToString();
                tempSize.Text = (tempTableSpaceObj[row])[3].ToString();

                tRow.Cells.Add(tempTableSpaceName);
                tRow.Cells.Add(tempSize);
                subTableTableSpaceName.Rows.Add(tRow);
            }
        }
        /* Database Parameter*/
        List<object[]> databaseParameterObj = dbHelper.GetMultiQueryObject("SELECT * FROM DatabaseParameter WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
        if (databaseParameterObj != null)
        {
            for (int row = 0; row < databaseParameterObj.Count(); row++)
            {
                TableRow tRow = new TableRow();
                TableCell parameter = new TableCell();
                TableCell valueParam = new TableCell();
                TableCell free = new TableCell();

                parameter.Text = (databaseParameterObj[row])[2].ToString();
                valueParam.Text = (databaseParameterObj[row])[3].ToString();

                tRow.Cells.Add(parameter);
                tRow.Cells.Add(valueParam);
                databaseParameter.Rows.Add(tRow);
            }
        }

        /* Major Security Initailization Parameters*/
        string majorSecureStr = "SELECT * FROM DatabaseParameter WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "'";
        majorSecureStr = majorSecureStr + " AND ( header = 'O7_DICTIONARY_ACCESSIBILITY' OR header = 'audit_trail' OR header = 'remote_login_passwordfile' OR header = 'remote_os_authent') ;";
        List<object[]> majorSecureObj = dbHelper.GetMultiQueryObject(majorSecureStr);
        if (majorSecureObj != null)
        {
            for (int row = 0; row < majorSecureObj.Count(); row++)
            {
                TableRow tRow = new TableRow();
                TableCell parameter = new TableCell();
                TableCell valueParam = new TableCell();
                TableCell free = new TableCell();

                parameter.Text = (majorSecureObj[row])[2].ToString();
                valueParam.Text = (majorSecureObj[row])[3].ToString();

                tRow.Cells.Add(parameter);
                tRow.Cells.Add(valueParam);
                majorSecure.Rows.Add(tRow);
            }
        }

        /* 4.4 Database File */
        List<object[]> databaseFileObj = dbHelper.GetMultiQueryObject("SELECT * FROM DatabaseFile WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
        if (databaseFileObj != null)
        {
            for (int row = 0; row < databaseFileObj.Count(); row++)
            {
                TableRow tRow = new TableRow();
                TableCell val1 = new TableCell();
                TableCell val2 = new TableCell();
                TableCell val3 = new TableCell();
                TableCell val4 = new TableCell();
                TableCell val5 = new TableCell();
                TableCell val6 = new TableCell();

                val1.Text = (databaseFileObj[row])[2].ToString();
                val2.Text = (databaseFileObj[row])[3].ToString();
                val3.Text = (databaseFileObj[row])[4].ToString();
                val4.Text = (databaseFileObj[row])[5].ToString();
                val5.Text = (databaseFileObj[row])[6].ToString();
                val6.Text = (databaseFileObj[row])[7].ToString();

                tRow.Cells.Add(val1);
                tRow.Cells.Add(val2);
                tRow.Cells.Add(val3);
                tRow.Cells.Add(val4);
                tRow.Cells.Add(val5);
                tRow.Cells.Add(val6);
                databaseFileTable.Rows.Add(tRow);
            }
        }

        /* 4.5 Temporary File */
        List<object[]> tempFileObj = dbHelper.GetMultiQueryObject("SELECT * FROM TempFile WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
        if (tempFileObj != null)
        {
            for (int row = 0; row < tempFileObj.Count(); row++)
            {
                TableRow tRow = new TableRow();
                TableCell val1 = new TableCell();
                TableCell val2 = new TableCell();
                TableCell val3 = new TableCell();
                TableCell val4 = new TableCell();
                TableCell val5 = new TableCell();
                TableCell val6 = new TableCell();

                val1.Text = (tempFileObj[row])[2].ToString();
                val2.Text = (tempFileObj[row])[3].ToString();
                val3.Text = (tempFileObj[row])[4].ToString();
                val4.Text = (tempFileObj[row])[5].ToString();
                val5.Text = (tempFileObj[row])[6].ToString();
                val6.Text = (tempFileObj[row])[7].ToString();

                tRow.Cells.Add(val1);
                tRow.Cells.Add(val2);
                tRow.Cells.Add(val3);
                tRow.Cells.Add(val4);
                tRow.Cells.Add(val5);
                tRow.Cells.Add(val6);
                temporaryTable.Rows.Add(tRow);
            }
        }

        /* 4.5 Redo Log File */
        List<object[]> redoLogObj = dbHelper.GetMultiQueryObject("SELECT * FROM RedoLogFile WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
        if (redoLogObj != null)
        {
            for (int row = 0; row < redoLogObj.Count(); row++)
            {
                TableRow tRow = new TableRow();
                TableCell val1 = new TableCell();
                TableCell val2 = new TableCell();
                TableCell val3 = new TableCell();

                val1.Text = (redoLogObj[row])[2].ToString();
                val2.Text = (redoLogObj[row])[3].ToString();
                val3.Text = (redoLogObj[row])[4].ToString();

                tRow.Cells.Add(val1);
                tRow.Cells.Add(val2);
                tRow.Cells.Add(val3);
                redoLogFileTable.Rows.Add(tRow);
            }
        }

        /* 4.7 Control File */
        List<object[]> controlObj = dbHelper.GetMultiQueryObject("SELECT * FROM ControlFile WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
        if (controlObj != null)
        {
            for (int row = 0; row < controlObj.Count(); row++)
            {
                TableRow tRow = new TableRow();
                TableCell val1 = new TableCell();

                val1.Text = (controlObj[row])[2].ToString();

                tRow.Cells.Add(val1);
                controlFileTable.Rows.Add(tRow);
            }
        }

        /* 4.8 Daily Calendar */
        List<object[]> dayCalObj = dbHelper.GetMultiQueryObject("SELECT * FROM DailyCalendarWorksheet WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
        if (dayCalObj != null)
        {
            for (int row = 0; row < dayCalObj.Count(); row++)
            {
                TableRow tRow = new TableRow();
                TableCell val1 = new TableCell();
                TableCell val2 = new TableCell();
                TableCell val3 = new TableCell();

                val1.Text = (dayCalObj[row])[2].ToString();
                val2.Text = (dayCalObj[row])[3].ToString();
                val3.Text = (dayCalObj[row])[4].ToString();

                tRow.Cells.Add(val1);
                tRow.Cells.Add(val2);
                tRow.Cells.Add(val3);
                dayCalendar.Rows.Add(tRow);
            }
        }
        /* 4.9 Monthly Calendar */
        List<object[]> monthCalObj = dbHelper.GetMultiQueryObject("SELECT * FROM MonthlyCalendarWorksheet WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
        if (monthCalObj != null)
        {
            for (int row = 0; row < monthCalObj.Count(); row++)
            {
                TableRow tRow = new TableRow();
                TableCell val1 = new TableCell();
                TableCell val2 = new TableCell();
                TableCell val3 = new TableCell();

                val1.Text = (monthCalObj[row])[2].ToString();
                val2.Text = (monthCalObj[row])[3].ToString();
                val3.Text = (monthCalObj[row])[4].ToString();

                tRow.Cells.Add(val1);
                tRow.Cells.Add(val2);
                tRow.Cells.Add(val3);
                monthCalendar.Rows.Add(tRow);
            }
        }

        /* Perfomance Review */
        List<object[]> perfReviceObj = dbHelper.GetMultiQueryObject("SELECT * FROM performanceReview WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
        for(int row = 0; row < perfReviceObj.Count(); row++)
        {
            TableRow tRow = new TableRow();
            TableCell parameter = new TableCell();
            TableCell valueEnv = new TableCell();

            parameter.Text = (perfReviceObj[row])[2].ToString();            
            valueEnv.Text  = (perfReviceObj[row])[3].ToString();

            tRow.Cells.Add(parameter);
            tRow.Cells.Add(valueEnv);
            perfReview.Rows.Add(tRow);

        }


        Table hitRatio = new Table();
        TableRow hitRatioHeader = new TableRow();
        TableCell hitCell1 = new TableCell();        
        TableCell hitCell2 = new TableCell();
        hitCell1.Text = "1. What is the gethitratio of the librarycache ?";
        hitCell2.Controls.Add(hitRatio);

        hitRatio.CssClass = "table table-striped table-bordered";
        hitRatioHeader.Cells.Add(hitCell1);
        hitRatioHeader.Cells.Add(hitCell2);
        perfReview.Rows.AddAt(1, hitRatioHeader);

        List<object[]> getHitRatioObj = dbHelper.GetMultiQueryObject("SELECT * FROM getHitRatio WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
       
        TableRow tRow1 = new TableRow();
        TableHeaderCell parameter1 = new TableHeaderCell();
        parameter1.Text = "Name Space";
        TableHeaderCell value1 = new TableHeaderCell();
        value1.Text = "Get Hit Ratio";

        tRow1.Cells.Add(parameter1);
        tRow1.Cells.Add(value1);
        hitRatio.Rows.Add(tRow1);

        for(int row = 0; row < getHitRatioObj.Count(); row++)
        {
            TableRow tRow = new TableRow();
            TableCell parameter = new TableCell();
            TableCell valueEnv = new TableCell();

            parameter.Text = (getHitRatioObj[row])[2].ToString();           
            valueEnv.Text =  (getHitRatioObj[row])[3].ToString();

            tRow.Cells.Add(parameter);
            tRow.Cells.Add(valueEnv);
            hitRatio.Rows.Add(tRow);
        }

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

        List<object[]> pinRatioObj = dbHelper.GetMultiQueryObject("SELECT * FROM PinRatio WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");

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
        if (pinRatioObj != null)
        {
            for (int row = 0; row < pinRatioObj.Count(); row++)
            {
                TableRow tRow = new TableRow();
                TableCell value4 = new TableCell();
                TableCell value5 = new TableCell();
                TableCell value6 = new TableCell();

                value4.Text = (pinRatioObj[row])[2].ToString();
                value5.Text = (pinRatioObj[row])[3].ToString();
                value6.Text = (pinRatioObj[row])[4].ToString();

                tRow.Cells.Add(value4);
                tRow.Cells.Add(value5);
                tRow.Cells.Add(value6);
                pinRatio.Rows.Add(tRow);
            }
        }

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

        List<object[]> segmentObj = dbHelper.GetMultiQueryObject("SELECT * FROM undoSegmentsSize WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
       
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
        if (segmentObj != null)
        {
            for (int row = 0; row < segmentObj.Count(); row++)
            {
                TableRow tRow = new TableRow();
                TableCell value4 = new TableCell();
                TableCell value5 = new TableCell();
                TableCell value6 = new TableCell();

                value4.Text = (segmentObj[row])[2].ToString();
                value5.Text = (segmentObj[row])[3].ToString();
                value6.Text = (segmentObj[row])[4].ToString();

                tRow.Cells.Add(value4);
                tRow.Cells.Add(value5);
                tRow.Cells.Add(value6);
                segmentTable.Rows.Add(tRow);
            }
        }

        /* 6.1 Tablespace Free Space */
        List<object[]> tbsFreeObj = dbHelper.GetMultiQueryObject("SELECT * FROM TablespaceFreespace WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
        if (tbsFreeObj != null)
        {
            for (int row = 0; row < tbsFreeObj.Count(); row++)
            {
                TableRow tRow = new TableRow();
                TableCell val1 = new TableCell();
                TableCell val2 = new TableCell();
                TableCell val3 = new TableCell();
                TableCell val4 = new TableCell();
                TableCell val5 = new TableCell();

                val1.Text = (tbsFreeObj[row])[2].ToString();
                val2.Text = (tbsFreeObj[row])[3].ToString();
                val3.Text = (tbsFreeObj[row])[4].ToString();
                val4.Text = (tbsFreeObj[row])[5].ToString();
                val5.Text = (tbsFreeObj[row])[6].ToString();

                tRow.Cells.Add(val1);
                tRow.Cells.Add(val2);
                tRow.Cells.Add(val3);
                tRow.Cells.Add(val4);
                tRow.Cells.Add(val5);
                freespaceTable.Rows.Add(tRow);
            }
        }
            
        /* 7.1 Default Tablespace Free Space */
        List<object[]> defTbsFreeObj = dbHelper.GetMultiQueryObject("SELECT * FROM TablespaceAndTempTablespace WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
        if (defTbsFreeObj != null)
        {
            for (int row = 0; row < defTbsFreeObj.Count(); row++)
            {
                TableRow tRow = new TableRow();
                TableCell val1 = new TableCell();
                TableCell val2 = new TableCell();
                TableCell val3 = new TableCell();

                val1.Text = (defTbsFreeObj[row])[2].ToString();
                val2.Text = (defTbsFreeObj[row])[3].ToString();
                val3.Text = (defTbsFreeObj[row])[4].ToString();

                tRow.Cells.Add(val1);
                tRow.Cells.Add(val2);
                tRow.Cells.Add(val3);
                defAndTemp.Rows.Add(tRow);
            }
        }

        /* 8.1 Database Registry */
        List<object[]> dbRegObj = dbHelper.GetMultiQueryObject("SELECT * FROM DatabaseRegistry WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
        if (dbRegObj != null)
        {
            for (int row = 0; row < dbRegObj.Count(); row++)
            {
                TableRow tRow = new TableRow();

                TableCell val1 = new TableCell();
                TableCell val2 = new TableCell();
                TableCell val3 = new TableCell();
                TableCell val4 = new TableCell();

                val1.Text = (dbRegObj[row])[2].ToString();
                val2.Text = (dbRegObj[row])[3].ToString();
                val3.Text = (dbRegObj[row])[4].ToString();
                val4.Text = (dbRegObj[row])[5].ToString();

                tRow.Cells.Add(val1);
                tRow.Cells.Add(val2);
                tRow.Cells.Add(val3);
                tRow.Cells.Add(val4);
                databaseRegistry.Rows.Add(tRow);
            }
        }

        /*  BACKUP SECTION   */
        object[] backupFIle = dbHelper.GetSingleQueryObject("SELECT * FROM backupfile WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
        if (backupFIle != null)
        {
            backupDB.Text = backupFIle[4].ToString();
        }
        else {
            backupDB.Text = "";
        }

        if (backupFIle != null)
        {
            backupAL.Text = backupFIle[3].ToString();
        }
        else
        {
            backupAL.Text = "";
        }

        if (backupFIle != null)
        {
            backupCF.Text = backupFIle[2].ToString();
        }
        else
        {
            backupCF.Text = "";
        }

        Object[] alertObj = dbHelper.GetSingleQueryObject("SELECT * FROM alert WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
        if (alertObj != null)
        {
            alertMsg.Text = alertObj[2].ToString();
        }

        /*  DATABASE GROWTH RATE  */
        Object[] obj = dbHelper.GetSingleQueryObject("SELECT * FROM DBGrowthRate WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
        if (obj != null)
        {
            double allocateSpace = double.Parse(obj[2].ToString());
            double usedSpace = double.Parse(obj[3].ToString());
            double growthDay = double.Parse(obj[4].ToString());
            double growthMonth = double.Parse(obj[5].ToString());
            double UsedGrowth = growthDay * 30;
            double AllowGrowth = growthMonth * 30;

            Chart1.Series.Clear();
            Chart1.Series.Add("Series1");
            Chart1.Series["Series1"].Points.AddXY(1, usedSpace);
            Chart1.Series["Series1"].Points.AddXY(2, usedSpace += UsedGrowth);
            Chart1.Series["Series1"].Points.AddXY(3, usedSpace += UsedGrowth);
            Chart1.Series["Series1"].Points.AddXY(4, usedSpace += UsedGrowth);
            Chart1.Series["Series1"].ChartType = SeriesChartType.Line;
            Chart1.Series["Series1"].BorderWidth = 3;

            Chart1.Series.Add("Series2");
            Chart1.Series["Series2"].Points.AddXY(1, allocateSpace);
            Chart1.Series["Series2"].Points.AddXY(2, allocateSpace += AllowGrowth);
            Chart1.Series["Series2"].Points.AddXY(3, allocateSpace += AllowGrowth);
            Chart1.Series["Series2"].Points.AddXY(4, allocateSpace += AllowGrowth);
            Chart1.Series["Series2"].ChartType = SeriesChartType.Line;
            Chart1.Series["Series2"].BorderWidth = 3;

            Chart1.ChartAreas[0].AxisX.Interval = 1;
            Chart1.Width = 500;
        }
    }
}