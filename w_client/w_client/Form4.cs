using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using libchange;
using System.Collections.Specialized;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Threading;
using System.Configuration;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using w_client.OperationChangesRef;

namespace w_client
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Regex regex = new Regex("^[А-Яа-я]+$");
            if (regex.IsMatch(textBox2.Text))
            {
                changes ch = new changes();
                ch.ID_group = Convert.ToInt32(textBox1.Text);
                ch.name_group = textBox2.Text;
                try
                {
                    for (int i = 0; i <= dataGridView1.RowCount - 1; i++)
                    {
                        if (dataGridView1.Rows[i].Cells[2].Value.ToString() == "True")
                        {
                            if (ch.IDs_privgr != null)
                            {
                                ch.IDs_privgr = ch.IDs_privgr + ", " + dataGridView1.Rows[i].Cells[0].Value;
                            }
                            else
                            {
                                ch.IDs_privgr = dataGridView1.Rows[i].Cells[0].Value.ToString();
                            }
                        }
                    }
                }
                catch { }
                Form2 ad = new Form2();
                ch.ID_person = -1;
                ch.name = "-1";
                //admin ad = new admin();             
                OperationChangesClient operationclient = new OperationChangesClient("BasicHttpBinding_IOperationChanges");
                int flag = operationclient.update_groups(ch);
                if (flag == 1)
                {
                    MessageBox.Show("Операция прошла успешно.");
                }
                else
                {
                    MessageBox.Show("Ошибка! Попробуйте еще раз.");
                }
            }
            else
            {
                MessageBox.Show("Ошибка! Некорректный ввод.");
            }
        }

        
    }
}
