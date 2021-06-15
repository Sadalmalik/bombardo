using System;
using System.Collections.Generic;
using System.IO;

namespace Bombardo.V2
{
	public static partial class Names
	{
		// Legacy?

		public static readonly string FS_LOAD = "load";
		public static readonly string FS_SAVE = "save";

		// Path operations

		public static readonly string FS_PATH_SUBMODULE = "path";
		public static readonly string FS_PATH_COMBINE = "combine";              // fsPathCombine
		public static readonly string FS_PATH_GET_FULL = "getFull";             // fsPathGetFull
		public static readonly string FS_PATH_GET_EXTENSION = "getExtension";   // fsPathGetExtension
		public static readonly string FS_PATH_GET_FILENAME = "getFileName";     // fsPathGetFileName
		public static readonly string FS_PATH_GET_DIRNAME = "getDirectoryName"; // fsPathGetDirectoryName

		// Directory operations

		public static readonly string FS_DIRECTORY_SUBMODULE = "directory";
		public static readonly string FS_DIRECTORY_READ = "read";     // fsReadDirectory
		public static readonly string FS_DIRECTORY_CREATE = "create"; // fsCreateDirectory
		public static readonly string FS_DIRECTORY_REMOVE = "remove"; // fsRemoveDirectory

		// File operations

		public static readonly string FS_FILE_SUBMODULE = "file";
		public static readonly string FS_FILE_OPEN = "open";   // fsOpen
		public static readonly string FS_FILE_FLUSH = "flush"; // fsFlush
		public static readonly string FS_FILE_CLOSE = "close"; // fsClose

		public static readonly string FS_FILE_READ = "read";   // fsRead
		public static readonly string FS_FILE_WRITE = "write"; // fsWrite

		public static readonly string FS_FILE_READ_LINE = "readLine";   // fsReadline
		public static readonly string FS_FILE_WRITE_LINE = "writeLine"; // fsWriteLine

		public static readonly string FS_FILE_READ_TEXT = "readText";   // fsReadText
		public static readonly string FS_FILE_WRITE_TEXT = "writeText"; // fsWriteText

		public static readonly string FS_FILE_READ_LINES = "readLines";   // fsReadLines
		public static readonly string FS_FILE_WRITE_LINES = "writeLines"; // fsWriteLines

		public static readonly string FS_FILE_APPEND_TEXT = "appendText";   // fsAppendText
		public static readonly string FS_FILE_APPEND_LINES = "appendLines"; // fsAppendLines

		// Predicates

		public static readonly string FS_PRED_EXISTS = "exist?";           // fsExist?
		public static readonly string FS_PRED_FILE = "isFile?";            // fsIsFile?
		public static readonly string FS_PRED_DIR = "isDirectory?";        // fsIsDirectory?
		public static readonly string FS_PRED_EMPTY = "isDirectoryEmpty?"; // fsDirectoryIsEmpty?
	}

	public class FileSystemFunctions
	{
		public static void Define(Context ctx)
		{
			//  (load "filepath") -> list
			//  (save "filepath" (symbol1 symbol2 symbol3 (expression1) (expression2) (expression3)))

			ctx.DefineFunction(Names.FS_LOAD, Load);
			ctx.DefineFunction(Names.FS_SAVE, Save);

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
			dir.DefineFunction(Names.FS_DIRECTORY_REMOVE, RemoveDirectory);

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
			Atom args = frame.args;

			Atom path = (Atom) args?.value;

			if (path == null || !path.IsString)
				throw new ArgumentException("Path must be string!");

			string file = (string) path.value;
			if (File.Exists(file))
			{
				string raw   = File.ReadAllText(file);
				Atom   nodes = BombardoLang.Parse(raw);

				eval.Return(nodes);
				return;
			}

			eval.Return(null);
		}

