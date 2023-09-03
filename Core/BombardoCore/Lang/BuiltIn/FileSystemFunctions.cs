using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bombardo.Core
{
	public static partial class Names
	{
		// Generalized stuff
		public static readonly string FS_LOAD   = "load";   // fx.load
		public static readonly string FS_SAVE   = "save";   // fs.save
		public static readonly string FS_FIND   = "find";   // fs.find
		public static readonly string FS_LOOKUP = "lookup"; // fs.lookup

		// Path operations

		public static readonly string FS_PATH_SUBMODULE     = "path";             // submodule
		public static readonly string FS_PATH_COMBINE       = "combine";          // fs.path.combine
		public static readonly string FS_PATH_GET_FULL      = "getFull";          // fs.path.getFull
		public static readonly string FS_PATH_GET_RELATIVE  = "getRelative";      // fs.path.getFull
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
			// Main Operations
			//  (fs.load "filepath") -> list
			//  (fs.save "filepath" (symbol1 symbol2 symbol3 (expression1) (expression2) (expression3)))
			//  (fs.find "filepath") -> founded path
			//  (fs.lookup "filepath") -> founded path

			ctx.DefineFunction(Names.FS_LOAD, Load);
			ctx.DefineFunction(Names.FS_SAVE, Save);
			ctx.DefineFunction(Names.FS_FIND, Find);
			ctx.DefineFunction(Names.FS_LOOKUP, LookUp);

			// Path operations
			//  (fs.path.combine          "path1" "path2" "path3") -> "path1/path2/path3"
			//  (fs.path.getFull          "directoryPath.ext") -> "extended/directoryPath.ext"
			//  (fs.path.getExtension     "directoryPath/file.ext") -> "ext"
			//  (fs.path.getFileName      "directoryPath/file.ext") -> "file.ext"
			//  (fs.path.getDirectoryName "directoryPath/file.ext") -> "directoryPath"

			var path = new Context();
			ctx.Define(Names.FS_PATH_SUBMODULE, path.self);
			path.DefineFunction(Names.FS_PATH_COMBINE, PathCombine);
			path.DefineFunction(Names.FS_PATH_GET_FULL, PathGetFull);
			path.DefineFunction(Names.FS_PATH_GET_RELATIVE, PathGetRelative);
			path.DefineFunction(Names.FS_PATH_GET_EXTENSION, PathGetExtension);
			path.DefineFunction(Names.FS_PATH_GET_FILENAME, PathGetFileName);
			path.DefineFunction(Names.FS_PATH_GET_DIRNAME, PathGetDirectoryName);

			// Directory operations
			//  (fs.directory.create "directoryPath") -> null
			//  (fs.directory.read "directoryPath") -> ( "file1" "file2" "file3" ... )
			//  (fs.directory.move "sourcePath" "destinationPath") -> null
			//  (fs.directory.delete "directoryPath") -> True|False

			var dir = new Context();
			ctx.Define(Names.FS_DIRECTORY_SUBMODULE, dir.self);
			dir.DefineFunction(Names.FS_DIRECTORY_READ, DirectoryRead);
			dir.DefineFunction(Names.FS_DIRECTORY_CREATE, DirectoryCreate);
			dir.DefineFunction(Names.FS_DIRECTORY_MOVE, DirectoryMove);
			dir.DefineFunction(Names.FS_DIRECTORY_DELETE, DirectoryDelete);

			// File operations

			//  (fs.file.open "filePath" [read] [write] [|create|append]) -> handler
			//  (fs.file.flush handler)
			//  (fs.file.close handler)
			//  (fs.file.move "sourcePath" "destinationPath")
			//  (fs.file.delete "filePath")

			//  (fs.file.read handler) -> char
			//  (fs.file.readLine handler) -> string
			//  (fs.file.write handler char|string|symbol|number)
			//  (fs.file.writeLine handler char|string|symbol|number)

			//  (fs.file.readText "filepath") -> string
			//  (fs.file.writeText "filepath") -> ( string string string ... )
			//  (fs.file.readLines "filepath" string)
			//  (fs.file.writeLines "filepath" ( string string string ... ))
			//  (fs.file.appendText "filepath" string)
			//  (fs.file.appendLines "filepath" ( string string string ... ))

			var file = new Context();
			ctx.Define(Names.FS_FILE_SUBMODULE, file.self);

			file.DefineFunction(Names.FS_FILE_OPEN, FileOpen);
			file.DefineFunction(Names.FS_FILE_FLUSH, FileFlush);
			file.DefineFunction(Names.FS_FILE_CLOSE, FileClose);
			file.DefineFunction(Names.FS_FILE_MOVE, FileMove);
			file.DefineFunction(Names.FS_FILE_DELETE, FileDelete);

			file.DefineFunction(Names.FS_FILE_READ, FileRead);
			file.DefineFunction(Names.FS_FILE_READ_LINE, FileReadLine);
			file.DefineFunction(Names.FS_FILE_WRITE, FileWrite);
			file.DefineFunction(Names.FS_FILE_WRITE_LINE, FileWriteLine);

			file.DefineFunction(Names.FS_FILE_READ_TEXT, FileReadText);
			file.DefineFunction(Names.FS_FILE_READ_LINES, FileReadLines);
			file.DefineFunction(Names.FS_FILE_WRITE_TEXT, FileWriteText);
			file.DefineFunction(Names.FS_FILE_WRITE_LINES, FileWriteLines);
			file.DefineFunction(Names.FS_FILE_APPEND_TEXT, FileAppendText);
			file.DefineFunction(Names.FS_FILE_APPEND_LINES, FileAppendLines);

			// Predicates

			//  (fs.exist? "filepath") -> true|false
			//  (fs.isFile? "path") -> true | false
			//  (fs.isDirectory? "path") -> true | false
			//  (fs.isDirectoryEmpty? "path") -> true | false

			ctx.DefineFunction(Names.FS_PRED_EXISTS, Exists);
			ctx.DefineFunction(Names.FS_PRED_FILE, FilePredicate);
			ctx.DefineFunction(Names.FS_PRED_DIR, DirectoryPredicate);
			ctx.DefineFunction(Names.FS_PRED_EMPTY, DirectoryEmptyPredicate);
		}


#region Special

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
				output.Write(item.Stringify());
				output.Write(item.IsPair ? "\n" : " ");
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
			(Atom programPath, Atom currentPath, Atom modulesFolder, Atom module, Atom moduleRoot) =
				StructureUtils.Split5(frame.args);

			var file = FSUtils.LookupModuleFile(
				programPath?.value as string,
				currentPath?.value as string,
				modulesFolder?.value as string,
				module?.value as string,
				moduleRoot?.value as string
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

		private static void PathCombine(Evaluator eval, StackFrame frame)
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
			var (atomPath, atomBase) = StructureUtils.Split2(frame.args);

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

		private static void PathGetRelative(Evaluator eval, StackFrame frame)
		{
			var (basePath, relatedPath) = StructureUtils.Split2(frame.args);
			
			if (!basePath.IsString)
				throw new ArgumentException("Base Path must be string!");
			if (!relatedPath.IsString)
				throw new ArgumentException("Related Path must be string!");
			
			Uri bPath = new Uri((string) basePath.value);
			Uri rPath = new Uri((string) relatedPath.value);
			var newPath =bPath.MakeRelativeUri(rPath).ToString();
			
			eval.Return(new Atom(AtomType.String, newPath));
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

		private static void DirectoryRead(Evaluator eval, StackFrame frame)
		{
			var (path, pattern, mode) = StructureUtils.Split3(frame.args);

			if (path == null || !path.IsString)
				throw new ArgumentException("Path must be string!");
			var directory = (string) path.value;
			
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

		private static void DirectoryCreate(Evaluator eval, StackFrame frame)
		{
			var args = frame.args;

			var path = (Atom) args?.value;
			if (path == null || !path.IsString)
				throw new ArgumentException("Path must be string!");
			var directoryPath = (string) path.value;

			Directory.CreateDirectory(directoryPath);

			eval.Return(null);
		}

		private static void DirectoryMove(Evaluator eval, StackFrame frame)
		{
			var (srcPath, dstPath) = StructureUtils.Split2(frame.args);

			if (srcPath == null || !srcPath.IsString)
				throw new ArgumentException("Source Path must be string!");
			if (dstPath == null || !dstPath.IsString)
				throw new ArgumentException("Destination Path must be string!");

			var src = (string) srcPath.value;
			var dst = (string) dstPath.value;
			Directory.Move(src, dst);

			eval.Return(null);
		}

		private static void DirectoryDelete(Evaluator eval, StackFrame frame)
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

		private static void FileOpen(Evaluator eval, StackFrame frame)
		{
			var (path, accessType, modeType) = StructureUtils.Split3(frame.args);

			if (path == null || !path.IsString)
				throw new ArgumentException("Path must be string!");
			var file = (string) path.value;

			var access = ArgUtils.GetEnum(accessType, 1, FileAccess.Read);
			var mode   = ArgUtils.GetEnum(modeType, 2, FileMode.Open);

			Console.WriteLine($"fs.file.open args:\n\taccess = {access}\n\tmode = {mode}");

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

		private static void FileFlush(Evaluator eval, StackFrame frame)
		{
			var stream = frame.args?.atom;

			if (stream.type != AtomType.Native)
				throw new ArgumentException("Argument must be stream!");

			var writer = stream.value as StreamWriter;
			writer?.Flush();

			eval.Return(null);
		}

		private static void FileClose(Evaluator eval, StackFrame frame)
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

		private static void FileMove(Evaluator eval, StackFrame frame)
		{
			var (srcPath, dstPath) = StructureUtils.Split2(frame.args);

			if (srcPath == null || !srcPath.IsString)
				throw new ArgumentException("Source Path must be string!");
			if (dstPath == null || !dstPath.IsString)
				throw new ArgumentException("Destination Path must be string!");

			var src = (string) srcPath.value;
			var dst = (string) dstPath.value;
			File.Move(src, dst);

			eval.Return(null);
		}

		private static void FileDelete(Evaluator eval, StackFrame frame)
		{
			var path = frame.args.atom;
			
			if (path == null || !path.IsString)
				throw new ArgumentException("Path must be string!");
			
			File.Delete((string) path.value);
			
			eval.Return(null);
		}

		private static void FileRead(Evaluator eval, StackFrame frame)
		{
			var stream = frame.args?.atom;

			if (stream.type != AtomType.Native)
				throw new ArgumentException("Argument must be stream!");

			var reader = stream.value as StreamReader;
			if (reader == null)
				throw new ArgumentException("Argument must be stream!");

			eval.Return(new Atom(AtomType.Number, reader.Read()));
		}

		private static void FileReadLine(Evaluator eval, StackFrame frame)
		{
			var stream = frame.args?.atom;

			if (stream.type != AtomType.Native)
				throw new ArgumentException("Argument must be stream!");

			var reader = stream.value as StreamReader;
			if (reader == null)
				throw new ArgumentException("Argument must be stream!");

			eval.Return(new Atom(AtomType.String, reader.ReadLine()));
		}
		
		private static void FileWrite(Evaluator eval, StackFrame frame)
		{
			var (stream, value) = StructureUtils.Split2(frame.args);

			if (stream.type != AtomType.Native)
				throw new ArgumentException("Argument must be stream!");

			var writer = stream?.value as StreamWriter;
			if (writer == null) throw new ArgumentException("Argument must be stream!");

			if (value == null) throw new ArgumentException("Second argument can't be null!");

			Console.WriteLine($"fs.file.write args:\n\tvalue = {value}");
			WriteAtomInternal(writer, value);

			eval.Return(null);
		}
		
		private static void WriteAtomInternal(StreamWriter writer, Atom value)
		{
			switch (value.type)
			{
				case AtomType.Number:
					var type = AtomNumberOperations.NumberType(value?.value);
					switch (type)
					{
						case AtomNumberOperations.UINT_8:
							writer.Write(Convert.ToByte(value.value));
							break;
						case AtomNumberOperations.SINT_8:
							writer.Write(Convert.ToSByte(value.value));
							break;
						case AtomNumberOperations.UINT16:
							writer.Write(Convert.ToUInt16(value.value));
							break;
						case AtomNumberOperations.SINT16:
							writer.Write(Convert.ToInt16(value.value));
							break;
						case AtomNumberOperations._CHAR_:
							writer.Write(Convert.ToChar(value.value));
							break;
						case AtomNumberOperations.UINT32:
							writer.Write(Convert.ToUInt32(value.value));
							break;
						case AtomNumberOperations.SINT32:
							writer.Write(Convert.ToInt32(value.value));
							break;
						case AtomNumberOperations.UINT64:
							writer.Write(Convert.ToUInt64(value.value));
							break;
						case AtomNumberOperations.SINT64:
							writer.Write(Convert.ToInt64(value.value));
							break;
						case AtomNumberOperations.FLO32:
							writer.Write(Convert.ToSingle(value.value));
							break;
						case AtomNumberOperations.FLO64:
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
		}

		private static void FileWriteLine(Evaluator eval, StackFrame frame)
		{
			var (stream, value) = StructureUtils.Split2(frame.args);

			if (stream.type != AtomType.Native)
				throw new ArgumentException("Argument must be stream!");

			var writer = stream?.value as StreamWriter;
			if (writer == null) throw new ArgumentException("Argument must be stream!");

			if (value == null) throw new ArgumentException("Second argument can't be null!");
			
			writer.WriteLine(value.Stringify());
			
			eval.Return(null);
		}

		private static void FileReadText(Evaluator eval, StackFrame frame)
		{
			var path = frame.args?.atom;

			if (path == null || !path.IsString)
				throw new ArgumentException("Path must be string!");
			var file = (string) path.value;

			var text = File.ReadAllText(file, Encoding.UTF8);

			eval.Return(new Atom(AtomType.String, text));
		}

		private static void FileReadLines(Evaluator eval, StackFrame frame)
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

		private static void FileWriteText(Evaluator eval, StackFrame frame)
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

		private static void FileWriteLines(Evaluator eval, StackFrame frame)
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

		private static void FileAppendText(Evaluator eval, StackFrame frame)
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

		private static void FileAppendLines(Evaluator eval, StackFrame frame)
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