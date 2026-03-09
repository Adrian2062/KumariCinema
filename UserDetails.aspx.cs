using System;
using System.Data;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Web.UI.WebControls;

namespace KumariCinemas
{
    public partial class UserDetails : System.Web.UI.Page
    {
        private readonly string connStr = ConfigurationManager.ConnectionStrings["OracleDBConn"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        private void BindGrid()
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                OracleDataAdapter da = new OracleDataAdapter("SELECT user_id, user_name, address FROM USERS ORDER BY user_id", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvUsers.DataSource = dt;
                gvUsers.DataBind();
            }
        }

        protected void BtnInsert_Click(object sender, EventArgs e)
        {
            // 1. Validate that the User ID is actually a number
            if (!int.TryParse(txtUserId.Text.Trim(), out int userId))
            {
                lblMessage.Text = "Invalid User ID. Please enter a valid number.";
                lblMessage.CssClass = "text-danger ms-3 fw-bold";
                return;
            }

            using (OracleConnection conn = new OracleConnection(connStr))
            {
                OracleCommand cmd = new OracleCommand("INSERT INTO USERS (user_id, user_name, address) VALUES (:id, :name, :address)", conn);
                cmd.BindByName = true; // Best practice for Oracle parameters

                cmd.Parameters.Add("id", OracleDbType.Int32).Value = userId;
                cmd.Parameters.Add("name", OracleDbType.Varchar2).Value = txtUserName.Text.Trim();
                cmd.Parameters.Add("address", OracleDbType.Varchar2).Value = txtAddress.Text.Trim();

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    lblMessage.Text = "User Added successfully!";
                    lblMessage.CssClass = "text-success ms-3 fw-bold"; // Green success text

                    // Clear textboxes after insert
                    txtUserId.Text = "";
                    txtUserName.Text = "";
                    txtAddress.Text = "";

                    BindGrid();
                }
                catch (OracleException ex)
                {
                    // Error Number 1 is ORA-00001 (Unique Constraint Violated)
                    if (ex.Number == 1)
                    {
                        lblMessage.Text = "Error: A user with this ID already exists!";
                    }
                    else
                    {
                        lblMessage.Text = "Database Error: " + ex.Message;
                    }
                    lblMessage.CssClass = "text-danger ms-3 fw-bold"; // Red error text
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Application Error: " + ex.Message;
                    lblMessage.CssClass = "text-danger ms-3 fw-bold";
                }
            }
        }

        protected void GvUsers_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvUsers.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void GvUsers_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvUsers.EditIndex = -1;
            BindGrid();
        }

        protected void GvUsers_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                OracleCommand cmd = new OracleCommand("UPDATE USERS SET user_name = :name, address = :addr WHERE user_id = :id", conn);
                cmd.BindByName = true;

                cmd.Parameters.Add("name", OracleDbType.Varchar2).Value = (gvUsers.Rows[e.RowIndex].Cells[1].Controls[0] as TextBox).Text.Trim();
                cmd.Parameters.Add("addr", OracleDbType.Varchar2).Value = (gvUsers.Rows[e.RowIndex].Cells[2].Controls[0] as TextBox).Text.Trim();
                cmd.Parameters.Add("id", OracleDbType.Int32).Value = Convert.ToInt32(gvUsers.DataKeys[e.RowIndex].Value);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    gvUsers.EditIndex = -1;
                    lblMessage.Text = "User Updated successfully!";
                    lblMessage.CssClass = "text-success ms-3 fw-bold";

                    BindGrid();
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Update Error: " + ex.Message;
                    lblMessage.CssClass = "text-danger ms-3 fw-bold";
                }
            }
        }

        protected void GvUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                OracleCommand cmd = new OracleCommand("DELETE FROM USERS WHERE user_id = :id", conn);
                cmd.BindByName = true;
                cmd.Parameters.Add("id", OracleDbType.Int32).Value = Convert.ToInt32(gvUsers.DataKeys[e.RowIndex].Value);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    lblMessage.Text = "User Deleted successfully!";
                    lblMessage.CssClass = "text-success ms-3 fw-bold";

                    BindGrid();
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Delete Error: " + ex.Message;
                    lblMessage.CssClass = "text-danger ms-3 fw-bold";
                }
            }
        }
    }
}