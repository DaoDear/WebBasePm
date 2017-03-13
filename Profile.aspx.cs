using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class production_Profile : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["PreviousPage"] = Request.Url.AbsoluteUri; //Saves the Previous page url in ViewState

        if (Session["personID"] == null)
        {
            Response.Redirect("Login.aspx");
        }
    }

    protected void LogoutButton_Click(object sender, EventArgs e)
    {
        Session.RemoveAll();
    }
}