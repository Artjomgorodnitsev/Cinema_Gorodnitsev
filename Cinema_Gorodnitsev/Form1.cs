using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;

namespace Cinema_Gorodnitsev
{
    public partial class Form1 : Form
    {
        SqlConnection sqlCon = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\AppData\Database1.mdf;Integrated Security=True");
        int Id = 0;
        public Form1()
        {
            InitializeComponent();
        }
        StreamWriter to_file;
        Label[,] _arr = new Label[4, 4];
        Label[] rida = new Label[4];
        Button btn, btnk, btnDB;
        bool ost = false;
        public string text;


        private void Form1_Load(object sender, EventArgs e)
        {
            string text = "";
            StreamWriter to_file;
            if (!File.Exists("Kino.txt"))
            {
                to_file = new StreamWriter("Kino.txt", false);
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        text += i + "," + j + ",false;";
                    }
                    text += "\n";
                }
                to_file.Write(text);
                to_file.Close();
            }
            StreamReader from_file = new StreamReader("Kino.txt", false);
            string[] arr = from_file.ReadToEnd().Split('\n');
            from_file.Close();
            this.Size = new Size(500, 430);
            this.Text = "kino";


            for (int i = 0; i < 4; i++)
            {
                rida[i] = new Label();
                rida[i].Text = "Rida" + (i + 1);
                rida[i].Size = new Size(50, 50);

                rida[i].Location = new Point(1, i * 50);
                this.Controls.Add(rida[i]);

                for (int j = 0; j < 4; j++)
                {
                    _arr[i, j] = new Label();
                    string[] arv = arr[i].Split(';');
                    string[] ardNum = arv[j].Split(',');
                    if (ardNum[2] == "true")
                    {
                        _arr[i, j].BackColor = Color.Red;
                    }
                    else
                    {
                        _arr[i, j].BackColor = Color.Green;
                    }
                    _arr[i, j].Text = "Koht" + (j + 1);
                    _arr[i, j].Size = new Size(50, 50);
                    _arr[i, j].BorderStyle = BorderStyle.Fixed3D;
                    _arr[i, j].Location = new Point(j * 50 + 50, i * 50);
                    this.Controls.Add(_arr[i, j]);
                    _arr[i, j].Tag = new int[] { i, j };
                    _arr[i, j].Click += new System.EventHandler(Form1_Click);
                }
            }
            btn = new Button();
            btn.Text = "Osta";
            btn.Location = new Point(176, 200);
            btn.Click += Btn_Click;
            this.Controls.Add(btn);
            btnk = new Button();
            btnk.Text = "Kinni";
            btnk.Location = new Point(1, 200);
            this.Controls.Add(btnk);
            btnk.Click += Btnk_Click;
            btnDB = new Button();
            btnDB.Text = "В базу";
            btnDB.Location = new Point(1, 500);
            this.Controls.Add(btnDB);
            btnDB.Click += Btnk_Click1; ;


        }

        private void Btnk_Click1(object sender, EventArgs e)
        {
            
        }

        public void Btnk_Click(object sender, EventArgs e)
        {
            string text = "";
            try
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (_arr[i, j].BackColor == Color.Yellow)
                        {
                            Btn_Click_Func();
                        }
                    }

                }
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (_arr[i, j].BackColor == Color.Red)
                        {
                            text += i + "," + j + ",true;";
                        }
                        else
                        {
                            text += i + "," + j + ",false;";
                        }
                    }
                    text += "\n";




                }
                

                to_file.Write(text);
                to_file.Close();
                this.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Message");
            }
            finally
            {
                sqlCon.Close();
            }
            to_file = new StreamWriter("Kino.txt", false);
            
            




        }
        public void Btn_Click_Func()
        {
            DialogResult result = MessageBox.Show("Kas te olete kindel, et soovite osta pilet?", "Pileti ostamine",
            MessageBoxButtons.YesNoCancel);
            if (result == DialogResult.Yes)
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (_arr[i, j].BackColor == Color.Yellow)
                        {
                            _arr[i, j].BackColor = Color.Red;
                            Insert_To_DataBase(i,j);

                        }
                    }
                }

            }
            if (result == DialogResult.No)
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (_arr[i, j].BackColor == Color.Yellow)
                        {
                            _arr[i, j].Text = "Koht" + (j + 1);
                            _arr[i, j].BackColor = Color.Green;
                            ost = false;
                        }

                    }
                }
            }
            else
            {
                DialogResult result2 = MessageBox.Show("Kas te soovite piletid emaili saada?", "Pileti ostamine",
                MessageBoxButtons.YesNoCancel);
                if (result2 == DialogResult.Yes)
                {

                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                    string maill = "";
                    ShowInputDialog(ref maill);

                    mail.From = new MailAddress("vlrptv@gmail.com");

                    mail.To.Add(maill);
                    mail.Subject = "Teie pilet. Kõik head!";
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            if (_arr[i, j].BackColor == Color.Red)
                            {
                                text += "Rida: " + (i + 1) + "; Koht: " + (j + 1) + "<br>";
                            }

                        }
                    }
                    mail.Body = text;
                    mail.IsBodyHtml = true;


                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("artem228z@mail.ru", "");
                    SmtpServer.EnableSsl = true;

                    SmtpServer.Send(mail);
                    MessageBox.Show("Mail saadetud");

                }
                if (result2 == DialogResult.No)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            if (_arr[i, j].BackColor == Color.Yellow)
                            {
                                _arr[i, j].Text = "Koht" + (j + 1);
                                _arr[i, j].BackColor = Color.Green;
                                ost = false;
                            }

                        }
                    }
                }
            }

        }


        public void Insert_To_DataBase(int i,int j)
        {
            string conenectionString;
            SqlConnection con;
            conenectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\opilane\Source\Repos\Cinema_Gorodnitsev4\Cinema_Gorodnitsev\AppData\Database1.mdf;Integrated Security=True";
            con = new SqlConnection(conenectionString);
            con.Open();
            SqlCommand command;
            string sql = "INSERT INTO Piletid(Id,rida,koht) VALUES("+(i+1)+","+(j+1)+")";
            command = new SqlCommand(sql, con);
            command.ExecuteNonQuery();
            command.Dispose();
            con.Close();

        }


        private static DialogResult ShowInputDialog(ref string input)
        {
            System.Drawing.Size size = new System.Drawing.Size(200, 70);
            Form inputBox = new Form();

            inputBox.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            inputBox.ClientSize = size;
            inputBox.Text = "Email";

            System.Windows.Forms.TextBox textBox = new TextBox();
            textBox.Size = new System.Drawing.Size(size.Width - 10, 23);
            textBox.Location = new System.Drawing.Point(5, 5);
            textBox.Text = input;
            inputBox.Controls.Add(textBox);

            Button okButton = new Button();
            okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            okButton.Name = "okButton";
            okButton.Size = new System.Drawing.Size(75, 23);
            okButton.Text = "&OK";
            okButton.Location = new System.Drawing.Point(size.Width - 80 - 80, 39);
            inputBox.Controls.Add(okButton);

            Button cancelButton = new Button();
            cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(75, 23);
            cancelButton.Text = "&Cancel";
            cancelButton.Location = new System.Drawing.Point(size.Width - 80, 39);
            inputBox.Controls.Add(cancelButton);

            inputBox.AcceptButton = okButton;
            inputBox.CancelButton = cancelButton;

            DialogResult result = inputBox.ShowDialog();
            input = textBox.Text;
            return result;
        }
        public void Btn_Click(object sender, EventArgs e)
        {
            Btn_Click_Func();
            

        }

        private void Form1_Click(object sender, EventArgs e)
        {
            var label = (Label)sender;
            var tag = (int[])label.Tag;
            if (_arr[tag[0], tag[1]].BackColor == Color.Green)
            {
                _arr[tag[0], tag[1]].Text = "kinni";
                _arr[tag[0], tag[1]].BackColor = Color.Yellow;
                ost = true;

            }
            if (_arr[tag[0], tag[1]].BackColor == Color.Red)

            {
                string message = "See koht juba ostatud";
                string title = "Error";
                MessageBox.Show(message, title);

            }

        }
    }
}
