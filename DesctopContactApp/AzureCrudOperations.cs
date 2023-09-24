using DesctopContactApp.Classes;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesctopContactApp
{
    public class Operations
    {


        public static async Task AddAsync(CloudTable table, Contact contact)
        {
            var insertOperation = TableOperation.Insert(contact);
            await table.ExecuteAsync(insertOperation);
        }

        public static async Task<Contact> GetAsync(CloudTable table, string lastName, string firstName)
        {
            var retrieve = TableOperation.Retrieve<Contact>(lastName, firstName);
            var result = await table.ExecuteAsync(retrieve);
            return (Contact)result.Result;
        }

        public static async Task DeleteAsync(CloudTable table, Contact contact)
        {
            var deleteOperation = TableOperation.Delete(contact);
            await table.ExecuteAsync(deleteOperation);
        }

        public static async Task<List<Contact>> FindByFieldAsync(CloudTable table, string field)
        {
            var filterCondition = TableQuery.GenerateFilterCondition("Email", QueryComparisons.Equal, field);
            var query = new TableQuery<Contact>().Where(filterCondition);
            var results = await table.ExecuteQuerySegmentedAsync(query, null);
            return results.ToList();
        }
    }
}
