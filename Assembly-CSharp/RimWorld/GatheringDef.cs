using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000F95 RID: 3989
	public class GatheringDef : Def
	{
		// Token: 0x17000D7A RID: 3450
		// (get) Token: 0x0600577B RID: 22395 RVA: 0x0003CA98 File Offset: 0x0003AC98
		public bool IsRandomSelectable
		{
			get
			{
				return this.randomSelectionWeight > 0f;
			}
		}

		// Token: 0x17000D7B RID: 3451
		// (get) Token: 0x0600577C RID: 22396 RVA: 0x0003CAA7 File Offset: 0x0003ACA7
		public GatheringWorker Worker
		{
			get
			{
				if (this.worker == null)
				{
					this.worker = (GatheringWorker)Activator.CreateInstance(this.workerClass);
					this.worker.def = this;
				}
				return this.worker;
			}
		}

		// Token: 0x0600577D RID: 22397 RVA: 0x0003CAD9 File Offset: 0x0003ACD9
		public bool CanExecute(Map map, Pawn organizer = null, bool ignoreGameConditions = false)
		{
			return (ignoreGameConditions || GatheringsUtility.AcceptableGameConditionsToStartGathering(map, this)) && this.Worker.CanExecute(map, organizer);
		}

		// Token: 0x04003935 RID: 14645
		public Type workerClass = typeof(GatheringWorker);

		// Token: 0x04003936 RID: 14646
		public DutyDef duty;

		// Token: 0x04003937 RID: 14647
		public float randomSelectionWeight;

		// Token: 0x04003938 RID: 14648
		public bool respectTimetable = true;

		// Token: 0x04003939 RID: 14649
		public List<ThingDef> gatherSpotDefs;

		// Token: 0x0400393A RID: 14650
		[MustTranslate]
		public string letterTitle;

		// Token: 0x0400393B RID: 14651
		[MustTranslate]
		public string letterText;

		// Token: 0x0400393C RID: 14652
		[MustTranslate]
		public string calledOffMessage;

		// Token: 0x0400393D RID: 14653
		[MustTranslate]
		public string finishedMessage;

		// Token: 0x0400393E RID: 14654
		public List<RoyalTitleDef> requiredTitleAny = new List<RoyalTitleDef>();

		// Token: 0x0400393F RID: 14655
		private GatheringWorker worker;
	}
}
