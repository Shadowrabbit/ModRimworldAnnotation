using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D03 RID: 3331
	public class JoyGiver_Skygaze : JoyGiver
	{
		// Token: 0x06004C7C RID: 19580 RVA: 0x001AA310 File Offset: 0x001A8510
		public override float GetChance(Pawn pawn)
		{
			float num = pawn.Map.gameConditionManager.AggregateSkyGazeChanceFactor(pawn.Map);
			return base.GetChance(pawn) * num;
		}

		// Token: 0x06004C7D RID: 19581 RVA: 0x001AA340 File Offset: 0x001A8540
		public override Job TryGiveJob(Pawn pawn)
		{
			if (!JoyUtility.EnjoyableOutsideNow(pawn, null) || pawn.Map.weatherManager.curWeather.rainRate > 0.1f)
			{
				return null;
			}
			IntVec3 c;
			if (!RCellFinder.TryFindSkygazeCell(pawn.Position, pawn, out c))
			{
				return null;
			}
			return JobMaker.MakeJob(this.def.jobDef, c);
		}
	}
}
