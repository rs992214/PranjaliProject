<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CallBack.aspx.cs" Inherits="CallBack" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta charset="utf-8" />
    <title>Paytm Callback</title>
    <meta name="description" content="User login page" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0" />
    <link type="text/css" rel="stylesheet" href="login-resource/css/bootstrap.min.css" />
    <link type="text/css" rel="stylesheet" href="login-resource/fonts/font-awesome/css/font-awesome.min.css" />
    <link type="text/css" rel="stylesheet" href="login-resource/fonts/flaticon/font/flaticon.css" />

    <!-- Favicons -->
    <%--<link href="assets/img/logoagm.png" rel="icon">--%>
    <link rel="shortcut icon" href="img/logos-sliders/favicon.png">
    <!-- Google fonts -->
    <link rel="stylesheet" type="text/css" href="https://fonts.googleapis.com/css?family=Open+Sans:400,300,600,700,800%7CPoppins:400,500,700,800,900%7CRoboto:100,300,400,400i,500,700" />

    <!-- Custom Stylesheet -->
    <link type="text/css" rel="stylesheet" href="login-resource/css/style.css" />
    <link rel="stylesheet" type="text/css" href="login-resource/css/skins/default.css" />

    <style>
        .btn1 {
            margin-right: 500px;
        }


        @media screen and (max-width: 355px) and (min-width: 320px) {
            .btn1 {
                margin-right: 230px;
            }
        }

        @media screen and (max-width: 360px) and (min-width: 356px) {
            .btn1 {
                margin-right: 230px;
            }
        }

        @media screen and (max-width:425px) and (min-width: 375px) {
            .btn1 {
                margin-right: 280px;
            }
        }

        @media screen and (max-width:768px) and (min-width: 426px) {
            .btn1 {
                margin-right: 530px;
            }
        }


        .popupHeader {
            width: 410px;
            text-align: left;
            margin-right: 5px;
        }



        @media screen and (max-width: 375px) and (min-width: 320px) {
            .popupHeader {
                text-align: left;
                margin-left: 40px;
            }
        }

        @media screen and (max-width: 375px) and (min-width: 320px) {
            .btn2 {
                text-align: justify;
                margin-left: 25px;
                margin-right: 50px;
            }
        }


        .text5 {
            margin-top: 8px;
        }


        @media screen and (max-width: 355px) and (min-width: 320px) {
            .text5 {
                margin-top: 25px;
                padding-left: 10px;
            }
        }

        @media screen and (max-width: 360px) and (min-width: 356px) {
            .text5 {
                margin-top: 25px;
                padding-left: 10px;
            }
        }

        @media screen and (max-width:425px) and (min-width: 375px) {
            .text5 {
                margin-top: 25px;
                padding-left: 10px;
            }
        }

        @media screen and (max-width:768px) and (min-width: 426px) {
            .text5 {
                margin-top: 25px;
                padding-left: 10px;
            }
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
        <div class="login-22">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-lg-6 col-md-12 bg-img none-992">
                        <div class="info">
                            <h1>Welcome !</h1>
                            <p>Register Now</p>
                        </div>
                    </div>

                    <div class="col-lg-6 col-md-12 bg-color-13">

                        <div class="form-section" style="max-width: 500px;">

                            <div class="logo clearfix">

                                <a href="/">
                                    <asp:Image ID="imgLogo3" runat="server" alt="COMPANY" style="width: 120px;" />
                                    <%--<img src="assets/img/logoagm.png" alt="logo" style="width: 120px;">--%>
                                </a>

                            </div>

                            <!--<h3>Create an account</h3>-->
                            <div class="btn-section clearfix" style="box-shadow: none">
                                <div class="btn-section clearfix" style="margin-top: -25px;">
                                    <a href="login.aspx" class="link-btn active btn-1 default-bg">Login</a>
                                    <a href="register.aspx" class="link-btn btn-2  active-bg">Register</a>
                                </div>

                                <a href="Default.aspx" class="link-btn btn-2  btn1  active-bg">Home</a>
                            </div>

                            <div class="login-inner-form">
                                <div class="form-group form-box">
                                    <asp:TextBox ID="txtSponsorid" runat="server" CssClass="input-text" placeholder="Sponsor ID" AutoPostBack="true" OnTextChanged="txtSponsorid_TextChanged1" style="text-transform:uppercase;"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Enter Sponsor ID" Display="Dynamic" ForeColor="Red" ControlToValidate="txtSponsorid" ValidationGroup="Sign" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    <asp:Label ID="lblsponsorname" runat="server" Style="color: red" Text="" Font-Bold="true"></asp:Label>
                                    <%--<input type="text" class="input-text" placeholder="Reference ID" />--%>
                                    <i class="flaticon-user"></i>
                                </div>
                                <div class="form-group form-box" hidden="hidden">
                                    <asp:DropDownList ID="drppoistion" runat="server" Style="font-size: 14px; color: #616161; border: 1px solid transparent; background: #fff; box-shadow: 0 0 5px rgba(0, 0, 0, 0.2);">
                                        <asp:ListItem Value="0">--Select position--</asp:ListItem>
                                        <asp:ListItem Value="LLeg" Selected="True">LEFT</asp:ListItem>
                                        <asp:ListItem Value="RLeg">RIGHT</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Please Select Position" Display="Dynamic" ForeColor="Red" ControlToValidate="drppoistion" ValidationGroup="Sign" InitialValue="0" SetFocusOnError="true"></asp:RequiredFieldValidator>

                                    <%--<input type="text" class="input-text" placeholder="Sponser ID" />--%>
                                    <i class="flaticon-user"></i>
                                </div>
                                <div class="form-group form-box">
                                    <asp:TextBox ID="txtaadharno" runat="server" CssClass="input-text" placeholder="UserID" ReadOnly="true"></asp:TextBox>
                                    <%--<input type="text" class="input-text" placeholder="Sponser ID" />--%>
                                    <i class="flaticon-user"></i>
                                </div>

                                <div class="form-group form-box">
                                    <asp:TextBox ID="txtusername" runat="server" CssClass="input-text" placeholder="Enter Name"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please Enter Name" Display="Dynamic" ForeColor="Red" ControlToValidate="txtusername" ValidationGroup="Sign" SetFocusOnError="true"></asp:RequiredFieldValidator>

                                    <%-- <input type="text" class="input-text" placeholder="Name" />--%>
                                    <i class="flaticon-user"></i>
                                </div>

                                <div class="form-group form-box">
                                    <%--<input type="text" class="input-text" placeholder="Email Id" />--%>
                                    <asp:TextBox ID="txtEmailid" runat="server" CssClass="input-text" placeholder="Enter Email" Text="" OnTextChanged="txtEmailid_TextChanged" AutoPostBack="true"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Please Enter Email" Display="Dynamic" ForeColor="Red" ControlToValidate="txtEmailid" ValidationGroup="Sign" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmailid"
                                        ForeColor="Red" ValidationExpression="^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
                                        Display="Dynamic" ErrorMessage="Invalid email address" />
                                    <asp:Label ID="lblemail" runat="server" Text="Label" Visible="false"></asp:Label>

                                    <i class="fa fa-envelope"></i>
                                </div>

                                <div class="form-group form-box">
                                    <asp:TextBox ID="txtmobileNo" runat="server" CssClass="input-text" placeholder="Enter Mobile No" MaxLength="10" OnTextChanged="txtmobileNo_TextChanged" AutoPostBack="true"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Please Mobile No" Display="Dynamic" ForeColor="Red" ControlToValidate="txtmobileNo" ValidationGroup="Sign" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    <asp:Label ID="lblmobile" runat="server" Text="Label" Visible="false"></asp:Label>
                                    <%--<input type="text" maxlength="10" class="input-text" placeholder="Mobile No" />--%>
                                    <i class="ace-icon fa fa-phone"></i>
                                </div>

                                <div class="form-group form-box">

                                    <asp:DropDownList ID="ddlGender" runat="server" Style="font-size: 14px; color: #616161; border: 1px solid transparent; background: #fff; box-shadow: 0 0 5px rgba(0, 0, 0, 0.2);">
                                        <asp:ListItem Value="0">--Select Gender--</asp:ListItem>
                                        <asp:ListItem>Male</asp:ListItem>
                                        <asp:ListItem>Female</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Please Select Gender" Display="Dynamic" ForeColor="Red" ControlToValidate="ddlGender" ValidationGroup="Sign" InitialValue="0" SetFocusOnError="true"></asp:RequiredFieldValidator>

                                    <i class="flaticon-user"></i>
                                </div>

                                <%--   <div class="form-group form-box">
                            <input type="text" maxlength="10" class="input-text" placeholder="Adhar Card / Pan Card Number" />
                            <i class="ace-icon fa fa-id-card"></i>
                       </div>--%>

                                <div class="form-group form-box">
                                    <%--<input type="text" class="input-text" placeholder="Password" />--%>
                                    <asp:TextBox ID="txtpassword1" runat="server" CssClass="input-text" placeholder="Enter Password" TextMode="Password"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Please Enter Password" Display="Dynamic" ForeColor="Red" ControlToValidate="txtpassword1" ValidationGroup="Sign" SetFocusOnError="true"></asp:RequiredFieldValidator>

                                    <i class="flaticon-password"></i>
                                </div>
                                <%-- <div class="form-group form-box">
                            <input type="text" class="input-text" placeholder="Re-enter Password" />
                            <i class="flaticon-password"></i>
                       </div>--%>


                                <div class="checkbox clearfix">
                                    <div class="form-check checkbox-theme">
                                        <input class="form-check-input" type="checkbox" value="" id="rememberMe">
                                        <label class="form-check-label" for="rememberMe">
                                            I accept Terms & Condition
                                        </label>
                                    </div>
                                    <a href="#" style="color: blue">Terms & Privacy</a>
                                </div>

                                <div class="form-group">
                                    <asp:Button ID="btnsignup" runat="server" Text="Register" CssClass="btn-md btn-theme btn-block" ValidationGroup="Sign" OnClick="btnsignup_Click1" />                                    
                                    <%--<input type="submit" value="Register"  class="btn-md btn-theme btn-block" />--%>
                                </div>

                                <div class="form-group">
                                </div>
                            </div>
                            <div class="social-list">
                                <a href="#" class="facebook-bg">
                                    <i class="fa fa-facebook"></i>
                                </a>
                                <a href="#" class="twitter-bg">
                                    <i class="fa fa-twitter"></i>
                                </a>
                                <a href="#" class="google-bg">
                                    <i class="fa fa-google"></i>
                                </a>
                                <a href="#" class="linkedin-bg">
                                    <i class="fa fa-linkedin"></i>
                                </a>
                                <a href="#" class="instagram-bg">
                                    <i class="fa fa-instagram"></i>
                                </a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <script src="login-resource/js/jquery-2.2.0.min.js"></script>
        <script src="login-resource/js/popper.min.js"></script>
        <script src="login-resource/js/bootstrap.min.js"></script>
    </form>
</body>
</html>

