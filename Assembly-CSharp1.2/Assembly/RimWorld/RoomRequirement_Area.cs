using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DAF RID: 7599
	public class RoomRequirement_Area : RoomRequirement
	{
		// Token: 0x0600A52A RID: 42282 RVA: 0x002FFF98 File Offset: 0x002FE198
		public override string Label(Room r = null)
		{
			return ((!this.labelKey.NullOrEmpty()) ? this.labelKey : "RoomRequirementArea").Translate(((r != null) ? (r.CellCount + "/") : "") + this.area);
		}

		// Token: 0x0600A52B RID: 42283 RVA: 0x0006D783 File Offset: 0x0006B983
		public override bool Met(Room r, Pawn p = null)
		{
			return r.CellCount >= this.area;
		}

		// Token: 0x0600A52C RID: 42284 RVA: 0x0006D796 File Offset: 0x0006B996
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

		// Token: 0x0400700E RID: 28686
		public int area;
	}
}
