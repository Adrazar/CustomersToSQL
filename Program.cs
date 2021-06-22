using System;
using System.IO;
using System.Data;
//using System.Text;
using System.Collections.Generic;
//using System.Collections.Specialized;
//using System.Web.Script.Serialization;
using System.Data.SqlClient;
//using Microsoft.Data.SqlClient;
using System.Text;

namespace CustomersToSQL
{
    class Program
    {
        static void Main(string[] args)
        {
            //el reader of the customers csv
            var MyReader = new Microsoft.VisualBasic.FileIO.TextFieldParser("C:/Users/Marcos/source/repos/CustomersToSQL/Customers.csv");
            MyReader.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
            MyReader.SetDelimiters(new string[] { ";" });

            var tablaCus = new DataTable();
            tablaCus.TableName = "CustomersInercya";//thetable
            tablaCus.Columns.Add("Id", typeof(string));//datatype string
            tablaCus.Columns.Add("Name", typeof(string));//datatype string
            tablaCus.Columns.Add("Address", typeof(string));//datatype string
            tablaCus.Columns.Add("City", typeof(string));//datatype string
            tablaCus.Columns.Add("Country", typeof(string));//datatype string
            tablaCus.Columns.Add("PostalCode", typeof(string));//datatype string
            tablaCus.Columns.Add("Phone", typeof(string));//datatype string
            if (!MyReader.EndOfData)
            {
                MyReader.ReadLine();
            }
            while (!MyReader.EndOfData)
            {
                MyReader.ReadLine();
                //   tablaCus.Rows.Add(MyReader.ReadFields());
                tablaCus.Rows.Add(MyReader.ReadFields());
                /* reads to table*/
            }





            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "DESKTOP-H1I0IQ8";
                builder.IntegratedSecurity = true;
               // builder.UserID = "<your_username>";
               // builder.Password = "<your_password>";
                builder.InitialCatalog = "NORTHWND";

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    //Console.WriteLine("\nQuery data example:");
                    //Console.WriteLine("=========================================\n");

                    String sql = "DROP TABLE IF EXISTS CustomersInercya; CREATE TABLE CustomersInercya (Id VARCHAR(30),Name VARCHAR(50),Address VARCHAR(100),City VARCHAR(30),Country VARCHAR(30),PostalCode VARCHAR(30),Phone VARCHAR(40));";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine(reader[0]);
                            }
                        }
                    }
                }


                SqlConnection connectionB = new SqlConnection(builder.ConnectionString);
                SqlBulkCopy bulkcopy = new SqlBulkCopy(connectionB);
                connectionB.Open();
                bulkcopy.DestinationTableName = tablaCus.TableName;
                try
                {
                    bulkcopy.WriteToServer(tablaCus);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }




            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.ReadLine();






        }
    }
}

