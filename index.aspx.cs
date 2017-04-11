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
using WebBasePM;

public partial class production_index : System.Web.UI.Page
{
    string projectCoded = "", projectQuarter = "";
    DatabaseHelper databaseHelper;

    protected void Page_Load(object sender, EventArgs e)
    {
        Session["PreviousPage"] = Request.Url.AbsoluteUri;

        if (Session["personID"] == null) {
            Response.Redirect("Login.aspx");            
        }

        nameHeader.Text = Session["Name"].ToString();
        nameHeader2.Text = Session["Name"].ToString();


        databaseHelper = new DatabaseHelper();
        List<object[]> docList = databaseHelper.GetMultiQueryObject("SELECT * FROM PmInfo  ORDER BY createdDate;");
        /* Load */        
        for(int i = 0; i < docList.Count; i++)
        {
            TableRow tRow = new TableRow();
            string cmdPrj = (docList[i])[0].ToString() +","+ (docList[i])[4].ToString();
            Button viewBtn = new Button();
            Button updateBtn = new Button();
            Button reviewBtn = new Button();
            viewBtn.Text = "View Report";
            viewBtn.CssClass = "btn btn-success";
            viewBtn.CommandName = "Reserve";
            viewBtn.CommandArgument = cmdPrj;
            viewBtn.Command += new CommandEventHandler(RePageDirect);

            updateBtn.Text = "Edit Report";
            updateBtn.CssClass = "btn btn-danger";
            updateBtn.CommandName = "Update";
            updateBtn.CommandArgument = cmdPrj;
            updateBtn.Command += new CommandEventHandler(RePageDirect);

            reviewBtn.Text = "Review Report";
            reviewBtn.CssClass = "btn btn-warning";
            reviewBtn.CommandName = "Review";
            reviewBtn.CommandArgument = cmdPrj;
            reviewBtn.Command += new CommandEventHandler(RePageDirect);


            TableCell value1 = new TableCell();
            value1.Text = (docList[i])[8].ToString();
            TableCell value2 = new TableCell();
            value2.Text = (docList[i])[1].ToString() + "("+ (docList[i])[2].ToString()  + ")";
            TableCell value3 = new TableCell();
            value3.Text = (docList[i])[0].ToString();
            TableCell value4 = new TableCell();
            value4.Text = (docList[i])[5].ToString();
            TableCell value5 = new TableCell();
            value5.Controls.Add(viewBtn);
            value5.Controls.Add(updateBtn);
            value5.Controls.Add(reviewBtn);

            tRow.Cells.Add(value1);
            tRow.Cells.Add(value2);
            tRow.Cells.Add(value3);
            tRow.Cells.Add(value4);
            tRow.Cells.Add(value5);
            showPmTable.Rows.Add(tRow);

        }
    }

    protected void RePageDirect(object sender, CommandEventArgs e)
    {
        if (e.CommandName == "Reserve")
        {
            string code = e.CommandArgument.ToString();
            string[] cmd = code.Split(',');

            Response.Redirect("pm_info.aspx?project=" + cmd[0].ToString() + "&quarter=" + cmd[1].ToString());

        }
        else if (e.CommandName == "Update")
        {
            string code = e.CommandArgument.ToString();
            string[] cmd = code.Split(',');

            Response.Redirect("pm_update.aspx?project=" + cmd[0].ToString() + "&quarter=" + cmd[1].ToString());

        }
        else if (e.CommandName == "Review")
        {
            string code = e.CommandArgument.ToString();
            string[] cmd = code.Split(',');

            Response.Redirect("pm_review.aspx?project=" + cmd[0].ToString() + "&quarter=" + cmd[1].ToString());

        }
    }
}