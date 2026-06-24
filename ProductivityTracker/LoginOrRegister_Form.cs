using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProductivityTracker
{
    public partial class LoginOrRegister_Form : Form
    {
        GlobalConfig Connection;
        int KeyCount = 0;

        public LoginOrRegister_Form()
        {
            InitializeComponent();
            Connection = new GlobalConfig();
            textBoxPasswordRegister.UseSystemPasswordChar = true;
            textBoxPasswordLogin.UseSystemPasswordChar = true;
        }

        private void LoginOrRegister_Form_Load(object sender, EventArgs e)
        {
            buttonClose.BackgroundImage = Image.FromFile("C:\\Users\\USER\\source\\repos\\drumin4\\time-eclipse\\ProductivityTracker\\img\\X_Image.png");
            buttonClose.BackgroundImageLayout = ImageLayout.Zoom;
            buttonMinimize.BackgroundImage = Image.FromFile("C:\\Users\\USER\\source\\repos\\drumin4\\time-eclipse\\ProductivityTracker\\img\\-_Image.png");
            buttonMinimize.BackgroundImageLayout = ImageLayout.Zoom;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(panelLayout.Location.X > -500)
            {
                panelLayout.Location = new Point(panelLayout.Location.X - 8, panelLayout.Location.Y);
            }
            else
            {
                timer1.Stop();
                labelGoToLogin.Enabled = true;
                labelGoToRegister.Enabled = true;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (panelLayout.Location.X < 0)
            {
                panelLayout.Location = new Point(panelLayout.Location.X + 8, panelLayout.Location.Y);
            }
            else
            {
                timer2.Stop();
                labelGoToLogin.Enabled = true;
                labelGoToRegister.Enabled = true;
            }
        }

        private void labelGoToRegister_Click(object sender, EventArgs e)
        {
            timer1.Start();
            labelGoToLogin.Enabled = false;
            labelGoToRegister.Enabled = false;
        }

        private void labelGoToLogin_Click(object sender, EventArgs e)
        {
            timer2.Start();
            labelGoToLogin.Enabled = false;
            labelGoToRegister.Enabled = false;
        }

        private void buttonMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buttonRegister_Click(object sender, EventArgs e)
        {
            string Query = $"select * from usertable where Username = '{textBoxUsernameRegister.Text}'";
            DataTable dataTable = Connection.GetData(Query);

            if(textBoxFirstNameRegister.Text == "" || textBoxLastNameRegister.Text == "" || textBoxUsernameRegister.Text == "" || textBoxPasswordRegister.Text == "")
            {
                MessageBox.Show("Missing Information!", "Warning");
            } 
            else if(dataTable.Rows.Count > 0)
            {
                labelRegisterUsernameError.Text = "The username is taken. Try another.";
                panelRegisterUsernameError.Visible = true;
                panelRegisterPasswordError.Visible = false;
            }
            else if(textBoxPasswordRegister.Text.Length < 8 || (textBoxUsernameRegister.Text.Length < 6 || textBoxUsernameRegister.Text.Length > 30))
            {
                panelRegisterUsernameError.Visible = false;

                if (textBoxPasswordRegister.Text.Length < 8 && (textBoxUsernameRegister.Text.Length < 6 || textBoxUsernameRegister.Text.Length > 30))
                {
                    labelRegisterUsernameError.Text = "Sorry, your username must be between 6 and 30 characters long.";
                    panelRegisterUsernameError.Visible = true;
                    panelRegisterPasswordError.Visible = true;
                }
                else if(textBoxUsernameRegister.Text.Length < 6 || textBoxUsernameRegister.Text.Length > 30)
                {
                    labelRegisterUsernameError.Text = "Sorry, your username must be between 6 and 30 characters long.";
                    panelRegisterUsernameError.Visible = true;
                    panelRegisterPasswordError.Visible = false;
                }

                else if(textBoxPasswordRegister.Text.Length < 8)
                {
                    panelRegisterPasswordError.Visible = true;
                    panelRegisterUsernameError.Visible = false;
                }
            }
            else
            {
                panelRegisterUsernameError.Visible = false;
                panelRegisterPasswordError.Visible = false;
                SaveUser();
                MessageBox.Show("Registration Complete!", "Info");
                labelGoToLogin_Click(sender, e);
            }
        }

        private void SaveUser()
        {
            string FirstName = textBoxFirstNameRegister.Text;
            string LastName = textBoxLastNameRegister.Text;
            string Username = textBoxUsernameRegister.Text;
            string Password = textBoxPasswordRegister.Text;
            DataTable dt = Connection.GetData("select * from UserTable");
            dt.Rows.Add();
            
            KeyCount = dt.Rows.Count;

            string Query = $"insert into UserTable values ({KeyCount}, '{FirstName}', '{LastName}', '{Username}', '{Password}', {1})";
            Connection.SetData(Query);
            SetToDefault();
        }

        private void SetToDefault()
        {
            textBoxFirstNameRegister.Text = "";
            textBoxLastNameRegister.Text = "";
            textBoxUsernameRegister.Text = "";
            textBoxPasswordRegister.Text = "";
            KeyCount = 0;
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            if(textBoxUsernameLogin.Text == "" || textBoxPasswordLogin.Text == "")
            {
                MessageBox.Show("Missing Information!", "Warning");
            }
            else
            {
                string Username = textBoxUsernameLogin.Text;
                string Password = textBoxPasswordLogin.Text;
                string Query = $"select * from UserTable where Username = '{Username}' and Password = '{Password}'";
                DataTable table = Connection.GetData(Query);

                if(table.Rows.Count == 0)
                {
                    labelLoginUsernameOrPasswordError.Visible = true;
                    textBoxPasswordLogin.Text = "";
                }
                else
                {
                    labelLoginUsernameOrPasswordError.Visible = false;
                    string firstName = table.Rows[0][1].ToString();
                    string lastName = table.Rows[0][2].ToString();

                    if(Convert.ToInt32(table.Rows[0][5].ToString()) == 1)
                    {
                        DialogResult askIfTutorial = MessageBox.Show("Would you like to go through a tutorial first?", "Greetings", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if(askIfTutorial == DialogResult.Yes)
                        {
                            Guide_Form guide = new Guide_Form(firstName, lastName, Username);
                            guide.Show();
                            this.Hide();
                        }
                    }
                    else
                    {
                        Activity_Form activity = new Activity_Form(firstName, lastName, Username);
                        activity.Show();
                        this.Hide();
                    }

                    string QueryUpdate = $"update UserTable set NewUser = {0} where Username = '{Username}' and Password = '{Password}'";
                    Connection.SetData(QueryUpdate);
                }
            }
        }

        private void textBoxUsernameLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                buttonLogin_Click(sender, e);

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void textBoxPasswordLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonLogin_Click(sender, e);

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                FormMover.ReleaseCapture();
                FormMover.SendMessage(Handle, FormMover.WM_NCLBUTTONDOWN, FormMover.HT_CAPTION, 0);
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                FormMover.ReleaseCapture();
                FormMover.SendMessage(Handle, FormMover.WM_NCLBUTTONDOWN, FormMover.HT_CAPTION, 0);
            }
        }
    }
}
