#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;
using VoxelBusters.EssentialKit.Common.Android;

namespace VoxelBusters.EssentialKit.NotificationServicesCore.Android
{
    public class NativeLocationNotificationTrigger : NativeAndroidJavaObjectWrapper
    {
        #region Static properties

         private static AndroidJavaClass m_nativeClass;

        #endregion
        #region Public properties

        public bool Repeat
        {
            get
            {
                return Get<bool>("repeat");
            }

            set
            {
                Set<bool>("repeat", value);
            }
        }

        #endregion
        #region Constructor

        // Default constructor
        public NativeLocationNotificationTrigger(AndroidJavaObject androidJavaObject) : base(Native.kClassName, androidJavaObject)
        {
        }
        public NativeLocationNotificationTrigger(NativeAndroidJavaObjectWrapper wrapper) : base(wrapper)
        {
        }
        public NativeLocationNotificationTrigger(double latitude, double longitude, float radius, bool repeat) : base(Native.kClassName ,latitude, longitude, radius, repeat)
        {
        }

#if NATIVE_PLUGINS_DEBUG_ENABLED
        ~NativeLocationNotificationTrigger()
        {
            DebugLogger.Log("Disposing NativeLocationNotificationTrigger");
        }
#endif
        #endregion
        #region Static methods
        private static AndroidJavaClass GetClass()
        {
            if (m_nativeClass == null)
            {
                m_nativeClass = new AndroidJavaClass(Native.kClassName);
            }
            return m_nativeClass;
        }
        public static NativeLocationNotificationTrigger FromJson(NativeJSONObject jsonObject)
        {
            AndroidJavaObject nativeObj = GetClass().CallStatic<AndroidJavaObject>(Native.Method.kFromJson, jsonObject.NativeObject);
            NativeLocationNotificationTrigger data  = new  NativeLocationNotificationTrigger(nativeObj);
            return data;
        }

        #endregion
        #region Public methods

        public void Build()
        {
            Call(Native.Method.kBuild);
        }
        public NativePointF GetLocationCoordinate()
        {
            AndroidJavaObject nativeObj = Call<AndroidJavaObject>(Native.Method.kGetLocationCoordinate);
            NativePointF data  = new  NativePointF(nativeObj);
            return data;
        }
        public float GetRadius()
        {
            return Call<float>(Native.Method.kGetRadius);
        }
        public bool IsNotifyOnEntry()
        {
            return Call<bool>(Native.Method.kIsNotifyOnEntry);
        }
        public bool IsNotifyOnExit()
        {
            return Call<bool>(Native.Method.kIsNotifyOnExit);
        }
        public void SetNotifyOnEntry(bool notify)
        {
            Call(Native.Method.kSetNotifyOnEntry, notify);
        }
        public void SetNotifyOnExit(bool notify)
        {
            Call(Native.Method.kSetNotifyOnExit, notify);
        }
        public NativeJSONObject ToJson()
        {
            AndroidJavaObject nativeObj = Call<AndroidJavaObject>(Native.Method.kToJson);
            NativeJSONObject data  = new  NativeJSONObject(nativeObj);
            return data;
        }

        #endregion

        internal class Native
        {
            internal const string kClassName = "com.voxelbusters.essentialkit.notificationservices.datatypes.LocationNotificationTrigger";

            internal class Method
            {
                internal const string kFromJson = "fromJson";
                internal const string kGetLocationCoordinate = "getLocationCoordinate";
                internal const string kGetRadius = "getRadius";
                internal const string kIsNotifyOnExit = "isNotifyOnExit";
                internal const string kSetNotifyOnExit = "setNotifyOnExit";
                internal const string kIsNotifyOnEntry = "isNotifyOnEntry";
                internal const string kToJson = "toJson";
                internal const string kSetNotifyOnEntry = "setNotifyOnEntry";
                internal const string kBuild = "build";
            }

        }
    }
}
#endif