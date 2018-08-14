using System;
using System.Collections.Generic;
using System.Text;

namespace LersMobile.Core
{
    public class SelectableData<T>
    {
        public T Data { get; set; }
        public bool Selected { get; set; }
    }
}
