using StudentProject.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using StudentProject.Report;
using System.ComponentModel;

namespace StudentProject.Controllers
{
    public class StudentController : Controller
    {

        string connectionString = "Data Source=.;Initial Catalog=BasicLayeredDBnew;Integrated Security=True";

        // GET: Student
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {

            List<StudentModel> students = new List<StudentModel>();

            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetAllStudents", myConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    myConnection.Open();

                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            var student = new StudentModel();

                            //TODO: Check null.
                            student.stu_id = Convert.ToInt32(dataReader["stu_id"]);
                            student.full_name = Convert.ToString(dataReader["full_name"]);
                            student.phone_number = Convert.ToString(dataReader["phone_number"]);
                            student.stu_address = Convert.ToString(dataReader["stu_address"]);
                            student.birthday = Convert.ToDateTime(dataReader["birthday"]);
                            student.join_date = Convert.ToDateTime(dataReader["join_date"]);
                            student.gpa = Convert.ToSingle(dataReader["gpa"]);

                            students.Add(student);
                        }
                    }
                }
            }

            return View(students);
        }

        public ActionResult Delete(int? id)
        {
            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("DeleteStudent", myConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@stu_id", id);
                    myConnection.Open();
                    cmd.ExecuteNonQuery();
                }

                return RedirectToAction("List");

            }
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(StudentModel student)
        {

            if (ModelState.IsValid)
            {

                using (SqlConnection myConnection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("InsertStudent", myConnection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@full_name", student.full_name);
                        cmd.Parameters.AddWithValue("@phone_number", student.phone_number);
                        cmd.Parameters.AddWithValue("@stu_address", student.stu_address);
                        cmd.Parameters.AddWithValue("@birthday", student.birthday);
                        cmd.Parameters.AddWithValue("@join_date", student.join_date);
                        cmd.Parameters.AddWithValue("@gpa", student.gpa);
                        myConnection.Open();
                        cmd.ExecuteNonQuery();
                    }

                    return RedirectToAction("List");

                }
            }

            return View();

        }

        public ActionResult Edit(int id)
        {

            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetStudentByID", myConnection))
                {
                    cmd.Parameters.AddWithValue("@stu_id", id);
                    cmd.CommandType = CommandType.StoredProcedure;
                    myConnection.Open();

                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            var student = new StudentModel();

                            //TODO: Check null.
                            student.stu_id = Convert.ToInt32(dataReader["stu_id"]);
                            student.full_name = Convert.ToString(dataReader["full_name"]);
                            student.phone_number = Convert.ToString(dataReader["phone_number"]);
                            student.stu_address = Convert.ToString(dataReader["stu_address"]);
                            student.birthday = Convert.ToDateTime(dataReader["birthday"]);
                            student.join_date = Convert.ToDateTime(dataReader["join_date"]);
                            student.gpa = Convert.ToSingle(dataReader["gpa"]);

                            return View(student);
                        }
                    }
                }
            }

            return null;
        }



        [HttpPost]
        public ActionResult Edit(StudentModel student, int id)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection myConnection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("UpdateStudent", myConnection))
                    {
                        cmd.Parameters.AddWithValue("@stu_id", id);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@full_name", student.full_name);
                        cmd.Parameters.AddWithValue("@phone_number", student.phone_number);
                        cmd.Parameters.AddWithValue("@stu_address", student.stu_address);
                        cmd.Parameters.AddWithValue("@birthday", student.birthday);
                        cmd.Parameters.AddWithValue("@join_date", student.join_date);
                        cmd.Parameters.AddWithValue("@gpa", student.gpa);
                        myConnection.Open();
                        cmd.ExecuteNonQuery();
                    }
                    return RedirectToAction("List");
                }
            }
            return View();
        }


        public ActionResult ExportStudents()
        {
            List<StudentModel> students = new List<StudentModel>();

            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetAllStudents", myConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    myConnection.Open();

                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            var student = new StudentModel();

                            //TODO: Check null.
                            student.stu_id = Convert.ToInt32(dataReader["stu_id"]);
                            student.full_name = Convert.ToString(dataReader["full_name"]);
                            student.phone_number = Convert.ToString(dataReader["phone_number"]);
                            student.stu_address = Convert.ToString(dataReader["stu_address"]);
                            student.birthday = Convert.ToDateTime(dataReader["birthday"]);
                            student.join_date = Convert.ToDateTime(dataReader["join_date"]);
                            student.gpa = Convert.ToSingle(dataReader["gpa"]);

                            students.Add(student);
                        }
                    }
                }
            }

            StudentReport rd = new StudentReport();

            rd.SetDataSource(ListToDataTable(students));
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "StudentList.pdf");
            }
            catch
            {
                throw;
            }
        }


        private DataTable ListToDataTable<T>(List<T> items)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in items)
            {
                for (int i = 0; i < values.Length; i++)
                    values[i] = props[i].GetValue(item) ?? DBNull.Value;
                table.Rows.Add(values);
            }
            return table;
        }

        public ActionResult JoinReport()
        {
            return View();
        }

        [HttpPost]
        public ActionResult JoinReport(DateTime JoinDate1, DateTime JoinDate2)
        {

            List<StudentModel> students = new List<StudentModel>();

            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetStuFromJoin", myConnection))
                {
                    cmd.Parameters.AddWithValue("@from", JoinDate1);
                    cmd.Parameters.AddWithValue("@to", JoinDate2);

                    cmd.CommandType = CommandType.StoredProcedure;
                    myConnection.Open();

                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            var student = new StudentModel();

                            //TODO: Check null.
                            student.stu_id = Convert.ToInt32(dataReader["stu_id"]);
                            student.full_name = Convert.ToString(dataReader["full_name"]);
                            student.phone_number = Convert.ToString(dataReader["phone_number"]);
                            student.stu_address = Convert.ToString(dataReader["stu_address"]);
                            student.birthday = Convert.ToDateTime(dataReader["birthday"]);
                            student.join_date = Convert.ToDateTime(dataReader["join_date"]);
                            student.gpa = Convert.ToSingle(dataReader["gpa"]);

                            students.Add(student);
                        }
                    }
                }
            }

            StudentReport rd = new StudentReport();

            rd.SetDataSource(ListToDataTable(students));
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "StudentList.pdf");
            }
            catch
            {
                throw;
            }

        }


    }



}