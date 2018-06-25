using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfService_
{
    [ServiceContract]
    public interface IOperationChanges
    {
        [OperationContract]
        int add_priv(changes ch);
        [OperationContract]
        [ServiceKnownType(typeof(Person))]
        [ServiceKnownType(typeof(Group_))]
        [ServiceKnownType(typeof(Privilege))]
        [ServiceKnownType(typeof(Group_Privileges))]
        [ServiceKnownType(typeof(Persons_Privileges))]
        object[] GetList(object ob);
        [OperationContract]
        int add_group(changes ch);
        [OperationContract]
        int add_pers(changes ch);
        [OperationContract]
        int update_pers(changes ch);
        [OperationContract]
        int update_groups(changes ch);
        [OperationContract]
        int update_per_priv(changes ch);
    }
}
