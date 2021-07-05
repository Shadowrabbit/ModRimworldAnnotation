using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001DC3 RID: 7619
	public class WorkGiverDef : Def
	{
		// Token: 0x1700195C RID: 6492
		// (get) Token: 0x0600A5A3 RID: 42403 RVA: 0x0006DB30 File Offset: 0x0006BD30
		public WorkGiver Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (WorkGiver)Activator.CreateInstance(this.giverClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x0600A5A4 RID: 42404 RVA: 0x0006DB62 File Offset: 0x0006BD62
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.verb.NullOrEmpty())
			{
				yield return this.defName + " lacks a verb.";
			}
			if (this.gerund.NullOrEmpty())
			{
				yield return this.defName + " lacks a gerund.";
			}
			yield break;
			yield break;
		}

		// Token: 0x04007053 RID: 28755
		public Type giverClass;

		// Token: 0x04007054 RID: 28756
		public WorkTypeDef workType;

		// Token: 0x04007055 RID: 28757
		public WorkTags workTags;

		// Token: 0x04007056 RID: 28758
		public int priorityInType;

		// Token: 0x04007057 RID: 28759
		[MustTranslate]
		public string verb;

		// Token: 0x04007058 RID: 28760
		[MustTranslate]
		public string gerund;

		// Token: 0x04007059 RID: 28761
		public bool scanThings = true;

		// Token: 0x0400705A RID: 28762
		public bool scanCells;

		// Token: 0x0400705B RID: 28763
		public bool emergency;

		// Token: 0x0400705C RID: 28764
		public List<PawnCapacityDef> requiredCapacities = new List<PawnCapacityDef>();

		// Token: 0x0400705D RID: 28765
		public bool directOrderable = true;

		// Token: 0x0400705E RID: 28766
		public bool prioritizeSustains;

		// Token: 0x0400705F RID: 28767
		public bool nonColonistsCanDo;

		// Token: 0x04007060 RID: 28768
		public JobTag tagToGive = JobTag.MiscWork;

		// Token: 0x04007061 RID: 28769
		public WorkGiverEquivalenceGroupDef equivalenceGroup;

		// Token: 0x04007062 RID: 28770
		public bool canBeDoneWhileDrafted;

		// Token: 0x04007063 RID: 28771
		public int autoTakeablePriorityDrafted = -1;

		// Token: 0x04007064 RID: 28772
		public ThingDef forceMote;

		// Token: 0x04007065 RID: 28773
		public List<ThingDef> fixedBillGiverDefs;

		// Token: 0x04007066 RID: 28774
		public bool billGiversAllHumanlikes;

		// Token: 0x04007067 RID: 28775
		public bool billGiversAllHumanlikesCorpses;

		// Token: 0x04007068 RID: 28776
		public bool billGiversAllMechanoids;

		// Token: 0x04007069 RID: 28777
		public bool billGiversAllMechanoidsCorpses;

		// Token: 0x0400706A RID: 28778
		public bool billGiversAllAnimals;

		// Token: 0x0400706B RID: 28779
		public bool billGiversAllAnimalsCorpses;

		// Token: 0x0400706C RID: 28780
		public bool tendToHumanlikesOnly;

		// Token: 0x0400706D RID: 28781
		public bool tendToAnimalsOnly;

		// Token: 0x0400706E RID: 28782
		public bool feedHumanlikesOnly;

		// Token: 0x0400706F RID: 28783
		public bool feedAnimalsOnly;

		// Token: 0x04007070 RID: 28784
		public ThingDef scannerDef;

		// Token: 0x04007071 RID: 28785
		[Unsaved(false)]
		private WorkGiver workerInt;
	}
}
