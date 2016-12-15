
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Specialized;



namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            //class to be called when the application finishes loading

            //creating a new datatable to insert the data that will be pulled out from the database
            DataTable ds = new DataTable();
            //in the code block below I created the sql connection and wrote the command that would select the data that i
            //require to use for my application

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString))
            {
                SqlCommand myCommand = default(SqlCommand);
                string SQL = "SELECT * FROM tblSubmission";
                myCommand = new SqlCommand(SQL, cn);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = myCommand;
                da.Fill(ds);
                da.Dispose();
                //here we set the gridviews datasource to the data that are inside the datatable that were pulled from the DB
                grdGrades.ItemsSource = ds.DefaultView;
            }
        }


        private void search_Click(object sender, RoutedEventArgs e)
        {
            //when the user clicks search this class is fired.

            DataTable ds = new DataTable();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString))
            {
                SqlCommand myCommand = default(SqlCommand);
                //here we create an sql connection and call a select statement with a parameter using where
                string SQL = "SELECT * FROM tblSubmission WHERE StudentCard=@SC";
                myCommand = new SqlCommand(SQL, cn);
                //the parameter is pulled from the textbox and searches in the database for any matches 
                myCommand.Parameters.Add(new SqlParameter("@SC", txtSearch.Text));
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = myCommand;
                da.Fill(ds);
                da.Dispose();
                //then it populates the gridview
                grdGrades.ItemsSource = ds.DefaultView;
            }
        }

        private void clear_Click(object sender, RoutedEventArgs e)
        {
            //this class is fired when the user clicks the clear button
            //it repopulates the gridview with all the students details
            DataTable ds = new DataTable();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString))
            {
                SqlCommand myCommand = default(SqlCommand);
                string SQL = "SELECT * FROM tblSubmission";
                myCommand = new SqlCommand(SQL, cn);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = myCommand;
                da.Fill(ds);
                da.Dispose();
                grdGrades.ItemsSource = ds.DefaultView;
            }
        }

        private void LoadStudents()
        {
            //sql connection to pull all students data
            DataTable ds = new DataTable();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString))
            {
                SqlCommand myCommand = default(SqlCommand);
                string SQL = "SELECT * FROM tblSubmission";
                myCommand = new SqlCommand(SQL, cn);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = myCommand;
                da.Fill(ds);
                da.Dispose();
                grdGrades.ItemsSource = ds.DefaultView;
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            //when the user clicks the update button on the row we need to pull the matching id for that student 
            //to update the correct field in the DB 

            //here we get the row data of the selected item
            object item = grdGrades.SelectedItem;

            //here we get the data we need from the row data we pulled before 
            int id = int.Parse((grdGrades.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text);

            string grade = ((grdGrades.SelectedCells[3].Column.GetCellContent(item) as TextBlock).Text);


            if (grade == "")
            {
                //some basic validation if the grade is updated to empty. if the field is empty the app doesnt update the DB
                MessageBox.Show("The grade cannot be empty");
                LoadStudents();
            }
            else
            {
                if (grade == "100")
                //if the grade is exactly 100 then we just update the table
                {
                    using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString))
                    {
                        SqlCommand myCommand = default(SqlCommand);
                        cn.Open();
                        string SQL = "Update tblSubmission SET Grade=@GR WHERE StudentCard=@SC";
                        myCommand = new SqlCommand(SQL, cn);
                        myCommand.Parameters.Add(new SqlParameter("@GR", grade));
                        myCommand.Parameters.Add(new SqlParameter("@SC", id));
                        myCommand.ExecuteNonQuery();
                        cn.Close();
                    }

                    MessageBox.Show("The grade has been updated");
                }
                else if (grade.Length > 2)
                //if the grade is not 100 and the legth of it is more than 2 digits then we dont update the table
                {
                    MessageBox.Show("The grade needs to be less than 100");
                    LoadStudents();
                }
                else

                {
                    //if the grade is not 100 and not more than 2 characters we update the table
                    //create the sql connection
                    using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString))
                    {
                        SqlCommand myCommand = default(SqlCommand);
                        cn.Open();
                        //sql update statement, updating the column grade with the text the user inputed on 
                        //the column that matches the studentCard
                        //
                        string SQL = "Update tblSubmission SET Grade=@GR WHERE StudentCard=@SC";
                        myCommand = new SqlCommand(SQL, cn);
                        myCommand.Parameters.Add(new SqlParameter("@GR", grade));
                        myCommand.Parameters.Add(new SqlParameter("@SC", id));
                        myCommand.ExecuteNonQuery();
                        cn.Close();
                    }

                    MessageBox.Show("The grade has been updated");
                }
            }

        }


    }
}



