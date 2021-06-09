using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000FF1 RID: 4081
	public class TaleDef : Def
	{
		// Token: 0x060058F4 RID: 22772 RVA: 0x0003DCC8 File Offset: 0x0003BEC8
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

		// Token: 0x060058F5 RID: 22773 RVA: 0x0003DCD8 File Offset: 0x0003BED8
		public static TaleDef Named(string str)
		{
			return DefDatabase<TaleDef>.GetNamed(str, true);
		}

		// Token: 0x04003B6E RID: 15214
		public TaleType type;

		// Token: 0x04003B6F RID: 15215
		public Type taleClass;

		// Token: 0x04003B70 RID: 15216
		public bool usableForArt = true;

		// Token: 0x04003B71 RID: 15217
		public bool colonistOnly = true;

		// Token: 0x04003B72 RID: 15218
		public int maxPerPawn = -1;

		// Token: 0x04003B73 RID: 15219
		public float ignoreChance;

		// Token: 0x04003B74 RID: 15220
		public float expireDays = -1f;

		// Token: 0x04003B75 RID: 15221
		public RulePack rulePack;

		// Token: 0x04003B76 RID: 15222
		[NoTranslate]
		public string firstPawnSymbol;

		// Token: 0x04003B77 RID: 15223
		[NoTranslate]
		public string secondPawnSymbol;

		// Token: 0x04003B78 RID: 15224
		[NoTranslate]
		public string defSymbol;

		// Token: 0x04003B79 RID: 15225
		public Type defType = typeof(ThingDef);

		// Token: 0x04003B7A RID: 15226
		public float baseInterest;

		// Token: 0x04003B7B RID: 15227
		public Color historyGraphColor = Color.white;
	}
}
