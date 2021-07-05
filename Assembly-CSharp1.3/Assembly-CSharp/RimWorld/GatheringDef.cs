using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A69 RID: 2665
	public class GatheringDef : Def
	{
		// Token: 0x17000B36 RID: 2870
		// (get) Token: 0x06004001 RID: 16385 RVA: 0x0015AFC0 File Offset: 0x001591C0
		public bool IsRandomSelectable
		{
			get
			{
				return this.randomSelectionWeight > 0f;
			}
		}

		// Token: 0x17000B37 RID: 2871
		// (get) Token: 0x06004002 RID: 16386 RVA: 0x0015AFCF File Offset: 0x001591CF
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

		// Token: 0x06004003 RID: 16387 RVA: 0x0015B001 File Offset: 0x00159201
		public bool CanExecute(Map map, Pawn organizer = null, bool ignoreGameConditions = false)
		{
			return (ignoreGameConditions || GatheringsUtility.AcceptableGameConditionsToStartGathering(map, this)) && this.Worker.CanExecute(map, organizer);
		}

		// Token: 0x04002447 RID: 9287
		public Type workerClass = typeof(GatheringWorker);

		// Token: 0x04002448 RID: 9288
		public DutyDef duty;

		// Token: 0x04002449 RID: 9289
		public float randomSelectionWeight;

		// Token: 0x0400244A RID: 9290
		public bool respectTimetable = true;

		// Token: 0x0400244B RID: 9291
		public List<ThingDef> gatherSpotDefs;

		// Token: 0x0400244C RID: 9292
		[MustTranslate]
		public string letterTitle;

		// Token: 0x0400244D RID: 9293
		[MustTranslate]
		public string letterText;

		// Token: 0x0400244E RID: 9294
		[MustTranslate]
		public string calledOffMessage;

		// Token: 0x0400244F RID: 9295
		[MustTranslate]
		public string finishedMessage;

		// Token: 0x04002450 RID: 9296
		public List<RoyalTitleDef> requiredTitleAny = new List<RoyalTitleDef>();

		// Token: 0x04002451 RID: 9297
		private GatheringWorker worker;
	}
}
