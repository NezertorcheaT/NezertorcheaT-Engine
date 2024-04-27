using System;
using System.Diagnostics;
using System.IO;

namespace Engine.Core
{
    public static class Logger
    {
        private static readonly string Lines = "   ";
        private static string TempNow => $"logs\\temp({CurrentProcessId}).now";
        private static string CurrentProcessId => Environment.ProcessId.ToString();
        private static DateTime StartTime => Process.GetCurrentProcess().StartTime;

        /// <summary>
        /// Stopping current logger session
        /// </summary>
        public static void Stop(string message = "logging over")
        {
            if (!Directory.Exists("logs")) return;

            Log(message, "stop");

            var s = File.ReadAllText(TempNow);

            File.AppendAllText(
                $"logs\\{StartTime.ToString().Replace(':', '.')} - {DateTime.Now.ToString().Replace(':', '.')} [{CurrentProcessId}].log",
                s);
            File.Delete(TempNow);
        }

        /// <summary>
        /// Starting new logger session
        /// Closing existing session
        /// </summary>
        public static void Initialise()
        {
            if (!Directory.Exists("logs"))
                Directory.CreateDirectory("logs");
            if (File.Exists(TempNow))
            {
                Log("temp.now file was not deleted", "logger error");
                Stop();
            }

            Directory.CreateDirectory("logs");
            File.CreateText(TempNow).Close();
            Log("initialising", "init");
        }

        /// <summary>
        /// Add message to your session
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="logType">Type of message, like tag</param>
        /// <exception cref="Exception">Initialise before logging</exception>
        public static void Log(object? message, string logType = "info", int tabs = 0)
        {
            if (File.Exists(TempNow))
            {
                try
                {
                    File.AppendAllText(TempNow,
                        $"\n{Lines.Multiply(Math.Max(tabs, 0))}[{logType.ToUpper()}][{DateTime.Now.ToString().Replace(':', '.')}]: {message}");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Console.ReadKey();
                    throw;
                }

                return;
            }

            throw new Exception("Initialise before logging");
        }

        public static void Assert(bool condition, object falseMessage, string logType = "assert message")
        {
            if (!condition) Log(falseMessage, logType);
        }

        public static void Assert<T>(Func<T> condition, T consequent, object falseMessage,
            string logType = "log message")
        {
            T operand;
            try
            {
                operand = condition();
            }
            catch (Exception e)
            {
                Assert(false, falseMessage, logType);
                Log(e, "Assert error");
                return;
            }

            Assert(operand != null && operand.Equals(consequent), falseMessage, logType);
        }

        /// <summary>
        /// Show existence of session in current time
        /// </summary>
        public static bool IsSessionExist => Directory.Exists("logs") && File.Exists(TempNow);
    }
}