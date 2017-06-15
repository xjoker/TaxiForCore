# TaxiForCore 库是个人对于C#常用功能封装

本库基于.NET CORE 1.1

===


## 目前支持功能：

### 数组操作类
* ArrayHelper.JoinToString 数组转为String类型
* ArrayHelper.SlowEquals 慢速比较byte类型


### 时间类
* DateTimeHelper.StampToDateTime 将UNIX时间戳转为时间
* DateTimeHelper.DateTimeToStamp 将DateTime时间格式转换为Unix时间戳格式
* DateTimeHelper.LastDayOfMonth 取得某月的第一天
* DateTimeHelper.FirstDayOfPreviousMonth 取得上个月第一天
* DateTimeHelper.LastDayOfPrdviousMonth 取得上个月的最后一天
* DateTimeHelper.LastDayOfYear 获取年份的最后一天
* DateTimeHelper.GetWeekDayEnglish 获取星期X 描述(英文)
* DateTimeHelper.GetWeekDayChinese 获取星期X 描述(中文)
* DateTimeHelper.FormatDate 格式化日期 重载：(格式 yyyy-MM-dd HH:mm:ss) (格式yyyy-MM-dd)
* DateTimeHelper.IsLeapYear 判断年份是否为闰年
* DateTimeHelper.IsWeekEnd 判断日期是否为周末 可选是否不判断周六


### 字典类型
* DictionaryHelper.MergeDictionaryAdd 字典合并拓展 如果不存在key才添加，存在则不报错直接忽略
* DictionaryHelper.MergeDictionaryReplace 字典合并拓展 如果不存在key则添加，存在则替换,替换法则为second替换first
* DictionaryHelper.GetValue 获取字典key的值，如果没有结果则返回空或者指定的值
* DictionaryHelper.AddRange 向字典中批量添加键值对
* DictionaryHelper.DictionarySort 字典简单排序(默认以值做基准正向排序)
* DictionaryHelper.DictionaryEqual 字典相等比较


### AES
* AESEncryptsAndDecrypts.Encrypt AES 加密模块
* AESEncryptsAndDecrypts.Decrypt AES 解密模块
* AESEncryptsAndDecrypts.SimpleEncrypt AES 加密模块 可使用任意长度密钥 无需IV
* AESEncryptsAndDecrypts.SimpleDecrypt AES 解密模块 只可以解密由 SimpleEncrypt 加密的

### 密码加密/校验
* PasswordHash.CreateHash 密码加密方法 sha1
* PasswordHash.ValidatePassword 校验密码

### MD5加密/校验
* MD5Hash.GetMd5Hash MD5加密
* MD5Hash.VerifyMd5Hash MD5校验

### Token生成和校验
* TokenHelper.GenTokenString 只生成TOKEN不返回GUID
* TokenHelper.CheckTokenBool 只返回校验结果不返回GUID
* TokenHelper.GenToken 用于生成TOKEN 返回key为GUID,value为token
* TokenHelper.CheckToken 用于校验TOKEN是否过期 


### htpasswd 加密
* UnixMd5CryptTool.crypt Apache MD5加密


### 文件类
* FileHelper.ReadFile 读取文件
* FileHelper.SetDirectoryAccessRule 设定目录完全访问权限
* FileHelper.GetParentDirectoryName 返回上层路径的文件夹名
* FileHelper.GetGrampaDirectoryName 返回当前路径上两层的文件夹名
* FileHelper.IsSubdirectory 判断是否为子目录
* FileHelper.EnsureTrailingSlash 为目录路径结尾增加"/"
* FileHelper.GetAbsolutePath 获取绝对路径
* FileHelper.EnsureTrailingCharacter 路径尾部增加字符
* FileHelper.ReadFileForAll 一次性读取文件所有内容
* FileHelper.WriteFile 写入文件 (自定义编码) 重载：(以UTF-8格式写入文件)
* FileHelper.GetFileList 搜索路径下特定名称的文件并返回
* FileHelper.DirectoryExists 目录是否存在
* FileHelper.FileExists 文件是否存在
* FileHelper.OpenFile 打开文件(返回Stream)
* FileHelper.CreateDirectory 创建目录
* FileHelper.AddFile Stream流写入文件
* FileHelper.MoveFile 移动文件，路径需完整并且包含源/目标文件名
* FileHelper.GetAllDirectorys 获取一个目录下的所有目录(递归实现)
* FileHelper.GetAllFiles 获取一个目录下的所有文件(递归实现)
* FileHelper.RenameFile 文件重命名
* FileHelper.GetFileMD5 计算文件MD5值
* FileHelper.GetFileSHA1 计算文件SHA1值


