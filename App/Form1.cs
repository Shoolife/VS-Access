using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using static System.Collections.Specialized.BitVector32;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Diagnostics;

namespace App
{
    public partial class Form1 : Form
    {
        public static string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=DB.mdb";
        private OleDbConnection myConn;
        public Form1()
        {
            InitializeComponent();
            myConn = new OleDbConnection(connectString);
            myConn.Open();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "dBDataSet.Заказ". При необходимости она может быть перемещена или удалена.
            this.заказTableAdapter.Fill(this.dBDataSet.Заказ);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "dBDataSet.Товар". При необходимости она может быть перемещена или удалена.
            this.товарTableAdapter.Fill(this.dBDataSet.Товар);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "dBDataSet.Клиенты". При необходимости она может быть перемещена или удалена.
            this.клиентыTableAdapter.Fill(this.dBDataSet.Клиенты);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "dBDataSet.Заказ". При необходимости она может быть перемещена или удалена.
            this.заказTableAdapter.Fill(this.dBDataSet.Заказ);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            myConn.Close();
        }

        private String Clients(String s)
        {
            string result = "";

            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                if (dataGridView2[1, i].Value.ToString() == s)
                {
                    result = dataGridView2[0, i].Value.ToString();
                    break;
                }
            }
            return result;
        }
        private String Product(String s)
        {
            string result = "";

            for (int i = 0; i < dataGridView3.Rows.Count; i++)
            {
                if (dataGridView3[1, i].Value.ToString() == s)
                {
                    result = dataGridView3[0, i].Value.ToString();
                    break;
                }
            }
            return result;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                Order dialog = new Order();
                dialog.Text = "Добавление заказа";
                dialog.textBox1.Text = "";
                dialog.textBox2.Text = "";
                if (dialog.ShowDialog() == DialogResult.OK)
                {

                    int volume = Convert.ToInt32(dialog.textBox1.Text);
                    string date = dialog.textBox2.Text;

                    string Order = "insert into Заказ(Товар, Объем, Дата, Клиент)values(" + Product(dialog.comboBox1.Text) +
                                    ", '" + volume + "', '" + date + "', " + Clients(dialog.comboBox2.Text) + ")";
                    OleDbCommand command = new OleDbCommand(Order, myConn);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Заказ успешно добавлен в базу! База обновлена!");
                }
                
            }
            else
            {
                if (tabControl1.SelectedIndex == 1)
                {
                    Clients dialog = new Clients();
                    dialog.Text = "Добавление клиента";
                    dialog.textBox1.Text = "";
                    dialog.textBox2.Text = "";
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        string name = dialog.textBox1.Text;
                        string address = dialog.textBox2.Text;

                        string Clients = "insert into Клиенты(Наименование, Адрес)values('" + name + "', '" + address + "')";
                        OleDbCommand command = new OleDbCommand(Clients, myConn);
                        command.ExecuteNonQuery();
                        MessageBox.Show("Клиент успешно добавлен в базу! База обновлена!");
                    }
                }
                else
                {
                    if (tabControl1.SelectedIndex == 2)
                    {

                        Product dialog = new Product();
                        dialog.Text = "Добавление товара";
                        if (dialog.ShowDialog() == DialogResult.OK)
                        {
                            string name = dialog.textBox1.Text;
                            string term = dialog.textBox2.Text;
                            int price = Convert.ToInt32(dialog.textBox3.Text);
                            int availability = Convert.ToInt32(dialog.textBox4.Text);

                            string Product = "insert into Товар(Наименование, [Срок реализации], Цена, [Наличие на складе])values('" + name + "', '" + term + "', '" + price + "', '" + availability + "')";
                            OleDbCommand command = new OleDbCommand(Product, myConn);
                            command.ExecuteNonQuery();
                            MessageBox.Show("Товар успешно добавлен в базу! База обновлена!");
                        }
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                if (dataGridView1.CurrentCell.RowIndex != -1)
                {
                    int SelectedIndex = dataGridView1.CurrentCell.RowIndex;
                    Order dialog = new Order();
                    dialog.Text = "Изменение заказа";
                    dialog.textBox1.Text = dataGridView1[2, SelectedIndex].Value.ToString();
                    dialog.textBox2.Text = dataGridView1[3, SelectedIndex].Value.ToString();

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        int volume = Convert.ToInt32(dialog.textBox1.Text);
                        string date = dialog.textBox2.Text;

                        int kod = Convert.ToInt32(dataGridView1[0, SelectedIndex].Value.ToString());

                        string Order = "update Заказ set Товар = " + Product(dialog.comboBox1.Text) + ", Объем = '" + volume + 
                                        "', Дата = '" + date + "', Клиент = " + Clients(dialog.comboBox2.Text) + " where Код = " + kod + "";
                        OleDbCommand command = new OleDbCommand(Order, myConn);
                        command.ExecuteNonQuery();
                        MessageBox.Show("Заказ успешно изменен в базе! База обновлена!");
                    }
                }
            }
            else
            {
                if (tabControl1.SelectedIndex == 1)
                {
                    if (dataGridView2.CurrentCell.RowIndex != -1)
                    {
                        int SelectedIndex = dataGridView2.CurrentCell.RowIndex;
                        Clients dialog = new Clients();
                        dialog.Text = "Изменение клиента";
                        dialog.textBox1.Text = dataGridView2[1, SelectedIndex].Value.ToString();
                        dialog.textBox2.Text = dataGridView2[2, SelectedIndex].Value.ToString();

                        if (dialog.ShowDialog() == DialogResult.OK)
                        {

                            string name = dialog.textBox1.Text;
                            string address = dialog.textBox2.Text;

                            int kod = Convert.ToInt32(dataGridView2[0, SelectedIndex].Value.ToString());

                            string Clients = "update Клиенты set Наименование = '" + name + "', Адрес = '" + address + "' where Код = " + kod + "";
                            OleDbCommand command = new OleDbCommand(Clients, myConn);
                            command.ExecuteNonQuery();
                            MessageBox.Show("Клиент успешно изменен в базе! База обновлена!");
                        }
                    }
                }
                else
                {
                    if (tabControl1.SelectedIndex == 2)
                    {
                        if (dataGridView3.CurrentCell.RowIndex != -1)
                        {
                            int SelectedIndex = dataGridView3.CurrentCell.RowIndex;
                            Product dialog = new Product();
                            dialog.Text = "Изменение товара";
                            dialog.textBox1.Text = dataGridView3[1, SelectedIndex].Value.ToString();
                            dialog.textBox2.Text = dataGridView3[2, SelectedIndex].Value.ToString();
                            dialog.textBox3.Text = dataGridView3[3, SelectedIndex].Value.ToString();
                            dialog.textBox4.Text = dataGridView3[4, SelectedIndex].Value.ToString();

                            if (dialog.ShowDialog() == DialogResult.OK)
                            {
                                string name = dialog.textBox1.Text;
                                string term = dialog.textBox2.Text;
                                int price = Convert.ToInt32(dialog.textBox3.Text);
                                int availability = Convert.ToInt32(dialog.textBox4.Text);

                                int kod = Convert.ToInt32(dataGridView3[0, SelectedIndex].Value.ToString());

                                string Product = "update Товар set Наименование = '" + name + "', [Срок реализации] = '" + term + "', Цена = '" + price + "' , [Наличие на складе] = '" + availability + "' where Код = " + kod + "";
                                OleDbCommand command = new OleDbCommand(Product, myConn);
                                command.ExecuteNonQuery();
                                MessageBox.Show("Товар успешно изменен в базе! База обновлена!");
                            }
                        }
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                if (dataGridView1.CurrentCell.RowIndex != -1)
                {
                    int SelectedIndex = dataGridView1.CurrentCell.RowIndex;

                    int kod = Convert.ToInt32(dataGridView1[0, SelectedIndex].Value.ToString());

                    string Orders = "delete from Заказ where Код = " + kod + "";
                    OleDbCommand command = new OleDbCommand(Orders, myConn);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Заказ успешно удален из базы! База обновлена!");
                }
            }
            else
            {
                if (tabControl1.SelectedIndex == 1)
                {
                    if (dataGridView2.CurrentCell.RowIndex != -1)
                    {
                        int SelectedIndex = dataGridView2.CurrentCell.RowIndex;

                        int kod = Convert.ToInt32(dataGridView2[0, SelectedIndex].Value.ToString());

                        string Clients = "delete from Клиенты where Код = " + kod + "";
                        OleDbCommand command = new OleDbCommand(Clients, myConn);
                        command.ExecuteNonQuery();
                        MessageBox.Show("Клиент успешно удален из базы! База обновлена!");
                    }
                }
                else
                {
                    if (tabControl1.SelectedIndex == 2)
                    {
                        if (dataGridView3.CurrentCell.RowIndex != -1)
                        {
                            int SelectedIndex = dataGridView3.CurrentCell.RowIndex;

                            int kod = Convert.ToInt32(dataGridView3[0, SelectedIndex].Value.ToString());

                            string Product = "delete from Товар where Код = " + kod + "";
                            OleDbCommand command = new OleDbCommand(Product, myConn);
                            command.ExecuteNonQuery();
                            MessageBox.Show("Товар успешно удален из базы! База обновлена!");
                        }
                    }
                }
            }
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "dBDataSet.Товар". При необходимости она может быть перемещена или удалена.
            this.товарTableAdapter.Fill(this.dBDataSet.Товар);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "dBDataSet.Клиенты". При необходимости она может быть перемещена или удалена.
            this.клиентыTableAdapter.Fill(this.dBDataSet.Клиенты);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "dBDataSet.Заказ". При необходимости она может быть перемещена или удалена.
            this.заказTableAdapter.Fill(this.dBDataSet.Заказ);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string element = comboBox1.Text;

            using (OleDbConnection connection = new OleDbConnection(connectString))
            {
                
                string sqlQuery = "SELECT Клиенты.Наименование, Товар.Наименование, Заказ.Дата, Заказ.Объем " +
                                  "FROM Товар " +
                                  "INNER JOIN (Клиенты INNER JOIN Заказ ON Клиенты.Код = Заказ.Клиент) ON Товар.Код = Заказ.Товар " +
                                  "WHERE(((Клиенты.Наименование) = '" + element + "'))";




                OleDbCommand command = new OleDbCommand(sqlQuery, connection);

                OleDbDataAdapter adapter = new OleDbDataAdapter(command);

                DataSet dataSet = new DataSet();

                connection.Open();

                adapter.Fill(dataSet);

                connection.Close();

                dataGridView5.DataSource = dataSet.Tables[0];
            }
        }
    }
}
