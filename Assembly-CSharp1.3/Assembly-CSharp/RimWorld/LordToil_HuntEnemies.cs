using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008B6 RID: 2230
	public class LordToil_HuntEnemies : LordToil
	{
		// Token: 0x17000A8F RID: 2703
		// (get) Token: 0x06003AE4 RID: 15076 RVA: 0x001493B4 File Offset: 0x001475B4
		private LordToilData_HuntEnemies Data
		{
			get
			{
				return (LordToilData_HuntEnemies)this.data;
			}
		}

		// Token: 0x17000A90 RID: 2704
		// (get) Token: 0x06003AE5 RID: 15077 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool ForceHighStoryDanger
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003AE6 RID: 15078 RVA: 0x001493C1 File Offset: 0x001475C1
		public LordToil_HuntEnemies(IntVec3 fallbackLocation)
		{
			this.data = new LordToilData_HuntEnemies();
			this.Data.fallbackLocation = fallbackLocation;
		}

		// Token: 0x06003AE7 RID: 15079 RVA: 0x001493E0 File Offset: 0x001475E0
		public override void UpdateAllDuties()
		{
			LordToilData_HuntEnemies data = this.Data;
			if (!data.fallbackLocation.IsValid)
			{
				for (int i = 0; i < this.lord.ownedPawns.Count; i++)
				{
					Pawn pawn = this.lord.ownedPawns[i];
					if (pawn.Spawned && RCellFinder.TryFindRandomSpotJustOutsideColony(pawn, out data.fallbackLocation) && data.fallbackLocation.IsValid)
					{
						break;
					}
				}
			}
			for (int j = 0; j < this.lord.ownedPawns.Count; j++)
			{
				Pawn pawn2 = this.lord.ownedPawns[j];
				pawn2.mindState.duty = new PawnDuty(DutyDefOf.HuntEnemiesIndividual);
				pawn2.mindState.duty.focusSecond = data.fallbackLocation;
			}
		}
	}
}
