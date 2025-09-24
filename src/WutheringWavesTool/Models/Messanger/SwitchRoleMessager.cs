using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haiyu.Models.Messanger
{
    public class SwitchRoleMessager
    {
        public GameRoilDataWrapper Data { get; set; }

        public SwitchRoleMessager(GameRoilDataWrapper data)
        {
            Data = data;
        }
    }
}
