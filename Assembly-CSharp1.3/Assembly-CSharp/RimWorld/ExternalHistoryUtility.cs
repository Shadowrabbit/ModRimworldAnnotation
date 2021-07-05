using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AF7 RID: 2807
	public static class ExternalHistoryUtility
	{
		// Token: 0x06004213 RID: 16915 RVA: 0x00161C3C File Offset: 0x0015FE3C
		static ExternalHistoryUtility()
		{
			try
			{
				ExternalHistoryUtility.cachedFiles = GenFilePaths.AllExternalHistoryFiles.ToList<FileInfo>();
			}
			catch (Exception ex)
			{
				Log.Error("Could not get external history files: " + ex.Message);
			}
		}

		// Token: 0x17000BA2 RID: 2978
		// (get) Token: 0x06004214 RID: 16916 RVA: 0x00161C94 File Offset: 0x0015FE94
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

		// Token: 0x06004215 RID: 16917 RVA: 0x00161CA0 File Offset: 0x0015FEA0
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
				Log.Error("Could not load external history (" + path + "): " + ex.Message);
				return null;
			}
			return result;
		}

		// Token: 0x06004216 RID: 16918 RVA: 0x00161D28 File Offset: 0x0015FF28
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

		// Token: 0x06004217 RID: 16919 RVA: 0x00161D74 File Offset: 0x0015FF74
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

		// Token: 0x06004218 RID: 16920 RVA: 0x00161DE0 File Offset: 0x0015FFE0
		public static string GetCurrentUploadDate()
		{
			return DateTime.UtcNow.ToString("yyMMdd");
		}

		// Token: 0x06004219 RID: 16921 RVA: 0x00161E00 File Offset: 0x00160000
		public static int GetCurrentUploadTime()
		{
			return (int)(DateTime.UtcNow.TimeOfDay.TotalSeconds / 2.0);
		}

		// Token: 0x04002843 RID: 10307
		private static List<FileInfo> cachedFiles;

		// Token: 0x04002844 RID: 10308
		private static int gameplayIDLength = 20;

		// Token: 0x04002845 RID: 10309
		private static string gameplayIDAvailableChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
	}
}
