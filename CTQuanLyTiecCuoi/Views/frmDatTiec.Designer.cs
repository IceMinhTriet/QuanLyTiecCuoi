namespace CTQuanLyTiecCuoi.Views
{
    partial class frmDatTiec
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDatTiec));
            this.panel1 = new System.Windows.Forms.Panel();
            this.numericUpDownDuration = new System.Windows.Forms.NumericUpDown();
            this.label16 = new System.Windows.Forms.Label();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtPurpose = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtNin = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.txtCapacity = new System.Windows.Forms.TextBox();
            this.txtHallPrice = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.cbbHalls = new System.Windows.Forms.ComboBox();
            this.dtPickerOrganizingDate = new System.Windows.Forms.DateTimePicker();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.txtNumOfTables = new System.Windows.Forms.TextBox();
            this.txtCustomerName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cbbDishTypes = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.gropBox1 = new System.Windows.Forms.GroupBox();
            this.dataGridViewDishes = new System.Windows.Forms.DataGridView();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbEstimateCost = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.lblTinhTien = new System.Windows.Forms.Label();
            this.dataGridViewServices = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDuration)).BeginInit();
            this.panel2.SuspendLayout();
            this.gropBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDishes)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewServices)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.numericUpDownDuration);
            this.panel1.Controls.Add(this.label16);
            this.panel1.Controls.Add(this.txtStatus);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.txtPurpose);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.txtNin);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.txtAddress);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtEmail);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label14);
            this.panel1.Controls.Add(this.txtCapacity);
            this.panel1.Controls.Add(this.txtHallPrice);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.cbbHalls);
            this.panel1.Controls.Add(this.dtPickerOrganizingDate);
            this.panel1.Controls.Add(this.txtPhone);
            this.panel1.Controls.Add(this.txtNumOfTables);
            this.panel1.Controls.Add(this.txtCustomerName);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(878, 268);
            this.panel1.TabIndex = 0;
            // 
            // numericUpDownDuration
            // 
            this.numericUpDownDuration.Location = new System.Drawing.Point(726, 144);
            this.numericUpDownDuration.Name = "numericUpDownDuration";
            this.numericUpDownDuration.Size = new System.Drawing.Size(140, 24);
            this.numericUpDownDuration.TabIndex = 20;
            this.numericUpDownDuration.ValueChanged += new System.EventHandler(this.numericUpDownDuration_ValueChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(645, 144);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(68, 18);
            this.label16.TabIndex = 19;
            this.label16.Text = "Duration:";
            // 
            // txtStatus
            // 
            this.txtStatus.Enabled = false;
            this.txtStatus.Location = new System.Drawing.Point(726, 98);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.Size = new System.Drawing.Size(140, 24);
            this.txtStatus.TabIndex = 18;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(645, 102);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(77, 18);
            this.label9.TabIndex = 17;
            this.label9.Text = "Trạng thái:";
            // 
            // txtPurpose
            // 
            this.txtPurpose.Location = new System.Drawing.Point(726, 57);
            this.txtPurpose.Name = "txtPurpose";
            this.txtPurpose.Size = new System.Drawing.Size(140, 24);
            this.txtPurpose.TabIndex = 16;
            this.txtPurpose.TextChanged += new System.EventHandler(this.txtPurpose_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(645, 65);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 18);
            this.label6.TabIndex = 15;
            this.label6.Text = "Mục dích:";
            // 
            // txtNin
            // 
            this.txtNin.Location = new System.Drawing.Point(180, 201);
            this.txtNin.Name = "txtNin";
            this.txtNin.Size = new System.Drawing.Size(140, 24);
            this.txtNin.TabIndex = 13;
            this.txtNin.TextChanged += new System.EventHandler(this.txtNin_TextChanged);
            this.txtNin.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNin_KeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(57, 204);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 18);
            this.label5.TabIndex = 14;
            this.label5.Text = "CCCD:";
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(180, 165);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(140, 24);
            this.txtAddress.TabIndex = 11;
            this.txtAddress.TextChanged += new System.EventHandler(this.txtAddress_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(57, 168);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 18);
            this.label4.TabIndex = 12;
            this.label4.Text = "Địa chỉ:";
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(180, 129);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(140, 24);
            this.txtEmail.TabIndex = 9;
            this.txtEmail.TextChanged += new System.EventHandler(this.txtEmail_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(57, 132);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 18);
            this.label2.TabIndex = 10;
            this.label2.Text = "Email:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 17F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.Blue;
            this.label14.Location = new System.Drawing.Point(311, 9);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(214, 30);
            this.label14.TabIndex = 7;
            this.label14.Text = "ĐẶT TIỆC CƯỚI";
            // 
            // txtCapacity
            // 
            this.txtCapacity.Location = new System.Drawing.Point(532, 128);
            this.txtCapacity.Name = "txtCapacity";
            this.txtCapacity.ReadOnly = true;
            this.txtCapacity.Size = new System.Drawing.Size(91, 24);
            this.txtCapacity.TabIndex = 6;
            // 
            // txtHallPrice
            // 
            this.txtHallPrice.Location = new System.Drawing.Point(490, 96);
            this.txtHallPrice.Name = "txtHallPrice";
            this.txtHallPrice.ReadOnly = true;
            this.txtHallPrice.Size = new System.Drawing.Size(133, 24);
            this.txtHallPrice.TabIndex = 6;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(371, 132);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(139, 18);
            this.label13.TabIndex = 5;
            this.label13.Text = "Số lượng bàn tối đa:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(371, 98);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(99, 18);
            this.label12.TabIndex = 4;
            this.label12.Text = "Đơn giá sảnh:";
            // 
            // cbbHalls
            // 
            this.cbbHalls.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbHalls.FormattingEnabled = true;
            this.cbbHalls.Location = new System.Drawing.Point(490, 57);
            this.cbbHalls.Name = "cbbHalls";
            this.cbbHalls.Size = new System.Drawing.Size(133, 26);
            this.cbbHalls.TabIndex = 6;
            this.cbbHalls.SelectedIndexChanged += new System.EventHandler(this.cbbHalls_SelectedIndexChanged);
            // 
            // dtPickerOrganizingDate
            // 
            this.dtPickerOrganizingDate.CustomFormat = "";
            this.dtPickerOrganizingDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtPickerOrganizingDate.Location = new System.Drawing.Point(482, 206);
            this.dtPickerOrganizingDate.Name = "dtPickerOrganizingDate";
            this.dtPickerOrganizingDate.Size = new System.Drawing.Size(193, 24);
            this.dtPickerOrganizingDate.TabIndex = 8;
            this.dtPickerOrganizingDate.ValueChanged += new System.EventHandler(this.dtPickerOrganizingDate_ValueChanged);
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(180, 57);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(140, 24);
            this.txtPhone.TabIndex = 1;
            this.txtPhone.TextChanged += new System.EventHandler(this.txtPhone_TextChanged);
            this.txtPhone.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPhone_KeyPress);
            // 
            // txtNumOfTables
            // 
            this.txtNumOfTables.Location = new System.Drawing.Point(477, 167);
            this.txtNumOfTables.Name = "txtNumOfTables";
            this.txtNumOfTables.Size = new System.Drawing.Size(146, 24);
            this.txtNumOfTables.TabIndex = 5;
            this.txtNumOfTables.TextChanged += new System.EventHandler(this.txtNumOfTables_TextChanged);
            this.txtNumOfTables.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNumOfTables_KeyPress);
            // 
            // txtCustomerName
            // 
            this.txtCustomerName.Location = new System.Drawing.Point(180, 93);
            this.txtCustomerName.Name = "txtCustomerName";
            this.txtCustomerName.Size = new System.Drawing.Size(140, 24);
            this.txtCustomerName.TabIndex = 0;
            this.txtCustomerName.TextChanged += new System.EventHandler(this.txtCustomerName_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(57, 60);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(98, 18);
            this.label7.TabIndex = 0;
            this.label7.Text = "Số điện thoại:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(371, 170);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(99, 18);
            this.label10.TabIndex = 0;
            this.label10.Text = "Số lượng bàn:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(371, 63);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(46, 18);
            this.label8.TabIndex = 0;
            this.label8.Text = "Sảnh:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(371, 206);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 18);
            this.label3.TabIndex = 0;
            this.label3.Text = "Ngày tổ chức:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(57, 96);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tên khách hàng:";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.cbbDishTypes);
            this.panel2.Controls.Add(this.label11);
            this.panel2.Controls.Add(this.gropBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 268);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(878, 203);
            this.panel2.TabIndex = 1;
            // 
            // cbbDishTypes
            // 
            this.cbbDishTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbDishTypes.FormattingEnabled = true;
            this.cbbDishTypes.Location = new System.Drawing.Point(316, 7);
            this.cbbDishTypes.Name = "cbbDishTypes";
            this.cbbDishTypes.Size = new System.Drawing.Size(283, 26);
            this.cbbDishTypes.TabIndex = 0;
            this.cbbDishTypes.SelectedIndexChanged += new System.EventHandler(this.cbbDishTypes_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(222, 9);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(85, 18);
            this.label11.TabIndex = 3;
            this.label11.Text = "Chọn theo: ";
            // 
            // gropBox1
            // 
            this.gropBox1.Controls.Add(this.dataGridViewDishes);
            this.gropBox1.Location = new System.Drawing.Point(3, 9);
            this.gropBox1.Name = "gropBox1";
            this.gropBox1.Size = new System.Drawing.Size(875, 188);
            this.gropBox1.TabIndex = 0;
            this.gropBox1.TabStop = false;
            this.gropBox1.Text = "Chọn món ăn/bàn:";
            // 
            // dataGridViewDishes
            // 
            this.dataGridViewDishes.AllowUserToAddRows = false;
            this.dataGridViewDishes.AllowUserToDeleteRows = false;
            this.dataGridViewDishes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDishes.Location = new System.Drawing.Point(1, 30);
            this.dataGridViewDishes.MultiSelect = false;
            this.dataGridViewDishes.Name = "dataGridViewDishes";
            this.dataGridViewDishes.RowHeadersVisible = false;
            this.dataGridViewDishes.RowHeadersWidth = 45;
            this.dataGridViewDishes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewDishes.Size = new System.Drawing.Size(874, 158);
            this.dataGridViewDishes.TabIndex = 0;
            this.dataGridViewDishes.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewDishes_CellValueChanged);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(569, 259);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(122, 43);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Lưu";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(721, 259);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 43);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Hủy";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbEstimateCost);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.lblTinhTien);
            this.groupBox1.Controls.Add(this.btnCancel);
            this.groupBox1.Controls.Add(this.dataGridViewServices);
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 471);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(878, 311);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Chọn dịch vụ:";
            // 
            // lbEstimateCost
            // 
            this.lbEstimateCost.AutoSize = true;
            this.lbEstimateCost.Location = new System.Drawing.Point(156, 271);
            this.lbEstimateCost.Name = "lbEstimateCost";
            this.lbEstimateCost.Size = new System.Drawing.Size(16, 18);
            this.lbEstimateCost.TabIndex = 7;
            this.lbEstimateCost.Text = "0";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(29, 271);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(120, 18);
            this.label15.TabIndex = 6;
            this.label15.Text = "Tổng tiền dự tính:";
            // 
            // lblTinhTien
            // 
            this.lblTinhTien.AutoSize = true;
            this.lblTinhTien.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTinhTien.ForeColor = System.Drawing.Color.Red;
            this.lblTinhTien.Location = new System.Drawing.Point(155, 244);
            this.lblTinhTien.Name = "lblTinhTien";
            this.lblTinhTien.Size = new System.Drawing.Size(0, 29);
            this.lblTinhTien.TabIndex = 5;
            // 
            // dataGridViewServices
            // 
            this.dataGridViewServices.AllowUserToAddRows = false;
            this.dataGridViewServices.AllowUserToDeleteRows = false;
            this.dataGridViewServices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewServices.Dock = System.Windows.Forms.DockStyle.Top;
            this.dataGridViewServices.Location = new System.Drawing.Point(3, 20);
            this.dataGridViewServices.MultiSelect = false;
            this.dataGridViewServices.Name = "dataGridViewServices";
            this.dataGridViewServices.RowHeadersVisible = false;
            this.dataGridViewServices.RowHeadersWidth = 45;
            this.dataGridViewServices.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewServices.Size = new System.Drawing.Size(872, 218);
            this.dataGridViewServices.TabIndex = 0;
            this.dataGridViewServices.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewServices_CellValueChanged);
            // 
            // frmDatTiec
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(878, 782);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "frmDatTiec";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Đặt tiệc";
            this.Load += new System.EventHandler(this.frmDatTiec_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDuration)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.gropBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDishes)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewServices)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cbbHalls;
        private System.Windows.Forms.DateTimePicker dtPickerOrganizingDate;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.TextBox txtNumOfTables;
        private System.Windows.Forms.TextBox txtCustomerName;
        private System.Windows.Forms.ComboBox cbbDishTypes;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox gropBox1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dataGridViewServices;
        private System.Windows.Forms.Label lblTinhTien;
        private System.Windows.Forms.TextBox txtCapacity;
        private System.Windows.Forms.TextBox txtHallPrice;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtNin;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView dataGridViewDishes;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtPurpose;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lbEstimateCost;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.NumericUpDown numericUpDownDuration;
        private System.Windows.Forms.Label label16;
    }
}