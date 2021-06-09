namespace Bombardo.V2.Lang
{
	public class Module
	{
		public bool loading;
		public string currentPath;
		public Context ModuleContext { get; private set; }
		public Context ExportContext { get; private set; }

		public Atom Result => ModuleContext.Get(Names.MODULE, true);

		public Module(string fullPath, Context rootContext)
		{
			loading       = false;
			currentPath   = fullPath;
			ExportContext = new Context();
			
			ModuleContext = new Context(rootContext);
			ModuleContext.Define(Names.MODULE_PATH, new Atom(AtomType.String, currentPath));
			ModuleContext.Define(Names.MODULE, new Atom(AtomType.Native, ExportContext));
		}
	}
}