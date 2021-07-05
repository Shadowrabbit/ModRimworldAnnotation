using System;
using System.Collections.Generic;
using System.IO;

namespace RimWorld.IO
{
	// Token: 0x0200181E RID: 6174
	public static class AbstractFilesystem
	{
		// Token: 0x060090D7 RID: 37079 RVA: 0x0033F62B File Offset: 0x0033D82B
		public static void ClearAllCache()
		{
			TarDirectory.ClearCache();
		}

		// Token: 0x060090D8 RID: 37080 RVA: 0x0033F634 File Offset: 0x0033D834
		public static List<VirtualDirectory> GetDirectories(string filesystemPath, string searchPattern, SearchOption searchOption, bool allowArchiveAndRealFolderDuplicates = false)
		{
			List<VirtualDirectory> list = new List<VirtualDirectory>();
			foreach (string text in Directory.GetDirectories(filesystemPath, searchPattern, searchOption))
			{
				string text2 = text + ".tar";
				if (!allowArchiveAndRealFolderDuplicates && File.Exists(text2))
				{
					list.Add(TarDirectory.ReadFromFileOrCache(text2));
				}
				else
				{
					list.Add(new FilesystemDirectory(text));
				}
			}
			foreach (string text3 in Directory.GetFiles(filesystemPath, searchPattern, searchOption))
			{
				if (!(Path.GetExtension(text3) != ".tar"))
				{
					if (!allowArchiveAndRealFolderDuplicates)
					{
						string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text3);
						bool flag = false;
						using (List<VirtualDirectory>.Enumerator enumerator = list.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								if (enumerator.Current.Name == fileNameWithoutExtension)
								{
									flag = true;
									break;
								}
							}
						}
						if (flag)
						{
							goto IL_D7;
						}
					}
					list.Add(TarDirectory.ReadFromFileOrCache(text3));
				}
				IL_D7:;
			}
			return list;
		}

		// Token: 0x060090D9 RID: 37081 RVA: 0x0033F734 File Offset: 0x0033D934
		public static VirtualDirectory GetDirectory(string filesystemPath)
		{
			if (Path.GetExtension(filesystemPath) == ".tar")
			{
				return TarDirectory.ReadFromFileOrCache(filesystemPath);
			}
			string text = filesystemPath + ".tar";
			if (File.Exists(text))
			{
				return TarDirectory.ReadFromFileOrCache(text);
			}
			return new FilesystemDirectory(filesystemPath);
		}
	}
}
