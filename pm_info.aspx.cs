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
using iTextSharp;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;

public partial class production_pm_info : System.Web.UI.Page
{   
    string projectCoded = "", projectQuarter = "";
    protected DatabaseHelper dbHelper;

    protected void Page_Load(object sender, EventArgs e)
    {

        Session["PreviousPage"] = Request.Url.AbsoluteUri;

        if (Session["personID"] == null)
        {
            Response.Redirect("Login.aspx");
            
        }

        nameHeader.Text = Session["Name"].ToString();
        nameHeader2.Text = Session["Name"].ToString();

        dbHelper = new DatabaseHelper();       
        string os = "";
        projectCoded = Request.QueryString["project"];
        projectQuarter = Request.QueryString["quarter"];

        if (projectCoded == null || projectQuarter == null) {
            if (ViewState["PreviousPage"] != null) //check if the webpage is loaded for the first time.
            {
                var prevState = ViewState["PreviousPage"].ToString();
                ViewState["PreviousPage"] = Request.Url.AbsoluteUri; //Saves the Previous page url in ViewState
                Response.Redirect(prevState);
            }
            else {
                Response.Redirect("index.aspx");
            }
        }

        object[] pminfo = dbHelper.GetSingleQueryObject("SELECT * FROM PmInfo WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
        hostTitle.Text = pminfo[2] + " " + pminfo[3] + " " + pminfo[4];
        alertDatabaeName.Text = pminfo[8].ToString();

        object[] saleObj = dbHelper.GetSingleQueryObject("SELECT * FROM Personinfo WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "' AND personType = 'Sale';");
        object[] cusObj = dbHelper.GetSingleQueryObject("SELECT * FROM Personinfo WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "' AND personType = 'Customer';");
        object[] engObj = dbHelper.GetSingleQueryObject("SELECT * FROM Personinfo WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "' AND personType = 'Engineer';");

        projectName.Text = pminfo[3].ToString();
        projectCode.Text = pminfo[0].ToString();
        if (saleObj != null)
        {
            salePerson.Text = saleObj[3].ToString() + " " + saleObj[4].ToString();
            salePhone.Text = saleObj[6].ToString();
            saleEmail.Text = saleObj[5].ToString();
        }

        if (cusObj != null)
        {
            cusName.Text = cusObj[3].ToString() + " " + cusObj[4].ToString();
            cusPhone.Text = cusObj[6].ToString();
            cusEmail.Text = cusObj[5].ToString();
        }

        if (engObj != null)
        {
            engName.Text = engObj[3].ToString() + " " + engObj[4].ToString();
            engPhone.Text = engObj[6].ToString();
            engEmail.Text = engObj[5].ToString();
        }

        List<object[]> author = dbHelper.GetMultiQueryObject("SELECT *  FROM AuthorLog WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
        if (author != null)
        {
            for (int row = 0; row < author.Count(); row++)
            {
                TableRow tRow = new TableRow();
                TableCell dateCreated = new TableCell();
                TableCell authorName = new TableCell();
                TableCell version = new TableCell();
                TableCell refference = new TableCell();

                dateCreated.Text = (author[row])[3].ToString();
                authorName.Text = (author[row])[4].ToString();
                version.Text = (author[row])[5].ToString();
                refference.Text = (author[row])[6].ToString();


                tRow.Cells.Add(dateCreated);
                tRow.Cells.Add(authorName);
                tRow.Cells.Add(version);
                tRow.Cells.Add(refference);
                authorTable.Rows.Add(tRow);
            }
        }

        List<object[]> reviewer = dbHelper.GetMultiQueryObject("SELECT *  FROM reviewerLog WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
        if (reviewer != null)
        {
            for (int row = 0; row < reviewer.Count(); row++)
            {
                TableRow tRow = new TableRow();
                TableCell reviewDate = new TableCell();
                TableCell reviewName = new TableCell();
                TableCell position = new TableCell();

                reviewDate.Text = (reviewer[row])[3].ToString();
                reviewName.Text = (reviewer[row])[4].ToString();
                position.Text = (reviewer[row])[5].ToString();


                tRow.Cells.Add(reviewDate);
                tRow.Cells.Add(reviewName);
                tRow.Cells.Add(position);
                reviewerTable.Rows.Add(tRow);
            }
        }

        /*  Check server    */
        object[] chkServerObj = dbHelper.GetSingleQueryObject("SELECT * FROM ChkServerMacSpec WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");

        hostname.Text  = chkServerObj[2].ToString();
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
                
        List<object[]> userEnvObj = userEnvObj = dbHelper.GetMultiQueryObject("SELECT * FROM UserEnvironment WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
        
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
            ipaddresses.Text = (hardwareObj[row])[12].ToString();
            subNetMask.Text = (hardwareObj[row])[13].ToString();
            crontab.Text = (hardwareObj[row])[14].ToString();
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

        /* 4.6 Redo Log File */
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

        List<object[]> listOfAlert = dbHelper.GetMultiQueryObject("SELECT * FROM AlertLog WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
        Panel alertpanel = (Panel)alertLogPanel;
        if (listOfAlert != null)
        {
            for (int i = 0; i < listOfAlert.Count; i++)
            {
                Table alertTable = new Table();
                TableRow tRowHeader = new TableRow();
                TableRow tRowA1 = new TableRow();
                TableRow tRowA2 = new TableRow();
                TableRow tRowA3 = new TableRow();

                TableCell alertTopic = new TableCell();
                alertTopic.Text = "Key search: " + (listOfAlert[i])[2].ToString();
                tRowHeader.Cells.Add(alertTopic);
                tRowHeader.Cells[0].ColumnSpan = 3;
                alertTable.Rows.Add(tRowHeader);

                TableCell cause = new TableCell();
                TableCell action = new TableCell();
                TableCell score = new TableCell();


                TableCell causeTopic = new TableCell();
                TableCell actionTopic = new TableCell();
                TableCell scoreTopic = new TableCell();

                cause.Text = (listOfAlert[i])[3].ToString();
                action.Text = (listOfAlert[i])[4].ToString();
                score.Text = (listOfAlert[i])[5].ToString();

                causeTopic.Text = "Caused";
                actionTopic.Text = "Action";
                scoreTopic.Text = "Score";

                tRowA1.Cells.Add(causeTopic);
                tRowA1.Cells.Add(cause);
                tRowA2.Cells.Add(actionTopic);
                tRowA2.Cells.Add(action);
                tRowA3.Cells.Add(scoreTopic);
                tRowA3.Cells.Add(score);

                alertTable.Rows.Add(tRowA1);
                alertTable.Rows.Add(tRowA2);
                alertTable.Rows.Add(tRowA3);

                alertpanel.Controls.Add(alertTable);

                alertTable.CssClass = "table table-striped table-bordered";
                tRowA1.Cells[0].Width = 200;
            }
        }
        /*  DATABASE GROWTH RATE  */
        Object[] obj = dbHelper.GetSingleQueryObject("SELECT * FROM DBGrowthRate WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
        if (obj != null)
        {

            List<double> allocateSpaceList = new List<double>();
            List<double> usedSpaceList = new List<double>();

            double allocateSpaceInit = double.Parse(obj[2].ToString());
            double usedSpaceInit = double.Parse(obj[3].ToString());
            double growthDay = double.Parse(obj[4].ToString());
            double growthMonth = double.Parse(obj[5].ToString());
            double UsedGrowth = growthDay * 30;
            double AllowGrowth = growthMonth * 30;

            allocateSpaceList.Add(allocateSpaceInit);
            usedSpaceList.Add(usedSpaceInit);

            for (int i = 0; i < 3; i++) {                
                allocateSpaceList.Add(allocateSpaceInit += AllowGrowth);
                usedSpaceList.Add(usedSpaceInit += UsedGrowth);
            }

            Chart1.Series.Clear();
            Chart1.Series.Add("Series1");
            Chart1.Series["Series1"].Points.AddXY(1, usedSpaceList[0]);
            Chart1.Series["Series1"].Points.AddXY(2, usedSpaceList[1]);
            Chart1.Series["Series1"].Points.AddXY(3, usedSpaceList[2]);
            Chart1.Series["Series1"].Points.AddXY(4, usedSpaceList[3]);
            Chart1.Series["Series1"].ChartType = SeriesChartType.Line;
            Chart1.Series["Series1"].BorderWidth = 3;

            Chart1.Series.Add("Series2");
            Chart1.Series["Series2"].Points.AddXY(1, allocateSpaceList[0]);
            Chart1.Series["Series2"].Points.AddXY(2, allocateSpaceList[1]);
            Chart1.Series["Series2"].Points.AddXY(3, allocateSpaceList[2]);
            Chart1.Series["Series2"].Points.AddXY(4, allocateSpaceList[3]);
            Chart1.Series["Series2"].ChartType = SeriesChartType.Line;
            Chart1.Series["Series2"].BorderWidth = 3;

            Chart1.ChartAreas[0].AxisX.Interval = 1;
            Chart1.Width = 500;

            string summarytxt = "Current size of database is ${datasize} GB. We compare data growth rate between current allocate data which is ${dataallocate} and total use data per month ${totalused} GB (The percentage of data growth per month is around ${datagrowth}%), ${databasename} database growth rate increasing around ${increaserate} GB per quarter (The percentage is around ${increaseratepercent}%).";
            summarytxt += "As the result of database size forecast, the current allocate size of database is sufficient to support database growth rate in next quarter. The percentage of usage space is around ${percentused}%, it’s nearly 100% so you should concern about data growth in the future because maybe cause space of database is insufficient.";

            summarytxt = summarytxt.Replace("${datasize}", String.Format("{0:0,0.00}", usedSpaceList[0]));
            summarytxt = summarytxt.Replace("${dataallocate}", String.Format("{0:0,0.00}", allocateSpaceList[0]));
            summarytxt = summarytxt.Replace("${totalused}", String.Format("{0:0,0.00}", usedSpaceList[1] - usedSpaceList[0]));
            summarytxt = summarytxt.Replace("${datagrowth}", String.Format("{0:0.0}", (UsedGrowth*100)/ usedSpaceInit));
            summarytxt = summarytxt.Replace("${increaseratepercent}", String.Format("{0:0.0}", (((usedSpaceList[1] - usedSpaceList[0]) * 3) *100)/ usedSpaceInit));
            summarytxt = summarytxt.Replace("${percentused}", String.Format("{0:0.00}", (usedSpaceList[0]*100)/allocateSpaceList[0]));
            summarytxt = summarytxt.Replace("${increaserate}", String.Format("{0:0,0.0}", (usedSpaceList[1] - usedSpaceList[0])*3));
            summarytxt = summarytxt.Replace("${databasename}", pminfo[8].ToString());

            DatabaseNameLabel.Text = pminfo[8].ToString();
            DatabaseNameLabel2.Text = pminfo[8].ToString();
            currAllocated.Text = allocateSpaceList[0].ToString();
            currUsed.Text = usedSpaceList[0].ToString();
            percUsed.Text = String.Format("{0:0.00}", ((usedSpaceList[0] * 100) / allocateSpaceList[0]));
            allocGrowth.Text = growthDay.ToString();
            usedGrowth.Text = growthMonth.ToString();
            SummaryLabel.Text = summarytxt;

        }
    }

    protected void exportButton_Click(object sender, EventArgs e)
    {
        DatabaseHelper dbHelper = new DatabaseHelper();
        // Create a Document object
        var document = new Document(PageSize.A4, 50, 50, 25, 25);

        // Create a new PdfWriter object, specifying the output stream
        string timeStamp = GetTimestamp(DateTime.Now);
        string tempPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory.ToString(), "TEMP", "FILES_" + timeStamp);
        Directory.CreateDirectory(tempPath);
        string filePath = Path.Combine(tempPath, "mfec_report_" + projectCoded+".pdf");
        var output = new FileStream(filePath, FileMode.Create);
        var writer = PdfWriter.GetInstance(document, output);

        var titleFont = FontFactory.GetFont("Microsoft Sans Serif", 18, Font.BOLD, BaseColor.BLUE);
        var subTitleFont = FontFactory.GetFont("Microsoft Sans Serif", 14, Font.BOLD);
        var boldTableFont = FontFactory.GetFont("Microsoft Sans Serif", 12, Font.BOLD);
        var endingMessageFont = FontFactory.GetFont("Microsoft Sans Serif", 10, Font.ITALIC);
        var bodyFont = FontFactory.GetFont("Microsoft Sans Serif", 12, Font.NORMAL);


        object[] pminfo = dbHelper.GetSingleQueryObject("SELECT * FROM PmInfo WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
        object[] saleObj = dbHelper.GetSingleQueryObject("SELECT * FROM Personinfo WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "' AND personType = 'Sale';");
        object[] cusObj = dbHelper.GetSingleQueryObject("SELECT * FROM Personinfo WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "' AND personType = 'Customer';");
        object[] engObj = dbHelper.GetSingleQueryObject("SELECT * FROM Personinfo WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "' AND personType = 'Engineer';");
        List<object[]> author = dbHelper.GetMultiQueryObject("SELECT *  FROM AuthorLog WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
        List<object[]> reviewer = dbHelper.GetMultiQueryObject("SELECT *  FROM reviewerLog WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");


        // Open the Document for writing
        document.Open();
        document.Add(new Paragraph("1. General Project Information", titleFont));
        Paragraph p = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLUE, Element.ALIGN_LEFT, 0.8F)));
        document.Add(p);
        document.Add(new Paragraph("1.1 Project Information", subTitleFont));               
        var pminfoPdf = new PdfPTable(2);
        pminfoPdf.DefaultCell.BorderWidth = 1f;
        pminfoPdf.HorizontalAlignment = 0;
        pminfoPdf.SpacingBefore = 15;
        pminfoPdf.SpacingAfter = 15;
        pminfoPdf.SetWidths(new int[] { 3, 5 });

        pminfoPdf.AddCell(new Phrase("Project Name:", boldTableFont));
        pminfoPdf.AddCell(new Phrase(pminfo[3].ToString() , bodyFont));
        pminfoPdf.AddCell(new Phrase("Project No:", boldTableFont));
        pminfoPdf.AddCell(new Phrase(pminfo[0].ToString(), bodyFont));
        if (saleObj != null)
        {
            pminfoPdf.AddCell(new Phrase("Sale Name:", boldTableFont));
            pminfoPdf.AddCell(new Phrase(saleObj[3].ToString() ?? "" + " " + saleObj[4].ToString() ?? "", bodyFont));
            pminfoPdf.AddCell(new Phrase("Phone Number:", boldTableFont));
            pminfoPdf.AddCell(new Phrase(saleObj[6].ToString() ?? "", bodyFont));
            pminfoPdf.AddCell(new Phrase("Email Address:", boldTableFont));
            pminfoPdf.AddCell(new Phrase(saleObj[5].ToString() ?? "", bodyFont));
        }
        document.Add(pminfoPdf);

        var cusInfoPdf = new PdfPTable(2);
        cusInfoPdf.HorizontalAlignment = 0;
        cusInfoPdf.SpacingBefore = 15;
        cusInfoPdf.SpacingAfter = 15;
        cusInfoPdf.DefaultCell.BorderWidth = 1f;
        cusInfoPdf.SetWidths(new int[] { 3, 5 });

        document.Add(new Paragraph("1.2 Customer Contact Information", subTitleFont));
        if (cusObj != null)
        {
            cusInfoPdf.AddCell(new Phrase("Customer Name:", boldTableFont));
            cusInfoPdf.AddCell(new Phrase(cusObj[3].ToString() ?? "" + " " + cusObj[4].ToString() ?? "", bodyFont));
            cusInfoPdf.AddCell(new Phrase("Phone Number:", boldTableFont));
            cusInfoPdf.AddCell(new Phrase(cusObj[6].ToString() ?? "", bodyFont));
            cusInfoPdf.AddCell(new Phrase("Email Address:", boldTableFont));
            cusInfoPdf.AddCell(new Phrase(cusObj[5].ToString() ?? "", bodyFont));
        }
        document.Add(cusInfoPdf);


        var engInfoPdf = new PdfPTable(2);
        engInfoPdf.HorizontalAlignment = 0;
        engInfoPdf.SpacingBefore = 15;
        engInfoPdf.SpacingAfter = 15;
        engInfoPdf.DefaultCell.BorderWidth = 1f;
        engInfoPdf.SetWidths(new int[] { 3, 5 });

        document.Add(new Paragraph("1.3 MFEC Engineers' Information", subTitleFont));
        if (cusObj != null)
        {
            engInfoPdf.AddCell(new Phrase("Engineer Name:", boldTableFont));
            engInfoPdf.AddCell(new Phrase(engObj[3].ToString() ?? "" + " " + engObj[4].ToString() ?? "", boldTableFont));
            engInfoPdf.AddCell(new Phrase("Phone Number:", bodyFont));
            engInfoPdf.AddCell(new Phrase(engObj[6].ToString() ?? "", boldTableFont));
            engInfoPdf.AddCell(new Phrase("Email Address:", boldTableFont));
            engInfoPdf.AddCell(new Phrase(engObj[5].ToString() ?? "", bodyFont));
        }
        document.Add(engInfoPdf);

        var changeInfo = new PdfPTable(4);
        changeInfo.HorizontalAlignment = 0;
        changeInfo.SpacingBefore = 15;
        changeInfo.SpacingAfter = 15;
        changeInfo.DefaultCell.BorderWidth = 1f;
        changeInfo.SetWidths(new int[] { 3, 5, 2, 4 });

        document.Add(new Paragraph("1.4 Change Record", subTitleFont));
        if (author != null)
        {
            changeInfo.AddCell(new Phrase("Date", boldTableFont));
            changeInfo.AddCell(new Phrase("Author", boldTableFont));
            changeInfo.AddCell(new Phrase("Version", boldTableFont));
            changeInfo.AddCell(new Phrase("Change Reference", boldTableFont));
            for (int i = 0; i < author.Count; i++) {
                changeInfo.AddCell(new Phrase((author[i])[3].ToString(), bodyFont));
                changeInfo.AddCell(new Phrase((author[i])[4].ToString(), bodyFont));
                changeInfo.AddCell(new Phrase((author[i])[5].ToString(), bodyFont));
                changeInfo.AddCell(new Phrase((author[i])[6].ToString(), bodyFont));
            }

        }
        document.Add(changeInfo);


        var reviewInfo = new PdfPTable(3);
        reviewInfo.HorizontalAlignment = 0;
        reviewInfo.SpacingBefore = 15;
        reviewInfo.SpacingAfter = 15;
        reviewInfo.DefaultCell.BorderWidth = 1f;
        reviewInfo.SetWidths(new int[] { 3, 5, 4 });

        document.Add(new Paragraph("1.5 Reviewers", subTitleFont));
        if (reviewer != null)
        {
            reviewInfo.AddCell(new Phrase("Date", boldTableFont));
            reviewInfo.AddCell(new Phrase("Name", boldTableFont));
            reviewInfo.AddCell(new Phrase("Postition", boldTableFont));
            for (int i = 0; i < author.Count; i++)
            {
                reviewInfo.AddCell(new Phrase((reviewer[i])[3].ToString(), bodyFont));
                reviewInfo.AddCell(new Phrase((reviewer[i])[4].ToString(), bodyFont));
                reviewInfo.AddCell(new Phrase((reviewer[i])[5].ToString(), bodyFont));
            }

        }
        document.Add(reviewInfo);

        document.Add(new Paragraph("2. Oracle minimum requirement", titleFont));
        Paragraph p2 = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLUE, Element.ALIGN_LEFT, 0.8F)));
        document.Add(p2);
        document.Add(new Paragraph("2.1 Checking server machine specification", subTitleFont));

        object[] chkServerObj = dbHelper.GetSingleQueryObject("SELECT * FROM ChkServerMacSpec WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");

        var csms = new PdfPTable(2);
        csms.HorizontalAlignment = 0;
        csms.SpacingBefore = 15;
        csms.SpacingAfter = 15;
        csms.DefaultCell.BorderWidth = 1f;
        csms.SetWidths(new int[] { 3, 5 });

        if (csms != null)
        {
            csms.AddCell(new Phrase("", boldTableFont));
            csms.AddCell(new Phrase("Values", boldTableFont));
            csms.AddCell(new Phrase("Hostname", boldTableFont));
            csms.AddCell(new Phrase(chkServerObj[2].ToString(), bodyFont));
            csms.AddCell(new Phrase("IP Address", boldTableFont));
            csms.AddCell(new Phrase(chkServerObj[3].ToString(), bodyFont));

            var oracleOwner = new PdfPTable(3);
            oracleOwner.HorizontalAlignment = 0;
            oracleOwner.SpacingBefore = 15;
            oracleOwner.SpacingAfter = 15;
            oracleOwner.DefaultCell.BorderWidth = 1f;
            oracleOwner.SetWidths(new int[] { 3, 5, 5 });
            oracleOwner.AddCell(new Phrase("Login", boldTableFont));
            oracleOwner.AddCell(new Phrase("Home's directory", boldTableFont));
            oracleOwner.AddCell(new Phrase("Shell", boldTableFont));
            oracleOwner.AddCell(new Phrase(chkServerObj[4].ToString(), bodyFont));
            oracleOwner.AddCell(new Phrase(chkServerObj[5].ToString(), bodyFont));
            oracleOwner.AddCell(new Phrase(chkServerObj[6].ToString(), bodyFont));
            csms.AddCell(new Phrase("Oracle Owner", boldTableFont));
            csms.AddCell(oracleOwner);

            var oracleGroup = new PdfPTable(2);
            oracleGroup.HorizontalAlignment = 0;
            oracleGroup.SpacingBefore = 15;
            oracleGroup.SpacingAfter = 15;
            oracleGroup.DefaultCell.BorderWidth = 1f;
            oracleGroup.SetWidths(new int[] { 3, 5 });
            oracleGroup.AddCell(new Phrase("First group", boldTableFont));
            oracleGroup.AddCell(new Phrase("Second group", boldTableFont));
            oracleGroup.AddCell(new Phrase(chkServerObj[7].ToString(), bodyFont));
            oracleGroup.AddCell(new Phrase(chkServerObj[8].ToString(), bodyFont));
            csms.AddCell(new Phrase("Oracle Group", boldTableFont));
            csms.AddCell(oracleGroup);
        }
        document.Add(csms);
        document.Add(new Paragraph("2.2 Compare to Oracle requirement for " + chkServerObj[2].ToString(), subTitleFont));
        object[] oracleReqObj = dbHelper.GetSingleQueryObject("SELECT * FROM CompareOracleRequirement WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");

        var oraReq = new PdfPTable(2);
        oraReq.HorizontalAlignment = 0;
        oraReq.SpacingBefore = 15;
        oraReq.SpacingAfter = 15;
        oraReq.DefaultCell.BorderWidth = 1f;
        oraReq.SetWidths(new int[] { 3, 5 });

        oraReq.AddCell(new Phrase("Requirement", boldTableFont));
        oraReq.AddCell(new Phrase("Server Specification", boldTableFont));
        oraReq.AddCell(new Phrase(oracleReqObj[3].ToString()+" OS:", boldTableFont));
        oraReq.AddCell(new Phrase(oracleReqObj[4].ToString(), bodyFont));

        List<object[]> diskSpaceObj = dbHelper.GetMultiQueryObject("SELECT * FROM OSDiskSpace WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
        var dspo = new PdfPTable(7);
        dspo.HorizontalAlignment = 0;
        dspo.SpacingBefore = 15;
        dspo.SpacingAfter = 15;
        dspo.DefaultCell.BorderWidth = 1f;
        dspo.SetWidths(new int[] { 3,3,3,3,3,3,3 });

        dspo.AddCell(new Phrase("Filesystem", boldTableFont));
        dspo.AddCell(new Phrase("MB blocks", boldTableFont));
        dspo.AddCell(new Phrase("Free", boldTableFont));
        dspo.AddCell(new Phrase("%Used", boldTableFont));
        dspo.AddCell(new Phrase("Iused", boldTableFont));
        dspo.AddCell(new Phrase("%Iused", boldTableFont));
        dspo.AddCell(new Phrase("Mounted on", boldTableFont));
        if (diskSpaceObj.Count > 0) {
            for (int i = 0; i < diskSpaceObj.Count; i++) {
                dspo.AddCell(new Phrase((diskSpaceObj[i])[3].ToString(), bodyFont));
                dspo.AddCell(new Phrase((diskSpaceObj[i])[4].ToString(), bodyFont));
                dspo.AddCell(new Phrase((diskSpaceObj[i])[5].ToString(), bodyFont));
                dspo.AddCell(new Phrase((diskSpaceObj[i])[6].ToString(), bodyFont));
                dspo.AddCell(new Phrase((diskSpaceObj[i])[7].ToString(), bodyFont));
                dspo.AddCell(new Phrase((diskSpaceObj[i])[8].ToString(), bodyFont));
                dspo.AddCell(new Phrase((diskSpaceObj[i])[9].ToString(), bodyFont));
            }
        }
        oraReq.AddCell(new Phrase(oracleReqObj[3].ToString() + " Disk Space:", boldTableFont));
        oraReq.AddCell(dspo);
        oraReq.AddCell(new Phrase(oracleReqObj[3].ToString() + " RAM:", boldTableFont));
        oraReq.AddCell(new Phrase(oracleReqObj[5].ToString(), bodyFont));
        oraReq.AddCell(new Phrase(oracleReqObj[3].ToString() + " Swap:", boldTableFont));
        oraReq.AddCell(new Phrase(oracleReqObj[6].ToString(), bodyFont));
        oraReq.AddCell(new Phrase(oracleReqObj[3].ToString() + " Tmp:", boldTableFont));
        oraReq.AddCell(new Phrase(oracleReqObj[7].ToString(), bodyFont));
        oraReq.AddCell(new Phrase("JAVA", boldTableFont));
        oraReq.AddCell(new Phrase(oracleReqObj[8].ToString(), bodyFont));
        oraReq.AddCell(new Phrase("Kernel", boldTableFont));
        oraReq.AddCell(new Phrase(oracleReqObj[9].ToString(), bodyFont));
        document.Add(oraReq);

        document.Add(new Paragraph("2.3 User's environment for " + chkServerObj[2].ToString(), subTitleFont));
        List<object[]> userEnvObj = dbHelper.GetMultiQueryObject("SELECT * FROM UserEnvironment WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");

        var ueo = new PdfPTable(2);
        ueo.HorizontalAlignment = 0;
        ueo.SpacingBefore = 15;
        ueo.SpacingAfter = 15;
        ueo.DefaultCell.BorderWidth = 1f;
        ueo.SetWidths(new int[] { 3, 5 });

        ueo.AddCell(new Phrase("Parameter", boldTableFont));
        ueo.AddCell(new Phrase("Value", boldTableFont));
        if (userEnvObj.Count > 0) {
            for (int i = 0; i < userEnvObj.Count; i++) {
                ueo.AddCell(new Phrase((userEnvObj[i])[2].ToString(), bodyFont));
                ueo.AddCell(new Phrase((userEnvObj[i])[3].ToString(), bodyFont));
            }
        }
        document.Add(ueo);

        document.Add(new Paragraph("3. System Checklist", titleFont));
        Paragraph p3 = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLUE, Element.ALIGN_LEFT, 0.8F)));
        document.Add(p3);
        document.Add(new Paragraph("3.1 Hardware configuration for " + chkServerObj[2].ToString(), subTitleFont));
        object[] hardwareObj = dbHelper.GetSingleQueryObject("SELECT * FROM HardwareConfiguration WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");

        var systemCheck = new PdfPTable(2);
        systemCheck.HorizontalAlignment = 0;
        systemCheck.SpacingBefore = 15;
        systemCheck.SpacingAfter = 15;
        systemCheck.DefaultCell.BorderWidth = 1f;
        systemCheck.SetWidths(new int[] { 3, 5 });

        systemCheck.AddCell(new Phrase("", boldTableFont));
        systemCheck.AddCell(new Phrase("Values", boldTableFont));
        if (hardwareObj != null) {
                systemCheck.AddCell(new Phrase("System Model", bodyFont));
                systemCheck.AddCell(new Phrase(hardwareObj[3].ToString(), bodyFont));
                systemCheck.AddCell(new Phrase("Machine Serial Number", bodyFont));
                systemCheck.AddCell(new Phrase(hardwareObj[4].ToString(), bodyFont));
                systemCheck.AddCell(new Phrase("Processor Type", bodyFont));
                systemCheck.AddCell(new Phrase(hardwareObj[5].ToString(), bodyFont));
                systemCheck.AddCell(new Phrase("Processor Implementation Mode", bodyFont));
                systemCheck.AddCell(new Phrase(hardwareObj[6].ToString(), bodyFont));
                systemCheck.AddCell(new Phrase("Processor Version", bodyFont));
                systemCheck.AddCell(new Phrase(hardwareObj[7].ToString(), bodyFont));
                systemCheck.AddCell(new Phrase("Number Of Processors", bodyFont));
                systemCheck.AddCell(new Phrase(hardwareObj[8].ToString(), bodyFont));
                systemCheck.AddCell(new Phrase("Processor Clock Speed", bodyFont));
                systemCheck.AddCell(new Phrase(hardwareObj[9].ToString(), bodyFont));
                systemCheck.AddCell(new Phrase("CPU Type", bodyFont));
                systemCheck.AddCell(new Phrase(hardwareObj[10].ToString(), bodyFont));
                systemCheck.AddCell(new Phrase("Kernel Type", bodyFont));
                systemCheck.AddCell(new Phrase(hardwareObj[11].ToString(), bodyFont));
        }
        document.Add(systemCheck);

        document.Add(new Paragraph("3.2 Network configuration for " + chkServerObj[2].ToString(), subTitleFont));
        var network = new PdfPTable(2);
        network.HorizontalAlignment = 0;
        network.SpacingBefore = 15;
        network.SpacingAfter = 15;
        network.DefaultCell.BorderWidth = 1f;
        network.SetWidths(new int[] { 3, 5 });

        network.AddCell(new Phrase("", boldTableFont));
        network.AddCell(new Phrase("Values", boldTableFont));
        network.AddCell(new Phrase("IP Address", bodyFont));
        network.AddCell(new Phrase(hardwareObj[12].ToString(), bodyFont));
        network.AddCell(new Phrase("Sub Netmask", bodyFont));
        network.AddCell(new Phrase(hardwareObj[13].ToString(), bodyFont));
        document.Add(network);

        document.Add(new Paragraph("3.3 Crontab information for " + chkServerObj[2].ToString(), subTitleFont));
        var contrab = new PdfPTable(1);
        contrab.HorizontalAlignment = 0;
        contrab.SpacingBefore = 15;
        contrab.SpacingAfter = 15;
        contrab.DefaultCell.BorderWidth = 1f;
        contrab.SetWidths(new int[] {  5 });

        contrab.AddCell(new Phrase("Crontab Information", boldTableFont));
        contrab.AddCell(new Phrase(hardwareObj[14].ToString(), bodyFont));        
        document.Add(contrab);


        document.Add(new Paragraph("4. Database Information", titleFont));
        Paragraph p4 = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLUE, Element.ALIGN_LEFT, 0.8F)));
        document.Add(p4);
        document.Add(new Paragraph("4.1 Database Configuration", subTitleFont));
        var dbconfig = new PdfPTable(2);
        dbconfig.HorizontalAlignment = 0;
        dbconfig.SpacingBefore = 15;
        dbconfig.SpacingAfter = 15;
        dbconfig.DefaultCell.BorderWidth = 1f;
        dbconfig.SetWidths(new int[] { 5,5 });

        dbconfig.AddCell(new Phrase("What?", boldTableFont));
        dbconfig.AddCell(new Phrase("Value?", boldTableFont));
        List<object[]> databaseConfigObj = dbHelper.GetMultiQueryObject("SELECT * FROM DatabaseConfiguration WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
        if (databaseConfigObj != null) {
            for (int i = 0; i < databaseConfigObj.Count; i++) {
                dbconfig.AddCell(new Phrase((databaseConfigObj[i])[2].ToString(), bodyFont));
                dbconfig.AddCell(new Phrase((databaseConfigObj[i])[3].ToString(), bodyFont));
            }
        }
        document.Add(dbconfig);

        document.Add(new Paragraph("4.2 Database Parameter", subTitleFont));

        List<object[]> databaseParameterObj = dbHelper.GetMultiQueryObject("SELECT * FROM DatabaseParameter WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
        var dbParam = new PdfPTable(2);
        dbParam.HorizontalAlignment = 0;
        dbParam.SpacingBefore = 15;
        dbParam.SpacingAfter = 15;
        dbParam.DefaultCell.BorderWidth = 1f;
        dbParam.SetWidths(new int[] { 5, 5 });

        dbParam.AddCell(new Phrase("Name", boldTableFont));
        dbParam.AddCell(new Phrase("Values", boldTableFont));
        
        if (databaseParameterObj != null)
        {
            for (int i = 0; i < databaseParameterObj.Count; i++)
            {
                dbParam.AddCell(new Phrase((databaseParameterObj[i])[2].ToString(), bodyFont));
                dbParam.AddCell(new Phrase((databaseParameterObj[i])[3].ToString(), bodyFont));
            }
        }
        document.Add(dbParam);

        document.Add(new Paragraph("4.3 Major Security Initailization Parameters", subTitleFont));
        string majorSecureStr = "SELECT * FROM DatabaseParameter WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "'";
        majorSecureStr = majorSecureStr + " AND ( header = 'O7_DICTIONARY_ACCESSIBILITY' OR header = 'audit_trail' OR header = 'remote_login_passwordfile' OR header = 'remote_os_authent') ;";
        List<object[]> majorSecureObj = dbHelper.GetMultiQueryObject(majorSecureStr);
        var majorSecure = new PdfPTable(2);
        majorSecure.HorizontalAlignment = 0;
        majorSecure.SpacingBefore = 15;
        majorSecure.SpacingAfter = 15;
        majorSecure.DefaultCell.BorderWidth = 1f;
        majorSecure.SetWidths(new int[] { 5, 5 });

        majorSecure.AddCell(new Phrase("Name", boldTableFont));
        majorSecure.AddCell(new Phrase("Values", boldTableFont));

        if (majorSecureObj != null)
        {
            for (int i = 0; i < majorSecureObj.Count; i++)
            {
                majorSecure.AddCell(new Phrase((majorSecureObj[i])[2].ToString(), bodyFont));
                majorSecure.AddCell(new Phrase((majorSecureObj[i])[3].ToString(), bodyFont));
            }
        }
        document.Add(majorSecure);

        document.Add(new Paragraph("4.4 Database Files", subTitleFont));
        List<object[]> databaseFileObj = dbHelper.GetMultiQueryObject("SELECT * FROM DatabaseFile WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");

        var dbf = new PdfPTable(6);
        dbf.HorizontalAlignment = 0;
        dbf.SpacingBefore = 15;
        dbf.SpacingAfter = 15;
        dbf.DefaultCell.BorderWidth = 1f;
        dbf.SetWidths(new int[] { 3,5,3,3,2,3 });

        dbf.AddCell(new Phrase("Tbs Name", boldTableFont));
        dbf.AddCell(new Phrase("File Name", boldTableFont));
        dbf.AddCell(new Phrase("Size(MB)", boldTableFont));
        dbf.AddCell(new Phrase("Max(MB)", boldTableFont));
        dbf.AddCell(new Phrase("Aut", boldTableFont));
        dbf.AddCell(new Phrase("Inc.(block)", boldTableFont));

        if (databaseFileObj != null)
        {
            for (int i = 0; i < databaseFileObj.Count; i++)
            {
                dbf.AddCell(new Phrase((databaseFileObj[i])[2].ToString(), bodyFont));
                dbf.AddCell(new Phrase((databaseFileObj[i])[3].ToString(), bodyFont));
                dbf.AddCell(new Phrase((databaseFileObj[i])[4].ToString(), bodyFont));
                dbf.AddCell(new Phrase((databaseFileObj[i])[5].ToString(), bodyFont));
                dbf.AddCell(new Phrase((databaseFileObj[i])[6].ToString(), bodyFont));
                dbf.AddCell(new Phrase((databaseFileObj[i])[7].ToString(), bodyFont));
            }
        }
        document.Add(dbf);

        document.Add(new Paragraph("4.5 Temporary Files", subTitleFont));
        List<object[]> tempFileObj = dbHelper.GetMultiQueryObject("SELECT * FROM TempFile WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");

        var tf = new PdfPTable(6);
        tf.HorizontalAlignment = 0;
        tf.SpacingBefore = 15;
        tf.SpacingAfter = 15;
        tf.DefaultCell.BorderWidth = 1f;
        tf.SetWidths(new int[] { 3, 5, 3, 3, 2, 3 });

        tf.AddCell(new Phrase("Tbs Name", boldTableFont));
        tf.AddCell(new Phrase("File Name", boldTableFont));
        tf.AddCell(new Phrase("Size(MB)", boldTableFont));
        tf.AddCell(new Phrase("Max(MB)", boldTableFont));
        tf.AddCell(new Phrase("Aut", boldTableFont));
        tf.AddCell(new Phrase("Inc.(block)", boldTableFont));

        if (tempFileObj != null)
        {
            for (int i = 0; i < tempFileObj.Count; i++)
            {
                tf.AddCell(new Phrase((tempFileObj[i])[2].ToString(), bodyFont));
                tf.AddCell(new Phrase((tempFileObj[i])[3].ToString(), bodyFont));
                tf.AddCell(new Phrase((tempFileObj[i])[4].ToString(), bodyFont));
                tf.AddCell(new Phrase((tempFileObj[i])[5].ToString(), bodyFont));
                tf.AddCell(new Phrase((tempFileObj[i])[6].ToString(), bodyFont));
                tf.AddCell(new Phrase((tempFileObj[i])[7].ToString(), bodyFont));
            }
        }
        document.Add(tf);

        document.Add(new Paragraph("4.6 Redo Log File", subTitleFont));
        List<object[]> redoLogObj = dbHelper.GetMultiQueryObject("SELECT * FROM RedoLogFile WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");

        var rdf = new PdfPTable(3);
        rdf.HorizontalAlignment = 0;
        rdf.SpacingBefore = 15;
        rdf.SpacingAfter = 15;
        rdf.DefaultCell.BorderWidth = 1f;
        rdf.SetWidths(new int[] { 3, 5, 3});
        
        rdf.AddCell(new Phrase("Group#", boldTableFont));
        rdf.AddCell(new Phrase("Member", boldTableFont));
        rdf.AddCell(new Phrase("Size(MB)", boldTableFont));

        if (redoLogObj != null)
        {
            for (int i = 0; i < redoLogObj.Count; i++)
            {
                rdf.AddCell(new Phrase((redoLogObj[i])[2].ToString(), bodyFont));
                rdf.AddCell(new Phrase((redoLogObj[i])[3].ToString(), bodyFont));
                rdf.AddCell(new Phrase((redoLogObj[i])[4].ToString(), bodyFont));
            }
        }
        document.Add(rdf);

        document.Add(new Paragraph("4.7 ControlFile", subTitleFont));
        List<object[]> controlObj = dbHelper.GetMultiQueryObject("SELECT * FROM ControlFile WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");

        var cf = new PdfPTable(1);
        cf.HorizontalAlignment = 0;
        cf.SpacingBefore = 15;
        cf.SpacingAfter = 15;
        cf.DefaultCell.BorderWidth = 1f;
        cf.SetWidths(new int[] { 5 });
        
        cf.AddCell(new Phrase("Control File Name#", boldTableFont));

        if (controlObj != null)
        {
            for (int i = 0; i < controlObj.Count; i++)
            {
                cf.AddCell(new Phrase((controlObj[i])[2].ToString(), bodyFont));
            }
        }
        document.Add(cf);

        document.Add(new Paragraph("4.8 Daily Calendar Worksheet", subTitleFont));
        List<object[]> dayCalObj = dbHelper.GetMultiQueryObject("SELECT * FROM DailyCalendarWorksheet WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");

        var dc = new PdfPTable(3);
        dc.HorizontalAlignment = 0;
        dc.SpacingBefore = 15;
        dc.SpacingAfter = 15;
        dc.DefaultCell.BorderWidth = 1f;
        dc.SetWidths(new int[] { 3, 5, 3 });
        
        dc.AddCell(new Phrase("Time", boldTableFont));
        dc.AddCell(new Phrase("Description of Housekeeping and Batch Processing Schedule", boldTableFont));
        dc.AddCell(new Phrase("Estimated Duration", boldTableFont));

        if (dayCalObj != null)
        {
            for (int i = 0; i < dayCalObj.Count; i++)
            {
                dc.AddCell(new Phrase((dayCalObj[i])[2].ToString(), bodyFont));
                dc.AddCell(new Phrase((dayCalObj[i])[3].ToString(), bodyFont));
                dc.AddCell(new Phrase((dayCalObj[i])[4].ToString(), bodyFont));
            }
        }
        document.Add(dc);

        document.Add(new Paragraph("4.9 Monthly Calendar Worksheet", subTitleFont));
        List<object[]> monthCalObj = dbHelper.GetMultiQueryObject("SELECT * FROM MonthlyCalendarWorksheet WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");

        var mc = new PdfPTable(3);
        mc.HorizontalAlignment = 0;
        mc.SpacingBefore = 15;
        mc.SpacingAfter = 15;
        mc.DefaultCell.BorderWidth = 1f;
        mc.SetWidths(new int[] { 3, 5, 3 });
        
        mc.AddCell(new Phrase("Day", boldTableFont));
        mc.AddCell(new Phrase("Description of Housekeeping and Batch Processing Schedule", boldTableFont));
        mc.AddCell(new Phrase("Estimated Duration", boldTableFont));

        if (dayCalObj != null)
        {
            for (int i = 0; i < dayCalObj.Count; i++)
            {
                mc.AddCell(new Phrase((monthCalObj[i])[2].ToString(), bodyFont));
                mc.AddCell(new Phrase((monthCalObj[i])[3].ToString(), bodyFont));
                mc.AddCell(new Phrase((monthCalObj[i])[4].ToString(), bodyFont));
            }
        }
        document.Add(mc);


        document.Add(new Paragraph("5. RDBMS Performance", titleFont));
        Paragraph p5 = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLUE, Element.ALIGN_LEFT, 0.8F)));
        document.Add(p5);
        document.Add(new Paragraph("5.1 Performance Review", subTitleFont));
        List<object[]> perfReviceObj = dbHelper.GetMultiQueryObject("SELECT * FROM performanceReview WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");

        var perfRev = new PdfPTable(2);
        perfRev.HorizontalAlignment = 0;
        perfRev.SpacingBefore = 15;
        perfRev.SpacingAfter = 15;
        perfRev.DefaultCell.BorderWidth = 1f;
        perfRev.SetWidths(new int[] { 5,5 });
        
        perfRev.AddCell(new Phrase("Information Required to Tune Memory Allocation", boldTableFont));
        perfRev.AddCell(new Phrase("Answer", boldTableFont));

        if (perfReviceObj != null)
        {
            List<object[]> getHitRatioObj = dbHelper.GetMultiQueryObject("SELECT * FROM getHitRatio WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
            List<object[]> pinRatioObj = dbHelper.GetMultiQueryObject("SELECT * FROM PinRatio WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");

            var getHit = new PdfPTable(2);
            getHit.HorizontalAlignment = 0;
            getHit.SpacingBefore = 15;
            getHit.SpacingAfter = 15;
            getHit.DefaultCell.BorderWidth = 1f;
            getHit.SetWidths(new int[] { 5, 5 });
            getHit.AddCell(new Phrase("NameSpace", boldTableFont));
            getHit.AddCell(new Phrase("GetHitRatio", boldTableFont));
            if (getHitRatioObj != null)
            {
                for (int j = 0; j < 0; j++)
                {
                    getHit.AddCell(new Phrase((pinRatioObj[j])[2].ToString(), bodyFont));
                    getHit.AddCell(new Phrase((pinRatioObj[j])[3].ToString(), bodyFont));
                }
            }
            perfRev.AddCell(new Phrase("1. What is the gethitratio of the librarycache?", bodyFont));
            perfRev.AddCell(getHit);

            var pin = new PdfPTable(3);
            pin.HorizontalAlignment = 0;
            pin.SpacingBefore = 15;
            pin.SpacingAfter = 15;
            pin.DefaultCell.BorderWidth = 1f;
            pin.SetWidths(new int[] { 5, 5, 5 });
            pin.AddCell(new Phrase("Execution", boldTableFont));
            pin.AddCell(new Phrase("Cache Misses", boldTableFont));
            pin.AddCell(new Phrase("Sum", boldTableFont));
            if (pinRatioObj != null)
            {
                for (int i = 0; i < pinRatioObj.Count; i++)
                {
                    pin.AddCell(new Phrase((pinRatioObj[i])[2].ToString(), bodyFont));
                    pin.AddCell(new Phrase((pinRatioObj[i])[3].ToString(), bodyFont));
                    pin.AddCell(new Phrase((pinRatioObj[i])[4].ToString(), bodyFont));
                }
                perfRev.AddCell(new Phrase("2. What is the PIN / RELOAD ratio within the librarycache?", bodyFont));
                perfRev.AddCell(pin);
            }

            for (int i = 0; i < perfReviceObj.Count; i++)
            {
                if (i == 11)
                {
                    perfRev.AddCell(new Phrase((perfReviceObj[i])[2].ToString(), bodyFont));
                    perfRev.AddCell(new Phrase((perfReviceObj[i])[3].ToString(), bodyFont));

                    var undoSeg = new PdfPTable(3);
                    undoSeg.HorizontalAlignment = 0;
                    undoSeg.SpacingBefore = 15;
                    undoSeg.SpacingAfter = 15;
                    undoSeg.DefaultCell.BorderWidth = 1f;
                    undoSeg.SetWidths(new int[] { 5, 5, 5 });
                    undoSeg.AddCell(new Phrase("Amount", boldTableFont));
                    undoSeg.AddCell(new Phrase("Segment Type", boldTableFont));
                    undoSeg.AddCell(new Phrase("Size(MB)", boldTableFont));
                    List<object[]> segmentObj = dbHelper.GetMultiQueryObject("SELECT * FROM undoSegmentsSize WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
                    if (segmentObj != null) {
                        for (int j = 0; j < segmentObj.Count; j++) {
                            undoSeg.AddCell(new Phrase((segmentObj[j])[2].ToString(), bodyFont));
                            undoSeg.AddCell(new Phrase((segmentObj[j])[3].ToString(), bodyFont));
                            undoSeg.AddCell(new Phrase((segmentObj[j])[4].ToString(), bodyFont));
                        }
                    }
                    perfRev.AddCell(new Phrase("Number and size of Undo Segments?", bodyFont));
                    perfRev.AddCell(undoSeg);
                }
                else
                {
                    perfRev.AddCell(new Phrase((perfReviceObj[i])[2].ToString(), bodyFont));
                    perfRev.AddCell(new Phrase((perfReviceObj[i])[3].ToString(), bodyFont));
                }
            }
        }

        document.Add(perfRev);

        document.Add(new Paragraph("5.2 Database Growth Rate", subTitleFont));
        var image = iTextSharp.text.Image.GetInstance(Chart());
        image.ScalePercent(75f);
        document.Add(image);

        document.Add(new Paragraph("6. Tablespace Free Space", titleFont));
        Paragraph p6 = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLUE, Element.ALIGN_LEFT, 0.8F)));
        document.Add(p6);
        document.Add(new Paragraph("6.1 Tablespace Free Space", subTitleFont));
        List<object[]> tbsFreeObj = dbHelper.GetMultiQueryObject("SELECT * FROM TablespaceFreespace WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");

        var tfs = new PdfPTable(5);
        tfs.HorizontalAlignment = 0;
        tfs.SpacingBefore = 15;
        tfs.SpacingAfter = 15;
        tfs.DefaultCell.BorderWidth = 1f;
        tfs.SetWidths(new int[] { 3, 5, 3, 3, 2 });
        
        tfs.AddCell(new Phrase("Tablespace Name", boldTableFont));
        tfs.AddCell(new Phrase("Max Blocks", boldTableFont));
        tfs.AddCell(new Phrase("Count Blocks", boldTableFont));
        tfs.AddCell(new Phrase("Sum Free Blocks", boldTableFont));
        tfs.AddCell(new Phrase("PCT_FREE", boldTableFont));

        if (tbsFreeObj != null)
        {
            for (int i = 0; i < tbsFreeObj.Count; i++)
            {
                tfs.AddCell(new Phrase((tbsFreeObj[i])[2].ToString(), bodyFont));
                tfs.AddCell(new Phrase((tbsFreeObj[i])[3].ToString(), bodyFont));
                tfs.AddCell(new Phrase((tbsFreeObj[i])[4].ToString(), bodyFont));
                tfs.AddCell(new Phrase((tbsFreeObj[i])[5].ToString(), bodyFont));
                tfs.AddCell(new Phrase((tbsFreeObj[i])[6].ToString(), bodyFont));
            }
        }
        document.Add(tfs);

        document.Add(new Paragraph("7. Default tablespace and temporary tablespace", titleFont));
        Paragraph p7 = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLUE, Element.ALIGN_LEFT, 0.8F)));
        document.Add(p7);
        document.Add(new Paragraph("7.1 Default tablespace and temporary tablespace", subTitleFont));
        List<object[]> defTbsFreeObj = dbHelper.GetMultiQueryObject("SELECT * FROM TablespaceAndTempTablespace WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");

        var deftfs = new PdfPTable(3);
        deftfs.HorizontalAlignment = 0;
        deftfs.SpacingBefore = 15;
        deftfs.SpacingAfter = 15;
        deftfs.DefaultCell.BorderWidth = 1f;
        deftfs.SetWidths(new int[] { 3, 5, 3 });
        
        deftfs.AddCell(new Phrase("User Name", boldTableFont));
        deftfs.AddCell(new Phrase("Default Tablespace", boldTableFont));
        deftfs.AddCell(new Phrase("Temporary Tablespace", boldTableFont));

        if (defTbsFreeObj != null)
        {
            for (int i = 0; i < defTbsFreeObj.Count; i++)
            {
                deftfs.AddCell(new Phrase((defTbsFreeObj[i])[2].ToString(), bodyFont));
                deftfs.AddCell(new Phrase((defTbsFreeObj[i])[3].ToString(), bodyFont));
                deftfs.AddCell(new Phrase((defTbsFreeObj[i])[4].ToString(), bodyFont));
            }
        }
        document.Add(deftfs);


        document.Add(new Paragraph("8. Database Registry", titleFont));
        Paragraph p8 = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLUE, Element.ALIGN_LEFT, 0.8F)));
        document.Add(p8);
        document.Add(new Paragraph("8.1 Check Database Registry", subTitleFont));
        List<object[]> dbRegObj = dbHelper.GetMultiQueryObject("SELECT * FROM DatabaseRegistry WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");

        var dbReg = new PdfPTable(4);
        dbReg.HorizontalAlignment = 0;
        dbReg.SpacingBefore = 15;
        dbReg.SpacingAfter = 15;
        dbReg.DefaultCell.BorderWidth = 1f;
        dbReg.SetWidths(new int[] { 3, 5, 3, 3 });
        
        dbReg.AddCell(new Phrase("Comp ID", boldTableFont));
        dbReg.AddCell(new Phrase("Version", boldTableFont));
        dbReg.AddCell(new Phrase("Status", boldTableFont));
        dbReg.AddCell(new Phrase("Last Modified", boldTableFont));

        if (dbRegObj != null)
        {
            for (int i = 0; i < dbRegObj.Count; i++)
            {
                dbReg.AddCell(new Phrase((dbRegObj[i])[2].ToString(), bodyFont));
                dbReg.AddCell(new Phrase((dbRegObj[i])[3].ToString(), bodyFont));
                dbReg.AddCell(new Phrase((dbRegObj[i])[4].ToString(), bodyFont));
                dbReg.AddCell(new Phrase((dbRegObj[i])[5].ToString(), bodyFont));
            }
        }
        document.Add(dbReg);



        document.Add(new Paragraph("9. Information from Alert Log", titleFont));
        Paragraph p9 = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLUE, Element.ALIGN_LEFT, 0.8F)));
        document.Add(p9);
        document.Add(new Paragraph("9.1 alert log", subTitleFont));
        List<object[]> listOfAlert = dbHelper.GetMultiQueryObject("SELECT * FROM AlertLog WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");

        var alertLog = new PdfPTable(2);
        alertLog.HorizontalAlignment = 0;
        alertLog.SpacingBefore = 15;
        alertLog.SpacingAfter = 15;
        alertLog.DefaultCell.BorderWidth = 1f;
        alertLog.SetWidths(new int[] { 3, 5 });       

        if (listOfAlert != null)
        {
            for (int i = 0; i < listOfAlert.Count; i++)
            {
                alertLog.AddCell(new Phrase("Key Search", boldTableFont));
                alertLog.AddCell(new Phrase((listOfAlert[i])[2].ToString(), bodyFont));
                alertLog.AddCell(new Phrase("Action", boldTableFont));
                alertLog.AddCell(new Phrase((listOfAlert[i])[3].ToString(), bodyFont));
                alertLog.AddCell(new Phrase("Caused", boldTableFont));
                alertLog.AddCell(new Phrase((listOfAlert[i])[4].ToString(), bodyFont));
                alertLog.AddCell(new Phrase("Score", boldTableFont));
                alertLog.AddCell(new Phrase((listOfAlert[i])[5].ToString(), bodyFont));
            }
        }
        document.Add(alertLog);


        document.Add(new Paragraph("10. Backup History", titleFont));
        Paragraph p10 = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLUE, Element.ALIGN_LEFT, 0.8F)));
        document.Add(p10);
        object[] backupFIle = dbHelper.GetSingleQueryObject("SELECT * FROM backupfile WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
        if (backupFIle != null)
        {
            document.Add(new Paragraph("10.1 Backup Database", subTitleFont));
            var bd = new PdfPTable(1);
            bd.HorizontalAlignment = 0;
            bd.SpacingBefore = 15;
            bd.SpacingAfter = 15;
            bd.DefaultCell.BorderWidth = 1f;
            bd.SetWidths(new int[] { 5 });
            bd.AddCell(new Phrase("Last Backup Database", boldTableFont));
            bd.AddCell(new Phrase(backupFIle[4].ToString(), bodyFont));

            document.Add(new Paragraph("10.2 Backup Archivelog", subTitleFont));
            var ba = new PdfPTable(1);
            ba.HorizontalAlignment = 0;
            ba.SpacingBefore = 15;
            ba.SpacingAfter = 15;
            ba.DefaultCell.BorderWidth = 1f;
            ba.SetWidths(new int[] { 5 });
            ba.AddCell(new Phrase("Last Backup Archivelog", boldTableFont));
            ba.AddCell(new Phrase(backupFIle[3].ToString(), bodyFont));

            document.Add(new Paragraph("10.3 Backup Controlfile", subTitleFont));
            var bc = new PdfPTable(1);
            bc.HorizontalAlignment = 0;
            bc.SpacingBefore = 15;
            bc.SpacingAfter = 15;
            bc.DefaultCell.BorderWidth = 1f;
            bc.SetWidths(new int[] { 5 });
            bc.AddCell(new Phrase("Last Backup Controlfile", boldTableFont));
            bc.AddCell(new Phrase(backupFIle[2].ToString(), bodyFont));
        }

        // Close the Document - this saves the document contents to the output stream
        document.Close();
    }

    public static string GetTimestamp(DateTime value)
    {
        return value.ToString("yyyyMMddHHmmssffff");
    }

    private Byte[] Chart()
    {
        Object[] obj = dbHelper.GetSingleQueryObject("SELECT * FROM DBGrowthRate WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
        if (obj != null)
        {

            List<double> allocateSpaceList = new List<double>();
            List<double> usedSpaceList = new List<double>();

            double allocateSpaceInit = double.Parse(obj[2].ToString());
            double usedSpaceInit = double.Parse(obj[3].ToString());
            double growthDay = double.Parse(obj[4].ToString());
            double growthMonth = double.Parse(obj[5].ToString());
            double UsedGrowth = growthDay * 30;
            double AllowGrowth = growthMonth * 30;

            allocateSpaceList.Add(allocateSpaceInit);
            usedSpaceList.Add(usedSpaceInit);

            for (int i = 0; i < 3; i++)
            {
                allocateSpaceList.Add(allocateSpaceInit += AllowGrowth);
                usedSpaceList.Add(usedSpaceInit += UsedGrowth);
            }

            Chart1.Series.Clear();
            Chart1.Series.Add("Series1");
            Chart1.Series["Series1"].Points.AddXY(1, usedSpaceList[0]);
            Chart1.Series["Series1"].Points.AddXY(2, usedSpaceList[1]);
            Chart1.Series["Series1"].Points.AddXY(3, usedSpaceList[2]);
            Chart1.Series["Series1"].Points.AddXY(4, usedSpaceList[3]);
            Chart1.Series["Series1"].ChartType = SeriesChartType.Line;
            Chart1.Series["Series1"].BorderWidth = 3;

            Chart1.Series.Add("Series2");
            Chart1.Series["Series2"].Points.AddXY(1, allocateSpaceList[0]);
            Chart1.Series["Series2"].Points.AddXY(2, allocateSpaceList[1]);
            Chart1.Series["Series2"].Points.AddXY(3, allocateSpaceList[2]);
            Chart1.Series["Series2"].Points.AddXY(4, allocateSpaceList[3]);
            Chart1.Series["Series2"].ChartType = SeriesChartType.Line;
            Chart1.Series["Series2"].BorderWidth = 3;

            Chart1.ChartAreas[0].AxisX.Interval = 1;
            Chart1.Width = 500;

            using (var chartimage = new MemoryStream())
            {
                Chart1.SaveImage(chartimage, ChartImageFormat.Png);
                return chartimage.GetBuffer();
            }

        }
        else { return null; }
    }
}