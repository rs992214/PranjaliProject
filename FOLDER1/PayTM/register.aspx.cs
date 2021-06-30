using DataAccessLayer;
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
using Newtonsoft.Json;
using paytm;

public partial class pages_Default2 : System.Web.UI.Page
{
    string message = string.Empty;
    string message1 = string.Empty;
    DAL dal = new DAL();

    static string userid = string.Empty;
    static string password = string.Empty;
    static string senderid = string.Empty;
    static string route = string.Empty;
    Label CompanyName = new Label();
    protected void Page_Load(object sender, EventArgs e)
    {
        string Refferalid = Request.QueryString["Refferalid"];
        string Position = Request.QueryString["position"];
        ViewState["Placeunder"] = Request.QueryString["Placeunder"];//referlink joining code karan
        getcompanyName();
        Showdatalogo();
        if (!IsPostBack)
        {

            //btnPaytm.Enabled = false;
            if (Refferalid != null)
            {
                txtSponsorid.Text = Refferalid;
                drppoistion.SelectedValue = Position;
                if (Position != "")
                {
                    drppoistion.Enabled = false;
                    txtSponsorid.Enabled = false;
                }
                GetUserID();
                GetNameSporid();


                //btnPaytm.Enabled = false;
            }
            else
            {
                GetUserID();
                //BindState();
                //btnPaytm.Enabled = false;
            }
        }
    }

    public void Showdatalogo()
    {
        try
        {
            CompanyInfo CI = new CompanyInfo();
            DataTable dt = CI.GetData(ref message);
            if (dt.Rows.Count > 0)
            {


                string Logo = dt.Rows[0]["Logo"].ToString();
                if (!string.IsNullOrEmpty(Logo))
                {
                    byte[] bytes = (byte[])dt.Rows[0]["Logo"];
                    string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                    imgLogo3.ImageUrl = "data:image/png;base64," + base64String;

                }
            }
            else
            {
            }
        }
        catch (Exception ex)
        {


        }
    }

