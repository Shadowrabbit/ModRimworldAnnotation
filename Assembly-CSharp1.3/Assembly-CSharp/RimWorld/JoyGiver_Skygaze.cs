using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007E4 RID: 2020
	public class JoyGiver_Skygaze : JoyGiver
	{
		// Token: 0x0600362C RID: 13868 RVA: 0x00132B44 File Offset: 0x00130D44
		public override float GetChance(Pawn pawn)
		{
			float num = pawn.Map.gameConditionManager.AggregateSkyGazeChanceFactor(pawn.Map);
			return base.GetChance(pawn) * num;
		}

		// Token: 0x0600362D RID: 13869 RVA: 0x00132B74 File Offset: 0x00130D74
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
