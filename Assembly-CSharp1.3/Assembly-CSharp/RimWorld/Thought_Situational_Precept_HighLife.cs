using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200095C RID: 2396
	public class Thought_Situational_Precept_HighLife : Thought_Situational
	{
		// Token: 0x17000ACF RID: 2767
		// (get) Token: 0x06003D18 RID: 15640 RVA: 0x00151244 File Offset: 0x0014F444
		protected override float BaseMoodOffset
		{
			get
			{
				if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
				{
					return 0f;
				}
				float x = (float)(Find.TickManager.TicksGame - this.pawn.mindState.lastTakeRecreationalDrugTick) / 60000f;
				return (float)Mathf.RoundToInt(ThoughtWorker_Precept_HighLife.MoodOffsetFromDaysSinceLastDrugCurve.Evaluate(x));
			}
		}
	}
}
