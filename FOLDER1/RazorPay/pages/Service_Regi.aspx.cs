using DataAccessLayer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Service_Service_Regi : System.Web.UI.Page
{
    DAL dal = new DAL();
    string message = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtSponsorID.Text = "CaringClone";
            txtSponsorName.Text = "Caring Clone";
            //txtpassword.Text = "12345";
            //txtcnfpassword.Text = "12345";
            WorkTimeBind();
            ServicesBind();
            HelperID();
            Category();
        }

    }
    public void ServicesBind()
    {
        try
        {
            DataTable dt = dal.Gettable("Select ServiceName,ServiceId from ServiceMaster where Status='Active'", ref message);
            if (dt.Rows.Count > 0)
            {
                ddlServices.DataSource = dt;
                ddlServices.DataBind();
                ddlServices.DataTextField = "ServiceName";
                ddlServices.DataValueField = "ServiceId";
                ddlServices.SelectedIndex = 1;

                ddlServices.DataBind();
                ddlServices.Items.Insert(0, "Select Service");

                lstService.DataSource = dt;
                lstService.DataBind();
                lstService.DataTextField = "ServiceName";
                lstService.DataValueField = "ServiceId";
                lstService.DataBind();
            }
        }
        catch (Exception ex)
        {

        }
    }
    public void WorkTimeBind()
    {
        DataTable dt = dal.Gettable("select TimeId,TimingName from TimingMaster where Status='Active'", ref message);
        if (dt.Rows.Count > 0)
        {
            ddlWorkTime.DataSource = dt;
            ddlWorkTime.DataBind();
            ddlWorkTime.DataTextField = "TimingName";
            ddlWorkTime.DataValueField = "TimeId";
            ddlWorkTime.DataBind();
            ddlWorkTime.Items.Insert(0, "---- Select Work Time ----");
        }
    }
    public void HelperID()
    {
        string connstring = ConfigurationManager.ConnectionStrings["CN"].ConnectionString;
        SqlConnection con = new SqlConnection(connstring);
        con.Open();
        int flag = 1;
        while (flag == 1)
        {
            string Package_ID = "SP" + GetUniqueKey(6);
            SqlCommand cmd = new SqlCommand("Unique_ID", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", Package_ID);
            cmd.Parameters.AddWithValue("@Mode", "CHECK_HelperID");
            flag = (int)cmd.ExecuteScalar();
            ViewState["Id"] = Package_ID;
            txtHelperID.Text = Package_ID;
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
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            //InsertIntoServiceKycDetails();    // After Comment for Chaning 10june2021
            SaveData();
        }
    }
    protected void SaveData()
    {
        try
        {
            if (!string.IsNullOrEmpty(txtSponsorID.Text) && !string.IsNullOrEmpty(txtSponsorName.Text) && !string.IsNullOrEmpty(txtname.Text) && !string.IsNullOrEmpty(txtmobile.Text) && !string.IsNullOrEmpty(txtEmail.Text)  && ddlWorkTime.SelectedIndex != 0)
            {
                string connstring = ConfigurationManager.ConnectionStrings["CN"].ConnectionString;
                SqlConnection con = new SqlConnection(connstring);
                con.Open();
                SqlCommand cmd = new SqlCommand("Auth_ServiceProviderRegister", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HelperID", ViewState["Id"].ToString());
                cmd.Parameters.AddWithValue("@SponsorID", txtSponsorID.Text);
                cmd.Parameters.AddWithValue("@Name", txtname.Text);
                cmd.Parameters.AddWithValue("@Password", "12345");
                cmd.Parameters.AddWithValue("@Mobile", txtmobile.Text);
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@ServiceID", ddlServices.SelectedValue);
                cmd.Parameters.AddWithValue("@WorkTimeID", ddlWorkTime.SelectedValue);
                cmd.Parameters.AddWithValue("@Mode", "Service_Reg");
                int flag = cmd.ExecuteNonQuery();
                //int flag = 1;
                if (flag > 0)
                {
                    //InsertIntoServiceKycDetails();
                    //ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Congratulation,Dear "+ txtname.Text + " Registration has been created successfully... ');window.location='Service_Regi.aspx';", true);

                    con.Close();
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('You Have Been Successfully Registered');window.location='Service_Regi.aspx';", true);
                    //string RegistrationFee = string.Empty;
                    //DataTable dtFee = dal.Gettable("Select ServiceProviderFee from RegistrationFee_Master Where Status='Active'", ref message);
                    //if (dtFee.Rows.Count > 0)
                    //{
                    //    RegistrationFee = dtFee.Rows[0]["ServiceProviderFee"].ToString();
                    //}
                    //else
                    //{
                    //    RegistrationFee = "0";
                    //}

                    //HttpCookie userInfo = new HttpCookie("UserData1");
                    //userInfo["HelperID"] = ViewState["Id"].ToString();
                    //userInfo["SponsorID"] = txtSponsorID.Text;
                    //userInfo["Name"] = txtname.Text;
                    //userInfo["Password"] = "12345";
                    //userInfo["Mobile"] = txtmobile.Text;
                    //userInfo["Email"] = txtEmail.Text;
                    //userInfo["ServiceID"] = ddlServices.SelectedValue;
                    //userInfo["WorkTimeID"] = ddlWorkTime.SelectedValue;
                    //userInfo["Total1"] = Convert.ToDecimal(RegistrationFee).ToString();
                    //userInfo.Expires = DateTime.Now.AddMinutes(60);
                    //Response.Cookies.Add(userInfo);
                    //Response.Redirect("OnlineRegistrationPayment.aspx");


                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Fill All Details');", true);
            }

        }
        catch (Exception ex)
        {

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
                string text = "Congratulations, Dear User Your Account Has Been Created Successfully. Your User ID: " + txtmobile.Text + ". Thank You. ";
                try
                {
                    string jsonValue = "";
                    string sURL;
                    StreamReader objReader;
                    // sURL = "http://203.129.225.69/API/WebSMS/Http/v1.0a/index.php?username=" + username + "&password=" + password + "&sender=" + senderId + "&to=" + txtmobileNo.Text + "&message=" + text + " &reqid=1&format={json|text}&route_id=" + routeId + "";
                    // sURL = "http://sms.probuztech.com/sms-panel/api/http/index.php?username=WYSE&apikey=FC144-3DD84&apirequest=Text&sender=PROBUZ&mobile=8055002299&message=TEST&route=TRANS&format=JSON";
                    //sURL = "" + url + "?username=" + username + "&apikey=" + key + "&apirequest=" + request + "&sender=" + sender + "&mobile=" + txtmob.Text + "&message=" + text + "&route=" + route + "&format=JSON";
                    sURL = "" + url + "?username=" + username + "&apikey=" + key + "&apirequest=" + request + "&sender=" + sender + "&mobile=" + txtmobile.Text + "&message=" + text + "&route=" + route + "&TemplateID=1707161744326123203&format=JSON";
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
                            SMSHistory(txtmobile.Text, txtname.Text, txtmobile.Text, text, "Not Send");
                        }
                        else
                        {
                            SMSdebit();
                            SMSHistory(txtmobile.Text, txtname.Text, txtmobile.Text, text, "Send");
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
    public void ServiceKycID()
    {
        string connstring = ConfigurationManager.ConnectionStrings["CN"].ConnectionString;
        SqlConnection con = new SqlConnection(connstring);
        con.Open();
        int flag = 1;
        while (flag == 1)
        {
            string Package_ID = "SK" + GetUniqueKey(6);
            SqlCommand cmd = new SqlCommand("Unique_ID", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", Package_ID);
            cmd.Parameters.AddWithValue("@Mode", "CHECK_HelperID");
            flag = (int)cmd.ExecuteScalar();
            ViewState["SK_ID"] = Package_ID;
        }
        con.Close();
    }
    protected void InsertIntoServiceKycDetails()
    {
        if (!string.IsNullOrEmpty(txtname.Text) && !string.IsNullOrEmpty(txtmobile.Text) && !string.IsNullOrEmpty(txtEmail.Text) && ddlServices.SelectedIndex != 0 && ddlWorkTime.SelectedIndex != 0)
        {
            //try
            //{
            //    string message = "";
            //    int count = 0;
            //    foreach (ListItem item in lstService.Items)
            //    {
            //        if (item.Selected)
            //        {
            //            string SK_ID = "";
            //            count++;
            //            ServiceKycID();
            //            SK_ID = ViewState["SK_ID"].ToString();
            //            message += item.Text + " " + item.Value + "\\n";
            //            string connstring = ConfigurationManager.ConnectionStrings["CN"].ConnectionString;
            //            SqlConnection con = new SqlConnection(connstring);
            //            con.Open();
            //            SqlCommand cmd = new SqlCommand("Auth_ServiceKycDeatils", con);
            //            cmd.CommandType = CommandType.StoredProcedure;
            //            cmd.Parameters.AddWithValue("@ServiceKycID", SK_ID);
            //            cmd.Parameters.AddWithValue("@ServiceProviderID", txtHelperID.Text);
            //            cmd.Parameters.AddWithValue("@ServiceID", item.Value);
            //            cmd.Parameters.AddWithValue("@Mode", "ServiceKyc_Insert");
            //            int flag = cmd.ExecuteNonQuery();
            //            {

            //            }
            //        }

            //    }

            //    if (count > 0)
            //    {
            //        SaveData();
            //    }
            //    else
            //    {
            //        message = "Please Select Service";
            //        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + message + "');", true);
            //    }
            //    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + message + "');", true);
            //}
            //catch (Exception ex)
            //{

            //}


            try
            {
                string s = "";
                string sid = string.Empty;
                int count = 0;
                foreach (RepeaterItem item in rptCategory.Items)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        Repeater rptSubCategory = (Repeater)item.FindControl("rptSubCategory");
                        foreach (RepeaterItem dli in rptSubCategory.Items)
                        {
                            if (dli.ItemType == ListItemType.Item || dli.ItemType == ListItemType.AlternatingItem)
                            {
                                var checkBox = (CheckBox)dli.FindControl("cbInterest");
                                if (checkBox.Checked)
                                {
                                    //s += (s == "") ? checkBox.Text : "^" + checkBox.Text;
                                    s = checkBox.Text;
                                    DataTable dtadd = dal.Gettable("select ServiceId,ServiceName from ServiceMaster where ServiceName='" + s + "'", ref message);
                                    if (dtadd.Rows.Count > 0)
                                    {
                                        count++;
                                        sid = dtadd.Rows[0]["ServiceId"].ToString();

                                        string SK_ID = "";
                                        count++;
                                        ServiceKycID();
                                        SK_ID = ViewState["SK_ID"].ToString();
                                        string connstring = ConfigurationManager.ConnectionStrings["CN"].ConnectionString;
                                        SqlConnection con = new SqlConnection(connstring);
                                        con.Open();
                                        SqlCommand cmd = new SqlCommand("Auth_ServiceKycDeatils", con);
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.AddWithValue("@ServiceKycID", SK_ID);
                                        cmd.Parameters.AddWithValue("@ServiceProviderID", txtHelperID.Text);
                                        cmd.Parameters.AddWithValue("@ServiceID", sid);
                                        cmd.Parameters.AddWithValue("@Mode", "ServiceKyc_Insert");
                                        int flag = cmd.ExecuteNonQuery();
                                        {

                                        }


                                    }
                                }

                            }

                        }


                    }
                }

                if (count > 0)
                {
                    SaveData();
                    //message = count + " Service Selected ";
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + message + "');", true);
                }

                else
                {
                    message = "Please Select Service";
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + message + "');", true);
                }
            }
            catch
            {

            }

        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Fill All Details');", true);
        }

    }
    protected void txtmobile_TextChanged(object sender, EventArgs e)
    {
        try
        {

            DataTable dt = dal.Gettable("select Mobile from ServiceProviderRegistration where Mobile = '" + txtmobile.Text + "'", ref message);
            if (dt.Rows.Count > 0)
            {
                txtmobile.Text = "";
                lblEMsgMobile.Text = "Mobile No Already Exists";
                lblEMsgMobile.Visible = true;
            }
            else
            {
                lblEMsgMobile.Visible = false;
            }
        }
        catch (Exception ex)
        {

        }

    }
    protected void txtSponsorID_TextChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = dal.Gettable("select Name,Mobile from ServiceProviderRegistration where Mobile='" + txtSponsorID.Text + "'", ref message);
            if (dt.Rows.Count > 0)
            {
                txtSponsorName.Text = dt.Rows[0]["Name"].ToString();
                lblNotAvailable.Visible = false;
            }
            else
            {

                lblNotAvailable.Visible = true;
                lblNotAvailable.Text = txtSponsorID.Text + " : ID Not Be Exists or Invalid";
                txtSponsorID.Text = "";
            }
        }
        catch (Exception ex)
        {

        }
    }
    protected void Category()
    {
        try
        {
            DataTable dt = dal.Gettable("select Service_CategoryID,Service_CategoryName from Service_CategoryMaster where Status='Active' ", ref message);
            if (dt.Rows.Count > 0)
            {
                rptCategory.DataSource = dt;
                rptCategory.DataBind();
            }
            else
            {
                rptCategory.DataSource = dt;
                rptCategory.DataBind();
            }

        }
        catch (Exception ex)
        {

        }
    }
    protected void rptCategory_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            string lblCID = (e.Item.FindControl("lblCID") as Label).Text;
            Repeater rptSubCategory = e.Item.FindControl("rptSubCategory") as Repeater;
            DataTable dts = dal.Gettable("select ServiceId,ServiceName from ServiceMaster where Service_CategoryID='" + lblCID + "'", ref message);
            if (dts.Rows.Count > 0)
            {
                rptSubCategory.DataSource = dts;
                rptSubCategory.DataBind();
            }
            else
            {
                rptSubCategory.DataSource = dts;
                rptSubCategory.DataBind();
            }

        }
    }
    protected void btnSelected_Click(object sender, EventArgs e)
    {
        
    }
}