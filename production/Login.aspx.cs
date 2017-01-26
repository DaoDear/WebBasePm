using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Cryp = System.Security.Cryptography;

public partial class production_Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       
    }

    protected void LoginSubmit_Click(object sender, EventArgs e)
    {
        string user = username.Text;
        string pass = password.Text;

        string title = ""
              , firstName = ""
              , lastName = ""
              , email = ""
              , phone = ""
              , position = "";
        int permission = -1;

        string connectionStr = "Server=localhost;Uid=sa;PASSWORD=08102535;database=PM;Max Pool Size=400;Connect Timeout=600;";
        SqlConnection objConn;
        SqlCommand objCmd;
        objConn = new SqlConnection(connectionStr);
        objConn.Open();
        string authentication = "SELECT * FROM Person WHERE Username LIKE '" + user + "' AND Password LIKE '" + pass + "';";
        SqlDataReader reader;
        objCmd = new SqlCommand(authentication, objConn);
        reader = objCmd.ExecuteReader();
        if (reader.HasRows)
        {
            while (reader.Read())
            {
                title = reader["Title"].ToString();
                firstName = reader["Firstname"].ToString();
                lastName = reader["Lastname"].ToString();
                email = reader["Email"].ToString();
                phone = reader["Phone"].ToString();
                position = reader["Position"].ToString();
                permission = int.Parse(reader["Permission"].ToString());
            }
            reader.Close();
            reader = null;
            MessageBox.Show("Login Success", "My Application", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
        }
        else
        {
            MessageBox.Show("Login Fail", "My Application", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
        }
    }
}