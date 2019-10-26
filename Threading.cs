using CSUR_TMPE_Laneconnector_Fix.CustomAI;
using CSUR_TMPE_Laneconnector_Fix.Util;
using ColossalFramework;
using ColossalFramework.Globalization;
using ColossalFramework.UI;
using ICities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CSUR_TMPE_Laneconnector_Fix
{
    public class Threading : ThreadingExtensionBase
    {
        public static bool isFirstTime = true;
        public static bool isDetoured = false;

        public override void OnBeforeSimulationFrame()
        {
            base.OnBeforeSimulationFrame();

            if (CSUR_TMPE_Laneconnector_Fix.IsEnabled)
            {
                CheckDetour();
            }
        }

        public void DetourAfterLoad()
        {
            //This is for Detour Other Mod method
            DebugLog.LogToFileOnly("Init DetourAfterLoad");
            bool detourFailed = false;

            if (Loader.is1637663252 | Loader.is583429740)
            {
                DebugLog.LogToFileOnly("Detour LaneConnectorTool::CheckSegmentsTurningAngle calls");
                try
                {
                    Loader.Detours.Add(new Loader.Detour(Assembly.Load("TrafficManager").GetType("TrafficManager.UI.SubTools.LaneConnectorTool").GetMethod("CheckSegmentsTurningAngle", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(ushort), typeof(NetSegment).MakeByRefType(), typeof(bool), typeof(ushort), typeof(NetSegment).MakeByRefType(), typeof(bool) }, null),
                                           typeof(NewLaneConnectorTool).GetMethod("CheckSegmentsTurningAngle", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(ushort), typeof(NetSegment).MakeByRefType(), typeof(bool), typeof(ushort), typeof(NetSegment).MakeByRefType(), typeof(bool) }, null)));
                }
                catch (Exception)
                {
                    DebugLog.LogToFileOnly("Could not detour LaneConnectorTool::CheckSegmentsTurningAngle");
                    detourFailed = true;
                }

                if (detourFailed)
                {
                    DebugLog.LogToFileOnly("DetourAfterLoad failed");
                }
                else
                {
                    DebugLog.LogToFileOnly("DetourAfterLoad successful");
                }
            }
        }

        public void CheckDetour()
        {
            if (isFirstTime)
            {
                isFirstTime = false;
                DetourAfterLoad();
                if (true)
                {
                    DebugLog.LogToFileOnly("ThreadingExtension.OnBeforeSimulationFrame: First frame detected. Checking detours.");
                    List<string> list = new List<string>();
                    foreach (Loader.Detour current in Loader.Detours)
                    {
                        if (!RedirectionHelper.IsRedirected(current.OriginalMethod, current.CustomMethod))
                        {
                            list.Add(string.Format("{0}.{1} with {2} parameters ({3})", new object[]
                            {
                    current.OriginalMethod.DeclaringType.Name,
                    current.OriginalMethod.Name,
                    current.OriginalMethod.GetParameters().Length,
                    current.OriginalMethod.DeclaringType.AssemblyQualifiedName
                            }));
                        }
                    }
                    DebugLog.LogToFileOnly(string.Format("ThreadingExtension.OnBeforeSimulationFrame: First frame detected. Detours checked. Result: {0} missing detours", list.Count));
                    if (list.Count > 0)
                    {
                        string error = "AdvancedJunctionRuleThreading detected an incompatibility with another mod! You can continue playing but it's NOT recommended. AdvancedJunctionRuleThreading will not work as expected. Send AdvancedJunctionRuleThreading.txt to Author.";
                        DebugLog.LogToFileOnly(error);
                        string text = "The following methods were overriden by another mod:";
                        foreach (string current2 in list)
                        {
                            text += string.Format("\n\t{0}", current2);
                        }
                        DebugLog.LogToFileOnly(text);
                        UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel").SetMessage("Incompatibility Issue", text, true);
                    }
                }
            }
        }
    }
}
