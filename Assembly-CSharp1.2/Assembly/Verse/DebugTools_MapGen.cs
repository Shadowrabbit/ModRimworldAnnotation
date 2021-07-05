using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020006BD RID: 1725
	public static class DebugTools_MapGen
	{
		// Token: 0x06002C99 RID: 11417 RVA: 0x0012FB1C File Offset: 0x0012DD1C
		public static List<DebugMenuOption> Options_Scatterers()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Type localSt2 in typeof(GenStep_Scatterer).AllLeafSubclasses())
			{
				Type localSt = localSt2;
				list.Add(new DebugMenuOption(localSt.ToString(), DebugMenuOptionMode.Tool, delegate()
				{
					((GenStep_Scatterer)Activator.CreateInstance(localSt)).ForceScatterAt(UI.MouseCell(), Find.CurrentMap);
				}));
			}
			return list;
		}
	}
}
