﻿using System;
using System.IO;
using System.Net.Http;

namespace Engine
{
    public static class Logger
    {
        /// <summary>
        /// Stopping current logger session
        /// </summary>
        public static void Stop(string message="logging over")
        {
            if (!Directory.Exists("logs")) return;
            
            Log(message, "stop");

            var s = File.ReadAllText("logs\\.now");

            File.AppendAllText($"logs\\{DateTime.Now.ToString().Replace(':','.')}.log", s);
            File.Delete("logs\\.now");
        }
        
        /// <summary>
        /// Starting new logger session
        /// Closing existing session
        /// </summary>
        public static void Initialise()
        {
            if (!Directory.Exists("logs"))
                Directory.CreateDirectory("logs");
            if (File.Exists("logs\\.now"))
            {
                Log(".now file was not deleted", "logger error");
                Stop();
            }

            Directory.CreateDirectory("logs");
            File.CreateText("logs\\.now").Close();
            Log("initialising", "init");
        }
        
        /// <summary>
        /// Add message to your session
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="logType">Type of message, like tag</param>
        /// <exception cref="Exception">Initialise before logging</exception>
        public static void Log(object message, string logType = "log message")
        {
            if (File.Exists("logs\\.now"))
            {
                File.AppendAllText("logs\\.now", $"\n[{logType.ToUpper()}][{DateTime.Now.ToString().Replace(':','.')}]: {message.ToString()}");
            }
            else
            {
                throw new Exception("Initialise before logging");
            }
        }

        /// <summary>
        /// Show existence of session in current time
        /// </summary>
        public static bool IsSessionExist => Directory.Exists("logs") && File.Exists("logs\\.now");
    }
}