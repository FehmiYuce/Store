using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestParameters
{
	public abstract class RequestParameters
	{
        // requestlerdeli ortak alanları burada tanımlayacağız
        public String? SearchTerm { get; set; }
    }
}
