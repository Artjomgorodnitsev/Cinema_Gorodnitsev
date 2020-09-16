using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
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
        public Form1()
        {
            InitializeComponent();
        }
        Label[,] _arr = new Label[4, 4];
        Label[] read = new Label[4];
        Button osta;
        Button kinni;
        StreamWriter to_file;
        bool ost = false;
        List<string> arr_pilet;

        private void Form1_Load(object sender, EventArgs e)
        {
            string text = "";
            StreamWriter to_file;
            if (!File.Exists("TextFile1.txt"))
            {
                to_file = new StreamWriter("TextFile1.txt", false);
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
            StreamReader from_file = new StreamReader("TextFile1.txt", false);
            string[] arr = from_file.ReadToEnd().Split('\n');


            for (int i = 0; i < 4; i++)
            {
                read[i] = new Label();
                read[i].Text = "Ряд " + (i + 1);
                read[i].Size = new Size(70, 70);
                read[i].Location = new Point(1, i * 70);
                this.Controls.Add(read[i]);
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

                    _arr[i, j].Text = "Место " + (j + 1);
                    _arr[i, j].Size = new Size(70, 70);
                    _arr[i, j].BackColor = Color.Green;
                    _arr[i, j].BorderStyle = BorderStyle.Fixed3D;
                    _arr[i, j].Location = new Point(j * 70 + 70, i * 70);
                    this.Controls.Add(_arr[i, j]);
                    _arr[i, j].Tag = new int[] { i, j };
                    _arr[i, j].Click += new System.EventHandler(Form1_Click);

                }
            }
            from_file.Close();

            osta = new Button();
            osta.Text = "Купить";
            osta.Location = new Point(275, 290);
            this.Controls.Add(osta);
            osta.Click += Osta_Click;

            kinni = new Button();
            kinni.Text = "Подтвердить";
            kinni.Location = new Point(100, 290);
            this.Controls.Add(kinni);
            kinni.Click += Kinni_Click; ;

        }

        private void Kinni_Click(object sender, EventArgs e)
        {
            string text = "";
            to_file = new StreamWriter("TextFile1.txt", false);
            for (int i = 0; i < 4; i++)
            {

                for (int j = 0; j < 4; j++)
                {
                    if (_arr[i, j].BackColor == Color.Yellow)
                    {
                        Osta_Click_Func();
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

        private void Osta_Click_Func()
        {
            arr_pilet = new List<string>();
            var vastus = MessageBox.Show("Вы уверены в выбраных местах?", "Appolo спрашивает", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ost == true)
            {
                if (vastus == DialogResult.Yes)
                {
                    //int t=0;
                    for (int i = 0; i < 4; i++)
                    {

                        for (int j = 0; j < 4; j++)
                        {
                            if (_arr[i, j].BackColor == Color.Yellow)
                            {
                                //t++;
                                _arr[i, j].BackColor = Color.Red;
                                /*StreamWriter pilet = new StreamWriter("Билет "+ t.ToString() +"Ряд " +i.ToString() + "Место"+j.ToString() + ".txt");
                                arr_pilet[t-1]="Билет " + t.ToString() + "Ряд " + i.ToString() + "Место" + j.ToString();
                                pilet.WriteLine("Билет "+ t.ToString() +"Ряд " +i.ToString() + "Место"+j.ToString() + ".txt");
                                pilet.Close();*/
                            }

                        }
                    }
                    

                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {

                        for (int j = 0; j < 4; j++)
                        {
                            if (_arr[i, j].BackColor == Color.Yellow)
                            {
                                _arr[i, j].BackColor = Color.Green;
                                _arr[i, j].Text = "Место " + (j + 1);
                                ost = false;
                            }

                        }
                    }
                }
            }
            else { MessageBox.Show("Надо выбрать место"); }


        }

        private void Osta_Click(object sender, EventArgs e)
        {
            Osta_Click_Func();
        }

        /*void Pilet_Saada(int i, int j)
        {
            string adress = Interaction.InputBox("Введи эмаил", "Куда отослать?", "artemgorodnitsev@gmail.com");
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtpClient = new SmtpClient("smpt.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("mvc.programmeerimine@gmail.com", "3.Kuursus"),                    
                    EnableSsl = true
                };
                mail.From = new MailAddress("mvc.programmeerimine@gmail.com");
                mail.To.Add(adress);
                mail.Subject = "Pilet";
                mail.Body = "Ряд " + i + " Место" + j;
                foreach (var item in arr_pilet)
                {
                    mail.Attachments.Add(new Attachment(item));
                }
                smtpClient.Send(mail);
                MessageBox.Show("Билеты были отправлены на почту: " + mail);
            }
            catch (Exception)
            {
                MessageBox.Show("Билеты отправлены");
            }
        }*/

        private void Form1_Click(object sender, EventArgs e)
        {
            string message = "Эти места заняты";
            var label = (Label)sender;
            var tag = (int[])label.Tag;
            if (_arr[tag[0], tag[1]].BackColor != Color.Red)
            {
                _arr[tag[0], tag[1]].Text = "Выбрано";
                _arr[tag[0], tag[1]].BackColor = Color.Yellow;
                ost = true;
            }
            else
            {
                MessageBox.Show(message);
            }



        }


    }
}
