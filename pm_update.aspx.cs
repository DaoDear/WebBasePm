﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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
    int authorRowCount = 0;



    protected void Page_Load(object sender, EventArgs e)
    {
        projectCoded = Request.QueryString["project"];
        projectQuarter = Request.QueryString["quarter"];

        DatabaseHelper dbHelper = new DatabaseHelper();
        dbHelper.getUpdate("UPDATE [dbo].[PmInfo] SET [pmstatus] = 'Wait Review' WHERE projectCode like '" + projectCoded + "' AND projectQuarter like '" + projectQuarter + "';");


        dateToday.Text = DateTime.Now.ToLongDateString();
        Session["PreviousPage"] = Request.Url.AbsoluteUri;
        dateToday.Text = DateTime.Now.ToLongDateString();
        if (Session["personID"] == null)
        {
            Response.Redirect("Login.aspx");
        }
        else {
            if (!IsPostBack)
            {
                LoadAuthor();
                LoadReviewer();
                LoadInfo();
                LoadAlert();
            }
        }

    }


    protected void LoadAuthor()
    {
        DatabaseHelper dbHelper = new DatabaseHelper();
        projectCoded = Request.QueryString["project"];
        projectQuarter = Request.QueryString["quarter"];
        SqlDataAdapter da = dbHelper.getConnection("SELECT *  FROM AuthorLog WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
        DataSet ds = new DataSet();
        da.Fill(ds);
        int count = ds.Tables[0].Rows.Count;

        if (ds.Tables[0].Rows.Count > 0)
        {
            authorGridView.DataSource = ds;
            authorGridView.DataBind();
        }
        else
        {
            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
            authorGridView.DataSource = ds;
            authorGridView.DataBind();
            int columncount = authorGridView.Rows[0].Cells.Count;
            //lblmsg.Text = " No data found !!!";
        }
    }

    void SqlPortClickSave(Object sender, EventArgs e)
    {
        DatabaseHelper dbHelper = new DatabaseHelper();
        dbHelper.getUpdate("UPDATE [dbo].[DatabaseConfiguration] SET [value] ='" + ((TextBox)FindControl("sqlPortID")).Text + "' WHERE projectCode like '" + projectCoded + "' AND projectQuarter like '" + projectQuarter + "' AND header like 'SQL*Net Port?';");
    }

    void AchieveSave(Object sender, EventArgs e)
    {
        DatabaseHelper dbHelper = new DatabaseHelper();
        dbHelper.getUpdate("UPDATE [dbo].[performanceReview] SET [value] ='" + ((TextBox)FindControl("archiveID")).Text + "' WHERE projectCode like '" + projectCoded + "' AND projectQuarter like '" + projectQuarter + "' AND header like 'Archive log names include sequence number?';");
    }

    protected void authorGridView_RowEditing(object sender, GridViewEditEventArgs e)
    {
        authorGridView.EditIndex = e.NewEditIndex;
        LoadAuthor();
    }
    protected void authorGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        DatabaseHelper dbHelper = new DatabaseHelper();
        string authorID = authorGridView.DataKeys[e.RowIndex].Values["authorID"].ToString();
        TextBox authorName = (TextBox)authorGridView.Rows[e.RowIndex].FindControl("txtname");
        TextBox authorVersion = (TextBox)authorGridView.Rows[e.RowIndex].FindControl("txtversion");
        TextBox authorChangeRef = (TextBox)authorGridView.Rows[e.RowIndex].FindControl("txtchangeref");
        dbHelper.getUpdate("UPDATE [dbo].[AuthorLog] SET [date] ='" + DateTime.Now + "',[author] ='" + authorName.Text + "',[version] ='" + authorVersion.Text + "' ,[changeReference] = '" + authorChangeRef.Text + "' WHERE authorID = " + authorID + ";");
        authorGridView.EditIndex = -1;
        LoadAuthor();
    }
    protected void authorGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        authorGridView.EditIndex = -1;
        LoadAuthor();
    }
    protected void authorGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DatabaseHelper dbHelper = new DatabaseHelper();
        string authorID = authorGridView.DataKeys[e.RowIndex].Values["authorID"].ToString();
        dbHelper.getUpdate("DELETE FROM [dbo].[AuthorLog]  WHERE authorID = " + authorID + ";");
        LoadAuthor();
    }
    protected void authorGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void authorGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        DatabaseHelper dbHelper = new DatabaseHelper();

        if (e.CommandName.Equals("AddNew"))
        {
            TextBox inname = (TextBox)authorGridView.FooterRow.FindControl("inname");
            TextBox inversion = (TextBox)authorGridView.FooterRow.FindControl("inversion");
            TextBox inchangeref = (TextBox)authorGridView.FooterRow.FindControl("inchangeref");

            projectCoded = Request.QueryString["project"];
            projectQuarter = Request.QueryString["quarter"];
            object[] authorList = new object[] { DateTime.Now, inname.Text, inversion.Text, inchangeref.Text };
            dbHelper.InsertAuthor(projectCoded, projectQuarter, authorList);

            LoadAuthor();
        }
    }

    protected void LoadReviewer()
    {
        DatabaseHelper dbHelper = new DatabaseHelper();
        projectCoded = Request.QueryString["project"];
        projectQuarter = Request.QueryString["quarter"];
        SqlDataAdapter da = dbHelper.getConnection("SELECT *  FROM reviewerLog WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
        DataSet ds = new DataSet();
        da.Fill(ds);
        int count = ds.Tables[0].Rows.Count;

        if (ds.Tables[0].Rows.Count > 0)
        {
            reviewerGridView.DataSource = ds;
            reviewerGridView.DataBind();
        }
        else
        {
            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
            reviewerGridView.DataSource = ds;
            reviewerGridView.DataBind();
            int columncount = reviewerGridView.Rows[0].Cells.Count;
            //lblmsg.Text = " No data found !!!";
        }
    }


    protected void LoadAlert()
    {
        DatabaseHelper dbHelper = new DatabaseHelper();
        projectCoded = Request.QueryString["project"];
        projectQuarter = Request.QueryString["quarter"];

        List<object[]> listOfAlert = dbHelper.GetMultiQueryObject("SELECT * FROM alert WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
        List<object[]> alert = null;
        if (listOfAlert != null)
            alert =  GetAlertFromAPI(listOfAlert);
        List<AlertInfo> alertList = new List<AlertInfo>();
        if (alert != null)
        {
            for (int al = 0; al < alert.Count; al++)
                alertList.Add(new AlertInfo((alert[al])[0].ToString(), (alert[al])[1].ToString(), (alert[al])[2].ToString(), (alert[al])[3].ToString()));
            if (alertList.Count > 0)
            {
                alertGridView.DataSource = alertList;
                alertGridView.DataBind();
            }
        }
    }

    public class AlertInfo
    {        
        public int? caseID { set; get; }
        public string keysearch { set; get; }
        public string caused { set; get; }
        public string actions { set; get; }
        public AlertInfo(string a1, string a2, string a3, string a4) {
            if (a1 == "null")
            {
                caseID = -1;
            }
            else {
                caseID = Int16.Parse(a1);
            }
            keysearch = a2;
            caused = a3;
            actions = a4;
        }
    }
    protected void reviewerGridView_RowEditing(object sender, GridViewEditEventArgs e)
    {
        reviewerGridView.EditIndex = e.NewEditIndex;
        LoadReviewer();
    }
    protected void reviewerGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        DatabaseHelper dbHelper = new DatabaseHelper();

        string reviewerID = reviewerGridView.DataKeys[e.RowIndex].Values["reviewerID"].ToString();
        TextBox reviewerDate = (TextBox)reviewerGridView.Rows[e.RowIndex].FindControl("txtreviewerdate");
        TextBox reviewerName = (TextBox)reviewerGridView.Rows[e.RowIndex].FindControl("txtreviewername");
        TextBox reviewerPosion = (TextBox)reviewerGridView.Rows[e.RowIndex].FindControl("txtreviewerposition");
        dbHelper.getUpdate("UPDATE [dbo].[reviewerLog] SET [reviewDate] ='" + DateTime.Now + "',[reviewerName] ='" + reviewerName.Text + "',[position] ='" + reviewerPosion.Text + "' WHERE reviewerID = " + reviewerID + ";");
        reviewerGridView.EditIndex = -1;
        LoadReviewer();

    }

    public void LoadInfo()
    {

        nameHeader.Text = Session["Name"].ToString();
        nameHeader2.Text = Session["Name"].ToString();

        dbHelper = new DatabaseHelper();
        string os = "";
        projectCoded = Request.QueryString["project"];
        projectQuarter = Request.QueryString["quarter"];

        if (projectCoded == null || projectQuarter == null)
        {
            if (ViewState["PreviousPage"] != null) //check if the webpage is loaded for the first time.
            {
                var prevState = ViewState["PreviousPage"].ToString();
                ViewState["PreviousPage"] = Request.Url.AbsoluteUri; //Saves the Previous page url in ViewState
                Response.Redirect(prevState);
            }
            else
            {
                Response.Redirect("index.aspx");
            }
        }

        object[] pminfo = dbHelper.GetSingleQueryObject("SELECT * FROM PmInfo WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
        if (pminfo != null) {
            hostTitle.Text = pminfo[2] + " " + pminfo[3] + " " + pminfo[4];
            alertDatabaeName.Text = pminfo[8].ToString();
        }

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

        /*  Check server    */
        object[] chkServerObj = dbHelper.GetSingleQueryObject("SELECT * FROM ChkServerMacSpec WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
        if (chkServerObj != null) {
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
        }
        /*  Oracle Requirement  */
        object[] oracleReqObj = dbHelper.GetSingleQueryObject("SELECT * FROM CompareOracleRequirement WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
        if (oracleReqObj != null) {
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
        }
        List<object[]> diskSpaceObj = dbHelper.GetMultiQueryObject("SELECT * FROM OSDiskSpace WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
        if (diskSpaceObj != null) {
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
        }
        List<object[]> userEnvObj = userEnvObj = dbHelper.GetMultiQueryObject("SELECT * FROM UserEnvironment WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
        if (userEnvObj != null) {
            for (int row = 0; row < userEnvObj.Count(); row++) {
                TableRow tRow = new TableRow();
                TableCell parameter = new TableCell();
                TableCell valueEnv = new TableCell();
                parameter.Text = (userEnvObj[row])[2].ToString();
                valueEnv.Text = (userEnvObj[row])[3].ToString();

                tRow.Cells.Add(parameter);
                tRow.Cells.Add(valueEnv);
                envTable.Rows.Add(tRow);
            }
        }
        /*  Hardware Configuration  */
        List<object[]> hardwareObj = dbHelper.GetMultiQueryObject("SELECT * FROM HardwareConfiguration WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
        if (hardwareObj != null) {
            for (int row = 0; row < hardwareObj.Count(); row++)
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
        }

        /* Datbase Configuration */
        List<object[]> databaseConfigObj = dbHelper.GetMultiQueryObject("SELECT * FROM DatabaseConfiguration WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
        if (databaseConfigObj != null) {
            for (int row = 0; row < databaseConfigObj.Count(); row++)
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
        }
        List<object[]> tempTableSizeObj = dbHelper.GetMultiQueryObject("SELECT * FROM TempTableSize WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
        if (tempTableSizeObj != null)
        {
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

        List<object[]> tempTableSpaceObj = dbHelper.GetMultiQueryObject("SELECT * FROM TablespaceName WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
        if (tempTableSpaceObj != null)
        {
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
        if (perfReviceObj != null) {
            for (int row = 0; row < perfReviceObj.Count(); row++)
            {
                TableRow tRow = new TableRow();
                TableCell parameter = new TableCell();
                TableCell valueEnv = new TableCell();

                parameter.Text = (perfReviceObj[row])[2].ToString();
                valueEnv.Text = (perfReviceObj[row])[3].ToString();

                tRow.Cells.Add(parameter);
                tRow.Cells.Add(valueEnv);
                perfReview.Rows.Add(tRow);
            }
        }
        List<object[]> getHitRatioObj = dbHelper.GetMultiQueryObject("SELECT * FROM getHitRatio WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");

        if (getHitRatioObj != null) {
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


            TableRow tRow1 = new TableRow();
            TableHeaderCell parameter1 = new TableHeaderCell();
            parameter1.Text = "Name Space";
            TableHeaderCell value1 = new TableHeaderCell();
            value1.Text = "Get Hit Ratio";

            tRow1.Cells.Add(parameter1);
            tRow1.Cells.Add(value1);
            hitRatio.Rows.Add(tRow1);
            for (int row = 0; row < getHitRatioObj.Count(); row++)
            {
                TableRow tRow = new TableRow();
                TableCell parameter = new TableCell();
                TableCell valueEnv = new TableCell();

                parameter.Text = (getHitRatioObj[row])[2].ToString();
                valueEnv.Text = (getHitRatioObj[row])[3].ToString();

                tRow.Cells.Add(parameter);
                tRow.Cells.Add(valueEnv);
                hitRatio.Rows.Add(tRow);
            }
        }

        /* 2 Pin Ratio */
        List<object[]> pinRatioObj = dbHelper.GetMultiQueryObject("SELECT * FROM PinRatio WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
        if (pinRatioObj != null)
        {
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
        List<object[]> segmentObj = dbHelper.GetMultiQueryObject("SELECT * FROM undoSegmentsSize WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
        if (segmentObj != null)
        {
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
            summarytxt = summarytxt.Replace("${datagrowth}", String.Format("{0:0.0}", (UsedGrowth * 100) / usedSpaceInit));
            summarytxt = summarytxt.Replace("${increaseratepercent}", String.Format("{0:0.0}", (((usedSpaceList[1] - usedSpaceList[0]) * 3) * 100) / usedSpaceInit));
            summarytxt = summarytxt.Replace("${percentused}", String.Format("{0:0.00}", (usedSpaceList[0] * 100) / allocateSpaceList[0]));
            summarytxt = summarytxt.Replace("${increaserate}", String.Format("{0:0,0.0}", (usedSpaceList[1] - usedSpaceList[0]) * 3));
            summarytxt = summarytxt.Replace("${databasename}", pminfo[8].ToString());

            DatabaseNameLabel.Text = pminfo[8].ToString();
            DatabaseNameLabel2.Text = pminfo[8].ToString();
            currAllocated.Text = allocateSpaceList[0].ToString();
            currUsed.Text = usedSpaceList[0].ToString();
            allocGrowth.Text = growthDay.ToString();
            usedGrowth.Text = growthMonth.ToString();
            SummaryLabel.Text = summarytxt;

        }

        List<Object[]> obj2 = dbHelper.GetMultiQueryObject("SELECT mountedOn, percentUsed FROM OSDiskSpace WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "';");
        if (obj2 != null)
        {
            ChartDataSpace.Series.Clear();
            ChartDataSpace.Series.Add("Series1");
            ChartDataSpace.Series["Series1"].ChartType = SeriesChartType.Bar;
            double[] y = { 90.4, 45, 43, 23, 43 };
            //ChartDataSpace.Series["Series1"].Points.AddY(Math.Round((double)60, 0));
            for (int i = 0; i < obj2.Count; i++)
            {
                string yValue = (obj2[i])[1].ToString().Substring(0, (obj2[i])[1].ToString().Length - 1);
                double yValueDouble;
                try
                {
                    yValueDouble = Double.Parse(yValue);
                }
                catch (Exception ex)
                {
                    yValueDouble = 0;
                    new System.Exception("Crash", ex);
                }
                ChartDataSpace.Series["Series1"].Points.AddY(yValueDouble);
                ChartDataSpace.Series["Series1"].Points[i].AxisLabel = (obj2[i])[0].ToString();
                ChartDataSpace.Series["Series1"].Points[i].Label = (obj2[i])[1].ToString();
            }


            ChartDataSpace.ChartAreas[0].AxisX.Interval = 1;
            ChartDataSpace.Width = 800;
        }

        List<Object[]> obj3 = dbHelper.GetMultiQueryObject("SELECT TOP 10 tablespaceName, pct_free from dbo.TablespaceFreespace WHERE projectCode = '" + projectCoded + "' AND projectQuarter = '" + projectQuarter + "' ORDER BY convert(float,pct_free) ASC;");
        
        if (obj3 != null)
        {
            obj3.Reverse();
            TableSpaceChart.Series.Clear();
            TableSpaceChart.Series.Add("Series1");
            TableSpaceChart.Series["Series1"].ChartType = SeriesChartType.Bar;
            for (int i = 0; i < obj3.Count; i++)
            {
                double yValue = Double.Parse((obj3[i])[1].ToString());
                TableSpaceChart.Series["Series1"].Points.AddY(yValue);
                TableSpaceChart.Series["Series1"].Points[i].AxisLabel = (obj3[i])[0].ToString();
                TableSpaceChart.Series["Series1"].Points[i].Label = (obj3[i])[1].ToString();
                TableSpaceChart.Series["Series1"].Color = System.Drawing.Color.Green;
            }


            TableSpaceChart.ChartAreas[0].AxisX.Interval = 1;
            TableSpaceChart.Width = 800;
        }
    }

    void buttonOk_Click(object sender, EventArgs e)
    {
        System.Windows.Forms.MessageBox.Show("clicked");
    }

    public List<object[]> GetAlertFromAPI(List<object[]> listOfAlert)
    {
        List<object[]> alertList = new List<object[]>();
        try
        {
            string postData = "  {\"searchSet\" : [";
            for (int al = 0; al < listOfAlert.Count(); al++)
            {
                postData += "{\"searchKey\" : \"" + (listOfAlert[al])[2].ToString() + "\"}";
                if (al != listOfAlert.Count() - 1)
                {
                    postData += ",";
                }
            }
            postData += "]}";

            WebRequest request = WebRequest.Create("http://103.27.202.76/api/v1/findByOraId");
            request.Method = "POST";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            dataStream = response.GetResponseStream();
            StreamReader reader1 = new StreamReader(dataStream);
            string responseFromServer = reader1.ReadToEnd();

            Root jsonObject = JsonConvert.DeserializeObject<Root>(responseFromServer);
            for (int ai = 0; ai < jsonObject.RootWord.Count; ai++)
            {
                if (jsonObject.RootWord[ai].Results[0].ObjResults != null)
                {
                    for (int aj = 0; aj < jsonObject.RootWord[ai].Results[0].ObjResults.Count(); aj++)
                    {
                        alertList.Add(new object[] { jsonObject.RootWord[ai].Results[0].ObjResults[aj].word[0].caseID, jsonObject.RootWord[ai].Results[0].KeySearch[0].ora_id, jsonObject.RootWord[ai].Results[0].ObjResults[aj].word[0].caused, jsonObject.RootWord[ai].Results[0].ObjResults[aj].word[0].actions, jsonObject.RootWord[ai].Results[0].ObjResults[aj].word[0].score });
                    }
                }
                else
                {
                    alertList.Add(new object[] { "null", jsonObject.RootWord[ai].Results[0].KeySearch[0].ora_id, "Please contact oracle support for more information.", "N/A", "N/A" });
                }
            }
            reader1.Close();
            dataStream.Close();
            response.Close();
        }
        catch (Exception ex)
        {
            throw new HttpException("BAD REQUEST", ex);
        }
        
        return alertList;
    }
    protected void reviewerGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        reviewerGridView.EditIndex = -1;
        LoadReviewer();
    }
    protected void reviewerGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DatabaseHelper dbHelper = new DatabaseHelper();
        string reviewerID = reviewerGridView.DataKeys[e.RowIndex].Values["reviewerID"].ToString();
        dbHelper.getUpdate("DELETE FROM [dbo].[reviewerLog]  WHERE reviewerID = " + reviewerID+";");
        LoadReviewer();
    }
    protected void reviewerGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void reviewerGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        DatabaseHelper dbHelper = new DatabaseHelper();

        if (e.CommandName.Equals("AddNew"))
        {
            TextBox inname = (TextBox)reviewerGridView.FooterRow.FindControl("inreviewername");
            TextBox inposition = (TextBox)reviewerGridView.FooterRow.FindControl("inreviewerposition");

            projectCoded = Request.QueryString["project"];
            projectQuarter = Request.QueryString["quarter"];
            object[] reviewerList = new object[] { DateTime.Now, inname.Text, inposition.Text };
            dbHelper.InsertReviewer(projectCoded, projectQuarter, reviewerList);

            LoadReviewer();
        }
    }

    protected void alertGridView_RowEditing(object sender, GridViewEditEventArgs e)
    {
        alertGridView.EditIndex = e.NewEditIndex;
        LoadAlert();
    }
    protected void alertGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string caseID = alertGridView.DataKeys[e.RowIndex].Values["caseID"].ToString();
        TextBox keysearch = (TextBox)alertGridView.Rows[e.RowIndex].FindControl("txtkeysearch");
        TextBox caused = (TextBox)alertGridView.Rows[e.RowIndex].FindControl("txtcaused");
        TextBox actions = (TextBox)alertGridView.Rows[e.RowIndex].FindControl("txtaction");
        try
        {
            string postData = "  {\"searchSet\" : [";          
            postData += "{\"case_id\" : \"" + caseID + "\", \"keysearch\" : \"" + keysearch.Text + "\", \"caused\" : \""+ caused.Text +"\", \"actions\" : \"" + actions.Text +"\"}";
            postData += "]}";

            WebRequest request = WebRequest.Create("http://103.27.202.76/api/v1/addCaseBase");
            request.Method = "PUT";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            dataStream = response.GetResponseStream();
            StreamReader reader1 = new StreamReader(dataStream);
            string responseFromServer = reader1.ReadToEnd();           
            reader1.Close();
            dataStream.Close();
            response.Close();
            
        }
        catch (Exception ex)
        {
            throw new HttpException("BAD REQUEST", ex);
        }
        alertGridView.EditIndex = -1;
        LoadAlert();
    }
    protected void alertGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        alertGridView.EditIndex = -1;
        LoadAlert();
    }
    protected void alertGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
   
    protected void growthSave_Click(object sender, EventArgs e)
    {
        DatabaseHelper dbHelper = new DatabaseHelper();
        dbHelper.getUpdate("UPDATE [dbo].[DBGrowthRate] SET [allocatedSpace] = "+ currAllocated .Text + " ,[usedSpace] = "+ currUsed.Text + ",[growthDay] = "+ allocGrowth.Text + " ,[growthMonth] = "+ usedGrowth.Text + "  WHERE projectCode like '" + projectCoded + "' AND projectQuarter like '" + projectQuarter+"';");
        dbHelper.getUpdate("UPDATE[dbo].[PmInfo] SET[databaseName] = '"+ DatabaseNameLabel2.Text + "' WHERE projectCode like '" + projectCoded + "' AND projectQuarter like '" + projectQuarter+"';");        
    }

    // Alert log Class.
    public class KeySearch
    {
        public string ora_id { get; set; }
    }

    public class Word
    {
        public string caseID { get; set; }
        public string caused { get; set; }
        public string actions { get; set; }
        public double score { get; set; }
    }

    public class ObjResult
    {
        public IList<Word> word { get; set; }
    }

    public class Result
    {
        public IList<KeySearch> KeySearch { get; set; }
        public IList<ObjResult> ObjResults { get; set; }
    }

    public class RootWord
    {
        public IList<Result> Results { get; set; }
    }

    public class Root
    {
        public IList<RootWord> RootWord { get; set; }
    }
}