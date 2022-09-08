using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CTQuanLyTiecCuoi.Entities;

namespace CTQuanLyTiecCuoi.Views
{
    public partial class frmCapNhatMonAn : Form
    {
        List<Dish> dishes;
        bool isUpdating;
        readonly List<string> types = new List<string>() { "Khai vi", "Mon chinh", "Trang mieng" };

        public frmCapNhatMonAn()
        {
            InitializeComponent();
            dishes = new List<Dish>();
        }

        private void GetDishData()
        {
            var res = Program.dishRepository.GetDishes();
            if (!res.isSuccess) MessageBox.Show(res.message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                dishes = res.Dishes;
            }
        }

        private void PopulateDataOfDishes(List<Dish> dishes)
        {
            var columns = from d in dishes
                          select new
                          {
                              d.Id,
                              d.Name,
                              d.UnitPrice,
                              d.Type,
                              d.Note
                          };

            dataGridViewDishes.Columns.Clear();
            dataGridViewDishes.DataSource = columns.ToList();
            dataGridViewDishes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void frmCapNhatMonAn_Load(object sender, EventArgs e)
        {
            GetDishData();
            PopulateDataOfDishes(dishes);

            cbbType.Items.Clear();
            cbbType.DataSource = types;

            DisableInput();
        }

        private void DisableInput()
        {
            txtName.Enabled = false;
            txtNote.Enabled = false;
            txtPrice.Enabled = false;
            cbbType.Enabled = false;
            btnSave.Enabled = false;
        }

        private void EnableInput()
        {
            txtName.Enabled = true;
            txtNote.Enabled = true;
            txtPrice.Enabled = true;
            cbbType.Enabled = true;
            btnSave.Enabled = true;
        }

        private void ClearTextBoxes()
        {
            txtPrice.Text = "";
            txtNote.Text = "";
            txtName.Text = "";
        }
        
        private void txtDonGia_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void frmCapNhatMonAn_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn muốn thoát?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                e.Cancel = false;
            }
            else if (result == DialogResult.No)
                e.Cancel = true;
        }

        private void dataGridViewDishes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtName.Text = dataGridViewDishes.SelectedRows[0].Cells[1].Value.ToString();
            txtPrice.Text = dataGridViewDishes.SelectedRows[0].Cells[2].Value.ToString();
            cbbType.Text = dataGridViewDishes.SelectedRows[0].Cells[3].Value.ToString();
            txtNote.Text = dataGridViewDishes.SelectedRows[0].Cells[4].Value.ToString();
            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "" || txtPrice.Text == "")
            {
                MessageBox.Show("Thông tin trống!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Dish newDish = new Dish
            {
                Name = txtName.Text,
                UnitPrice = Convert.ToDecimal(txtPrice.Text),
                Type = types.ElementAt(cbbType.SelectedIndex),
                Note = txtNote.Text
            };

            if (isUpdating)
            {
                if (dataGridViewDishes.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn một dich vụ!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                newDish.Id = dataGridViewDishes.SelectedRows[0].Cells[0].Value.ToString();
                var res = Program.dishRepository.UpdateDish(newDish);
                if (!res.isSuccess)
                {
                    MessageBox.Show(res.message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                var res = Program.dishRepository.InsertDish(newDish);
                if (!res.isSuccess)
                {
                    MessageBox.Show(res.message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            GetDishData();
            PopulateDataOfDishes(dishes);
            DisableInput();
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
            if (dataGridViewDishes.SelectedRows.Count == 1)
            {
                DialogResult result = MessageBox.Show("Bạn muốn xóa?",
                    "Xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes)
                    return;

                string id = dataGridViewDishes.SelectedRows[0].Cells[0].Value.ToString();
                var res = Program.dishRepository.DeleteDish(id);
                if (!res.isSuccess)
                {
                    MessageBox.Show(res.message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                ClearTextBoxes();
                GetDishData();
                PopulateDataOfDishes(dishes);

            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dich vụ!");
            }
        }
    }
}
