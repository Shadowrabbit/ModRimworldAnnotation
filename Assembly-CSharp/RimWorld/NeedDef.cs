using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FBC RID: 4028
	public class NeedDef : Def
	{
		// Token: 0x06005808 RID: 22536 RVA: 0x0003D1EF File Offset: 0x0003B3EF
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

		// Token: 0x04003A02 RID: 14850
		public Type needClass;

		// Token: 0x04003A03 RID: 14851
		public Intelligence minIntelligence;

		// Token: 0x04003A04 RID: 14852
		public bool colonistAndPrisonersOnly;

		// Token: 0x04003A05 RID: 14853
		public bool colonistsOnly;

		// Token: 0x04003A06 RID: 14854
		public bool onlyIfCausedByHediff;

		// Token: 0x04003A07 RID: 14855
		public bool neverOnPrisoner;

		// Token: 0x04003A08 RID: 14856
		public List<RoyalTitleDef> titleRequiredAny;

		// Token: 0x04003A09 RID: 14857
		public List<HediffDef> hediffRequiredAny;

		// Token: 0x04003A0A RID: 14858
		public bool showOnNeedList = true;

		// Token: 0x04003A0B RID: 14859
		public float baseLevel = 0.5f;

		// Token: 0x04003A0C RID: 14860
		public bool major;

		// Token: 0x04003A0D RID: 14861
		public int listPriority;

		// Token: 0x04003A0E RID: 14862
		[NoTranslate]
		public string tutorHighlightTag;

		// Token: 0x04003A0F RID: 14863
		public bool showForCaravanMembers;

		// Token: 0x04003A10 RID: 14864
		public bool scaleBar;

		// Token: 0x04003A11 RID: 14865
		public float fallPerDay = 0.5f;

		// Token: 0x04003A12 RID: 14866
		public float seekerRisePerHour;

		// Token: 0x04003A13 RID: 14867
		public float seekerFallPerHour;

		// Token: 0x04003A14 RID: 14868
		public bool freezeWhileSleeping;

		// Token: 0x04003A15 RID: 14869
		public bool freezeInMentalState;
	}
}
