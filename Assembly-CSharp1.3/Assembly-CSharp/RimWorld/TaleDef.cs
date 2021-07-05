using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000AD5 RID: 2773
	public class TaleDef : Def
	{
		// Token: 0x06004158 RID: 16728 RVA: 0x0015F410 File Offset: 0x0015D610
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.taleClass == null)
			{
				yield return this.defName + " taleClass is null.";
			}
			if (this.expireDays < 0f)
			{
				if (this.type == TaleType.Expirable)
				{
					yield return "Expirable tale type is used but expireDays<0";
				}
			}
			else if (this.type != TaleType.Expirable)
			{
				yield return "Non expirable tale type is used but expireDays>=0";
			}
			if (this.baseInterest > 1E-06f && !this.usableForArt)
			{
				yield return "Non-zero baseInterest but not usable for art";
			}
			if (this.firstPawnSymbol == "pawn" || this.secondPawnSymbol == "pawn")
			{
				yield return "pawn symbols should not be 'pawn', this is the default and only choice for SinglePawn tales so using it here is confusing.";
			}
			yield break;
			yield break;
		}

		// Token: 0x06004159 RID: 16729 RVA: 0x0015F420 File Offset: 0x0015D620
		public static TaleDef Named(string str)
		{
			return DefDatabase<TaleDef>.GetNamed(str, true);
		}

		// Token: 0x04002733 RID: 10035
		public TaleType type;

		// Token: 0x04002734 RID: 10036
		public Type taleClass;

		// Token: 0x04002735 RID: 10037
		public bool usableForArt = true;

		// Token: 0x04002736 RID: 10038
		public bool colonistOnly = true;

		// Token: 0x04002737 RID: 10039
		public int maxPerPawn = -1;

		// Token: 0x04002738 RID: 10040
		public float ignoreChance;

		// Token: 0x04002739 RID: 10041
		public float expireDays = -1f;

		// Token: 0x0400273A RID: 10042
		public RulePack rulePack;

		// Token: 0x0400273B RID: 10043
		[NoTranslate]
		public string firstPawnSymbol;

		// Token: 0x0400273C RID: 10044
		[NoTranslate]
		public string secondPawnSymbol;

		// Token: 0x0400273D RID: 10045
		[NoTranslate]
		public string defSymbol;

		// Token: 0x0400273E RID: 10046
		public Type defType = typeof(ThingDef);

		// Token: 0x0400273F RID: 10047
		public float baseInterest;

		// Token: 0x04002740 RID: 10048
		public Color historyGraphColor = Color.white;
	}
}
