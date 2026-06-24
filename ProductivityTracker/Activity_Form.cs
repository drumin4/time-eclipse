using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ProductivityTracker
{
    public partial class Activity_Form : Form
    {
        GlobalConfig Connection;
        private bool FormStarted = true;
        private string FirstName;
        private string LastName;
        private string Username;
        private int seconds = 0;
        private int minutes = 0;
        private int hours = 0;
        private bool timerStarted = false;
        private int currentIndex = 0;
        readonly PrivateFontCollection pfc = new PrivateFontCollection();
        
        public Activity_Form(string firstname, string lastname, string username)
        {
            FirstName = firstname;
            LastName = lastname;
            Username = username;
            InitializeComponent();
            Connection = new GlobalConfig();
            labelFullName.Text = FirstName + " " + LastName;
            
            SetWatchFont();

            GetCategories();
            GetActivities();
        }
        
        private void SetWatchFont()
        {
            pfc.AddFontFile(@"C:\Users\USER\source\repos\drumin4\time-eclipse\ProductivityTracker\font\LCDMono2 Bold.ttf");
            labelWatch.Font = new Font(pfc.Families[0], 35, FontStyle.Bold);
        }

        private void GetCategories()
        {
            string Query = $"select * from dbo.CategoryTable where Username = '{Username}' order by CategoryName";
            comboBoxActivities.DisplayMember = Connection.GetData(Query).Columns["CategoryName"].ToString();
            comboBoxActivities.ValueMember = Connection.GetData(Query).Columns["CategoryPath"].ToString();
            comboBoxActivities.DataSource = Connection.GetData(Query);
        }

        private void GetActivities()
        {
            string Query = $"select ActivityId, ActivityName, DateStarted, CurrentDate, TimeSpent, TargetTime, Status from ActivityTable where Username = '{Username}'";
            listActivities.DataSource = Connection.GetData(Query);
        }

        private void GetActivities(int index)
        {
            string Query = $"select * from ActivityTable where Username = '{Username}'";
            listActivities.DataSource = Connection.GetData(Query);
            listActivities.Columns[7].Visible = false;
            listActivities.Rows[index - 1].Selected = true;
            listActivities.Rows[0].Selected = false;
        }

        private void comboBoxActivities_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FormStarted == false)
            {
                pictureBoxActivityImage.BackgroundImage = Image.FromFile(comboBoxActivities.SelectedValue.ToString());
                pictureBoxActivityImage.BackgroundImageLayout = ImageLayout.Zoom;
            }

            else
            {
                comboBoxActivities.SelectedIndex = -1;
                pictureBoxActivityImage.Image = null;
                FormStarted = false;
            }
        }

        private void timerWatch_Tick(object sender, EventArgs e)
        {
            SetWatchFont();

            if (timerStarted)
            {
                seconds = Convert.ToInt32(labelWatch.Text.Substring(6, 2));
                seconds++;

                if (seconds >= 60)
                {
                    minutes = Convert.ToInt32(labelWatch.Text.Substring(3, 2));
                    minutes++;
                    seconds = 0;

                    if (minutes >= 60)
                    {
                        hours = Convert.ToInt32(labelWatch.Text.Substring(0, 2));
                        hours++;
                        minutes = 0;
                    }
                }
            }

            UpdateTimer();
        }

        private void UpdateTimer()
        {
            labelWatch.Text = $"{String.Format("{0:00}",hours)}:{String.Format("{0:00}", minutes)}:{String.Format("{0:00}", seconds)}";
        }

        private void SetTimerToDefault()
        {
            seconds = 0;
            minutes = 0;
            hours = 0;
        }

        private void buttonStartWatch_Click(object sender, EventArgs e)
        {
            SetWatchFont();

            try
            {
                if (listActivities.SelectedRows[0].Cells[6].Value.ToString() == "Finished")
                {
                    MessageBox.Show("You have already finished this activity. Further changes can not be done.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (listActivities.SelectedRows[0].Cells[6].Value.ToString() != "Ongoing")
                {
                    timerStarted = true;
                    timerWatch.Start();
                    string Status = "Ongoing";
                    string ActivityName = listActivities.SelectedRows[0].Cells[1].Value.ToString();
                    int Index = Convert.ToInt32(listActivities.SelectedRows[0].Cells[0].Value.ToString());
                    

                    string Query = $"update ActivityTable set Status = '{Status}' where Username = '{Username}' and ActivityName = '{ActivityName}'";
                    Connection.SetData(Query);
                    GetActivities(Index);
                    listActivities.Rows[Index - 1].Selected = true;
                }
            }
            catch(Exception ex)
            {

            }
        }

        private void buttonPauseWatch_Click(object sender, EventArgs e)
        {
            SetWatchFont();

            try
            {
                if (listActivities.SelectedRows[0].Cells[6].Value.ToString() == "Finished")
                {
                    MessageBox.Show("You have already finished this activity. Further changes can not be done.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (listActivities.SelectedRows[0].Cells[6].Value.ToString() == "Unstarted")
                {
                    MessageBox.Show("You can't pause an unstarted activity.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (listActivities.SelectedRows[0].Cells[6].Value.ToString() != "Paused")
                {

                    timerStarted = false;
                    string Status = "Paused";
                    string timeWhenPaused = labelWatch.Text;
                    string ActivityName = listActivities.SelectedRows[0].Cells[1].Value.ToString();
                    int Index = Convert.ToInt32(listActivities.SelectedRows[0].Cells[0].Value.ToString());
                    

                    string Query = $"update ActivityTable set TimeSpent = '{timeWhenPaused}', Status = '{Status}' where Username = '{Username}' and ActivityName = '{ActivityName}'";
                    Connection.SetData(Query);
                    GetActivities(Index);
                    listActivities.Rows[Index - 1].Selected = true;
                }
            }
            catch(Exception ex)
            {

            }
        }
        
        private void buttonStopWatch_Click(object sender, EventArgs e)
        {
            SetWatchFont();

            try
            {
                if (listActivities.SelectedRows[0].Cells[6].Value.ToString() != "Finished")
                {
                    timerStarted = false;
                    string Status = "Finished";
                    string timeWhenStopped = labelWatch.Text;
                    string ActivityName = listActivities.SelectedRows[0].Cells[1].Value.ToString();
                    int Index = Convert.ToInt32(listActivities.SelectedRows[0].Cells[0].Value.ToString());

                    string Query = $"update ActivityTable set TimeSpent = '{timeWhenStopped}', Status = '{Status}' where Username = '{Username}' and ActivityName = '{ActivityName}'";
                    Connection.SetData(Query);
                    GetActivities(Index);
                    listActivities.Rows[Index - 1].Selected = true;

                    timerWatch.Stop();
                    SetTimerToDefault();
                    UpdateTimer();
                }
            }
            catch(Exception ex)
            {

            }
            
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            try
            {
                if (listActivities.SelectedRows[0].Cells[6].Value.ToString() == "Finished")
                {
                    MessageBox.Show("You have already finished this activity. Further changes can not be done.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (listActivities.SelectedRows[0].Cells[6].Value.ToString() != "Unstarted")
                {
                    timerStarted = false;
                    timerWatch.Stop();
                    SetTimerToDefault();
                    UpdateTimer();

                    int Index = Convert.ToInt32(listActivities.SelectedRows[0].Cells[0].Value.ToString());
                    string Status = "Unstarted";
                    string ActivityName = listActivities.SelectedRows[0].Cells[1].Value.ToString();

                    string Query = $"update ActivityTable set TimeSpent = '{"00:00:00"}', Status = '{Status}' where Username = '{Username}' and ActivityName = '{ActivityName}'";
                    Connection.SetData(Query);
                    GetActivities(Index);
                    listActivities.Rows[Index - 1].Selected = true;
                }
            }
            catch(Exception ex)
            {

            }
        }

        private void buttonAddActivity_Click(object sender, EventArgs e)
        {
            string datePerformed = DateTime.Now.Date.ToString("yyyy/MM/dd");
            DataTable checkerTable = Connection.GetData($"select * from RecordTable where RecordName = '{comboBoxActivities.Text}' and DatePerformed = '{datePerformed}'");

            if (comboBoxActivities.Text == "")
            {
                MessageBox.Show("No activity was selected.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if(checkerTable.Rows.Count != 0)
            {
                MessageBox.Show("The activity has already been added", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string Query = $"select CategoryStarted, CategoryTargetTime from CategoryTable where CategoryName = '{comboBoxActivities.Text}' and Username = '{Username}'";
                DataTable dt = Connection.GetData(Query);
                string checkIfActivityAlreadyStarted = dt.Rows[0][0].ToString();

                int ActivityId = listActivities.Rows.Count;
                string ActivityName = comboBoxActivities.Text;
                string DateStarted;
                if (checkIfActivityAlreadyStarted == "false")
                {
                    DateStarted = DateTime.Today.Date.ToString("yyyy/MM/dd");
                    checkIfActivityAlreadyStarted = "true";
                    string QueryUpdate = $"update CategoryTable set CategoryStarted = '{checkIfActivityAlreadyStarted}', DateStarted = '{DateStarted}' where CategoryName = '{ActivityName}' and Username = '{Username}'";
                    Connection.SetData(QueryUpdate);
                }
                else
                {
                    DataTable table = Connection.GetData($"select DateStarted from CategoryTable where CategoryName = '{ActivityName}' and Username = '{Username}'");
                    DateStarted = table.Rows[0][0].ToString();
                }
                string CurrentDate = DateTime.Today.Date.ToString("yyyy/MM/dd");
                string TimeSpent = "00:00:00";
                string TargetTime = dt.Rows[0][1].ToString();
                string Status = "Unstarted";

                Connection.SetData($"insert into ActivityTable values({ActivityId}, '{ActivityName}', '{DateStarted}', '{CurrentDate}', '{TimeSpent}', '{TargetTime}', '{Status}', '{Username}')");
                GetActivities();
                ReorderingRows();
                GetActivities();
            }
        }

        private bool rowSelected(DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != listActivities.Rows.Count - 1 && e.RowIndex != -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void listActivities_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            bool isRow = rowSelected(e);

            if (isRow)
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this activity?", "Delete activity", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    string ActivityName = listActivities.SelectedRows[0].Cells[1].Value.ToString();
                    string Query = $"Delete from ActivityTable where Username = '{Username}' and ActivityName = '{ActivityName}'";
                    Connection.SetData(Query);
                    Connection.SetData($"Update CategoryTable set CategoryStarted = '{"false"}' where Username = '{Username}' and CategoryName = '{ActivityName}'");
                    GetActivities();
                    ReorderingRows();
                    GetActivities();
                }
            }
        }

        private void ReorderingRows()
        {
            int i = 0;

            while (i < listActivities.Rows.Count - 1)
            {
                string ActivityName = listActivities.Rows[i].Cells[1].Value.ToString();              
                i++;
                string Query = $"update ActivityTable set ActivityId = {i} where Username = '{Username}' and ActivityName = '{ActivityName}'";
                Connection.SetData(Query);
            }
        }

        private void listActivities_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            bool isValidRow = rowSelected(e);
            if (listActivities.Rows[currentIndex].Cells[6].Value.ToString() != "Ongoing" && isValidRow)
            {
                currentIndex = e.RowIndex;
                labelWatch.Text = listActivities.SelectedRows[0].Cells[4].Value.ToString();
                SetWatchFont();
                seconds = Convert.ToInt32(labelWatch.Text.Substring(6, 2));
                minutes = Convert.ToInt32(labelWatch.Text.Substring(3, 2));
                hours = Convert.ToInt32(labelWatch.Text.Substring(0, 2));
            }
            else if (isValidRow)
            {
                listActivities.CurrentRow.Selected = false;
                listActivities.Rows[currentIndex].Selected = true;
                MessageBox.Show("Can't select another activity while the current one is still ongoing.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

        private void listActivities_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            listActivities.ClearSelection();
        }
        
        private void buttonToAddActivity_Click(object sender, EventArgs e)
        {
            if (listActivities.Rows.Count > 1 && listActivities.SelectedRows.Count == 1)
            {
                if (listActivities.SelectedRows[0].Cells[6].Value.ToString() != "Ongoing")
                {
                    Category_Form category = new Category_Form(FirstName, LastName, Username);
                    category.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Please pause or stop all your started activities before changing tabs.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                Category_Form category = new Category_Form(FirstName, LastName, Username);
                category.Show();
                this.Hide();
            }
        }

        private void buttonToAddActivity_MouseEnter(object sender, EventArgs e)
        {
            buttonToAddActivity.BackColor = Color.Sienna;
        }

        private void buttonToAddActivity_MouseLeave(object sender, EventArgs e)
        {
            buttonToAddActivity.BackColor = Color.Transparent;
        }

        private void buttonToAddActivity_MouseDown(object sender, MouseEventArgs e)
        {
            buttonToAddActivity.BackColor = Color.SaddleBrown;
        }

        private void buttonToAddActivity_MouseUp(object sender, MouseEventArgs e)
        {
            buttonToAddActivity.BackColor = Color.Sienna;
        }

        private void buttonToGuide_Click(object sender, EventArgs e)
        {
            if (listActivities.Rows.Count > 1 && listActivities.SelectedRows.Count == 1)
            {
                if (listActivities.SelectedRows[0].Cells[6].Value.ToString() != "Ongoing")
                {
                    Guide_Form guide = new Guide_Form(FirstName, LastName, Username);
                    guide.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Please pause or stop all your started activities before changing tabs.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                Guide_Form guide = new Guide_Form(FirstName, LastName, Username);
                guide.Show();
                this.Hide();
            }
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
            if (listActivities.Rows.Count > 1 && listActivities.SelectedRows.Count == 1)
            {
                if (listActivities.SelectedRows[0].Cells[6].Value.ToString() != "Ongoing")
                {
                    Record_Form record = new Record_Form(FirstName, LastName, Username);
                    record.Show();
                    this.Hide();
                }
                
                else
                {
                    MessageBox.Show("Please pause or stop all your started activities before changing tabs.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                Record_Form record = new Record_Form(FirstName, LastName, Username);
                record.Show();
                this.Hide();
            }
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
            if(listActivities.Rows.Count > 1 && listActivities.SelectedRows.Count == 1)
            {
                if (listActivities.SelectedRows[0].Cells[6].Value.ToString() != "Ongoing")
                {
                    LoginOrRegister_Form login = new LoginOrRegister_Form();
                    login.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Please pause or stop all your started activities before signing out.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                LoginOrRegister_Form login = new LoginOrRegister_Form();
                login.Show();
                this.Hide();
            }
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

        private bool CheckIfActivityAlreadyRecorded()
        {
            string datePerformed = listActivities.Rows[0].Cells[3].Value.ToString();

            for (int i = 0; i < listActivities.Rows.Count - 1; i++)
            {
                string activityName = listActivities.Rows[i].Cells[1].Value.ToString();

                DataTable tableChecker = Connection.GetData($"Select RecordName from RecordTable where RecordName = '{activityName}' and DatePerformed = '{datePerformed}' and Username = '{Username}'");
                if(tableChecker.Rows.Count > 0)
                {
                    MessageBox.Show("Some activities in your list have already been recorded for today. Please delete them or try again tomorrow.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            
            return true;
        }

        private void buttonFinish_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to finish all your activities for today?", "Confirmation Request", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(dialogResult == DialogResult.Yes && CheckIfActivityAlreadyRecorded())
            {
                string datePerformed = listActivities.Rows[0].Cells[3].Value.ToString();
                List<KeyValuePair<string, string>> listPerformance = new List<KeyValuePair<string, string>>();
                Dictionary<string, List<KeyValuePair<string, string>>> activityAndPerformance = new Dictionary<string, List<KeyValuePair<string, string>>>();
                
                for(int i = 0; i < listActivities.Rows.Count - 1; i++)
                {
                    string activityName = listActivities.Rows[i].Cells[1].Value.ToString();

                    listPerformance.Add(new KeyValuePair<string, string>(listActivities.Rows[i].Cells[4].Value.ToString(), listActivities.Rows[i].Cells[5].Value.ToString()));

                    activityAndPerformance.Add(activityName, listPerformance);
                    listPerformance = new List<KeyValuePair<string, string>>();
                }
                
                string[] dateStarted = new string[listActivities.Rows.Count - 1];
                for(int i = 0; i < listActivities.Rows.Count - 1; i++)
                {
                    dateStarted[i] = listActivities.Rows[i].Cells[2].Value.ToString();
                }

                Record_Form recordSet = new Record_Form(datePerformed, activityAndPerformance, dateStarted, Username, FirstName, LastName);
                MessageBox.Show("Your performance for today has been recorded. Well done!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Connection.SetData("Delete from ActivityTable");
                GetActivities();
                this.Hide();
                recordSet.Show();
            }
        }
    }
}
