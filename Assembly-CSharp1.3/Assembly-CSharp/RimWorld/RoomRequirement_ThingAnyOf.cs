using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200152C RID: 5420
	public class RoomRequirement_ThingAnyOf : RoomRequirement
	{
		// Token: 0x060080F9 RID: 33017 RVA: 0x002DA2E4 File Offset: 0x002D84E4
		public override string Label(Room r = null)
		{
			return ((!this.labelKey.NullOrEmpty()) ? this.labelKey.Translate() : this.things[0].label) + ((r != null) ? " 0/1" : "");
		}

		// Token: 0x060080FA RID: 33018 RVA: 0x002DA338 File Offset: 0x002D8538
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

		// Token: 0x060080FB RID: 33019 RVA: 0x002DA394 File Offset: 0x002D8594
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

		// Token: 0x060080FC RID: 33020 RVA: 0x002DA408 File Offset: 0x002D8608
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

		// Token: 0x060080FD RID: 33021 RVA: 0x002DA418 File Offset: 0x002D8618
		public override bool PlayerCanBuildNow()
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

		// Token: 0x060080FE RID: 33022 RVA: 0x002DA451 File Offset: 0x002D8651
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<ThingDef>(ref this.things, "things", LookMode.Def, Array.Empty<object>());
		}

		// Token: 0x04005057 RID: 20567
		public List<ThingDef> things;
	}
}
