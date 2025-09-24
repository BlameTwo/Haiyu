using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haiyu.Helpers
{
    internal class RoleHelper
    {
        public static string SwitchType(int RoleData)
        {
            var TypeImage = "";
            switch (RoleData)
            {
                case 1:
                    TypeImage = GameIcon.Icon1;
                    break;
                case 2:
                    TypeImage = GameIcon.Icon2;
                    break;
                case 3:
                    TypeImage = GameIcon.Icon3;
                    break;
                case 4:
                    TypeImage = GameIcon.Icon4;
                    break;
                case 5:
                    TypeImage = GameIcon.Icon5;
                    break;
                case 6:
                    TypeImage = GameIcon.Icon6;
                    break;
            }
            return TypeImage;
        }
    }
}
