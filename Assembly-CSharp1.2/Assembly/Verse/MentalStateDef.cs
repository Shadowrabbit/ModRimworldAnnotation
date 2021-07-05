using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000152 RID: 338
	public class MentalStateDef : Def
	{
		// Token: 0x17000193 RID: 403
		// (get) Token: 0x060008A6 RID: 2214 RVA: 0x0000CD6E File Offset: 0x0000AF6E
		public MentalStateWorker Worker
		{
			get
			{
				if (this.workerInt == null && this.workerClass != null)
				{
					this.workerInt = (MentalStateWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x060008A7 RID: 2215 RVA: 0x0000CDAE File Offset: 0x0000AFAE
		public bool IsAggro
		{
			get
			{
				return this.category == MentalStateCategory.Aggro;
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x060008A8 RID: 2216 RVA: 0x00096E94 File Offset: 0x00095094
		public bool IsExtreme
		{
			get
			{
				List<MentalBreakDef> allDefsListForReading = DefDatabase<MentalBreakDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					if (allDefsListForReading[i].intensity == MentalBreakIntensity.Extreme && allDefsListForReading[i].mentalState == this)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x0000CDB9 File Offset: 0x0000AFB9
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.beginLetterDef == null)
			{
				this.beginLetterDef = LetterDefOf.NegativeEvent;
			}
		}

		// Token: 0x040006E9 RID: 1769
		public Type stateClass = typeof(MentalState);

		// Token: 0x040006EA RID: 1770
		public Type workerClass = typeof(MentalStateWorker);

		// Token: 0x040006EB RID: 1771
		public MentalStateCategory category;

		// Token: 0x040006EC RID: 1772
		public bool prisonersCanDo = true;

		// Token: 0x040006ED RID: 1773
		public bool unspawnedCanDo;

		// Token: 0x040006EE RID: 1774
		public bool colonistsOnly;

		// Token: 0x040006EF RID: 1775
		public List<PawnCapacityDef> requiredCapacities = new List<PawnCapacityDef>();

		// Token: 0x040006F0 RID: 1776
		public bool blockNormalThoughts;

		// Token: 0x040006F1 RID: 1777
		public List<InteractionDef> blockInteractionInitiationExcept;

		// Token: 0x040006F2 RID: 1778
		public List<InteractionDef> blockInteractionRecipientExcept;

		// Token: 0x040006F3 RID: 1779
		public bool blockRandomInteraction;

		// Token: 0x040006F4 RID: 1780
		public EffecterDef stateEffecter;

		// Token: 0x040006F5 RID: 1781
		public TaleDef tale;

		// Token: 0x040006F6 RID: 1782
		public bool allowBeatfire;

		// Token: 0x040006F7 RID: 1783
		public DrugCategory drugCategory = DrugCategory.Any;

		// Token: 0x040006F8 RID: 1784
		public bool ignoreDrugPolicy;

		// Token: 0x040006F9 RID: 1785
		public float recoveryMtbDays = 1f;

		// Token: 0x040006FA RID: 1786
		public int minTicksBeforeRecovery = 500;

		// Token: 0x040006FB RID: 1787
		public int maxTicksBeforeRecovery = 99999999;

		// Token: 0x040006FC RID: 1788
		public bool recoverFromSleep;

		// Token: 0x040006FD RID: 1789
		public bool recoverFromDowned = true;

		// Token: 0x040006FE RID: 1790
		public bool recoverFromCollapsingExhausted = true;

		// Token: 0x040006FF RID: 1791
		public ThoughtDef moodRecoveryThought;

		// Token: 0x04000700 RID: 1792
		[MustTranslate]
		public string beginLetter;

		// Token: 0x04000701 RID: 1793
		[MustTranslate]
		public string beginLetterLabel;

		// Token: 0x04000702 RID: 1794
		public LetterDef beginLetterDef;

		// Token: 0x04000703 RID: 1795
		public Color nameColor = Color.green;

		// Token: 0x04000704 RID: 1796
		[MustTranslate]
		public string recoveryMessage;

		// Token: 0x04000705 RID: 1797
		[MustTranslate]
		public string baseInspectLine;

		// Token: 0x04000706 RID: 1798
		public bool escapingPrisonersIgnore;

		// Token: 0x04000707 RID: 1799
		private MentalStateWorker workerInt;
	}
}
