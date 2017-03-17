using System;
using Gtk;
using System.Web.Script.Serialization;
using System.Collections;
using System.Net;
using MySql.Data.MySqlClient;
using System.Data;

public partial class MainWindow : Gtk.Window
{
	private MySql.Data.MySqlClient.MySqlConnection connection;

	public MainWindow() : base(Gtk.WindowType.Toplevel)
	{
		Build();
	}

	protected void OnDeleteEvent(object sender, DeleteEventArgs a)
	{
		Application.Quit();
		a.RetVal = true;
	}

	protected void OnButton1Clicked(object sender, EventArgs e)
	{

		sync();

	}

		private void sync()
	{
		var address = "http://hms.dev/api/test";
		var json = new JavaScriptSerializer().Serialize(
			new
			{
				test = ""
			}
		);
		var cli = new WebClient();
		cli.Headers[HttpRequestHeader.ContentType] = "application/json";
		string response = cli.UploadString(address, json.ToString());
		Console.WriteLine(response);

		var connectionString = "Server=localhost;database=db_HMS_temp;UID=root;password=123456";
		connection = new MySql.Data.MySqlClient.MySqlConnection(connectionString);
		connection.Open();

		string query = "SELECT * FROM tbl_hospital WHERE sync=0";
		var cmd = new MySqlCommand(query, connection);
		var reader = cmd.ExecuteReader();
		var hospitals = new ArrayList();
		while (reader.Read()) {
			Console.WriteLine(reader.GetString("hospitalid"));
			hospitals.Add(
				new
				{
					hospitalid = reader.GetString("hospitalid"),
					hospitaldescription = reader.GetString("hospitaldescription"),
					remarks = reader.GetString("remarks"),
					usercode = reader.GetString("usercode"),
					
					dateentry = reader.GetString("dateentry")
				}
			);
		}

		var tables = new ArrayList();
		tables.Add(fetchPatients());

		var jsonString = new JavaScriptSerializer().Serialize(
			new { 
				hospital = new { 
					hospitalid = "HCAMSUR0001",
					hospitaldescription = "RAGAY DISTRICT HOSPITAL",
					remarks = "RAGAY",
					status = "ACTIVE",
					dateentry = "2017-01-31 14:20:38"
				},
				tables = tables
			}
		);
	}

	private ArrayList fetchPatients()
	{
		var patients = new ArrayList();
		connection.Open();
		string query = "SELECT * FROM patients WHERE sync = 0";
		var cmd = new MySqlCommand(query, connection);
		var reader = cmd.ExecuteReader();
		while (reader.Read())
		{
			patients.Add(
				new
				{
					patient = reader.GetString("patient"),
					firstname = reader.GetString("firstname"),
					middlename = reader.GetString("middlename"),
					lastname = reader.GetString("lastname"),
					extension = reader.GetString("extension"),
					age = reader.GetString("age"),
					desc = reader.GetString("desc"),
					gender = reader.GetString("gender"),
					address = reader.GetString("address"),
					contact = reader.GetString("contact"),
					bloodtype = reader.GetString("bloodtype"),
					height = reader.GetString("height"),
				weight = reader.GetString("weight"),
					fathername = reader.GetString("fathername"),
				mothername = reader.GetString("mothername"),
					fatherstat = reader.GetString("fatherstat"),
				motherstat = reader.GetString("motherstat"),
				parentstat = reader.GetString("parentstat"),
				insurancestat = reader.GetString("insurancestat"),
				photo = reader.GetString("photo"),
				remarks = reader.GetString("remarks"),
				date = reader.GetString("date"),

				
				}
			);
		}
		return patients;
	}
}
