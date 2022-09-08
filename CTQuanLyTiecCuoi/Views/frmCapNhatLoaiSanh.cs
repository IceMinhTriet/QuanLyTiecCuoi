using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CTQuanLyTiecCuoi.Entities;

namespace CTQuanLyTiecCuoi.Views
{
    public partial class frmCapNhatLoaiSanh : Form
    {
        List<HallType> hallTypes;
        bool isUpdating;

        public frmCapNhatLoaiSanh()
        {
            InitializeComponent();
            hallTypes = new List<HallType>();
        }

        private void GetHallTypeData()
        {
            var res = Program.hallRepository.GetHallTypes();
            if (!res.isSuccess) MessageBox.Show(res.message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                hallTypes = res.HallTypes;
            }
        }

        private void PopulateDataOfHallTypes(List<HallType> types)
        {
            var columns = from t in types
                          select new
                          {
                              t.Id,
                              t.TypeName,
                              t.UnitPrice
                          };

            dataGridViewHallTypes.Columns.Clear();
            dataGridViewHallTypes.DataSource = columns.ToList();
            dataGridViewHallTypes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void frmCapNhatLoaiSanh_Load(object sender, EventArgs e)
        {
            GetHallTypeData();
            PopulateDataOfHallTypes(hallTypes);

            txtName.Enabled = false;
            txtPrice.Enabled = false;
            btnSave.Enabled = false;
        }

        private void ClearTextBoxes()
        {
            txtName.Text = "";
            txtPrice.Text = "";
        }

        private void DisableInput()
        {
            txtName.Enabled = false;
            txtPrice.Enabled = false;
            btnSave.Enabled = false;
        }

        private void EnableInput()
        {
            txtName.Enabled = true;
            txtPrice.Enabled = true;
            btnSave.Enabled = true;
        }

        private void frmCapNhatLoaiSanh_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn muốn thoát?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                e.Cancel = false;
            }
            else if (result == DialogResult.No)
                e.Cancel = true;
        }

        private void dataGridViewHallTypes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtName.Text = dataGridViewHallTypes.SelectedRows[0].Cells[1].Value.ToString();
            txtPrice.Text = dataGridViewHallTypes.SelectedRows[0].Cells[2].Value.ToString();
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
            if (dataGridViewHallTypes.SelectedRows.Count == 1)
            {
                DialogResult result = MessageBox.Show("Bạn muốn xóa?", "Xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes)
                    return;

                string id = dataGridViewHallTypes.SelectedRows[0].Cells[0].Value.ToString();
                var res = Program.hallRepository.DeleteHallType(id);
                if (!res.isSuccess)
                {
                    MessageBox.Show(res.message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                ClearTextBoxes();
                GetHallTypeData();
                PopulateDataOfHallTypes(hallTypes);

            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dich vụ!");
            }
        }

        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "" || txtPrice.Text == "")
            {
                MessageBox.Show("Thông tin trống!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            HallType newType = new HallType
            {
                TypeName = txtName.Text,
                UnitPrice = Convert.ToDecimal(txtPrice.Text),
            };

            if (isUpdating)
            {
                if (dataGridViewHallTypes.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn một dich vụ!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                newType.Id = dataGridViewHallTypes.SelectedRows[0].Cells[0].Value.ToString();
                var res = Program.hallRepository.UpdateHallType(newType);
                if (!res.isSuccess)
                {
                    MessageBox.Show(res.message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                var res = Program.hallRepository.InsertHallType(newType);
                if (!res.isSuccess)
                {
                    MessageBox.Show(res.message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            GetHallTypeData();
            PopulateDataOfHallTypes(hallTypes);
            DisableInput();
        }
    }
}
