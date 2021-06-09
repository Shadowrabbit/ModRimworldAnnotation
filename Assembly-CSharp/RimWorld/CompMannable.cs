using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017DB RID: 6107
	public class CompMannable : ThingComp
	{
		// Token: 0x17001508 RID: 5384
		// (get) Token: 0x0600872C RID: 34604 RVA: 0x0005AC9E File Offset: 0x00058E9E
		public bool MannedNow
		{
			get
			{
				return Find.TickManager.TicksGame - this.lastManTick <= 1 && this.lastManPawn != null && this.lastManPawn.Spawned;
			}
		}

		// Token: 0x17001509 RID: 5385
		// (get) Token: 0x0600872D RID: 34605 RVA: 0x0005ACC9 File Offset: 0x00058EC9
		public Pawn ManningPawn
		{
			get
			{
				if (!this.MannedNow)
				{
					return null;
				}
				return this.lastManPawn;
			}
		}

		// Token: 0x1700150A RID: 5386
		// (get) Token: 0x0600872E RID: 34606 RVA: 0x0005ACDB File Offset: 0x00058EDB
		public CompProperties_Mannable Props
		{
			get
			{
				return (CompProperties_Mannable)this.props;
			}
		}

		// Token: 0x0600872F RID: 34607 RVA: 0x0005ACE8 File Offset: 0x00058EE8
		public void ManForATick(Pawn pawn)
		{
			this.lastManTick = Find.TickManager.TicksGame;
			this.lastManPawn = pawn;
			pawn.mindState.lastMannedThing = this.parent;
		}

		// Token: 0x06008730 RID: 34608 RVA: 0x0005AD12 File Offset: 0x00058F12
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn pawn)
		{
			if (!pawn.RaceProps.ToolUser)
			{
				yield break;
			}
			if (!pawn.CanReserveAndReach(this.parent, PathEndMode.InteractionCell, Danger.Deadly, 1, -1, null, false))
			{
				yield break;
			}
			if (this.Props.manWorkType != WorkTags.None && pawn.WorkTagIsDisabled(this.Props.manWorkType))
			{
				if (this.Props.manWorkType == WorkTags.Violent)
				{
					yield return new FloatMenuOption("CannotManThing".Translate(this.parent.LabelShort, this.parent) + " (" + "IsIncapableOfViolenceLower".Translate(pawn.LabelShort, pawn) + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
				}
				yield break;
			}
			FloatMenuOption floatMenuOption = new FloatMenuOption("OrderManThing".Translate(this.parent.LabelShort, this.parent), delegate()
			{
				Job job = JobMaker.MakeJob(JobDefOf.ManTurret, this.parent);
				pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
			}, MenuOptionPriority.Default, null, null, 0f, null, null);
			yield return floatMenuOption;
			yield break;
		}

		// Token: 0x040056E0 RID: 22240
		private int lastManTick = -1;

		// Token: 0x040056E1 RID: 22241
		private Pawn lastManPawn;
	}
}
