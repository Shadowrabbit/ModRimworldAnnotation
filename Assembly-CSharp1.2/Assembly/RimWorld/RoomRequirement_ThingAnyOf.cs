using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DB7 RID: 7607
	public class RoomRequirement_ThingAnyOf : RoomRequirement
	{
		// Token: 0x0600A563 RID: 42339 RVA: 0x003006D4 File Offset: 0x002FE8D4
		public override string Label(Room r = null)
		{
			return ((!this.labelKey.NullOrEmpty()) ? this.labelKey.Translate() : this.things[0].label) + ((r != null) ? " 0/1" : "");
		}

		// Token: 0x0600A564 RID: 42340 RVA: 0x00300728 File Offset: 0x002FE928
		public override bool Met(Room r, Pawn p = null)
		{
			foreach (ThingDef def in this.things)
			{
				if (r.ContainsThing(def))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600A565 RID: 42341 RVA: 0x00300784 File Offset: 0x002FE984
		public override bool SameOrSubsetOf(RoomRequirement other)
		{
			if (!base.SameOrSubsetOf(other))
			{
				return false;
			}
			RoomRequirement_ThingAnyOf roomRequirement_ThingAnyOf = (RoomRequirement_ThingAnyOf)other;
			foreach (ThingDef item in this.things)
			{
				if (!roomRequirement_ThingAnyOf.things.Contains(item))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600A566 RID: 42342 RVA: 0x0006D96F File Offset: 0x0006BB6F
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.things.NullOrEmpty<ThingDef>())
			{
				yield return "things are null or empty";
			}
			yield break;
			yield break;
		}

		// Token: 0x0600A567 RID: 42343 RVA: 0x003007F8 File Offset: 0x002FE9F8
		public override bool PlayerHasResearched()
		{
			for (int i = 0; i < this.things.Count; i++)
			{
				if (this.things[i].IsResearchFinished)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04007025 RID: 28709
		public List<ThingDef> things;
	}
}
