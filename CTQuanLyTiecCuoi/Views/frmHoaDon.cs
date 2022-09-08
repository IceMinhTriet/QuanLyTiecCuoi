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
    public partial class frmHoaDon : Form
    {
        private Billing bill;
        readonly Reservation reserv;

        public frmHoaDon(Reservation r)
        {
            InitializeComponent();
            reserv = r;
            GetBill();
        }

        private void GetBill ()
        {
            var res = Program.billingRepository.GetBillingByReservationId(reserv.Id);
            if (!res.isSuccess)
            {
                MessageBox.Show(res.message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bill = res.Bill;
        }

        

        private void frmHoaDon_Load(object sender, EventArgs e)
        {
            lbCustomerName.Text = reserv.Customer.Fullname;
            lbNumOfTables.Text = reserv.NumOfTables.ToString();
            lbPaymentDate.Text = bill.PaymentDate.ToString();
            lbHall.Text = reserv.Hall.Name;
            lbDuration.Text = reserv.Duration.ToString();
            lbHallPrice.Text = Math.Round(reserv.Hall.Type.UnitPrice, 2) + "VND";
            lbTotalFoodCost.Text = Math.Round(bill.FoodCost, 2) + "VND";
            lbTotalServiceCost.Text = Math.Round(bill.ServiceCost, 2) + "VND";
            lbTotalBill.Text = Math.Round(bill.TotalCost, 2)+ "VND";
            lbDeposit.Text = Math.Round(bill.Deposit, 2) + "VND";
            lbRemainingCost.Text = Math.Round(bill.RemainingCost, 2) + "VND";

            populateServices();
            populateDishes();

        }

        private void populateServices()
        {
            reserv.Services.ForEach(e =>
            {
                ListViewItem lvi = new ListViewItem(e.Name);
                lvi.SubItems.Add(Math.Round(e.UnitPrice).ToString());
                listViewServices.Items.Add(lvi);
            });
        }

        private void populateDishes()
        {
            reserv.Menu.ForEach(e =>
            {
                ListViewItem lvi = new ListViewItem(e.Name);
                lvi.SubItems.Add(Math.Round(e.UnitPrice).ToString());
                listViewDishes.Items.Add(lvi);
            });
        }


        private void frmHoaDon_FormClosing(object sender, FormClosingEventArgs e)
        {
           
        }
    }
}
