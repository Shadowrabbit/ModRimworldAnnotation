using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EAA RID: 3754
	public class ThoughtWorker_Hot : ThoughtWorker
	{
		// Token: 0x060053A3 RID: 21411 RVA: 0x001C157C File Offset: 0x001BF77C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			float statValue = p.GetStatValue(StatDefOf.ComfyTemperatureMax, true);
			float num = p.AmbientTemperature - statValue;
			if (num <= 0f)
			{
				return ThoughtState.Inactive;
			}
			if (num < 10f)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			if (num < 20f)
			{
				return ThoughtState.ActiveAtStage(1);
			}
			if (num < 30f)
			{
				return ThoughtState.ActiveAtStage(2);
			}
			return ThoughtState.ActiveAtStage(3);
		}
	}
}
