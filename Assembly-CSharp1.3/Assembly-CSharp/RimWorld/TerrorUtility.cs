using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001505 RID: 5381
	public static class TerrorUtility
	{
		// Token: 0x0600803E RID: 32830 RVA: 0x002D6C31 File Offset: 0x002D4E31
		public static IEnumerable<Thought_MemoryObservationTerror> GetTerrorThoughts(Pawn pawn)
		{
			List<Thought_Memory> memories = pawn.needs.mood.thoughts.memories.Memories;
			int num;
			for (int i = 0; i < memories.Count; i = num + 1)
			{
				Thought_MemoryObservationTerror thought_MemoryObservationTerror;
				if ((thought_MemoryObservationTerror = (memories[i] as Thought_MemoryObservationTerror)) != null)
				{
					yield return thought_MemoryObservationTerror;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x0600803F RID: 32831 RVA: 0x002D6C41 File Offset: 0x002D4E41
		public static IEnumerable<Thought_MemoryObservationTerror> TakeTopTerrorThoughts(IEnumerable<Thought_MemoryObservationTerror> thoughts)
		{
			return (from t in thoughts
			orderby t.intensity descending
			select t).Take(3);
		}

		// Token: 0x06008040 RID: 32832 RVA: 0x002D6C70 File Offset: 0x002D4E70
		public static float GetTerrorLevel(this Pawn pawn)
		{
			int num = 0;
			if (!pawn.IsSlave)
			{
				return (float)num;
			}
			foreach (Thought_MemoryObservationTerror thought_MemoryObservationTerror in TerrorUtility.GetTerrorThoughts(pawn))
			{
				num += thought_MemoryObservationTerror.intensity;
			}
			return (float)Mathf.Min(num, 100) / 100f;
		}

		// Token: 0x06008041 RID: 32833 RVA: 0x002D6CDC File Offset: 0x002D4EDC
		public static void RemoveAllTerrorThoughts(Pawn pawn)
		{
			foreach (Thought_MemoryObservationTerror th in TerrorUtility.GetTerrorThoughts(pawn))
			{
				pawn.needs.mood.thoughts.memories.RemoveMemory(th);
			}
		}

		// Token: 0x06008042 RID: 32834 RVA: 0x002D6D40 File Offset: 0x002D4F40
		public static bool TryCreateTerrorThought(Thing thing, out Thought_MemoryObservationTerror thought)
		{
			thought = null;
			if (!ModsConfig.IdeologyActive)
			{
				return false;
			}
			float statValue = thing.GetStatValue(StatDefOf.TerrorSource, true);
			if (statValue <= 0f)
			{
				return false;
			}
			thought = (Thought_MemoryObservationTerror)ThoughtMaker.MakeThought(ThoughtDefOf.ObservedTerror);
			thought.Target = thing;
			thought.intensity = (int)statValue;
			return true;
		}

		// Token: 0x04004FDE RID: 20446
		private const int MaxTerror = 100;

		// Token: 0x04004FDF RID: 20447
		private const int TerrorThoughtsToTake = 3;

		// Token: 0x04004FE0 RID: 20448
		public static SimpleCurve SuppressionFallRateOverTerror = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(25f, -15f),
				true
			},
			{
				new CurvePoint(50f, -25f),
				true
			},
			{
				new CurvePoint(100f, -45f),
				true
			}
		};
	}
}
