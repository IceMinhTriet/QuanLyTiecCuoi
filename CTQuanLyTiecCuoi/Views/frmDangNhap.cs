using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CTQuanLyTiecCuoi.Entities;
using CTQuanLyTiecCuoi.Entities.Responses;

namespace CTQuanLyTiecCuoi.Views
{
    public partial class frmDangNhap : Form
    {
        private readonly Account account;

        public frmDangNhap()
        {
            InitializeComponent();
            account = new Account();
        }        

        private void btnLogin_Click(object sender, EventArgs e)
        {
            HandleLogin();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            account.Username = txtUsername.Text;
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            account.Password = txtPassword.Text;
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                HandleLogin();
            }
        }

        private void HandleLogin()
        {
            UserResponse res = Program.userRepository.Login(account.Username, account.Password);
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

        private void btnRegister_Click(object sender, EventArgs e)
        {
            var frm = new frmDangKy();
            Hide();
            frm.ShowDialog();
            Close();
        }
    }
}
