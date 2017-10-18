using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace TaxiForCore.Log
{
    /// <summary>
    /// 使用类似于
    /// Log.Instance.LogWrite(Thread.CurrentThread.ManagedThreadId, LogLevel.Warn, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, "测试");
    /// 或者直接传入 Exception
    /// Log.Instance.LogWrite(ex);
    /// </summary>
    public class LogHelper : IDisposable
    {
        public void Dispose()
        {
            _state = false;
        }
        private static LogHelper _instance = null;
        private static readonly object _synObject = new object();
        /// <summary>
        /// 单例模式
        /// </summary>
        public static LogHelper Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (_synObject)
                    {
                        if (null == _instance)
                        {
                            _instance = new LogHelper();
                        }
                    }
                }
                return _instance;
            }
        }

        #region 日志设定用变量
        /// <summary>
        /// 日志是否处于写入中的标识
        /// </summary>
        private bool _state;

        /// <summary>
        /// 日志写入的队列
        /// </summary>
        private static Queue<LogType> _logs;

        /// <summary>
        /// 日志默认存储路径，默认为程序路径
        /// </summary>
        private string _logDirectory = AppContext.BaseDirectory;
        public string LogDirectory
        {
            get { return _logDirectory; }
            set { _logDirectory = value; }
        }

        /// <summary>  
        /// 日志拆分类型  
        /// </summary>  
        private LogFileSplit _logFileSplit = LogFileSplit.Day;
        public LogFileSplit LogFileSplit
        {
            get { return _logFileSplit; }
            set { _logFileSplit = value; }
        }

        private LogLevel _currentLogLevel = LogLevel.Debug;
        /// <summary>  
        /// 日志记录等级  
        /// </summary>  
        public LogLevel CurrentMsgType
        {
            get { return _currentLogLevel; }
            set { _currentLogLevel = value; }
        }


        /// <summary>  
        /// 默认用于记录日志的文件名  
        /// </summary>  
        private string _currentFileName = "TaxiLog.log";

        private string _fileNamePrefix = "log_";
        /// <summary>  
        /// 日志的默认前缀名称，默认为log_  
        /// </summary>  
        public string FileNamePrefix
        {
            get { return _fileNamePrefix; }
            set { _fileNamePrefix = value; }
        }

        private string _logDirectoryName = "LogFile";

        /// <summary>
        /// 日志存储的主目录名称
        /// </summary>
        public string LogDirectoryName
        {
            get { return _logDirectoryName; }
            set { _logDirectoryName = value; }
        }

        /// <summary>  
        /// 单个日志文件默认大小(单位：兆)  
        /// </summary> 
        private int _maxFileSize = 5;
        public string MaxFileSize
        {
            get { return MaxFileSize; }
            set { MaxFileSize = value; }
        }

        /// <summary>  
        /// 文件后缀号,用于默认日志名中递增
        /// </summary>  
        private int _fileSymbol = 1;

        /// <summary>  
        /// 日志文件生命周期的时间标记  
        /// </summary>  
        private DateTime _CurrentFileTimeSign = new DateTime();

        /// <summary>  
        /// 当前文件大小(单位：B)  
        /// </summary>  
        private long _fileSize = 0;

        /// <summary>
        /// 日志写入线程启动/关闭标志
        /// </summary>
        public bool OnLogThread = true;

        /// <summary>
        /// 日志主线程
        /// </summary>
        Thread thread = null;

        #endregion

        private LogHelper()
        {
            if (_logs == null)
            {
                GetCurrentFilename();
                _state = true;
                _logs = new Queue<LogType>();
                thread = new Thread(Work);
                thread.Start();
            }
        }

        /// <summary>
        /// 日志文件写入线程执行的方法
        /// </summary>
        private void Work()
        {
            
            while (OnLogThread)
            {
                //判断队列中是否存在待写入的日志  
                if (_logs.Count > 0)
                {
                    LogType l = null;
                    lock (_logs)
                    {
                        l = _logs.Dequeue();

                        if (l != null)
                        {
                            FileWrite(l);
                        }
                    }
                }
                else
                {
                    //判断是否已经发出终止日志并关闭的消息  
                    if (_state)
                    {
                        Thread.Sleep(1);
                    }
                    else
                    {
                        Abort(thread);
                    }
                }
            }
        }


        private static void Abort(Thread thread)
        {
            MethodInfo abort = null;
            foreach (MethodInfo m in thread.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (m.Name.Equals("AbortInternal") && m.GetParameters().Length == 0)
                    abort = m;
            }

            if (abort == null)
            {
                throw new Exception("Failed to get Thread.Abort method");
            }

            abort.Invoke(thread, new object[0]);
        }


        private void FileWrite(LogType log)
        {
            try
            {
                //判断文件到期标志,如果当前文件到期则关闭当前文件创建新的日志文件  
                if ((_logFileSplit != LogFileSplit.Size && DateTime.Now >= _CurrentFileTimeSign) ||
                        (_logFileSplit == LogFileSplit.Size && ((double)_fileSize / 1048576) > _maxFileSize))
                {
                    GetCurrentFilename();
                }
                using (FileStream fs = new FileStream(Path.Combine(LogDirectory ,_currentFileName), FileMode.Append))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        // 日志文件模板
                        sw.WriteLine($"记录时间: {log.DateTime}");
                        sw.WriteLine($"日志级别：{log.LogLevel.ToString()}");
                        sw.WriteLine($"出错类： {log.ErrorClass}");
                        sw.WriteLine($"行号： {log.LineNumber.ToString()}");
                        sw.WriteLine($"错误描述：{log.ErrorMessage}");
                        sw.WriteLine($"错误详细：{log.ErrorDetails}");
                        sw.WriteLine("");
                        sw.WriteLine("--------------------------------------------------------------\n");
                        sw.WriteLine("");
                        _fileSize += Encoding.UTF8.GetBytes(log.ToString()).Length;
                        sw.Flush();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("写入出现异常");
                Console.WriteLine(e);
            }
        }


        /// <summary>
        /// 根据年月生成目录
        /// </summary>
        /// <param name="currentDate">传入需要创建的时间</param>
        private void BuiderDir(DateTime currentDate)
        {
            string year = currentDate.ToString("yyyy");
            string month = currentDate.ToString("MM");
            //   年/年月
            //string subdir = string.Concat(_logDirectoryName, '\\', year, '\\', year + month, '\\');
            string subdir = Path.Combine(_logDirectoryName, year, year + month);
            string path = Path.Combine(LogDirectory, subdir);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            LogDirectory = path;
        }

        /// <summary>
        /// 日志写入,依据logType
        /// </summary>
        private void LogWrite(LogType log)
        {
            //enum 类型 等同 (int)msg.type < (int)CurrentMsgType  
            if (log.LogLevel < CurrentMsgType)
                return;
            if (log != null)
            {
                lock (_logs)
                {
                    _logs.Enqueue(log);
                }
            }
        }

        /// <summary>
        /// 日志写入
        /// </summary>
        /// <param name="dt">日志时间</param>
        /// <param name="lineNumber">线程ID</param>
        /// <param name="level">日志等级</param>
        /// <param name="errorClass">日志触发的类名</param>
        /// <param name="errorMessage">日志信息</param>
        public void LogWrite(DateTime dt, int lineNumber, LogLevel level, string errorClass, string errorMessage, string errorDetails)
        {
            LogWrite(new LogType(dt, lineNumber, level, errorClass, errorMessage, errorDetails));
        }

        /// <summary>
        /// 日志写入，时间默认为当前时间
        /// </summary>
        /// <param name="lineNumber">线程ID</param>
        /// <param name="level">日志等级</param>
        /// <param name="errorClass">日志触发的类名</param>
        /// <param name="errorMessage">日志信息</param>
        public void LogWrite(int lineNumber, LogLevel level, string errorClass, string errorMessage, string errorDetails)
        {
            LogWrite(new LogType(DateTime.Now, lineNumber, level, errorClass, errorMessage, errorDetails));
        }

        /// <summary>
        /// 日志写入
        /// </summary>
        /// <param name="text">日志错误信息</param>
        /// <param name="level">日志等级</param>
        public void LogWrite(string text, LogLevel level)
        {
            LogWrite(new LogType(text, level));
        }

        /// <summary>
        /// 日志写入
        /// </summary>
        /// <param name="text">日志错误信息</param>
        public void LogWrite(string text)
        {
            LogWrite(text, LogLevel.Debug);
        }

        /// <summary>
        /// 日志写入，传入类型为Exception
        /// </summary>
        /// <param name="e">Exception 类型</param>
        public void LogWrite(Exception ex, string errorMessage = "")
        {
            var trace = new StackTrace(ex, true);
            var frame = trace.GetFrames().Last();
            var lineNumber = frame.GetFileLineNumber();
            var fileName = frame.GetFileName();
            LogWrite(new LogType(DateTime.Now, lineNumber, LogLevel.Error, frame.GetMethod().Name, errorMessage, ex.ToString()));
        }

        /// <summary>
        /// 设定日志名称
        /// </summary>
        private void GetCurrentFilename()
        {
            DateTime now = DateTime.Now;
            string format = "";
            switch (_logFileSplit)
            {
                case LogFileSplit.Day:
                    _CurrentFileTimeSign = new DateTime(now.Year, now.Month, now.Day);
                    _CurrentFileTimeSign = _CurrentFileTimeSign.AddDays(1);
                    format = now.ToString("yyyyMMdd'.log'");
                    BuiderDir(now);
                    break;
                case LogFileSplit.Week:
                    _CurrentFileTimeSign = new DateTime(now.Year, now.Month, now.Day);
                    _CurrentFileTimeSign = _CurrentFileTimeSign.AddDays(7);
                    format = now.ToString("yyyyMMdd'.log'");
                    BuiderDir(now);
                    break;
                case LogFileSplit.Month:
                    _CurrentFileTimeSign = new DateTime(now.Year, now.Month, 1);
                    _CurrentFileTimeSign = _CurrentFileTimeSign.AddMonths(1);
                    format = now.ToString("yyyyMM'.log'");
                    BuiderDir(now);
                    break;
                default:
                    _fileSymbol++;
                    format = _fileSymbol.ToString() + ".log";
                    break;
            }
            // 如果文件存在则获取当前日志大小
            if (File.Exists(Path.Combine(LogDirectory, _currentFileName)))
            {
                _fileSize = new FileInfo(Path.Combine(LogDirectory, _currentFileName)).Length;
            }
            else
            {
                _fileSize = 0;
            }
            _currentFileName = _fileNamePrefix + format.Trim();
        }
    }

    /// <summary>
    /// 日志基础类型
    /// </summary>
    public class LogType
    {
        public LogType(string errorMessage) : this(errorMessage, LogLevel.Debug)
        { }

        public LogType(string errorMessage, LogLevel level) : this(DateTime.Now, 0, level, "", errorMessage, "")
        { }

        public LogType(DateTime dt, int lineNumber, LogLevel level, string errorClass, string errorMessage, string errorDetails)
        {
            DateTime = dt;
            LineNumber = lineNumber;
            LogLevel = level;
            ErrorClass = errorClass;
            ErrorMessage = errorMessage;
            ErrorDetails = errorDetails;
        }

        /// <summary>
        /// 日志时间
        /// </summary>
        public DateTime DateTime { get; set; }

        private int _lineNumber = -1;
        /// <summary>
        /// 线程ID
        /// </summary>
        public int LineNumber
        {
            get { return _lineNumber; }
            set { _lineNumber = value; }
        }
        /// <summary>
        /// 日志等级
        /// </summary>
        public LogLevel LogLevel { get; set; }
        /// <summary>
        /// 日志触发的类名
        /// </summary>
        public string ErrorClass { get; set; }
        /// <summary>
        /// 日志信息
        /// </summary>
        public string ErrorMessage { get; set; }
        /// <summary>
        /// 日志详细信息
        /// </summary>
        public string ErrorDetails { get; set; }
    }

    /// <summary>
    /// 日志生成模式枚举
    /// </summary>
    public enum LogFileSplit
    {
        /// <summary>  
        /// 每天创建一个日志 
        /// </summary>  
        Day,
        /// <summary>  
        /// 每周创建一个日志  
        /// </summary>  
        Week,
        /// <summary>  
        /// 每月创建一个日志  
        /// </summary>  
        Month,
        /// <summary>  
        /// 根据尺寸来切分日志
        /// </summary>  
        Size
    }

    /// <summary>
    /// 日志等级
    /// </summary>
    public enum LogLevel
    {

        /// <summary>
        /// 调试信息
        /// </summary>
        Debug = 0,
        /// <summary>
        /// 一般信息
        /// </summary>
        Info = 1,
        /// <summary>
        /// 警告信息
        /// </summary>
        Warn = 2,
        /// <summary>
        /// 错误日志
        /// </summary>
        Error = 3,
        /// <summary>
        /// 严重错误
        /// </summary>
        Fatal = 4


    }

}
