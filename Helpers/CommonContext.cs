using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;

namespace LAMPSServer.Helpers
{
    interface IDbContext
    {
        bool ReadFromFile();
        bool ReadFromText(string text);
        bool ReadFromCashe();
    }

    public class CommonContext : DbContext, IDbContext
    {

        public CommonContext(DbContextOptions options) : base(options)
        {
        }

        public virtual bool ReadFromFile()
        {
            return false;
        }

        public virtual bool ReadFromCashe()
        {
            return false;
        }

        public virtual bool ReadFromText(string text)
        {
            return false;
        }

        public static string ReadStringValueFromDymanic(dynamic obj, string name, string defaultValue = "")
        {
            try
            {
                var value = obj[name];
                var result = value.ToObject<string>();
                return result ?? defaultValue;
            }
            catch (Exception)
            {
                return defaultValue;
            }

        }

        public static ByteData ReadByteValueFromDymanic(dynamic obj, string name)
        {

            try
            {
                var value = obj[name];
                var result = value.ToObject<string>();
                var items = JsonConvert.DeserializeObject<ByteData>(result);
                return items;
            }
            catch (Exception)
            {
                return new ByteData();
            }

        }
    }
}