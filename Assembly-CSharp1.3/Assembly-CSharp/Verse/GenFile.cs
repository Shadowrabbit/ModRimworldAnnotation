using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000027 RID: 39
	public static class GenFile
	{
		// Token: 0x0600022C RID: 556 RVA: 0x0000B5C0 File Offset: 0x000097C0
		public static string TextFromRawFile(string filePath)
		{
			return File.ReadAllText(filePath);
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000B5C8 File Offset: 0x000097C8
		public static string TextFromResourceFile(string filePath)
		{
			TextAsset textAsset = Resources.Load("Text/" + filePath) as TextAsset;
			if (textAsset == null)
			{
				Log.Message("Found no text asset in resources at " + filePath);
				return null;
			}
			return GenFile.GetTextWithoutBOM(textAsset);
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0000B60C File Offset: 0x0000980C
		public static string GetTextWithoutBOM(TextAsset textAsset)
		{
			string result = null;
			using (MemoryStream memoryStream = new MemoryStream(textAsset.bytes))
			{
				using (StreamReader streamReader = new StreamReader(memoryStream, true))
				{
					result = streamReader.ReadToEnd();
				}
			}
			return result;
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0000B66C File Offset: 0x0000986C
		public static IEnumerable<string> LinesFromFile(string filePath)
		{
			string text = GenFile.TextFromResourceFile(filePath);
			foreach (string text2 in GenText.LinesFromString(text))
			{
				yield return text2;
			}
			IEnumerator<string> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0000B67C File Offset: 0x0000987C
		public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs, bool useLinuxLineEndings = false)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(sourceDirName);
			DirectoryInfo[] directories = directoryInfo.GetDirectories();
			if (!directoryInfo.Exists)
			{
				throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDirName);
			}
			if (!Directory.Exists(destDirName))
			{
				Directory.CreateDirectory(destDirName);
			}
			foreach (FileInfo fileInfo in directoryInfo.GetFiles())
			{
				string text = Path.Combine(destDirName, fileInfo.Name);
				if (useLinuxLineEndings && (fileInfo.Extension == ".sh" || fileInfo.Extension == ".txt"))
				{
					if (!File.Exists(text))
					{
						File.WriteAllText(text, File.ReadAllText(fileInfo.FullName).Replace("\r\n", "\n").Replace("\r", "\n"));
					}
				}
				else
				{
					fileInfo.CopyTo(text, false);
				}
			}
			if (copySubDirs)
			{
				foreach (DirectoryInfo directoryInfo2 in directories)
				{
					string destDirName2 = Path.Combine(destDirName, directoryInfo2.Name);
					GenFile.DirectoryCopy(directoryInfo2.FullName, destDirName2, copySubDirs, useLinuxLineEndings);
				}
			}
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0000B790 File Offset: 0x00009990
		public static string SanitizedFileName(string fileName)
		{
			char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
			string text = "";
			for (int i = 0; i < fileName.Length; i++)
			{
				if (!invalidFileNameChars.Contains(fileName[i]))
				{
					text += fileName[i].ToString();
				}
			}
			if (text.Length == 0)
			{
				text = "unnamed";
			}
			return text;
		}
	}
}
