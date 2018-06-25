using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;
using System.Collections.Specialized;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Management;
using System.Text.RegularExpressions;
//using libchange;
using w_client.OperationChangesRef;

namespace w_client
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            GetTable();
           
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;

            dataGridView2.ReadOnly = true;
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.AllowUserToDeleteRows = false;  
        }
        public string tableName { get; set; }
        public List<object> GetL(object filter)
        {
          
            OperationChangesClient operationclient = new OperationChangesClient("BasicHttpBinding_IOperationChanges");
            object[] o = operationclient.GetList(filter);
            List<object> lo = o.ToList();
            return lo;
        }
        public void GetTable()
        {
            this.dataGridView1.Rows.Clear();
            w_client.OperationChangesRef.Person p = new w_client.OperationChangesRef.Person();
            p.tableName = "data_base.persons";
            List<object> list = GetL(p);
            foreach (object person_ in list)
            {
                Person obj = (Person)person_;
                if (obj.WasDel != 1)
                {
                    this.dataGridView1.Rows.Add(obj.ID_person, obj.surname, obj.name, obj.middle_name, "Привилегии");
                }
            }

            this.dataGridView2.Rows.Clear();
            w_client.OperationChangesRef.Group_ g = new w_client.OperationChangesRef.Group_();
            g.tableName = "data_base.groups";
            List<object> gr = GetL(g);
            foreach (object g_ in gr)
            {
                Group_ v = (Group_)g_;
                this.dataGridView2.Rows.Add(v.ID_group, v.name, "Привилегии");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentCell.ColumnIndex == 4)
            {
                person personForm = new person();
                personForm.button1.Hide();
                personForm.textBox1.Text = this.dataGridView1.CurrentRow.Cells[0].Value.ToString();
                personForm.textBox2.Text = this.dataGridView1.CurrentRow.Cells[1].Value.ToString();
                personForm.textBox3.Text = this.dataGridView1.CurrentRow.Cells[2].Value.ToString();
                personForm.textBox4.Text = this.dataGridView1.CurrentRow.Cells[3].Value.ToString();

                w_client.OperationChangesRef.Person p = new w_client.OperationChangesRef.Person();
                p.ID_person = Convert.ToInt16(this.dataGridView1.CurrentRow.Cells[0].Value);
                p.tableName = "data_base.persons";
                List<object> pn = GetL(p);
                p = (w_client.OperationChangesRef.Person)pn[0];

                Dictionary<int, string> id_namegr = new Dictionary<int, string>();
                Dictionary<string, int> namegr_id = new Dictionary<string, int>();
                Group_ gr = new Group_();
                gr.tableName = "data_base.groups";
                List<object> g = GetL(gr);
                foreach (object g_ in g)
                {
                    Group_ group = (Group_)g_;
                    personForm.comboBox1.Items.Add(group.name.ToString());
                    id_namegr.Add(group.ID_group, group.name);
                    try
                    {
                        namegr_id.Add(group.name, group.ID_group);
                    }
                    catch { }
                }
                personForm.comboBox1.SelectedIndex = personForm.comboBox1.FindStringExact(id_namegr[p.Groups_ID_Group]);

                Dictionary<int, string> id_privname = new Dictionary<int, string>();
                w_client.OperationChangesRef.Privilege pg = new w_client.OperationChangesRef.Privilege(); // все существующие привилегии
                pg.tableName = "data_base.privileges";
                List<object> up = GetL(pg);
                List<w_client.OperationChangesRef.Privilege> pl = new List<w_client.OperationChangesRef.Privilege>();
                foreach (object u in up)
                {
                    w_client.OperationChangesRef.Privilege u_ = (w_client.OperationChangesRef.Privilege)u;
                    id_privname.Add(u_.ID_privilege, u_.name_func);
                    pl.Add(u_);
                }

                w_client.OperationChangesRef.Group_Privileges gp = new w_client.OperationChangesRef.Group_Privileges();
                gp.Groups_ID_group = namegr_id[personForm.comboBox1.SelectedItem.ToString()];
                gp.tableName = "data_base.groups_privileges";
                List<object> grp = GetL(gp);
                List<w_client.OperationChangesRef.Group_Privileges> r = new List<w_client.OperationChangesRef.Group_Privileges>();
                foreach (object g_ in grp)
                { r.Add((w_client.OperationChangesRef.Group_Privileges)g_); }


                w_client.OperationChangesRef.Persons_Privileges pp = new w_client.OperationChangesRef.Persons_Privileges();
                pp.Persons_ID_person = Convert.ToInt32(personForm.textBox1.Text);
                pp.tableName = "data_base.persons_privileges";
                List<object> j = GetL(pp);
                List<w_client.OperationChangesRef.Persons_Privileges> pi = new List<w_client.OperationChangesRef.Persons_Privileges>();
                foreach (object j_ in j)
                {
                    w_client.OperationChangesRef.Persons_Privileges j0 = (w_client.OperationChangesRef.Persons_Privileges)j_;
                    pi.Add(j0);
                }

                foreach (w_client.OperationChangesRef.Persons_Privileges p_p in pi)
                {
                    personForm.dataGridView2.Rows.Add(p_p.Privileges_ID_privilege, id_privname[p_p.Privileges_ID_privilege], true);
                }

                foreach (w_client.OperationChangesRef.Privilege pt in pl)
                {
                    int flag = 0;
                    for (int i = 0; i <= personForm.dataGridView1.RowCount - 1; i++)
                    {
                        if (pt.ID_privilege.ToString() == personForm.dataGridView1.Rows[i].Cells[0].Value.ToString())
                        {
                            flag = 1;
                        }
                    }

                    for (int i = 0; i <= personForm.dataGridView2.RowCount - 1; i++)
                    {
                        if (pt.ID_privilege.ToString() == personForm.dataGridView2.Rows[i].Cells[0].Value.ToString())
                        {
                            flag = 1;
                        }
                    }

                    if (flag == 0)
                    {
                        personForm.dataGridView2.Rows.Add(pt.ID_privilege, id_privname[pt.ID_privilege]);
                    }
                }
                personForm.ShowDialog();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            person p = new person();
            p.dataGridView1.Rows.Clear();
            p.dataGridView2.Hide();
            p.label7.Text = "*Дополнительные привилегии будут доступны после создания человека.";
            Dictionary<string, int> namegr_id = new Dictionary<string, int>();
            Group_ gr = new Group_();
            gr.tableName = "data_base.groups";
            List<object> g = GetL(gr);
            foreach (object g_ in g)
            {
                Group_ group = (Group_)g_;
                p.comboBox1.Items.Add(group.name.ToString());
                try
                { namegr_id.Add(group.name, group.ID_group); }
                catch { }
            }
            p.comboBox1.SelectedIndex = 1;

            Dictionary<int, string> id_privname = new Dictionary<int, string>();
            w_client.OperationChangesRef.Privilege pg = new w_client.OperationChangesRef.Privilege(); // все существующие привилегии
            pg.tableName = "data_base.privileges";
            List<object> up = GetL(pg);
            foreach (object u in up)
            {
                w_client.OperationChangesRef.Privilege u_ = (w_client.OperationChangesRef.Privilege)u;
                id_privname.Add(u_.ID_privilege, u_.name_func);
            }

            w_client.OperationChangesRef.Group_Privileges gp = new w_client.OperationChangesRef.Group_Privileges();
            gp.Groups_ID_group = namegr_id[p.comboBox1.SelectedItem.ToString()];
            gp.tableName = "data_base.groups_privileges";
            List<object> grp = GetL(gp);
            List<w_client.OperationChangesRef.Group_Privileges> r = new List<w_client.OperationChangesRef.Group_Privileges>();
            foreach (object g_ in grp)
            { r.Add((w_client.OperationChangesRef.Group_Privileges)g_); }

            foreach (object u in up)
            {
                int flag = 0;
                w_client.OperationChangesRef.Privilege u_ = (w_client.OperationChangesRef.Privilege)u;
                for (int i = 0; i <= p.dataGridView1.RowCount - 1; i++)
                {
                    if (u_.ID_privilege.ToString() == p.dataGridView1.Rows[i].Cells[0].Value.ToString())
                    {
                        flag = 1;
                    }
                }
                if (flag == 0)
                {
                    p.dataGridView2.Rows.Add(u_.ID_privilege, u_.name_func);
                }
            }

            p.button1.Show();
            p.button2.Show();
            p.button3.Hide();
            p.ShowDialog();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.CurrentCell.ColumnIndex == 2)
            {
                Form4 f = new Form4();
                f.button1.Hide();
                f.textBox1.Text = this.dataGridView2.CurrentRow.Cells[0].Value.ToString();
                f.textBox2.Text = this.dataGridView2.CurrentRow.Cells[1].Value.ToString();

                Dictionary<int, string> id_privname = new Dictionary<int, string>();

                w_client.OperationChangesRef.Privilege pg = new w_client.OperationChangesRef.Privilege(); // все существующие привилегии
                pg.tableName = "data_base.privileges";
                List<object> up = GetL(pg);
                List<w_client.OperationChangesRef.Privilege> pl = new List<w_client.OperationChangesRef.Privilege>();
                foreach (object u in up)
                {
                    w_client.OperationChangesRef.Privilege u_ = (w_client.OperationChangesRef.Privilege)u;
                    id_privname.Add(u_.ID_privilege, u_.name_func);
                    pl.Add(u_);
                }

                w_client.OperationChangesRef.Group_Privileges gp = new w_client.OperationChangesRef.Group_Privileges();
                gp.Groups_ID_group = Convert.ToInt32(f.textBox1.Text);
                gp.tableName = "data_base.groups_privileges";
                List<object> grp = GetL(gp);
                List<w_client.OperationChangesRef.Group_Privileges> r = new List<w_client.OperationChangesRef.Group_Privileges>();
                foreach (object g_ in grp)
                { r.Add((w_client.OperationChangesRef.Group_Privileges)g_); }

                foreach (w_client.OperationChangesRef.Group_Privileges gp0 in r)
                {
                    f.dataGridView1.Rows.Add(gp0.Privileges_ID_privilege.ToString(), id_privname[gp0.Privileges_ID_privilege], true);
                }

                foreach (w_client.OperationChangesRef.Privilege p in pl)
                {
                    int flag = 0;
                    for (int i = 0; i < f.dataGridView1.Rows.Count; i++)
                    {
                        if (p.ID_privilege == Convert.ToInt32(f.dataGridView1.Rows[i].Cells[0].Value))
                        {
                            flag = 1;
                        }
                    }
                    if (flag == 0)
                    {
                        f.dataGridView1.Rows.Add(p.ID_privilege, p.name_func, false);
                    }
                }
                f.ShowDialog();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            GetTable();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form5 nf = new Form5();
            nf.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form3 f = new Form3();
            f.ShowDialog();
        }
    }
}
