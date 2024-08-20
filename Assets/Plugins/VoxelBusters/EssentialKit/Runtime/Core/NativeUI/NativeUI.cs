using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.EssentialKit.NativeUICore;
using VoxelBusters.CoreLibrary.NativePlugins;
using System;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Provides a cross-platform interface to access native UI components.
    /// </summary>
    public static class NativeUI
    {
        #region Static fields

        [ClearOnReload]
        private     static  INativeUIInterface      s_nativeInterface   = null;

        #endregion

        #region Static properties

        public     static  INativeUIInterface    NativeInterface
        {
            get
            {
                if(s_nativeInterface == null)
                {
                    DebugLogger.LogException(EssentialKitDomain.Default, new Exception("Native UI is not enabled. You need to enable this feature to use it."));
                }

                return s_nativeInterface;
            }

            set
            {
                s_nativeInterface = value;
            }
        }

        public static NativeUIUnitySettings UnitySettings { get; private set; }

        #endregion

        #region Static methods

        public static void Initialize(NativeUIUnitySettings settings)
        {
            Assert.IsArgNotNull(settings, nameof(settings));

            // Set properties
            UnitySettings = settings;

            // Configure interface
            NativeInterface = NativeFeatureActivator.CreateInterface<INativeUIInterface>(ImplementationSchema.NativeUI, true);
        }

        /// <summary>
        /// Creates a new alert dialog with specified values.
        /// </summary>
        /// <param name="title">The title of the alert.</param>
        /// <param name="message">The descriptive text that provides more details.</param>
        /// <param name="preferredActionLabel">The title of the button.</param>
        /// <param name="preferredActionCallback">The method to execute when the user selects preferred action button.</param>
        /// <param name="cancelActionLabel">The title of the cancel button.</param>
        /// <param name="cancelActionCallback">The method to execute when the user selects cancel button.</param>
        public static void ShowAlertDialog(string title, string message, string preferredActionLabel, Callback preferredActionCallback = null, string cancelActionLabel = null, Callback cancelActionCallback = null)
        {
            var     newInstance     = AlertDialog.CreateInstance();
            newInstance.Title       = title;
            newInstance.Message     = message;
            if (preferredActionLabel != null)
            {
                newInstance.AddButton(preferredActionLabel, preferredActionCallback);
            }
            if (cancelActionLabel != null)
            {
                newInstance.AddCancelButton(cancelActionLabel, cancelActionCallback);
            }
            newInstance.Show();
        }

        #endregion
    }
}