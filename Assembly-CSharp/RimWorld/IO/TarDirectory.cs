using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ICSharpCode.SharpZipLib.Tar;

namespace RimWorld.IO
{
	// Token: 0x020021FA RID: 8698
	internal class TarDirectory : VirtualDirectory
	{
		// Token: 0x0600BA70 RID: 47728 RVA: 0x00078C88 File Offset: 0x00076E88
		public static void ClearCache()
		{
			TarDirectory.cache.Clear();
		}

		// Token: 0x0600BA71 RID: 47729 RVA: 0x00359870 File Offset: 0x00357A70
		public static TarDirectory ReadFromFileOrCache(string file)
		{
			string key = file.Replace('\\', '/');
			TarDirectory tarDirectory;
			if (!TarDirectory.cache.TryGetValue(key, out tarDirectory))
			{
				tarDirectory = new TarDirectory(file, "");
				tarDirectory.lazyLoadArchive = file;
				TarDirectory.cache.Add(key, tarDirectory);
			}
			return tarDirectory;
		}

		// Token: 0x0600BA72 RID: 47730 RVA: 0x003598B8 File Offset: 0x00357AB8
		private void CheckLazyLoad()
		{
			if (this.lazyLoadArchive != null)
			{
				using (FileStream fileStream = File.OpenRead(this.lazyLoadArchive))
				{
					using (TarInputStream tarInputStream = new TarInputStream(fileStream))
					{
						TarDirectory.ParseTAR(this, tarInputStream, this.lazyLoadArchive);
					}
				}
				this.lazyLoadArchive = null;
			}
		}

