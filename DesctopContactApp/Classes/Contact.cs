using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace DesctopContactApp.Classes
{
    [Serializable]
    public class Contact : TableEntity
    {

        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }


    }

}
