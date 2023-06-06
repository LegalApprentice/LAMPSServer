using LAMPSServer.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace LAMPSServer.Helpers
{
    public class UserDataContext : CommonContext
    {
        public UserDataContext(DbContextOptions<UserDataContext> options) : base(options) { }

        public DbSet<UserEntity> Users { get; set; }

        public void SetupDatabase(List<UserEntity> maps = null)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();

            if (maps != null)
            {
                AddRange(maps);
            }

            SaveChanges();
        }


        public override bool ReadFromText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            var maps = new List<UserEntity>();

            var source = JToken.Parse(text);
            foreach (var map in source.ToObject<dynamic[]>())
            {
                var PasswordHash = CommonContext.ReadByteValueFromDymanic(map, "PasswordHash").Data;
                var PasswordSalt = CommonContext.ReadByteValueFromDymanic(map, "PasswordSalt").Data;

                var obj = new UserEntity()
                {
                    firstName = CommonContext.ReadStringValueFromDymanic(map, "firstName"),
                    lastName = CommonContext.ReadStringValueFromDymanic(map, "lastName"),
                    username = CommonContext.ReadStringValueFromDymanic(map, "username"),
                    email = CommonContext.ReadStringValueFromDymanic(map, "email"),
                    PasswordHash = PasswordHash,
                    PasswordSalt = PasswordSalt,
                };

                maps.Add(obj);
            }


            SetupDatabase(maps);
            return true;
        }
    }
   
}