using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001528 RID: 5416
	public class RoomRequirement_Area : RoomRequirement
	{
		// Token: 0x060080DF RID: 32991 RVA: 0x002D9F1C File Offset: 0x002D811C
		public override string Label(Room r = null)
		{
			return ((!this.labelKey.NullOrEmpty()) ? this.labelKey : "RoomRequirementArea").Translate(((r != null) ? (r.CellCount + "/") : "") + this.area);
		}

		// Token: 0x060080E0 RID: 32992 RVA: 0x002D9F81 File Offset: 0x002D8181
		public override bool Met(Room r, Pawn p = null)
		{
			return r.CellCount >= this.area;
		}

		// Token: 0x060080E1 RID: 32993 RVA: 0x002D9F94 File Offset: 0x002D8194
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.area <= 0)
			{
				yield return "area must be larger than 0";
			}
			yield break;
			yield break;
		}

		// Token: 0x060080E2 RID: 32994 RVA: 0x002D9FA4 File Offset: 0x002D81A4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.area, "area", 0, false);
		}

		// Token: 0x04005053 RID: 20563
		public int area;
	}
}
