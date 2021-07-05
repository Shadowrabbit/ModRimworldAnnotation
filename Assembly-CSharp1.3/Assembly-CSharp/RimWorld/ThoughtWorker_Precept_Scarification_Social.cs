using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200093D RID: 2365
	public class ThoughtWorker_Precept_Scarification_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x06003CC8 RID: 15560 RVA: 0x001502E8 File Offset: 0x0014E4E8
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			int num = 0;
			foreach (Precept precept in p.Ideo.PreceptsListForReading)
			{
				num = Mathf.Max(num, precept.def.requiredScars);
			}
			if (num == 0)
			{
				return false;
			}
			if (ThoughtWorker_Precept_Scarification_Social.<ShouldHaveThought>g__CountScars|0_0(p) < num)
			{
				return false;
			}
			int num2 = ThoughtWorker_Precept_Scarification_Social.<ShouldHaveThought>g__CountScars|0_0(otherPawn);
			if (num2 >= num)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			if (num2 == 0)
			{
				return ThoughtState.ActiveAtStage(1);
			}
			if (num2 < num)
			{
				return ThoughtState.ActiveAtStage(2);
			}
			return false;
		}

		// Token: 0x06003CCA RID: 15562 RVA: 0x0015039C File Offset: 0x0014E59C
		[CompilerGenerated]
		internal static int <ShouldHaveThought>g__CountScars|0_0(Pawn pawn)
		{
			int num = 0;
			using (List<Hediff>.Enumerator enumerator = pawn.health.hediffSet.hediffs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.def == HediffDefOf.Scarification)
					{
						num++;
					}
				}
			}
			return num;
		}
	}
}
