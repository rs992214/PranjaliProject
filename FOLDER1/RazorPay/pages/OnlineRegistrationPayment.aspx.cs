using Razorpay.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccessLayer;
using System.Data;

public partial class OnlineRegistration_Payment : System.Web.UI.Page
{
    public string orderId;
    public string UserID;

    public string Amount;
    public string Pakage;

    public string Key;
    public string secrete;

    public string name;
    public string mobile;
    public string email;

    string Total;

    public decimal PakageAmount = 0;
    DAL dal = new DAL();
    string message = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            HttpCookie reqCookies = Request.Cookies["UserData1"];
            if (reqCookies != null)
            {
                GetApiCredential();
                name = reqCookies["Name"].ToString();
                mobile = reqCookies["Mobile"].ToString();
                email = reqCookies["Email"].ToString();
                Total = reqCookies["Total1"].ToString();

                
                PakageAmount = Convert.ToDecimal(Total) * 100;
                Amount = PakageAmount.ToString();
                
                Dictionary<string, object> input = new Dictionary<string, object>();
                //input.Add("amount", calcolateAmount); // this amount should be same as transaction amount
                input.Add("amount", Amount); // this amount should be same as transaction amount
                input.Add("currency", "INR");
                input.Add("receipt", "12121");
                input.Add("payment_capture", 1);
                RazorpayClient client = new RazorpayClient(Key, secrete);
                Razorpay.Api.Order order = client.Order.Create(input);
                orderId = order["id"].ToString();
            }

        }
        catch (Exception ex)
        {

        }
    }
    public void GetApiCredential()
    {
        DataTable dt = dal.Gettable("SELECT * FROM Razor_Credential_Master", ref message);
        string status = dt.Rows[0]["Razor_Status"].ToString();
        if (status == "Test")
        {
            Key = dt.Rows[0]["Razor_Test_Key"].ToString();
            secrete = dt.Rows[0]["Razor_Test_Secrete"].ToString();
        }
        else
        {
            Key = dt.Rows[0]["Razor_Live_Key"].ToString();
            secrete = dt.Rows[0]["Razor_Live_Secrete"].ToString();
        }

    }
}