using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LAMPSServer.Models;

    public interface ISuccessOrFailure
    {
        bool Status { get; set; }
        string Message { get; set; }
    }

    public class Success: ISuccessOrFailure
    {
        public bool Status { get; set; }
        public string Message { get; set; }

        public Success()
        {
            Status = true;
            Message = string.Empty;
        }
    }

    public class Failure: ISuccessOrFailure
    {
        public bool Status { get; set; }
        public string Message { get; set; }

        public Failure()
        {
            Status = false;
            Message = string.Empty;
        }
    }

    public class PayloadWrapper<T>
    {
        public DateTime dateTime { get; set; }
        public int length { get; set; }
        public String payloadType { get; set; }
        public ICollection<T> payload { get; set; }
        public bool hasError { get; set; }
        public string message { get; set; }

        public PayloadWrapper(T obj, string error= "") 
        {
            this.dateTime = DateTime.UtcNow;
            this.payloadType = obj == null ? "NONE" : obj.GetType().Name;
            this.payload = new List<T>() { obj };
            this.length = this.payload.Count;
            this.hasError = error != string.Empty ? true : false;
            this.message = error != string.Empty ? error : string.Empty;
        }

        public PayloadWrapper(ICollection<T> list, string error = "") : this(list.FirstOrDefault(), error)
        {
            this.payload = list;
            this.length = payload.Count;
        }


        public PayloadWrapper(IEnumerable<T> list, string error = "") : this(list.FirstOrDefault(), error)
        {
            this.payload = list.ToArray();
            this.length = payload.Count;
        }

        public PayloadWrapper(string error)
        {
            this.length = 0;
            this.dateTime = DateTime.UtcNow;
            this.payloadType = typeof(T).Name;
            this.payload = new List<T>() { };
            this.hasError = true;
            this.message = error;
        }

    public static PayloadWrapper<Success> success(string message = "")
    {
        var wrap = new PayloadWrapper<Success>(new Success()
        {
            Message = message,
            Status = true
        })
        {
            hasError = false
        };
        return wrap;
    }
    public static PayloadWrapper<Failure> exception(string message = "")
    {
        var wrap = new PayloadWrapper<Failure>(new Failure()
        {
            Message = message,
            Status = false
        })
        {
            hasError = true
        };
        return wrap;
    }

}

