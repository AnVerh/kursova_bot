using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoroscopeBot.ChTranslate
{
    class RequestModel
    {
        public string fromLang { get; set; }
        public string text { get; set; }
        public string to { get; set; }
    }
}
