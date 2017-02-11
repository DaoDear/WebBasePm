using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class production_index : System.Web.UI.Page
{
    SqlConnection objConn;
    SqlCommand objCmd;
    string projectCoded = "", projectQuarter = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        /*
        if (Session["personID"] == null) {
            Response.Redirect("Login.aspx");
        }
        */

        string strConnString = "Server=localhost;Uid=sa;PASSWORD=08102535;database=PM;Max Pool Size=400;Connect Timeout=600;";
        objConn = new SqlConnection(strConnString);
        objConn.Open();
        projectCoded = "BFSI160002";
        projectQuarter = "1/2016";

        /* Load */
        string initShow = "SELECT * FROM PmInfo;";
        SqlDataReader initShowReader;
        objCmd = new SqlCommand(initShow, objConn);
        initShowReader = objCmd.ExecuteReader();

        while (initShowReader.Read())
        {
            TableRow tRow = new TableRow();

            string cmdPrj = initShowReader["projectCode"].ToString() +","+ initShowReader["quater"].ToString();

            Button viewBtn = new Button();
            viewBtn.Text = "View Report";
            viewBtn.CssClass = "btn btn-success";
            viewBtn.CommandName = "Reserve";
            viewBtn.CommandArgument = cmdPrj;
            viewBtn.Command += new CommandEventHandler(RePageDirect);


            TableCell value1 = new TableCell();
            value1.Text = initShowReader["databaseName"].ToString();
            TableCell value2 = new TableCell();
            value2.Text = initShowReader["customerCompanyFull"].ToString() + "("+ initShowReader["customerAbbv"].ToString()  + ")";
            TableCell value3 = new TableCell();
            value3.Text = initShowReader["projectCode"].ToString();
            TableCell value4 = new TableCell();
            value4.Text = initShowReader["pmstatus"].ToString();
            TableCell value5 = new TableCell();
            value5.Controls.Add(viewBtn);

            tRow.Cells.Add(value1);
            tRow.Cells.Add(value2);
            tRow.Cells.Add(value3);
            tRow.Cells.Add(value4);
            tRow.Cells.Add(value5);
            showPmTable.Rows.Add(tRow);

        }
        initShowReader.Close();

    }

    protected void RePageDirect(object sender, CommandEventArgs e)
    {
        if (e.CommandName == "Reserve")
        {
            string code = e.CommandArgument.ToString();
            string[] cmd = code.Split(',');

            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('"+ cmd[0].ToString() +" " + cmd[1].ToString() + "')", true);

            Response.Redirect("pm_info.aspx?project=" + cmd[0].ToString() + "&quarter=" + cmd[1].ToString());

        }
    }
}