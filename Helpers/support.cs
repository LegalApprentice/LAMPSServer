using System;
using System.Collections.Generic;

namespace LAMPSServer.Helpers;

public class ClusterHealth
{
    public string status { get; set; }
}

public class IndexDataSource
{
    public string Name { get; set; }
    public DateTime? LastUpdated { get; set; }
    public DateTime? NextUpdate { get; set; }
    public DateTime? LastErrorDate { get; set; }
    public string LastErrorMessage { get; set; }

}

public class MasterNode
{
    public string master_node { get; set; }
    public Dictionary<string, NodeItem> nodes { get; set; }
}

public class NodeItem
{
    public string name { get; set; }
    public string transport_address { get; set; }
}

public class ResultWrapper<T>
{
    public T Result { get; set; }
    public bool IsError { get; set; }
    public string ErrorMessage { get; set; }
    public int? StatusCode { get; set; }
}