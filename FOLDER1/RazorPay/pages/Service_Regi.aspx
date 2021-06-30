<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Service_Regi.aspx.cs" Inherits="Service_Service_Regi" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <title>Service Provider Registration</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <link rel="shortcut icon" href="../../pages/images/favicon.png" />
    <!-- MATERIAL DESIGN ICONIC FONT -->
    <link rel="stylesheet" href="Register/fonts/material-design-iconic-font/css/material-design-iconic-font.min.css">

    <!-- STYLE CSS -->
    <link rel="stylesheet" href="Register/css/style.css">

    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.6.0/dist/css/bootstrap.min.css" integrity="sha384-B0vP5xmATw1+K9KRQjQERJvTumQW0nPEzvF6L/Z6nronJ3oUOFUFpCjEUQouq2+l" crossorigin="anonymous">
    <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js" integrity="sha384-DfXdz2htPH0lsSSs5nCTpuj/zy4C+OGpamoFVy38MVBnE+IbbVYUew+OrCXaRkfj" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.0/dist/js/bootstrap.bundle.min.js" integrity="sha384-Piv4xVNRyMGpqkS2by6br4gNJ7DXjqk09RmUpJ8jgGtD7zP9yug3goQfGII0yAns" crossorigin="anonymous"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-multiselect/0.9.16/css/bootstrap-multiselect.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-multiselect/0.9.16/js/bootstrap-multiselect.js"></script>
    <script type="text/javascript">
        $(function () {
            $('[id*=lstService]').multiselect({
                includeSelectAllOption: true
            });
        });
    </script>

    <script>
        function ValidNumeric() {

            var charCode = (event.which) ? event.which : event.keyCode;
            if (charCode >= 48 && charCode <= 57) { return true; }
            else { return false; }
        }
    </script>


    <style>
        .inner {
            margin: 40px 150px;
            background: url(Register/images/bg-004.jpg);
            background-size: cover;
            background-position: center
        }

        .form-wrapper {
            margin-bottom: 17px;
            width: 50%;
            padding: 0 20px;
        }

        .row {
            display: -ms-flexbox;
            display: flex;
            -ms-flex-wrap: wrap;
            flex-wrap: wrap;
            margin-right: -15px;
            margin-left: -15px;
        }

        @media only screen and (max-width: 600px) {
            .form-wrapper {
                margin-bottom: 17px;
                width: 100%;
                padding: 0 20px;
            }

            .inner {
                margin: 20px 10px;
                padding: 10px
            }
        }
    </style>


    <script type="text/javascript">
        function ValidateListBox(sender, args) {
            var options = document.getElementById("<%=lstService.ClientID%>").options;
            for (var i = 0; i < options.length; i++) {
                if (options[i].selected == true) {
                    args.IsValid = true;
                    return;
                }
            }
            args.IsValid = false;
        }
    </script>

</head>


<body>

    <div class="wrapper" style="background: #01d6a3">
        <div class="inner">
            <form action="" runat="server">
                <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                <h3>Service Provider Registration</h3>
                <!--New --->
                <%--<div class="row">--%>
                <p style="display:none">
                    <a class="btn btn-primary" data-toggle="collapse" href="#collapseExample" role="button" aria-expanded="false" aria-controls="collapseExample">Select Service
                    </a>
                    <%--   <button class="btn btn-primary" type="button" data-toggle="collapse" data-target="#collapseExample" aria-expanded="false" aria-controls="collapseExample">
                        Button with data-target
                    </button>--%>
                </p>
                <div class="collapse" id="collapseExample">
                    <div class="card card-body">
                        <div class="row">
                            <asp:Repeater ID="rptCategory" runat="server" OnItemDataBound="rptCategory_ItemDataBound">
                                <ItemTemplate>
                                    <div class="col-md-3">
                                        <asp:Label ID="lblCID" runat="server" Text='<%# Eval("Service_CategoryID") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblCname" runat="server" Text='<%# Eval("Service_CategoryName") %>' Style="font-weight: 600; color: blueviolet"></asp:Label>
                                        <asp:Repeater ID="rptSubCategory" runat="server">
                                            <ItemTemplate>
                                                <div class="">
                                                    <asp:CheckBox ID="cbInterest" runat="server" Data-Id='<%# DataBinder.Eval(Container.DataItem, "ServiceId") %>' Text='<%# DataBinder.Eval(Container.DataItem, "ServiceName") %>' />
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                        <div>
                            <asp:Button ID="btnSelected" runat="server" CssClass="btn btn-danger" Text="Selected" OnClick="btnSelected_Click" Visible="false" />
                        </div>

                    </div>
                </div>
                <%--</div>--%>
                <!--NEw End-->

                <div class="row">
                    <div class="form-wrapper" style="display: none">
                        <label for="">Services</label>
                        <asp:ListBox ID="lstService" runat="server" SelectionMode="Multiple" CssClass="form-control"></asp:ListBox>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="lstService" ErrorMessage="Select Service" InitialValue="0" ValidationGroup="SaveService" SetFocusOnError="true" Display="Dynamic" EnableClientScript="true"></asp:RequiredFieldValidator>--%>
                        <%-- <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="*Required" ControlToValidate="lstService"
