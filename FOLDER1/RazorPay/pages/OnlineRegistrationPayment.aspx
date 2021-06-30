<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OnlineRegistrationPayment.aspx.cs" Inherits="OnlineRegistration_Payment" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
      <title>
        Caring Clone Payment
    </title>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <style>
        body {
            margin: 0;
            font-size: 28px;
            font-family: Arial, Helvetica, sans-serif;
        }

        .header {
            background-color: #f1f1f1;
            padding: 30px;
            text-align: center;
        }

        #navbar {
            overflow: hidden;
            background-color: #333;
        }

            #navbar a {
                float: left;
                display: block;
                color: #f2f2f2;
                text-align: center;
                padding: 14px 16px;
                text-decoration: none;
                font-size: 17px;
            }

                #navbar a:hover {
                    background-color: #333333;
                    color: black;
                }

                #navbar a.active {
                    background-color: #333333;
                    color: white;
                }

        .content {
            padding: 16px;
        }

        .sticky {
            position: fixed;
            top: 0;
            width: 100%;
        }

            .sticky + .content {
                padding-top: 60px;
            }

        .myDiv {
            /*border: 5px outset red;*/
            background-color: #f2f2f2;
            text-align: center;
        }
    </style>
</head>
<body>
       <div id="navbar">
       
        <a href="javascript:void(0)"><img src="../../Content/CompanyLogo/CC.png" height="70" width="200px"></a>
        
    </div>
    <div class="content">
  <div class="myDiv">
  <h2>Welcome To Caring Clone</h2>
  <p>For Make Payment Please Click On Pay Now Button.</p>
    <form action="OnlineRegistrationCharge.aspx" method="post">
<script
    src="https://checkout.razorpay.com/v1/checkout.js"
   <%-- data-key="rzp_test_0aNUEMulM0Nd5B"--%>   <%--TEST--%>
    <%--data-key="rzp_live_1R5wlRzgyhgodB"--%>  <%--LIVE--%>
    data-key="<%=Key %>" <%--LIVE--%>
    data-amount="<%=Amount%>"
    data-name="CaringClone"
    data-description="<%=Pakage%>""
    data-order_id="<%=orderId%>"
    data-image="../../Content/CompanyLogo/CC.png"
    data-prefill.name="<%=name%>"
    data-prefill.email="<%=email%>"
    data-prefill.contact="<%=mobile%>"
    <%--data-theme.color="#17499d"--%>
     data-theme.color="#D4AF37"
></script>
<input type="hidden" value="Hidden Element" name="hidden"/>
     <a href="/"><input type="button"  name="Home" value="Home"/></a>
        </form>
      </div>

        </div>
</body>
</html>
