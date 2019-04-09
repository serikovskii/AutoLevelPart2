using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using System.Configuration;

namespace AutoLevelPart2
{
    class Program
    {
        static void Main(string[] args)
        {
            var providerName = ConfigurationManager.ConnectionStrings["appConnection"].ProviderName; 
            var connectionString = ConfigurationManager.ConnectionStrings["appConnection"].ConnectionString;

            var factory = DbProviderFactories.GetFactory(providerName);
            var connection = factory.CreateConnection();
            var command = connection.CreateCommand();

            connection.ConnectionString = connectionString;
            command.CommandText = "select * from Users";

            var dataSet = new DataSet("usersApp");
            var dataAdapter = factory.CreateDataAdapter();

            dataAdapter.SelectCommand = command;

            var commandBuilder = factory.CreateCommandBuilder();
            commandBuilder.DataAdapter = dataAdapter;

            connection.Open();
            dataAdapter.Fill(dataSet, "Users");
            connection.Close();
            //commandBuilder.Dispose();

            var firstRow = dataSet.Tables["Users"].Rows[0];

            var user = new User
            {
                Id = (int)firstRow["Id"],
                Login = firstRow["Login"].ToString(),
                Password = firstRow["Password"].ToString()
            };

            firstRow.BeginEdit();
            firstRow.ItemArray = new object[] {user.Id, "asd", "dsa" };
            firstRow.EndEdit();

            dataAdapter.Update(dataSet, "Users");
        }
    }
}
