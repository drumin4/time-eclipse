using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProductivityTracker
{
    public partial class Category_Form : Form
    {
        GlobalConfig Connection;
        bool FormStarted = true;
        static private string FirstName;
        static private string LastName;
        static private string Username;

        public Category_Form(string firstname, string lastname, string username)
        {
            InitializeComponent();
            Connection = new GlobalConfig();
            FirstName = firstname;
            LastName = lastname;
            Username = username;

            GetCategories();
            buttonToAddActivity.Enabled = false;
        }

        public void InsertInImageTable(string fileName, string filePath, byte[] image)
        {
            using (SqlConnection cn = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=TimeEclipseDb;Integrated Security=True;TrustServerCertificate=True"))
            {
                if(cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                }
                
                using (SqlCommand cmd = new SqlCommand("insert into ImageTable(ImageName, ImagePath, Image, Username) values (@imagename, @imagepath, @image, @username)", cn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@imagename", fileName);
                    cmd.Parameters.AddWithValue("@imagepath", filePath);
                    cmd.Parameters.AddWithValue("@image", image);
                    cmd.Parameters.AddWithValue("@username", Username);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public byte[] ConvertImageToBytes(Image img)
        {   
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        } 

        private void buttonSearchImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "Image files(*.png)|*.png", Multiselect = false })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    pictureBoxAddedActivityImage.Image = Image.FromFile(ofd.FileName);
                    textBoxImagePath.Text = ofd.FileName;
                }
            }
        }

        private void GetCategories()
        {
            string Query = $"select * from dbo.ImageTable where Username = '{Username}' order by ImageName";
            comboBoxActivities.DataSource = Connection.GetData(Query);
            comboBoxActivities.DisplayMember = Connection.GetData(Query).Columns["ImageName"].ToString();
            comboBoxActivities.ValueMember = Connection.GetData(Query).Columns["ImagePath"].ToString();
            FormStarted = true;
            comboBoxActivities.SelectedIndex = -1;
            pictureBoxActivityImage.Image = null;
        }

        private void comboBoxActivities_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(FormStarted == false)
            {
                pictureBoxActivityImage.Image = Image.FromFile(comboBoxActivities.SelectedValue.ToString());
                pictureBoxActivityImage.BackgroundImageLayout = ImageLayout.Zoom;
            }
            
            else
            {
                FormStarted = false;
            }
        }

        private void ResetCreateActivity()
        {
            textBoxImageName.Text = "";
            textBoxImagePath.Text = "";
            pictureBoxAddedActivityImage.Image = null;
        }

        private void buttonCreateActivity_Click(object sender, EventArgs e)
        {
            if(textBoxImagePath.Text == "" || textBoxImageName.Text == "")
            {
                MessageBox.Show("Missing Information. Please try again.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string QueryFindData = $"SELECT TOP 1 ImagePath FROM ImageTable WHERE ImageName = '{textBoxImageName.Text}' and ImagePath = '{textBoxImagePath.Text}' and Username = '{Username}'";
                DataTable dataTable = Connection.GetData(QueryFindData);
                if (dataTable.Rows.Count == 0)
                {
                    InsertInImageTable(textBoxImageName.Text, textBoxImagePath.Text, ConvertImageToBytes(pictureBoxAddedActivityImage.Image));
                    ResetCreateActivity();
                    GetCategories();

                    MessageBox.Show("Activity has been created!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("The Activity has already been created!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void buttonSaveActivity_Click(object sender, EventArgs e)
        {
            if (comboBoxActivities.Text == "" || comboBoxSeconds.Text == "" || comboBoxMinutes.Text == "" || comboBoxHours.Text == "")
            {
                MessageBox.Show("Missing Information!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string categoryName = comboBoxActivities.Text;
                string categoryPath = comboBoxActivities.SelectedValue.ToString();
                string hours = comboBoxHours.Text;
                string minutes = comboBoxMinutes.Text;
                string seconds = comboBoxSeconds.Text;
                string categoryTargetTime = $"{hours}:{minutes}:{seconds}";

                DataTable checkCategoryTable = Connection.GetData($"select * from CategoryTable where CategoryName = '{categoryName}' and CategoryPath = '{categoryPath}' and Username = '{Username}'");

                if (checkCategoryTable.Rows.Count == 0)
                {
                    InsertInCategoryTable(categoryName, categoryPath, categoryTargetTime, ConvertImageToBytes(pictureBoxActivityImage.Image)); //maybe Image has to be changed back to BackgroundImage
                    ResetSaveActivity();
                    MessageBox.Show("The Activity has been added!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("The Activity already exists!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void ResetSaveActivity()
        {
            GetCategories();
            
            pictureBoxActivityImage.BackgroundImage = null;
            FormStarted = true;
            comboBoxActivities.SelectedIndex = -1;
            comboBoxHours.SelectedIndex = -1;
            comboBoxMinutes.SelectedIndex = -1;
            comboBoxSeconds.SelectedIndex = -1;
        }

        public void InsertInCategoryTable(string fileName, string filePath, string categoryTargetTime, byte[] image)
        {
            using (SqlConnection cn = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=TimeEclipseDb;Integrated Security=True;TrustServerCertificate=True"))
            {
                if (cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                }

                using (SqlCommand cmd = new SqlCommand("insert into CategoryTable(CategoryName, CategoryPath, CategoryStarted, CategoryTargetTime, Username, Image) values (@categoryname, @categorypath, @categorystarted, @categorytargettime, @username, @image)", cn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@categoryname", fileName);
                    cmd.Parameters.AddWithValue("@categorypath", filePath);
                    cmd.Parameters.AddWithValue("@categorystarted", "false");
                    cmd.Parameters.AddWithValue("@categorytargettime", categoryTargetTime);
                    cmd.Parameters.AddWithValue("@username", Username);
                    cmd.Parameters.AddWithValue("@image", image);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void textBoxImageName_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                buttonCreateActivity_Click(sender, e);
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

        private void pictureBox3_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                FormMover.ReleaseCapture();
                FormMover.SendMessage(Handle, FormMover.WM_NCLBUTTONDOWN, FormMover.HT_CAPTION, 0);
            }
        }

        private void buttonToTracker_Click(object sender, EventArgs e)
        {
            ResetSaveActivity();
            ResetCreateActivity();
            Activity_Form activity = new Activity_Form(FirstName, LastName, Username);
            activity.Show();
            this.Hide();
        }

        private void buttonToTracker_MouseEnter(object sender, EventArgs e)
        {
            buttonToTracker.BackColor = Color.Sienna;
        }

        private void buttonToTracker_MouseLeave(object sender, EventArgs e)
        {
            buttonToTracker.BackColor = Color.Transparent;
        }

        private void buttonToTracker_MouseDown(object sender, MouseEventArgs e)
        {
            buttonToTracker.BackColor = Color.SaddleBrown;
        }

        private void buttonToTracker_MouseUp(object sender, MouseEventArgs e)
        {
            buttonToTracker.BackColor = Color.Sienna;
        }

        private void buttonToGuide_Click(object sender, EventArgs e)
        {
            Guide_Form guide = new Guide_Form(FirstName, LastName, Username);
            guide.Show();
            this.Hide();
        }

        private void buttonToGuide_MouseEnter(object sender, EventArgs e)
        {
            buttonToGuide.BackColor = Color.Sienna;
        }

        private void buttonToGuide_MouseLeave(object sender, EventArgs e)
        {
            buttonToGuide.BackColor = Color.Transparent;
        }

        private void buttonToGuide_MouseDown(object sender, MouseEventArgs e)
        {
            buttonToGuide.BackColor = Color.SaddleBrown;
        }

        private void buttonToGuide_MouseUp(object sender, MouseEventArgs e)
        {
            buttonToGuide.BackColor = Color.Sienna;
        }

        private void buttonToRecords_Click(object sender, EventArgs e)
        {
            Record_Form record = new Record_Form(FirstName, LastName, Username);
            record.Show();
            this.Hide();
        }

        private void buttonToRecords_MouseEnter(object sender, EventArgs e)
        {
            buttonToRecords.BackColor = Color.Sienna;
        }

        private void buttonToRecords_MouseLeave(object sender, EventArgs e)
        {
            buttonToRecords.BackColor = Color.Transparent;
        }

        private void buttonToRecords_MouseDown(object sender, MouseEventArgs e)
        {
            buttonToRecords.BackColor = Color.SaddleBrown;
        }

        private void buttonToRecords_MouseUp(object sender, MouseEventArgs e)
        {
            buttonToRecords.BackColor = Color.Sienna;
        }

        private void buttonToSignOut_Click(object sender, EventArgs e)
        {
            LoginOrRegister_Form login = new LoginOrRegister_Form();
            login.Show();
            this.Hide();
        }

        private void buttonToSignOut_MouseEnter(object sender, EventArgs e)
        {
            buttonToSignOut.BackColor = Color.Sienna;
        }

        private void buttonToSignOut_MouseLeave(object sender, EventArgs e)
        {
            buttonToSignOut.BackColor = Color.Transparent;
        }

        private void buttonToSignOut_MouseDown(object sender, MouseEventArgs e)
        {
            buttonToSignOut.BackColor = Color.SaddleBrown;
        }

        private void buttonToSignOut_MouseUp(object sender, MouseEventArgs e)
        {
            buttonToSignOut.BackColor = Color.Sienna;
        }
    }
}
