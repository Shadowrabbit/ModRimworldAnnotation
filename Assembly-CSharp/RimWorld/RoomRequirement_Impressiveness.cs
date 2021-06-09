using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DB9 RID: 7609
	public class RoomRequirement_Impressiveness : RoomRequirement
	{
		// Token: 0x0600A573 RID: 42355 RVA: 0x0030098C File Offset: 0x002FEB8C
		public override string Label(Room r = null)
		{
			return ((!this.labelKey.NullOrEmpty()) ? this.labelKey : "RoomRequirementImpressiveness").Translate() + " " + ((r != null) ? (Mathf.Round(r.GetStat(RoomStatDefOf.Impressiveness)) + "/") : "") + this.impressiveness;
		}

		// Token: 0x0600A574 RID: 42356 RVA: 0x0006D9C5 File Offset: 0x0006BBC5
		public override bool Met(Room r, Pawn p = null)
		{
			return Mathf.Round(r.GetStat(RoomStatDefOf.Impressiveness)) >= (float)this.impressiveness;
		}

		// Token: 0x0600A575 RID: 42357 RVA: 0x0006D9E3 File Offset: 0x0006BBE3
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

		// Token: 0x0400702B RID: 28715
		public int impressiveness;
	}
}