		// Token: 0x0600BA73 RID: 47731 RVA: 0x00359928 File Offset: 0x00357B28
		private static void ParseTAR(TarDirectory root, TarInputStream input, string fullPath)
		{
			Dictionary<string, byte[]> dictionary = new Dictionary<string, byte[]>();
			List<TarEntry> list = new List<TarEntry>();
			List<TarDirectory> list2 = new List<TarDirectory>();
			byte[] buffer = new byte[16384];
			try
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					TarEntry nextEntry;
					while ((nextEntry = input.GetNextEntry()) != null)
					{
						TarDirectory.ReadTarEntryData(input, memoryStream, buffer);
						dictionary.Add(nextEntry.Name, memoryStream.ToArray());
						list.Add(nextEntry);
						memoryStream.Position = 0L;
						memoryStream.SetLength(0L);
					}
				}
				list2.Add(root);
				foreach (TarEntry tarEntry in from e in list
				where e.IsDirectory && !string.IsNullOrEmpty(e.Name)
				select e)
				{
					string str = TarDirectory.FormatFolderPath(tarEntry.Name);
					list2.Add(new TarDirectory(fullPath + "/" + str, str));
				}
				foreach (TarEntry tarEntry2 in from e in list
				where !e.IsDirectory
				select e)
				{
					string b = TarDirectory.FormatFolderPath(Path.GetDirectoryName(tarEntry2.Name));
					TarDirectory tarDirectory = null;
					foreach (TarDirectory tarDirectory2 in list2)
					{
						if (tarDirectory2.inArchiveFullPath == b)
						{
							tarDirectory = tarDirectory2;
							break;
						}
					}
					tarDirectory.files.Add(new TarFile(dictionary[tarEntry2.Name], fullPath + "/" + tarEntry2.Name, Path.GetFileName(tarEntry2.Name)));
				}
				foreach (TarDirectory tarDirectory3 in list2)
				{
					if (!string.IsNullOrEmpty(tarDirectory3.inArchiveFullPath))
					{
						string b2 = TarDirectory.FormatFolderPath(Path.GetDirectoryName(tarDirectory3.inArchiveFullPath));
						TarDirectory tarDirectory4 = null;
						foreach (TarDirectory tarDirectory5 in list2)
						{
							if (tarDirectory5.inArchiveFullPath == b2)
							{
								tarDirectory4 = tarDirectory5;
								break;
							}
						}
						tarDirectory4.subDirectories.Add(tarDirectory3);
					}
				}
			}
			finally
			{
				input.Close();
			}
		}

		// Token: 0x0600BA74 RID: 47732 RVA: 0x00359C64 File Offset: 0x00357E64
		private static string FormatFolderPath(string str)
		{
			if (str.Length == 0)
			{
				return str;
			}
			if (str.IndexOf('\\') != -1)
			{
				str = str.Replace('\\', '/');
			}
			if (str[str.Length - 1] == '/')
			{
				str = str.Substring(0, str.Length - 1);
			}
			return str;
		}

		// Token: 0x0600BA75 RID: 47733 RVA: 0x00359CB8 File Offset: 0x00357EB8
		private static void ReadTarEntryData(TarInputStream tarIn, Stream outStream, byte[] buffer = null)
		{
			if (buffer == null)
			{
				buffer = new byte[4096];
			}
			for (int i = tarIn.Read(buffer, 0, buffer.Length); i > 0; i = tarIn.Read(buffer, 0, buffer.Length))
			{
				outStream.Write(buffer, 0, i);
			}
		}

		// Token: 0x0600BA76 RID: 47734 RVA: 0x00078C94 File Offset: 0x00076E94
		private static IEnumerable<TarDirectory> EnumerateAllChildrenRecursive(TarDirectory of)
		{
			foreach (TarDirectory dir in of.subDirectories)
			{
				yield return dir;
				foreach (TarDirectory tarDirectory in TarDirectory.EnumerateAllChildrenRecursive(dir))
				{
					yield return tarDirectory;
				}
				IEnumerator<TarDirectory> enumerator2 = null;
				dir = null;
			}
			List<TarDirectory>.Enumerator enumerator = default(List<TarDirectory>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600BA77 RID: 47735 RVA: 0x00078CA4 File Offset: 0x00076EA4
		private static IEnumerable<TarFile> EnumerateAllFilesRecursive(TarDirectory of)
		{
			foreach (TarFile tarFile in of.files)
			{
				yield return tarFile;
			}
			List<TarFile>.Enumerator enumerator = default(List<TarFile>.Enumerator);
			foreach (TarDirectory of2 in of.subDirectories)
			{
				foreach (TarFile tarFile2 in TarDirectory.EnumerateAllFilesRecursive(of2))
				{
					yield return tarFile2;
				}
				IEnumerator<TarFile> enumerator3 = null;
			}
			List<TarDirectory>.Enumerator enumerator2 = default(List<TarDirectory>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600BA78 RID: 47736 RVA: 0x00359CFC File Offset: 0x00357EFC
		private static Func<string, bool> GetPatternMatcher(string searchPattern)
		{
			Func<string, bool> func = null;
			if (searchPattern.Length == 1 && searchPattern[0] == '*')
			{
				func = ((string str) => true);
			}
			else if (searchPattern.Length > 2 && searchPattern[0] == '*' && searchPattern[1] == '.')
			{
				string extension = searchPattern.Substring(2);
				func = ((string str) => str.Substring(str.Length - extension.Length) == extension);
			}
			if (func == null)
			{
				func = ((string str) => false);
			}
			return func;
		}

		// Token: 0x17001BC7 RID: 7111
		// (get) Token: 0x0600BA79 RID: 47737 RVA: 0x00078CB4 File Offset: 0x00076EB4
		public override string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17001BC8 RID: 7112
		// (get) Token: 0x0600BA7A RID: 47738 RVA: 0x00078CBC File Offset: 0x00076EBC
		public override string FullPath
		{
			get
			{
				return this.fullPath;
			}
		}

		// Token: 0x17001BC9 RID: 7113
		// (get) Token: 0x0600BA7B RID: 47739 RVA: 0x00078CC4 File Offset: 0x00076EC4
		public override bool Exists
		{
			get
			{
				return this.exists;
			}
		}

		// Token: 0x0600BA7C RID: 47740 RVA: 0x00078CCC File Offset: 0x00076ECC
		private TarDirectory(string fullPath, string inArchiveFullPath)
		{
			this.name = Path.GetFileNameWithoutExtension(fullPath);
			this.fullPath = fullPath;
			this.inArchiveFullPath = inArchiveFullPath;
			this.exists = true;
		}

		// Token: 0x0600BA7D RID: 47741 RVA: 0x00078D0B File Offset: 0x00076F0B
		private TarDirectory()
		{
			this.exists = false;
		}

		// Token: 0x0600BA7E RID: 47742 RVA: 0x00359DA4 File Offset: 0x00357FA4
		public override VirtualDirectory GetDirectory(string directoryName)
		{
			this.CheckLazyLoad();
			string text = directoryName;
			if (!string.IsNullOrEmpty(this.fullPath))
			{
				text = this.fullPath + "/" + text;
			}
			foreach (TarDirectory tarDirectory in this.subDirectories)
			{
				if (tarDirectory.fullPath == text)
				{
					return tarDirectory;
				}
			}
			return TarDirectory.NotFound;
		}

		// Token: 0x0600BA7F RID: 47743 RVA: 0x00359E30 File Offset: 0x00358030
		public override VirtualFile GetFile(string filename)
		{
			this.CheckLazyLoad();
			VirtualDirectory virtualDirectory = this;
			string[] array = filename.Split(new char[]
			{
				'/',
				'\\'
			});
			for (int i = 0; i < array.Length - 1; i++)
			{
				virtualDirectory = virtualDirectory.GetDirectory(array[i]);
			}
			filename = array[array.Length - 1];
			if (virtualDirectory == this)
			{
				using (List<TarFile>.Enumerator enumerator = this.files.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						TarFile tarFile = enumerator.Current;
						if (tarFile.Name == filename)
						{
							return tarFile;
						}
					}
					goto IL_93;
				}
				goto IL_8B;
				IL_93:
				return TarFile.NotFound;
			}
			IL_8B:
			return virtualDirectory.GetFile(filename);
		}

		// Token: 0x0600BA80 RID: 47744 RVA: 0x00078D30 File Offset: 0x00076F30
		public override IEnumerable<VirtualFile> GetFiles(string searchPattern, SearchOption searchOption)
		{
			this.CheckLazyLoad();
			IEnumerable<TarFile> enumerable = this.files;
			if (searchOption == SearchOption.AllDirectories)
			{
				enumerable = TarDirectory.EnumerateAllFilesRecursive(this);
			}
			Func<string, bool> matcher = TarDirectory.GetPatternMatcher(searchPattern);
			foreach (TarFile tarFile in enumerable)
			{
				if (matcher(tarFile.Name))
				{
					yield return tarFile;
				}
			}
			IEnumerator<TarFile> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BA81 RID: 47745 RVA: 0x00078D4E File Offset: 0x00076F4E
		public override IEnumerable<VirtualDirectory> GetDirectories(string searchPattern, SearchOption searchOption)
		{
			this.CheckLazyLoad();
			IEnumerable<TarDirectory> enumerable = this.subDirectories;
			if (searchOption == SearchOption.AllDirectories)
			{
				enumerable = TarDirectory.EnumerateAllChildrenRecursive(this);
			}
			Func<string, bool> matcher = TarDirectory.GetPatternMatcher(searchPattern);
			foreach (TarDirectory tarDirectory in enumerable)
			{
				if (matcher(tarDirectory.Name))
				{
					yield return tarDirectory;
				}
			}
			IEnumerator<TarDirectory> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BA82 RID: 47746 RVA: 0x00359EE8 File Offset: 0x003580E8
		public override string ToString()
		{
			return string.Format("TarDirectory [{0}], {1} files", this.fullPath, this.files.Count.ToString());
		}

		// Token: 0x04007F44 RID: 32580
		private static Dictionary<string, TarDirectory> cache = new Dictionary<string, TarDirectory>();

		// Token: 0x04007F45 RID: 32581
		private string lazyLoadArchive;

		// Token: 0x04007F46 RID: 32582
		private static readonly TarDirectory NotFound = new TarDirectory();

		// Token: 0x04007F47 RID: 32583
		private string fullPath;

		// Token: 0x04007F48 RID: 32584
		private string inArchiveFullPath;

		// Token: 0x04007F49 RID: 32585
		private string name;

		// Token: 0x04007F4A RID: 32586
		private bool exists;

		// Token: 0x04007F4B RID: 32587
		public List<TarDirectory> subDirectories = new List<TarDirectory>();

		// Token: 0x04007F4C RID: 32588
		public List<TarFile> files = new List<TarFile>();
	}
}
