// ReSharper disable InconsistentNaming
// ReSharper disable ConvertToConstant.Global

namespace Bombardo.Core
{
	public static partial class Names
	{
		public static readonly string NULL_SYMBOL = "null";
		
		// module system:
		
		public static readonly string PREPROCESS = "preprocess:";
        
        public static readonly string MODULES_FOLDER = "modules";

        public static readonly string MODULE = "module";
        public static readonly string MODULE_PATH = "#path";

        public static readonly string MODULE_REQUIRE = "require";
        public const           string MODULE_REQUIRE_AS = "as";
        public const           string MODULE_REQUIRE_IMPORT = "import";
        public const           string MODULE_REQUIRE_IMPORT_ALL = "importAll";

        public static readonly string MODULE_EXPORT = "export";
	}
}