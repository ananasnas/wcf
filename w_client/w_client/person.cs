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
    public partial class person : Form
    {
        public int id_group { get; set; }
        public person()
        {
            InitializeComponent();
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.AllowUserToDeleteRows = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            w_client.OperationChangesRef.changes ch = new w_client.OperationChangesRef.changes();
            ch.ID_person = Convert.ToInt32(this.textBox1.Text);
            for (int i = 0; i <= dataGridView2.RowCount - 1; i++)
            {
                try
                {
                    if (dataGridView2.Rows[i].Cells[2].Value.ToString() == "True")
                    {
                        if (ch.IDs_privch != null)
                        {
                            ch.IDs_privch = ch.IDs_privch + ", " + dataGridView2.Rows[i].Cells[0].Value;
                        }
                        else
                        {
                            ch.IDs_privch = dataGridView2.Rows[i].Cells[0].Value.ToString();
                        }
                    }
                }
                catch { }
            }

            Form2 a = new Form2();
            w_client.OperationChangesRef.Group_ g = new w_client.OperationChangesRef.Group_();
            g.name = comboBox1.SelectedItem.ToString();
            g.tableName = "data_base.groups";
            List<object> s = a.GetL(g);
            g = (w_client.OperationChangesRef.Group_)s[0];
            ch.ID_group = g.ID_group;
            ch.surname = textBox2.Text;
            ch.name = textBox3.Text;
        
            ch.middle_name = textBox4.Text;
            OperationChangesClient operationclient = new OperationChangesClient("BasicHttpBinding_IOperationChanges");

            int flag = operationclient.update_per_priv(ch);
            int flag_ = operationclient.update_pers(ch);
            if ((flag == 1) && (flag_ == 1))
            {
                MessageBox.Show("Операция прошла успешно.");
            }
            else
            {
                MessageBox.Show("Ошибка! Попробуйте еще раз.");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Form2 ad = new Form2();
            Dictionary<string, int> namegr_id = new Dictionary<string, int>();
            w_client.OperationChangesRef.Group_ gr = new w_client.OperationChangesRef.Group_();
            gr.tableName = "data_base.groups";
            List<object> g = ad.GetL(gr);
            foreach (object g_ in g)
            {
                w_client.OperationChangesRef.Group_ group = (w_client.OperationChangesRef.Group_)g_;
                try
                {
                    namegr_id.Add(group.name, group.ID_group);
                }
                catch { }
            }

            w_client.OperationChangesRef.Group_Privileges gp = new w_client.OperationChangesRef.Group_Privileges();
            gp.Groups_ID_group = namegr_id[comboBox1.SelectedItem.ToString()];
            gp.tableName = "data_base.groups_privileges";
            List<object> grp = ad.GetL(gp);
            List<w_client.OperationChangesRef.Group_Privileges> r = new List<w_client.OperationChangesRef.Group_Privileges>();
            foreach (object g_ in grp)
            { r.Add((Group_Privileges)g_); }

            Dictionary<int, string> id_privname = new Dictionary<int, string>();
            w_client.OperationChangesRef.Privilege pg = new w_client.OperationChangesRef.Privilege(); // все существующие привилегии
            pg.tableName = "data_base.privileges";
            List<object> up = ad.GetL(pg);
            List<w_client.OperationChangesRef.Privilege> pl = new List<w_client.OperationChangesRef.Privilege>();
            foreach (object u in up)
            {
                w_client.OperationChangesRef.Privilege u_ = (w_client.OperationChangesRef.Privilege)u;
                id_privname.Add(u_.ID_privilege, u_.name_func);
                pl.Add(u_);
            }

            dataGridView1.Rows.Clear();
            foreach (w_client.OperationChangesRef.Group_Privileges gp0 in r)
            {
                dataGridView1.Rows.Add(gp0.Privileges_ID_privilege.ToString(), id_privname[gp0.Privileges_ID_privilege]);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Regex regex = new Regex("^[А-Яа-я]+$");
            if (regex.IsMatch(textBox2.Text) && regex.IsMatch(textBox3.Text) && regex.IsMatch(textBox4.Text))
            {
                Form2 ad = new Form2();
                w_client.OperationChangesRef.changes ch = new w_client.OperationChangesRef.changes();
                ch.surname = textBox2.Text;
                ch.name = textBox3.Text;
                ch.middle_name = textBox4.Text;

                Dictionary<string, int> namegr_id = new Dictionary<string, int>();
                w_client.OperationChangesRef.Group_ gr = new w_client.OperationChangesRef.Group_();
                gr.tableName = "data_base.groups";
                List<object> g = ad.GetL(gr);
                foreach (object g_ in g)
                {
                    w_client.OperationChangesRef.Group_ group = (w_client.OperationChangesRef.Group_)g_;
                    try
                    { namegr_id.Add(group.name, group.ID_group); }
                    catch { }
                }
                ch.ID_group = namegr_id[comboBox1.SelectedItem.ToString()];

                OperationChangesClient operationclient = new OperationChangesClient("BasicHttpBinding_IOperationChanges");
                int flag = operationclient.add_pers(ch);
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
