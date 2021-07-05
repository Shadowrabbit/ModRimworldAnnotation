using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200099F RID: 2463
	public class ThoughtWorker_Hot : ThoughtWorker
	{
		// Token: 0x06003DC7 RID: 15815 RVA: 0x0015353C File Offset: 0x0015173C
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
