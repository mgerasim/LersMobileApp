using System;
using System.Collections.Generic;
using System.Text;

namespace LersMobile
{
    public interface IMessage
    {
        void Show(string text, bool isLong = false);
    }
}
