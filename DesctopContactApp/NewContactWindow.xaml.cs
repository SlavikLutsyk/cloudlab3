using DesctopContactApp.Classes;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.Windows;

namespace DesctopContactApp
{
    /// <summary>
    /// Interaction logic for NewContactWindow.xaml
    /// </summary>
    public partial class NewContactWindow : Window
    {
        private static string _connectionString = "DefaultEndpointsProtocol=https;AccountName=mycontactapp;AccountKey=NX6s/z9BwCcZwP/Wl8ngHsA7+eb+SAaooXvYqKlupkXcIMQP/Y0tpJ1TTUPIC0WHnEu+yKiH7UK/+AStYn3OOQ==;EndpointSuffix=core.windows.net";

        public NewContactWindow()
        {
            InitializeComponent();
            Owner = Application.Current.MainWindow;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {


            var storageAccount = CloudStorageAccount.Parse(_connectionString);

            var tableClient = storageAccount.CreateCloudTableClient();

            var table = tableClient.GetTableReference("contact");

            await table.CreateIfNotExistsAsync();


            Contact contact = new Contact()
            {
                PartitionKey = Guid.NewGuid().ToString(),
                RowKey = Guid.NewGuid().ToString(),
                Name = nameTextBox.Text,
                SurName = surnameTextBox.Text,
                Patronymic = PatronymicTextBox.Text,
                Address = addressTextBox.Text,
                Phone = phoneTextBox.Text

            };



            string contactJson = JsonConvert.SerializeObject(contact);


            var contactEntity = new DynamicTableEntity(contact.PartitionKey, contact.RowKey);
            contactEntity.Properties.Add("Contact", EntityProperty.GeneratePropertyForString(contactJson));



            var insertOperation = TableOperation.InsertOrReplace(contactEntity);
            await table.ExecuteAsync(insertOperation);

            if (Owner is MainWindow mainWindow)
            {
                mainWindow.ReadDatabase();
            }
            Close();
        }

    }
}
