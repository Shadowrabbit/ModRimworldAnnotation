using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000CF9 RID: 3321
	public abstract class JoyGiver
	{
		// Token: 0x06004C55 RID: 19541 RVA: 0x000363C7 File Offset: 0x000345C7
		public virtual float GetChance(Pawn pawn)
		{
			return this.def.baseChance;
		}

		// Token: 0x06004C56 RID: 19542 RVA: 0x001A9920 File Offset: 0x001A7B20
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

		// Token: 0x06004C57 RID: 19543
		public abstract Job TryGiveJob(Pawn pawn);

		// Token: 0x06004C58 RID: 19544 RVA: 0x0000C32E File Offset: 0x0000A52E
		public virtual Job TryGiveJobWhileInBed(Pawn pawn)
		{
			return null;
		}

		// Token: 0x06004C59 RID: 19545 RVA: 0x0000C32E File Offset: 0x0000A52E
		public virtual Job TryGiveJobInGatheringArea(Pawn pawn, IntVec3 gatherSpot)
		{
			return null;
		}

		// Token: 0x06004C5A RID: 19546 RVA: 0x000363D4 File Offset: 0x000345D4
		public virtual bool CanBeGivenTo(Pawn pawn)
		{
			return this.MissingRequiredCapacity(pawn) == null && this.def.joyKind.PawnCanDo(pawn);
		}

		// Token: 0x06004C5B RID: 19547 RVA: 0x001A99C0 File Offset: 0x001A7BC0
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

		// Token: 0x04003244 RID: 12868
		public JoyGiverDef def;
	}
}