### ini 文件操作
* IniFileHelper.ReadSections 返回该配置文件中所有Section名称的集合
* IniFileHelper.ReadKeys  获取节点的所有KEY值
* IniFileHelper.IniWriteValue 写INI文件
* IniFileHelper.IniReadValue 读取INI文件
* IniFileHelper.ClearAllSection 删除ini文件下所有段落
* IniFileHelper.ClearSection 删除ini文件下personal段落下的所有键


### List类
* ListHelper.GetLast 获取List内最后一个元素
* ListHelper.Search List 搜索功能
* ListHelper.Compare List 对比相等


### 简便的日志类
* LogHelper.LogWrite 日志写入


### 网络检测
* NetworkTools.Ping  Ping测试 返回完整的PingReply
* NetworkTools.PingCheck  Ping测试 返回是否能通
* NetworkTools.PingDelay  Ping测试延时
* NetworkTools.PingCheckDetailed  PING 检测，可定义次数和间隔
* NetworkTools.DomainToIP  域名解析为IP地址
* NetworkTools.GetLocalIPs  获取本机所有网卡的IP地址
* NetworkTools.GetLocalMacs  获取本机所有网卡的MAC地址
* NetworkTools.GetAllUsePort  获取所有在用状态的TCP/UDP端口
* NetworkTools.CheckTCPPortIsUse  检测TCP端口是否已被使用
* NetworkTools.CheckUDPPortIsUse  检测UDP端口是否已被使用


<del>### Windows 共享操作</del>
<del>* WindowsShareFolder.ShareFolder 文件夹共享</del>
<del>* WindowsShareFolder.Delete 删除共享</del>
<del>* WindowsShareFolder.GetLocalPathForShare 获取共享名称对应路径</del>
<del>* WindowsShareFolder.ShareFolder 文件夹共享</del>
<del>* WindowsShareFolder.ShareFolder 文件夹共享</del>


### String 类
* StringHelper.Decode 转换为指定编码
* StringHelper.IsNullOrEmpty 封装 IsNullOrEmpty为Bool
* StringHelper.IsNullOrWhiteSpace 封装 IsNullOrWhiteSpace为Bool
* StringHelper.IsInt 判断是否为int类型
* StringHelper.ToInt 转换为int类型
<del>* StringHelper.IsValidJson 鉴定是否为JSON格式</del>
* StringHelper.IsEqualsString 字符串相等判断
* StringHelper.ContainsIgnoreCase 忽略大小写对比
* StringHelper.WriteToFile 将文本快速写入文件
* StringHelper.Decode 转换为指定编码
* StringHelper.Decode 转换为指定编码
* StringHelper.Decode 转换为指定编码


### 系统类
* CheckRunAs.IsRunAsAdmin 检测程序当前是否以管理员级别启动
* CheckRunAs.RunAsUser 获取当前启动的用户名称


<del>### WMI</del>
<del>* WMIHelper.WMIRemoteQueryHelper  WMI 远程查询帮助类 多个重载</del>
<del>* WMIHelper.WMIQueryHelper  WMI 查询帮助类 多个重载</del>


### 随机帮助类
* RandomHelper.GenerateRandomNumber 生成随机数字,取值范围为(0-9)
* RandomHelper.GenerateRandomString 生成随机字符串,字符范围为(0-9 a-b A-B)
* RandomHelper.GenerateRandomString 生成随机字符串,字符范围为(a-b)或者(A-B)
* RandomHelper.GenerateRandomPassword 生成随机密码
* RandomHelper.GetRandomString  随机类的基础生成方法


### 枚举帮助类
* EnumHelper.ToEnum  String类型转为对应的Enum类型
* EnumHelper.ToEnumName  将枚举类型的枚举项转换为List<String>
* EnumHelper.GetIntValue  获得Enum项对应的数值
* EnumHelper.GetDescription  获取Enum项的描述信息
* EnumHelper.ToDictionary  Enum 转换为字典类型 Dictionary<int, string>