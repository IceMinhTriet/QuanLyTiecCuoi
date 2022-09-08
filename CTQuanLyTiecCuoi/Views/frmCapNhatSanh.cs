using CTQuanLyTiecCuoi.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace CTQuanLyTiecCuoi.Views
{
    public partial class frmCapNhatSanh : Form
    {
        private List<HallType> hallTypes;
        private List<Hall> halls;

        // private List<Hall> halls;
        Hall _hall;

        public frmCapNhatSanh()
        {
            InitializeComponent();
            hallTypes = new List<HallType>();
            halls = new List<Hall>();
            _hall = new Hall();
        }

        private void GetHallData()
        {
            var res = Program.hallRepository.GetHalls();
            if (!res.isSuccess) MessageBox.Show(res.message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                halls = res.Halls;
            }
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

        private void FillDataOfHalls(List<Hall> halls)
        {
            var columns = from h in halls
                          select new
                          {
                              h.Id,
                              h.Name,
                              HallType = h.Type.TypeName,
                              h.Capacity,
                              h.Address,
                              h.Note
                          };
            dataGridViewHalls.DataSource = columns.ToList();
        }

        private void populateHallTypes(List<HallType> hallTypes)
        {
            List<string> types = new List<string>();
            hallTypes.ForEach(e => types.Add(e.TypeName));
            cbbHallTypes.Items.Clear();
            cbbHallTypes.DataSource = types;
        }
        
        private void frmCapNhatSanh_Load(object sender, EventArgs e)
        {
            GetHallData();
            FillDataOfHalls(halls);

            GetHallTypeData();
            populateHallTypes(hallTypes);
        }

        private void frmCapNhatSanh_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn muốn thoát?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                e.Cancel = false;
            }
            else if (result == DialogResult.No)
                e.Cancel = true;
        }

        private void dataGridViewHalls_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // int selectedIndex = dataGridViewHalls.CurrentCell.RowIndex;
            txtHallname.Text = dataGridViewHalls.SelectedRows[0].Cells[1].Value.ToString();
            numericUpDownCapacity.Value = Convert.ToInt32(dataGridViewHalls.SelectedRows[0].Cells[3].Value);
            txtAddress.Text = dataGridViewHalls.SelectedRows[0].Cells[4].Value.ToString();
            richTxtNote.Text = dataGridViewHalls.SelectedRows[0].Cells[5].Value.ToString();
            _hall.Id = dataGridViewHalls.SelectedRows[0].Cells[0].Value.ToString();
            cbbHallTypes.SelectedIndex = hallTypes.FindIndex(h => h.TypeName.Equals(dataGridViewHalls.SelectedRows[0].Cells[2].Value.ToString()));
                //dataGridViewHalls.SelectedRows[0].Cells[2].Value.ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var frm = new frmThemSanh();
            var dialogRes = frm.ShowDialog();
            GetHallData();
            FillDataOfHalls(halls);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string id = dataGridViewHalls.SelectedRows[0].Cells[0].Value.ToString();
            var res = Program.hallRepository.DeleteHall(id);
            if (!res.isSuccess) MessageBox.Show(res.message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                GetHallData();
                FillDataOfHalls(halls);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            var res = Program.hallRepository.UpdateHall(_hall);
            if (!res.isSuccess) MessageBox.Show(res.message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                GetHallData();
                FillDataOfHalls(halls);
                txtHallname.Text = "";
                numericUpDownCapacity.Value = 0;
                richTxtNote.Text = "";
                txtAddress.Text = "";
            }
        }

        private void numericUpDownCapacity_ValueChanged(object sender, EventArgs e)
        {
            _hall.Capacity = Convert.ToInt32(numericUpDownCapacity.Value);
        }

        private void txtHallname_TextChanged(object sender, EventArgs e)
        {
            _hall.Name = txtHallname.Text;
        }

        private void cbbHallTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = cbbHallTypes.SelectedIndex;
            _hall.Type = hallTypes.ElementAt(selectedIndex);
        }

        private void richTxtNote_TextChanged(object sender, EventArgs e)
        {
            _hall.Note = richTxtNote.Text;
        }

        private void txtAddress_TextChanged(object sender, EventArgs e)
        {
            _hall.Address = txtAddress.Text;
        }
    }
}
