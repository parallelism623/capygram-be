using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace capygram.Common.Shared
{
    public class ResultDetail
    {
        public static ResultDetail None = new(string.Empty, string.Empty);
        public ResultDetail(string _code, string _message) 
        {
            Message = _message;
            Code = _code;   
        }
        public string Message { get; set; }
        public string Code { get; set; }
    }
}
