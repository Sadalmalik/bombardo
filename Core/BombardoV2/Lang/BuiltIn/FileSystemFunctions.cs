using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bombardo.V2
{
	public static partial class Names
	{
		// Generalized stuff
		public static readonly string FS_LOAD = "load"; // fx.load
		public static readonly string FS_SAVE = "save"; // fs.save
		public static readonly string FS_FIND = "find"; // fs.find
		public static readonly string FS_LOOKUP = "lookup"; // fs.lookup

		// Path operations

		public static readonly string FS_PATH_SUBMODULE     = "path";             // submodule
		public static readonly string FS_PATH_COMBINE       = "combine";          // fs.path.combine
		public static readonly string FS_PATH_GET_FULL      = "getFull";          // fs.path.getFull
		public static readonly string FS_PATH_GET_EXTENSION = "getExtension";     // fs.path.getExtension
		public static readonly string FS_PATH_GET_FILENAME  = "getFileName";      // fs.path.getFileName
		public static readonly string FS_PATH_GET_DIRNAME   = "getDirectoryName"; // fs.path.getDirectoryName

		// Directory operations

		public static readonly string FS_DIRECTORY_SUBMODULE = "directory"; // submodule
		public static readonly string FS_DIRECTORY_READ      = "read";      // fs.directory.read
		public static readonly string FS_DIRECTORY_CREATE    = "create";    // fs.directory.create
		public static readonly string FS_DIRECTORY_MOVE      = "move";      // fs.directory.move
		public static readonly string FS_DIRECTORY_DELETE    = "delete";    // fs.directory.delete

		// File operations

		public static readonly string FS_FILE_SUBMODULE = "file";   // submodule
		public static readonly string FS_FILE_OPEN      = "open";   // fs.file.open
		public static readonly string FS_FILE_FLUSH     = "flush";  // fs.file.flush
		public static readonly string FS_FILE_CLOSE     = "close";  // fs.file.close
		public static readonly string FS_FILE_MOVE      = "move";   // fs.file.move
		public static readonly string FS_FILE_DELETE    = "delete"; // fs.file.delete

		public static readonly string FS_FILE_READ         = "read";        // fs.file.read
		public static readonly string FS_FILE_WRITE        = "write";       // fs.file.write
		public static readonly string FS_FILE_READ_LINE    = "readLine";    // fs.file.readLine
		public static readonly string FS_FILE_WRITE_LINE   = "writeLine";   // fs.file.writeLine
		public static readonly string FS_FILE_READ_TEXT    = "readText";    // fs.file.readText
		public static readonly string FS_FILE_WRITE_TEXT   = "writeText";   // fs.file.writeText
		public static readonly string FS_FILE_READ_LINES   = "readLines";   // fs.file.readLines
		public static readonly string FS_FILE_WRITE_LINES  = "writeLines";  // fs.file.writeLines
		public static readonly string FS_FILE_APPEND_TEXT  = "appendText";  // fs.file.appendText
		public static readonly string FS_FILE_APPEND_LINES = "appendLines"; // fs.file.appendLines

		// Predicates

		public static readonly string FS_PRED_EXISTS = "exist?";            // fs.exist?
		public static readonly string FS_PRED_FILE   = "isFile?";           // fs.isFile?
		public static readonly string FS_PRED_DIR    = "isDirectory?";      // fs.isDirectory?
		public static readonly string FS_PRED_EMPTY  = "isDirectoryEmpty?"; // fs.isDirectoryEmpty?
	}

	public class FileSystemFunctions
	{
		public static void Define(Context ctx)
		{
			//  (load "filepath") -> list
			//  (save "filepath" (symbol1 symbol2 symbol3 (expression1) (expression2) (expression3)))

			ctx.DefineFunction(Names.FS_LOAD, Load);
			ctx.DefineFunction(Names.FS_SAVE, Save);
			ctx.DefineFunction(Names.FS_FIND, Find);
			ctx.DefineFunction(Names.FS_LOOKUP, LookUp);

			// Path operations

			//  (path.combine "path1" "path2" "path3") -> "path1/path2/path3"
			//  (path.getFull "directoryPath.ext") -> "extended/directoryPath.ext"
			//  (path.getExtension "directoryPath/file.ext") -> "ext"
			//  (path.getFileName "directoryPath/file.ext") -> "file.ext"
			//  (path.getDirectoryName "directoryPath/file.ext") -> "directoryPath"

			var path = new Context();
			ctx.Define(Names.FS_PATH_SUBMODULE, path.self);
			path.DefineFunction(Names.FS_PATH_COMBINE, PathCombile);
			path.DefineFunction(Names.FS_PATH_GET_FULL, PathGetFull);
			path.DefineFunction(Names.FS_PATH_GET_EXTENSION, PathGetExtension);
			path.DefineFunction(Names.FS_PATH_GET_FILENAME, PathGetFileName);
			path.DefineFunction(Names.FS_PATH_GET_DIRNAME, PathGetDirectoryName);

			// Directory operations

			//  (directory.read "directoryPath") -> ( "file1" "file2" "file3" ... )
			//  (directory.create "directoryPath") -> null
			//  (directory.remove "directoryPath") -> null

			var dir = new Context();
			ctx.Define(Names.FS_DIRECTORY_SUBMODULE, dir.self);
			dir.DefineFunction(Names.FS_DIRECTORY_READ, ReadDirectory);
			dir.DefineFunction(Names.FS_DIRECTORY_CREATE, CreateDirectory);
			dir.DefineFunction(Names.FS_DIRECTORY_DELETE, RemoveDirectory);

			// File operations

			//  (fsOpen "filepath" [read] [write] [|create|append]) -> handler
			//  (fsFlush handler)
			//  (fsClose handler)

			//  (fsRead handler) -> char
			//  (fsReadline handler) -> string
			//  (fsWrite handler char|string|symbol|number)

			//  (fsReadText "filepath") -> string
			//  (fsReadLines "filepath") -> ( string string string ... )
			//  (fsWriteText "filepath" string)
			//  (fsWriteLines "filepath" ( string string string ... ))
			//  (fsAppendText "filepath" string)
			//  (fsAppendLines "filepath" ( string string string ... ))

			var file = new Context();
			ctx.Define(Names.FS_FILE_SUBMODULE, dir.self);

			file.DefineFunction(Names.FS_FILE_OPEN, Open);
			file.DefineFunction(Names.FS_FILE_FLUSH, Flush);
			file.DefineFunction(Names.FS_FILE_CLOSE, Close);

			file.DefineFunction(Names.FS_FILE_READ, Read);
			file.DefineFunction(Names.FS_FILE_READ_LINE, ReadLine);
			file.DefineFunction(Names.FS_FILE_WRITE, Write);

			file.DefineFunction(Names.FS_FILE_READ_TEXT, ReadText);
			file.DefineFunction(Names.FS_FILE_READ_LINES, ReadLines);
			file.DefineFunction(Names.FS_FILE_WRITE_TEXT, WriteText);
			file.DefineFunction(Names.FS_FILE_WRITE_LINES, WriteLines);
			file.DefineFunction(Names.FS_FILE_APPEND_TEXT, AppendText);
			file.DefineFunction(Names.FS_FILE_APPEND_LINES, AppendLines);

			// Predicates

			//  (exist? "filepath") -> true|false
			//  (isFile? "path") -> true | false
			//  (isDirectory? "path") -> true | false
			//  (isDirectoryEmpty? "path") -> true | false

			ctx.DefineFunction(Names.FS_PRED_EXISTS, Exists);
			ctx.DefineFunction(Names.FS_PRED_FILE, FilePredicate);
			ctx.DefineFunction(Names.FS_PRED_DIR, DirectoryPredicate);
			ctx.DefineFunction(Names.FS_PRED_EMPTY, DirectoryEmptyPredicate);
		}


#region Legacy?

		private static void Load(Evaluator eval, StackFrame frame)
		{
			var args = frame.args;

			var path = (Atom) args?.value;

			if (path == null || !path.IsString)
				throw new ArgumentException("Path must be string!");

			var file = (string) path.value;
			if (File.Exists(file))
			{
				var raw   = File.ReadAllText(file);
				var nodes = BombardoLang.Parse(raw);

				eval.Return(nodes);
				return;
			}

			eval.Return(null);
		}

		private static void Save(Evaluator eval, StackFrame frame)
		{
			var args = frame.args;

			var path = (Atom) args?.value;
			var list = (Atom) args?.next?.value;

			if (path == null || !path.IsString)
				throw new ArgumentException("Path must be string!");
			if (list == null || !list.IsPair)
				throw new ArgumentException("second argument must be list!");

			var stream = File.Open((string) path.value, FileMode.Create);
			var output = new StreamWriter(stream);

			while (list != null)
			{
				var item = (Atom) list?.value;
				output.WriteLine(item != null ? item.Stringify() : "");
				list = list.next;
			}

			output.Flush();
			output.Dispose();
			stream.Dispose();

			eval.Return(null);
		}
		
		private static void Find(Evaluator eval, StackFrame frame)
		{
			var args = frame.args;

			var path = (Atom) args?.value;

			if (path == null || !path.IsString)
				throw new ArgumentException("Path must be string!");

			var file = FSUtils.FindFile(path.value as string);
			if (!string.IsNullOrEmpty(file))
			{
				eval.Return(Atom.FromString(file));
				return;
			}
			
			eval.Return(null);
		}
		
		private static void LookUp(Evaluator eval, StackFrame frame)
		{
			(Atom programPath, Atom currentPath, Atom modulesFolder, Atom module) = StructureUtils.Split4(frame.args);

			var file = FSUtils.LookupModuleFile(
					programPath?.value as string,   
					currentPath?.value as string,   
					modulesFolder?.value as string, 
					module?.value as string
				);
			if (!string.IsNullOrEmpty(file))
			{
				eval.Return(Atom.FromString(file));
				return;
			}
			
			eval.Return(null);
		}

#endregion


#region Path operations

		private static void PathCombile(Evaluator eval, StackFrame frame)
		{
			var args = frame.args;

			var i     = 0;
			var parts = new string[args.ListLength()];

			for (var iter = args; iter != null; iter = iter.next)
			{
				if (!iter.atom.IsString)
					throw new ArgumentException("All arguments must be strings!");
				parts[i] = (string) iter.atom?.value;
				i++;
			}

			eval.Return(new Atom(AtomType.String, Path.Combine(parts)));
		}

		private static void PathGetFull(Evaluator eval, StackFrame frame)
		{
			var args = frame.args;

			var (atomPath, atomBase) = StructureUtils.Split2(args);

			if (!atomPath.IsString)
				throw new ArgumentException("Path must be string!");

			string path = atomPath.value as string;
			string root = atomBase?.value as string;
			
			if (string.IsNullOrEmpty(path))
				throw new ArgumentException("Path unexpectedly is null!");
			
			if (!string.IsNullOrEmpty(root))
				if (!Path.IsPathRooted(path))
					path = Path.Combine(root, path);
			
			path = Path.GetFullPath(path);
			
			eval.Return(new Atom(AtomType.String, path));
		}

		private static void PathGetExtension(Evaluator eval, StackFrame frame)
		{
			var args = frame.args;

			var path = args?.atom;

			if (!path.IsString)
				throw new ArgumentException("Path must be string!");

			eval.Return(new Atom(AtomType.String, Path.GetExtension((string) path.value)));
		}

		private static void PathGetFileName(Evaluator eval, StackFrame frame)
		{
			var args = frame.args;

			var path = args?.atom;

			if (!path.IsString)
				throw new ArgumentException("Path must be string!");

			eval.Return(new Atom(AtomType.String, Path.GetFileNameWithoutExtension((string) path.value)));
		}

		private static void PathGetDirectoryName(Evaluator eval, StackFrame frame)
		{
			var args = frame.args;

			var path = args?.atom;

			if (!path.IsString)
				throw new ArgumentException("Path must be string!");

			eval.Return(new Atom(AtomType.String, Path.GetDirectoryName((string) path.value)));
		}

#endregion


#region Directory operations

		private static void ReadDirectory(Evaluator eval, StackFrame frame)
		{
			var args = frame.args;

			var path = args?.atom;
			if (path == null || !path.IsString)
				throw new ArgumentException("Path must be string!");
			var directory = (string) path.value;
			var pattern   = args?.next?.atom;
			var mode      = args?.next?.next?.atom;
			var option    = ArgUtils.GetEnum<SearchOption>(mode, 3);

			string[] dirs = null;
			if (pattern == null) dirs = Directory.GetDirectories(directory);
			else dirs                 = Directory.GetDirectories(directory, (string) pattern.value, option);

			string[] files = null;
			if (pattern == null) files = Directory.GetFiles(directory);
			else files                 = Directory.GetFiles(directory, (string) pattern.value, option);

			var elements                                                     = new Atom[dirs.Length + files.Length];
			for (var i = 0; i < dirs.Length; i++) elements[i]                = new Atom(AtomType.String, dirs[i]);
			for (var i = 0; i < files.Length; i++) elements[i + dirs.Length] = new Atom(AtomType.String, files[i]);

			eval.Return(StructureUtils.List(elements));
		}

		private static void CreateDirectory(Evaluator eval, StackFrame frame)
		{
			var args = frame.args;

			var path = (Atom) args?.value;
			if (path == null || !path.IsString)
				throw new ArgumentException("Path must be string!");
			var directoryPath = (string) path.value;

			Directory.CreateDirectory(directoryPath);

			eval.Return(null);
		}

		private static void RemoveDirectory(Evaluator eval, StackFrame frame)
		{
			var args = frame.args;

			var path = (Atom) args?.value;
			if (path == null || !path.IsString)
				throw new ArgumentException("Path must be string!");
			var directoryPath = (string) path.value;

			var dirs  = Directory.GetDirectories(directoryPath);
			var files = Directory.GetFiles(directoryPath);
			var empty = dirs.Length == 0 && files.Length == 0;

			if (empty) Directory.Delete(directoryPath);

			eval.Return(empty ? Atoms.TRUE : Atoms.FALSE);
		}

#endregion


#region File operations

		private static void Open(Evaluator eval, StackFrame frame)
		{
			var (path, accessType, modeType) = StructureUtils.Split3(frame.args);

			if (path == null || !path.IsString)
				throw new ArgumentException("Path must be string!");
			var file = (string) path.value;

			var access = ArgUtils.GetEnum(accessType, 1, FileAccess.Read);
			var mode   = ArgUtils.GetEnum(modeType, 2, FileMode.Open);

			if (access == FileAccess.Read)
			{
				var reader = new StreamReader(File.Open(file, mode, access));

				eval.Return(new Atom(AtomType.Native, reader));
				return;
			}

			if (access == FileAccess.Write)
			{
				var writer = new StreamWriter(File.Open(file, mode, access));

				eval.Return(new Atom(AtomType.Native, writer));
				return;
			}

			eval.Return(null);
		}

		private static void Flush(Evaluator eval, StackFrame frame)
		{
			var stream = frame.args?.atom;

			if (stream.type != AtomType.Native)
				throw new ArgumentException("Argument must be stream!");

			var writer = stream.value as StreamWriter;
			writer?.Flush();

			eval.Return(null);
		}

		private static void Close(Evaluator eval, StackFrame frame)
		{
			var stream = frame.args?.atom;

			if (stream.type != AtomType.Native)
				throw new ArgumentException("Argument must be stream!");

			var reader = stream.value as StreamReader;
			reader?.Close();

			var writer = stream.value as StreamWriter;
			writer?.Close();

			eval.Return(null);
		}

		private static void Read(Evaluator eval, StackFrame frame)
		{
			var stream = frame.args?.atom;

			if (stream.type != AtomType.Native)
				throw new ArgumentException("Argument must be stream!");

			var reader = stream.value as StreamReader;
			if (reader == null)
				throw new ArgumentException("Argument must be stream!");

			eval.Return(new Atom(AtomType.Number, reader.Read()));
		}

		private static void ReadLine(Evaluator eval, StackFrame frame)
		{
			var stream = frame.args?.atom;

			if (stream.type != AtomType.Native)
				throw new ArgumentException("Argument must be stream!");

			var reader = stream.value as StreamReader;
			if (reader == null)
				throw new ArgumentException("Argument must be stream!");

			eval.Return(new Atom(AtomType.String, reader.ReadLine()));
		}

		private static void Write(Evaluator eval, StackFrame frame)
		{
			var (stream, value) = StructureUtils.Split2(frame.args);

			if (stream.type != AtomType.Native)
				throw new ArgumentException("Argument must be stream!");

			var writer = stream?.value as StreamWriter;
			if (writer == null) throw new ArgumentException("Argument must be stream!");

			if (value == null) throw new ArgumentException("Second argument can't be null!");

			switch (value.type)
			{
				case AtomType.Number:
					var type = UNumber.NumberType(value?.value);
					switch (type)
					{
						case UNumber.UINT_8:
							writer.Write(Convert.ToByte(value.value));
							break;
						case UNumber.SINT_8:
							writer.Write(Convert.ToSByte(value.value));
							break;
						case UNumber.UINT16:
							writer.Write(Convert.ToUInt16(value.value));
							break;
						case UNumber.SINT16:
							writer.Write(Convert.ToInt16(value.value));
							break;
						case UNumber._CHAR_:
							writer.Write(Convert.ToChar(value.value));
							break;
						case UNumber.UINT32:
							writer.Write(Convert.ToUInt32(value.value));
							break;
						case UNumber.SINT32:
							writer.Write(Convert.ToInt32(value.value));
							break;
						case UNumber.UINT64:
							writer.Write(Convert.ToUInt64(value.value));
							break;
						case UNumber.SINT64:
							writer.Write(Convert.ToInt64(value.value));
							break;
						case UNumber.FLO32:
							writer.Write(Convert.ToSingle(value.value));
							break;
						case UNumber.FLO64:
							writer.Write(Convert.ToDouble(value.value));
							break;
						default:
							writer.Write(value.value);
							break;
					}

					break;
				default:
					writer.Write(value.value);
					break;
			}

			eval.Return(null);
		}

		private static void ReadText(Evaluator eval, StackFrame frame)
		{
			var path = frame.args?.atom;

			if (path == null || !path.IsString)
				throw new ArgumentException("Path must be string!");
			var file = (string) path.value;

			var text = File.ReadAllText(file, Encoding.UTF8);

			eval.Return(new Atom(AtomType.String, text));
		}

		private static void ReadLines(Evaluator eval, StackFrame frame)
		{
			var path = frame.args?.atom;

			if (path == null || !path.IsString)
				throw new ArgumentException("Path must be string!");
			var file = (string) path.value;

			var lines = File.ReadAllLines(file, Encoding.UTF8);

			var atoms = new Atom[lines.Length];
			for (var i = 0; i < lines.Length; i++)
				atoms[i] = new Atom(AtomType.String, lines[i]);

			eval.Return(StructureUtils.List(atoms));
		}

		private static void WriteText(Evaluator eval, StackFrame frame)
		{
			var (path, text) = StructureUtils.Split2(frame.args);

			if (path == null || !path.IsString)
				throw new ArgumentException("First argument must be string!");
			var file = (string) path.value;

			if (text == null || !text.IsString)
				throw new ArgumentException("Second argument must be string!");
			var data = (string) text.value;

			File.WriteAllText(file, data, Encoding.UTF8);

			eval.Return(null);
		}

		private static void WriteLines(Evaluator eval, StackFrame frame)
		{
			var (path, list) = StructureUtils.Split2(frame.args);

			if (path == null || !path.IsString)
				throw new ArgumentException("First argument must be string!");
			var file = (string) path.value;

			if (list == null || !list.IsPair)
				throw new ArgumentException("Second argument must be list of strings!");

			var lines = new List<string>();
			for (var iter = list; iter != null; iter = iter.next)
			{
				var line = iter.atom;
				if (line == null || !line.IsString)
					throw new ArgumentException("Second argument must be list of strings!");
				lines.Add((string) line.value);
			}

			File.WriteAllLines(file, lines, Encoding.UTF8);

			eval.Return(null);
		}

		private static void AppendText(Evaluator eval, StackFrame frame)
		{
			var (path, text) = StructureUtils.Split2(frame.args);

			if (path == null || !path.IsString)
				throw new ArgumentException("First argument must be string!");
			var file = (string) path.value;

			if (text == null || !text.IsString)
				throw new ArgumentException("Second argument must be string!");
			var data = (string) text.value;

			File.AppendAllText(file, data, Encoding.UTF8);

			eval.Return(null);
		}

		private static void AppendLines(Evaluator eval, StackFrame frame)
		{
			var (path, list) = StructureUtils.Split2(frame.args);

			if (path == null || !path.IsString)
				throw new ArgumentException("First argument must be string!");
			var file = (string) path.value;

			if (list == null || !list.IsPair)
				throw new ArgumentException("Second argument must be list of strings!");

			var lines = new List<string>();
			for (var iter = list; iter != null; iter = iter.next)
			{
				var line = iter.atom;
				if (line == null || !line.IsString)
					throw new ArgumentException("Second argument must be list of strings!");
				lines.Add((string) line.value);
			}

			File.AppendAllLines(file, lines, Encoding.UTF8);

			eval.Return(null);
		}

#endregion


#region Predicates

		private static void Exists(Evaluator eval, StackFrame frame)
		{
			var path = (Atom) frame.args?.value;

			if (path == null || !path.IsString)
				throw new ArgumentException("Path must be string!");
			var file = (string) path.value;

			eval.Return(File.Exists(file) ? Atoms.TRUE : Atoms.FALSE);
		}


		private static void FilePredicate(Evaluator eval, StackFrame frame)
		{
			var path = (Atom) frame.args?.value;

			if (path == null || !path.IsString)
				throw new ArgumentException("Path must be string!");
			var file = (string) path.value;

			if (!File.Exists(file))
			{
				eval.Return(Atoms.FALSE);
				return;
			}

			var attr = File.GetAttributes(file);
			eval.Return(attr.HasFlag(FileAttributes.Directory) ? Atoms.FALSE : Atoms.TRUE);
		}

		private static void DirectoryPredicate(Evaluator eval, StackFrame frame)
		{
			var path = (Atom) frame.args?.value;

			if (path == null || !path.IsString)
				throw new ArgumentException("Path must be string!");
			var file = (string) path.value;

			if (!File.Exists(file))
			{
				eval.Return(Atoms.FALSE);
				return;
			}

			var attr = File.GetAttributes(file);
			eval.Return(attr.HasFlag(FileAttributes.Directory) ? Atoms.TRUE : Atoms.FALSE);
		}

		private static void DirectoryEmptyPredicate(Evaluator eval, StackFrame frame)
		{
			var path = (Atom) frame.args?.value;

			if (path == null || !path.IsString)
				throw new ArgumentException("Path must be string!");
			var directoryPath = (string) path.value;

			var dirs  = Directory.GetDirectories(directoryPath);
			var files = Directory.GetFiles(directoryPath);
			var empty = dirs.Length == 0 && files.Length == 0;

			eval.Return(empty ? Atoms.TRUE : Atoms.FALSE);
		}

#endregion
	}
}