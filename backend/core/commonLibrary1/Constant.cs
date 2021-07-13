using System;

namespace CommonLibrary1
{
    public class Constant
    {
        public static string connectionString = "server=42.192.76.159;uid=difu;pwd=1234;port=3306;database=lunhui;SslMode=None";

        // 状态码
        public class DeadStatus
        {
            public const int DELETED = 0;
            public const int NOTCALCULATED = 1;
            public const int CALCULATED = 2;
            public const int PASSED = 3;
        }

        public class ZhuanshengStatus
        {
            public const int DELETED = 0;
            public const int WAITING = 1;
            public const int PASSED = 2;
        }

        public class Toutai
        {
            public const int DELETED = 0;
            public const int PASSED = 1;
        }

        // 待启用
        public class SpeciesStatus
        {
            public const int DELETED = 0;
            public const int EXISTED = 1;
        }
    }
}
