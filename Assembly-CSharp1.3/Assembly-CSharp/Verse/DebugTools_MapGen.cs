using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020003BC RID: 956
	public static class DebugTools_MapGen
	{
		// Token: 0x06001D8E RID: 7566 RVA: 0x000B8B28 File Offset: 0x000B6D28
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
