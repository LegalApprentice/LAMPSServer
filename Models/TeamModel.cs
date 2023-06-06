using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LAMPSServer.Models
{
    public class TeamData
    {
        public string teamName { get; set; }
        public string leader { get; set; }
        public string member { get; set; }
        public bool invited { get; set; }
        public bool active { get; set; }
        public string workspace { get; set; }
        public string pattern { get; set; }

        public void CopyFrom(TeamData source)
        {
            this.teamName = source.teamName;
            this.leader = source.leader;
            this.member = source.member;
            this.invited = source.invited;
            this.active = source.active;
            this.workspace = source.workspace;
            this.pattern = source.pattern;
        }

        public void toLowerCase()
        {
            this.leader = this.leader.ToLower();
            this.member = this.member.ToLower();
        }
    }

    public class TeamModel : TeamData
    {
        public string guidKey { get; set; }
    }

    public class TeamEntity : TeamData
    {
        [Key]
        public string guidKey { get; set; }
    }
}