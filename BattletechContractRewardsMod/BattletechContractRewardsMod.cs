using Harmony;
using System.Diagnostics;
using System.Reflection;
using System;
using System.IO;

namespace BattletechContractRewardsMod {
    public class Mod {
        public static Logger Logger;

        public static void Init(string modDirectory) {
            Logger = new Logger(modDirectory, "contract_rewards");

            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

            Logger.Info("Loading mod..");
            Logger.Info($"Assembly version: {fileVersionInfo.ProductVersion}");
            Logger.Debug($"Mod directory: {modDirectory}");

            HarmonyInstance.DEBUG = true;
            var harmony = HarmonyInstance.Create("cm.elliot.battletech.mod.ContractRewards");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }

    public class Logger {
        private static StreamWriter LogStream;
        private static string LogFile;

        public Logger(string modDir, string logName) {
            if (LogFile == null) {
                LogFile = Path.Combine(modDir, $"{logName}.log");
            }
            if (File.Exists(LogFile)) {
                File.Delete(LogFile);
            }

            LogStream = File.AppendText(LogFile);
        }

        //public void Debug(string message) { if (Mod.Config.Debug) { Info(message); } }
        //public void Trace(string message) { if (Mod.Config.Trace) { Info(message); } }
        public void Debug(string message) { Info(message, "DEBUG"); }
        public void Trace(string message) { Info(message, "TRACE"); }

        public void Info(string message, string type = "INFO ") {
            string now = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
            LogStream.WriteLine($"[{type}] {now}: {message}");
            LogStream.Flush();
        }

    }
}