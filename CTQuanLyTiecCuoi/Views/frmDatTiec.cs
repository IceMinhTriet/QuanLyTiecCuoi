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
using CTQuanLyTiecCuoi.Entities.Responses;

namespace CTQuanLyTiecCuoi.Views
{
    public partial class frmDatTiec : Form
    {
        List<string> dishTypes;
        List<Dish> dishes;
        List<Service> services;
        List<Hall> halls;
        Reservation reserv;
        DateTime originalOrganizationDate;
        bool isUpdating;
        bool isCustomerExisted = false;
        bool isFirstTimeUpdated;

        public frmDatTiec(Reservation r)
        {
            InitializeComponent();
            dishTypes = new List<string>();
            dishes = new List<Dish>();
            services = new List<Service>();
            halls = new List<Hall>();
            reserv = new Reservation
            {
                Services = new List<Service>(),
                Hall = new Hall(),
                Menu = new List<Dish>(),
                Customer = new Customer()
            };
            if (r == null)
            {
                isUpdating = false;
            }
            else
            {
                isUpdating = true;
                isFirstTimeUpdated = true;
                reserv = r;
                originalOrganizationDate = reserv.OrganizingDate;

                //int hallIndex = halls.IndexOf(reservation.Hall);
                //cbbHalls.SelectedIndex = hallIndex;
            }
                        
        }

        private void frmDatTiec_Load(object sender, EventArgs e)
        {
            CreateColsForDataGVDishes();
            CreateColsForDataGVServices();

            PopulateDishTypes();
            GetHalls();
            PopulateHalls(halls);
            dtPickerOrganizingDate.Format = DateTimePickerFormat.Custom;
            dtPickerOrganizingDate.CustomFormat = "dd/MM/yyyy  HH:mm:ss";


            if (isUpdating)
            {
                txtPhone.Text = reserv.Customer.Phone;
                txtEmail.Text = reserv.Customer.Email;
                txtCustomerName.Text = reserv.Customer.Fullname;
                txtAddress.Text = reserv.Customer.Address;
                txtNin.Text = reserv.Customer.NIN;
                dtPickerOrganizingDate.Value = reserv.OrganizingDate;
                txtNumOfTables.Text = reserv.NumOfTables.ToString();
                txtPurpose.Text = reserv.Purpose;
                txtStatus.Text = reserv.Status;
                lbEstimateCost.Text = reserv.EstimatedCost.ToString();
                numericUpDownDuration.Value = reserv.Duration;
                

                int hallIndex = halls.IndexOf(reserv.Hall);
                cbbHalls.SelectedIndex = hallIndex;

                txtHallPrice.Text = reserv.Hall.Type.UnitPrice.ToString();
                txtCapacity.Text = reserv.Hall.Capacity.ToString();

                GetDishes();
                PopulateSelectedDishes(dishes, reserv.Menu);
                GetServices();
                PopulateSelectedServices(services, reserv.Services);

            }
            else
            {
                GetDishes();
                PopulateDishes(dishes);
                GetServices();
                PopulateServices(services);
            }
        }

        private decimal GetEstimateCost()
        {
            decimal cost = 0;
            // hall
            int duration = (reserv.Duration == 0) ? 1 : reserv.Duration;
            int tables = (reserv.NumOfTables == 0) ? 1 : reserv.NumOfTables;
            cost += reserv.Hall.Type.UnitPrice * duration;
            // services
            if (reserv.Services.Count > 0)
                cost += reserv.Services.Sum(s => s.UnitPrice);
            // dishes
            if (reserv.Menu.Count > 0)
            {
                decimal foodCost = reserv.Menu.Sum(d => d.UnitPrice);
                cost += foodCost * tables;
            }
            return cost;
        }

