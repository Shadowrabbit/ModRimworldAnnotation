using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B4C RID: 2892
	public class QuestPart_ShuttleDelay : QuestPart_Delay
	{
		// Token: 0x17000BDB RID: 3035
		// (get) Token: 0x060043A8 RID: 17320 RVA: 0x001686BA File Offset: 0x001668BA
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

		// Token: 0x17000BDC RID: 3036
		// (get) Token: 0x060043A9 RID: 17321 RVA: 0x001686CA File Offset: 0x001668CA
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

		// Token: 0x17000BDD RID: 3037
		// (get) Token: 0x060043AA RID: 17322 RVA: 0x001686EF File Offset: 0x001668EF
		public override bool AlertCritical
		{
			get
			{
				return base.TicksLeft < 60000;
			}
		}

		// Token: 0x17000BDE RID: 3038
		// (get) Token: 0x060043AB RID: 17323 RVA: 0x001686FE File Offset: 0x001668FE
		public override string AlertLabel
		{
			get
			{
				return "QuestPartShuttleArriveDelay".Translate(base.TicksLeft.ToStringTicksToPeriod(true, false, true, true));
			}
		}

		// Token: 0x17000BDF RID: 3039
		// (get) Token: 0x060043AC RID: 17324 RVA: 0x00168724 File Offset: 0x00166924
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

		// Token: 0x060043AD RID: 17325 RVA: 0x001687E4 File Offset: 0x001669E4
		public override string ExtraInspectString(ISelectable target)
		{
			Pawn pawn = target as Pawn;
			if (pawn != null && this.lodgers.Contains(pawn))
			{
				return "ShuttleDelayInspectString".Translate(base.TicksLeft.ToStringTicksToPeriod(true, false, true, true));
			}
			return null;
		}

		// Token: 0x060043AE RID: 17326 RVA: 0x00168830 File Offset: 0x00166A30
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

		// Token: 0x060043AF RID: 17327 RVA: 0x0016889E File Offset: 0x00166A9E
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			if (Find.AnyPlayerHomeMap != null)
			{
				this.lodgers.AddRange(Find.RandomPlayerHomeMap.mapPawns.FreeColonists);
			}
		}

		// Token: 0x060043B0 RID: 17328 RVA: 0x001688C7 File Offset: 0x00166AC7
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.lodgers.Replace(replace, with);
		}

		// Token: 0x0400291A RID: 10522
		public List<Pawn> lodgers = new List<Pawn>();

		// Token: 0x0400291B RID: 10523
		public bool alert;
	}
}
