using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FEB RID: 4075
	public class StorytellerDef : Def
	{
		// Token: 0x060058D7 RID: 22743 RVA: 0x001D0E1C File Offset: 0x001CF01C
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

		// Token: 0x060058D8 RID: 22744 RVA: 0x0003DBA8 File Offset: 0x0003BDA8
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

		// Token: 0x04003B40 RID: 15168
		public int listOrder = 9999;

		// Token: 0x04003B41 RID: 15169
		public bool listVisible = true;

		// Token: 0x04003B42 RID: 15170
		public bool tutorialMode;

		// Token: 0x04003B43 RID: 15171
		public bool disableAdaptiveTraining;

		// Token: 0x04003B44 RID: 15172
		public bool disableAlerts;

		// Token: 0x04003B45 RID: 15173
		public bool disablePermadeath;

		// Token: 0x04003B46 RID: 15174
		public DifficultyDef forcedDifficulty;

		// Token: 0x04003B47 RID: 15175
		[NoTranslate]
		private string portraitLarge;

		// Token: 0x04003B48 RID: 15176
		[NoTranslate]
		private string portraitTiny;

		// Token: 0x04003B49 RID: 15177
		public List<StorytellerCompProperties> comps = new List<StorytellerCompProperties>();

		// Token: 0x04003B4A RID: 15178
		public SimpleCurve populationIntentFactorFromPopCurve;

		// Token: 0x04003B4B RID: 15179
		public SimpleCurve populationIntentFactorFromPopAdaptDaysCurve;

		// Token: 0x04003B4C RID: 15180
		public SimpleCurve pointsFactorFromDaysPassed;

		// Token: 0x04003B4D RID: 15181
		public float adaptDaysMin;

		// Token: 0x04003B4E RID: 15182
		public float adaptDaysMax = 100f;

		// Token: 0x04003B4F RID: 15183
		public float adaptDaysGameStartGraceDays;

		// Token: 0x04003B50 RID: 15184
		public SimpleCurve pointsFactorFromAdaptDays;

		// Token: 0x04003B51 RID: 15185
		public SimpleCurve adaptDaysLossFromColonistLostByPostPopulation;

		// Token: 0x04003B52 RID: 15186
		public SimpleCurve adaptDaysLossFromColonistViolentlyDownedByPopulation;

		// Token: 0x04003B53 RID: 15187
		public SimpleCurve adaptDaysGrowthRateCurve;

		// Token: 0x04003B54 RID: 15188
		[Unsaved(false)]
		public Texture2D portraitLargeTex;

		// Token: 0x04003B55 RID: 15189
		[Unsaved(false)]
		public Texture2D portraitTinyTex;
	}
}
