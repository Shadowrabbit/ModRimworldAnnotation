using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200152D RID: 5421
	public class RoomRequirement_Impressiveness : RoomRequirement
	{
		// Token: 0x06008101 RID: 33025 RVA: 0x002DA470 File Offset: 0x002D8670
		public override string Label(Room r = null)
		{
			return ((!this.labelKey.NullOrEmpty()) ? this.labelKey : "RoomRequirementImpressiveness").Translate() + " " + ((r != null) ? (Mathf.Round(r.GetStat(RoomStatDefOf.Impressiveness)) + "/") : "") + this.impressiveness;
		}

		// Token: 0x06008102 RID: 33026 RVA: 0x002DA4E9 File Offset: 0x002D86E9
		public override bool Met(Room r, Pawn p = null)
		{
			return Mathf.Round(r.GetStat(RoomStatDefOf.Impressiveness)) >= (float)this.impressiveness;
		}

		// Token: 0x06008103 RID: 33027 RVA: 0x002DA507 File Offset: 0x002D8707
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.impressiveness <= 0)
			{
				yield return "impressiveness must be larger than 0";
			}
			yield break;
			yield break;
		}

		// Token: 0x06008104 RID: 33028 RVA: 0x002DA517 File Offset: 0x002D8717
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.impressiveness, "impressiveness", 0, false);
		}

		// Token: 0x04005058 RID: 20568
		public int impressiveness;
	}
}
