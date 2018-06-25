using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Regex regex = new Regex("^[А-Яа-я]+$");
            if (regex.IsMatch(textBox1.Text))
            {
                w_client.OperationChangesRef.changes ch = new w_client.OperationChangesRef.changes();
                ch.name_priv = textBox1.Text;
                ch.IDs_privgr = "-1";
              
                OperationChangesClient operationclient = new OperationChangesClient("BasicHttpBinding_IOperationChanges");
                int flag = operationclient.add_priv(ch);
                if (flag == 1)
                {
                    MessageBox.Show("Операция прошла успешно.");
                }
                else
                {
                    MessageBox.Show("Ошибка! Попробуйте еще раз.");
                }
                this.Close();
            }
            else
            {
                MessageBox.Show("Некорректный ввод.");
            }
        }
    }
}
