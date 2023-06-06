namespace LAMPSServer.Models;



public class LABase
{
    public string caseNumber { get; set; }
}

public class LASentence : LABase
{
    public string sentID { get; set; }
    public string text { get; set; }
    public string notes { get; set; }
    public string author { get; set; }
    public string rhetClass { get; set; }
    public string sectionType { get; set; }
    
}

public class LAParagraph : LABase
{
    public string paraID { get; set; }
    public string notes { get; set; }
    public string author { get; set; }
    public string[] sentIDList { get; set; }
    
}

public class LACaseInfo
{
    public string guidKey { get; set; }
    public string name { get; set; }
    public string owner { get; set; }
    public string source { get; set; }
    public string title { get; set; }
}

public class LACase : LABase
{
    public LACaseInfo caseInfo { get; set; }
}

public class LASearchResult<T> where T : LABase
{
    public string _index { get; set; }
    public string _type { get; set; }
    public string _id { get; set; }
    public double? _score { get; set; }
    public T _source { get; set; }
}
