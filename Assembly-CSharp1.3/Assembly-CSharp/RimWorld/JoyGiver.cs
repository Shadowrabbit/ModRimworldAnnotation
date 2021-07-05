using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007DF RID: 2015
	public abstract class JoyGiver
	{
		// Token: 0x06003612 RID: 13842 RVA: 0x00132169 File Offset: 0x00130369
		public virtual float GetChance(Pawn pawn)
		{
			return this.def.baseChance;
		}

		// Token: 0x06003613 RID: 13843 RVA: 0x00132178 File Offset: 0x00130378
		protected virtual void GetSearchSet(Pawn pawn, List<Thing> outCandidates)
		{
			outCandidates.Clear();
			if (this.def.thingDefs == null)
			{
				return;
			}
			if (this.def.thingDefs.Count == 1)
			{
				outCandidates.AddRange(pawn.Map.listerThings.ThingsOfDef(this.def.thingDefs[0]));
				return;
			}
			for (int i = 0; i < this.def.thingDefs.Count; i++)
			{
				outCandidates.AddRange(pawn.Map.listerThings.ThingsOfDef(this.def.thingDefs[i]));
			}
		}

		// Token: 0x06003614 RID: 13844
		public abstract Job TryGiveJob(Pawn pawn);

		// Token: 0x06003615 RID: 13845 RVA: 0x00002688 File Offset: 0x00000888
		public virtual Job TryGiveJobWhileInBed(Pawn pawn)
		{
			return null;
		}

		// Token: 0x06003616 RID: 13846 RVA: 0x00002688 File Offset: 0x00000888
		public virtual Job TryGiveJobInGatheringArea(Pawn pawn, IntVec3 gatherSpot)
		{
			return null;
		}

		// Token: 0x06003617 RID: 13847 RVA: 0x00132216 File Offset: 0x00130416
		public virtual bool CanBeGivenTo(Pawn pawn)
		{
			return this.MissingRequiredCapacity(pawn) == null && this.def.joyKind.PawnCanDo(pawn);
		}

		// Token: 0x06003618 RID: 13848 RVA: 0x00132234 File Offset: 0x00130434
		public PawnCapacityDef MissingRequiredCapacity(Pawn pawn)
		{
			for (int i = 0; i < this.def.requiredCapacities.Count; i++)
			{
				if (!pawn.health.capacities.CapableOf(this.def.requiredCapacities[i]))
				{
					return this.def.requiredCapacities[i];
				}
			}
			return null;
		}

		// Token: 0x04001ECF RID: 7887
		public JoyGiverDef def;
	}
}
