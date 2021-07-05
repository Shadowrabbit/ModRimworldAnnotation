using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001076 RID: 4214
	public class QuestPart_MoodBelow : QuestPartActivable
	{
		// Token: 0x17000E31 RID: 3633
		// (get) Token: 0x06005BB5 RID: 23477 RVA: 0x001D8D84 File Offset: 0x001D6F84
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

		// Token: 0x17000E32 RID: 3634
		// (get) Token: 0x06005BB6 RID: 23478 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool AlertCritical
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000E33 RID: 3635
		// (get) Token: 0x06005BB7 RID: 23479 RVA: 0x0003F97D File Offset: 0x0003DB7D
		public override string AlertLabel
		{
			get
			{
				return "QuestPartMoodBelowThreshold".Translate();
			}
		}

		// Token: 0x17000E34 RID: 3636
		// (get) Token: 0x06005BB8 RID: 23480 RVA: 0x001D8E00 File Offset: 0x001D7000
		public override string AlertExplanation
		{
			get
			{
				return "QuestPartMoodBelowThresholdDesc".Translate(this.quest.name, GenLabel.ThingsLabel(this.pawns.Where(new Func<Pawn, bool>(this.MoodBelowThreshold)).Cast<Thing>(), "  - "));
			}
		}

		// Token: 0x06005BB9 RID: 23481 RVA: 0x001D8E58 File Offset: 0x001D7058
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

		// Token: 0x06005BBA RID: 23482 RVA: 0x001D8F14 File Offset: 0x001D7114
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

		// Token: 0x06005BBB RID: 23483 RVA: 0x001D8FC0 File Offset: 0x001D71C0
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

		// Token: 0x06005BBC RID: 23484 RVA: 0x0003F98E File Offset: 0x0003DB8E
		private bool MoodBelowThreshold(Pawn pawn)
		{
			return pawn.needs != null && pawn.needs.mood != null && pawn.needs.mood.CurLevelPercentage < this.threshold;
		}

		// Token: 0x06005BBD RID: 23485 RVA: 0x0003F9BF File Offset: 0x0003DBBF
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x04003D85 RID: 15749
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04003D86 RID: 15750
		public float threshold;

		// Token: 0x04003D87 RID: 15751
		public int minTicksBelowThreshold;

		// Token: 0x04003D88 RID: 15752
		public bool showAlert = true;

		// Token: 0x04003D89 RID: 15753
		private List<int> moodBelowThresholdTicks = new List<int>();

		// Token: 0x04003D8A RID: 15754
		private List<Pawn> culpritsResult = new List<Pawn>();
	}
}
