using LAMPSServer.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace LAMPSServer.Helpers
{
    public class TeamDataContext : CommonContext
    {
        public TeamDataContext(DbContextOptions<TeamDataContext> options) : base(options) { }

        public DbSet<TeamEntity> Teams { get; set; }


        public void SetupDatabase(List<TeamEntity> maps = null)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();

            if (maps != null)
            {
                AddRange(maps);
            }

            SaveChanges();
        }

        public override bool ReadFromFile()
        {
            using FileStream fs = System.IO.File.Open("./Data/team.json", FileMode.Open);
            var text = new StreamReader(fs).ReadToEnd();
            return ReadFromText(text);
        }

        public override bool ReadFromText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            var maps = new List<TeamEntity>();


            var source = JToken.Parse(text);
            foreach (var map in source.ToObject<dynamic[]>())
            {
                var guidKey = CommonContext.ReadStringValueFromDymanic(map, "guidKey");
                if ( string.IsNullOrEmpty(guidKey) )
                {
                    guidKey = Guid.NewGuid().ToString();
                }

                var invited = bool.Parse(CommonContext.ReadStringValueFromDymanic(map, "invited"));
                var active = bool.Parse(CommonContext.ReadStringValueFromDymanic(map, "active"));

                var obj = new TeamEntity()
                {
                    guidKey = guidKey,
                    teamName = CommonContext.ReadStringValueFromDymanic(map, "teamName"),
                    leader = CommonContext.ReadStringValueFromDymanic(map, "leader"),
                    member = CommonContext.ReadStringValueFromDymanic(map, "member"),
                    workspace = CommonContext.ReadStringValueFromDymanic(map, "workspace"),
                    pattern = CommonContext.ReadStringValueFromDymanic(map, "pattern"),
                    invited = invited,
                    active = active,
                };

                maps.Add(obj);
            }


            SetupDatabase(maps);
            return true;
        }
    }
   
}