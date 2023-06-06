using LAMPSServer.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace LAMPSServer.Helpers
{
    public class CaseDirectoryContext : CommonContext
    {

        public CaseDirectoryContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<CaseDirectoryItem> CaseInfo { get; set; }

        public void SetupDatabase(List<CaseDirectoryItem> maps = null)
        {
            ClearAll();

            if (maps != null)
            {
                AddRange(maps);
            }

            SaveChanges();
        }

        public void ClearAll()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public override bool ReadFromText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            var maps = new List<CaseDirectoryItem>();

            var source = JToken.Parse(text);
            foreach (var map in source.ToObject<dynamic[]>())
            {

                var obj = new CaseDirectoryItem()
                {
                    uri = new Uri(CommonContext.ReadStringValueFromDymanic(map, "uri")),
                    version = CommonContext.ReadStringValueFromDymanic(map, "version"),
                    owner = CommonContext.ReadStringValueFromDymanic(map, "owner"),
                    name = CommonContext.ReadStringValueFromDymanic(map, "name"),
                    nextFileName = CommonContext.ReadStringValueFromDymanic(map, "nextFileName"),
                    prevFileName = CommonContext.ReadStringValueFromDymanic(map, "prevFileName"),
                    fileName = CommonContext.ReadStringValueFromDymanic(map, "fileName"),
                    extension = CommonContext.ReadStringValueFromDymanic(map, "extension"),
                    title = CommonContext.ReadStringValueFromDymanic(map, "title"),
                    description = CommonContext.ReadStringValueFromDymanic(map, "description"),
                    keywords = CommonContext.ReadStringValueFromDymanic(map, "keywords"),
                    notes = CommonContext.ReadStringValueFromDymanic(map, "notes"),
                    workspace = CommonContext.ReadStringValueFromDymanic(map, "workspace")
                };
 

                maps.Add(obj);
            }


            SetupDatabase(maps);
            return true;
        }

        public override bool ReadFromFile()
        {
            using FileStream fs = System.IO.File.Open("./Data/CaseDirectoryInfo.json", FileMode.Open);
            var text = new StreamReader(fs).ReadToEnd();
            return ReadFromText(text);
        }
    }
}
