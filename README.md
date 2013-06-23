Getting Started with SQLite in .NET
==================================



This tutorial will teach you how to create and connect to an SQLite database in C#.
You will also learn how to create and modify tables and how to execute SQL queries on the database
and how to read the returned results.


I’ll assume that you’re already familiar with SQL and at least have some knowledge of how it works
(for example: what to expect as a result from “select * from table1″ )


There will probably be two parts, with the first one discussing the basics needed to do pretty much anything,
and in the second part I’ll discuss some miscellaneous subjects like how to parameterize your queries
to make them much faster and safer.


Create a C# console project:
-----------------------------


Create a standard C# console project.


Since we’re working in C# we’ll be using the System.Data.SQLite library.
This library is not a standard library (packaged with .NET for example)
so we’ll need to download it.
It is being developed by the people who’re also working on the (original) SQLite.


All you’ll need are two files, a .dll and a .xml file for some documentation.
These files are available for download at the end of this article,
you can also download these from their website, but you’ll also get some files that you don’t need.


Put these files in the folder of your project and add an assembly reference to the .dll.
Just browse to System.Data.SQLite.dll and select it.



Now add using System.Data.SQLite; to the usings and you’re done.
You’ve successfully added the SQLite library to you project!


Creating a database file:
-------------------------

You usually don’t need to create a new database file, you work with an existing one,
but for those cases where you do need to create a brand new one, here’s the code:


SQLiteConnection.CreateFile("MyDatabase.sqlite");



Connecting to a database:
--------------------------

Before you can use the database, you’ll need to connect to it.
This connection is stored inside a connection object.
Every time you interact with the database, you’ll need to provide the connection object.
Therefore, we’ll declare the connection object as a member variable.



SQLiteConnection m_dbConnection;


When creating a connection, we’ll need to provide a “connection string”. 
This string can contain information about the… connection.
Things like the filename of the database, the version,
but can also contain things like a password, if it’s required.


You can find a few of these at: http://www.connectionstrings.com/sqlite


The first one is good enough to get our connection up and running, so we get:


m_dbConnection = new SQLiteConnection("Data Source=MyDatabase.sqlite;Version=3;");
m_dbConnection.Open();



After we create the connection object, we’ll have to open it.
And with every Open() there comes a Close(),
so don’t forget to call that after you’re done with your connection.



Creating a table:
------------------


Let’s write some SQL now. We’ll create a table with two columns,
the first one contains names and the second one contains scores.
See it as a high scores table.


string sql = "create table highscores (name varchar(20), score int)";

You could also spam caps if you like and get something like this:

string sql = "CREATE TABLE highscores (name VARCHAR(20), score INT)";



Now we’ll need to create an SQL command in order to execute it.
Luckily, we’ve got the SQLiteCommand class for this.
We create a command by entering the sql query along with the connection object.



SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);



Afterwards, we execute the command. But before we execute our command,
i’d like to mention that not all commands are the same,
some commands return results (like SELECT etc.) and others don’t (like the one we just wrote)
That’s why there are two execute methods (actually,
there are three) One returns the actual results
(the rows of the table) the other returns an integer indicating the number of rows that have been modified.
We’ll use the last one now.


command.ExecuteNonQuery();


At this time, we’re not interested in the number of rows that have been modified (it’s 0)
But you could imagine that it might be interesting to know this information in UPDATE queries.




Filling our table:
-------------------


Let’s fill our table with some values, so we can do some SELECT queries.
Let’s create a new command. We’ll see later that this process can be made
a bit easier and fasterwith command parameters.


string sql = "insert into highscores (name, score) values ('Me', 9001)";


We create and execute the command the same way as we created the table.
I added two more rows (or records) to the table. Here’s the code:



string sql = "insert into highscores (name, score) values ('Me', 3000)";

SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);

command.ExecuteNonQuery();

sql = "insert into highscores (name, score) values ('Myself', 6000)";

command = new SQLiteCommand(sql, m_dbConnection);

command.ExecuteNonQuery();

sql = "insert into highscores (name, score) values ('And I', 9001)";

command = new SQLiteCommand(sql, m_dbConnection);

command.ExecuteNonQuery();



As you can see, this is three times pretty much the same piece code. But it works!






Getting the high scores out of our database:
---------------------------------------------



Let’s query the database for the high scores sorted on score in descending order.
Our SQL query becomes: “select * from highscores order by score desc”

We create a command in the regular fashion:


string sql = "select * from highscores order by score desc";

SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);


However, we execute this command using a different method,
we’ll use the ExecuteReader() method which returns an SQLiteDataReader object.
We’ll use this object to read the results of the query.




SQLiteDataReader reader = command.ExecuteReader();





With this reader you can read the result row by row.
Here’s some code that iterates trough all the rows and writes them to the console:



string sql = "select * from highscores order by score desc";

SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);

SQLiteDataReader reader = command.ExecuteReader();

while (reader.Read())
       Console.WriteLine
       ("Name: " 
       + reader["name"] 
       + "\tScore: " 
       + reader["score"]);
       
       
The Read() method of the reader moves the reader to the next row.
With the [] operators, you can read the value of a certain column.
The value returned is of the type object.
So you’ll usually need to cast it before you can use it.
Fortunately, you usually know what this type is.



Well, that’s about it for the first part of this tutorial.
You should now be able to do pretty much anything you want with your database.. except transactions.
But that’s something for part 2!



