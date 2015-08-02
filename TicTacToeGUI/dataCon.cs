using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data.SqlClient;


namespace TicTacToeGUI
{
    class dataCon
    {
        string conString="Data Source=(LocalDB)\\v11.0;Database=GamePlayer;Initial Catalog=GamePlayer;Integrated Security=True;User Instance=False";
        SqlConnection connect;
        SqlCommand _command;

        #region Make database connection
        public void ConnectDB(){
            connect=new SqlConnection(conString);
        }
        #endregion

        #region Write to database
        public bool writeCommand(string query)
        {
            _command = new SqlCommand(query, connect);
            connect.Open();
            try
            {
                
                int rows=_command.ExecuteNonQuery();
                if (rows > 0) {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connect.Close();
            }
            return false;
        }
        #endregion

        #region Read from database
        public ArrayList runMyCommand(string query){
            ArrayList list = new ArrayList();
            _command = connect.CreateCommand();
            _command.CommandText = query;

            connect.Open();
            try
            {
                
                SqlDataReader reader=_command.ExecuteReader();

                while (reader.Read()) {
                    if (reader.FieldCount > 1)
                    {
                        object[] data = new object[reader.FieldCount];
                        reader.GetValues(data);
                        list.Add(data);
                    }
                    else {
                        object data=reader.GetValue(0);
                        list.Add(data);
                    }
                }
                for (int i = 0; i < list.Count; i++) {
                    object[] data =(object[]) list[i];
                    Console.WriteLine(data[1]);
                }
            }catch(Exception ex){
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connect.Close();
            }
            return list;
        }
#endregion
    }

}
