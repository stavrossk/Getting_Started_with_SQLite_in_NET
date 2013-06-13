// Author: Tigran Gasparian

// This sample is part Part One of the 'Getting Started with SQLite in C#'
//tutorial at http://www.blog.tigrangasparian.com/


using System;
using System.Data.SQLite;

namespace SQLiteSamples_p1
{


    class Program
    {


        // Holds our connection with the database
        SQLiteConnection _dbConnection;



        static void Main()
        {
            new Program();
        }


        public Program()
        {
            CreateNewDatabase();
            ConnectToDatabase();
            CreateTable();
            FillTable();
            PrintHighscores();
        }



        // Creates an empty database file
        static void CreateNewDatabase()
        {
            SQLiteConnection.CreateFile("MyDatabase.sqlite");
        }




        // Creates a connection with our database file.
        void ConnectToDatabase()
        {
            _dbConnection = new SQLiteConnection("Data Source=MyDatabase.sqlite;Version=3;");
            _dbConnection.Open();
        }




        // Creates a table named 'highscores' with two columns: name (a string of max 20 characters) and score (an int)
        void CreateTable()
        {

            const string sql = "create table highscores (name varchar(20), score int)";
        
            var command = new SQLiteCommand(sql, _dbConnection);
            
            command.ExecuteNonQuery();
        
        }




        // Inserts some values in the highscores table.
        // As you can see, there is quite some duplicate code here, we'll solve this in part two.
        void FillTable()
        {


            string sql = "insert into highscores (name, score) values ('Me', 3000)";
            
            var command = new SQLiteCommand(sql, _dbConnection);
            
            command.ExecuteNonQuery();
            
            sql = "insert into highscores (name, score) values ('Myself', 6000)";
            
            command = new SQLiteCommand(sql, _dbConnection);
            
            command.ExecuteNonQuery();
            
            sql = "insert into highscores (name, score) values ('And I', 9001)";
            
            command = new SQLiteCommand(sql, _dbConnection);
            
            command.ExecuteNonQuery();
        
        
        }





        // Writes the highscores to the console sorted on score in descending order.
        void PrintHighscores()
        {


            const string sql = "select * from highscores order by score desc";
            
            var command = new SQLiteCommand(sql, _dbConnection);
            
            SQLiteDataReader reader = command.ExecuteReader();
            

            while (reader.Read())
                Console.WriteLine(
                    "Name: "
                    + reader["name"] 
                    + "\tScore: " 
                    + reader["score"]);
            

            Console.ReadLine();
        
        
        }



    }




}
