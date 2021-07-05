using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200152A RID: 5418
	public class RoomRequirement_Thing : RoomRequirement
	{
		// Token: 0x060080EB RID: 33003 RVA: 0x002DA12E File Offset: 0x002D832E
		public override bool Met(Room r, Pawn p = null)
		{
			return r.ContainsThing(this.thingDef);
		}

		// Token: 0x060080EC RID: 33004 RVA: 0x002DA13C File Offset: 0x002D833C
		public override bool SameOrSubsetOf(RoomRequirement other)
		{
			if (!base.SameOrSubsetOf(other))
			{
				return false;
			}
			RoomRequirement_Thing roomRequirement_Thing = (RoomRequirement_Thing)other;
			return this.thingDef == roomRequirement_Thing.thingDef;
		}

		// Token: 0x060080ED RID: 33005 RVA: 0x002DA169 File Offset: 0x002D8369
		public override string Label(Room r = null)
		{
			return ((!this.labelKey.NullOrEmpty()) ? this.labelKey.Translate() : this.thingDef.label) + ((r != null) ? " 0/1" : "");
		}

		// Token: 0x060080EE RID: 33006 RVA: 0x002DA1A9 File Offset: 0x002D83A9
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.thingDef == null)
			{
				yield return "thingDef is null";
			}
			yield break;
		}

		// Token: 0x060080EF RID: 33007 RVA: 0x002DA1B9 File Offset: 0x002D83B9
		public override bool PlayerCanBuildNow()
		{
			return this.thingDef.IsResearchFinished;
		}

		// Token: 0x060080F0 RID: 33008 RVA: 0x002DA1C6 File Offset: 0x002D83C6
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
		}

		// Token: 0x04005055 RID: 20565
		public ThingDef thingDef;
	}
}
