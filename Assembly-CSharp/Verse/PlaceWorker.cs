using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002BF RID: 703
	public abstract class PlaceWorker
	{
		// Token: 0x060011CA RID: 4554 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public virtual bool IsBuildDesignatorVisible(BuildableDef def)
		{
			return true;
		}

		// Token: 0x060011CB RID: 4555 RVA: 0x00012DFE File Offset: 0x00010FFE
		public virtual AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x060011CC RID: 4556 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void PostPlace(Map map, BuildableDef def, IntVec3 loc, Rot4 rot)
		{
		}

		// Token: 0x060011CD RID: 4557 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
		}

		// Token: 0x060011CE RID: 4558 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool ForceAllowPlaceOver(BuildableDef other)
		{
			return false;
		}

		// Token: 0x060011CF RID: 4559 RVA: 0x00012E05 File Offset: 0x00011005
		public virtual IEnumerable<TerrainAffordanceDef> DisplayAffordances()
		{
			yield break;
		}
	}
}
