using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200095E RID: 2398
	public class ThoughtWorker_Precept_NoRecentExecution : ThoughtWorker_Precept, IPreceptCompDescriptionArgs
	{
		// Token: 0x06003D1F RID: 15647 RVA: 0x00151428 File Offset: 0x0014F628
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!p.IsColonist || p.IsSlave)
			{
				return false;
			}
			int num = Mathf.Max(0, p.Faction.lastExecutionTick);
			return Find.TickManager.TicksGame - num > 1800000;
		}

		// Token: 0x06003D20 RID: 15648 RVA: 0x00151476 File Offset: 0x0014F676
		public IEnumerable<NamedArgument> GetDescriptionArgs()
		{
			yield return 30.Named("MINDAYSLASTEXECUTION");
			yield break;
		}

		// Token: 0x040020CB RID: 8395
		private const int MinDaysSinceLastExecutionForThought = 30;
	}
}