        private void cbbDishTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isUpdating)
            {
                GetDishes();
                PopulateSelectedDishes(dishes, reserv.Menu);
            }
            else
            {
                GetDishes();
                PopulateDishes(dishes);
            }

        }

        private void txtPhone_TextChanged(object sender, EventArgs e)
        {
            if(isUpdating)
            {
                reserv.Customer.Phone = txtPhone.Text;
            }
            else
            {
                string phone = txtPhone.Text;
                if (phone.Length == 10)
                {
                    var res = Program.customerRepository.GetCustomerByPhone(phone);
                    if (res.isSuccess)
                    {
                        reserv.Customer = res.Customer;
                        txtCustomerName.Text = res.Customer.Fullname;
                        txtEmail.Text = res.Customer.Email;
                        txtAddress.Text = res.Customer.Address;
                        txtNin.Text = res.Customer.NIN;
                        reserv.Customer = res.Customer;
                        isCustomerExisted = true;
                        return;
                    }
                }

                reserv.Customer.Phone = txtPhone.Text;
            }
        }

        private void cbbHalls_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = cbbHalls.SelectedIndex;
            Hall hall = halls.ElementAt(index);

            txtHallPrice.Text = hall.Type.UnitPrice.ToString();
            txtCapacity.Text = hall.Capacity.ToString();
            reserv.Hall = hall;            

            if (isUpdating && isFirstTimeUpdated)
            {
                GetServices();
                PopulateSelectedServices(services, reserv.Services);
                isFirstTimeUpdated = false;
                return;

            }

            reserv.Services.Clear();
            GetServices();
            PopulateServices(services);
            

            lbEstimateCost.Text = GetEstimateCost().ToString();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn muốn hủy thao tác?",
                "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Close();
            }
            else if (result == DialogResult.No)
                return;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (isUpdating)
            {
                if (!reserv.OrganizingDate.Equals(originalOrganizationDate))
                {
                    bool isValidOrganizingDate = Program.reservationRepositoy.CheckValidOrganizingDatetime(reserv.OrganizingDate);
                    if (!isValidOrganizingDate)
                    {
                        MessageBox.Show("Thời gian đã được đặt!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                
                var updatingCustomerRes = Program.customerRepository.UpdateCustomer(reserv.Customer);
                if (!updatingCustomerRes.isSuccess)
                {
                    MessageBox.Show(updatingCustomerRes.message, "Error");
                    return;
                }
                var res = Program.reservationRepositoy.UpdateReservation(reserv);
                if (!res.isSuccess)
                {
                    MessageBox.Show(res.message, "Error");
                    return;
                }
                else MessageBox.Show(res.message);
                Close();
            } 
            else
            {
                bool isValidOrganizingDate = Program.reservationRepositoy.CheckValidOrganizingDatetime(reserv.OrganizingDate);
                if (!isValidOrganizingDate)
                {
                    MessageBox.Show("Thời gian đã được đặt!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!isCustomerExisted)
                { 
                    var insertCustomer = Program.customerRepository.InsertCustomer(reserv.Customer);
                    if (!insertCustomer.isSuccess)
                    {
                        MessageBox.Show(insertCustomer.message, "Error");
                        return;
                    }
                    var gettingNewCustomerRes = Program.customerRepository.GetCustomerByPhone(reserv.Customer.Phone);
                    reserv.Customer.Id = gettingNewCustomerRes.Customer.Id;
                }

                var res = Program.reservationRepositoy.InsertReservation(reserv);
                if (!res.isSuccess)
                {
                    MessageBox.Show(res.message, "Error");
                    return;
                }
                MessageBox.Show(res.message);
                var frm = new frmDanhSachTiec();
                Hide();
                frm.ShowDialog();
                Close();
            }

            
        }

        private void dataGridViewDishes_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewDishes.Columns[e.ColumnIndex].Name == "IsSelected")
            {
                DataGridViewCheckBoxCell checkCell = (DataGridViewCheckBoxCell) dataGridViewDishes.Rows[e.RowIndex].Cells["IsSelected"];
                int i = dataGridViewDishes.CurrentRow.Index;
                Dish d = dishes.ElementAt(i);
                if ((bool)checkCell.Value == true)
                {                    
                    if (!reserv.Menu.Contains(d))
                        reserv.Menu.Add(d);
                }
                else
                {                    
                    if (reserv.Menu.Contains(d))
                        reserv.Menu.Remove(d);

                }
                lbEstimateCost.Text = GetEstimateCost().ToString();
            }
        }

        private void dataGridViewServices_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCheckBoxCell checkCell = (DataGridViewCheckBoxCell)dataGridViewServices.Rows[e.RowIndex].Cells["IsSelected"];
            int i = dataGridViewServices.CurrentRow.Index;
            Service s = services.ElementAt(i);

            if ((bool)checkCell.Value == true)
            {
                if (!reserv.Services.Contains(s))
                    reserv.Services.Add(s);
            }
            else
            {
                if (reserv.Services.Contains(s))
                    reserv.Services.Remove(s);
            }
            lbEstimateCost.Text = GetEstimateCost().ToString();
        }

        private void numericUpDownDuration_ValueChanged(object sender, EventArgs e)
        {
            reserv.Duration = Convert.ToInt32(numericUpDownDuration.Value);
            lbEstimateCost.Text = GetEstimateCost().ToString();
        }

        private void txtCustomerName_TextChanged(object sender, EventArgs e)
        {
            reserv.Customer.Fullname = txtCustomerName.Text;
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            reserv.Customer.Email = txtEmail.Text;
        }

        private void txtAddress_TextChanged(object sender, EventArgs e)
        {
            reserv.Customer.Address = txtAddress.Text;
        }

        private void txtNin_TextChanged(object sender, EventArgs e)
        {
            reserv.Customer.NIN = txtNin.Text;
        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtNumOfTables_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtNin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtNumOfTables_TextChanged(object sender, EventArgs e)
        {
           int not = Convert.ToInt32(txtNumOfTables.Text);
            if (not > reserv.Hall.Capacity)
            {
                MessageBox.Show("Invalid number of tables", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            reserv.NumOfTables = Convert.ToInt32(txtNumOfTables.Text);
        }

        private void dtPickerOrganizingDate_ValueChanged(object sender, EventArgs e)
        {
            DateTime dt = dtPickerOrganizingDate.Value;

            reserv.OrganizingDate = dt;
        }

        private void txtPurpose_TextChanged(object sender, EventArgs e)
        {
            reserv.Purpose = txtPurpose.Text;
        }

        private void GetHalls()
        {
            var res = Program.hallRepository.GetHalls();
            if (!res.isSuccess)
            {
                MessageBox.Show(res.message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            halls = res.Halls;
        }

        private void GetDishes()
        {
            string type = dishTypes.ElementAt(cbbDishTypes.SelectedIndex);

            DishResponse res;

            if (type == "") res = Program.dishRepository.GetDishes();
            else res = Program.dishRepository.GetDishesByType(type);

            if (!res.isSuccess)
            {
                MessageBox.Show(res.message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            dishes = res.Dishes;

        }

        private void GetServices()
        {
            //int selectedIndex = cbbHalls.SelectedIndex;
            Hall selectedHall = halls.ElementAt(cbbHalls.SelectedIndex);
            //Hall selectedHall = reservation.Hall;

            var res = Program.serviceRepository.GetSevicesByHallId(selectedHall.Id);
            if (!res.isSuccess)
            {
                MessageBox.Show(res.message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            services = res.Services;

        }

        private void PopulateDishTypes()
        {
            dishTypes = Program.dishRepository.GetDishTypes();

            cbbDishTypes.Items.Clear();
            cbbDishTypes.DataSource = dishTypes;
            cbbDishTypes.SelectedIndex = 0;
        }

        private void PopulateHalls(List<Hall> halls)
        {
            List<string> hallNames = halls.Select(h => h.Name).ToList();

            cbbHalls.Items.Clear();
            cbbHalls.DataSource = hallNames;
            cbbHalls.SelectedIndex = 0;
        }

        private void PopulateDishes(List<Dish> dishes)
        {
            dataGridViewDishes.Rows.Clear();
            //CreateColsForDataGVDishes();
            dishes.ForEach(d =>
            {
                dataGridViewDishes.Rows.Add(d.Id, d.Name, d.UnitPrice, d.Type, d.Note, false);
            });

            dataGridViewDishes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewDishes.AllowUserToAddRows = false;
            //dataGridViewDishes.Columns[4].Visible = false;
        }

        private void PopulateSelectedDishes(List<Dish> dishes, List<Dish> selectedDishes)
        {
            dataGridViewDishes.Rows.Clear();

            dishes.ForEach(d =>
            {
                bool isSelected = false;
                if (selectedDishes.Exists(t => t.Id == d.Id)) isSelected = true;
                dataGridViewDishes.Rows.Add(d.Id, d.Name, d.UnitPrice, d.Type, d.Note, isSelected);
            });

            dataGridViewDishes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewDishes.AllowUserToAddRows = false;
            //dataGridViewDishes.Columns[4].Visible = false;
        }

        private void PopulateServices(List<Service> services)
        {
            dataGridViewServices.Rows.Clear();

            services.ForEach(s =>
            {
                dataGridViewServices.Rows.Add(s.Id, s.Name, s.UnitPrice, false);
            });

            dataGridViewServices.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewServices.AllowUserToAddRows = false;
            //dataGridViewServices.Columns[4].Visible = false;
        }

        private void PopulateSelectedServices(List<Service> services, List<Service> selectedServices)
        {
            dataGridViewServices.Rows.Clear();

            services.ForEach(d =>
            {
                bool isSelected = false;
                if (selectedServices.Exists(t => t.Id == d.Id)) isSelected = true;
                dataGridViewServices.Rows.Add(d.Id, d.Name, d.UnitPrice, isSelected);
            });

            dataGridViewServices.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewServices.AllowUserToAddRows = false;
        }

        private void CreateColsForDataGVDishes()
        {
            dataGridViewDishes.Columns.Clear();
            DataGridViewTextBoxColumn dishNameCol = new DataGridViewTextBoxColumn();
            dishNameCol.Name = "Name";
            dishNameCol.HeaderText = "Tên món ăn";
            DataGridViewTextBoxColumn dishPriceCol = new DataGridViewTextBoxColumn();
            dishPriceCol.Name = "Price";
            dishPriceCol.HeaderText = "Đơn giá";
            DataGridViewTextBoxColumn dishTypeCol = new DataGridViewTextBoxColumn();
            dishTypeCol.Name = "Type";
            dishTypeCol.HeaderText = "Loại món ăn";
            DataGridViewTextBoxColumn dishNoteCol = new DataGridViewTextBoxColumn();
            dishNoteCol.Name = "Note";
            dishNoteCol.HeaderText = "Ghi chú";
            DataGridViewTextBoxColumn dishIdCol = new DataGridViewTextBoxColumn();
            dishIdCol.Name = "Id";
            dishIdCol.HeaderText = "Id";
            DataGridViewCheckBoxColumn dishIsSelectedCol = new DataGridViewCheckBoxColumn();
            dishIsSelectedCol.Name = "IsSelected";
            dishIsSelectedCol.HeaderText = "Chọn";
            dataGridViewDishes.Columns.Add(dishIdCol);
            dataGridViewDishes.Columns.Add(dishNameCol);
            dataGridViewDishes.Columns.Add(dishPriceCol);
            dataGridViewDishes.Columns.Add(dishTypeCol);
            dataGridViewDishes.Columns.Add(dishNoteCol);
            dataGridViewDishes.Columns.Add(dishIsSelectedCol);
        }

        private void CreateColsForDataGVServices()
        {
            dataGridViewServices.Columns.Clear();
            DataGridViewTextBoxColumn serviceNameCol = new DataGridViewTextBoxColumn();
            serviceNameCol.Name = "Name";
            serviceNameCol.HeaderText = "Tên dịch vụ";
            DataGridViewTextBoxColumn servicePriceCol = new DataGridViewTextBoxColumn();
            servicePriceCol.Name = "Price";
            servicePriceCol.HeaderText = "Đơn giá";
            DataGridViewTextBoxColumn serviceIdCol = new DataGridViewTextBoxColumn();
            serviceIdCol.Name = "Id";
            serviceIdCol.HeaderText = "Id";
            DataGridViewCheckBoxColumn serviceIsSelectedCol = new DataGridViewCheckBoxColumn();
            serviceIsSelectedCol.Name = "IsSelected";
            serviceIsSelectedCol.HeaderText = "Chọn";
            dataGridViewServices.Columns.Add(serviceIdCol);
            dataGridViewServices.Columns.Add(serviceNameCol);
            dataGridViewServices.Columns.Add(servicePriceCol);
            dataGridViewServices.Columns.Add(serviceIsSelectedCol);
        }

    }
}
