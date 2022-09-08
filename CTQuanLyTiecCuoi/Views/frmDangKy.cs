using CTQuanLyTiecCuoi.Entities;
using CTQuanLyTiecCuoi.Entities.Responses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CTQuanLyTiecCuoi.Views
{
    public partial class frmDangKy : Form
    {
        readonly PreAccount preAccount;

        public frmDangKy()
        {
            InitializeComponent();
            preAccount = new PreAccount();
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            preAccount.Username = txtUsername.Text;
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            preAccount.Password = txtPassword.Text;
        }

        private void txtConfirmPassword_TextChanged(object sender, EventArgs e)
        {
            preAccount.ConfirmPassword = txtConfirmPassword.Text;
        }

        private void txtConfirmPassword_Leave(object sender, EventArgs e)
        {
            if (preAccount.Password != preAccount.ConfirmPassword)
            {
                errorProvider.SetError(sender as Control, "Password does not match");
                return;
            }
            else errorProvider.Clear();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            UserResponse res = Program.userRepository.Register(preAccount);

            if (!res.isSuccess)
            {
                MessageBox.Show(res.message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Program.session = Program.userRepository.Session;

            var frm = new frmMain();
            Hide();
            frm.ShowDialog();
            Close();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var frm = new frmDangNhap();
            Hide();
            frm.ShowDialog();
            Close();
        }
    }

    class PreAccount : Account
    {
        public string ConfirmPassword;
    }
}
