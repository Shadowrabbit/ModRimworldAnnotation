using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000960 RID: 2400
	public class ThoughtWorker_Precept_NoRecentSlaughter : ThoughtWorker_Precept, IPreceptCompDescriptionArgs
	{
		// Token: 0x06003D25 RID: 15653 RVA: 0x001514C0 File Offset: 0x0014F6C0
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (p.IsPrisoner || p.IsSlave)
			{
				return false;
			}
			if (p.MapHeld != null && this.def.minExpectationForNegativeThought != null && ExpectationsUtility.CurrentExpectationFor(p.MapHeld).order < this.def.minExpectationForNegativeThought.order)
			{
				return false;
			}
			if (p.Faction == null || p.Faction.ideos == null)
			{
				return false;
			}
			int num = Mathf.Max(0, p.Faction.ideos.LastAnimalSlaughterTick);
			return Find.TickManager.TicksGame - num > 600000;
		}

		// Token: 0x06003D26 RID: 15654 RVA: 0x0015156D File Offset: 0x0014F76D
		public IEnumerable<NamedArgument> GetDescriptionArgs()
		{
			yield return 10.Named("ANIMALSLAUGHTERREQUIREDINTERVAL");
			yield break;
		}

		// Token: 0x040020CD RID: 8397
		public const int MinDaysSinceLastAnimalSlaughterForThought = 10;
	}
}
