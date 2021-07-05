using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001535 RID: 5429
	public class WorkGiverDef : Def
	{
		// Token: 0x170015EE RID: 5614
		// (get) Token: 0x0600811D RID: 33053 RVA: 0x002DAA58 File Offset: 0x002D8C58
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

		// Token: 0x0600811E RID: 33054 RVA: 0x002DAA8A File Offset: 0x002D8C8A
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

		// Token: 0x04005072 RID: 20594
		public Type giverClass;

		// Token: 0x04005073 RID: 20595
		public WorkTypeDef workType;

		// Token: 0x04005074 RID: 20596
		public WorkTags workTags;

		// Token: 0x04005075 RID: 20597
		public int priorityInType;

		// Token: 0x04005076 RID: 20598
		[MustTranslate]
		public string verb;

		// Token: 0x04005077 RID: 20599
		[MustTranslate]
		public string gerund;

		// Token: 0x04005078 RID: 20600
		public bool scanThings = true;

		// Token: 0x04005079 RID: 20601
		public bool scanCells;

		// Token: 0x0400507A RID: 20602
		public bool emergency;

		// Token: 0x0400507B RID: 20603
		public List<PawnCapacityDef> requiredCapacities = new List<PawnCapacityDef>();

		// Token: 0x0400507C RID: 20604
		public bool directOrderable = true;

		// Token: 0x0400507D RID: 20605
		public bool prioritizeSustains;

		// Token: 0x0400507E RID: 20606
		public bool nonColonistsCanDo;

		// Token: 0x0400507F RID: 20607
		public JobTag tagToGive = JobTag.MiscWork;

		// Token: 0x04005080 RID: 20608
		public WorkGiverEquivalenceGroupDef equivalenceGroup;

		// Token: 0x04005081 RID: 20609
		public bool canBeDoneWhileDrafted;

		// Token: 0x04005082 RID: 20610
		public int autoTakeablePriorityDrafted = -1;

		// Token: 0x04005083 RID: 20611
		public ThingDef forceMote;

		// Token: 0x04005084 RID: 20612
		public FleckDef forceFleck;

		// Token: 0x04005085 RID: 20613
		public List<ThingDef> fixedBillGiverDefs;

		// Token: 0x04005086 RID: 20614
		public bool billGiversAllHumanlikes;

		// Token: 0x04005087 RID: 20615
		public bool billGiversAllHumanlikesCorpses;

		// Token: 0x04005088 RID: 20616
		public bool billGiversAllMechanoids;

		// Token: 0x04005089 RID: 20617
		public bool billGiversAllMechanoidsCorpses;

		// Token: 0x0400508A RID: 20618
		public bool billGiversAllAnimals;

		// Token: 0x0400508B RID: 20619
		public bool billGiversAllAnimalsCorpses;

		// Token: 0x0400508C RID: 20620
		public bool tendToHumanlikesOnly;

		// Token: 0x0400508D RID: 20621
		public bool tendToAnimalsOnly;

		// Token: 0x0400508E RID: 20622
		public bool feedHumanlikesOnly;

		// Token: 0x0400508F RID: 20623
		public bool feedAnimalsOnly;

		// Token: 0x04005090 RID: 20624
		public bool doesSmoothing;

		// Token: 0x04005091 RID: 20625
		public ThingDef scannerDef;

		// Token: 0x04005092 RID: 20626
		[Unsaved(false)]
		private WorkGiver workerInt;
	}
}
