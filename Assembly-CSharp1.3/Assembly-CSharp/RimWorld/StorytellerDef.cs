using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AC7 RID: 2759
	public class StorytellerDef : Def
	{
		// Token: 0x0600413B RID: 16699 RVA: 0x0015F0DC File Offset: 0x0015D2DC
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				if (!this.portraitTiny.NullOrEmpty())
				{
					this.portraitTinyTex = ContentFinder<Texture2D>.Get(this.portraitTiny, true);
					this.portraitLargeTex = ContentFinder<Texture2D>.Get(this.portraitLarge, true);
				}
			});
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].ResolveReferences(this);
			}
		}

		// Token: 0x0600413C RID: 16700 RVA: 0x0015F128 File Offset: 0x0015D328
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.pointsFactorFromAdaptDays == null)
			{
				yield return "pointsFactorFromAdaptDays is null";
			}
			if (this.adaptDaysLossFromColonistLostByPostPopulation == null)
			{
				yield return "adaptDaysLossFromColonistLostByPostPopulation is null";
			}
			if (this.adaptDaysLossFromColonistViolentlyDownedByPopulation == null)
			{
				yield return "adaptDaysLossFromColonistViolentlyDownedByPopulation is null";
			}
			if (this.adaptDaysGrowthRateCurve == null)
			{
				yield return "adaptDaysGrowthRateCurve is null";
			}
			if (this.pointsFactorFromDaysPassed == null)
			{
				yield return "pointsFactorFromDaysPassed is null";
			}
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			int num;
			for (int i = 0; i < this.comps.Count; i = num + 1)
			{
				foreach (string text2 in this.comps[i].ConfigErrors(this))
				{
					yield return text2;
				}
				enumerator = null;
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x040026F0 RID: 9968
		public int listOrder = 9999;

		// Token: 0x040026F1 RID: 9969
		public bool listVisible = true;

		// Token: 0x040026F2 RID: 9970
		public bool tutorialMode;

		// Token: 0x040026F3 RID: 9971
		public bool disableAdaptiveTraining;

		// Token: 0x040026F4 RID: 9972
		public bool disableAlerts;

		// Token: 0x040026F5 RID: 9973
		public bool disablePermadeath;

		// Token: 0x040026F6 RID: 9974
		public DifficultyDef forcedDifficulty;

		// Token: 0x040026F7 RID: 9975
		[NoTranslate]
		private string portraitLarge;

		// Token: 0x040026F8 RID: 9976
		[NoTranslate]
		private string portraitTiny;

		// Token: 0x040026F9 RID: 9977
		public List<StorytellerCompProperties> comps = new List<StorytellerCompProperties>();

		// Token: 0x040026FA RID: 9978
		public SimpleCurve populationIntentFactorFromPopCurve;

		// Token: 0x040026FB RID: 9979
		public SimpleCurve populationIntentFactorFromPopAdaptDaysCurve;

		// Token: 0x040026FC RID: 9980
		public SimpleCurve pointsFactorFromDaysPassed;

		// Token: 0x040026FD RID: 9981
		public float adaptDaysMin;

		// Token: 0x040026FE RID: 9982
		public float adaptDaysMax = 100f;

		// Token: 0x040026FF RID: 9983
		public float adaptDaysGameStartGraceDays;

		// Token: 0x04002700 RID: 9984
		public SimpleCurve pointsFactorFromAdaptDays;

		// Token: 0x04002701 RID: 9985
		public SimpleCurve adaptDaysLossFromColonistLostByPostPopulation;

		// Token: 0x04002702 RID: 9986
		public SimpleCurve adaptDaysLossFromColonistViolentlyDownedByPopulation;

		// Token: 0x04002703 RID: 9987
		public SimpleCurve adaptDaysGrowthRateCurve;

		// Token: 0x04002704 RID: 9988
		[Unsaved(false)]
		public Texture2D portraitLargeTex;

		// Token: 0x04002705 RID: 9989
		[Unsaved(false)]
		public Texture2D portraitTinyTex;
	}
}
