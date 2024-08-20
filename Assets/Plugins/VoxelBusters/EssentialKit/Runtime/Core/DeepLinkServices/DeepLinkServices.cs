using System;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.EssentialKit.DeepLinkServicesCore;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Provides cross-platform interface to handle deep links.    
    /// </summary>
    /// <description>
    public static class DeepLinkServices
    {
        #region Static fields

        [ClearOnReload]
        private     static  INativeDeepLinkServicesInterface    s_nativeInterface;

        #endregion

        #region Static properties

        private     static  INativeDeepLinkServicesInterface    NativeInterface
        {
            get
            {
                if(s_nativeInterface == null)
                {
                    DebugLogger.LogException(EssentialKitDomain.Default, new Exception("Deep Link Services is not enabled. You need to enable this feature to use it."));
                }

                return s_nativeInterface;
            }

            set
            {
                s_nativeInterface = value;
            }
        }

        public static DeepLinkServicesUnitySettings UnitySettings { get; private set; }

        public static IDeepLinkServicesDelegate Delegate { get; set; }

        #endregion

        #region Static events

        /// <summary>
        /// Event that will be called when url scheme is opened.
        /// </summary>
        public static event Callback<DeepLinkServicesDynamicLinkOpenResult> OnCustomSchemeUrlOpen;

        /// <summary>
        /// Event that will be called when universal link is opened.
        /// </summary>
        public static event Callback<DeepLinkServicesDynamicLinkOpenResult> OnUniversalLinkOpen;

        #endregion

        #region Setup methods

        public static bool IsAvailable()
        {
            return (NativeInterface != null) && NativeInterface.IsAvailable;
        }

        internal static void Initialize(DeepLinkServicesUnitySettings settings)
        {
            Assert.IsArgNotNull(settings, nameof(settings));

            // Reset event properties
            OnCustomSchemeUrlOpen   = null;
            OnUniversalLinkOpen     = null;

            // Set properties
            UnitySettings           = settings;

            // Configure interface
            NativeInterface       = NativeFeatureActivator.CreateInterface<INativeDeepLinkServicesInterface>(ImplementationSchema.DeepLinkServices, true);
            NativeInterface.SetCanHandleCustomSchemeUrl(handler: CanHandleCustomSchemeUrl);
            NativeInterface.SetCanHandleUniversalLink(handler: CanHandleUniversalLink);
            NativeInterface.OnCustomSchemeUrlOpen    += HandleOnCustomSchemeUrlOpen;
            NativeInterface.OnUniversalLinkOpen      += HandleOnUniversalLinkOpen;
            NativeInterface.Init();
        }

        private static bool CanHandleCustomSchemeUrl(string url)
        {
            return (Delegate == null) || Delegate.CanHandleCustomSchemeUrl(new Uri(url));
        }

        private static bool CanHandleUniversalLink(string url)
        {
            return (Delegate == null) || Delegate.CanHandleUniversalLink(new Uri(url));
        }

        #endregion

        #region Callback methods

        private static void HandleOnCustomSchemeUrlOpen(string url)
        {
            DebugLogger.Log(EssentialKitDomain.Default, $"Detected url scheme: {url}");

            // notify listeners
            var     result      = new DeepLinkServicesDynamicLinkOpenResult(new Uri(url), url);

            if (OnCustomSchemeUrlOpen != null)
            {
                CallbackDispatcher.InvokeOnMainThread(OnCustomSchemeUrlOpen, result);
            }
            else
            {
                SurrogateCoroutine.WaitUntilAndInvoke(new WaitUntil(() => OnCustomSchemeUrlOpen != null), () =>
                {
                    CallbackDispatcher.InvokeOnMainThread(OnCustomSchemeUrlOpen, result);
                });
            }
        }

        private static void HandleOnUniversalLinkOpen(string url)
        {
            DebugLogger.Log(EssentialKitDomain.Default, $"Detected universal link: {url}");

            // notify listeners
            var     result      = new DeepLinkServicesDynamicLinkOpenResult(new Uri(url), url);

            if (OnUniversalLinkOpen != null)
            {
                CallbackDispatcher.InvokeOnMainThread(OnUniversalLinkOpen, result);
            }
            else
            {
                SurrogateCoroutine.WaitUntilAndInvoke(new WaitUntil(() => OnUniversalLinkOpen != null), () =>
                {
                    CallbackDispatcher.InvokeOnMainThread(OnUniversalLinkOpen, result);
                });
            }
        }

        #endregion
    }
}