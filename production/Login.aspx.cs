using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Script.Serialization;
using System.Web.Security;
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
        string user = "";
        string pass = "";
       

      
    }

    protected void ValidateUser(object sender, EventArgs e)
    {
        int userId = 0;

            string title
             , firstName
             , lastName
             , email
             , phone
             , position;
            int permission, personID;

            Object[] obj = dbHelper.GetSingleQueryObject("SELECT * FROM Person WHERE Username LIKE '" + user + "' AND Password LIKE '" + pass + "';");
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
            }
            switch (userId)
            {
                case -1:
                    Login1.FailureText = "Username and/or password is incorrect.";
                    break;
                case -2:
                    Login1.FailureText = "Account has not been activated.";
                    break;
                default:
                    FormsAuthentication.RedirectFromLoginPage(Login1.UserName, Login1.RememberMeSet);
                    break;
            }
        
    }
}