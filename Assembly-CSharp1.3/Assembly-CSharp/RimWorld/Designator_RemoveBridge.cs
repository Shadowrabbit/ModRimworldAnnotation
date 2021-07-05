using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012B8 RID: 4792
	public class Designator_RemoveBridge : Designator_RemoveFloor
	{
		// Token: 0x0600727B RID: 29307 RVA: 0x00263794 File Offset: 0x00261994
		public Designator_RemoveBridge()
		{
			this.defaultLabel = "DesignatorRemoveBridge".Translate();
			this.defaultDesc = "DesignatorRemoveBridgeDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/RemoveBridge", true);
			this.hotKey = KeyBindingDefOf.Misc5;
		}

		// Token: 0x0600727C RID: 29308 RVA: 0x002637ED File Offset: 0x002619ED
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (c.InBounds(base.Map) && !c.GetTerrain(base.Map).bridge)
			{
				return false;
			}
			return base.CanDesignateCell(c);
		}
	}
}
