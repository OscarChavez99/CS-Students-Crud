using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CS_Students_Crud
{
    public partial class Form1 : Form
    {
        private ClStudent obj_student;
        private List <ClStudent> list_students;
        //For SQL Database
        private ClSqlStudent obj_sql_student;
        public Form1()
        {
            InitializeComponent();
            obj_student     = new ClStudent();
            list_students   = new List <ClStudent>();
            //Sql Database
            obj_sql_student = new ClSqlStudent();
            LoadDataGrid(txtSearch.Text);
        }

        //-----------Functions---------------

        private bool FullData()
        {
            if(txtName.Text == "" || txtMail.Text == "")
            {
                MessageBox.Show("Incomplete data", "Error");
                return false;
            }
            return true;
        }

        private ClStudent CreateObject()
        {
            if (txtID.Text != "")
                obj_student.id      = int.Parse(txtID.Text);
            obj_student.name    = txtName.Text;
            obj_student.mail    = txtMail.Text;
            obj_student.picture = ImageToByteArray(pictureBoxStudent.Image);
            obj_student.comment = txtComment.Text;

            return obj_student;
        }

        private byte[] ImageToByteArray(Image image)
        {
            if (image == null)
                return null;
            MemoryStream mMemoryStream = new MemoryStream();
            image.Save(mMemoryStream, ImageFormat.Png);
            return mMemoryStream.ToArray();
        }

        private void Clear()
        {
            txtID.Text      = ""; 
            txtName.Text    = "";
            txtMail.Text    = "";
            txtSearch.Text  = "";
            txtComment.Text = "";

            pictureBoxStudent.Image = Properties.Resources.icon;
            txtAddImage.Text = "Click->\nto add\npicture";
        }

        //------------Events---------------
        private void LoadDataGrid(string filter)
        {
            dgvStudents.Rows.Clear();
            dgvStudents.Refresh();
            list_students.Clear();
            list_students = obj_sql_student.SelectAllStudents(filter);

            for (int i = 0; i<list_students.Count(); i++)
            {
                dgvStudents.RowTemplate.Height = 100;
                dgvStudents.Rows.Add(
                    list_students[i].id,
                    list_students[i].name,
                    list_students[i].mail,
                    Image.FromStream(new MemoryStream(list_students[i].picture)),
                    list_students[i].comment
                );
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadDataGrid(txtSearch.Text);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(txtID.Text != "")
            {
                MessageBox.Show("Student already exist, use UPDATE button", "Error");
                return;
            }

            if (!FullData())
                return;

            obj_student = CreateObject();
            if (obj_sql_student.InsertStudent(obj_student))
            {
                MessageBox.Show("Student Added!", "Success");
                Clear();
                LoadDataGrid(txtSearch.Text);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!FullData())
                return;

            if (txtID.Text == "")
            {
                MessageBox.Show("Student doesn't exist, use SAVE button", "Error");
                return;
            }

            obj_student = CreateObject();
            if (obj_sql_student.UpdateStudent(obj_student))
                MessageBox.Show("Student Updated!", "Success");

            Clear();
            LoadDataGrid(txtSearch.Text);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtID.Text == "")
            {
                MessageBox.Show("Select a student to delete", "Error");
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete '" + txtName.Text + "'?",
                "Delete product", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int id = int.Parse(txtID.Text);
                if (obj_sql_student.DeleteStudent(id))
                {
                    MessageBox.Show("Student deleted", "Success");
                    Clear();
                    LoadDataGrid(txtSearch.Text);
                }
            }
        }

        private void dgvStudents_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // If the user DON'T click the headers... 
            {
                DataGridViewRow row = dgvStudents.Rows[e.RowIndex];
                txtID.Text = Convert.ToString(row.Cells[0].Value);
                txtName.Text = Convert.ToString(row.Cells["NameColumn"].Value);
                txtMail.Text = Convert.ToString(row.Cells["MailColumn"].Value);

                MemoryStream memoryStream = new MemoryStream();
                Bitmap bitmap = (Bitmap)dgvStudents.CurrentRow.Cells["PictureColumn"].Value;
                bitmap.Save(memoryStream, ImageFormat.Png);
                pictureBoxStudent.Image = Image.FromStream(memoryStream);

                txtComment.Text = Convert.ToString(row.Cells["CommentColumn"].Value);
                txtAddImage.Text = "Image:";
            }
        }

        private void pictureBoxStudent_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog(); //Open file explorer

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBoxStudent.ImageLocation = openFileDialog.FileName;
                txtAddImage.Text = "Image:";
            }
        }
    }
}
