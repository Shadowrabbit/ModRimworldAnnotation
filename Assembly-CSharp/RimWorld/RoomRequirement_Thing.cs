using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DB3 RID: 7603
	public class RoomRequirement_Thing : RoomRequirement
	{
		// Token: 0x0600A546 RID: 42310 RVA: 0x0006D852 File Offset: 0x0006BA52
		public override bool Met(Room r, Pawn p = null)
		{
			return r.ContainsThing(this.thingDef);
		}

		// Token: 0x0600A547 RID: 42311 RVA: 0x00300410 File Offset: 0x002FE610
		public override bool SameOrSubsetOf(RoomRequirement other)
		{
			if (!base.SameOrSubsetOf(other))
			{
				return false;
			}
			RoomRequirement_Thing roomRequirement_Thing = (RoomRequirement_Thing)other;
			return this.thingDef == roomRequirement_Thing.thingDef;
		}

		// Token: 0x0600A548 RID: 42312 RVA: 0x0006D860 File Offset: 0x0006BA60
		public override string Label(Room r = null)
		{
			return ((!this.labelKey.NullOrEmpty()) ? this.labelKey.Translate() : this.thingDef.label) + ((r != null) ? " 0/1" : "");
		}

		// Token: 0x0600A549 RID: 42313 RVA: 0x0006D8A0 File Offset: 0x0006BAA0
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.thingDef == null)
			{
				yield return "thingDef is null";
			}
			yield break;
		}

		// Token: 0x0600A54A RID: 42314 RVA: 0x0006D8B0 File Offset: 0x0006BAB0
		public override bool PlayerHasResearched()
		{
			return this.thingDef.IsResearchFinished;
		}

		// Token: 0x0400701A RID: 28698
		public ThingDef thingDef;
	}
}
