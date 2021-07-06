using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001021 RID: 4129
	public static class ExternalHistoryUtility
	{
		// Token: 0x06005A1F RID: 23071 RVA: 0x001D45F4 File Offset: 0x001D27F4
		static ExternalHistoryUtility()
		{
			try
			{
				ExternalHistoryUtility.cachedFiles = GenFilePaths.AllExternalHistoryFiles.ToList<FileInfo>();
			}
			catch (Exception ex)
			{
				Log.Error("Could not get external history files: " + ex.Message, false);
			}
		}

		// Token: 0x17000DFB RID: 3579
		// (get) Token: 0x06005A20 RID: 23072 RVA: 0x0003E8B3 File Offset: 0x0003CAB3
		public static IEnumerable<FileInfo> Files
		{
			get
			{
				int num;
				for (int i = 0; i < ExternalHistoryUtility.cachedFiles.Count; i = num)
				{
					yield return ExternalHistoryUtility.cachedFiles[i];
					num = i + 1;
				}
				yield break;
			}
		}

		// Token: 0x06005A21 RID: 23073 RVA: 0x001D464C File Offset: 0x001D284C
		public static ExternalHistory Load(string path)
		{
			ExternalHistory result = null;
			try
			{
				result = new ExternalHistory();
				Scribe.loader.InitLoading(path);
				try
				{
					Scribe_Deep.Look<ExternalHistory>(ref result, "externalHistory", Array.Empty<object>());
					Scribe.loader.FinalizeLoading();
				}
				catch
				{
					Scribe.ForceStop();
					throw;
				}
			}
			catch (Exception ex)
			{
				Log.Error("Could not load external history (" + path + "): " + ex.Message, false);
				return null;
			}
			return result;
		}

		// Token: 0x06005A22 RID: 23074 RVA: 0x001D46D4 File Offset: 0x001D28D4
		public static string GetRandomGameplayID()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < ExternalHistoryUtility.gameplayIDLength; i++)
			{
				int index = Rand.Range(0, ExternalHistoryUtility.gameplayIDAvailableChars.Length);
				stringBuilder.Append(ExternalHistoryUtility.gameplayIDAvailableChars[index]);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06005A23 RID: 23075 RVA: 0x001D4720 File Offset: 0x001D2920
		public static bool IsValidGameplayID(string ID)
		{
			if (ID.NullOrEmpty() || ID.Length != ExternalHistoryUtility.gameplayIDLength)
			{
				return false;
			}
			for (int i = 0; i < ID.Length; i++)
			{
				bool flag = false;
				for (int j = 0; j < ExternalHistoryUtility.gameplayIDAvailableChars.Length; j++)
				{
					if (ID[i] == ExternalHistoryUtility.gameplayIDAvailableChars[j])
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06005A24 RID: 23076 RVA: 0x001D478C File Offset: 0x001D298C
		public static string GetCurrentUploadDate()
		{
			return DateTime.UtcNow.ToString("yyMMdd");
		}

		// Token: 0x06005A25 RID: 23077 RVA: 0x001D47AC File Offset: 0x001D29AC
		public static int GetCurrentUploadTime()
		{
			return (int)(DateTime.UtcNow.TimeOfDay.TotalSeconds / 2.0);
		}

		// Token: 0x04003CA8 RID: 15528
		private static List<FileInfo> cachedFiles;

		// Token: 0x04003CA9 RID: 15529
		private static int gameplayIDLength = 20;

		// Token: 0x04003CAA RID: 15530
		private static string gameplayIDAvailableChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
	}
}
