using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTQuanLyTiecCuoi.Entities
{
    public class Service
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public List<Hall> Halls { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Service objAsService = obj as Service;
            if (objAsService == null) return false;
            else return Equals(objAsService);
        }
       
        public bool Equals(Service other)
        {
            if (other == null) return false;
            return (Id.Equals(other.Id));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
