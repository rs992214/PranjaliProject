using DataAccessLayer;
using Newtonsoft.Json;
using Razorpay.Api;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class OnlineRegistration_Charge : System.Web.UI.Page
{
    string constring1 = ConfigurationManager.ConnectionStrings["CN"].ConnectionString;
    SqlConnection con;
    SqlCommand cmd;
    string Size, Colour;
    string message = string.Empty;
    string custid = string.Empty;
    DAL dal = new DAL();
    int orderno;
    string Address = string.Empty;
    string state = string.Empty;
    string UserId = string.Empty;
    string City = string.Empty;

    string Total = string.Empty;
    string HelperID = string.Empty;
    string SponsorID = string.Empty;
    string Name = string.Empty;
    string Mobile = string.Empty;
    string Email = string.Empty;
    string Password = string.Empty;
    string ServiceID = string.Empty;
    string WorkTimeID = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            HttpCookie reqCookies = Request.Cookies["UserData1"];
            if (reqCookies != null)
            {
                HelperID = reqCookies["HelperID"].ToString();
                SponsorID = reqCookies["SponsorID"].ToString();
                Name = reqCookies["Name"].ToString();
                Password = reqCookies["Password"].ToString();
                Mobile = reqCookies["Mobile"].ToString();
                Email = reqCookies["Email"].ToString();
                ServiceID = reqCookies["ServiceID"].ToString();
                WorkTimeID = reqCookies["WorkTimeID"].ToString();
                string connstring = ConfigurationManager.ConnectionStrings["CN"].ConnectionString;
                SqlConnection con = new SqlConnection(connstring);
                con.Open();
                SqlCommand cmd = new SqlCommand("Auth_ServiceProviderRegister", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HelperID", HelperID);
                cmd.Parameters.AddWithValue("@SponsorID", SponsorID);
                cmd.Parameters.AddWithValue("@Name", Name);
                cmd.Parameters.AddWithValue("@Password", Password);
                cmd.Parameters.AddWithValue("@Mobile", Mobile);
                cmd.Parameters.AddWithValue("@Email", Email);
                cmd.Parameters.AddWithValue("@ServiceID", ServiceID);
                cmd.Parameters.AddWithValue("@WorkTimeID", WorkTimeID);
                cmd.Parameters.AddWithValue("@Mode", "Service_Reg");
                int flag = cmd.ExecuteNonQuery();
                if (flag > 0)
                {
                    //con.Close();
                    //ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Congratulation,Dear "+ Name + " Registration has been created successfully... ');window.location='Service_Regi.aspx';", true);
                    SendMsg();
                    SendMail();
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('You Have Been Successfully Registered');window.location='Service_Regi.aspx';", true);
                }

            }
        }
        catch (Exception ex)
        {
        }
    }
    public void Gettxid()
    {
        Random rnd = new Random();
        string strHash = Generatehash512(rnd.ToString() + DateTime.Now);
        ViewState["txid"] = strHash.ToString().Substring(0, 20);

    }
    public string Generatehash512(string text)
    {

        byte[] message = Encoding.UTF8.GetBytes(text);

        UnicodeEncoding UE = new UnicodeEncoding();
        byte[] hashValue;
        SHA512Managed hashString = new SHA512Managed();
        string hex = "";
        hashValue = hashString.ComputeHash(message);
        foreach (byte x in hashValue)
        {
            hex += String.Format("{0:x2}", x);
        }
        return hex;
    }
    public string GetUniqueKey(int maxSize)
    {
        char[] chars = new char[62];
        chars = "123456789".ToCharArray();
        byte[] data = new byte[1];
        RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
        crypto.GetNonZeroBytes(data);
        data = new byte[maxSize];
        crypto.GetNonZeroBytes(data);
        System.Text.StringBuilder result = new System.Text.StringBuilder(maxSize);
        foreach (byte b in data)
        {
            result.Append(chars[b % (chars.Length)]);
        }
        return result.ToString();
    }

    //------------------------------------------------- SMS-----------------------------------------------------------
    public void SendMsg()
    {
        DAL dal = new DAL();
        string url = "";
        string username = "";
        string key = "";
        string request = "";
        string sender = "";
        string route = "";
        int sms = 0;
        string status = "";
        string message = string.Empty;

        DataTable dt = dal.Gettable("select APIurl,Username,APIkey,APIrequest,Sender,Route,isnull(CreditSMS,0) as CreditSMS,Status from SmsmasterNew where Status='Active'", ref message);
        if (dt.Rows.Count > 0)
        {
            url = dt.Rows[0]["APIurl"].ToString();
            username = dt.Rows[0]["Username"].ToString();
            key = dt.Rows[0]["APIkey"].ToString();
            request = dt.Rows[0]["APIrequest"].ToString();
            sender = dt.Rows[0]["Sender"].ToString();
            route = dt.Rows[0]["Route"].ToString();
            sms = Convert.ToInt32(dt.Rows[0]["CreditSMS"].ToString());

            if (sms != 0)
            {
                string text = "Congratulations, Dear " + Name + " Your Account Has Been Created Successfully. Thank You. ";
                try
                {
                    string jsonValue = "";
                    string sURL;
                    StreamReader objReader;
                    // sURL = "http://203.129.225.69/API/WebSMS/Http/v1.0a/index.php?username=" + username + "&password=" + password + "&sender=" + senderId + "&to=" + txtmobileNo.Text + "&message=" + text + " &reqid=1&format={json|text}&route_id=" + routeId + "";
                    // sURL = "http://sms.probuztech.com/sms-panel/api/http/index.php?username=WYSE&apikey=FC144-3DD84&apirequest=Text&sender=PROBUZ&mobile=8055002299&message=TEST&route=TRANS&format=JSON";
                    //sURL = "" + url + "?username=" + username + "&apikey=" + key + "&apirequest=" + request + "&sender=" + sender + "&mobile=" + txtmob.Text + "&message=" + text + "&route=" + route + "&format=JSON";
                    sURL = "" + url + "?username=" + username + "&apikey=" + key + "&apirequest=" + request + "&sender=" + sender + "&mobile=" + Mobile + "&message=" + text + "&route=" + route + "&TemplateID=1707161744326123203&format=JSON";
                    WebRequest wrGETURL;
                    wrGETURL = WebRequest.Create(sURL);
                    try
                    {
                        Stream objStream;
                        objStream = wrGETURL.GetResponse().GetResponseStream();
                        objReader = new StreamReader(objStream);
                        jsonValue = objReader.ReadToEnd();
                        var myDetails = JsonConvert.DeserializeObject<MyDetail>(jsonValue);
                        string status1 = myDetails.status;
                        if (status1 == "error")
                        {
                            SMSHistory(Mobile, Name, Mobile, text, "Not Send");
                        }
                        else
                        {
                            SMSHistory(Mobile, Name, Mobile, text, "Send");
                        }

                        objReader.Close();
                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                    }
                }
                catch (Exception)
                {
                    throw;
                }

            }

        }

    }
    protected void SMSdebit()
    {
        int sms = 0;
        int smsAvailable = 0;
        DataTable dt = dal.Gettable("select APIurl,Username,APIkey,APIrequest,Sender,Route,isnull(CreditSMS,0) as CreditSMS,Status from SmsmasterNew where Status='Active'", ref message);
        if (dt.Rows.Count > 0)
        {
            sms = Convert.ToInt32(dt.Rows[0]["CreditSMS"].ToString());
            smsAvailable = sms - 1;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Update SmsmasterNew set CreditSMS='{0}'", smsAvailable);
            try
            {
                int rowaffected = dal.Executequery(sb.ToString(), ref message);
                if (rowaffected > 0)
                {

                }
                else
                {
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {

            }

        }


    }
    protected void SMSHistory(string userid, string name, string mobile, string msg, string sts)
    {
        try
        {
            DAL dal = new DAL();
            string message = string.Empty;
            StringBuilder sba = new StringBuilder();
            sba.AppendLine("insert into SMSHistory(UserID,Name,Mobileno,Message,Status)");
            sba.AppendFormat("values('{0}','{1}','{2}','{3}','{4}')", userid, name, mobile, msg, sts);
            int rowaffected1 = dal.Executequery(sba.ToString(), ref message);
            if (rowaffected1 > 0)
            {

            }
        }
        catch (Exception ex)
        {

        }

    }
    public class MyDetail
    {
        public string status
        {
            get;
            set;
        }
    }
    //-------------------------------------------------- END ---------------------------------------------------------

    //--------------------------------------------- Email ---------------------------------------------------------
    public void SendMail()
    {
        DAL dal = new DAL();
        string email = "";
        string password = "";
        string smtp = "";
        int port = 0;
        Boolean ssl = false;
        string logolink = "";
        string loginlink = "";

        string message2 = string.Empty;
        string companyname = string.Empty;

        DataTable dtcomapny = dal.Gettable("select CompanyName from Company_Registration ", ref message2);
        if (dtcomapny.Rows.Count > 0)
        {
            companyname = dtcomapny.Rows[0]["CompanyName"].ToString();
        }


        string message1 = string.Empty;
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("select EmailID,Password,SMTP,PortNo,EnableSSl,Logolink,Loginlink from EmailConfig where Status='Active'");
        DataTable dt = dal.Gettable(sb.ToString(), ref message1);
        if (dt.Rows.Count > 0)
        {
            email = dt.Rows[0]["EmailID"].ToString();
            password = dt.Rows[0]["Password"].ToString();
            smtp = dt.Rows[0]["SMTP"].ToString();
            port = Convert.ToInt32(dt.Rows[0]["PortNo"].ToString());
            ssl = Convert.ToBoolean(dt.Rows[0]["EnableSSl"].ToString());
            logolink = dt.Rows[0]["Logolink"].ToString();
            loginlink = dt.Rows[0]["Loginlink"].ToString();

            //gather email from form textbox
            string remail = Email;
            //MailAddress from = new MailAddress("info@paxpluewealth.com");
            MailAddress from = new MailAddress(email);
            MailAddress to = new MailAddress(remail);
            MailMessage message = new MailMessage(from, to);
            message.Subject = companyname + " Registration Success";
            string note = "<!DOCTYPE html>";
            note += "<html><body>";
            note += "<h1><img src='" + logolink + "' height=100px width=150px></h1>";
            note += "<p>Hello <b>'" + Name + "'</b>,</p>";
            note += "<p>Welcome to <b>" + companyname + "</b>, You have Registered successfully with us. Your Mobile No: " + Mobile + ".</p>";
            note += "<p>Following are the log-in Credential.</p>";
            //note += "<p><blink><a href='" + loginlink + "' target='_blank'>Click Here</a></blink></p>";
            //note += "<p>Username : <b>'" + Username1 + "'</b></p>";
            //note += "<p>Password : <b>'" + userPassword + "'</b></p>";
            //note += "<br><br><br>";
            note += "<p>Regards</p>";
            note += "<p><a href='" + loginlink + "' target='_blank'>" + companyname + "</a><p>";
            note += "</body></html>";
            message.Body = note;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient(smtp, port);
            client.UseDefaultCredentials = false;
            client.EnableSsl = ssl;
            client.Credentials = new NetworkCredential(email, password);
            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                //error message?
            }
            finally
            {

            }

        }

    }
    //------------------------------------------------ END --------------------------------------------------------------

}