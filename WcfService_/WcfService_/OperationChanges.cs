using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Reflection;


namespace WcfService_
{
   // [ServiceBehavior(IncludeExceptionDetailInFaults=true)]
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class OperationChanges : IOperationChanges
    {
        public int add_priv(changes ch)
        {
            using (MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySQLCon"].ConnectionString))
            {
                try
                {
                    Regex regex = new Regex("^[А-Яа-я]+$");
                    if (regex.IsMatch(ch.name_priv))
                    {
                        string sql = @"insert into data_base.privileges (name_func) values ('" + ch.name_priv + "')";
                        con.Open();
                        MySqlCommand cmd = new MySqlCommand(sql, con);
                        cmd.ExecuteReader();
                        con.Close();
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch
                {
                    return 0;
                }
            }
        }
        public string tableName { get; set; }
        public object[] GetList(object filter)
        {
            List<object> ListObjects = new List<object>();
            List<string> valuesFilter = new List<string>();

            using (MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySQLCon"].ConnectionString))
            {
                string sql = null;

                con.Open();
                foreach (var prop in filter.GetType().GetProperties())
                {
                    if (prop.Name == "tableName")
                    {                     
                        this.tableName = (string)prop.GetValue(filter);
                        continue;
                    }
                    if ((prop.GetValue(filter) != null) && (prop.GetValue(filter).ToString() != "0") && (prop.GetValue(filter).ToString() != "01.01.0001 0:00:00"))
                    {
                        valuesFilter.Add(prop.Name + "=" + "'" + prop.GetValue(filter) + "'");
                    }
                }
                if (valuesFilter.Count != 0)
                {
                    sql = @"select * from " + this.tableName + " where " + "(" + valuesFilter.Aggregate((workingSQL, next) => next + " and " + workingSQL) + ")";
                }
                else
                {
                    sql = @"select * from " + this.tableName;
                }
                MySqlCommand cmd = new MySqlCommand(sql, con);
                MySqlDataReader result = cmd.ExecuteReader();
                Type t = filter.GetType();
                if (result.HasRows)
                {
                    while (result.Read())
                    {
                        var SomeObject = Activator.CreateInstance(t);
                        foreach (var props in t.GetProperties())
                        {
                            PropertyInfo propertyInfo = filter.GetType().GetProperty(props.Name);
                            string NameResult;
                            if (propertyInfo.PropertyType.Name == "Int32")
                            {
                                NameResult = propertyInfo.Name;
                                try
                                {
                                    propertyInfo.SetValue(SomeObject, Convert.ChangeType(result.GetInt32(NameResult), propertyInfo.PropertyType), null);
                                }
                                catch { }
                            }
                            if (propertyInfo.PropertyType.Name == "String")
                            {
                                if (props.Name == "tableName") continue;
                                NameResult = propertyInfo.Name;
                                try
                                {
                                    propertyInfo.SetValue(SomeObject, Convert.ChangeType(result.GetString(NameResult), propertyInfo.PropertyType), null);
                                }
                                catch { }
                            }
                        }
                        ListObjects.Add(SomeObject);
                    }
                }
            }
            object[] list = ListObjects.ToArray();
            return list;
        }
        public int add_group(changes ch)
        {
            using (MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySQLCon"].ConnectionString))
            {
                try
                {
                    Regex regex = new Regex("^[А-Яа-я]+$");
                    if (regex.IsMatch(ch.name_group))
                    {
                        string sql = @"insert into data_base.groups (name) values ('" + ch.name_group + "')";
                        con.Open();
                        MySqlCommand cmd = new MySqlCommand(sql, con);
                        cmd.ExecuteReader();
                        con.Close();
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch
                {
                    return 0;
                }
            }
        }
        public int add_pers(changes ch)
        {
            using (MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySQLCon"].ConnectionString))
            {
                try
                {
                    Convert.ToInt32(ch.ID_group);
                    Regex regex = new Regex("^[А-Яа-я]+$");
                    if (regex.IsMatch(ch.surname) && regex.IsMatch(ch.name) && regex.IsMatch(ch.middle_name))
                    {
                        string sql = @"insert into data_base.persons (surname, name, middle_name, Groups_ID_group) values ('" + ch.surname + "','" + ch.name + "','" + ch.middle_name + "','" + ch.ID_group + "')";
                        con.Open();
                        MySqlCommand cmd = new MySqlCommand(sql, con);
                        cmd.ExecuteReader();
                        con.Close();
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch
                {
                    return 0;
                }
            }
        }
        public int update_pers(changes ch)
        {
            using (MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySQLCon"].ConnectionString))
            {
                try
                {
                    Convert.ToInt32(ch.ID_group);
                    Convert.ToInt32(ch.ID_person);
                    Regex regex = new Regex("^[А-Яа-я]+$");

                    if (regex.IsMatch(ch.surname) && regex.IsMatch(ch.name) && regex.IsMatch(ch.middle_name))
                    {
                        sql(@"SET SQL_SAFE_UPDATES = 0");
                        string sql1 = @"update data_base.persons set Groups_ID_group= '" + ch.ID_group + "' , surname = '" + ch.surname + "', " + "name='" + ch.name + "', middle_name='" + ch.middle_name + "' where ID_person='" + ch.ID_person + "'";
                        con.Open();
                        MySqlCommand cmd = new MySqlCommand(sql1, con);
                        cmd.ExecuteReader();
                        con.Close();
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch
                {
                    return 0;
                }
            }
        }
        public void sql(string str)
        {
            using (MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySQLCon"].ConnectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(str, con);
                cmd.ExecuteReader();
                con.Close();
            }
        }
        public int update_groups(changes ch) // привилегии в группах
        {
            using (MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySQLCon"].ConnectionString))
            {
                sql(@"SET SQL_SAFE_UPDATES = 0");
                try
                {
                    Convert.ToInt32(ch.ID_group);
                    sql(@"delete from data_base.groups_privileges where Groups_ID_group='" + ch.ID_group + "'");

                    sql(@"update data_base.groups set name='" + ch.name_group + "' where ID_group='" + ch.ID_group + "'");

                    string[] mas = ch.IDs_privgr.Split(',');

                    try
                    {
                        foreach (string m in mas)
                        {
                            Convert.ToInt32(m);
                            sql(@"insert into data_base.groups_privileges set Groups_ID_group='" + ch.ID_group + "', Privileges_ID_privilege ='" + m + "'");
                        }
                        return 1;
                    }
                    catch
                    {
                        return 0;
                    }

                }
                catch
                {
                   //тот же принцип, что внизу
                    return 1;
                }
            }
        }
        public int update_per_priv(changes ch) // привилегии человека
        {
            int flag = 0;
            sql(@"SET SQL_SAFE_UPDATES = 0");
            try
            {
                Convert.ToInt32(ch.ID_person);
                sql(@"delete from data_base.persons_privileges where Persons_ID_person='" + ch.ID_person + "'");
                string[] mas = ch.IDs_privch.Split(',');
                foreach (string m in mas)
                {
                    try
                    {
                        Convert.ToInt32(m);
                        sql(@"insert into data_base.persons_privileges set Persons_ID_person='" + ch.ID_person + "', Privileges_ID_privilege ='" + m + "'");
                        flag = 1;
                    }
                    catch
                    {
                        flag = 0;
                    }
                }
                return flag;
            }
            catch
            {
                flag = 1; //т.к. все принимается пустые привилегии - таблица с этим id очищается
                return flag;
            }
        }
    }
}
