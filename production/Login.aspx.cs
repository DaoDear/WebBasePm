using System;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using WebBasePM;

public partial class production_Login : System.Web.UI.Page
{
    protected DatabaseHelper dbHelper;

    protected void Page_Load(object sender, EventArgs e)
    {
        dbHelper = new DatabaseHelper();
    }

    protected void LoginSubmit_Click(object sender, EventArgs e)
    {
        string user = username.Text;
        string pass = password.Text;

        string title
              , firstName
              , lastName
              , email
              , phone
              , position;
        int permission,personID;

        Object[] obj  = dbHelper.GetSingleQueryObject("SELECT * FROM Person WHERE Username LIKE '" + user + "' AND Password LIKE '" + pass + "';");
        if (obj != null)
        {
            personID = (int)int.Parse(obj[0].ToString());
            title = (string)obj[1];
            firstName = (string)obj[2];
            lastName = (string)obj[3];
            email = (string)obj[6];
            phone = (string)obj[7];
            position = (string)obj[8];
            permission = (int)int.Parse(obj[10].ToString());

            Session["id"] = HttpContext.Current.Session.SessionID;
            Session["personID"] = personID;
            Session["permission"] = permission;

            Response.Redirect("index.aspx");         
        }
    }
}