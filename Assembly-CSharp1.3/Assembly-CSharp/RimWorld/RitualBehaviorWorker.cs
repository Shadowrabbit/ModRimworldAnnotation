using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000F22 RID: 3874
	public class RitualBehaviorWorker : IExposable
	{
		// Token: 0x06005C25 RID: 23589 RVA: 0x000033AC File Offset: 0x000015AC
		public RitualBehaviorWorker()
		{
		}

		// Token: 0x06005C26 RID: 23590 RVA: 0x001FCD4A File Offset: 0x001FAF4A
		public RitualBehaviorWorker(RitualBehaviorDef def)
		{
			this.def = def;
		}

		// Token: 0x17001010 RID: 4112
		// (get) Token: 0x06005C27 RID: 23591 RVA: 0x00002688 File Offset: 0x00000888
		public virtual Sustainer SoundPlaying
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06005C28 RID: 23592 RVA: 0x001FCD5C File Offset: 0x001FAF5C
		public virtual string CanStartRitualNow(TargetInfo target, Precept_Ritual ritual, Pawn selectedPawn = null, Dictionary<string, Pawn> forcedForRole = null)
		{
			using (List<LordJob_Ritual>.Enumerator enumerator = Find.IdeoManager.GetActiveRituals(target.Map).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Ritual == ritual)
					{
						return "CantStartRitualAlreadyInProgress".Translate(ritual.Label).CapitalizeFirst();
					}
				}
			}
			if (selectedPawn != null)
			{
				RitualBehaviorWorker behavior = ritual.behavior;
				if (((behavior != null) ? behavior.def.roles : null) != null)
				{
					foreach (RitualRole ritualRole in ritual.behavior.def.roles)
					{
						string text;
						if (ritualRole.defaultForSelectedColonist && !ritualRole.AppliesToPawn(selectedPawn, out text, null, null, ritual))
						{
							if (text.NullOrEmpty())
							{
								return "CantStartRitualSelectedPawnCannotBeRole".Translate(selectedPawn.Named("PAWN"), ritualRole.Label.Named("ROLE")).CapitalizeFirst();
							}
							return text;
						}
					}
				}
			}
			List<Pawn> list = target.Map.mapPawns.FreeColonistsAndPrisonersSpawned.ToList<Pawn>();
			list.AddRange(target.Map.mapPawns.SpawnedColonyAnimals);
			if (!ritual.behavior.def.roles.NullOrEmpty<RitualRole>())
			{
				using (List<RitualRole>.Enumerator enumerator2 = ritual.behavior.def.roles.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						RitualRole role = enumerator2.Current;
						if (role.required && !role.substitutable)
						{
							IEnumerable<RitualRole> source = (role.mergeId == null) ? Gen.YieldSingle<RitualRole>(role) : (from r in ritual.behavior.def.roles
							where r.mergeId == role.mergeId
							select r);
							if (list.Count(delegate(Pawn p)
							{
								string text2;
								return role.AppliesToPawn(p, out text2, null, null, null);
							}) < source.Count<RitualRole>() && (forcedForRole == null || !forcedForRole.ContainsKey(role.id)))
							{
								Precept precept = ritual.ideo.PreceptsListForReading.FirstOrDefault((Precept p) => p.def == role.precept);
								if (precept != null)
								{
									return "MessageNeedAssignedRoleToBeginRitual".Translate(role.missingDesc ?? Find.ActiveLanguageWorker.WithIndefiniteArticle(precept.LabelCap, false, false), ritual.Label);
								}
								if (source.Count<RitualRole>() == 1)
								{
									return "MessageNoRequiredRolePawnToBeginRitual".Translate(role.missingDesc ?? Find.ActiveLanguageWorker.WithIndefiniteArticle(role.Label, false, false), ritual.Label);
								}
								return "MessageNoRequiredRolePawnToBeginRitual".Translate(source.Count<RitualRole>().ToString() + " " + (role.missingDesc ?? Find.ActiveLanguageWorker.Pluralize(role.Label, -1)), ritual.Label);
							}
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06005C29 RID: 23593 RVA: 0x00002688 File Offset: 0x00000888
		public virtual string GetExplanation(Precept_Ritual ritual, RitualRoleAssignments assignments, float quality)
		{
			return null;
		}

		// Token: 0x06005C2A RID: 23594 RVA: 0x001FD138 File Offset: 0x001FB338
		public virtual bool CanExecuteOn(TargetInfo target, RitualObligation obligation)
		{
			return obligation == null || obligation.precept.obligationTargetFilter.CanUseTarget(target, obligation).canUse;
		}

		// Token: 0x06005C2B RID: 23595 RVA: 0x001FD158 File Offset: 0x001FB358
		public virtual void TryExecuteOn(TargetInfo target, Pawn organizer, Precept_Ritual ritual, RitualObligation obligation, RitualRoleAssignments assignments, bool playerForced = false)
		{
			if (this.CanStartRitualNow(target, ritual, null, null) != null)
			{
				return;
			}
			if (!this.CanExecuteOn(target, obligation))
			{
				return;
			}
			LordJob_Ritual lordJob = (LordJob_Ritual)this.CreateLordJob(target, organizer, ritual, obligation, assignments);
			LordMaker.MakeNewLord(Faction.OfPlayer, lordJob, target.Map, assignments.Participants.Where(delegate(Pawn p)
			{
				RitualRole ritualRole = lordJob.RoleFor(p, false);
				return ritualRole == null || ritualRole.addToLord;
			}));
			lordJob.PreparePawns();
			this.PostExecute(target, organizer, ritual, obligation, assignments);
			if (playerForced)
			{
				foreach (Pawn pawn in assignments.Participants)
				{
					pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, false, true);
				}
			}
		}

		// Token: 0x06005C2C RID: 23596 RVA: 0x001FD234 File Offset: 0x001FB434
		protected virtual void PostExecute(TargetInfo target, Pawn organizer, Precept_Ritual ritual, RitualObligation obligation, RitualRoleAssignments assignments)
		{
			Messages.Message("RitualBegun".Translate(ritual.Label).CapitalizeFirst(), target, MessageTypeDefOf.NeutralEvent, true);
		}

		// Token: 0x06005C2D RID: 23597 RVA: 0x001FD274 File Offset: 0x001FB474
		protected virtual LordJob CreateLordJob(TargetInfo target, Pawn organizer, Precept_Ritual ritual, RitualObligation obligation, RitualRoleAssignments assignments)
		{
			return new LordJob_Ritual(target, ritual, obligation, this.def.stages, assignments, null);
		}

		// Token: 0x06005C2E RID: 23598 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Tick(LordJob_Ritual ritual)
		{
		}

		// Token: 0x06005C2F RID: 23599 RVA: 0x001FD290 File Offset: 0x001FB490
		public bool SpectatorsRequired()
		{
			if (this.def.stages.NullOrEmpty<RitualStage>())
			{
				return true;
			}
			for (int i = 0; i < this.def.stages.Count; i++)
			{
				if (this.def.stages[i].spectatorsRequired)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005C30 RID: 23600 RVA: 0x001FD2E8 File Offset: 0x001FB4E8
		public bool HasAnyRole(Pawn p)
		{
			if (this.def.stages.NullOrEmpty<RitualStage>())
			{
				return false;
			}
			for (int i = 0; i < this.def.stages.Count; i++)
			{
				if (this.def.stages[i].HasRole(p))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005C31 RID: 23601 RVA: 0x001FD340 File Offset: 0x001FB540
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<RitualBehaviorDef>(ref this.def, "def");
		}

		// Token: 0x06005C32 RID: 23602 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Cleanup(LordJob_Ritual ritual)
		{
		}

		// Token: 0x06005C33 RID: 23603 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PostCleanup(LordJob_Ritual ritual)
		{
		}

		// Token: 0x06005C34 RID: 23604 RVA: 0x000B955A File Offset: 0x000B775A
		public virtual int ExpectedDurationOverride(Precept_Ritual ritual, RitualRoleAssignments assignments, float quality)
		{
			return -1;
		}

		// Token: 0x040035B8 RID: 13752
		public RitualBehaviorDef def;
	}
}
