using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020019AD RID: 6573
	public class Designator_RemoveBridge : Designator_RemoveFloor
	{
		// Token: 0x06009151 RID: 37201 RVA: 0x0029C740 File Offset: 0x0029A940
		public Designator_RemoveBridge()
		{
			this.defaultLabel = "DesignatorRemoveBridge".Translate();
			this.defaultDesc = "DesignatorRemoveBridgeDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/RemoveBridge", true);
			this.hotKey = KeyBindingDefOf.Misc5;
		}

		// Token: 0x06009152 RID: 37202 RVA: 0x0006168D File Offset: 0x0005F88D
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (c.InBounds(base.Map) && c.GetTerrain(base.Map) != TerrainDefOf.Bridge)
			{
				return false;
			}
			return base.CanDesignateCell(c);
		}
	}
}
