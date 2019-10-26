using CSUR_TMPE_Laneconnector_Fix.Util;
using ColossalFramework.UI;
using ICities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CSUR_TMPE_Laneconnector_Fix
{
    public class CSUR_TMPE_Laneconnector_Fix : IUserMod
    {
        public static bool IsEnabled = false;
        public static int language_idex = 0;

        public string Name
        {
            get { return "CSUR_TMPE_Laneconnector_Fix"; }
        }

        public string Description
        {
            get { return "Fix TMPE laneconnector issue in some CSUR road"; }
        }

        public void OnEnabled()
        {
            IsEnabled = true;
            FileStream fs = File.Create("CSUR_TMPE_Laneconnector_Fix.txt");
            fs.Close();
        }

        public void OnDisabled()
        {
            IsEnabled = false;
        }
    }
}
