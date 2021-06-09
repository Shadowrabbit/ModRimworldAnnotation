using System;
using System.Collections.Generic;
using System.IO;

namespace RimWorld.IO
{
	// Token: 0x020021F5 RID: 8693
	public static class AbstractFilesystem
	{
		// Token: 0x0600BA49 RID: 47689 RVA: 0x00078AEE File Offset: 0x00076CEE
		public static void ClearAllCache()
		{
			TarDirectory.ClearCache();
		}

		// Token: 0x0600BA4A RID: 47690 RVA: 0x003594F8 File Offset: 0x003576F8
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

		// Token: 0x0600BA4B RID: 47691 RVA: 0x003595F8 File Offset: 0x003577F8
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
