using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A9A RID: 2714
	public class NeedDef : Def
	{
		// Token: 0x0600408E RID: 16526 RVA: 0x0015D362 File Offset: 0x0015B562
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.description.NullOrEmpty() && this.showOnNeedList)
			{
				yield return "no description";
			}
			if (this.needClass == null)
			{
				yield return "needClass is null";
			}
			if (this.needClass == typeof(Need_Seeker) && (this.seekerRisePerHour == 0f || this.seekerFallPerHour == 0f))
			{
				yield return "seeker rise/fall rates not set";
			}
			yield break;
			yield break;
		}

		// Token: 0x0400255C RID: 9564
		public Type needClass;

		// Token: 0x0400255D RID: 9565
		public Intelligence minIntelligence;

		// Token: 0x0400255E RID: 9566
		public bool colonistAndPrisonersOnly;

		// Token: 0x0400255F RID: 9567
		public bool colonistsOnly;

		// Token: 0x04002560 RID: 9568
		public bool slavesOnly;

		// Token: 0x04002561 RID: 9569
		public bool onlyIfCausedByHediff;

		// Token: 0x04002562 RID: 9570
		public bool onlyIfCausedByTrait;

		// Token: 0x04002563 RID: 9571
		public bool neverOnPrisoner;

		// Token: 0x04002564 RID: 9572
		public bool neverOnSlave;

		// Token: 0x04002565 RID: 9573
		public List<RoyalTitleDef> titleRequiredAny;

		// Token: 0x04002566 RID: 9574
		public List<HediffDef> hediffRequiredAny;

		// Token: 0x04002567 RID: 9575
		public List<PreceptDef> nullifyingPrecepts;

		// Token: 0x04002568 RID: 9576
		public bool showOnNeedList = true;

		// Token: 0x04002569 RID: 9577
		public float baseLevel = 0.5f;

		// Token: 0x0400256A RID: 9578
		public bool major;

		// Token: 0x0400256B RID: 9579
		public int listPriority;

		// Token: 0x0400256C RID: 9580
		[NoTranslate]
		public string tutorHighlightTag;

		// Token: 0x0400256D RID: 9581
		public bool showForCaravanMembers;

		// Token: 0x0400256E RID: 9582
		public bool scaleBar;

		// Token: 0x0400256F RID: 9583
		public float fallPerDay = 0.5f;

		// Token: 0x04002570 RID: 9584
		public float seekerRisePerHour;

		// Token: 0x04002571 RID: 9585
		public float seekerFallPerHour;

		// Token: 0x04002572 RID: 9586
		public bool freezeWhileSleeping;

		// Token: 0x04002573 RID: 9587
		public bool freezeInMentalState;
	}
}
