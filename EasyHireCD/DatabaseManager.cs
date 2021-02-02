using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace EasyHireCD
{
    public class DatabaseManager
    {
        
        SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection(@"Data Source=LAPTOP-RAKIOMBV\SQLEXPRESS;Initial Catalog=EasyHireCD;Integrated Security=True");

        SqlCommand SqlStr = new SqlCommand();
        SqlDataReader SqlReader;
        String SqlStmt;

        // This Function will work according to the Statement
        public DataTable ListPopular(String Statement)
        {
            DataTable dt = new DataTable();
            SqlDataReader SqlReader;
            try
            {

                SqlStr.Connection = sqlConn;
                // SqlStmt = "Select * from Customer Where FirstName like '%'+ FirstName + '%' ";
                using (SqlStr = new SqlCommand(SqlStmt, sqlConn))
                {
                    SqlStr.Connection = sqlConn;
                    SqlStmt = Statement;
                    using (SqlStr = new SqlCommand(SqlStmt, sqlConn))
                    {
                        sqlConn.Open();
                        SqlReader = SqlStr.ExecuteReader();
                        if (SqlReader.HasRows)
                        {
                            dt.Load(SqlReader);
                        }
                        sqlConn.Close();
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Exception : " + ex.Message);
                sqlConn.Close();
                return null;
            }
        }


        //Login Check
        public DataTable Login(String username, String Password)
        {
            DataTable dt = new DataTable();
            SqlDataReader SqlReader;
            try
            {
                SqlStr.Connection = sqlConn;
                using (SqlStr = new SqlCommand(SqlStmt, sqlConn))
                {
                    SqlStr.Connection = sqlConn;
                    SqlStmt = "Select E.FirstName,E.LastName,U.UserType from Employee E Inner join Users U on E.Empid= U.EmpIDFK where Username=@UserName  and password =@Password ";
                    using (SqlStr = new SqlCommand(SqlStmt, sqlConn))
                    {
                        SqlStr.Parameters.AddWithValue("@UserName", username);
                        SqlStr.Parameters.AddWithValue("@Password", Password);
                        sqlConn.Open();
                        SqlReader = SqlStr.ExecuteReader();
                        if (SqlReader.HasRows)
                        {
                            dt.Load(SqlReader);
                            MessageBox.Show(" Login Successful");
                        }
                        sqlConn.Close();
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Exception Employee: " + ex.Message);
                sqlConn.Close();
                return null;
            }
        }

        // To check the same name already exist or not
        public int CheckName(String Title)
        {
            Int32 Count = 0;
            try
            {
                SqlStr.Connection = sqlConn;
                SqlStmt = "Select Count(*) AS Movie_Count from Movies where Title=@Title";
                SqlStr.CommandText = SqlStmt;
                using (SqlStr = new SqlCommand(SqlStmt, sqlConn))
                {
                    SqlStr.Parameters.AddWithValue("@Title", Title);
                    sqlConn.Open();
                    Count = (Int32)SqlStr.ExecuteScalar();
                    sqlConn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                sqlConn.Close();

            }
            return Count;
        }
        //Fucntion to get the number of movies not returned
        public int GetCopiesOut(int Mid)
        {
            Int32 Count = 0;
            try
            {
                SqlStr.Connection = sqlConn;
                SqlStmt = "Select Count(*) AS Movie_Count from RentedMovies where DateReturned IS NULL AND (MovieIDFK=@MovieIDFK OR CustIDFK=@CustIDFK) ";
                SqlStr.CommandText = SqlStmt;
                using (SqlStr = new SqlCommand(SqlStmt, sqlConn))
                {

                    SqlStr.Parameters.AddWithValue("@MovieIDFK", Mid);
                    SqlStr.Parameters.AddWithValue("@CustIDFK", Mid);
                    sqlConn.Open();
                    Count = (Int32)SqlStr.ExecuteScalar();
                    sqlConn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                sqlConn.Close();

            }
            return Count;
        }

        //List Customer Details
        public DataTable ListCustomers(String customerfirstname)
        {
            DataTable dt = new DataTable();
            SqlDataReader SqlReader;
            try
            {
                SqlStr.Connection = sqlConn;
                using (SqlStr = new SqlCommand(SqlStmt, sqlConn))
                {
                    SqlStr.Connection = sqlConn;
                    SqlStmt = "Select * from Customer Where  FirstName like '%' + @FirstName+ '%' OR LastName like'%' + @LastName+ '%' OR Phone like'%' + @Phone+ '%'OR Address like'%' + @Address+ '%'";
                    using (SqlStr = new SqlCommand(SqlStmt, sqlConn))
                    {
                        SqlStr.Parameters.AddWithValue("@FirstName", customerfirstname);
                        SqlStr.Parameters.AddWithValue("@LastName", customerfirstname);
                        SqlStr.Parameters.AddWithValue("@Phone", customerfirstname);
                        SqlStr.Parameters.AddWithValue("@Address", customerfirstname);
                        sqlConn.Open();
                        SqlReader = SqlStr.ExecuteReader();
                        if (SqlReader.HasRows)
                        {
                            dt.Load(SqlReader);
                        }
                        sqlConn.Close();
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Exception : " + ex.Message);
                sqlConn.Close();
                return null;
            }


        }

        // Function to List Employee Details
        public DataTable ListEmployee(String SearchEmp)
        {
            DataTable dt = new DataTable();
            SqlDataReader SqlReader;
            try
            {

                SqlStr.Connection = sqlConn;
                // SqlStmt = "Select * from Customer Where FirstName like '%'+ FirstName + '%' ";
                using (SqlStr = new SqlCommand(SqlStmt, sqlConn))
                {
                    SqlStr.Connection = sqlConn;
                    SqlStmt = "Select * from Employee Where FirstName like '%' + @FirstName+ '%' OR LastName like'%' + @LastName+ '%' OR Phone like'%' + @Phone+ '%'OR Address like'%' + @Address+ '%'";
                    using (SqlStr = new SqlCommand(SqlStmt, sqlConn))
                    {
                        SqlStr.Parameters.AddWithValue("@FirstName", SearchEmp);
                        SqlStr.Parameters.AddWithValue("@LastName", SearchEmp);
                        SqlStr.Parameters.AddWithValue("@Phone", SearchEmp);
                        SqlStr.Parameters.AddWithValue("@Address", SearchEmp);
                        sqlConn.Open();
                        SqlReader = SqlStr.ExecuteReader();
                        if (SqlReader.HasRows)
                        {
                            dt.Load(SqlReader);
                        }
                        sqlConn.Close();
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Exception Employee: " + ex.Message);
                sqlConn.Close();
                return null;
            }

        }

        // Function to List Movie Details
        public DataTable ListMovies(String movietitle)
        {
            DataTable dt = new DataTable();
            SqlDataReader SqlReader;
            try
            {
                SqlStr.Connection = sqlConn;
                SqlStmt = "Select * from Movies Where Title like '%' + @Title+ '%'OR Genre like '%' + @Genre+ '%'OR  Plot like '%' + @Plot+ '%'";

                using (SqlStr = new SqlCommand(SqlStmt, sqlConn))
                {
                    SqlStr.Parameters.AddWithValue("@Title", movietitle);
                    SqlStr.Parameters.AddWithValue("@Genre", movietitle);
                    SqlStr.Parameters.AddWithValue("@Plot", movietitle);
                    sqlConn.Open();
                    SqlReader = SqlStr.ExecuteReader();
                    if (SqlReader.HasRows)
                    {
                        dt.Load(SqlReader);
                    }
                    sqlConn.Close();
                    return dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Exception : " + ex.Message);
                sqlConn.Close();
                return null;
            }
        }

        // Function to List Rented Movie Details
        public DataTable ListRental(String SearchInfo)
        {
            DataTable dt = new DataTable();
            SqlDataReader SqlReader;
            try
            {
                // List Only Rented Out Movies.  & Search
                SqlStr.Connection = sqlConn;
                SqlStmt = " Select R.RMID,C.FirstName,C.LastName,C.Address, M.Title,R.dateRented, R.DateReturned,M.Rental_Cost from RentedMovies R inner join Movies M on R.MovieIDFK = M.MovieID inner join Customer C on R.CustIDFK = c.CustID where DateReturned IS NULL AND (Title like'%' + @Title + '%'OR FirstName like'%' + @FirstName + '%'  OR LastName like'%' + @LastName + '%' )";

                using (SqlStr = new SqlCommand(SqlStmt, sqlConn))
                {

                    SqlStr.Parameters.AddWithValue("@Title", SearchInfo);
                    SqlStr.Parameters.AddWithValue("@FirstName", SearchInfo);
                    SqlStr.Parameters.AddWithValue("@LastName", SearchInfo);
                    sqlConn.Open();
                    SqlReader = SqlStr.ExecuteReader();
                    if (SqlReader.HasRows)
                    {
                        dt.Load(SqlReader);
                    }
                    sqlConn.Close();
                    return dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Exception : " + ex.Message);
                sqlConn.Close();
                return null;
            }
        }

        // Function to Add New Customer
        public bool AddCustomer(String cfirstname, String clastname, string caddress, string cphone)
        {
            bool CustomerAdded = false;
            try
            {
                SqlStr.Connection = sqlConn;
                SqlStmt = "insert into Customer(FirstName,LastName,Address,Phone) values(@FirstName,@LastName,@Address,@Phone)";
                using (SqlStr = new SqlCommand(SqlStmt, sqlConn))
                {
                    SqlStr.Parameters.AddWithValue("@FirstName", cfirstname);
                    SqlStr.Parameters.AddWithValue("@LastName", clastname);
                    SqlStr.Parameters.AddWithValue("@Address", caddress);
                    SqlStr.Parameters.AddWithValue("@Phone", cphone);

                    SqlStr.CommandText = SqlStmt;
                    sqlConn.Open();
                    Int32 affectedRecords = SqlStr.ExecuteNonQuery();
                    if (affectedRecords > 0)
                    {
                        CustomerAdded = true;
                    }
                    sqlConn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Exception : " + ex.Message);
                sqlConn.Close();
            }
            return CustomerAdded;
        }

        // Function to Update Customer Details
        public bool UpdateCustomer(String cfirstname, String clastname, string caddress, string cphone, int custid)
        {
            bool CustomerEdited = false;
            try
            {
                SqlStr.Connection = sqlConn;
                SqlStmt = "Update Customer set FirstName=@FirstName,LastName=@LastName,Address=@Address,Phone=@Phone  where CustID=@CustID";
                using (SqlStr = new SqlCommand(SqlStmt, sqlConn))
                {
                    SqlStr.Parameters.AddWithValue("@CustID", custid);
                    SqlStr.Parameters.AddWithValue("@FirstName", cfirstname);
                    SqlStr.Parameters.AddWithValue("@LastName", clastname);
                    SqlStr.Parameters.AddWithValue("@Address", caddress);
                    SqlStr.Parameters.AddWithValue("@Phone", cphone);
                    SqlStr.CommandText = SqlStmt;
                    sqlConn.Open();
                    Int32 affectedRecords = SqlStr.ExecuteNonQuery();
                    if (affectedRecords > 0)
                    {
                        CustomerEdited = true;

                    }
                    sqlConn.Close();
                }
                sqlConn.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Database Exception Update Customers : " + ex.Message);
                sqlConn.Close();
            }
            return CustomerEdited;
        }

        // Function to Delete Selected Customer
        public bool DeleteCustomer(int custID)
        {
            bool CustomerDeleted = false;
            try
            {
                SqlStr.Connection = sqlConn;
                SqlStmt = "Delete from Customer where CustID=@CustID";
                using (SqlStr = new SqlCommand(SqlStmt, sqlConn))
                {
                    SqlStr.Parameters.AddWithValue("@CustID", custID);
                    sqlConn.Open();
                    Int32 affectedRecords = SqlStr.ExecuteNonQuery();
                    if (affectedRecords > 0)
                    {
                        CustomerDeleted = true;
                    }
                    sqlConn.Close();
                }
                return CustomerDeleted;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Exception : " + ex.Message);
                sqlConn.Close();
                return false;
            }
        }

        // Function to Add New Movie
        public bool AddMovies(String title, String rentalcost, String plot, String year, String copies, String genre, String rating)
        {
            bool MovieAdded = false;
            try
            {
                SqlStr.Connection = sqlConn;
                SqlStmt = "insert into Movies(Title,Rating,Year,Rental_Cost,Copies,Plot,Genre) values(@Title,@Rating,@Year,@Rental_Cost,@Copies,@Plot,@Genre)";
                using (SqlStr = new SqlCommand(SqlStmt, sqlConn))
                {
                    SqlStr.Parameters.AddWithValue("@Title", title);
                    SqlStr.Parameters.AddWithValue("@Rating", rating);
                    SqlStr.Parameters.AddWithValue("@Year", year);
                    SqlStr.Parameters.AddWithValue("@Rental_Cost", rentalcost);
                    SqlStr.Parameters.AddWithValue("@Copies", copies);
                    SqlStr.Parameters.AddWithValue("@Plot", plot);
                    SqlStr.Parameters.AddWithValue("@Genre", genre);
                    SqlStr.CommandText = SqlStmt;
                    sqlConn.Open();
                    Int32 affectedRecords = SqlStr.ExecuteNonQuery();
                    if (affectedRecords > 0)
                    {
                        MovieAdded = true;
                    }
                    sqlConn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Exception Movie Add : " + ex.Message);
                sqlConn.Close();
            }
            return MovieAdded;
        }

        // Function to Update Movie
        public bool UpdateMovie(String title, String rentalcost, String plot, String year, String copies, String genre, String rating, int movieID)
        {
            bool MovieEdited = false;
            try
            {
                SqlStr.Connection = sqlConn;
                SqlStmt = "Update Movies set Title=@Title,Rental_Cost=@Rental_Cost,Plot=@Plot,Year=@Year,Copies=@Copies,Genre=@Genre,Rating=@Rating  where MovieID=@MovieID";
                using (SqlStr = new SqlCommand(SqlStmt, sqlConn))
                {
                    SqlStr.Parameters.AddWithValue("@MovieID", movieID);
                    SqlStr.Parameters.AddWithValue("@Title", title);
                    SqlStr.Parameters.AddWithValue("@Rental_Cost", rentalcost);
                    SqlStr.Parameters.AddWithValue("@Plot", plot);
                    SqlStr.Parameters.AddWithValue("@Year", year);
                    SqlStr.Parameters.AddWithValue("@Copies", copies);
                    SqlStr.Parameters.AddWithValue("@Genre", genre);
                    SqlStr.Parameters.AddWithValue("@Rating", rating);
                    SqlStr.CommandText = SqlStmt;
                    sqlConn.Open();
                    Int32 affectedRecords = SqlStr.ExecuteNonQuery();
                    if (affectedRecords > 0)
                    {
                        MovieEdited = true;
                    }
                    sqlConn.Close();
                }
                sqlConn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Exception Update Movie : " + ex.Message);
                sqlConn.Close();
            }
            return MovieEdited;

        }
        // Function to Delete Movie
        public bool DeleteMovie(int movieID)
        {
            bool MovieDeleted = false;
            try
            {
                SqlStr.Connection = sqlConn;
                SqlStmt = "Delete from Movies where MovieID=@MovieID";
                using (SqlStr = new SqlCommand(SqlStmt, sqlConn))
                {
                    SqlStr.Parameters.AddWithValue("@MovieID", movieID);

                    //SqlStr.CommandText = SqlStmt;
                    sqlConn.Open();
                    Int32 affectedRecords = SqlStr.ExecuteNonQuery();
                    if (affectedRecords > 0)
                    {
                        MovieDeleted = true;
                    }
                    sqlConn.Close();
                }
                return MovieDeleted;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Exception Deleting Movie: " + ex.Message);
                sqlConn.Close();
                return false;
            }
        }

        // Function to Issue Movie
        public bool IssueMovie(int movieid, int custid, DateTime daterented)
        {
            bool Movieissued = false;
            int affectedRecords;

            try
            {
                SqlStr.Connection = sqlConn;
                SqlStmt = "insert into RentedMovies(MovieIDFK,CustIDFK,DateRented) values(@MovieID,@CustID,@DateRented)";
                using (SqlCommand cmd = new SqlCommand(SqlStmt, sqlConn))
                {
                    cmd.Parameters.AddWithValue("@MovieID", movieid);
                    cmd.Parameters.AddWithValue("@CustID", custid);
                    cmd.Parameters.AddWithValue("@DateRented", daterented);
                    sqlConn.Open();
                    affectedRecords = cmd.ExecuteNonQuery();
                }
                if (affectedRecords > 0)
                {
                    Movieissued = true;
                }
                sqlConn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Exception Movie Issue" + ex.Message);
                sqlConn.Close();
            }
            return Movieissued;
        }


        // Function to Return Movie
        public bool ReturnMovie(int rentmovieID, DateTime datereturned)
        {
            bool MovieReturned = false;
            int affectedRecords;
            try
            {
                SqlStr.Connection = sqlConn;
                SqlStmt = "Update RentedMovies set DateReturned=@DateReturned  where RMID='" + rentmovieID + "'";
                using (SqlCommand cmd = new SqlCommand(SqlStmt, sqlConn))
                {
                    cmd.Parameters.AddWithValue("@RMID", rentmovieID);
                    cmd.Parameters.AddWithValue("@DateReturned", datereturned);

                    sqlConn.Open();
                    affectedRecords = cmd.ExecuteNonQuery();
                }
                if (affectedRecords > 0)
                {
                    MovieReturned = true;
                }
                sqlConn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Exception Return Movie " + ex.Message);
                sqlConn.Close();
            }
            return MovieReturned;
        }
        // Function to Add New Employee
        public bool AddEmployee(String cfirstname, String clastname, string caddress, string cphone, String username, String password, String usertype)
        {

            bool EmployeeAdded = false;
            try
            {
                SqlStr.Connection = sqlConn;
                // Employee and  User table updated
                // Username is the firstname of the employee
                SqlStmt = "insert into Employee(FirstName,LastName,Address,Phone) values(@FirstName,@LastName,@Address,@Phone); insert into Users(UserName,Password,UserType) values(@UserName,@Password,@UserType)";
                using (SqlStr = new SqlCommand(SqlStmt, sqlConn))
                {
                    SqlStr.Parameters.AddWithValue("@FirstName", cfirstname);
                    SqlStr.Parameters.AddWithValue("@LastName", clastname);
                    SqlStr.Parameters.AddWithValue("@Address", caddress);
                    SqlStr.Parameters.AddWithValue("@Phone", cphone);

                    SqlStr.Parameters.AddWithValue("@UserName", username);
                    SqlStr.Parameters.AddWithValue("@Password", password);
                    SqlStr.Parameters.AddWithValue("@UserType", usertype);
                    SqlStr.CommandText = SqlStmt;
                    sqlConn.Open();
                    Int32 affectedRecords = SqlStr.ExecuteNonQuery();
                    if (affectedRecords > 0)
                    {
                        EmployeeAdded = true;
                    }
                    sqlConn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Exception Employee: " + ex.Message);
                sqlConn.Close();
            }
            return EmployeeAdded;
        }

        // Function to Update Employee
        public bool UpdateEmployee(String cfirstname, String clastname, string caddress, string cphone, String password, String usertype, int empid)
        {
            bool EmployeeEdited = false;
            try
            {
                SqlStr.Connection = sqlConn;
                SqlStmt = "Update Employee set FirstName=@FirstName,LastName=@LastName,Address=@Address,Phone=@Phone where EmpID=@EmpID;Update Users set Password=@Password,UserType=@UserType where EmpIDFK=@EmpIDFK";
                using (SqlStr = new SqlCommand(SqlStmt, sqlConn))
                {
                    SqlStr.Parameters.AddWithValue("@EmpID", empid);
                    SqlStr.Parameters.AddWithValue("@FirstName", cfirstname);
                    SqlStr.Parameters.AddWithValue("@LastName", clastname);
                    SqlStr.Parameters.AddWithValue("@Address", caddress);
                    SqlStr.Parameters.AddWithValue("@Phone", cphone);
                    SqlStr.Parameters.AddWithValue("@EmpIDFK", empid);
                    SqlStr.Parameters.AddWithValue("@Password", password);
                    SqlStr.Parameters.AddWithValue("@UserType", usertype);
                    SqlStr.CommandText = SqlStmt;
                    sqlConn.Open();
                    Int32 affectedRecords = SqlStr.ExecuteNonQuery();
                    if (affectedRecords > 0)
                    {
                        EmployeeEdited = true;

                    }
                    sqlConn.Close();
                }
                sqlConn.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show("Database Exception Update Employee: " + ex.Message);
                sqlConn.Close();
            }
            return EmployeeEdited;
        }
        // Function to Delete Employee
        public bool DeleteEmployee(int empID)
        {
            bool employeeDeleted = false;
            try
            {
                SqlStr.Connection = sqlConn;

                SqlStmt = "Delete from Users where EmpIDFK=@EmpIDFK; Delete from Employee where EmpID=@EmpID";
                using (SqlStr = new SqlCommand(SqlStmt, sqlConn))
                {
                    SqlStr.Parameters.AddWithValue("@EmpIDFK", empID);
                    SqlStr.Parameters.AddWithValue("@EmpID", empID);
                    sqlConn.Open();
                    Int32 affectedRecords = SqlStr.ExecuteNonQuery();
                    if (affectedRecords > 0)
                    {
                        employeeDeleted = true;
                    }
                    sqlConn.Close();
                }
                return employeeDeleted;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Exception Employee Delete: " + ex.Message);
                sqlConn.Close();
                return false;

            }

        }
        //Delete Rental
        public bool DeleteRental(int rmID)
        {
            bool RentalDeleted = false;
            try
            {
                SqlStr.Connection = sqlConn;
                SqlStmt = "Delete from RentedMovies where RMID=@RMID";
                using (SqlStr = new SqlCommand(SqlStmt, sqlConn))
                {
                    SqlStr.Parameters.AddWithValue("@RMID", rmID);
                    sqlConn.Open();
                    Int32 affectedRecords = SqlStr.ExecuteNonQuery();
                    if (affectedRecords > 0)
                    {
                        RentalDeleted = true;
                    }
                    sqlConn.Close();
                }
                return RentalDeleted;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Exception : " + ex.Message);
                sqlConn.Close();
                return false;
            }
        }
    }
}
