# Database-first application with relational database. A collab project by Robin Björkil & Jesper Stranne

To run the application, simply follow the steps below:

1. Download and restore attached .BAK-file "Bookshop.BAK" in SQLServer.

2. Build the repository project in Visual Studio and go to "Manage user Secrets" to create a secrets.json class that contains the following key: ["ConnectionString"] - which points to your chosen server in SQLServer. Here's a template for how your secrets.json file should look like:

{
  "ConnectionString": "Server=Your_Server_Name;Initial Catalog=Your_Data_Base;Integrated Security=True;TrustServerCertificate=True"
}

Change "Your_Server_Name" with your chosen SQLServer.
Change "Your_Data_Base" with the name of the database you want to connect to.

3. Run the project in Visual Studio.


See the following picture if you're unsure where to find "Manage User Secrets"
![image](https://github.com/user-attachments/assets/b2ab42b9-eb4d-4e11-be59-591e11da2222)


User Secrets Guide: https://dontpaniclabs.com/blog/post/2023/03/02/how-to-set-up-user-secrets-for-net-core-projects-in-visual-studio/