		private static void Save(Evaluator eval, StackFrame frame)
		{
			Atom args = frame.args;

			Atom path = (Atom) args?.value;
			Atom list = (Atom) args?.next?.value;

			if (path == null || !path.IsString)
				throw new ArgumentException("Path must be string!");
			if (list == null || !list.IsPair)
				throw new ArgumentException("second argument must be list!");

			FileStream   stream = File.Open((string) path.value, FileMode.Create);
			StreamWriter output = new StreamWriter(stream);

			while (list != null)
			{
				Atom item = (Atom) list?.value;
				output.WriteLine(item != null ? item.Stringify() : "");
				list = list.next;
			}

			output.Flush();
			output.Dispose();
			stream.Dispose();

			eval.Return(null);
		}

#endregion


#region Path operations

		private static void PathCombile(Evaluator eval, StackFrame frame)
		{
			Atom args = frame.args;

			int      i     = 0;
			string[] parts = new string[args.ListLength()];

			for (Atom iter = args; iter != null; iter = iter.next)
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
			Atom args = frame.args;

			Atom path = args?.atom;

			if (!path.IsString)
				throw new ArgumentException("Path must be string!");

			eval.Return(new Atom(AtomType.String, Path.GetFullPath((string) path.value)));
		}

		private static void PathGetExtension(Evaluator eval, StackFrame frame)
		{
			Atom args = frame.args;

			Atom path = args?.atom;

			if (!path.IsString)
				throw new ArgumentException("Path must be string!");

			eval.Return(new Atom(AtomType.String, Path.GetExtension((string) path.value)));
		}

		private static void PathGetFileName(Evaluator eval, StackFrame frame)
		{
			Atom args = frame.args;

			Atom path = args?.atom;

			if (!path.IsString)
				throw new ArgumentException("Path must be string!");

			eval.Return(new Atom(AtomType.String, Path.GetFileName((string) path.value)));
		}

		private static void PathGetDirectoryName(Evaluator eval, StackFrame frame)
		{
			Atom args = frame.args;

			Atom path = args?.atom;

			if (!path.IsString)
				throw new ArgumentException("Path must be string!");

			eval.Return(new Atom(AtomType.String, Path.GetDirectoryName((string) path.value)));
		}

#endregion


#region Directory operations

		private static void ReadDirectory(Evaluator eval, StackFrame frame)
		{
			Atom args = frame.args;

			Atom path = args?.atom;
			if (path == null || !path.IsString)
				throw new ArgumentException("Path must be string!");
			string       directory = (string) path.value;
			Atom         pattern   = args?.next?.atom;
			Atom         mode      = args?.next?.next?.atom;
			SearchOption option    = ArgUtils.GetEnum<SearchOption>(mode, 3);

			string[] dirs = null;
			if (pattern == null) dirs = Directory.GetDirectories(directory);
			else dirs                 = Directory.GetDirectories(directory, (string) pattern.value, option);

			string[] files = null;
			if (pattern == null) files = Directory.GetFiles(directory);
			else files                 = Directory.GetFiles(directory, (string) pattern.value, option);

			Atom[] elements                                                  = new Atom[dirs.Length + files.Length];
			for (int i = 0; i < dirs.Length; i++) elements[i]                = new Atom(AtomType.String, dirs[i]);
			for (int i = 0; i < files.Length; i++) elements[i + dirs.Length] = new Atom(AtomType.String, files[i]);

			eval.Return(StructureUtils.List(elements));
		}

		private static void CreateDirectory(Evaluator eval, StackFrame frame)
		{
			Atom args = frame.args;

			Atom path = (Atom) args?.value;
			if (path == null || !path.IsString)
				throw new ArgumentException("Path must be string!");
			string directoryPath = (string) path.value;

			Directory.CreateDirectory(directoryPath);

			eval.Return(null);
		}

