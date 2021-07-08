using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ListeBox
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection sqlConnection;

        public MainWindow()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ListeBox.Properties.Settings.WPFConnectionString"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            InitializeComponent();
            ShowPersonne();
            ShowAssociatedOrders();
        }

        private void ShowPersonne()
        {

            try
            {
                string query = "select * from Persons";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable PersonTable = new DataTable();
                    sqlDataAdapter.Fill(PersonTable);

                    listePersonne.DisplayMemberPath = "LastName";
                    listePersonne.SelectedValuePath = "id";
                    listePersonne.ItemsSource = PersonTable.DefaultView;

                }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }
        }


        private void ShowAssociatedOrders()
        {

            try
            {
                string query = "select * from Orders o inner join Persons p on o.PersonID = p.ID where p.ID = @id";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    
                    sqlCommand.Parameters.AddWithValue("@id", listePersonne.SelectedValue);

                    DataTable PersonTable = new DataTable();

                    sqlDataAdapter.Fill(PersonTable);

                    OrderBypersonne.DisplayMemberPath = "OrderNumber";
                    OrderBypersonne.ItemsSource = PersonTable.DefaultView;

                }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }
        }


        private void listePersonne_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowAssociatedOrders();
        }
    }
}
