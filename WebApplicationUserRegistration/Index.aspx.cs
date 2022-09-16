using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

// Membuat user registrasi dengan metode stored procedure
namespace WebApplicationUserRegistration
{
    public partial class Index : System.Web.UI.Page
    {
        // Koneksi database
        string connectionString = @"Data Source=martin-pc\martin880;Initial Catalog=UserRegistrationDB;Integrated Security=True";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Clear();
                if (!String.IsNullOrEmpty(Request.QueryString["Id"]))
                {
                    int userID = Convert.ToInt32(Request.QueryString["Id"]);
                    using (SqlConnection sqlCon = new SqlConnection(connectionString))
                    {
                        sqlCon.Open();
                        SqlDataAdapter sqlDa = new SqlDataAdapter("UserViewById", sqlCon);
                        sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                        sqlDa.SelectCommand.Parameters.AddWithValue("@UserId", userID);
                        DataTable dtbl = new DataTable();
                        sqlDa.Fill(dtbl);

                        hfUserId.Value = userID.ToString();
                        txtFirstName.Text = dtbl.Rows[0][1].ToString();
                        txtLastName.Text = dtbl.Rows[0][2].ToString();
                        txtContact.Text = dtbl.Rows[0][3].ToString();
                        ddlGender.Items.FindByValue(dtbl.Rows[0][4].ToString()).Selected = true;
                        txtAddress.Text = dtbl.Rows[0][5].ToString();
                        txtUserName.Text = dtbl.Rows[0][6].ToString();
                        txtPassword.Text = dtbl.Rows[0][7].ToString();
                        txtPassword.Attributes.Add("value", dtbl.Rows[0][7].ToString());
                        txtConfirmPassword.Text = dtbl.Rows[0][7].ToString();
                        txtConfirmPassword.Attributes.Add("value", dtbl.Rows[0][7].ToString());
                    }
                }
            }
        }

            // stored procedure
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            // Validasi
            if (txtUserName.Text == "" || txtPassword.Text == "")
                lblErrorMessage.Text = "Username dan Password tidak boleh kosong";
            else if (txtPassword.Text != txtConfirmPassword.Text)
                lblErrorMessage.Text = "Password tidak cocok";
            else 
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    SqlCommand sqlCmd = new SqlCommand("UserAddorEdit", sqlCon);
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("@UserId", Convert.ToInt32(hfUserId.Value == "" ? "0" : hfUserId.Value));
                    sqlCmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@LastName", txtLastName.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Contact", txtContact.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Gender", ddlGender.SelectedValue);
                    sqlCmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@UserName", txtUserName.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Password", txtPassword.Text.Trim());
                    sqlCmd.ExecuteNonQuery();
                    Clear();
                    lblSuccessMessage.Text = "Submitted Successfully";
                }
            }
        }

        void Clear() 
        {
            txtFirstName.Text = txtLastName.Text = txtContact.Text = txtAddress.Text = txtUserName.Text = txtPassword.Text = txtConfirmPassword.Text = "";
            lblSuccessMessage.Text = lblErrorMessage.Text = "";
        }
    }
}