using DesctopContactApp.Classes;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System.Windows;

namespace DesctopContactApp
{
    /// <summary>
    /// Interaction logic for ContactDetails.xaml
    /// </summary>
    public partial class ContactDetails : Window
    {
        Contact contact;
        private static string _connectionString = "DefaultEndpointsProtocol=https;AccountName=mycontactapp;AccountKey=NX6s/z9BwCcZwP/Wl8ngHsA7+eb+SAaooXvYqKlupkXcIMQP/Y0tpJ1TTUPIC0WHnEu+yKiH7UK/+AStYn3OOQ==;EndpointSuffix=core.windows.net";
        public ContactDetails(Contact contact)
        {

            InitializeComponent();

            Owner = Application.Current.MainWindow;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;

            this.contact = contact;
            nameTextBox.Text = contact.Name;
            phoneTextBox.Text = contact.Phone;
            emailTextBox.Text = contact.Email;
        }

        private async void updateButton_Click(object sender, RoutedEventArgs e)
        {
            //contact.Name = nameTextBox.Text;
            //contact.Phone = phoneTextBox.Text;
            //contact.Email = emailTextBox.Text;

            //using (SQLiteConnection connection = new SQLiteConnection(App.dbPath))
            //{
            //    connection.CreateTable<Contact>();
            //    connection.Update(contact);
            //}

            //Close();

            var storageAccount = CloudStorageAccount.Parse(_connectionString);
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference("contact");

            await table.CreateIfNotExistsAsync();


            TableOperation retrieveOperation = TableOperation.Retrieve<DynamicTableEntity>("PartitionKey", contact.RowKey);
            TableResult result = await table.ExecuteAsync(retrieveOperation);
            DynamicTableEntity existingEntity = (DynamicTableEntity)result.Result;

            if (existingEntity != null)
            {

                string newContactJson = JsonConvert.SerializeObject(contact);


                existingEntity.Properties["Contact"] = EntityProperty.GeneratePropertyForString(newContactJson);


                TableOperation updateOperation = TableOperation.Replace(existingEntity);
                await table.ExecuteAsync(updateOperation);


            }
            Close();

        }

        private async void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            //using (SQLiteConnection connection = new SQLiteConnection(App.dbPath))
            //{
            //    connection.CreateTable<Contact>();
            //    connection.Delete(contact);
            //}

            var storageAccount = CloudStorageAccount.Parse(_connectionString);
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference("contact");

            // Видалення запису за його PartitionKey і RowKey
            TableOperation deleteOperation = TableOperation.Delete(new TableEntity(contact.PartitionKey, contact.RowKey) { ETag = "*" });

            await table.ExecuteAsync(deleteOperation);

            Close();
        }
    }
}