ClientValidationFunction = "ValidateListBox"></asp:CustomValidator>--%>
                    </div>

                    <div class="form-wrapper">
                        <label for="">Service Provider ID</label>
                        <asp:TextBox ID="txtHelperID" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                    </div>

                </div>

                <div class="row">
                    <div class="form-wrapper" style="display: none">
                        <label for="">Services</label>
                        <asp:DropDownList ID="ddlServices" runat="server" CssClass="form-control"></asp:DropDownList>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlServices" ErrorMessage="Select Service" InitialValue="0" ValidationGroup="SaveService" SetFocusOnError="true" Display="Dynamic" EnableClientScript="true"></asp:RequiredFieldValidator>--%>
                    </div>

                    <div class="form-wrapper">
                        <%-- <asp:UpdatePanel ID="update2" runat="server">
                            <ContentTemplate>--%>
                        <label for="">Sponsor ID</label>
                        <asp:TextBox ID="txtSponsorID" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtSponsorID_TextChanged"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtSponsorID" ErrorMessage="Please Enter Sponsor ID Name" ForeColor="Red" ValidationGroup="SaveService"></asp:RequiredFieldValidator>
                        <asp:Label ID="lblAvailable" runat="server" Visible="false" ForeColor="Green"></asp:Label>
                        <asp:Label ID="lblNotAvailable" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                        <%--</ContentTemplate>
                        </asp:UpdatePanel>--%>
                    </div>
                    <div class="form-wrapper">
                        <label for="">Sponsor Name</label>
                        <asp:TextBox ID="txtSponsorName" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtSponsorName" ErrorMessage="Please Enter Sponsor Name" ForeColor="Red" ValidationGroup="SaveService"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-wrapper">
                        <label for="">Full Name</label>
                        <asp:TextBox ID="txtname" runat="server" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rpf1" runat="server" ControlToValidate="txtname" ErrorMessage="Please Enter Full Name" ForeColor="Red" ValidationGroup="SaveService"></asp:RequiredFieldValidator>
                    </div>

                    <div class="form-wrapper">
                        <asp:UpdatePanel ID="update1" runat="server">
                            <ContentTemplate>
                                <label for="">Mobile</label>
                                <asp:TextBox ID="txtmobile" runat="server" CssClass="form-control" onkeypress="return ValidNumeric()" AutoPostBack="true" OnTextChanged="txtmobile_TextChanged"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtmobile" ErrorMessage="Please Enter Mobile Number" ForeColor="Red" ValidationGroup="SaveService"></asp:RequiredFieldValidator>
                                <asp:Label ID="lblEMsgMobile" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                                <asp:Label ID="lblSMsgMobile" runat="server" Visible="false" ForeColor="Green"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                    <div class="form-wrapper">
                        <label for="">Email</label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEmail" ErrorMessage="Please Enter Email ID" ForeColor="Red" ValidationGroup="SaveService"></asp:RequiredFieldValidator>
                    </div>

                    <div class="form-wrapper">
                        <label for="">Work Time</label>
                        <asp:DropDownList ID="ddlWorkTime" runat="server" CssClass="form-control"></asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlWorkTime" ErrorMessage="Please Select Work Time" InitialValue="0" ValidationGroup="SaveService"></asp:RequiredFieldValidator>
                    </div>

                    <div class="form-wrapper" style="display: none;">
                        <label for="">Password</label>
                        <asp:TextBox ID="txtpassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtpassword" ErrorMessage="Please Enter Password" ForeColor="Red" ValidationGroup="SaveService1"></asp:RequiredFieldValidator>

                    </div>
                    <div class="form-wrapper" style="display: none;">
                        <label for="">Confirm Password</label>
                        <asp:TextBox ID="txtcnfpassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                        <%-- <asp:CompareValidator ID="comparePasswords"
                            runat="server"
                            ControlToCompare="txtpassword"
                            ControlToValidate="txtcnfpassword"
                            ErrorMessage="Your passwords do not match up!"
                            Display="Dynamic" Font-Names="Verdana" ForeColor="#666666" />--%>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtcnfpassword" ErrorMessage="Please Enter Confirm Password" ForeColor="Red" ValidationGroup="SaveService1"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-wrapper">
                        <br />
                        <br />
                        <div class="checkbox">
                            <label>
                                <input type="checkbox">
                                I accept the Terms of Use & Privacy Policy.
							<span class="checkmark"></span>
                            </label>
                        </div>
                    </div>
                </div>

                <%--<button>Register Now</button>--%>
                <%--<asp:LinkButton ID="btnSubmit" CssClass="button" runat="server" OnClick="btnSubmit_Click" ValidationGroup="SaveService">Register Now</asp:LinkButton>--%>
                <asp:Button ID="btnSubmit" CssClass="button" runat="server" OnClick="btnSubmit_Click" ValidationGroup="SaveService" Text="Register Now" />
                <br />
                <br />
                <div style="text-align: center">
                    <p style="color: #000">Already have an account? <a href="login" style="color: #ae3c33">Login</a></p>
                    &nbsp&nbsp
                 <%--   <a href="/"><i class="fa fa-home"></i>Home</a>--%>
                </div>
            </form>
        </div>
    </div>

</body>
</html>
