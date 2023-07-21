using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS_Students_Crud
{
    internal class ClSqlStudent
    {
        private ClMySqlCon con;
        private List <ClStudent> list_students;

        public ClSqlStudent()
        {
            con = new ClMySqlCon();
            list_students = new List<ClStudent>();
        }


        internal byte[] GetPictureToBytes(MySqlDataReader reader, int pictureColumnIndex)
        {
            long byteLength = reader.GetBytes(pictureColumnIndex, 0, null, 0, 0); //get size of bytes
            byte[] buffer = new byte[byteLength]; //Create byte array of previous size
            reader.GetBytes(pictureColumnIndex, 0, buffer, 0, (int)byteLength); //Fill the buffer array with the bytes

            return buffer;
        }        

        internal List<ClStudent> SelectAllStudents(string filter)
        {
            string QUERY = "SELECT * from students ";
            if (filter != "")
            {
                QUERY += "WHERE " +
                "id LIKE '%"   + filter + "%' OR " +
                "name LIKE '%" + filter + "%' OR " +
                "mail LIKE '%" + filter + "%'";
            }
            try
            {
                ClStudent student = null;
                MySqlDataReader reader = null;
                MySqlCommand command = new MySqlCommand(QUERY, con.GetConnection());
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    student = new ClStudent();
                    student.id      = reader.GetInt16("id");
                    student.name    = reader.GetString("name");
                    student.mail    = reader.GetString("mail");

                    int pictureColumnIndex = reader.GetOrdinal("picture"); //Get Index of picture column
                    if (!reader.IsDBNull(pictureColumnIndex)) //If exists a picture in database
                    {
                        student.picture = GetPictureToBytes(reader, pictureColumnIndex); //call function
                    }
                    student.comment = reader.GetString("comment");

                    list_students.Add(student);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can't load to Database, error:\n" + ex.ToString());
            }

            return list_students;
        }

        internal bool InsertStudent(ClStudent obj_student)
        {
            string QUERY = "INSERT INTO students (name, mail, picture, comment)" +
                "VALUES (@name, @mail, @picture, @comment)";
            try
            {
                MySqlCommand command = new MySqlCommand(QUERY, con.GetConnection());
                command.Parameters.Add(new MySqlParameter("@name", obj_student.name));
                command.Parameters.Add(new MySqlParameter("@mail", obj_student.mail));
                command.Parameters.Add(new MySqlParameter("@picture", obj_student.picture));
                command.Parameters.Add(new MySqlParameter("@comment", obj_student.comment));
                return command.ExecuteNonQuery() > 0;
            }
            catch (Exception)
            {
                MessageBox.Show("Non compatible image or,\nText cannot be longer than 100 chars", "Error");
                return false;
            }
            
        }
    }
}
