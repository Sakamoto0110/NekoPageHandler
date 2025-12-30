using PageNav.Contracts.Pages;
using PageNav.Core.Services;
using PageNav.Metadata;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PageNav.Core.Services.NavigationService;

namespace PageNav.Diagnostics
{
    public static class PageLoggerService
    {
        private const int MAX_LOG = 500;
        private static readonly List<PageLogEntry> _entries = new List<PageLogEntry>();


        public static void OnNavigationSuccess(IPageView from, IPageView to, NavigationArgs args) 
        {
            LogNavigation(new PageLogEntry(from?.GetType(),from?.Name,to?.GetType(),to?.Name,args,success: true));
        }
        public static void OnNavigationFailure(IPageView from, IPageView to, NavigationArgs args) 
        {
            LogNavigation(new PageLogEntry(from?.GetType(),from?.Name,to?.GetType(),to?.Name,args,success: false));
        }
        public static void OnNavigationTimedout(IPageView from, IPageView to, NavigationArgs args) 
        {
            LogNavigation(new PageLogEntry(from?.GetType(),from?.Name,to?.GetType(),to?.Name,args,isTimeout: true,success : true));
        }
        public static void OnNavigationBackwarded(IPageView from, IPageView to, NavigationArgs args)
        {
            LogNavigation(new PageLogEntry(from?.GetType(),from?.Name,to?.GetType(),to?.Name,args,isBackNavigation: true,success: true));
        }
        public static void LogNavigation(PageLogEntry log)
        {
            _entries.Add(log);
            if(_entries.Count > MAX_LOG)
                _entries.RemoveRange(0, _entries.Count - MAX_LOG);
        }

        public static IEnumerable<PageLogEntry> All => _entries;

        public static IEnumerable<PageLogEntry> Last(int count) =>
            _entries.Count <= count ? _entries : _entries.GetRange(_entries.Count - count, count);
    }



    /// <summary>
    /// Very simple logger used for internal registry operations.
    /// Replace with your real logger as needed.
    /// </summary>
    public static class PageLogger
    {
        private static readonly object _lock = new object();
        private static string _logPath = "page_registry.log";

        /// <summary>
        /// Sets the file where logs should be written.
        /// </summary>
        public static void SetLogFile(string path)
        {
            lock(_lock)
            {
                _logPath = path;
            }
        }

        /// <summary>
        /// Logs an event with timestamp and category.
        /// </summary>
        public static void Log(string category, string message)
        {
            lock(_lock)
            {
                var str = $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}][{category}]{message}";
                Console.WriteLine(str);
                try
                {
                    File.AppendAllText(_logPath, str);
                }catch(IOException ex) { Console.WriteLine(ex.ToString()); }
            }
        }

        public static void LogInfo(string msg) { Log("INFO", msg); }
        public static void LogWarn(string msg) { Log("WARN", msg); }
        public static void LogError(string msg) { Log("ERROR", msg); }
    }


}
