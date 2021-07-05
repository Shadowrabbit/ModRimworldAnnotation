using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ICSharpCode.SharpZipLib.Tar;

namespace RimWorld.IO
{
	// Token: 0x02001821 RID: 6177
	internal class TarDirectory : VirtualDirectory
	{
		// Token: 0x060090EE RID: 37102 RVA: 0x0033F8EC File Offset: 0x0033DAEC
		public static void ClearCache()
		{
			TarDirectory.cache.Clear();
		}

		// Token: 0x060090EF RID: 37103 RVA: 0x0033F8F8 File Offset: 0x0033DAF8
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

		// Token: 0x060090F0 RID: 37104 RVA: 0x0033F940 File Offset: 0x0033DB40
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

		// Token: 0x060090F1 RID: 37105 RVA: 0x0033F9B0 File Offset: 0x0033DBB0
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

		// Token: 0x060090F2 RID: 37106 RVA: 0x0033FCEC File Offset: 0x0033DEEC
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

		// Token: 0x060090F3 RID: 37107 RVA: 0x0033FD40 File Offset: 0x0033DF40
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

		// Token: 0x060090F4 RID: 37108 RVA: 0x0033FD83 File Offset: 0x0033DF83
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

		// Token: 0x060090F5 RID: 37109 RVA: 0x0033FD93 File Offset: 0x0033DF93
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

		// Token: 0x060090F6 RID: 37110 RVA: 0x0033FDA4 File Offset: 0x0033DFA4
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

		// Token: 0x170017DF RID: 6111
		// (get) Token: 0x060090F7 RID: 37111 RVA: 0x0033FE4A File Offset: 0x0033E04A
		public override string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x170017E0 RID: 6112
		// (get) Token: 0x060090F8 RID: 37112 RVA: 0x0033FE52 File Offset: 0x0033E052
		public override string FullPath
		{
			get
			{
				return this.fullPath;
			}
		}

		// Token: 0x170017E1 RID: 6113
		// (get) Token: 0x060090F9 RID: 37113 RVA: 0x0033FE5A File Offset: 0x0033E05A
		public override bool Exists
		{
			get
			{
				return this.exists;
			}
		}

		// Token: 0x060090FA RID: 37114 RVA: 0x0033FE64 File Offset: 0x0033E064
		private TarDirectory(string fullPath, string inArchiveFullPath)
		{
			this.name = Path.GetFileName(fullPath);
			if (this.name.IndexOf(".tar") == this.name.Length - 4)
			{
				this.name = this.name.Substring(0, this.name.Length - 4);
			}
			this.fullPath = fullPath;
			this.inArchiveFullPath = inArchiveFullPath;
			this.exists = true;
		}

		// Token: 0x060090FB RID: 37115 RVA: 0x0033FEEC File Offset: 0x0033E0EC
		private TarDirectory()
		{
			this.exists = false;
		}

		// Token: 0x060090FC RID: 37116 RVA: 0x0033FF14 File Offset: 0x0033E114
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

		// Token: 0x060090FD RID: 37117 RVA: 0x0033FFA0 File Offset: 0x0033E1A0
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

		// Token: 0x060090FE RID: 37118 RVA: 0x00340058 File Offset: 0x0033E258
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

		// Token: 0x060090FF RID: 37119 RVA: 0x00340076 File Offset: 0x0033E276
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

		// Token: 0x06009100 RID: 37120 RVA: 0x00340094 File Offset: 0x0033E294
		public override string ToString()
		{
			return string.Format("TarDirectory [{0}], {1} files", this.fullPath, this.files.Count.ToString());
		}

		// Token: 0x04005B05 RID: 23301
		private static Dictionary<string, TarDirectory> cache = new Dictionary<string, TarDirectory>();

		// Token: 0x04005B06 RID: 23302
		private string lazyLoadArchive;

		// Token: 0x04005B07 RID: 23303
		private static readonly TarDirectory NotFound = new TarDirectory();

		// Token: 0x04005B08 RID: 23304
		private string fullPath;

		// Token: 0x04005B09 RID: 23305
		private string inArchiveFullPath;

		// Token: 0x04005B0A RID: 23306
		private string name;

		// Token: 0x04005B0B RID: 23307
		private bool exists;

		// Token: 0x04005B0C RID: 23308
		public List<TarDirectory> subDirectories = new List<TarDirectory>();

		// Token: 0x04005B0D RID: 23309
		public List<TarFile> files = new List<TarFile>();
	}
}
