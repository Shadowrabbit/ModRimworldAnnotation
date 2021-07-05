using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B49 RID: 2889
	public class QuestPart_MoodBelow : QuestPartActivable
	{
		// Token: 0x17000BD6 RID: 3030
		// (get) Token: 0x06004394 RID: 17300 RVA: 0x001681EC File Offset: 0x001663EC
		public override AlertReport AlertReport
		{
			get
			{
				if (!this.showAlert || this.minTicksBelowThreshold < 60)
				{
					return AlertReport.Inactive;
				}
				this.culpritsResult.Clear();
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (this.MoodBelowThreshold(this.pawns[i]))
					{
						this.culpritsResult.Add(this.pawns[i]);
					}
				}
				return AlertReport.CulpritsAre(this.culpritsResult);
			}
		}

		// Token: 0x17000BD7 RID: 3031
		// (get) Token: 0x06004395 RID: 17301 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool AlertCritical
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000BD8 RID: 3032
		// (get) Token: 0x06004396 RID: 17302 RVA: 0x00168268 File Offset: 0x00166468
		public override string AlertLabel
		{
			get
			{
				return "QuestPartMoodBelowThreshold".Translate();
			}
		}

		// Token: 0x17000BD9 RID: 3033
		// (get) Token: 0x06004397 RID: 17303 RVA: 0x0016827C File Offset: 0x0016647C
		public override string AlertExplanation
		{
			get
			{
				return "QuestPartMoodBelowThresholdDesc".Translate(this.quest.name, GenLabel.ThingsLabel(this.pawns.Where(new Func<Pawn, bool>(this.MoodBelowThreshold)).Cast<Thing>(), "  - "));
			}
		}

		// Token: 0x06004398 RID: 17304 RVA: 0x001682D4 File Offset: 0x001664D4
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			while (this.moodBelowThresholdTicks.Count < this.pawns.Count)
			{
				this.moodBelowThresholdTicks.Add(0);
			}
			for (int i = 0; i < this.pawns.Count; i++)
			{
				if (this.MoodBelowThreshold(this.pawns[i]))
				{
					List<int> list = this.moodBelowThresholdTicks;
					int index = i;
					int num = list[index];
					list[index] = num + 1;
					if (this.moodBelowThresholdTicks[i] >= this.minTicksBelowThreshold)
					{
						base.Complete(this.pawns[i].Named("SUBJECT"));
						return;
					}
				}
				else
				{
					this.moodBelowThresholdTicks[i] = 0;
				}
			}
		}

		// Token: 0x06004399 RID: 17305 RVA: 0x00168390 File Offset: 0x00166590
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<float>(ref this.threshold, "threshold", 0f, false);
			Scribe_Values.Look<int>(ref this.minTicksBelowThreshold, "minTicksBelowThreshold", 0, false);
			Scribe_Values.Look<bool>(ref this.showAlert, "showAlert", true, false);
			Scribe_Collections.Look<int>(ref this.moodBelowThresholdTicks, "moodBelowThresholdTicks", LookMode.Value, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x0600439A RID: 17306 RVA: 0x0016843C File Offset: 0x0016663C
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			if (Find.AnyPlayerHomeMap != null)
			{
				Map randomPlayerHomeMap = Find.RandomPlayerHomeMap;
				this.pawns.Add(randomPlayerHomeMap.mapPawns.FreeColonists.FirstOrDefault<Pawn>());
				this.threshold = 0.5f;
				this.minTicksBelowThreshold = 2500;
			}
		}

		// Token: 0x0600439B RID: 17307 RVA: 0x0016848D File Offset: 0x0016668D
		private bool MoodBelowThreshold(Pawn pawn)
		{
			return pawn.needs != null && pawn.needs.mood != null && pawn.needs.mood.CurLevelPercentage < this.threshold;
		}

		// Token: 0x0600439C RID: 17308 RVA: 0x001684BE File Offset: 0x001666BE
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x0400290D RID: 10509
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x0400290E RID: 10510
		public float threshold;

		// Token: 0x0400290F RID: 10511
		public int minTicksBelowThreshold;

		// Token: 0x04002910 RID: 10512
		public bool showAlert = true;

		// Token: 0x04002911 RID: 10513
		private List<int> moodBelowThresholdTicks = new List<int>();

		// Token: 0x04002912 RID: 10514
		private List<Pawn> culpritsResult = new List<Pawn>();
	}
}
