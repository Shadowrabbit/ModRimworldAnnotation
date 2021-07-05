using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000890 RID: 2192
	public class LordJob_Ritual_Duel : LordJob_Ritual
	{
		// Token: 0x17000A58 RID: 2648
		// (get) Token: 0x060039FA RID: 14842 RVA: 0x0014575A File Offset: 0x0014395A
		public DuelBehaviorStage CurrentStage
		{
			get
			{
				if (this.attacksThisStage <= 0)
				{
					return DuelBehaviorStage.Move;
				}
				return DuelBehaviorStage.Attack;
			}
		}

		// Token: 0x17000A59 RID: 2649
		// (get) Token: 0x060039FB RID: 14843 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool RemoveDownedPawns
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A5A RID: 2650
		// (get) Token: 0x060039FC RID: 14844 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool NeverInRestraints
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060039FD RID: 14845 RVA: 0x00145768 File Offset: 0x00143968
		public LordJob_Ritual_Duel()
		{
		}

		// Token: 0x060039FE RID: 14846 RVA: 0x0014577C File Offset: 0x0014397C
		public LordJob_Ritual_Duel(TargetInfo selectedTarget, Precept_Ritual ritual, RitualObligation obligation, List<RitualStage> allStages, RitualRoleAssignments assignments, Pawn organizer = null) : base(selectedTarget, ritual, obligation, allStages, assignments, organizer)
		{
			foreach (RitualRole ritualRole in assignments.AllRolesForReading)
			{
				if (ritualRole != null && ritualRole.id.Contains("duelist"))
				{
					Pawn item = assignments.FirstAssignedPawn(ritualRole);
					this.duelists.Add(item);
					this.pawnsDeathIgnored.Add(item);
				}
			}
		}

		// Token: 0x060039FF RID: 14847 RVA: 0x0014581C File Offset: 0x00143A1C
		public void StartDuelIfNotStartedYet()
		{
			if (!this.duelStarted)
			{
				this.duelStarted = true;
				this.StartDuel();
			}
		}

		// Token: 0x06003A00 RID: 14848 RVA: 0x00145833 File Offset: 0x00143A33
		public override float VoluntaryJoinPriorityFor(Pawn p)
		{
			if (this.duelists.Contains(p))
			{
				return 1f;
			}
			return base.VoluntaryJoinPriorityFor(p);
		}

		// Token: 0x06003A01 RID: 14849 RVA: 0x00145850 File Offset: 0x00143A50
		private void InterruptDuelistJobs()
		{
			foreach (Pawn pawn in this.duelists)
			{
				Pawn_JobTracker jobs = pawn.jobs;
				if (jobs != null)
				{
					jobs.CheckForJobOverride();
				}
			}
		}

		// Token: 0x06003A02 RID: 14850 RVA: 0x001458AC File Offset: 0x00143AAC
		private void StartDuel()
		{
			this.StartMoving();
		}

		// Token: 0x06003A03 RID: 14851 RVA: 0x001458B4 File Offset: 0x00143AB4
		private void StartMoving()
		{
			this.attacksThisStage = 0;
			this.movingTicks = LordJob_Ritual_Duel.MovingTicksPerStage.RandomInRange;
			this.InterruptDuelistJobs();
		}

		// Token: 0x06003A04 RID: 14852 RVA: 0x001458E4 File Offset: 0x00143AE4
		private void StartAttacking()
		{
			this.movingTicks = 0;
			this.attacksThisStage = LordJob_Ritual_Duel.AttacksPerStage.RandomInRange;
			this.InterruptDuelistJobs();
		}

		// Token: 0x06003A05 RID: 14853 RVA: 0x00145911 File Offset: 0x00143B11
		public override void LordJobTick()
		{
			base.LordJobTick();
			if (this.movingTicks > 0)
			{
				this.movingTicks--;
				if (this.movingTicks <= 0)
				{
					this.StartAttacking();
				}
			}
		}

		// Token: 0x06003A06 RID: 14854 RVA: 0x00145940 File Offset: 0x00143B40
		public void Notify_MeleeAttack(Pawn duelist, Thing victim)
		{
			this.attacksThisStage--;
			if (this.attacksThisStage <= 0)
			{
				this.StartMoving();
			}
			if (this.usedWeapon)
			{
				return;
			}
			Verb verb = duelist.meleeVerbs.TryGetMeleeVerb(victim);
			if (verb != null && verb.EquipmentSource != null && verb.EquipmentSource.def.IsMeleeWeapon)
			{
				this.usedWeapon = true;
			}
		}

		// Token: 0x06003A07 RID: 14855 RVA: 0x001459A4 File Offset: 0x00143BA4
		protected override bool ShouldCallOffBecausePawnNoLongerOwned(Pawn p)
		{
			return base.ShouldCallOffBecausePawnNoLongerOwned(p) && !this.duelists.Contains(p);
		}

		// Token: 0x06003A08 RID: 14856 RVA: 0x001459C0 File Offset: 0x00143BC0
		public Pawn Opponent(Pawn duelist)
		{
			return this.duelists[(this.duelists.IndexOf(duelist) == 0) ? 1 : 0];
		}

		// Token: 0x06003A09 RID: 14857 RVA: 0x001459DF File Offset: 0x00143BDF
		public override void Notify_PawnLost(Pawn p, PawnLostCondition condition)
		{
			if (p.Dead && this.duelists.Contains(p))
			{
				p.health.killedByRitual = true;
			}
		}

		// Token: 0x06003A0A RID: 14858 RVA: 0x00145A04 File Offset: 0x00143C04
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.duelists, "duelists", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.usedWeapon, "usedWeapon", false, false);
			Scribe_Values.Look<int>(ref this.movingTicks, "movingTicks", 0, false);
			Scribe_Values.Look<int>(ref this.attacksThisStage, "attacksThisStage", 0, false);
			Scribe_Values.Look<bool>(ref this.duelStarted, "duelStarted", false, false);
		}

		// Token: 0x04001FE3 RID: 8163
		public List<Pawn> duelists = new List<Pawn>();

		// Token: 0x04001FE4 RID: 8164
		public bool usedWeapon;

		// Token: 0x04001FE5 RID: 8165
		private bool duelStarted;

		// Token: 0x04001FE6 RID: 8166
		private int attacksThisStage;

		// Token: 0x04001FE7 RID: 8167
		private int movingTicks;

		// Token: 0x04001FE8 RID: 8168
		private static readonly IntRange AttacksPerStage = new IntRange(4, 8);

		// Token: 0x04001FE9 RID: 8169
		private static readonly IntRange MovingTicksPerStage = new IntRange(360, 600);
	}
}
