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
using System.Web.UI.WebControls;
using System.Windows.Forms;
using WebBasePM;


public partial class production_Login : System.Web.UI.Page
{
    protected DatabaseHelper dbHelper;
    static string prevPage = String.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["personID"] != null)
        { 
            if (Session["PreviousPage"] != null) //check if the webpage is loaded for the first time.
            {
                Response.Redirect(Session["PreviousPage"].ToString()); //Saves the Previous page url in ViewState           
            }
            else{
                Response.Redirect("index.aspx");
            }            
        }
        
        dbHelper = new DatabaseHelper();
    }

    protected void ValidateUser(object sender, AuthenticateEventArgs e)
    {
           string title
            , firstName
            , lastName
            , email
            , phone
            , position;
        int permission, personID;
        string user = LoginForm.UserName;
        string pass = LoginForm.Password;

        Object[] obj = dbHelper.GetSingleQueryObject("SELECT TOP 1 * FROM Person WHERE Username LIKE '" + user + "' AND Password LIKE '" + pass + "';");
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

            Session["personID"] = personID;
            Session["Name"] = firstName + " " + lastName;
            
            if (Session["PreviousPage"] != null)  //Check if the ViewState 
                                                  //contains Previous page URL
            {
                Response.Redirect(Session["PreviousPage"].ToString());
            }
            else
            {
                Response.Redirect("index.aspx");
            }
        }
        else {
            e.Authenticated = false;
        }         
    }
}