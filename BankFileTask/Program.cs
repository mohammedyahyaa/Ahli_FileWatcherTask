using FileTask;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace BankFileTask
{
    internal class Program
    {
        static void Main(string[] args)
        {



            if (File.Exists(@"D:\BankTask\Employe_list.csv"))
            {
                Console.WriteLine("File Founded !");
            }



            deletDbData();

            //AddExcelFileToDB();


            using var watcher = new FileSystemWatcher(@"D:\BankTask\");


            watcher.Changed += OnChanged;
            watcher.Created += OnCreated;
            watcher.Deleted += OnDeleted;
            watcher.Renamed += OnRenamed;
            watcher.Error += OnError;

            watcher.Filter = "*";
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;

           
            Console.WriteLine("Transfer Excel File to Target folder ");
            Console.ReadLine();
        }


        public static void AddExcelFileToDB(string filePath)

        {
            List<Employees> employees = new List<Employees>();
            using (var CoursesReader = new StreamReader(filePath))
            {
                // Ignore the file header
                string Line = CoursesReader.ReadLine();

                // Read file line by line
                while ((Line = CoursesReader.ReadLine()) != null)
                {
                    // Read line columns
                    string[] Columns = Line.Split(',');

                    // Read Employee Data (name , mobile , title ,...)
                    string[] _employeeNameResult = Columns[0].Split(' ');
                    string[] _employeeMobileResult = Columns[1].Split(' ');
                    string[] _employeeTitleResult = Columns[2].Split(' ');
                    string[] _employeeEmailResult = Columns[3].Split(' ');
                    string[] _employeeAddressResult = Columns[4].Split(' ');
                    string[] _employeeNetSalaryResult = Columns[5].Split(' ');
                    string[] _employeeGrossSalaryResult = Columns[6].Split(' ');
                    string[] _employeeGenderResult = Columns[7].Split(' ');


                    // print all Excel data

                    Console.Write(_employeeNameResult[0] + ",");
                    Console.Write(_employeeMobileResult[0] + ",");
                    Console.Write(_employeeTitleResult[0] + ",");
                    Console.Write(_employeeEmailResult[0] + ",");
                    Console.Write(_employeeAddressResult[0] + ",");
                    Console.Write(_employeeNetSalaryResult[0] + ",");
                    Console.Write(_employeeGrossSalaryResult[0] + ",");
                    Console.WriteLine(_employeeGenderResult[0]);


                    SqlConnection conn;
                    SqlCommand insertCommand;
                   
                    using (conn = new SqlConnection())
                    {

                        // Create the connectionString
                        // Trusted_Connection is used to denote the connection uses Windows Authentication
                        conn.ConnectionString = "Server=.;Database=AhliBankTask;Trusted_Connection=true";
                        conn.Open();



                        // create sql query for transfer Excel file to DB

                        string sqlQuery = "INSERT INTO Employees ( employeeName,mobileNumber ,title,email, address,netSalary,grossSalary,gender) VALUES (@employeeName,@mobile, @title,@email, @employeeAddress, @netSalary,@grossSalary,@gender)";
                        insertCommand = new SqlCommand(sqlQuery, conn);
                        insertCommand.Parameters.AddWithValue("@employeeName", _employeeNameResult[0]);
                        insertCommand.Parameters.AddWithValue("@mobile", _employeeMobileResult[0]);
                        insertCommand.Parameters.AddWithValue("@title", _employeeTitleResult[0]);
                        insertCommand.Parameters.AddWithValue("@email", _employeeEmailResult[0]);
                        insertCommand.Parameters.AddWithValue("@employeeAddress", _employeeAddressResult[0]);
                        insertCommand.Parameters.AddWithValue("@netSalary", _employeeNetSalaryResult[0]);
                        insertCommand.Parameters.AddWithValue("@grossSalary", _employeeGrossSalaryResult[0]);
                        insertCommand.Parameters.AddWithValue("@gender", _employeeGenderResult[0]);
                        insertCommand.ExecuteNonQuery();


                        conn.Close();


                    }



                }

            }
        }


        // Clear all Table data in DB 
        public static void deletDbData()
        {

            SqlConnection conn;
            SqlCommand deleteCommand;
            using (conn = new SqlConnection())
            {

                // Create the connectionString
                // Trusted_Connection is used to denote the connection uses Windows Authentication

                conn.ConnectionString = "Server=.;Database=AhliBankTask;Trusted_Connection=true";
                conn.Open();

                string deleteQuery = "delete from Employees";

                deleteCommand = new SqlCommand(deleteQuery, conn);
                deleteCommand.ExecuteNonQuery();

                conn.Close();


            }

        }



        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                // if file Excel File changed clear all data in DB and and transfer again 


                //AddExcelFileToDB();
            }
            Console.WriteLine($"Changed: {e.FullPath}");
        }

        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            string value = $"Created: {e.FullPath}";
            AddExcelFileToDB(  e.FullPath);
            Console.WriteLine(value);


        }

        private static void OnDeleted(object sender, FileSystemEventArgs e)
        {

           // AddExcelFileToDB();
            Console.WriteLine($"Deleted: {e.FullPath}");

        }

        private static void OnRenamed(object sender, RenamedEventArgs e)
        {

            Console.WriteLine($"Renamed:");
            Console.WriteLine($"    Old: {e.OldFullPath}");
            Console.WriteLine($"    New: {e.FullPath}");
        }

        private static void OnError(object sender, ErrorEventArgs e) =>
            PrintException(e.GetException());

        private static void PrintException(Exception? ex)
        {
            if (ex != null)
            {
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine("Stacktrace:");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine();
                PrintException(ex.InnerException);
            }
        }
    }
}

