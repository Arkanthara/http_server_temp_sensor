using System.Data.Entity;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Drawing.Printing;

namespace littlemichelserver.TemperatureSensorDB
{
    public class SQLDB
    {
        // Our parameters of our class
        private static SQLiteConnection connection;
        private static int length_tab_cellule = 20;

        // Function for open an existent data base
        public void OpenDataBase(string name)
        {
            // We create a connection and we connect the connection
            try
            {
                connection = new SQLiteConnection("Data Source=" + name + ".db; FailIfMissing=true");
                connection.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                CreateDataBase(name);
            }
        }

        // Function for create new db and connect to it
        public void CreateDataBase(string name)
        {
            // Here we open the db in mode readwrite because it's the default mode.
            // If we want to modify this, we must add "Mode=ReadOnly" for instance...
            string parameters = "Data Source=" + name + ".db; FailIfMissing=false";
            connection = new SQLiteConnection(parameters);
            try
            {
                connection.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                OpenDataBase(name);
            }
        }
       
        // Function for create new table
        public void CreateTable()
        {
            // We initialize an sql command
            SQLiteCommand command;
            command = connection.CreateCommand();
            command.CommandText =
                @"CREATE TABLE Temperature_Sensor (
                    ID  TEXT,
                    MAC TEXT,
                    Temperature TEXT,
                    Date TEXT
                )";

            // We execute the command
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        // Function for insert line in our table.
        public static void InsertLine(string mac, string temperature, string date)
        {
            // We initialize an sql command
            SQLiteCommand command;
            string Id = Guid.NewGuid().ToString().ToUpper();
            command = connection.CreateCommand();
            command.CommandText =
                @"
                INSERT INTO Temperature_Sensor (ID,MAC, Temperature, Date)
                VALUES ('" + Id + "', '" + mac + "', '" + temperature + "', '" + date + "');"
                ;
            // We execute the command
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        private static string print_pretty_tab(string text)
        {
            string result = "";
            if (text.Length > length_tab_cellule)
            {
                Console.WriteLine("Error: data " + text + " is too long ! We don't print it !");
                for (int i = 0; i < length_tab_cellule; i++)
                {
                    result += " ";
                }
                return result;
            }
            result += text;
            for (int i = text.Length; i < length_tab_cellule; i++)
            {
                result += " ";
            }
            return result;
        }

        public static string PrintTable()
        {
            string result = "";
            SQLiteCommand command;
            SQLiteDataReader reader = null;
            command = connection.CreateCommand();
            command.CommandText =
                @"
                SELECT * FROM Temperature_Sensor
                ";
            reader = command.ExecuteReader();
            if (reader == null)
            {
                throw new Exception("The reader is null");
            }
            while (reader.Read())
            {
                result += print_pretty_tab(reader.GetString(1));
                result += print_pretty_tab(reader.GetString(2));
                result += print_pretty_tab(reader.GetString(3));
                result += "\n";
            }
            return result;
        }
    }
}
