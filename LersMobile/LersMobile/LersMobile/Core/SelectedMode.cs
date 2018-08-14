using System;
using System.Collections.Generic;
using System.Text;

namespace LersMobile.Core
{
    public enum SelectedMode
    {
        None = 0,
        Selecting = 1,
        Select = 2,
        UnSelect = 3
    }

    static public class SelectedModeUtils
    {
        static public string GetSourceImage(SelectedMode selectedMode)
        {
            switch (selectedMode)
            {   
                case SelectedMode.Select:
                    return "select.png";
                case SelectedMode.UnSelect:
                case SelectedMode.Selecting:
                    return "unselect.png";                
            }
            return string.Empty;
        }
    }
}
