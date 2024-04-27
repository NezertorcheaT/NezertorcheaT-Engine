using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Engine.Core
{
    public static class Taber
    {
        private static readonly string FirstLines = "+--";
        private static readonly string MidLines = "---";
        private static readonly string LastLines = "-->";
        private static readonly string FullLines = "+->";

        public static string Tabs(int n)
        {
            if (n <= 0) return string.Empty;
            if (n == 1) return FullLines;
            var str = new StringBuilder();
            str.Append(FirstLines);
            str.Append(MidLines.Multiply(n - 2));
            str.Append(LastLines);
            return str.ToString();
        }
    }

    public static class Logger
    {
        private static readonly string Lines = "   ";
        private static string TempNow => $"{Logs}\\temp({CurrentProcessId}).now";
        private static string Logs => "logs";
        private static string CurrentProcessId => Environment.ProcessId.ToString();
        private static DateTime StartTime => Process.GetCurrentProcess().StartTime;

        /// <summary>
        /// Stopping current logger session
        /// </summary>
        public static void Stop(string message = "logging over")
        {
            if (!Directory.Exists(Logs)) return;

            Log(message, "stop");

            var s = File.ReadAllText(TempNow);

            File.AppendAllText(
                $"{Logs}\\{StartTime.ToString().Replace(':', '.')} - {DateTime.Now.ToString().Replace(':', '.')} [{CurrentProcessId}].log",
                s);
            File.Delete(TempNow);
        }

        /// <summary>
        /// Starting new logger session
        /// Closing existing session
        /// </summary>
        public static void Initialise()
        {
            if (!Directory.Exists(Logs))
                Directory.CreateDirectory(Logs);
            if (IsSessionExist)
            {
                Log("temp.now file was not deleted", "logger error");
                Stop();
            }

            File.CreateText(TempNow).Close();
            Log("initialising", "init");
        }

        /// <summary>
        /// Add message to your session
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="logType">Type of message, like tag</param>
        /// <exception cref="Exception">Initialise before logging</exception>
        public static void Log(object? message, string logType = "info", int tabs = 0, int firstTabs = 0)
        {
            if (IsSessionExist)
            {
                try
                {
                    File.AppendAllText(TempNow,
                        $"\n{Lines.Multiply(firstTabs)}{Taber.Tabs(tabs - firstTabs)}[{logType.ToUpper()}][{DateTime.Now.ToString().Replace(':', '.')}]: {message}");
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
            string logType = "assert message")
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
        public static bool IsSessionExist => Directory.Exists(Logs) && File.Exists(TempNow);
    }
}