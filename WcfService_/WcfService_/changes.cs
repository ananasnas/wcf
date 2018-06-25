using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfService_
{
    [DataContract]

    public class changes
    {
        [DataMember]
        public int ID_person { get; set; }
        [DataMember]
        public string surname { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string middle_name { get; set; }
        [DataMember]
        public int ID_group { get; set; }
        [DataMember]
        public string name_group { get; set; }
        [DataMember]
        public string name_priv { get; set; }
        [DataMember]
        public string IDs_privgr { get; set; }
        [DataMember]
        public string IDs_privch { get; set; }
    }
    [DataContract]
    public class Group_
    {
        [DataMember]
        public int ID_group { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string tableName { get; set; }
    }
    [DataContract]
  
    public partial class Person
    {
        [DataMember]
        public int ID_person { get; set; }
        [DataMember]
        public string surname { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string middle_name { get; set; }
        [DataMember]
        public int Groups_ID_Group { get; set; }
        [DataMember]
        public int WasDel { get; set; }
        [DataMember]
        public string tableName { get; set; }
    }
    [DataContract]
    public class Group_Privileges
    {
        [DataMember]
        public int Groups_ID_group { get; set; }
        [DataMember]
        public int Privileges_ID_privilege { get; set; }
        [DataMember]
        public string tableName { get; set; }
    }
    [DataContract]
    public class Privilege
    {
        [DataMember]
        public int ID_privilege { get; set; }
        [DataMember]
        public string name_func { get; set; }
        [DataMember]
        public string tableName { get; set; }
    }
    [DataContract]
    public class Persons_Privileges
    {
        [DataMember]
        public int Privileges_ID_privilege { get; set; }
        [DataMember]
        public int Persons_ID_person { get; set; }
        [DataMember]
        public string tableName { get; set; }
    }
}
