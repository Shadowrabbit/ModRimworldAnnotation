using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001F2 RID: 498
	public abstract class PlaceWorker
	{
		// Token: 0x06000DF9 RID: 3577 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool IsBuildDesignatorVisible(BuildableDef def)
		{
			return true;
		}

		// Token: 0x06000DFA RID: 3578 RVA: 0x0004EE58 File Offset: 0x0004D058
		public virtual AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x06000DFB RID: 3579 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PostPlace(Map map, BuildableDef def, IntVec3 loc, Rot4 rot)
		{
		}

		// Token: 0x06000DFC RID: 3580 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool ForceAllowPlaceOver(BuildableDef other)
		{
			return false;
		}

		// Token: 0x06000DFE RID: 3582 RVA: 0x0004EE5F File Offset: 0x0004D05F
		public virtual IEnumerable<TerrainAffordanceDef> DisplayAffordances()
		{
			yield break;
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void DrawMouseAttachments(BuildableDef def)
		{
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void DrawPlaceMouseAttachments(float curX, ref float curY, BuildableDef def, IntVec3 center, Rot4 rot)
		{
		}
	}
}