		private static void RemoveDirectory(Evaluator eval, StackFrame frame)
		{
			Atom args = frame.args;

			Atom path = (Atom) args?.value;
			if (path == null || !path.IsString)
				throw new ArgumentException("Path must be string!");
			string directoryPath = (string) path.value;

			string[] dirs  = Directory.GetDirectories(directoryPath);
			string[] files = Directory.GetFiles(directoryPath);
			bool     empty = dirs.Length == 0 && files.Length == 0;

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
			string file = (string) path.value;

			FileAccess access = ArgUtils.GetEnum<FileAccess>(accessType, 1, FileAccess.Read);
			FileMode   mode   = ArgUtils.GetEnum<FileMode>(modeType, 2, FileMode.Open);

			if (access == FileAccess.Read)
			{
				StreamReader reader = new StreamReader(File.Open(file, mode, access));

				eval.Return(new Atom(AtomType.Native, reader));
				return;
			}

			if (access == FileAccess.Write)
			{
				StreamWriter writer = new StreamWriter(File.Open(file, mode, access));

				eval.Return(new Atom(AtomType.Native, writer));
				return;
			}

			eval.Return(null);
		}

		private static void Flush(Evaluator eval, StackFrame frame)
		{
			Atom stream = frame.args?.atom;

			if (stream.type != AtomType.Native)
				throw new ArgumentException("Argument must be stream!");

			StreamWriter writer = stream.value as StreamWriter;
			writer?.Flush();

			eval.Return(null);
		}

		private static void Close(Evaluator eval, StackFrame frame)
		{
			Atom stream = frame.args?.atom;

			if (stream.type != AtomType.Native)
				throw new ArgumentException("Argument must be stream!");

			StreamReader reader = stream.value as StreamReader;
			reader?.Close();

			StreamWriter writer = stream.value as StreamWriter;
			writer?.Close();

			eval.Return(null);
		}

		private static void Read(Evaluator eval, StackFrame frame)
		{
			Atom stream = frame.args?.atom;

			if (stream.type != AtomType.Native)
				throw new ArgumentException("Argument must be stream!");

			StreamReader reader = stream.value as StreamReader;
			if (reader == null)
				throw new ArgumentException("Argument must be stream!");

			eval.Return(new Atom(AtomType.Number, reader.Read()));
		}

		private static void ReadLine(Evaluator eval, StackFrame frame)
		{
			Atom stream = frame.args?.atom;

			if (stream.type != AtomType.Native)
				throw new ArgumentException("Argument must be stream!");

			StreamReader reader = stream.value as StreamReader;
			if (reader == null)
				throw new ArgumentException("Argument must be stream!");

			eval.Return(new Atom(AtomType.String, reader.ReadLine()));
		}

		private static void Write(Evaluator eval, StackFrame frame)
		{
			var (stream, value) = StructureUtils.Split2(frame.args);

			if (stream.type != AtomType.Native)
				throw new ArgumentException("Argument must be stream!");

			StreamWriter writer = stream?.value as StreamWriter;
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
			Atom path = frame.args?.atom;

			if (path == null || !path.IsString)
				throw new ArgumentException("Path must be string!");
			string file = (string) path.value;

			string text = File.ReadAllText(file, System.Text.Encoding.UTF8);

			eval.Return(new Atom(AtomType.String, text));
		}

		private static void ReadLines(Evaluator eval, StackFrame frame)
		{
			Atom path = frame.args?.atom;

			if (path == null || !path.IsString)
				throw new ArgumentException("Path must be string!");
			string file = (string) path.value;

			string[] lines = File.ReadAllLines(file, System.Text.Encoding.UTF8);

			Atom[] atoms = new Atom[lines.Length];
			for (int i = 0; i < lines.Length; i++)
				atoms[i] = new Atom(AtomType.String, lines[i]);

			eval.Return(StructureUtils.List(atoms));
		}

		private static void WriteText(Evaluator eval, StackFrame frame)
		{
			var (path, text) = StructureUtils.Split2(frame.args);

			if (path == null || !path.IsString)
				throw new ArgumentException("First argument must be string!");
			string file = (string) path.value;

			if (text == null || !text.IsString)
				throw new ArgumentException("Second argument must be string!");
			string data = (string) text.value;

			File.WriteAllText(file, data, System.Text.Encoding.UTF8);

			eval.Return(null);
		}

