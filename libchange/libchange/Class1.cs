using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libchange
{
    [Serializable]
    //public class changes
    //{
    //    public int ID_person { get; set; }
    //    public string surname { get; set; }
    //    public string name { get; set; }
    //    public string middle_name { get; set; }
    //    public int ID_group { get; set; }
    //    public string name_group { get; set; }
    //    public string name_priv { get; set; }
    //    public string IDs_privgr { get; set; }
    //    public string IDs_privch { get; set; }
    //    public changes()
    //    { }
    //}
    public class Group_
    {
        public int ID_group { get; set; }
        public string name { get; set; }
        public string tableName { get; set; }
        public Group_()
        {
            this.tableName = "data_base.groups";
        }
    }
    public class Person
    {
        public int ID_person { get; set; }
        public string surname { get; set; }
        public string name { get; set; }
        public string middle_name { get; set; }
        public int Groups_ID_Group { get; set; }
        public int WasDel { get; set; }
        public string tableName { get; set; }
        public Person()
        {
            this.tableName = "data_base.persons";
        }
    }
    public class Group_Privileges
    {
        public int Groups_ID_group { get; set; }
        public int Privileges_ID_privilege { get; set; }
        public string tableName { get; set; }
        public Group_Privileges()
        {
            this.tableName = "data_base.groups_privileges";
        }
    }
    public class Privilege
    {
        public int ID_privilege { get; set; }
        public string name_func { get; set; }
        public string tableName { get; set; }
        public Privilege()
        {
            this.tableName = "data_base.privileges";
        }
    }
    public class Persons_Privileges
    {
        public int Privileges_ID_privilege { get; set; }
        public int Persons_ID_person { get; set; }
        public string tableName { get; set; }
        public Persons_Privileges()
        {
            this.tableName = "data_base.persons_privileges";
        }
    }
}