    private void GetUserID()
    {
        string connstring = ConfigurationManager.ConnectionStrings["CN"].ConnectionString;
        SqlConnection con = new SqlConnection(connstring);
        con.Open();
        int flag = 1;
        while (flag == 1)
        {
            string COMPNY = "DUP";
            string UserID = GetUniqueKey(6);
            SqlCommand cmd = new SqlCommand("MLM_Registration_ALL", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserID", UserID);
            cmd.Parameters.AddWithValue("@Mode", "CHECK_USERID");
            flag = (int)cmd.ExecuteScalar();
            txtaadharno.Text = COMPNY + UserID;
        }
        con.Close();

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

    public void checkvalidity()
    {
        if (lblsponsorname.Text == "Invalid Sponcor ID / Account Is Inactive" || string.IsNullOrEmpty(lblsponsorname.Text))
        {
            btnPaytm.Enabled = false;
        }
        else
        {
            btnPaytm.Enabled = true;
        }
    }

    private void Save_Counting()
    {
        DAL objDAL = new DAL();
        // string type =
        string type = "LLeg";

        int Current = 0;
        int Previous = 0;
        int Total = 0;
        if (type == "LLeg")
        {
            type = "LEFT";
            DataTable dtCount = objDAL.Gettable("Select UserID,CurrentCount,PreviousCount,Total From LeftCounting Where UserID='" + txtSponsorid.Text + "'", ref message);
            if (dtCount.Rows.Count > 0)
            {
                Current = Convert.ToInt32(dtCount.Rows[0]["CurrentCount"]);
                Previous = Convert.ToInt32(dtCount.Rows[0]["PreviousCount"]);
                Current = Current + 1;
                Total = Previous + Current;
            }
            else
            {
                Current = 1;
                Total = Previous + Current;
            }
        }
        else if (type == "RLeg")
        {
            type = "RIGHT";
            DataTable dtCount = objDAL.Gettable("Select UserID,CurrentCount,PreviousCount,Total From RightCounting Where UserID='" + txtSponsorid.Text + "'", ref message);
            if (dtCount.Rows.Count > 0)
            {
                Current = Convert.ToInt32(dtCount.Rows[0]["CurrentCount"]);
                Previous = Convert.ToInt32(dtCount.Rows[0]["PreviousCount"]);
                Current = Current + 1;
                Total = Previous + Current;
            }
            else
            {
                Current = 1;
                Total = Previous + Current;
            }
        }

        string connstring = ConfigurationManager.ConnectionStrings["CN"].ConnectionString;
        SqlConnection con = new SqlConnection(connstring);
        con.Open();
        SqlCommand cmd = new SqlCommand("LEFT_RIGHT_ALL", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@UserID", txtSponsorid.Text);
        cmd.Parameters.AddWithValue("@CurrentCount", Current);
        cmd.Parameters.AddWithValue("@PreviousCount", Previous);
        cmd.Parameters.AddWithValue("@Total", Total);
        cmd.Parameters.AddWithValue("@Type", type);
        cmd.Parameters.AddWithValue("@Mode", "IN");
        int flag = cmd.ExecuteNonQuery();
    }

    public void SaveRecord(string PlaceunderID)
    {

        //saurabh k
        try
        {
            string date;
            //string joinstring = "/";
            //string[] date = txtdob.Text.Split('/');
            //string finaldate = date[2] + joinstring + date[1] + joinstring + date[0];

            DAL dal = new DAL();
            string message = string.Empty;
            StringBuilder sba = new StringBuilder();
            sba.AppendLine("insert into MLM_Registration(SponsorID,PlaceunderID,UserID,Password,Name,Mobile,Email,JoinType)");
            sba.AppendFormat("values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')", txtSponsorid.Text, PlaceunderID, txtaadharno.Text, txtpassword.Text, txtusername.Text, txtmobileNo.Text, txtEmailid.Text, "Paid");
            int rowaffected1 = dal.Executequery(sba.ToString(), ref message);
            if (rowaffected1 > 0)
            {
                StringBuilder sbb = new StringBuilder();
                sbb.AppendLine("insert into MLM_UserDetail(UserID,Gender)");
                sbb.AppendFormat("values('{0}','{1}')", txtaadharno.Text, ddlGender.SelectedItem.Text);
                int rowaffected2 = dal.Executequery(sbb.ToString(), ref message);
                if (rowaffected1 > 0 && rowaffected2 > 0)
                {


                    //tested mail and msg Start

                    //SendMsg();


                    //tested mail and msg end

                    StringBuilder sb = new StringBuilder();
                    //sb.AppendFormat("Update MLM_Registration set {0}='{1}' where UserID='{2}'", "LLeg", txtaadharno.Text, PlaceunderID);
                    sb.AppendFormat("Update MLM_Registration set {0}='{1}' where UserID='{2}'", drppoistion.SelectedValue, txtaadharno.Text, PlaceunderID);


                    int rowaffected = dal.Executequery(sb.ToString(), ref message);
                    if (rowaffected > 0)
                    {
                        Updatejoining1(txtaadharno.Text, PlaceunderID);
                        SendMail(txtEmailid.Text, txtaadharno.Text, txtpassword.Text);
                        SendMsg();
                    }
                    else
                    {
                        var errormessage = new JavaScriptSerializer().Serialize(message.ToString());
                        var script = string.Format("alert({0});", errormessage);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                    }
                }
            }
            else
            {
                var errormessage = new JavaScriptSerializer().Serialize(message.ToString());
                var script = string.Format("alert({0});", errormessage);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
            }
        }


        catch (Exception ex)
        {
            var errormessage = new JavaScriptSerializer().Serialize(ex.Message.ToString());
            var script = string.Format("alert({0});", errormessage);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
        }
        finally
        {

            //addph();
            //updateepinstatus();

            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Registration Completed Successfully.')", true);
            string msg = string.Empty;
            msg = "Registration Completed Successfully.";
            var errormessage = new JavaScriptSerializer().Serialize(msg.ToString());
            var script = string.Format("alert({0});window.location='register.aspx';", errormessage);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
            //Response.Redirect("auth-logindetails.aspx?userid='" + txtaadharno.Text + "'&password='" + txtpassword.Text + "'&name='" + txtusername.Text + "'");
            Clear();
            GetUserID();
        }
    }

    //public void updateepinstatus()
    //{
    //    StringBuilder sb = new StringBuilder();
    //    sb.AppendFormat("Update PinGenerateNew set Status='Used',UserID='"+txtaadharno.Text+"',ActivationDate=getdate(),UpdationDate=getdate() where PinNo='" + txtPin.Text + "'");
    //    int rowaffected = dal.Executequery(sb.ToString(), ref message);
    //}



    public void InsertDonationtoadmin()
    {
        string mob = "";
        DataTable dtmob = dal.Gettable("Select Mobile from MLM_Registration Where UserID='C1001' ", ref message);
        if (dtmob.Rows.Count > 0)
        {
            mob = dtmob.Rows[0]["Mobile"].ToString();
            StringBuilder sbb1 = new StringBuilder();


            sbb1.AppendLine("insert into DonationNew(UserID,SponsorID,Amount,MobileNo,Pinamount)");


            sbb1.AppendFormat("values('{0}','{1}','{2}','{3}','{4}')", txtaadharno.Text, "C1001", "2500", mob, "250");




            int rowaffected2 = dal.Executequery(sbb1.ToString(), ref message);

        }

    }

    public void InsertDonationtosponsor()
    {
        string mob = "";
        DataTable dtmob = dal.Gettable("Select Mobile from MLM_Registration Where UserID='" + txtSponsorid.Text + "' ", ref message);
        if (dtmob.Rows.Count > 0)
        {
            mob = dtmob.Rows[0]["Mobile"].ToString();
            StringBuilder sbb1 = new StringBuilder();
            //StringBuilder sbb2 = new StringBuilder();
            //StringBuilder sbb3 = new StringBuilder();
            //StringBuilder sbb4 = new StringBuilder();

            sbb1.AppendLine("insert into DonationNew(UserID,SponsorID,Amount,MobileNo)");
            //sbb2.AppendLine("insert into DonationNew(UserID,SponsorID,Amount,MobileNo)");
            //sbb3.AppendLine("insert into DonationNew(UserID,SponsorID,Amount,MobileNo)");
            //sbb4.AppendLine("insert into DonationNew(UserID,SponsorID,Amount,MobileNo)");

            sbb1.AppendFormat("values('{0}','{1}','{2}','{3}')", txtaadharno.Text, "TOP", "250", mob);
            //sbb2.AppendFormat("values('{0}','{1}','{2}','{3}')", txtaadharno.Text, "TOP", "500", mob);
            //sbb3.AppendFormat("values('{0}','{1}','{2}','{3}')", txtaadharno.Text, "TOP", "1000", mob);
            //sbb4.AppendFormat("values('{0}','{1}','{2}','{3}')", txtaadharno.Text, "TOP", "2500", mob);



            int rowaffected2 = dal.Executequery(sbb1.ToString(), ref message);
            //int rowaffected3 = dal.Executequery(sbb2.ToString(), ref message);
            //int rowaffected4 = dal.Executequery(sbb3.ToString(), ref message);
            //int rowaffected5 = dal.Executequery(sbb4.ToString(), ref message);
        }

    }

    //update joining binary
    public void Updatejoining1(string userid, string placeunder)
    {
        DAL dal = new DAL();
        string message = string.Empty;
        string Username = userid;
        string placeunderid = placeunder;
        int joiningcount = 0;
        do
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select PlaceunderID,LLeg,RLeg,ISNULL(LJoining,0)as LJoining,ISNULL(Rjoining,0) as Rjoining from MLM_Registration where UserID='{0}'", placeunderid);
            DataTable dt = dal.Gettable(sb.ToString(), ref message);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["LLeg"].ToString() == Username)
                {
                    joiningcount = Convert.ToInt32(dt.Rows[0]["LJoining"]);
                    joiningcount++;
                    StringBuilder sba = new StringBuilder();
                    sba.AppendFormat("update MLM_Registration set LJoining='{0}' where Userid='{1}'", joiningcount, placeunderid);
                    int rowaffected = dal.Executequery(sba.ToString(), ref message);
                    Username = placeunderid;
                    ViewState["PlaceunderID"] = dt.Rows[0]["PlaceunderID"];
                    if (ViewState["PlaceunderID"] != null)
                    {
                        placeunderid = ViewState["PlaceunderID"].ToString();
                    }
                    else
                    {
                        placeunderid = null;
                    }

                }
                else if (dt.Rows[0]["RLeg"].ToString() == Username)
                {
                    joiningcount = Convert.ToInt32(dt.Rows[0]["RJoining"]);
                    joiningcount++;
                    StringBuilder sba = new StringBuilder();
                    sba.AppendFormat("update MLM_Registration set Rjoining='{0}' where Userid='{1}'", joiningcount, placeunderid);
                    int rowaffected = dal.Executequery(sba.ToString(), ref message);
                    Username = placeunderid;
                    ViewState["PlaceunderID"] = dt.Rows[0]["PlaceunderID"];
                    if (ViewState["PlaceunderID"] != null)
                    {
                        placeunderid = ViewState["PlaceunderID"].ToString();
                    }
                    else
                    {
                        placeunderid = null;
                    }
                }
                else
                {
                    placeunderid = null;
                }

            }
            else
            {
                placeunderid = null;
            }

        } while (!string.IsNullOrEmpty(placeunderid));
    }
    public void ExploringNetwork(string Leg, string UserName)
    {
        DAL dal = new DAL();
        string message = string.Empty;
        bool Condition = true;
        string UserID = UserName;
        string Poisition = Leg;
        do
        {

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select {0} from MLM_Registration where UserID='{1}'", Poisition, UserID);
            object Result = dal.Getscalar(sb.ToString(), ref message);
            if (Result is DBNull)
            {
                SaveRecord(UserID);

                Condition = false;
            }
            else
            {
                if (Result != null)
                {
                    UserID = Result.ToString();
                }
                else
                {
                    SaveRecord(UserID);

                    Condition = false;
                }
            }
        } while (Condition);
    }

