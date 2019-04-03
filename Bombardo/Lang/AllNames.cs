namespace Bombardo
{
    //  Переношу все имена сюда дабы иметь возмож
    public static class AllNames
    {
        public static readonly string NULL_SYMBOL = "null";
        public static readonly string EMPTY_SYMBOL = "empty";

        #region Base lisp symbols

        public static readonly string LISP_CAR = "car";
        public static readonly string LISP_CDR = "cdr";
        public static readonly string LISP_CONS = "cons";
        public static readonly string LISP_GET = "get";
        public static readonly string LISP_LAST = "last";
        public static readonly string LISP_APPEND = "append";
        public static readonly string LISP_LIST = "list";
        public static readonly string LISP_REVERSE = "reverse";
        public static readonly string LISP_SET_CAR = "set-car!";
        public static readonly string LISP_SET_CDR = "set-cdr!";
        public static readonly string LISP_EACH = "each";
        public static readonly string LISP_MAP = "map";
        public static readonly string LISP_FILTER = "filter";

        public static readonly string LISP_CAAR = "caar";
        public static readonly string LISP_CADR = "cadr";
        public static readonly string LISP_CDAR = "cdar";
        public static readonly string LISP_CDDR = "cddr";

        public static readonly string LISP_CAAAR = "caaar";
        public static readonly string LISP_CAADR = "caadr";
        public static readonly string LISP_CADAR = "cadar";
        public static readonly string LISP_CADDR = "caddr";
        public static readonly string LISP_CDAAR = "cdaar";
        public static readonly string LISP_CDADR = "cdadr";
        public static readonly string LISP_CDDAR = "cddar";
        public static readonly string LISP_CDDDR = "cdddr";

        public static readonly string LISP_CAAAAR = "caaaar";
        public static readonly string LISP_CAAADR = "caaadr";
        public static readonly string LISP_CAADAR = "caadar";
        public static readonly string LISP_CAADDR = "caaddr";
        public static readonly string LISP_CADAAR = "cadaar";
        public static readonly string LISP_CADADR = "cadadr";
        public static readonly string LISP_CADDAR = "caddar";
        public static readonly string LISP_CADDDR = "cadddr";
        public static readonly string LISP_CDAAAR = "cdaaar";
        public static readonly string LISP_CDAADR = "cdaadr";
        public static readonly string LISP_CDADAR = "cdadar";
        public static readonly string LISP_CDADDR = "cdaddr";
        public static readonly string LISP_CDDAAR = "cddaar";
        public static readonly string LISP_CDDADR = "cddadr";
        public static readonly string LISP_CDDDAR = "cdddar";
        public static readonly string LISP_CDDDDR = "cddddr";

        #endregion

        #region Main lisp symbols

        public static readonly string LISP_NOPE = "nope";
        public static readonly string LISP_MARKER = "marker";
        public static readonly string LISP_QUOTE = "quote";

        public static readonly string LISP_PARSE = "parse";
        public static readonly string LISP_EVAL = "eval";
        public static readonly string LISP_EVALEACH = "block";

        public static readonly string LISP_COND = "cond";
        public static readonly string LISP_IF = "if";
        public static readonly string LISP_WHILE = "while";
        public static readonly string LISP_UNTIL = "until";

        public static readonly string LISP_LAMBDA = "lambda";
        public static readonly string LISP_MACROS = "macros";
        public static readonly string LISP_SYNTAX = "syntax";

        public static readonly string LISP_APPLY = "apply";
        public static readonly string LISP_MACROEXPAND = "macro-expand";

        public static readonly string LISP_ERROR = "error";



        public static readonly string LISP_PRINT = "print";
        public static readonly string LISP_READ = "read";

        public static readonly string LISP_DEFINE = "define";
        public static readonly string LISP_SETFIRST = "set!";

        public static readonly string LISP_TO_STRING = "toString";
        public static readonly string LISP_FROM_STRING = "fromString";

        public static readonly string LISP_SYMBOL_NAME = "symbolName";
        public static readonly string LISP_MAKE_SYMBOL = "symbolMake";

        public static readonly string LISP_GET_CONTEXT = "getContext";
        public static readonly string LISP_GET_CONTEXT_PARENT = "getContextParent";

        #endregion

        #region Base type predicates

        public static readonly string LISP_PRED_NULL = "null?";
        public static readonly string LISP_PRED_EMPTY = "empty?";
        public static readonly string LISP_PRED_SYM = "symbol?";
        public static readonly string LISP_PRED_PAIR = "pair?";
        public static readonly string LISP_PRED_LIST = "list?";
        public static readonly string LISP_PRED_STRING = "string?";
        public static readonly string LISP_PRED_BOOL = "bool?";
        public static readonly string LISP_PRED_NUMBER = "number?";
        public static readonly string LISP_PRED_PROCEDURE = "proc?";

        #endregion

        #region Base logic symbols

        public static readonly string LOGIC_EQ = "eq?";
        public static readonly string LOGIC_NEQ = "neq?";
        public static readonly string LOGIC_AND = "and";
        public static readonly string LOGIC_OR = "or";
        public static readonly string LOGIC_XOR = "xor";
        public static readonly string LOGIC_IMP = "imp";
        public static readonly string LOGIC_NOT = "not";

        #endregion

        #region Math symbols
        //  Base math

        public static readonly string MATH_SUM = "+";
        public static readonly string MATH_DIS = "-";
        public static readonly string MATH_MUL = "*";
        public static readonly string MATH_DIV = "/";
        public static readonly string MATH_MOD = "%";

        public static readonly string MATH_MIN = "min";
        public static readonly string MATH_MAX = "max";
        public static readonly string MATH_ABS = "abs";
        public static readonly string MATH_SIGN = "sign";
        public static readonly string MATH_CEIL = "ceil";
        public static readonly string MATH_FLOOR = "floor";

        public static readonly string MATH_TRUNC = "trunc";
        public static readonly string MATH_SQRT = "sqrt";
        public static readonly string MATH_POW = "pow";
        public static readonly string MATH_EXP = "exp";
        public static readonly string MATH_LOGN = "ln";
        public static readonly string MATH_LOG = "log";
        public static readonly string MATH_LOG10 = "log10";

        public static readonly string MATH_SIN = "sin";
        public static readonly string MATH_COS = "cos";
        public static readonly string MATH_TAN = "tan";
        public static readonly string MATH_ASIN = "asin";
        public static readonly string MATH_ACOS = "acos";
        public static readonly string MATH_ATAN = "atan";
        public static readonly string MATH_ATAN2 = "atan2";
        public static readonly string MATH_SINH = "sinh";
        public static readonly string MATH_COSH = "cosh";
        public static readonly string MATH_TANH = "tanh";

        public static readonly string MATH_PI = "#PI";
        public static readonly string MATH_E = "#E";

        public static readonly string MATH_AND = "&";
        public static readonly string MATH_OR = "|";
        public static readonly string MATH_XOR = "^";
        public static readonly string MATH_LSH = "<<";
        public static readonly string MATH_RSH = ">>";

        public static readonly string MATH_LT = "<";
        public static readonly string MATH_GT = ">";
        public static readonly string MATH_LE = "<=";
        public static readonly string MATH_GE = ">=";
        public static readonly string MATH_NE = "!=";
        public static readonly string MATH_EQ = "==";

        //  Type predicates

        public static readonly string MATH_PREDBYTE = "byte?";
        public static readonly string MATH_PREDUBYTE = "ubyte?";
        public static readonly string MATH_PREDSBYTE = "sbyte?";
        public static readonly string MATH_PREDCHAR = "char?";

        public static readonly string MATH_PREDSSHORT = "short?";
        public static readonly string MATH_PREDUSHORT = "ushort?";

        public static readonly string MATH_PREDSINT = "int?";
        public static readonly string MATH_PREDUINT = "uint?";

        public static readonly string MATH_PREDSLONG = "long?";
        public static readonly string MATH_PREDULONG = "ulong?";

        public static readonly string MATH_PREDFLOAT = "float?";
        public static readonly string MATH_PREDDOUBLE = "double?";

        //  Type cast

        public static readonly string MATH_CASTBYTE = "byte:";
        public static readonly string MATH_CASTUBYTE = "ubyte:";
        public static readonly string MATH_CASTSBYTE = "sbyte:";
        public static readonly string MATH_CASTCHAR = "char:";

        public static readonly string MATH_CASTSSHORT = "short:";
        public static readonly string MATH_CASTUSHORT = "ushort:";

        public static readonly string MATH_CASTSINT = "int:";
        public static readonly string MATH_CASTUINT = "uint:";

        public static readonly string MATH_CASTSLONG = "long:";
        public static readonly string MATH_CASTULONG = "ulong:";

        public static readonly string MATH_CASTFLOAT = "float:";
        public static readonly string MATH_CASTDOUBLE = "double:";

        #endregion

        #region Table|Context names

        public static readonly string LISP_TABLE_CREATE = "table";
        public static readonly string LISP_TABLE_GET = "tableGet";
        public static readonly string LISP_TABLE_SET = "tableSet";
        public static readonly string LISP_TEBLE_REMOVE = "tableRemove";
        public static readonly string LISP_TABLE_CLEAR = "tableClear";
        public static readonly string LISP_TABLE_IMPORT = "tableImport";
        public static readonly string LISP_TABLE_IMPORT_ALL = "tableImportAll";
        public static readonly string LISP_TABLE_EACH = "tableEach";

        public static readonly string LISP_TABLE_PRED = "table?";

        #endregion

        #region Text symbols

        public static readonly string TEXT_CREATE = "strCreate";
        public static readonly string TEXT_LENGTH = "strLength";
        public static readonly string TEXT_GETCHARS = "strChars";
        public static readonly string TEXT_GETCHAR = "strGet";

        public static readonly string TEXT_CONCAT = "strConcat";
        public static readonly string TEXT_SUBSTR = "strSubstr";
        public static readonly string TEXT_SPLIT = "strSplit";

        public static readonly string TEXT_STARTSWITH = "strStartsWith";
        public static readonly string TEXT_ENDSWITH = "strEndsWith";
        public static readonly string TEXT_CONTAINS = "strContains";

        public static readonly string TEXT_REPLACE = "strReplace";

        #endregion

        #region File System

        public static readonly string FS_LOAD = "load";
        public static readonly string FS_SAVE = "save";

        public static readonly string FS_EXISTS = "fsExists";

        public static readonly string FS_OPEN = "fsOpen";
        public static readonly string FS_FLUSH = "fsFlush";
        public static readonly string FS_CLOSE = "fsClose";

        public static readonly string FS_READ = "fsRead";
        public static readonly string FS_READ_LINE = "fsReadline";
        public static readonly string FS_WRITE = "fsWrite";

        public static readonly string FS_READ_TEXT = "fsReadText";
        public static readonly string FS_READ_LINES = "fsReadLines";
        public static readonly string FS_WRITE_TEXT = "fsWriteText";
        public static readonly string FS_WRITE_LINES = "fsWriteLines";
        public static readonly string FS_APPEND_TEXT = "fsAppendText";
        public static readonly string FS_APPEND_LINES = "fsAppendLines";
        
        public static readonly string FS_FILE_PRED = "fsIsFile?";
        public static readonly string FS_DIR_PRED = "fsIsDirectory?";
        public static readonly string FS_DIR_EMPTY_PRED = "fsDirectoryIsEmpty?";

        public static readonly string FS_READ_DIRECTORY = "fsReadDirectory";
        public static readonly string FS_CREATE_DIRECTORY = "fsCreateDirectory";
        public static readonly string FS_REMOVE_DIRECTORY = "fsRemoveDirectory";

        public static readonly string FS_PATH_COMBINE = "fsPathCombine";
        public static readonly string FS_PATH_GET_FULL = "fsPathGetFull";
        public static readonly string FS_PATH_GET_EXTENSION = "fsPathGetExtension";
        public static readonly string FS_PATH_GET_FILE_NAME = "fsPathGetFileName";
        public static readonly string FS_PATH_GET_DIR_NAME = "fsPathGetDirectoryName";

        #endregion

        #region Time System

        public static readonly string LISP_GET_TIME = "getTime";

        public static readonly string LISP_INTERVAL_PRED = "interval?";
        public static readonly string LISP_TIMEOUT_PRED = "timeout?";

        public static readonly string LISP_INTERVAL_SET = "intervalSet";
        public static readonly string LISP_INTERVAL_CLEAR = "intervalClear";
        public static readonly string LISP_INTERVAL_TAG = "intervalTag";

        public static readonly string LISP_TIMEOUT_SET = "timeoutSet";
        public static readonly string LISP_TIMEOUT_CLEAR = "timeoutClear";
        public static readonly string LISP_TIMEOUT_TAG = "timeoutTag";

        #endregion

        #region Concurrency

        public static readonly string CONCURENCY_QUEUE_CREATE = "concurentQueueCreate";
        public static readonly string CONCURENCY_QUEUE_ENQUEUE = "concurentQueueEnqueue";
        public static readonly string CONCURENCY_QUEUE_DEQUEUE = "concurentQueueDequeue";
        public static readonly string CONCURENCY_QUEUE_COUNT = "concurentQueueCount";
        public static readonly string CONCURENCY_QUEUE_PRED = "concurentQueue?";

        #endregion
    }
}