		private static void WriteLines(Evaluator eval, StackFrame frame)
		{
			var (path, list) = StructureUtils.Split2(frame.args);

			if (path == null || !path.IsString)
				throw new ArgumentException("First argument must be string!");
			string file = (string) path.value;

			if (list == null || !list.IsPair)
				throw new ArgumentException("Second argument must be list of strings!");

			List<string> lines = new List<string>();
			for (Atom iter = list; iter != null; iter = iter.next)
			{
				Atom line = iter.atom;
				if (line == null || !line.IsString)
					throw new ArgumentException("Second argument must be list of strings!");
				lines.Add((string) line.value);
			}

			File.WriteAllLines(file, lines, System.Text.Encoding.UTF8);

			eval.Return(null);
		}

		private static void AppendText(Evaluator eval, StackFrame frame)
		{
			var (path, text) = StructureUtils.Split2(frame.args);

			if (path == null || !path.IsString)
				throw new ArgumentException("First argument must be string!");
			string file = (string) path.value;

			if (text == null || !text.IsString)
				throw new ArgumentException("Second argument must be string!");
			string data = (string) text.value;

			File.AppendAllText(file, data, System.Text.Encoding.UTF8);

			eval.Return(null);
		}

		private static void AppendLines(Evaluator eval, StackFrame frame)
		{
			var (path, list) = StructureUtils.Split2(frame.args);

			if (path == null || !path.IsString)
				throw new ArgumentException("First argument must be string!");
			string file = (string) path.value;

			if (list == null || !list.IsPair)
				throw new ArgumentException("Second argument must be list of strings!");

			List<string> lines = new List<string>();
			for (Atom iter = list; iter != null; iter = iter.next)
			{
				Atom line = iter.atom;
				if (line == null || !line.IsString)
					throw new ArgumentException("Second argument must be list of strings!");
				lines.Add((string) line.value);
			}

			File.AppendAllLines(file, lines, System.Text.Encoding.UTF8);

			eval.Return(null);
		}

#endregion


#region Predicates

		private static void Exists(Evaluator eval, StackFrame frame)
		{
			Atom path = (Atom) frame.args?.value;

			if (path == null || !path.IsString)
				throw new ArgumentException("Path must be string!");
			var file = (string) path.value;

			eval.Return(File.Exists(file) ? Atoms.TRUE : Atoms.FALSE);
		}


		private static void FilePredicate(Evaluator eval, StackFrame frame)
		{
			Atom path = (Atom) frame.args?.value;

			if (path == null || !path.IsString)
				throw new ArgumentException("Path must be string!");
			var file = (string) path.value;

			if (!File.Exists(file))
			{
				eval.Return(Atoms.FALSE);
				return;
			}

			FileAttributes attr = File.GetAttributes(file);
			eval.Return(attr.HasFlag(FileAttributes.Directory) ? Atoms.FALSE : Atoms.TRUE);
		}

		private static void DirectoryPredicate(Evaluator eval, StackFrame frame)
		{
			Atom path = (Atom) frame.args?.value;

			if (path == null || !path.IsString)
				throw new ArgumentException("Path must be string!");
			string file = (string) path.value;

			if (!File.Exists(file))
			{
				eval.Return(Atoms.FALSE);
				return;
			}

			FileAttributes attr = File.GetAttributes(file);
			eval.Return(attr.HasFlag(FileAttributes.Directory) ? Atoms.TRUE : Atoms.FALSE);
		}

		private static void DirectoryEmptyPredicate(Evaluator eval, StackFrame frame)
		{
			Atom path = (Atom) frame.args?.value;

			if (path == null || !path.IsString)
				throw new ArgumentException("Path must be string!");
			string directoryPath = (string) path.value;

			string[] dirs  = Directory.GetDirectories(directoryPath);
			string[] files = Directory.GetFiles(directoryPath);
			bool     empty = dirs.Length == 0 && files.Length == 0;

			eval.Return(empty ? Atoms.TRUE : Atoms.FALSE);
		}

#endregion
	}
}