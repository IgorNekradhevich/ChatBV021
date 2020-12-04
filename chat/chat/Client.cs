using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Windows.Media;

namespace chat
{
    class Client
    {
        public string Name{ get; set; }
        public int Id{ get; set; }
       // Color color = Color.FromRgb(0, 0, 0);

        public Client (int id, string name)
        {
            Id = id;
            this.Name = name;
        }
    }
}
