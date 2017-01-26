<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="production_Login" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <!-- Meta, title, CSS, favicons, etc. -->
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>PM Report Support System</title>
    <!-- Bootstrap -->
    <link href="../vendors/bootstrap/dist/css/bootstrap.min.css" rel="stylesheet" />
    <!-- Font Awesome -->
    <link href="../vendors/font-awesome/css/font-awesome.min.css" rel="stylesheet" />
    <!-- NProgress -->
    <link href="../vendors/nprogress/nprogress.css" rel="stylesheet" />
    <!-- iCheck -->
    <link href="../vendors/iCheck/skins/flat/green.css" rel="stylesheet" />
    <!-- bootstrap-progressbar -->
    <link href="../vendors/bootstrap-progressbar/css/bootstrap-progressbar-3.3.4.min.css" rel="stylesheet" />
    <!-- JQVMap -->
    <link href="../vendors/jqvmap/dist/jqvmap.min.css" rel="stylesheet" />
    <!-- Custom Theme Style -->
    <link href="../build/css/custom.min.css" rel="stylesheet" />
  </head>
<body class="login">
    <div>
      <a class="hiddenanchor" id="signup"></a>
      <a class="hiddenanchor" id="signin"></a>

      <div class="login_wrapper">
        <div class="animate form login_form">
          <section class="login_content">
            <form runat="server">
              <h1>Login Form</h1>
              <div>
                <asp:RequiredFieldValidator ID="UsernameRequired" runat="server" ControlToValidate="username" ErrorMessage="Please input username" ForeColor="Red"></asp:RequiredFieldValidator>
                <asp:TextBox CssClass="form-control" Placeholder="Username" ValidateRequestMode="Enabled" ID="username" runat="server"></asp:TextBox>
                </div>
              <div>
                <asp:RequiredFieldValidator CssClass="form-inline" ID="PasswordRequired" runat="server" ControlToValidate="password" ErrorMessage="Please input password" ForeColor="Red"></asp:RequiredFieldValidator>
                <asp:TextBox CssClass="form-control" Placeholder="Password" ValidateRequestMode="Enabled" ID="password" runat="server" TextMode="Password"></asp:TextBox>
                </div>
              <div>
                <asp:Button CssClass="btn btn-default" Text="Log in" runat="server" ID="LoginSubmit" OnClick="LoginSubmit_Click" />
              </div>

              <div class="clearfix"></div>

              <div class="separator">
                <p class="change_link">New to site?
                </p>

                <div class="clearfix"></div>
                <br />

                <div>
                  <p>©2016 All Rights Reserved. Gentelella Alela! is a Bootstrap 3 template. Privacy and Terms</p>
                </div>
              </div>
            </form>
          </section>
        </div>        
      </div>
    </div>  
</body>
</html>
