<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GenPM.aspx.cs" Inherits="WebBasePM.ProductionGenerator" Debug="true" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <!-- Meta, title, CSS, favicons, etc. -->
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>PM Report Support System</title>
    <!-- Bootstrap -->
    <link href="../vendors/bootstrap/dist/css/bootstrap.min.css" rel="stylesheet">
    <!-- Font Awesome -->
    <link href="../vendors/font-awesome/css/font-awesome.min.css" rel="stylesheet">
    <!-- NProgress -->
    <link href="../vendors/nprogress/nprogress.css" rel="stylesheet">
    <!-- iCheck -->
    <link href="../vendors/iCheck/skins/flat/green.css" rel="stylesheet">
    <!-- bootstrap-progressbar -->
    <link href="../vendors/bootstrap-progressbar/css/bootstrap-progressbar-3.3.4.min.css" rel="stylesheet">
    <!-- JQVMap -->
    <link href="../vendors/jqvmap/dist/jqvmap.min.css" rel="stylesheet" />
    <!-- Custom Theme Style -->
    <link href="../build/css/custom.min.css" rel="stylesheet">
</head>
<body class="nav-md">
     
    <div class="container body">
        <div class="main_container">
            
            <div class="col-md-3 left_col menu_fixed">
                <div class="col-md-3 left_col">
                    <div class="left_col scroll-view">
                        <div class="navbar nav_title" style="border: 0;">
                            <a href="index.html" class="site_title"><span>PM Report</span></a>
                        </div>
                        <div class="clearfix"></div>
                        <!-- menu profile quick info -->
                        <div class="profile">
                            <div class="profile_pic">
                                <img src="images/img.jpg" alt="..." class="img-circle profile_img" />
                            </div>
                            <div class="profile_info">
                                <span>Welcome,</span>
                                <h2><asp:Label ID="nameHeader" runat="server"></asp:Label></h2>
                            </div>
                        </div>
                        <!-- /menu profile quick info -->
                        <br />
                        <!-- sidebar menu -->
                        <div id="sidebar-menu" class="main_menu_side hidden-print main_menu">
                            <div class="menu_section">
                                <h3>Monday 24 August 2016</h3>
                                <ul class="nav side-menu">
                                    <li>
                                        <a href="index.aspx"><i class="fa fa-home"></i>Home</a>
                                    </li>
                                    <li>
                                        <a href="GenPM.aspx"><i class="fa fa-edit"></i>Generate PM</a>
                                    </li>
                                    <li>
                                        <a><i class="fa fa-desktop"></i>Search PM</a>
                                    </li>
                                    <li>
                                        <a><i class="fa fa-table"></i>Review PM</a>
                                    </li>
                                </ul>
                            </div>
                        </div>
                        <!-- /sidebar menu -->
                    </div>
                </div>
            </div>
            <!-- top navigation -->
            <div class="top_nav">
                <div class="nav_menu">
                    <nav>
                        <div class="nav toggle">
                            <a id="menu_toggle"><i class="fa fa-bars"></i></a>
                        </div>
                        <ul class="nav navbar-nav navbar-right">
                            <li class="">
                                <a href="javascript:;" class="user-profile dropdown-toggle" data-toggle="dropdown" aria-expanded="false">
                                    <img src="images/img.jpg" alt=""><asp:Label ID="nameHeader2" runat="server"></asp:Label>
                                    <span class=" fa fa-angle-down"></span>
                                </a>
                                <ul class="dropdown-menu dropdown-usermenu pull-right">
                                    <li><a href="javascript:;">Profile</a></li>
                                    <li><a href="login.html"><i class="fa fa-sign-out pull-right"></i>Log Out</a></li>
                                </ul>
                            </li>
                        </ul>
                    </nav>
                </div>
            </div>
            <!-- /top navigation -->
            <!-- page content -->
            <div class="right_col" role="main">
                <div class="">
                    <div class="page-title">
                        <div class="title_left">
                            <h3>Preventive Maintenance</h3>
                        </div>
                    </div>
                    <div class="clearfix"></div>
                    <div class="row">
                        <div class="col-md-12 col-sm-12 col-xs-12">
                            <div class="x_panel">
                                <div class="x_title">
                                    <h2>Generate Preventive Maintenance Document</h2>
                                    <div class="clearfix"></div>
                                </div>
                                </div>
                                <div class="x_content">
                                <!-- Smart Wizard -->
                                 <form runat="server">
                                        <p>This is a basic form wizard example that inherits the colors from the selected scheme.</p>
                                        <div id="wizard" class="form_wizard wizard_horizontal">
                                            <ul class="wizard_steps">
                                                <li>
                                                    <a href="#step-1">
                                                        <span class="step_no">1</span>
                                                        <span class="step_descr">Step 1<br />
                                                            <small>Project Information</small>
                                                        </span>
                                                    </a>
                                                </li>
                                                <li>
                                                    <a href="#step-2">
                                                        <span class="step_no">2</span>
                                                        <span class="step_descr">Step 2<br />
                                                            <small>Personal Information</small>
                                                        </span>
                                                    </a>
                                                </li>
                                                <li>
                                                    <a href="#step-3">
                                                        <span class="step_no">3</span>
                                                        <span class="step_descr">Step 3<br />
                                                            <small>OS Configuration</small>
                                                        </span>
                                                    </a>
                                                </li>
                                                <li>
                                                    <a href="#step-4">
                                                        <span class="step_no">4</span>
                                                        <span class="step_descr">Step 4<br />
                                                            <small>Database Configuration</small>
                                                        </span>
                                                    </a>
                                                </li>                                                
                                            </ul>

                                            <div id="step-1">
                                                <div class="form-horizontal form-label-left">
                                                    <!-- Project Information -->
                                                    <div class="x_title">
                                                        <h2>Project Information</h2>
                                                        <div class="clearfix"></div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label class="control-label col-md-3 col-sm-3 col-xs-12" for="proj-code">Project Code</label>
                                                        <div class="col-md-6 col-sm-6 col-xs-12">
                                                            <div class="input-group">
                                                                <asp:TextBox ID="projCodeInput" runat="server" OnTextChanged="srhProjCode_Click" class="select2_single form-control" TabIndex="-1" name="projCodeInput"></asp:TextBox>
                                                                <span class="input-group-btn">
                                                                    <asp:Button runat="server" Text="Search" class="btn btn-primary" ID="srhProjCode" OnClick="srhProjCode_Click" />
                                                                </span>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label class="control-label col-md-3 col-sm-3 col-xs-12" for="cust-com-full">Customer Company Full</label>
                                                        <div class="col-md-6 col-sm-6 col-xs-12">
                                                            <asp:TextBox ID="custComFull" runat="server" class="form-control col-md-7 col-xs-12" name="custComFull"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="control-label col-md-3 col-sm-3 col-xs-12" for="cust-com">Customer Company</label>
                                                        <div class="col-md-6 col-sm-6 col-xs-12">
                                                            <asp:TextBox ID="custComAbbv" runat="server" class="form-control col-md-7 col-xs-12" name="custComAbbv"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label class="control-label col-md-3 col-sm-3 col-xs-12" for="proj-name">Project Name</label>
                                                        <div class="col-md-6 col-sm-6 col-xs-12">
                                                            <asp:TextBox ID="projInput" runat="server" class="form-control col-md-7 col-xs-12" name="projName"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="control-label col-md-3 col-sm-3 col-xs-12" for="quarter">Quarter</label>
                                                        <div class="col-md-6 col-sm-6 col-xs-12">
                                                            <asp:TextBox ID="quarterInput" runat="server" class="form-control col-md-7 col-xs-12" name="quarter"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <!-- End Project Information -->
                                                    <br>
                                                </div>
                                            </div>
                                            <div id="step-2">
                                                <div class="form-horizontal form-label-left">
                                                    <!-- Personal Information -->
                                                    <div class="x_title">
                                                        <h2>Personal Information</h2>
                                                        <div class="clearfix"></div>
                                                    </div>

                                                    <h4><b>Customer </b></h4>
                                                    <div class="col-md-6 col-sm-6 col-xs-12 form-group has-feedback">
                                                        <asp:TextBox ID="CustFName" runat="server" class="form-control has-feedback-left" placeholder="First Name"></asp:TextBox>
                                                        <span class="fa fa-user form-control-feedback left" aria-hidden="true"></span>
                                                    </div>
                                                    <div class="col-md-6 col-sm-6 col-xs-12 form-group has-feedback">
                                                        <asp:TextBox ID="CustLName" runat="server" class="form-control has-feedback-right" placeholder="Last Name"></asp:TextBox>
                                                        <span class="fa fa-user form-control-feedback right" aria-hidden="true"></span>
                                                    </div>
                                                    <div class="col-md-6 col-sm-6 col-xs-12 form-group has-feedback">
                                                        <asp:TextBox ID="CustEmail" runat="server" class="form-control has-feedback-left" placeholder="Email"></asp:TextBox>
                                                        <span class="fa fa-envelope form-control-feedback left" aria-hidden="true"></span>
                                                    </div>
                                                    <div class="col-md-6 col-sm-6 col-xs-12 form-group has-feedback">
                                                        <asp:TextBox ID="CustPhone" runat="server" class="form-control has-feedback-right" placeholder="Phone"></asp:TextBox>
                                                        <span class="fa fa-phone form-control-feedback right" aria-hidden="true"></span>
                                                    </div>

                                                    <h4><b>Sale </b></h4>
                                                    <div class="col-md-6 col-sm-6 col-xs-12 form-group has-feedback">
                                                        <asp:TextBox ID="SaleFName" runat="server" class="form-control has-feedback-left" placeholder="First Name"></asp:TextBox>
                                                        <span class="fa fa-user form-control-feedback left" aria-hidden="true"></span>
                                                    </div>
                                                    <div class="col-md-6 col-sm-6 col-xs-12 form-group has-feedback">
                                                        <asp:TextBox ID="SaleLName" runat="server" class="form-control has-feedback-right" placeholder="Last Name"></asp:TextBox>
                                                        <span class="fa fa-user form-control-feedback right" aria-hidden="true"></span>
                                                    </div>
                                                    <div class="col-md-6 col-sm-6 col-xs-12 form-group has-feedback">
                                                        <asp:TextBox ID="SaleEmail" runat="server" class="form-control has-feedback-left" placeholder="Email"></asp:TextBox>
                                                        <span class="fa fa-envelope form-control-feedback left" aria-hidden="true"></span>
                                                    </div>
                                                    <div class="col-md-6 col-sm-6 col-xs-12 form-group has-feedback">
                                                        <asp:TextBox ID="SalePhone" runat="server" class="form-control has-feedback-right" placeholder="Phone"></asp:TextBox>
                                                        <span class="fa fa-phone form-control-feedback right" aria-hidden="true"></span>
                                                    </div>                                                    
                                                </div>
                                            </div>
                                            
                                            <div id="step-3">
                                                <div class="form-horizontal form-label-left">
                                                    <div class="x_title">
                                                        <h2>OS Information</h2>
                                                        <div class="clearfix"></div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label class="control-label col-md-3 col-sm-3 col-xs-12" for="os-plat">Platform</label>
                                                        <div class="col-md-6 col-sm-6 col-xs-12">
                                                            <asp:DropDownList runat="server" ID="OSType" class="select2_single form-control" TabIndex="-1" name="OSType">
                                                                <asp:ListItem Text="Please select the OS" />
                                                                <asp:ListItem Text="Linux" Value="Linux" />
                                                                <asp:ListItem Text="AIX" Value="Aix" />
                                                                <asp:ListItem Text="Solaris" Value="Solaris" />
                                                                <asp:ListItem Text="Windows" Value="Windows" />
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label class="control-label col-md-3 col-sm-3 col-xs-12" for="ospath">Input Path</label>                                                        
                                                        <div class="col-md-6 col-sm-6 col-xs-12">
                                                            <div class="input-group">
                                                                  <asp:FileUpload ID="FolderUpload" webkitdirectory mozdirectory msdirectory odirectory directory AllowMultiple="true" runat="server" />
                                                            </div>
                                                        </div>                                                  
                                                    </div>
                                                </div>
                                            </div>

                                            <div id="step-4">
                                                <div class="form-horizontal form-label-left">
                                                    <div class="x_title">
                                                        <h2>Database Information</h2>
                                                        <div class="clearfix"></div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label class="control-label col-md-3 col-sm-3 col-xs-12" for="db-name">Database Name</label>
                                                        <div class="col-md-6 col-sm-6 col-xs-12">
                                                            <asp:TextBox ID="databaseName" runat="server" class="form-control col-md-7 col-xs-12"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label class="control-label col-md-3 col-sm-3 col-xs-12" for="dbcon">Configruation</label>
                                                        <div class="col-md-6 col-sm-6 col-xs-12">
                                                           <asp:FileUpload ID="DBConfigUpload" runat="server" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label class="control-label col-md-3 col-sm-3 col-xs-12" for="dbgrowth">Growth Rate</label>
                                                        <div class="col-md-6 col-sm-6 col-xs-12">
                                                            <div class="row">
                                                                <div class="col-md-3 col-sm-3 col-xs-3">
                                                                <asp:TextBox CssClass="form-control" Placeholder="Curr Allocate (GB)" ID="currAlloc" runat="server"></asp:TextBox>
                                                                </div>
                                                                <div class="col-md-3 col-sm-3 col-xs-3">
                                                                <asp:TextBox CssClass="form-control" Placeholder="Used Allocate (GB)" ID="usedAlloc" runat="server"></asp:TextBox>
                                                                </div>
                                                                <div class="col-md-3 col-sm-3 col-xs-3">
                                                                <asp:TextBox CssClass="form-control" Placeholder="Alloc Growth (GB)" ID="allocGrowth" runat="server"></asp:TextBox>
                                                                </div>
                                                                <div class="col-md-3 col-sm-3 col-xs-3">
                                                                <asp:TextBox CssClass="form-control" Placeholder="Used Growth (GB)" ID="usedGrowth" runat="server"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label class="control-label col-md-3 col-sm-3 col-xs-12" for="dbback">Backup Database</label>
                                                         <div class="col-md-6 col-sm-6 col-xs-12">
                                                            <asp:FileUpload ID="BackupDatabaseFile" runat="server" />
                                                         </div>
                                                       
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="control-label col-md-3 col-sm-3 col-xs-12" for="dbback">Backup Controlfile</label>
                                                         <div class="col-md-6 col-sm-6 col-xs-12">
                                                            <asp:FileUpload ID="BackupControlFile" runat="server" />
                                                         </div>
                                                       
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="control-label col-md-3 col-sm-3 col-xs-12" for="dbback">Backup Archieve</label>
                                                         <div class="col-md-6 col-sm-6 col-xs-12">
                                                            <asp:FileUpload ID="BackupArchieveFile" runat="server" />
                                                         </div>
                                                       
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="control-label col-md-3 col-sm-3 col-xs-12" for="dbalert">Alert Log</label>
                                                           <div class="col-md-6 col-sm-6 col-xs-12">
                                                            <asp:FileUpload ID="AlertLogFile" runat="server" />
                                                           </div>
                                                    </div>

                                                </div>
                                            </div>
               
                                             <div style="display: none;">
                                             <asp:Button OnClick="Finish_Click" ID="finishBtn" Text="FINISH" runat="server"  /> 
                                           
                                        
                                             </div>
                                        </div>
                                    </form>
                                    <!-- End SmartWizard Content -->

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <!-- /page content -->
            <!-- footer content -->
            <footer>
                <div class="pull-right">
                    Power by beebie
                </div>
                <div class="clearfix"></div>
            </footer>
            <!-- /footer content -->
        </div>

    <!-- jQuery -->
    <script src="../vendors/jquery/dist/jquery.min.js"></script>
    <!-- Bootstrap -->
    <script src="../vendors/bootstrap/dist/js/bootstrap.min.js"></script>
    <!-- FastClick -->
    <script src="../vendors/fastclick/lib/fastclick.js"></script>
    <!-- NProgress -->
    <script src="../vendors/nprogress/nprogress.js"></script>
    <!-- jQuery Smart Wizard -->
    <script src="../vendors/jQuery-Smart-Wizard/js/jquery.smartWizard.js"></script>
    <!-- Custom Theme Scripts -->
    <script src="../build/js/custom.min.js"></script>
    <!-- jQuery Smart Wizard -->
    <script>
       
        var indexPage = 0;
        $(document).ready(function () {
            $('#wizard').smartWizard({
                onFinish: onFinishCallback,
                saveState: true
            });

            $('#wizard_verticle').smartWizard({
                transitionEffect: 'slide',
                saveState: true
            });            
           
            $('.buttonNext').addClass('btn btn-success');
            $('.buttonPrevious').addClass('btn btn-primary');
            $('.buttonFinish').addClass('btn btn-default');
           
            function onFinishCallback(objs, context) {
                if (validateAllSteps()) {
                    $('#finishBtn').click();
                }
            }

            function validateAllSteps() {
                var isStepValid = true;
                // all step validation logic     
                return isStepValid;
            }

            var inps = document.getElementById('uploadFolder');
            [].forEach.call(inps, function (inp) {
                inp.onchange = function (e) {
                    console.log(this.files);
                };
            });

          

         });

            
  

        

    </script>
    <!-- /jQuery Smart Wizard -->    
</body>
</html>