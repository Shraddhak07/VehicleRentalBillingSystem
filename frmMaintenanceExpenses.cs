using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.OleDb;

namespace VehicleRentalApp
{
    public partial class frmMaintenanceExpenses : Form
    {
        private ComboBox cmbVehicle;
        private DateTimePicker dtpDate;
        private TextBox txtAmount, txtDescription;
        private Button btnSave;
        private Label lblTitle;

        public frmMaintenanceExpenses() => InitializeComponent();

        private void InitializeComponent()
        {
            lblTitle = new Label
            {
                Text = "Maintenance Expenses",
                Font = Theme.TitleFont,
                ForeColor = Theme.PrimaryColor,
                AutoSize = true,
                Location = new Point(20, 20)
            };

            var formPanel = new Panel
            {
                Size = new Size(500, 400),
                Location = new Point((Screen.PrimaryScreen.Bounds.Width - 500) / 2, (Screen.PrimaryScreen.Bounds.Height - 400) / 2),
                BackColor = Theme.CardColor,
                BorderStyle = BorderStyle.FixedSingle
            };

            cmbVehicle = new ComboBox
            {
                Width = 400,
                DropDownStyle = ComboBoxStyle.DropDownList,
                DataSource = DatabaseHelper.GetAllVehicles(),
                DisplayMember = "VehicleName",
                ValueMember = "VehicleID",
                Font = Theme.BodyFont,
                Location = new Point(40, 125)
            };
            dtpDate = new DateTimePicker { Width = 400, Value = DateTime.Today, Font = Theme.BodyFont, Location = new Point(40, 180) };
            txtAmount = new TextBox { Width = 400, Font = Theme.BodyFont, Location = new Point(40, 235), BorderStyle = BorderStyle.FixedSingle };
            txtDescription = new TextBox { Width = 400, Font = Theme.BodyFont, Location = new Point(40, 290), BorderStyle = BorderStyle.FixedSingle };

            void AddInput(Label label, Control control, int y)
            {
                label.ForeColor = Theme.TextColor;
                label.Font = Theme.HeadingFont;
                label.AutoSize = true;
                label.Location = new Point(40, y);
                formPanel.Controls.Add(label);
                formPanel.Controls.Add(control);
            }

            AddInput(new Label { Text = "Vehicle:" }, cmbVehicle, 80);
            AddInput(new Label { Text = "Expense Date:" }, dtpDate, 135);
            AddInput(new Label { Text = "Amount Spent:" }, txtAmount, 190);
            AddInput(new Label { Text = "Description:" }, txtDescription, 245);

            btnSave = new Button { Text = "Save Expense", Width = 200, Location = new Point(150, 350) };
            btnSave.FlatStyle = FlatStyle.Flat;
            Theme.ApplyButtonStyle(btnSave, Theme.WarningColor);
            btnSave.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 210, 80);
            btnSave.Click += BtnSave_Click;
            formPanel.Controls.Add(btnSave);

            Controls.Add(formPanel);
            Controls.Add(lblTitle);
            Text = "Maintenance Expenses";
            StartPosition = FormStartPosition.Manual;
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Theme.BackgroundColor;
            WindowState = FormWindowState.Maximized;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using var conn = DatabaseHelper.GetOpenConnection();
                using var cmd = new OleDbCommand(
                    "INSERT INTO Maintenance (VehicleID, ExpenseDate, AmountSpent, Description) VALUES (?, ?, ?, ?)", conn);
                cmd.Parameters.AddWithValue("?", Convert.ToInt32(cmbVehicle.SelectedValue));
                cmd.Parameters.AddWithValue("?", dtpDate.Value);
                cmd.Parameters.AddWithValue("?", decimal.Parse(txtAmount.Text));
                cmd.Parameters.AddWithValue("?", txtDescription.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Maintenance expense recorded!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtAmount.Clear(); txtDescription.Clear();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
    }
}
