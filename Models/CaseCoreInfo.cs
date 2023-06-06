using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LAMPSServer.Models
{
    public class CaseReference
    {
        [Key]
        public string fileName { get; set; }
        public string workspace { get; set; }
    }

    public class CaseCoreInfo: CaseReference
    {

        public string guidKey { get; set; }
        public string version { get; set; }
        public string lastChange { get; set; }
        public string prevFileName { get; set; }
        public string nextFileName { get; set; }
        public string name { get; set; }
        public string extension { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string keywords { get; set; }
        public string notes { get; set; }
        public string owner { get; set; }
        public string source { get; set; }

        public void CopyFrom(CaseCoreInfo source)
        {
            this.fileName = source.fileName;
           
            this.version = source.version;
            this.lastChange = source.lastChange;
            this.name = source.name;
            this.nextFileName = source.nextFileName;
            this.prevFileName = source.prevFileName;
            this.extension = source.extension;
            this.title = source.title;
            this.description = source.description;
            this.keywords = source.keywords;
            this.notes = source.notes;
            this.owner = source.owner;
            this.source = source.source;
            this.workspace = source.workspace;

            if ( !string.IsNullOrWhiteSpace(source.guidKey) ) 
            {
                this.guidKey = source.guidKey;
            }
        }

        public void refreshVersion(string version)
        {
            this.fileName = this.fileName.Replace($"-{this.version}", $"-{version}");
            this.version = version;
        }

    }
}
