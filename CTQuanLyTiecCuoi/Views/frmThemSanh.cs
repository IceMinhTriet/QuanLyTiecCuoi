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
    public partial class frmThemSanh : Form
    {
        List<HallType> hallTypes;
        Hall hall;

        public frmThemSanh()
        {
            InitializeComponent();
            hallTypes = new List<HallType>();
            hall = new Hall();
        }

        private void populateHallTypes(List<HallType> hallTypes)
        {
            List<string> types = new List<string>();
            //hallTypes.ForEach(e => types.Add(e.TypeName));
            types = hallTypes.Select(e => e.TypeName).ToList();
            cbbHallTypes.Items.Clear();
            cbbHallTypes.DataSource = types;
        }

        private void frmThemsanh_Load(object sender, EventArgs e)
        {
            var res = Program.hallRepository.GetHallTypes();
            if (!res.isSuccess) MessageBox.Show(res.message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                hallTypes = res.HallTypes;
                populateHallTypes(res.HallTypes);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(hall.Name) || string.IsNullOrEmpty(hall.Type.Id))
            {
                errorProvider.SetError(sender as Control, "Invalid data");
                return;
            }
            else errorProvider.Clear();

            var res = Program.hallRepository.InsertHall(hall);
            if (!res.isSuccess)
            {
                MessageBox.Show(res.message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            MessageBox.Show(res.message, "Inform", MessageBoxButtons.OK);
            Close();
        }

        private void txtHallname_TextChanged(object sender, EventArgs e)
        {
            hall.Name = txtHallname.Text;
        }

        private void numericUpDownCapacity_ValueChanged(object sender, EventArgs e)
        {
            hall.Capacity = Convert.ToInt32(numericUpDownCapacity.Value);
        }

        private void cbbHallTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = cbbHallTypes.SelectedIndex;
            hall.Type = hallTypes.ElementAt(selectedIndex);
        }

        private void txtAddress_TextChanged(object sender, EventArgs e)
        {
            hall.Address = txtAddress.Text;
        }

        private void richTxtNote_TextChanged(object sender, EventArgs e)
        {
            hall.Note = richTxtNote.Text;
        }
    }
}
