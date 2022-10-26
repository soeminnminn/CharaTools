using System;
using System.Linq;

namespace CharaTools.Plugin
{
    internal class Manager : AssmLoader<IPlugin>
    {
        #region Variables
        internal const string c_prefix = "Plugin.";
        #endregion

        #region Constructor
        public Manager()
            : base(AppDomain.CurrentDomain.BaseDirectory)
        {
            string systemPath = AppDomain.CurrentDomain.BaseDirectory;
            Load(systemPath, $"{c_prefix}*.dll");
        }
        #endregion

        #region Static
        private static Manager instance = null;

        public static Manager Plugins
        {
            get
            {
                if (instance == null)
                    instance = new Manager();
                return instance;
            }
        }

        public static void SendOnPluginLoaded()
        {
            foreach (var p in Plugins)
            {
                if (p.Module != null)
                    p.Module.OnPluginLoaded();
            }
        }

        public static void SendOnChaFileLoaded(AIChara.ChaFile file)
        {
            foreach (var p in Plugins)
            {
                if (p.Module != null)
                    p.Module.OnChaFileLoaded(file);
            }
        }

        public static void SendOnChaFileBeforeSave(AIChara.ChaFile file)
        {
            foreach (var p in Plugins)
            {
                if (p.Module != null)
                    p.Module.OnChaFileBeforeSave(file);
            }
        }

        public static void SendOnCoordinateLoaded(AIChara.ChaFileCoordinate file)
        {
            foreach (var p in Plugins)
            {
                if (p.Module != null)
                    p.Module.OnCoordinateLoaded(file);
            }
        }

        public static IPlugin GetDeserializer(string extKey)
        {
            var p = Plugins.FirstOrDefault(x => x.Module.CanDeserialize(extKey));
            return p != null ? p.Module : null;
        }
        #endregion
    }
}
