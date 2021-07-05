using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Verse
{
	// Token: 0x0200039D RID: 925
	public class DebugActionsMods
	{
		// Token: 0x06001B89 RID: 7049 RVA: 0x000A0614 File Offset: 0x0009E814
		[DebugAction("Mods", null, allowedGameStates = AllowedGameStates.Entry)]
		private static void LoadedFilesForMod()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (List<ModContentPack>.Enumerator enumerator = LoadedModManager.RunningModsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DebugActionsMods.<>c__DisplayClass0_0 CS$<>8__locals1 = new DebugActionsMods.<>c__DisplayClass0_0();
					CS$<>8__locals1.mod = enumerator.Current;
					list.Add(new DebugMenuOption(CS$<>8__locals1.mod.Name, DebugMenuOptionMode.Action, delegate()
					{
						ModMetaData metaData = ModLister.GetModWithIdentifier(CS$<>8__locals1.mod.PackageId, false);
						if (metaData.loadFolders != null && metaData.loadFolders.DefinedVersions().Count != 0)
						{
							Find.WindowStack.Add(new Dialog_DebugOptionListLister(from ver in metaData.loadFolders.DefinedVersions()
							select new DebugMenuOption(ver, DebugMenuOptionMode.Action, delegate()
							{
								DebugActionsMods.<>c__DisplayClass0_0 CS$<>8__locals4 = CS$<>8__locals1;
								IEnumerable<LoadFolder> source = metaData.loadFolders.FoldersForVersion(ver);
								Func<LoadFolder, string> selector;
								if ((selector = CS$<>8__locals1.<>9__13) == null)
								{
									selector = (CS$<>8__locals1.<>9__13 = ((LoadFolder f) => Path.Combine(CS$<>8__locals1.mod.RootDir, f.folderName)));
								}
								CS$<>8__locals4.<LoadedFilesForMod>g__ShowTable|1(source.Select(selector).Reverse<string>().ToList<string>());
							})));
							return;
						}
						base.<LoadedFilesForMod>g__ShowTable|1(null);
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}
	}
}
