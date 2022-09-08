using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using CTQuanLyTiecCuoi.Entities;

namespace CTQuanLyTiecCuoi.Views
{
    public partial class frmCapNhatDichVu : Form
    {
        List<Hall> halls;
        List<Service> services;
        bool isUpdating;

        public frmCapNhatDichVu()
        {
            InitializeComponent();
            halls = new List<Hall>();
            services = new List<Service>();
        }

        private void frmCapNhatDichVu_Load(object sender, EventArgs e)
        {
            GetServiceData();
            PopulateDataOfServices(services);            
            PopulateCheckedListBox();

            txtName.Enabled = false;
            txtPrice.Enabled = false;
            checkedListBoxHalls.Enabled = false;
            btnSave.Enabled = false;
        }

        private void ClearTextBoxes()
        {
            txtPrice.Text = "";
            txtName.Text = "";
        }

        private void EnableInput()
        {
            txtName.Enabled = true;
            txtPrice.Enabled = true;
            checkedListBoxHalls.Enabled = true;
            btnSave.Enabled = true;
        }

        private void DisableInput()
        {
            txtName.Enabled = false;
            txtPrice.Enabled = false;
            checkedListBoxHalls.Enabled = false;
            btnSave.Enabled = false;
        }

        private void PopulateCheckedListBox()
        {
            var res = Program.hallRepository.GetHalls();
            if (!res.isSuccess) MessageBox.Show(res.message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            halls = res.Halls;

            checkedListBoxHalls.Items.Clear();
            halls.ForEach(h => checkedListBoxHalls.Items.Add(h.Name, false));
        }

        private void CheckListBoxItems(List<Hall> serviceHalls)
        {
            for (int count = 0; count < checkedListBoxHalls.Items.Count; count++)
            {
                Hall tmp = halls.ElementAt(count);                
                if (serviceHalls.Exists(h => h.Id == tmp.Id))
                {
                    checkedListBoxHalls.SetItemChecked(count, true);
                }
                else
                {
                    checkedListBoxHalls.SetItemChecked(count, false);
                }
            }
        }

        private void GetServiceData()
        {
            var res = Program.serviceRepository.GetServices();
            if (!res.isSuccess) MessageBox.Show(res.message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                services = res.Services;
            }
        }

        private void PopulateDataOfServices(List<Service> services)
        {

            var columns = from s in services
                          select new
                          {
                              s.Id,
                              s.Name,
                              s.UnitPrice
                          };

            dataGridViewServices.Columns.Clear();
            dataGridViewServices.DataSource = columns.ToList();
            dataGridViewServices.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        

        private void frmCapNhatDichVu_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn muốn thoát?",
                "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                e.Cancel = false;
            }
            else if (result == DialogResult.No)
                e.Cancel = true;
        }

        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ClearTextBoxes();
            isUpdating = false;
            EnableInput();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            isUpdating = true;
            EnableInput();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewServices.SelectedRows.Count == 1)
            {
                DialogResult result = MessageBox.Show("Bạn muốn xóa?",
                    "Xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes)
                    return;

                string id = dataGridViewServices.SelectedRows[0].Cells[0].Value.ToString();
                var res = Program.serviceRepository.DeleteService(id);
                if (!res.isSuccess)
                {
                    MessageBox.Show(res.message, "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                ClearTextBoxes();
                GetServiceData();
                PopulateDataOfServices(services);

            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dich vụ!");
            }
        }

        private void dataGridViewServices_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int selectedIndex = dataGridViewServices.CurrentCell.RowIndex;
            Service currentService = services.ElementAt(selectedIndex);
            txtName.Text = currentService.Name;
            txtPrice.Text = currentService.UnitPrice.ToString();
            CheckListBoxItems(currentService.Halls);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "" || txtPrice.Text == "" || checkedListBoxHalls.CheckedItems.Count == 0)
            {
                MessageBox.Show("Thông tin trống!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Service newService = new Service
            {
                Name = txtName.Text,
                UnitPrice = Convert.ToDecimal(txtPrice.Text),
                Halls = new List<Hall>()
            };

            for (int i = 0; i < checkedListBoxHalls.Items.Count; i++)
            {
                if (checkedListBoxHalls.GetItemChecked(i))
                {
                    newService.Halls.Add(halls.ElementAt(i));
                }
            }

            if (isUpdating)
            {
                if (dataGridViewServices.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn một dich vụ!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                newService.Id = dataGridViewServices.SelectedRows[0].Cells[0].Value.ToString();
                var res = Program.serviceRepository.UpdateService(newService);
                if (!res.isSuccess)
                {
                    MessageBox.Show(res.message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                var res = Program.serviceRepository.InsertService(newService);
                if (!res.isSuccess)
                {
                    MessageBox.Show(res.message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }


            GetServiceData();
            PopulateDataOfServices(services);
            DisableInput();
        }

        private void checkedListBoxHalls_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }
}
