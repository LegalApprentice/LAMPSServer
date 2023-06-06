using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LAMPSServer.Models
{
    public class CaseDirectoryItem : CaseCoreInfo
    {
        public Uri uri { get; set; }
    }

    public class UploadedCase : CaseCoreInfo
    {
        public string data { get; set; }
    }

    public class DownloadedCase : CaseCoreInfo
    {
        public Uri uri { get; set; }
        public string data { get; set; }
    }
}
