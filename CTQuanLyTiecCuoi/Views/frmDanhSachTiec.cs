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
    public partial class frmDanhSachTiec : Form
    {
        string searchText = "";
        Reservation reservation;
        List<Reservation> reservations;

        public frmDanhSachTiec()
        {
            InitializeComponent();
            reservation = new Reservation();
            reservations = new List<Reservation>();
        }

        private void GetReservationData()
        {
            var res = Program.reservationRepositoy.GetReservations();
            if (!res.isSuccess) MessageBox.Show(res.message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                reservations = res.Reservations;
            }
        }

        private void PopulateDataOfResevations(List<Reservation> reservations)
        {
            var columns = from r in reservations
                          select new
                          {
                              r.Id,
                              CustomerId = r.Customer.Id,
                              r.Customer.Fullname,
                              r.Customer.Phone,
                              HallId = r.Hall.Id,
                              r.Duration,
                              r.EstimatedCost,
                              r.NumOfTables,
                              r.Purpose,
                              r.Status
                          };
            dataGridViewReservations.DataSource = columns.ToList();
        }

        private void HandleSearch()
        {
            List<Reservation> filtered = new List<Reservation>();
            filtered = reservations.FindAll(r => r.Customer.Phone.Contains(searchText));
            PopulateDataOfResevations(filtered);
        }
        
        private void EnableInput()
        {
            btnPay.Enabled = true;
            btnUpdate.Enabled = true;
            btnCancel.Enabled = true;
        }

        private void DisableInput()
        {
            btnPay.Enabled = false;
            btnUpdate.Enabled = false;
            btnCancel.Enabled = false;
        }

        private void frmDanhSachTiec_Load(object sender, EventArgs e)
        {
            GetReservationData();
            PopulateDataOfResevations(reservations);
            DisableInput();
        }


        private void btnSuaTiec_Click(object sender, EventArgs e)
        {
            if (dataGridViewReservations.SelectedRows.Count == 1)
            {
                var res = Program.reservationRepositoy.GetReservationById(reservation.Id);
                reservation = res.Reservation;
                frmDatTiec frm = new frmDatTiec(reservation);
                var dialogRes = frm.ShowDialog();
            }

        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            if(dataGridViewReservations.SelectedRows.Count==1)
            {
                var paymentRes = Program.billingRepository.UpdateBillingStatus(reservation.Id, Program.session.UserId);
                if (!paymentRes.isSuccess)
                {
                    MessageBox.Show(paymentRes.message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                var res = Program.reservationRepositoy.GetReservationById(reservation.Id);
                reservation = res.Reservation;
                frmHoaDon frm = new frmHoaDon(reservation);
                var dialogRes = frm.ShowDialog();

            }
        }

        private void frmDanhSachTiec_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn muốn thoát?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                //IsUpdate = false;
                e.Cancel = false;
            }
            else if (result == DialogResult.No)
                e.Cancel = true;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            HandleSearch();
        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                HandleSearch();
                return;
            }

            if (!char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
            else e.Handled = false;
        }

        private void txtPhone_TextChanged(object sender, EventArgs e)
        {
            searchText = txtPhone.Text;
        }

        private void dataGridViewReservations_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int selectedIndex = dataGridViewReservations.CurrentCell.RowIndex;
            reservation = reservations.ElementAt(selectedIndex);
            if (string.Equals(reservation.Status,"Pending", StringComparison.CurrentCultureIgnoreCase))
            {
                EnableInput();
            }
            else
            {
                DisableInput();
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            PopulateDataOfResevations(reservations);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            var res = Program.reservationRepositoy.CancelReservation(reservation.Id);
            if (!res.isSuccess)
            {
                MessageBox.Show(res.message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            GetReservationData();
            PopulateDataOfResevations(reservations);
        }
    }
}
