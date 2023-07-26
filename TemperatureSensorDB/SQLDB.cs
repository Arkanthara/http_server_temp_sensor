using System.Data.Entity;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing.Printing;

namespace littlemichelserver.TemperatureSensorDB
{
    public class SQLDB
    {
        // Our parameters of our class
        private static SQLiteConnection connection;
        private static int length_tab_cellule = 40;

        // Function for open an existent data base
        public void OpenDataBase(string name)
        {
            // We create a connection to a DB and we try to connect to this DB
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
        public int CreateTable()
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
                return -1;
            }
            return 0;
        }

        // Function for insert line in our table.
        public static int InsertLine(string data)
        {
            // If we have no data given, we return an error
            if (string.IsNullOrEmpty(data))
            {
                return -1;
            }

            // Else we split parameters
            string[] parameters;
            try
            {
                parameters = data.Split(";") ;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return -1;
            }

            // We initialize our id if the split has worked
            string Id = Guid.NewGuid().ToString().ToUpper();

            // Then, we initialize an sql command
            SQLiteCommand command;
            command = connection.CreateCommand();
            command.CommandText =
                @"
                INSERT INTO Temperature_Sensor (ID,MAC, Temperature, Date)
                VALUES ('" + Id + "', '" + parameters[0] + "', '" + parameters[1] + "', '" + DateTime.Now.ToString() + "');"
                ;
            // We execute the command
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception x)
            {
                Console.WriteLine(x.Message);
                return -1;
            }
            return 0;
        }

        // Function for print a tab in cmd... 
        private static string print_pretty_tab(string text)
        {
            // We initialize our line to write
            string result = "";

            // If the element of the cell is too long, we print an empty cell
            if (text.Length > length_tab_cellule)
            {
                Debug.WriteLine("Error: data " + text + " is too long ! We don't print it !");
                for (int i = 0; i < length_tab_cellule; i++)
                {
                    result += " ";
                }
                return result;
            }
            // Else we print our data, and we complete with " " for always have the same length
            result += text;
            for (int i = text.Length; i < length_tab_cellule; i++)
            {
                result += " ";
            }
            return result;
        }

        // This function is used for print all data in the table in our DB
        public static string PrintTable(bool id = false)
        {
            // We initialize the string to print
            string result = "";

            // We initialize our reader
            SQLiteDataReader reader = null;

            // We initialize our command
            SQLiteCommand command;
            command = connection.CreateCommand();
            command.CommandText =
                @"
                SELECT * FROM Temperature_Sensor
                ";

            // We read the result of our command and display it
            reader = command.ExecuteReader();
            if (reader == null)
            {
                throw new Exception("The reader is null");
            }
            while (reader.Read())
            {
                // If it's ask to print ID, we print ID
                if (id)
                {
                    result += print_pretty_tab(reader.GetString(0));
                    result += print_pretty_tab(reader.GetString(1));
                    result += print_pretty_tab(reader.GetString(2));
                    result += print_pretty_tab(reader.GetString(3));
                    result += "\n";
                }

                // Else we don't print ID
                else
                {
                    result += print_pretty_tab(reader.GetString(1));
                    result += print_pretty_tab(reader.GetString(2));
                    result += print_pretty_tab(reader.GetString(3));
                    result += "\n";
                }
            }
            return result;
        }

        // This function is used either to delete a line of our table, or to clean the table
        public static int DeleteLine(string id = "Your id", bool cleanTable = false)
        {
            // We initialize our SQL command
            SQLiteCommand command;
            command = connection.CreateCommand();

            // If cleanTable is true, we remove all elements of our table
            if (cleanTable)
            {
                command.CommandText =
                @"
                DELETE FROM Temperature_Sensor;";
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return -1;
                }
                return 0;
            }

            // Else, we try to delete the line specified by the parameter given
            command.CommandText =
                @"
                DELETE FROM Temperature_Sensor
                WHERE ID = '" + id + "';";
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return -1;
            }
            return 0;
        }
    }
}
