using System;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x020003A4 RID: 932
	public static class DebugToolsMisc
	{
		// Token: 0x06001BE7 RID: 7143 RVA: 0x000A389C File Offset: 0x000A1A9C
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void AttachFire()
		{
			foreach (Thing t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				t.TryAttachFire(1f);
			}
		}
	}
}
