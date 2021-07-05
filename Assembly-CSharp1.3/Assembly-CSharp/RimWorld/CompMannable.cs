using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200114D RID: 4429
	public class CompMannable : ThingComp
	{
		// Token: 0x1700124D RID: 4685
		// (get) Token: 0x06006A74 RID: 27252 RVA: 0x0023CF05 File Offset: 0x0023B105
		public bool MannedNow
		{
			get
			{
				return Find.TickManager.TicksGame - this.lastManTick <= 1 && this.lastManPawn != null && this.lastManPawn.Spawned;
			}
		}

		// Token: 0x1700124E RID: 4686
		// (get) Token: 0x06006A75 RID: 27253 RVA: 0x0023CF30 File Offset: 0x0023B130
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

		// Token: 0x1700124F RID: 4687
		// (get) Token: 0x06006A76 RID: 27254 RVA: 0x0023CF42 File Offset: 0x0023B142
		public CompProperties_Mannable Props
		{
			get
			{
				return (CompProperties_Mannable)this.props;
			}
		}

		// Token: 0x06006A77 RID: 27255 RVA: 0x0023CF4F File Offset: 0x0023B14F
		public void ManForATick(Pawn pawn)
		{
			this.lastManTick = Find.TickManager.TicksGame;
			this.lastManPawn = pawn;
			pawn.mindState.lastMannedThing = this.parent;
		}

		// Token: 0x06006A78 RID: 27256 RVA: 0x0023CF79 File Offset: 0x0023B179
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
					yield return new FloatMenuOption("CannotManThing".Translate(this.parent.LabelShort, this.parent) + " (" + "IsIncapableOfViolenceLower".Translate(pawn.LabelShort, pawn) + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				}
				yield break;
			}
			FloatMenuOption floatMenuOption = new FloatMenuOption("OrderManThing".Translate(this.parent.LabelShort, this.parent), delegate()
			{
				Job job = JobMaker.MakeJob(JobDefOf.ManTurret, this.parent);
				pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			yield return floatMenuOption;
			yield break;
		}

		// Token: 0x04003B54 RID: 15188
		private int lastManTick = -1;

		// Token: 0x04003B55 RID: 15189
		private Pawn lastManPawn;
	}
}
