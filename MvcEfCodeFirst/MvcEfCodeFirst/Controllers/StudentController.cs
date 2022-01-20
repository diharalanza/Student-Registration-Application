using MvcEfCodeFirst.Models;
using MvcEfCodeFirst.Report;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcEfCodeFirst.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index()
        {
            using (var context = new StudentContext())
            {
                List<StudentModel> StudentList = context.Students.ToList();
                return View(StudentList);
            }
        }


        public ActionResult Add()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Add(StudentModel student)
        {
            using (StudentContext context = new StudentContext())
            {
                StudentModel newStudent = new StudentModel()
                {
                    full_name = student.full_name,
                    phone_number = student.phone_number,
                    stu_address = student.stu_address,
                    birthday = student.birthday,
                    join_date = student.join_date,
                    gpa = student.gpa
                };

                context.Students.Add(newStudent);
                context.SaveChanges();


                return RedirectToAction("Index");
            }
        }

        public ActionResult Edit(int id)
        {
            using (StudentContext context = new StudentContext())
            {
                StudentModel found = context.Students.Find(id);
                return View(found);

            }
            
        }


        [HttpPost]
        public ActionResult Edit(StudentModel student)
        {
            if (ModelState.IsValid)
            {
                using (StudentContext context = new StudentContext())
                {
                    StudentModel found = context.Students.Find(student.stu_id);

                    found.full_name = student.full_name;
                    found.phone_number = student.phone_number;
                    found.stu_address = student.stu_address;
                    found.birthday = student.birthday;
                    found.join_date = student.join_date;
                    found.gpa = student.gpa;

                    context.SaveChanges();
                };   
            }
          return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {

            using (StudentContext context = new StudentContext())
            {
                StudentModel found = context.Students.Find(id);

                context.Students.Remove(found);

                context.SaveChanges();
            };

            return RedirectToAction("Index");
        }

        public ActionResult FullReport()
        {
            using (var context = new StudentContext())
            {
                List<StudentModel> StudentList = context.Students.ToList();

                StudentReport rd = new StudentReport();

                rd.SetDataSource(ListToDataTable(StudentList));
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

        public ActionResult JoinReport()
        {
            return View();
        }

        [HttpPost]
        public ActionResult JoinReport(DateTime JoinDate1, DateTime JoinDate2)
        {
            using (var context = new StudentContext())
            {
                List<StudentModel> StudentList = context.Students.Where(x => x.join_date >= JoinDate1 && x.join_date <= JoinDate2).ToList();

                StudentReport rd = new StudentReport();

                rd.SetDataSource(ListToDataTable(StudentList));
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
    }
}