    //NEW SMS API
    public string sendSMS()
    {
        String result;
        string apiKey = "pDgVmhh5U7w-8rkRuNEEIv5SmnpCaFvRMcn0JdWLe6";
        // string numbers = "918055002299"; // in a comma seperated list
        string numbers = txtmobileNo.Text;
        string smstext = "Congratulations, Dear Donor Your Account Has Been Created Successfully. Your User ID: '" + txtaadharno.Text + "' and password:'" + txtpassword.Text + "'. Thank You.";


        string message = smstext.ToString();



        string sender = "TXTLCL";

        String url = "https://api.textlocal.in/send/?apikey=" + apiKey + "&numbers=" + numbers + "&message=" + message + "&sender=" + sender;
        //refer to parameters to complete correct url string

        StreamWriter myWriter = null;
        HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);

        objRequest.Method = "POST";
        objRequest.ContentLength = Encoding.UTF8.GetByteCount(url);
        objRequest.ContentType = "application/x-www-form-urlencoded";
        try
        {
            myWriter = new StreamWriter(objRequest.GetRequestStream());
            myWriter.Write(url);
        }
        catch (Exception e)
        {
            return e.Message;
        }
        finally
        {
            myWriter.Close();
        }

        HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
        using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
        {
            result = sr.ReadToEnd();
            // Close and clean up the StreamReader
            sr.Close();
        }
        return result;
    }
    public string sendSMS_new()
    {

        DAL dal = new DAL();
        string website1 = "";
        string txtApiurl = "";
        string txtApikeys = "";
        string txtSender = "";
        string companyname = "";
        string message = string.Empty;

        DataTable dt = dal.Gettable("select api_key,url,sender,Status from Smsmaster Where Status='Active'", ref message);
        DataTable dt1 = dal.Gettable("Select CompanyName,Website from Companyinfo", ref message);

        if (dt.Rows.Count > 0)
        {
            txtApiurl = dt.Rows[0]["url"].ToString();
            txtApikeys = dt.Rows[0]["api_key"].ToString();
            txtSender = dt.Rows[0]["sender"].ToString();
            string status = dt.Rows[0]["Status"].ToString();
            website1 = dt1.Rows[0]["Website"].ToString();
            companyname = dt1.Rows[0]["CompanyName"].ToString();

        }

        //end
        String result;

        string apiKey = txtApikeys.ToString(); ;

        string numbers = txtmobileNo.Text; // in a comma seperated list


        string sms = "Congratulations, Dear '" + txtusername.Text + "' Welcome To " + companyname + " Your Account Created Successfully.Your Customer ID: '" + txtaadharno.Text + "'Password:'" + txtpassword.Text + "'.For More Info Visit '" + website1 + "'";



        String url = "" + txtApiurl + "" + apiKey + "&numbers=" + numbers + "&message=" + sms + "&sender=" + txtSender;
        //refer to parameters to complete correct url string

        StreamWriter myWriter = null;
        HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);

        objRequest.Method = "POST";
        objRequest.ContentLength = Encoding.UTF8.GetByteCount(url);
        objRequest.ContentType = "application/x-www-form-urlencoded";
        try
        {
            myWriter = new StreamWriter(objRequest.GetRequestStream());
            myWriter.Write(url);
        }
        catch (Exception e)
        {
            return e.Message;
        }
        finally
        {
            myWriter.Close();
        }

        HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
        using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
        {
            result = sr.ReadToEnd();

            sr.Close();
        }
        return result;
    }
    //public void SendMsg()
    //{
    //    DAL dal = new DAL();

    //    string username = "";
    //    string password = "";
    //    string senderId = "";
    //    string routeId = "";
    //    string website = "";
    //    string message = string.Empty;

    //    DataTable dt = dal.Gettable("select Userid,Password,Senderid,Routeid from Smsmaster Where Status='Active'", ref message);
    //    DataTable dt1 = dal.Gettable("Select Website from Companyinfo", ref message);


    //    if (dt.Rows.Count > 0)
    //    {
    //        username = dt.Rows[0]["Userid"].ToString();
    //        password = dt.Rows[0]["Password"].ToString();
    //        senderId = dt.Rows[0]["Senderid"].ToString();
    //        routeId = dt.Rows[0]["Routeid"].ToString();
    //        website = dt1.Rows[0]["Website"].ToString();
    //        string text = "Congratulations, Dear '" + txtusername.Text + "' Welcome To MVECREDIT. Your User ID: '" + txtaadharno.Text + "' and password:'" + txtpassword.Text + "'. Visit '" + website + "' Thank You.";
    //        try
    //        {
    //            string sURL;
    //            StreamReader objReader;
    //            sURL = "http://203.129.225.69/API/WebSMS/Http/v1.0a/index.php?username=" + username + "&password=" + password + "&sender=" + senderId + "&to=" + txtmobileNo.Text + "&message=" + text + " &reqid=1&format={json|text}&route_id=" + routeId + "";
    //            WebRequest wrGETURL;
    //            wrGETURL = WebRequest.Create(sURL);

    //            try
    //            {
    //                Stream objStream;
    //                objStream = wrGETURL.GetResponse().GetResponseStream();
    //                objReader = new StreamReader(objStream);
    //                objReader.Close();
    //            }
    //            catch (Exception ex)
    //            {
    //                ex.ToString();
    //            }
    //        }
    //        catch (Exception)
    //        {
    //            throw;
    //        }

    //    }



    //}
    public void Clear()
    {
        txtSponsorid.Text = "";
        lblsponsorname.Text = "";
        txtusername.Text = "";
        txtaadharno.Text = "";
        txtmobileNo.Text = "";
        txtEmailid.Text = "";
        txtpassword.Text = "";
        drppoistion.SelectedIndex = 0;
        ddlGender.SelectedIndex = 0;
        if (ViewState["PlaceunderID"] != null)
        {
            ViewState["PlaceunderID"] = null;
        }

    }

    //protected void txtPin_TextChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        DAL objDAL = new DAL();
    //        DataTable dt = objDAL.Gettable("Select PG.ID,PG.PinNo,P.PackageName,p.Amount,p.DonationAmount From PinGenerateNew PG Inner Join PackageInfo P On P.ID=PG.PackageID Where PinNo='" + txtPin.Text + "' and Status='Unused'", ref message);
    //        if (dt.Rows.Count > 0)
    //        {
    //            txtPin.Text = dt.Rows[0]["PinNo"].ToString();
    //            lblPackageName.Text = dt.Rows[0]["PackageName"].ToString();
    //            lblPkgamt.Text = dt.Rows[0]["Amount"].ToString();
    //            //lblPkgamt.Text = dt.Rows[0]["DonationAmount"].ToString();
    //            _Valid.Visible = true;
    //            _Invalid.Visible = false;
    //            btnPaytm.Enabled = true;
    //        }
    //        else
    //        {
    //            txtPin.Text = string.Empty;
    //            _Valid.Visible = false;
    //            _Invalid.Visible = true;
    //            btnPaytm.Enabled = false;

    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw;
    //    }
    //}

    public void pannumberUpdate()
    {
        string connstring = ConfigurationManager.ConnectionStrings["CN"].ConnectionString;
        SqlConnection con = new SqlConnection(connstring);
        con.Open();
        SqlCommand cmd = new SqlCommand("MLM_Registration_ALL", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@UserID", txtaadharno.Text);
        //cmd.Parameters.AddWithValue("@PANNO", txtpancardno.Text);
        cmd.Parameters.AddWithValue("@Mode", "Updatepannumber");
        int flag = cmd.ExecuteNonQuery();
    }
    public void GetNameSporid()
    {
        DAL dal = new DAL();
        string message = string.Empty;
        StringBuilder sb = new StringBuilder();
        //   sb.AppendFormat("select Name From MLM_Registration where UserID='{0}'", txtsponsorid.Text);
        try
        {
            //DataTable dt = dal.Gettable("select Name From MLM_Registration where UserID='"+txtsponsorid.Text+"'", ref message);
            DataTable dt1 = dal.Gettable("Select * From MLM_Registration where UserID='" + txtSponsorid.Text + "'", ref message);
            if (dt1.Rows.Count > 0)
            {
                btnPaytm.Enabled = true;
                lblsponsorname.ForeColor = System.Drawing.Color.White;
                lblsponsorname.Text = dt1.Rows[0]["Name"].ToString();
            }
            else
            {
                btnPaytm.Enabled = false;
                lblsponsorname.ForeColor = System.Drawing.Color.Red;
                lblsponsorname.Text = "Invalid Sponsor ID / Account Is Inactive";
            }
        }


        catch (Exception ex)
        {
            var errormessage = new JavaScriptSerializer().Serialize(ex.Message.ToString());
            var script = string.Format("alert({0});", errormessage);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
        }
    }
    protected void SendEmail()
    {
        //string email = "";
        //string password = "";
        //DAL dal = new DAL();
        //string message = string.Empty;
        //StringBuilder sb = new StringBuilder();
        //sb.AppendFormat("select Emailid,EmailPassword from Adminlogin");
        //DataTable dt = dal.Gettable(sb.ToString(), ref message);
        //if (dt.Rows.Count > 0)
        //{
        //    email = dt.Rows[0]["Emailid"].ToString();
        //    password = dt.Rows[0]["EmailPassword"].ToString();
        //}

        string date = DateTime.Now.ToString("dd/MM/yyyy");
        string MSG = "<table><tr><th><b>Dear Member,</b></th></tr><tr><td>Congratulations!!! Your Registration has been done Successfully . Your Login Credentials are given Below:</td></tr><tr><td><b>UserID: '" + txtaadharno.Text + "'</b></td></tr><tr><td><b>Password:'" + ViewState["Pass"] + "' </b></td></tr><tr><td><b>Regards<b></td></tr><tr><td>demosite</td></tr><tr><td>http://demosite.com</td></tr><tr><td>Date:'" + date + "'</td></tr></table>";
        const string SERVER = "mail.probuztech.co.in";
        System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
        mail.To.Add(txtEmailid.Text);
        mail.From = new MailAddress("no_reply@probuztech.co.in", "", System.Text.Encoding.UTF8); //r16smile@gmail.com
        mail.Subject = "Registration Confirmation";
        mail.SubjectEncoding = System.Text.Encoding.UTF8;
        mail.Body = MSG;
        mail.BodyEncoding = System.Text.Encoding.UTF8;
        mail.IsBodyHtml = true;
        mail.Priority = MailPriority.High;
        SmtpClient client = new SmtpClient("mail.probuztech.co.in", 587);
        //client.Credentials = new System.Net.NetworkCredential("info@bestopinion.in", "BestOpinion#Info64"); //("r16smile@gmail.com", "9860706979")
        client.Credentials = new System.Net.NetworkCredential("no_reply@probuztech.co.in", "No@123");
        client.UseDefaultCredentials = false;
        try
        {
            client.Send(mail);
            //ClientScript.RegisterStartupScript(GetType(), "alert", "alert('Email sent successfully.');", true); //comment Date :-06-02-2019
        }
        catch (Exception ex)
        {
            Exception ex2 = ex;
            string errorMessage = string.Empty;
            while (ex2 != null)
            {
                errorMessage += ex2.ToString();
                ex2 = ex2.InnerException;
            }
        }


    }

    //public void SendMail(string mailID, string Username1, string userPassword)
    //{
    //    //gather email from form textbox
    //    string remail = mailID;

    //    MailAddress from = new MailAddress("info@paxpluewealth.com");
    //    MailAddress to = new MailAddress(remail);
    //    MailMessage message = new MailMessage(from, to);

    //    message.Subject = "MVECREDIT Registration Success";

    //    //string note = "<div>Hello! <b>'" + txtname.Text + "'</b> </div>";
    //    //note += "<div><br><p>Your Registerd Username IS : <b>'" + Username1 + "'</b> AND Password IS : <b>'" + userPassword + "'</b>. Keep it secured.<br>Thank You.</p></div>";
    //    //note += "<div><br>Regards<br><a href='http://www.paxpluewealth.com/Member/login.aspx'>http://www.paxpluewealth.com</a></div>";

    //    //string note = MailBody(Username1, userPassword, TransactionPassword);

    //    string note = "<!DOCTYPE html>";
    //    note += "<html><body>";
    //    //note += "<h1><img src='http://www.paxpluewealth.com/pages/assets/images/pax.png' height=100px width=100px></h1>";
    //    note += "<h1><img src='http://www.mvecredit.in/pages/Content/front/img/logo/mvecredit122.png' height=100px width=100px></h1>";
    //    note += "<p>Hello <b>'" + txtusername.Text + "'</b>,</p>";
    //    note += "<p>Welcome to <b>MVECREDIT</b>, You have Registered successfully with us. We provide you your login Credentials. Please Keep it secure.</p>";
    //    note += "<p>Following are the log-in Credential.</p>";
    //    note += "<p>Username : <b>'" + Username1 + "'</b></p>";
    //    note += "<p>Password : <b>'" + userPassword + "'</b></p>";
    //    //note += "<p>Transaction Password : <b>'" + TransactionPassword + "'</b></p>";
    //    note += "<br><br><br>";
    //    note += "<p>Regards</p>";
    //    note += "<p><a href='http://www.mvecredit.in/customer/auth-login' target='_blank'>MVECREDIT</a><p>";
    //    note += "</body></html>";

    //    message.Body = note;
    //    message.BodyEncoding = System.Text.Encoding.UTF8;
    //    message.IsBodyHtml = true;

    //    //SmtpClient client = new SmtpClient("mail.probuztech.co.in", 587);
    //    SmtpClient client = new SmtpClient("smtp.paxpluewealth.com", 587);
    //    client.UseDefaultCredentials = false;
    //    client.EnableSsl = false;
    //    client.Credentials = new NetworkCredential("info@paxpluewealth.com", "aXGU(nT0");

    //    try
    //    {
    //        client.Send(message);
    //    }
    //    catch (Exception ex)
    //    {
    //        //error message?
    //    }
    //    finally
    //    {

    //    }
    //}


    protected void txtSponsorid_TextChanged1(object sender, EventArgs e)
    {
        //txtSponsorid.Text = "AG" + txtSponsorid.Text;
        DAL dal = new DAL();
        string message = string.Empty;
        StringBuilder sb = new StringBuilder();
        //txtSponsorid.Text = txtSponsorid.Text.ToUpper();

        try
        {
            DataTable dt1 = dal.Gettable("Select * From MLM_Registration where UserID='" + txtSponsorid.Text + "'", ref message);
            if (dt1.Rows.Count > 0)
            {
                btnPaytm.Enabled = true;
                txtSponsorid.ReadOnly = true;
                lblsponsorname.ForeColor = System.Drawing.Color.White;
                lblsponsorname.Text = dt1.Rows[0]["Name"].ToString();

            }
            else
            {

                lblsponsorname.ForeColor = System.Drawing.Color.Red;
                lblsponsorname.Text = "Invalid Sponsor ID / Account Is Inactive";
                btnPaytm.Enabled = false;
                //btnPaytm.Visible = false;
            }
        }


        catch (Exception ex)
        {
            var errormessage = new JavaScriptSerializer().Serialize(ex.Message.ToString());
            var script = string.Format("alert({0});", errormessage);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
        }
        //checkvalidity();
    }

    protected void btnsignup_Click1(object sender, EventArgs e)
    {

        //referlink joining code karan
        if (ViewState["Placeunder"] != null)
        {
            SaveRecord(ViewState["Placeunder"].ToString());

        }//referlink joining code karan
        else
        {
            DAL dal = new DAL();
            string message = string.Empty;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select {0} From MLM_Registration where UserID='{1}'", drppoistion.SelectedValue, txtSponsorid.Text);
            //sb.AppendFormat("select {0} From MLM_Registration where UserID='{1}'", "LLeg", txtSponsorid.Text);
            try
            {
                object Position = dal.Getscalar(sb.ToString(), ref message);
                if (Position is DBNull)
                {
                    SaveRecord(txtSponsorid.Text);

                }
                else
                {
                    if (Position != null)
                    {
                        ExploringNetwork(drppoistion.SelectedValue, Position.ToString());

                        // ExploringNetwork("LLeg", Position.ToString());


                    }
                    else
                    {
                        SaveRecord(txtSponsorid.Text);
                    }
                }
            }
            catch (Exception ex)
            {
                var errormessage = new JavaScriptSerializer().Serialize(ex.Message.ToString());
                var script = string.Format("alert({0});", errormessage);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
            }
        }

    }

    public void SendMail(string mailID, string Username1, string userPassword)
    {

        string email = "";
        string password = "";
        string smtp = "";
        int port = 0;
        Boolean ssl = false;
        string logolink = "";
        string loginlink = "";


        DAL dal = new DAL();
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
            string remail = mailID;

            //MailAddress from = new MailAddress("info@paxpluewealth.com");
            MailAddress from = new MailAddress(email);

            MailAddress to = new MailAddress(remail);
            MailMessage message = new MailMessage(from, to);

            message.Subject = CompanyName.Text + " Registration Success";

            //string note = "<div>Hello! <b>'" + txtname.Text + "'</b> </div>";
            //note += "<div><br><p>Your Registerd Username IS : <b>'" + Username1 + "'</b> AND Password IS : <b>'" + userPassword + "'</b>. Keep it secured.<br>Thank You.</p></div>";
            //note += "<div><br>Regards<br><a href='http://www.paxpluewealth.com/Member/login.aspx'>http://www.paxpluewealth.com</a></div>";

            //string note = MailBody(Username1, userPassword, TransactionPassword);

            string note = "<!DOCTYPE html>";
            note += "<html><body>";
            note += "<h1><img src='" + logolink + "' height=100px width=100px></h1>";
            note += "<p>Hello <b>'" + txtusername.Text + "'</b>,</p>";
            note += "<p>Welcome to <b>" + CompanyName.Text + "</b>, You have Registered successfully with us. We provide you your login Credentials. Please Keep it secure.</p>";
            note += "<p>Following are the log-in Credential.</p>";
            note += "<p><blink><a href='" + loginlink + "' target='_blank'>Click Here</a></blink></p>";
            note += "<p>Username : <b>'" + Username1 + "'</b></p>";
            note += "<p>Password : <b>'" + userPassword + "'</b></p>";
            note += "<br><br><br>";
            note += "<p>Regards</p>";
            note += "<p><a href='" + loginlink + "' target='_blank'>" + CompanyName.Text + "</a><p>";
            note += "</body></html>";

            message.Body = note;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;

            //SmtpClient client = new SmtpClient("smtp.paxpluewealth.com", 587);
            SmtpClient client = new SmtpClient(smtp, port);
            client.UseDefaultCredentials = false;
            client.EnableSsl = ssl;
            //client.Credentials = new NetworkCredential("info@paxpluewealth.com", "aXGU(nT0");
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
    public void getcompanyName()
    {
        string connstring = ConfigurationManager.ConnectionStrings["CN"].ConnectionString;
        SqlConnection con = new SqlConnection(connstring);
        try
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("select  CompanyName from CompanyInfo", con);
            cmd.CommandType = CommandType.Text;

            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                CompanyName.Text = dt.Rows[0]["CompanyName"].ToString();
            }

        }
        catch (Exception ex)
        {


        }
        finally
        {
            con.Close();
        }


    }

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

                string text = "Congratulations, Dear User Your Account Has Been Created Successfully. Your User ID: " + txtaadharno.Text + " and password:" + txtpassword.Text + ". Thank You. ";
                try
                {
                    string jsonValue = "";
                    string sURL;
                    StreamReader objReader;
                    // sURL = "http://203.129.225.69/API/WebSMS/Http/v1.0a/index.php?username=" + username + "&password=" + password + "&sender=" + senderId + "&to=" + txtmobileNo.Text + "&message=" + text + " &reqid=1&format={json|text}&route_id=" + routeId + "";
                    // sURL = "http://sms.probuztech.com/sms-panel/api/http/index.php?username=WYSE&apikey=FC144-3DD84&apirequest=Text&sender=PROBUZ&mobile=8055002299&message=TEST&route=TRANS&TemplateID=DLT-Template-ID&format=JSON";
                    sURL = "" + url + "?username=" + username + "&apikey=" + key + "&apirequest=" + request + "&sender=" + sender + "&mobile=" + txtmobileNo.Text + "&message=" + text + "&route=" + route + "&TemplateID=DLT-Template-ID&format=JSON";
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
                            SMSHistory(txtaadharno.Text, txtusername.Text, txtmobileNo.Text, text, "Not Send");
                        }
                        else
                        {
                            SMSdebit();
                            SMSHistory(txtaadharno.Text, txtusername.Text, txtmobileNo.Text, text, "Send");
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

    public class MyDetail
    {
        public string status
        {
            get;
            set;
        }
    }

    protected void txtmobileNo_TextChanged(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CN"].ConnectionString);
        con.Open();



        string query = "select Mobile,Email from MLM_Registration where Mobile='" + txtmobileNo.Text + "'";
        SqlCommand cmd = new SqlCommand(query, con);
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(dt);

        if (dt.Rows.Count >= 1)
        {
            lblmobile.ForeColor = System.Drawing.Color.Red;

            lblmobile.Visible = true;
            lblmobile.Text = "This Mobile Number Is Already Registered.";

            //int mobile = Convert.ToInt32(dt.Rows[0]["Mobile"]);
            //string email = dt.Rows[0]["Email"].ToString();
        }
        else
        {
            lblmobile.Visible = false;
        }
    }

    protected void txtEmailid_TextChanged(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CN"].ConnectionString);
        con.Open();



        string query = "select Mobile,Email from MLM_Registration where  Email='" + txtEmailid.Text + "'";
        SqlCommand cmd = new SqlCommand(query, con);
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(dt);

        if (dt.Rows.Count >= 1)
        {
            lblemail.ForeColor = System.Drawing.Color.Red;
            lblemail.Visible = true;
            lblemail.Text = "This Email Id Is Already Registered.";

            //int mobile = Convert.ToInt32(dt.Rows[0]["Mobile"]);
            //string email = dt.Rows[0]["Email"].ToString();
        }
        else
        {
            lblemail.Visible = false;
        }
    }


    public void PayTMpayment()
    {
        ViewState["CallBack"] = "http://localhost:56311/CallBack.aspx";
        //ViewState["CallBack"] = "http://dreamup.in.net/CallBack.aspx";

        string mKey = string.Empty;
        string MID = string.Empty;
        string CHANNEL_ID = string.Empty;
        string INDUSTRY_TYPE_ID = string.Empty;
        string WEBSITE = string.Empty;
        string orderid = Gettxid();

        //DataTable dtPaytm = dal.Gettable("select merchantKey,MID,CHANNEL_ID,INDUSTRY_TYPE_ID,WEBSITE from PaytmMaster where Status='Active'", ref message);
        //if (dtPaytm.Rows.Count > 0)
        //{
        //    mKey = dtPaytm.Rows[0]["merchantKey"].ToString();
        //    MID = dtPaytm.Rows[0]["MID"].ToString();
        //    CHANNEL_ID = dtPaytm.Rows[0]["CHANNEL_ID"].ToString();
        //    INDUSTRY_TYPE_ID = dtPaytm.Rows[0]["INDUSTRY_TYPE_ID"].ToString();
        //    WEBSITE = dtPaytm.Rows[0]["WEBSITE"].ToString();

        //// LIVE MODE

        //string merchantKey = "iW7JRMBrHI8D7Wto";
        //Dictionary<string, string> parameters = new Dictionary<string, string>();
        //parameters.Add("MID", "UbjGQo98720275523240");
        //parameters.Add("CHANNEL_ID", "WEB");
        //parameters.Add("INDUSTRY_TYPE_ID", "Retail");
        //parameters.Add("WEBSITE", "DEFAULT");

        //// LIVE MODE END

        //// TEST MODE

        string merchantKey = "eSKUEEBuXa15Q9WO";
        Dictionary<string, string> parameters = new Dictionary<string, string>();
        parameters.Add("MID", "SrcMaw86502710364017");
        parameters.Add("CHANNEL_ID", "WEB");
        parameters.Add("INDUSTRY_TYPE_ID", "Retail");
        parameters.Add("WEBSITE", "WEBSTAGING");

        //// TEST MODE END

        parameters.Add("EMAIL", "test@gmail.com");
        parameters.Add("MOBILE_NO", txtmobileNo.Text);
        parameters.Add("CUST_ID", txtaadharno.Text);
        parameters.Add("ORDER_ID", orderid);
        parameters.Add("TXN_AMOUNT", "500");
        //parameters.Add("TXN_AMOUNT", "1");
        parameters.Add("CALLBACK_URL", ViewState["CallBack"].ToString()); //This parameter is not mandatory. Use this to pass the callback url dynamically.

        string checksum = CheckSum.generateCheckSum(merchantKey, parameters);

        
        //string paytmURL = "https://pguat.paytm.com/oltp-web/processTransaction?orderid=" + orderid;

        string paytmURL = "https://securegw-stage.paytm.in/theia/processTransaction";   //TEST MODE
        //string paytmURL = "https://securegw.paytm.in/theia/processTransaction";    // LIVE MODE

        //////////////////////////////////// Send User Data To Callback Page /////////////////////////////////////
  
        HttpCookie userInfo = new HttpCookie("DUP_UserInfo");
        userInfo["UserID"] = txtaadharno.Text;
        userInfo["SponsorID"] = txtSponsorid.Text;
        userInfo["Name"] = txtusername.Text;
        userInfo["Email"] = txtEmailid.Text;
        userInfo["Mobile"] = txtmobileNo.Text;
        userInfo["Gender"] = ddlGender.SelectedItem.ToString();
        userInfo["Password"] = txtpassword.Text;
        userInfo["Position"] = drppoistion.SelectedValue.ToString();
        userInfo["TxnID"] = orderid;

        userInfo.Expires = DateTime.Now.AddMinutes(60);
        Response.Cookies.Add(userInfo);

        //////////////////////////////////// Send User Data To Callback Page /////////////////////////////////////

        string outputHTML = "<html>";
        outputHTML += "<head>";
        outputHTML += "<title>Merchant Check Out Page</title>";
        outputHTML += "</head>";
        outputHTML += "<body>";
        outputHTML += "<center><h1>Please do not refresh this page...</h1></center>";
        outputHTML += "<form method='post' action='" + paytmURL + "' name='f1'>";
        outputHTML += "<table border='1'>";
        outputHTML += "<tbody>";
        foreach (string key in parameters.Keys)
        {
            outputHTML += "<input type='hidden' name='" + key + "' value='" + parameters[key] + "'>";
        }
        outputHTML += "<input type='hidden' name='CHECKSUMHASH' value='" + checksum + "'>";
        outputHTML += "</tbody>";
        outputHTML += "</table>";
        outputHTML += "<script type='text/javascript'>";
        outputHTML += "document.f1.submit();";
        outputHTML += "</script>";
        outputHTML += "</form>";
        outputHTML += "</body>";
        outputHTML += "</html>";
        Response.Write(outputHTML);

        //}
        //else
        //{
        //    //Getway Under in process
        //}
    }

    public string Gettxid()
    {
        Random rnd = new Random();
        string strHash = Generatehash512(rnd.ToString() + DateTime.Now);
        return strHash.ToString().Substring(0, 20);

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

    protected void btnPaytm_Click(object sender, EventArgs e)
    {
        PayTMpayment();
    }
}
