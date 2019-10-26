using ColossalFramework.UI;
using ICities;
using UnityEngine;
using System.IO;
using ColossalFramework;
using System.Reflection;
using System;
using System.Linq;
using ColossalFramework.Math;
using ColossalFramework.PlatformServices;
using System.Collections.Generic;
using CSUR_TMPE_Laneconnector_Fix.Util;
using CSUR_TMPE_Laneconnector_Fix.CustomAI;

namespace CSUR_TMPE_Laneconnector_Fix
{
    public class Loader : LoadingExtensionBase
    {
        public static LoadMode CurrentLoadMode;
        public static UIPanel roadInfo;
        public static GameObject roadWindowGameObject;
        public static bool isGuiRunning = false;
        public static bool isLoaded = false;
        public static bool is583429740 = false;
        public static bool is1637663252 = false;

        public class Detour
        {
            public MethodInfo OriginalMethod;
            public MethodInfo CustomMethod;
            public RedirectCallsState Redirect;

            public Detour(MethodInfo originalMethod, MethodInfo customMethod)
            {
                this.OriginalMethod = originalMethod;
                this.CustomMethod = customMethod;
                this.Redirect = RedirectionHelper.RedirectCalls(originalMethod, customMethod);
            }
        }

        public static List<Detour> Detours { get; set; }

        public override void OnCreated(ILoading loading)
        {
            base.OnCreated(loading);
            Detours = new List<Detour>();
        }

        public override void OnLevelLoaded(LoadMode mode)
        {
            base.OnLevelLoaded(mode);
            Loader.CurrentLoadMode = mode;
            if (CSUR_TMPE_Laneconnector_Fix.IsEnabled)
            {
                if (mode == LoadMode.LoadGame || mode == LoadMode.NewGame)
                {
                    DebugLog.LogToFileOnly("OnLevelLoaded");
                    CheckTMPE();
                    if (mode == LoadMode.NewGame)
                    {
                        DebugLog.LogToFileOnly("New Game");
                    }
                    isLoaded = true;
                }
            }
        }

        public void CheckTMPE()
        {
            if (IsSteamWorkshopItemSubscribed(583429740) && IsSteamWorkshopItemSubscribed(1637663252))
            {
                UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel").SetMessage("Incompatibility Issue", "Can not sub two TM:PE, steamID:583429740 and 1637663252", true);
            } else if (IsSteamWorkshopItemSubscribed(583429740))
            {
                is583429740 = true;
            } else if (IsSteamWorkshopItemSubscribed(1637663252))
            {
                is1637663252 = true;
            }

            if (!this.Check3rdPartyModLoaded("TrafficManager", true))
            {
                UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel").SetMessage("Incompatibility Issue", "Require TM:PE steamID:583429740 or 1637663252", true);
            }
        }

        public override void OnLevelUnloading()
        {
            base.OnLevelUnloading();
            is583429740 = false;
            is1637663252 = false;

            if (Threading.isDetoured)
            {
                Threading.isFirstTime = true;
            }
        }

        public override void OnReleased()
        {
            base.OnReleased();
        }

        public static bool IsSteamWorkshopItemSubscribed(ulong itemId)
        {
            return ContentManagerPanel.subscribedItemsTable.Contains(new PublishedFileId(itemId));
        }

        private bool Check3rdPartyModLoaded(string namespaceStr, bool printAll = false)
        {
            bool thirdPartyModLoaded = false;

            var loadingWrapperLoadingExtensionsField = typeof(LoadingWrapper).GetField("m_LoadingExtensions", BindingFlags.NonPublic | BindingFlags.Instance);
            List<ILoadingExtension> loadingExtensions = (List<ILoadingExtension>)loadingWrapperLoadingExtensionsField.GetValue(Singleton<LoadingManager>.instance.m_LoadingWrapper);

            if (loadingExtensions != null)
            {
                foreach (ILoadingExtension extension in loadingExtensions)
                {
                    if (printAll)
                        DebugLog.LogToFileOnly($"Detected extension: {extension.GetType().Name} in namespace {extension.GetType().Namespace}");
                    if (extension.GetType().Namespace == null)
                        continue;

                    var nsStr = extension.GetType().Namespace.ToString();
                    if (namespaceStr.Equals(nsStr))
                    {
                        DebugLog.LogToFileOnly($"The mod '{namespaceStr}' has been detected.");
                        thirdPartyModLoaded = true;
                        break;
                    }
                }
            }
            else
            {
                DebugLog.LogToFileOnly("Could not get loading extensions");
            }

            return thirdPartyModLoaded;
        }

    }
}

