using System;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x020005BA RID: 1466
	public static class DebugToolsMisc
	{
		// Token: 0x060024C2 RID: 9410 RVA: 0x0011384C File Offset: 0x00111A4C
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
