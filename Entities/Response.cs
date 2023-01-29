using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class Response
    {
        public bool IsSuccessful { get; set; }
        public bool Detected { get; set; }
        public string ErrorMessage { get; set; }
        public string Symbol { get; set; }
        public string Close { get; set; }
    }
}
