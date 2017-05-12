Setup
-----
Run build.cmd, this should create the following folder:

- **Build/Database** - This is where the Database.dacpac lives
- **Build/Server** - This is where the server.exe lives

Create a new database on you sql server and [apply](https://docs.microsoft.com/en-us/sql/relational-databases/data-tier-applications/deploy-a-data-tier-application) the Database.dacpac

Create a new file in the root called `run.cmd` with the following contents:

```
build\Server\Server --home-folder ../../src/Client/wwwroot --conntection-string "Data Source=.; Integrated Security=True; Initial Catalog=SampleData"
```

Changing `--conntection-string` to point at the database you created

Execute `run.cmd`

Development workflow
--------------------

I like to change the `CompileConnectionString` in `src\RepositorySql\Common.fs` to point at my local development database, this way I can work in SQL Manage Studio and when I compile it reflects my active database.

Once I have a batch of changes I pull them into the `dacpac` using **Schema Compare** in in the Database project.


Artifacts
----------

Why is there an `Artifacts/Server.1.0.nupkg` file after a build - we use [Octopus Deploy](https://octopus.com) so I wanted to also demo this.