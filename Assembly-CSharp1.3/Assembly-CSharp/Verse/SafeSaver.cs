using System;
using System.IO;
using System.Threading;

namespace Verse
{
	// Token: 0x0200031E RID: 798
	public static class SafeSaver
	{
		// Token: 0x060016D2 RID: 5842 RVA: 0x000866F8 File Offset: 0x000848F8
		private static string GetFileFullPath(string path)
		{
			return Path.GetFullPath(path);
		}

		// Token: 0x060016D3 RID: 5843 RVA: 0x00086700 File Offset: 0x00084900
		private static string GetNewFileFullPath(string path)
		{
			return Path.GetFullPath(path + SafeSaver.NewFileSuffix);
		}

		// Token: 0x060016D4 RID: 5844 RVA: 0x00086712 File Offset: 0x00084912
		private static string GetOldFileFullPath(string path)
		{
			return Path.GetFullPath(path + SafeSaver.OldFileSuffix);
		}

		// Token: 0x060016D5 RID: 5845 RVA: 0x00086724 File Offset: 0x00084924
		public static void Save(string path, string documentElementName, Action saveAction, bool leaveOldFile = false)
		{
			try
			{
				SafeSaver.CleanSafeSaverFiles(path);
				if (!File.Exists(SafeSaver.GetFileFullPath(path)))
				{
					SafeSaver.DoSave(SafeSaver.GetFileFullPath(path), documentElementName, saveAction);
				}
				else
				{
					SafeSaver.DoSave(SafeSaver.GetNewFileFullPath(path), documentElementName, saveAction);
					try
					{
						SafeSaver.SafeMove(SafeSaver.GetFileFullPath(path), SafeSaver.GetOldFileFullPath(path));
					}
					catch (Exception ex)
					{
						Log.Warning(string.Concat(new object[]
						{
							"Could not move file from \"",
							SafeSaver.GetFileFullPath(path),
							"\" to \"",
							SafeSaver.GetOldFileFullPath(path),
							"\": ",
							ex
						}));
						throw;
					}
					try
					{
						SafeSaver.SafeMove(SafeSaver.GetNewFileFullPath(path), SafeSaver.GetFileFullPath(path));
					}
					catch (Exception ex2)
					{
						Log.Warning(string.Concat(new object[]
						{
							"Could not move file from \"",
							SafeSaver.GetNewFileFullPath(path),
							"\" to \"",
							SafeSaver.GetFileFullPath(path),
							"\": ",
							ex2
						}));
						SafeSaver.RemoveFileIfExists(SafeSaver.GetFileFullPath(path), false);
						SafeSaver.RemoveFileIfExists(SafeSaver.GetNewFileFullPath(path), false);
						try
						{
							SafeSaver.SafeMove(SafeSaver.GetOldFileFullPath(path), SafeSaver.GetFileFullPath(path));
						}
						catch (Exception ex3)
						{
							Log.Warning(string.Concat(new object[]
							{
								"Could not move file from \"",
								SafeSaver.GetOldFileFullPath(path),
								"\" back to \"",
								SafeSaver.GetFileFullPath(path),
								"\": ",
								ex3
							}));
						}
						throw;
					}
					if (!leaveOldFile)
					{
						SafeSaver.RemoveFileIfExists(SafeSaver.GetOldFileFullPath(path), true);
					}
				}
			}
			catch (Exception ex4)
			{
				GenUI.ErrorDialog("ProblemSavingFile".Translate(SafeSaver.GetFileFullPath(path), ex4.ToString()));
				throw;
			}
		}

		// Token: 0x060016D6 RID: 5846 RVA: 0x00086920 File Offset: 0x00084B20
		private static void CleanSafeSaverFiles(string path)
		{
			SafeSaver.RemoveFileIfExists(SafeSaver.GetOldFileFullPath(path), true);
			SafeSaver.RemoveFileIfExists(SafeSaver.GetNewFileFullPath(path), true);
		}

		// Token: 0x060016D7 RID: 5847 RVA: 0x0008693C File Offset: 0x00084B3C
		private static void DoSave(string fullPath, string documentElementName, Action saveAction)
		{
			try
			{
				Scribe.saver.InitSaving(fullPath, documentElementName);
				saveAction();
				Scribe.saver.FinalizeSaving();
			}
			catch (Exception ex)
			{
				Log.Warning(string.Concat(new object[]
				{
					"An exception was thrown during saving to \"",
					fullPath,
					"\": ",
					ex
				}));
				Scribe.saver.ForceStop();
				SafeSaver.RemoveFileIfExists(fullPath, false);
				throw;
			}
		}

		// Token: 0x060016D8 RID: 5848 RVA: 0x000869B4 File Offset: 0x00084BB4
		private static void RemoveFileIfExists(string path, bool rethrow)
		{
			try
			{
				if (File.Exists(path))
				{
					File.Delete(path);
				}
			}
			catch (Exception ex)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Could not remove file \"",
					path,
					"\": ",
					ex
				}));
				if (rethrow)
				{
					throw;
				}
			}
		}

		// Token: 0x060016D9 RID: 5849 RVA: 0x00086A14 File Offset: 0x00084C14
		private static void SafeMove(string from, string to)
		{
			Exception ex = null;
			for (int i = 0; i < 50; i++)
			{
				try
				{
					File.Move(from, to);
					return;
				}
				catch (Exception ex2)
				{
					if (ex == null)
					{
						ex = ex2;
					}
				}
				Thread.Sleep(1);
			}
			throw ex;
		}

		// Token: 0x04000FE0 RID: 4064
		private static readonly string NewFileSuffix = ".new";

		// Token: 0x04000FE1 RID: 4065
		private static readonly string OldFileSuffix = ".old";
	}
}
