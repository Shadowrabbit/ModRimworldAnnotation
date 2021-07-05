using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200099E RID: 2462
	public class ThoughtWorker_Cold : ThoughtWorker
	{
		// Token: 0x06003DC5 RID: 15813 RVA: 0x001534D8 File Offset: 0x001516D8
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			float statValue = p.GetStatValue(StatDefOf.ComfyTemperatureMin, true);
			float ambientTemperature = p.AmbientTemperature;
			float num = statValue - ambientTemperature;
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
