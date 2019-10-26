﻿using ColossalFramework.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSUR_TMPE_Laneconnector_Fix.Util
{
    public static class DebugLog
    {
        public static void LogToFileOnly(string msg)
        {
            using (FileStream fileStream = new FileStream("CSUR_TMPE_Laneconnector_Fix.txt", FileMode.Append))
            {
                StreamWriter streamWriter = new StreamWriter(fileStream);
                streamWriter.WriteLine(msg);
                streamWriter.Flush();
            }
        }

        public static void LogWarning(string msg)
        {
            DebugOutputPanel.AddMessage(PluginManager.MessageType.Warning, msg);
        }
    }
}
