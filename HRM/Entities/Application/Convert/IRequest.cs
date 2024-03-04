using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Application.Convert
{
    public interface IRequest
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public int Status { get; set; }
    }
}
