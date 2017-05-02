<%@ Page Language="C#" AutoEventWireup="true" CodeFile="pm_info.aspx.cs" Inherits="production_pm_info"  Debug="true"%>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<!DOCTYPE html>
<html lang="en">
 <head>
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
    <link href="../vendors/jqvmap/dist/jqvmap.min.css" rel="stylesheet"/>

    <!-- Custom Theme Style -->
    <link href="../build/css/custom.min.css" rel="stylesheet">
  </head>

  <body class="nav-md">
      <form id="form1" runat="server">
          <div class="container body">
              <div class="main_container">
                  <div class="col-md-3 left_col menu_fixed">
                      <div class="col-md-3 left_col">
                          <div class="left_col scroll-view">
                              <div class="navbar nav_title" style="border: 0;">
                                  <a href="index.html" class="site_title"> <span>PM Report</span></a>
                              </div>
                              <div class="clearfix"></div>
                              <!-- menu profile quick info -->
                              <div class="profile">
                                  <div class="profile_pic">
                                      <img src="images/img.jpg" alt="..." class="img-circle profile_img">
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
                                              <a href="index.aspx"><i class="fa fa-home"></i> Home</a>
                                          </li>
                                          <li>
                                              <a href="GenPM.aspx"><i class="fa fa-edit"></i> Generate PM</a>
                                          </li>
                                          <li>
                                              <a><i class="fa fa-desktop"></i> Search PM</a>
                                          </li>
                                          <li>
                                              <a><i class="fa fa-table"></i> Review PM</a>
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
                                          <li><a href="javascript:;"> Profile</a></li>
                                          <li><a href="login.html"><i class="fa fa-sign-out pull-right"></i> Log Out</a></li>
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
                                          <div class="row">
                                          <div class="col-md-8 col-sm-8 col-xs-8">
                                             <h2><asp:Label ID="hostTitle" runat="server"></asp:Label></h2>
                                          </div>
                                          <div class="col-md-4 col-sm-4 col-xs-4 right">
                                            <div class="pull-right"> 
                                            <asp:Button  CssClass="btn btn-success" runat="server" ID="exportButton" OnClick="exportButton_Click" Text="Export Document" />
                                            </div>
                                          </div>
                                          </div>
                                          <div class="clearfix"></div>


                                      </div>
                                      <div class="x_content">
                                          <!-- Flat -->
                                          <div class="" role="tabpanel" data-example-id="togglable-tabs">
                                              <!-- Head Tab -->
                                              <ul id="myTab" class="nav nav-tabs bar_tabs" role="tablist">
                                                  <li role="presentation" class="active">
                                                      <a href="#tab_content1" id="home-tab" role="tab" data-toggle="tab" aria-expanded="true">Summary</a>
                                                  </li>
                                                  <li role="presentation" class="">
                                                      <a href="#tab_content2" role="tab" id="profile-tab" data-toggle="tab" aria-expanded="false">1</a>
                                                  </li>
                                                  <li role="presentation" class="">
                                                      <a href="#tab_content3" role="tab" id="profile-tab2" data-toggle="tab" aria-expanded="false">2</a>
                                                  </li>
                                                  <li role="presentation" class="">
                                                      <a href="#tab_content4" role="tab" id="profile-tab3" data-toggle="tab" aria-expanded="false">3</a>
                                                  </li>
                                                  <li role="presentation" class="">
                                                      <a href="#tab_content5" role="tab" id="profile-tab4" data-toggle="tab" aria-expanded="false">4</a>
                                                  </li>
                                                  <li role="presentation" class="">
                                                      <a href="#tab_content6" role="tab" id="profile-tab5" data-toggle="tab" aria-expanded="false">5</a>
                                                  </li>
                                                  <li role="presentation" class="">
                                                      <a href="#tab_content7" role="tab" id="profile-tab6" data-toggle="tab" aria-expanded="false">6</a>
                                                  </li>
                                                  <li role="presentation" class="">
                                                      <a href="#tab_content8" role="tab" id="profile-tab7" data-toggle="tab" aria-expanded="false">7</a>
                                                  </li>
                                                  <li role="presentation" class="">
                                                      <a href="#tab_content9" role="tab" id="profile-tab8" data-toggle="tab" aria-expanded="false">8</a>
                                                  </li>                               
                                                  <li role="presentation" class="">
                                                      <a href="#tab_content10" role="tab" id="profile-tab10" data-toggle="tab" aria-expanded="false">9</a>
                                                  </li>
                                                  <li role="presentation" class="">
                                                      <a href="#tab_content11" role="tab" id="profile-tab11" data-toggle="tab" aria-expanded="false">10</a>
                                                  </li>
                                              </ul>
                                              <!-- End Head Tab -->
                                              <!-- Content on Tab -->
                                              <div id="myTabContent" class="tab-content">
                                                  <!-- Tab Summary -->
                                                  <div role="tabpanel" class="tab-pane fade active in" id="tab_content1" aria-labelledby="home-tab">
                                                      <h4><b>Summary Suggestion</b></h4>
                                                      <!-- Database Section -->
                                                         Operating System: The space usage of mount point.<br />
                                                      <center>
                                                        <asp:Chart ID="ChartDataSpace"  runat="server">
                                                              <Series>
                                                                  <asp:Series Name="Series1"></asp:Series>
                                                              </Series>
                                                              <ChartAreas>
                                                                  <asp:ChartArea Name="ChartArea1">
                                                                  </asp:ChartArea>
                                                              </ChartAreas>
                                                          </asp:Chart>
                                                        </center>
                                                      <br />
                                                       <!-- Database Section -->
                                                       Database: Top 10 Tablespace Free Space  <br />
                                                      <center>  
                                                      <asp:Chart ID="TableSpaceChart"  runat="server">
                                                              <Series>
                                                                  <asp:Series Name="Series1"></asp:Series>
                                                              </Series>
                                                              <ChartAreas>
                                                                  <asp:ChartArea Name="ChartArea1">
                                                                  </asp:ChartArea>
                                                              </ChartAreas>
                                                          </asp:Chart>
                                                     </center>
                                                      <!-- End OS Section -->
                                                  </div>
                                                  <!-- End Tab Summary -->
                                                  <!-- Tab 1 -->
                                                  <div role="tabpanel" class="tab-pane fade" id="tab_content2" aria-labelledby="profile-tab">
                                                      <h4><b>1. General Project Information</b></h4>
                                                      <!-- 1.1 -->
                                                      <h5><b>1.1 Project Information</b></h5>
                                                      <table class="table table-striped table-bordered">
                                                          <tbody>
                                                              <tr>
                                                                  <td><b>Project Name</b></td>
                                                                  <td><asp:Label ID="projectName" runat="server"></asp:Label></td>
                                                              </tr>
                                                              <tr>
                                                                  <td><b>Project Code</b></td>
                                                                  <td><asp:Label ID="projectCode" runat="server"></asp:Label></td>
                                                              </tr>
                                                              <tr>
                                                                  <td><b>Sale Name</b></td>
                                                                  <td><asp:Label ID="salePerson" runat="server"></asp:Label></td>
                                                              </tr>
                                                              <tr>
                                                                  <td><b>Phone Number</b></td>
                                                                  <td><asp:Label ID="salePhone" runat="server"></asp:Label></td>
                                                              </tr>
                                                              <tr>
                                                                  <td><b>Email Address</b></td>
                                                                  <td><asp:Label ID="saleEmail" runat="server"></asp:Label></td>
                                                              </tr>
                                                          </tbody>
                                                      </table>
                                                      <!-- End 1.1 -->
                                                      <!-- 1.2 -->
                                                      <h5><b>1.2 Customer Contact Information</b></h5>
                                                      <table class="table table-striped table-bordered">
                                                          <tbody>
                                                              <tr>
                                                                  <td><b>Customer Name</b></td>
                                                                  <td><asp:Label ID="cusName" runat="server"></asp:Label></td>
                                                              </tr>
                                                              <tr>
                                                                  <td><b>Phone Number</b></td>
                                                                  <td><asp:Label ID="cusPhone" runat="server"></asp:Label></td>
                                                              </tr>
                                                              <tr>
                                                                  <td><b>Email Address</b></td>
                                                                  <td><asp:Label ID="cusEmail" runat="server"></asp:Label></td>
                                                              </tr>                                                              
                                                          </tbody>
                                                      </table>
                                                      <!-- End 1.2 -->
                                                      <!-- 1.3 -->
                                                      <h5><b>1.3 MFEC Engineers' Information</b></h5>
                                                      <table class="table table-striped table-bordered">
                                                          <tbody>
                                                              <tr>
                                                                  <td><b>Engineer Name</b></td>
                                                                  <td><asp:Label ID="engName" runat="server"></asp:Label></td>
                                                              </tr>
                                                              <tr>
                                                                  <td><b>Phone Number</b></td>
                                                                  <td><asp:Label ID="engPhone" runat="server"></asp:Label></td>
                                                              </tr>
                                                              <tr>
                                                                  <td><b>Email Address</b></td>
                                                                  <td><asp:Label ID="engEmail" runat="server"></asp:Label></td>
                                                              </tr>
                                                          </tbody>
                                                      </table>
                                                      <!-- End 1.3 -->
                                                      <!-- 1.4 -->
                                                      <h5><b>1.4 Change Record</b></h5>
                                                        <asp:Table ID="authorTable" runat="server" CssClass ="table table-striped table-bordered">
                                                        <asp:TableHeaderRow>
                                                        <asp:TableHeaderCell>Date</asp:TableHeaderCell>
                                                        <asp:TableHeaderCell>Author</asp:TableHeaderCell>
                                                        <asp:TableHeaderCell>Version</asp:TableHeaderCell>
                                                        <asp:TableHeaderCell>Change Reference</asp:TableHeaderCell>
                                                        </asp:TableHeaderRow>
                                                        </asp:Table>
                                                     
                                                      <!-- End 1.4 -->
                                                      <!-- 1.5 -->
                                                      <h5><b>1.5 Reviewer</b></h5>
                                                        <asp:Table ID="reviewerTable" runat="server" CssClass ="table table-striped table-bordered">
                                                            <asp:TableHeaderRow>
                                                                <asp:TableHeaderCell>Date</asp:TableHeaderCell>
                                                                <asp:TableHeaderCell>Name</asp:TableHeaderCell>
                                                                <asp:TableHeaderCell>Position</asp:TableHeaderCell>
                                                            </asp:TableHeaderRow>
                                                        </asp:Table>                                                     
                                                      <!-- End 1.5 -->
                                                  </div>
                                                  <!-- End Tab 1 -->
                                                  <!-- Tab 2 -->
                                                  <div role="tabpanel" class="tab-pane fade" id="tab_content3" aria-labelledby="profile-tab">
                                                      <h4><b>2. Oracle Minimum Requirement</b></h4>
                                                      <!-- 2.1 -->
                                                      <h5><b>2.1 Checking Server Machine Specification</b></h5>
                                                      <table class="table table-striped table-bordered">
                                                          <thead>
                                                              <tr>
                                                                  <th><b></b></th>
                                                                  <th><b>Values</b></th>
                                                              </tr>
                                                          </thead>
                                                          <tbody>
                                                              <tr>
                                                                  <td><b>Hostname</b></td>
                                                                  <td><asp:Label ID="hostname" runat="server"></asp:Label></td>
                                                              </tr>
                                                              <tr>
                                                                  <td><b>IP Address</b></td>
                                                                  <td><asp:Label ID="ipAddress" runat="server"></asp:Label></td>
                                                              </tr>
                                                              <tr>
                                                                  <td><b>Oracle owner</b></td>
                                                                  <td>
                                                                      <table class="table table-striped table-bordered">
                                                                          <thead>
                                                                              <tr>
                                                                                  <th><b>Login</b></th>
                                                                                  <th><b>Home's Directory</b></th>
                                                                                  <th><b>Shell</b></th>
                                                                              </tr>
                                                                          </thead>
                                                                          <tbody>
                                                                              <tr>
                                                                                  <td><asp:Label ID="login" runat="server"></asp:Label></td>
                                                                                  <td><asp:Label ID="homeDirectory" runat="server"></asp:Label></td>
                                                                                  <td><asp:Label ID="shell" runat="server"></asp:Label></td>
                                                                              </tr>
                                                                          </tbody>
                                                                      </table>
                                                                  </td>
                                                              </tr>
                                                              <tr>
                                                                  <td><b>Oracle group</b></td>
                                                                  <td>
                                                                      <table class="table table-striped table-bordered">
                                                                          <thead>
                                                                              <tr>
                                                                                  <th><b>First Group</b></th>
                                                                                  <th><b>Second Group</b></th>
                                                                              </tr>
                                                                          </thead>
                                                                          <tbody>
                                                                              <tr>
                                                                                  <td><asp:Label ID="oracleFirstGroup" runat="server"></asp:Label></td>
                                                                                  <td><asp:Label ID="oracleSecondGroup" runat="server"></asp:Label></td>
                                                                              </tr>
                                                                          </tbody>
                                                                      </table>
                                                                  </td>
                                                              </tr>
                                                          </tbody>
                                                      </table>
                                                      <!-- End 2.1 -->
                                                      <!-- 2.2 -->
                                                      <h5><b>2.2 Compare to Oracle Requirement for <asp:Label ID="hostname4" runat="server"></asp:Label></b></h5></b></h5>
                                                      <table class="table table-striped table-bordered">
                                                          <thead>
                                                              <tr>
                                                                  <th><b>Requirement</b></th>
                                                                  <th><b>Server Specification</b></th>
                                                              </tr>
                                                          </thead>
                                                          <tbody>
                                                              <tr>
                                                                  <td><b><asp:Label ID="osType" runat="server"></asp:Label> OS: </b></td>
                                                                  <td><asp:Label ID="osInfo" runat="server"></asp:Label></td>
                                                              </tr>
                                                              <tr>
                                                                  <td><b><asp:Label  ID="osType1"  runat="server"></asp:Label> Disk Space: </b></td>
                                                                  <td> 
                                                                      <asp:Table ID="diskTable" runat="server" CssClass ="table table-striped table-bordered">
                                                                        <asp:TableHeaderRow>
                                                                            <asp:TableHeaderCell>File System</asp:TableHeaderCell>
                                                                            <asp:TableHeaderCell>Mb Blocks</asp:TableHeaderCell>
                                                                            <asp:TableHeaderCell>Free</asp:TableHeaderCell>
                                                                            <asp:TableHeaderCell>Percent Used</asp:TableHeaderCell>
                                                                            <asp:TableHeaderCell>iUsed</asp:TableHeaderCell>
                                                                            <asp:TableHeaderCell>percent IUsed</asp:TableHeaderCell>
                                                                            <asp:TableHeaderCell>Mounted On</asp:TableHeaderCell>
                                                                        </asp:TableHeaderRow>
                                                                       </asp:Table> 

                                                                  </td>
                                                              </tr>
                                                              <tr>
                                                                  <td><b><asp:Label ID="osType2" runat="server"></asp:Label> RAM: </b></td>
                                                                  <td><asp:Label ID="ram" runat="server"></asp:Label></td>
                                                              </tr>
                                                              <tr>
                                                                  <td><b><asp:Label ID="osType3" runat="server"></asp:Label> Swap: </b></td>
                                                                  <td> <asp:Label ID="swap" runat="server"></asp:Label> </td>
                                                              </tr>
                                                              <tr>
                                                                  <td><b><asp:Label ID="osType4" runat="server"></asp:Label> Tmp: </b></td>
                                                                  <td><asp:Label ID="tmp" runat="server"></asp:Label> </td>
                                                              </tr>
                                                               <tr>
                                                                  <td><b>Java: </b></td>
                                                                  <td><asp:Label ID="java" runat="server"></asp:Label> </td>
                                                              </tr>
                                                               <tr>
                                                                  <td><b>Kernel:</b></td>
                                                                  <td><asp:Label ID="kernel" runat="server"></asp:Label> </td>
                                                              </tr>
                                                          </tbody>
                                                      </table>
                                                      <!-- End 2.2 -->
                                                      <!-- 2.3 -->
                                                      <h5><b>2.3 User's Environment for <asp:Label ID="hostname5" runat="server"></asp:Label></b></h5>
                                                        <asp:Table ID="envTable" runat="server" CssClass ="table table-striped table-bordered">
                                                                <asp:TableHeaderRow>
                                                                    <asp:TableHeaderCell>Parameter</asp:TableHeaderCell>
                                                                    <asp:TableHeaderCell>Value</asp:TableHeaderCell>
                                                                </asp:TableHeaderRow>
                                                       </asp:Table> 
                                                      <!-- End 2.3 -->
                                                  </div>
                                                  <!-- End Tab 2 -->
                                                  <!-- Tab 3 -->
                                                  <div role="tabpanel" class="tab-pane fade" id="tab_content4" aria-labelledby="profile-tab">
                                                      <h4><b>3. System Checklist</b></h4>
                                                      <!-- 3.1 -->
                                                      <h5><b>3.1 Hardware Configuration for <asp:Label ID="hostname6" runat="server"></asp:Label></b></h5>
                                                      <table class="table table-striped table-bordered">
                                                          <thead>
                                                              <tr>
                                                                  <th><b></b></th>
                                                                  <th><b>Values</b></th>
                                                              </tr>
                                                          </thead>
                                                          <tbody>
                                                              <tr>
                                                                  <td><b>System Model</b></td>
                                                                  <td> <asp:Label ID="systemModel" runat="server"></asp:Label>  </td>
                                                              </tr>
                                                              <tr>
                                                                  <td><b>Machine Serial Number</b></td>
                                                                  <td> <asp:Label ID="machineSerialNumber" runat="server"></asp:Label>  </td>
                                                              </tr>
                                                              <tr>
                                                                  <td><b>Processor Type</b></td>
                                                                  <td> <asp:Label ID="processorType" runat="server"></asp:Label>  </td>
                                                              </tr>
                                                              <tr>
                                                                  <td><b>Processor Implementation Mode</b></td>
                                                                  <td> <asp:Label ID="processorImplementationMode" runat="server"></asp:Label></td>
                                                              </tr>
                                                              <tr>
                                                                  <td><b>Processor Version</b></td>
                                                                  <td> <asp:Label ID="processorVersion" runat="server"></asp:Label> </td>
                                                              </tr>
                                                              <tr>
                                                                  <td><b>Number Of Processors </b></td>
                                                                  <td> <asp:Label ID="numOfProc" runat="server"></asp:Label> </td>
                                                              </tr>
                                                              <tr>
                                                                  <td><b>CPU Type</b></td>
                                                                  <td> <asp:Label ID="cpuType" runat="server"></asp:Label> </td>
                                                              </tr>
                                                              <tr>
                                                                  <td><b>Kernel Type</b></td>
                                                                  <td> <asp:Label ID="kernelType" runat="server"></asp:Label> </td>
                                                              </tr>
                                                          </tbody>
                                                      </table>
                                                      <!-- End 3.1 -->
                                                      <!-- 3.2 -->
                                                      <h5><b>3.2 Network Configuration for <asp:Label ID="hostname2" runat="server"></asp:Label> </b></h5>
                                                      <table class="table table-striped table-bordered">
                                                          <thead>
                                                              <tr>
                                                                  <th><b></b></th>
                                                                  <th><b>Values</b></th>
                                                              </tr>
                                                          </thead>
                                                          <tbody>
                                                              <tr>
                                                                  <td><b>IP Address</b></td>
                                                                  <td> <asp:Label ID="ipaddresses" runat="server"></asp:Label> </td>
                                                              </tr>
                                                              <tr>
                                                                  <td><b>Sub Netmask</b></td>
                                                                  <td> <asp:Label ID="subNetMask" runat="server"></asp:Label> </td>
                                                              </tr>
                                                          </tbody>
                                                      </table>
                                                      <!-- End 3.2 -->
                                                      <!-- 3.3 -->
                                                      <h5><b>3.3 Crontab Information for <asp:Label ID="hostname3" runat="server"></asp:Label></b></h5>
                                                      <table class="table table-striped table-bordered">
                                                          <thead>
                                                              <tr>
                                                                  <th><b>Crontab Information</b></th>
                                                              </tr>
                                                          </thead>
                                                          <tbody>
                                                              <tr>
                                                                  <td><asp:Label ID="crontab" runat="server"></asp:Label></td>
                                                              </tr>
                                                          </tbody>
                                                      </table>
                                                      <!-- End 3.3 -->
                                                  </div>
                                                  <!-- End Tab 3 -->
                                                  <!-- Tab 4 -->
                                                  <div role="tabpanel" class="tab-pane fade" id="tab_content5" aria-labelledby="profile-tab">
                                                      <h4><b>4. Database Information</b></h4>
                                                      <!-- 4.1 -->
                                                      <h5><b>4.1 Database Configuration</b></h5>
                                                      <asp:Table ID="databaseConfigTable" runat="server" CssClass ="table table-striped table-bordered">
                                                                <asp:TableHeaderRow>
                                                                    <asp:TableHeaderCell>Parameter</asp:TableHeaderCell>
                                                                    <asp:TableHeaderCell>Value</asp:TableHeaderCell>
                                                                </asp:TableHeaderRow>
                                                                </asp:Table> 
                                                      <!-- End 4.1 -->
                                                      <!-- 4.2 -->
                                                      <h5><b>4.2 Database Parameter</b></h5>
                                                      <asp:Table ID="databaseParameter" runat="server" CssClass ="table table-striped table-bordered">
                                                                <asp:TableHeaderRow>
                                                                    <asp:TableHeaderCell>Name</asp:TableHeaderCell>
                                                                    <asp:TableHeaderCell>Value</asp:TableHeaderCell>
                                                                </asp:TableHeaderRow>
                                                       </asp:Table> 
                                                      <!-- End 4.2 -->
                                                      <!-- 4.3 -->
                                                      <h5><b>4.3 Major Security Initailization Parameters</b></h5>
                                                      <asp:Table ID="majorSecure" runat="server" CssClass ="table table-striped table-bordered">
                                                                <asp:TableHeaderRow>
                                                                    <asp:TableHeaderCell>Name</asp:TableHeaderCell>
                                                                    <asp:TableHeaderCell>Value</asp:TableHeaderCell>
                                                                </asp:TableHeaderRow>
                                                       </asp:Table> 
                                                      <!-- End 4.3 -->
                                                      <!-- 4.4 -->
                                                      <h5><b>4.4 Database Files</b></h5>
                                                      <asp:Table ID="databaseFileTable" runat="server" CssClass ="table table-striped table-bordered">
                                                        <asp:TableHeaderRow>
                                                            <asp:TableHeaderCell>TBS Name</asp:TableHeaderCell>
                                                            <asp:TableHeaderCell>File Name</asp:TableHeaderCell>
                                                            <asp:TableHeaderCell>Size (MB)</asp:TableHeaderCell>
                                                            <asp:TableHeaderCell>Max (MB)</asp:TableHeaderCell>
                                                            <asp:TableHeaderCell>Aut</asp:TableHeaderCell>
                                                            <asp:TableHeaderCell>Inc(block)</asp:TableHeaderCell>
                                                        </asp:TableHeaderRow>
                                                     </asp:Table>
 
                                                      <!-- End 4.4 -->
                                                      <!-- 4.5 -->
                                                      <h5><b>4.5 Temporary Files</b></h5>
                                                      <asp:Table ID="temporaryTable" runat="server" CssClass ="table table-striped table-bordered">
                                                        <asp:TableHeaderRow>
                                                            <asp:TableHeaderCell>TBS Name</asp:TableHeaderCell>
                                                            <asp:TableHeaderCell>File Name</asp:TableHeaderCell>
                                                            <asp:TableHeaderCell>Size (MB)</asp:TableHeaderCell>
                                                            <asp:TableHeaderCell>Max (MB)</asp:TableHeaderCell>
                                                            <asp:TableHeaderCell>Aut</asp:TableHeaderCell>
                                                            <asp:TableHeaderCell>Inc(block)</asp:TableHeaderCell>
                                                        </asp:TableHeaderRow>
                                                     </asp:Table>
                                                      <!-- End 4.5 -->
                                                      <!-- 4.6 -->
                                                      <h5><b>4.6 Redo Log Files</b></h5>
                                                        <asp:Table ID="redoLogFileTable" runat="server" CssClass ="table table-striped table-bordered">
                                                        <asp:TableHeaderRow>
                                                            <asp:TableHeaderCell>Group#</asp:TableHeaderCell>
                                                            <asp:TableHeaderCell>Member</asp:TableHeaderCell>
                                                            <asp:TableHeaderCell>Size (MB)</asp:TableHeaderCell>
                                                        </asp:TableHeaderRow>
                                                     </asp:Table>
                                                      <!-- End 4.6 -->
                                                      <!-- 4.7 -->
                                                      <h5><b>4.7 Control Files</b></h5>
                                                      <asp:Table ID="controlFileTable" runat="server" CssClass ="table table-striped table-bordered">
                                                        <asp:TableHeaderRow>
                                                            <asp:TableHeaderCell>Control File Name</asp:TableHeaderCell>
                                                        </asp:TableHeaderRow>
                                                     </asp:Table>
                                                      <!-- End 4.7 -->
                                                      <!-- 4.8 -->
                                                      <h5><b>4.8 Daily Calendar Worksheet</b></h5>
                                                       <asp:Table ID="dayCalendar" runat="server" CssClass ="table table-striped table-bordered">
                                                        <asp:TableHeaderRow>
                                                            <asp:TableHeaderCell>Time</asp:TableHeaderCell>
                                                            <asp:TableHeaderCell>Description of Housekeeping and Batch Processing Schedule</asp:TableHeaderCell>
                                                            <asp:TableHeaderCell>Estimated Duration</asp:TableHeaderCell>
                                                        </asp:TableHeaderRow>
                                                     </asp:Table>
                                                     
                                                      <!-- End 4.8 -->
                                                      <!-- 4.9 -->
                                                      <h5><b>4.9 Monthly Calendar Worksheet</b></h5>
                                                      <asp:Table ID="monthCalendar" runat="server" CssClass ="table table-striped table-bordered">
                                                        <asp:TableHeaderRow>
                                                            <asp:TableHeaderCell>Day</asp:TableHeaderCell>
                                                            <asp:TableHeaderCell>Description of Housekeeping and Batch Processing Schedule</asp:TableHeaderCell>
                                                            <asp:TableHeaderCell>Estimated Duration</asp:TableHeaderCell>
                                                        </asp:TableHeaderRow>
                                                     </asp:Table>
                                                     <!-- End 4.9 -->
                                                  </div>
                                                  <!-- End Tab 4 -->
                                                  <!-- Tab 5 -->
                                                  <div role="tabpanel" class="tab-pane fade" id="tab_content6" aria-labelledby="profile-tab">
                                                      <h4><b>5. RDBMS Performance</b></h4>
                                                      <!-- 5.1 -->
                                                      <h5><b>5.1 Performance Review</b></h5>
                                                      <asp:Table ID="perfReview" runat="server" CssClass ="table table-striped table-bordered">
                                                        <asp:TableHeaderRow>
                                                            <asp:TableHeaderCell>Information Required to Tune Memory Allocation</asp:TableHeaderCell>
                                                            <asp:TableHeaderCell>Answer</asp:TableHeaderCell>
                                                        </asp:TableHeaderRow>
                                                      </asp:Table> 
                                                      <h5><b>5.2 Database Growth Rate</b></h5>
                                                      <div>Current data information about space usage of database <asp:Label ID="DatabaseNameLabel" runat="server"></asp:Label> as below. <br /></div>
                                                          <table class="table table-striped table-bordered">
                                                            <tbody>
                                                                <tr>
                                                                    <th scope="row">Database Name</th>
                                                                    <td><asp:Label ID="DatabaseNameLabel2" runat="server"></asp:Label></td>
                                                                </tr>
                                                                <tr>
                                                                    <th scope="row">Current Allocated (GB)</th>
                                                                    <td><asp:Label ID="currAllocated" runat="server"></asp:Label></td>
                                                                </tr>
                                                                <tr>
                                                                    <th scope="row">Current Used data (GB) </th>
                                                                    <td><asp:Label ID="currUsed" runat="server"></asp:Label></td>
                                                                </tr>
                                                                <tr>
                                                                    <th scope="row">Percent Used </th>
                                                                    <td><asp:Label ID="percUsed" runat="server"></asp:Label></td>
                                                                </tr>
                                                                <tr>
                                                                    <th scope="row">Allocated Growth(GB) per day </th>
                                                                    <td><asp:Label ID="allocGrowth" runat="server"></asp:Label></td>
                                                                </tr>
                                                                 <tr>
                                                                    <th scope="row">Used Growth(GB) per day  </th>
                                                                    <td><asp:Label ID="usedGrowth" runat="server"></asp:Label></td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                        <div>The database growth rate graph and database size forecast graph as the following graph.  <br /></div>
                                                        <div class="form-group">
                                                        <center>
                                                         <asp:Chart ID="Chart1"  runat="server">
                                                              <Series>
                                                                  <asp:Series Name="Series1"></asp:Series>
                                                              </Series>
                                                              <ChartAreas>
                                                                  <asp:ChartArea Name="ChartArea1">
                                                                  </asp:ChartArea>
                                                              </ChartAreas>
                                                          </asp:Chart>
                                                            </center>
                                                          </div>
                                                          <table class="table table-striped table-bordered">
                                                          <thead>
                                                              <tr>
                                                                  <th> Summary </th>
                                                              </tr>
                                                          </thead>
                                                          <tbody>
                                                              <tr>
                                                                  <td><asp:Label ID="SummaryLabel" runat="server"></asp:Label></td>
                                                              </tr>
                                                          </tbody>
                                                          </table>
                                                  </div>
                                                  <!-- End Tab 5 -->
                                                  <!-- Tab 6 -->
                                                  <div role="tabpanel" class="tab-pane fade" id="tab_content7" aria-labelledby="profile-tab">
                                                      <h4><b>6. Tablespace Free Space</b></h4>
                                                      <!-- 6.1 -->
                                                      <h5><b>6.1 Tablespace Free Space</b></h5>
                                                      <asp:Table ID="freespaceTable" runat="server" CssClass ="table table-striped table-bordered">
                                                        <asp:TableHeaderRow>
                                                            <asp:TableHeaderCell>Tablespace Name</asp:TableHeaderCell>
                                                            <asp:TableHeaderCell>Max Blocks</asp:TableHeaderCell>
                                                            <asp:TableHeaderCell>Count Blocks</asp:TableHeaderCell>
                                                            <asp:TableHeaderCell>Sum Free Blocks</asp:TableHeaderCell>
                                                            <asp:TableHeaderCell>PCT_FREE</asp:TableHeaderCell>
                                                        </asp:TableHeaderRow>
                                                     </asp:Table>
                                                     <!-- End 6.1 -->
                                                  </div>
                                                  <!-- End Tab 6 -->
                                                  <!-- Tab 7 -->
                                                  <div role="tabpanel" class="tab-pane fade" id="tab_content8" aria-labelledby="profile-tab">
                                                      <h4><b>7. Default and Temporary Tablespace </b></h4>
                                                      <!-- 7.1 -->
                                                      <h5><b>7.1 Default Tablespace and Temporary Tablespace</b></h5>
                                                      <asp:Table ID="defAndTemp" runat="server" CssClass ="table table-striped table-bordered">
                                                        <asp:TableHeaderRow>
                                                            <asp:TableHeaderCell>User Name</asp:TableHeaderCell>
                                                            <asp:TableHeaderCell>Default Tablespace</asp:TableHeaderCell>
                                                            <asp:TableHeaderCell>Temporary Tablespace</asp:TableHeaderCell>
                                                        </asp:TableHeaderRow>
                                                     </asp:Table>
                                                      <!-- End 7.1 -->
                                                  </div>
                                                  <!-- End Tab 7 -->
                                                  <!-- Tab 8 -->
                                                  <div role="tabpanel" class="tab-pane fade" id="tab_content9" aria-labelledby="profile-tab">
                                                      <h4><b>8. Database Registry</b></h4>
                                                      <!-- 8.1 -->
                                                      <h5><b>8.1 Check Database Registry</b></h5>
                                                      <asp:Table ID="databaseRegistry" runat="server" CssClass ="table table-striped table-bordered">
                                                        <asp:TableHeaderRow>
                                                            <asp:TableHeaderCell>Comp ID</asp:TableHeaderCell>
                                                            <asp:TableHeaderCell>Version</asp:TableHeaderCell>
                                                            <asp:TableHeaderCell>Status</asp:TableHeaderCell>
                                                            <asp:TableHeaderCell>Last Modified</asp:TableHeaderCell>
                                                        </asp:TableHeaderRow>
                                                     </asp:Table>
                                                      
                                                      <!-- End 8.1 -->
                                                  </div>
                                                  <!-- End Tab 8 -->
                                                  <!-- Tab 9 -->
                                                  <div role="tabpanel" class="tab-pane fade" id="tab_content10" aria-labelledby="profile-tab">
                                                      <h4><b>9. Information from Alert Log</b></h4>
                                                      <!-- 9.1 -->
                                                      <h5><b>Alert Log of <asp:Label ID="alertDatabaeName" runat="server"></asp:Label></b></h5>
                                                      <asp:Panel ID="alertLogPanel" runat="server">
                                                      </asp:Panel>                                                      
                                                      <!-- End 9.1 -->
                                                  </div>
                                                  <!-- End Tab 9 -->
                                                  <!-- Tab 10 -->
                                                  <div role="tabpanel" class="tab-pane fade" id="tab_content11" aria-labelledby="profile-tab">
                                                      <h4><b>10. Backup History</b></h4>
                                                      <!-- Backup Database -->
                                                      <h5><b>Backup Database</b></h5>
                                                      <table class="table table-striped table-bordered">
                                                          <thead>
                                                              <tr>
                                                                  <th> Last Backup Database on</th>
                                                              </tr>
                                                          </thead>
                                                          <tbody>
                                                              <tr>
                                                                  <td><asp:Label ID="backupDB" runat="server"></asp:Label></td>
                                                              </tr>
                                                          </tbody>
                                                      </table>
                                                      <!-- End Backup Database -->
                                                      <!-- Backup Archivelog -->
                                                      <h5><b>Backup Archivelog</b></h5>
                                                      <table class="table table-striped table-bordered">
                                                          <thead>
                                                              <tr>
                                                                  <th> Last Backup Archivelog on</th>
                                                              </tr>
                                                          </thead>
                                                          <tbody>
                                                              <tr>
                                                                  <td><asp:Label ID="backupAL" runat="server"></asp:Label></td>
                                                              </tr>
                                                          </tbody>
                                                      </table>
                                                      <!-- End Backup Archivelog -->
                                                      <!-- Backup Controlfile -->
                                                      <h5><b>Backup Controlfile</b></h5>
                                                      <table class="table table-striped table-bordered">
                                                          <thead>
                                                              <tr>
                                                                  <th> Last Backup Controlfile on</th>
                                                              </tr>
                                                          </thead>
                                                          <tbody>
                                                              <tr>
                                                                  <td><asp:Label ID="backupCF" runat="server"></asp:Label></td>
                                                              </tr>
                                                          </tbody>
                                                      </table>                                                      
                                                      <!-- End Backup Controlfile -->
                                                  </div>
                                                  <!-- End Tab 10 -->
                                              </div>
                                              <!-- End Content on Tab -->
                                          </div>
                                          <!-- End Flat -->

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
          </div>
          </form>
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
              $(document).ready(function () {
                  $('#wizard').smartWizard();

                  $('#wizard_verticle').smartWizard({
                      transitionEffect: 'slide'
                  });

                  $('.buttonNext').addClass('btn btn-success');
                  $('.buttonPrevious').addClass('btn btn-primary');
                  $('.buttonFinish').addClass('btn btn-default');
              });
          </script>
          <!-- /jQuery Smart Wizard -->
</body>
</html>