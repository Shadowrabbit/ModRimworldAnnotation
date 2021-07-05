using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200107B RID: 4219
	public class QuestPart_ShuttleDelay : QuestPart_Delay
	{
		// Token: 0x17000E38 RID: 3640
		// (get) Token: 0x06005BD5 RID: 23509 RVA: 0x0003FAD4 File Offset: 0x0003DCD4
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				int num;
				for (int i = 0; i < this.lodgers.Count; i = num + 1)
				{
					yield return this.lodgers[i];
					num = i;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x17000E39 RID: 3641
		// (get) Token: 0x06005BD6 RID: 23510 RVA: 0x0003FAE4 File Offset: 0x0003DCE4
		public override AlertReport AlertReport
		{
			get
			{
				if (!this.alert || base.State != QuestPartState.Enabled)
				{
					return false;
				}
				return AlertReport.CulpritsAre(this.lodgers);
			}
		}

		// Token: 0x17000E3A RID: 3642
		// (get) Token: 0x06005BD7 RID: 23511 RVA: 0x0003FB09 File Offset: 0x0003DD09
		public override bool AlertCritical
		{
			get
			{
				return base.TicksLeft < 60000;
			}
		}

		// Token: 0x17000E3B RID: 3643
		// (get) Token: 0x06005BD8 RID: 23512 RVA: 0x0003FB18 File Offset: 0x0003DD18
		public override string AlertLabel
		{
			get
			{
				return "QuestPartShuttleArriveDelay".Translate(base.TicksLeft.ToStringTicksToPeriod(true, false, true, true));
			}
		}

		// Token: 0x17000E3C RID: 3644
		// (get) Token: 0x06005BD9 RID: 23513 RVA: 0x001D92B8 File Offset: 0x001D74B8
		public override string AlertExplanation
		{
			get
			{
				if (this.quest.hidden)
				{
					return "QuestPartShuttleArriveDelayDescHidden".Translate(base.TicksLeft.ToStringTicksToPeriod(true, false, true, true).Colorize(ColoredText.DateTimeColor));
				}
				return "QuestPartShuttleArriveDelayDesc".Translate(this.quest.name, base.TicksLeft.ToStringTicksToPeriod(true, false, true, true).Colorize(ColoredText.DateTimeColor), (from p in this.lodgers
				select p.LabelShort).ToLineList("- ", false));
			}
		}

		// Token: 0x06005BDA RID: 23514 RVA: 0x001D9378 File Offset: 0x001D7578
		public override string ExtraInspectString(ISelectable target)
		{
			Pawn pawn = target as Pawn;
			if (pawn != null && this.lodgers.Contains(pawn))
			{
				return "ShuttleDelayInspectString".Translate(base.TicksLeft.ToStringTicksToPeriod(true, false, true, true));
			}
			return null;
		}

		// Token: 0x06005BDB RID: 23515 RVA: 0x001D93C4 File Offset: 0x001D75C4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.lodgers, "lodgers", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.alert, "alert", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.lodgers.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x06005BDC RID: 23516 RVA: 0x0003FB3D File Offset: 0x0003DD3D
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			if (Find.AnyPlayerHomeMap != null)
			{
				this.lodgers.AddRange(Find.RandomPlayerHomeMap.mapPawns.FreeColonists);
			}
		}

		// Token: 0x06005BDD RID: 23517 RVA: 0x0003FB66 File Offset: 0x0003DD66
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.lodgers.Replace(replace, with);
		}

		// Token: 0x04003D99 RID: 15769
		public List<Pawn> lodgers = new List<Pawn>();

		// Token: 0x04003D9A RID: 15770
		public bool alert;
	}
}
