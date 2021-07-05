using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200074E RID: 1870
	public static class Toils_Interpersonal
	{
		// Token: 0x060033BA RID: 13242 RVA: 0x00125DBC File Offset: 0x00123FBC
		public static Toil GotoInteractablePosition(TargetIndex target)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Pawn pawn = (Pawn)((Thing)actor.CurJob.GetTarget(target));
				if (InteractionUtility.IsGoodPositionForInteraction(actor, pawn))
				{
					actor.jobs.curDriver.ReadyForNextToil();
					return;
				}
				actor.pather.StartPath(pawn, PathEndMode.Touch);
			};
			toil.tickAction = delegate()
			{
				Pawn actor = toil.actor;
				Pawn pawn = (Pawn)((Thing)actor.CurJob.GetTarget(target));
				Map map = actor.Map;
				if (InteractionUtility.IsGoodPositionForInteraction(actor, pawn) && actor.Position.InHorDistOf(pawn.Position, (float)Mathf.CeilToInt(3f)) && (!actor.pather.Moving || actor.pather.nextCell.GetDoor(map) == null))
				{
					actor.pather.StopDead();
					actor.jobs.curDriver.ReadyForNextToil();
					return;
				}
				if (!actor.pather.Moving)
				{
					IntVec3 intVec = IntVec3.Invalid;
					int num = 0;
					while (num < 9 && (num != 8 || !intVec.IsValid))
					{
						IntVec3 intVec2 = pawn.Position + GenAdj.AdjacentCellsAndInside[num];
						if (intVec2.InBounds(map) && intVec2.Walkable(map) && intVec2 != actor.Position && InteractionUtility.IsGoodPositionForInteraction(intVec2, pawn.Position, map) && actor.CanReach(intVec2, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn) && (!intVec.IsValid || actor.Position.DistanceToSquared(intVec2) < actor.Position.DistanceToSquared(intVec)))
						{
							intVec = intVec2;
						}
						num++;
					}
					if (intVec.IsValid)
					{
						actor.pather.StartPath(intVec, PathEndMode.OnCell);
						return;
					}
					actor.jobs.curDriver.EndJobWith(JobCondition.Incompletable);
				}
			};
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			return toil;
		}

		// Token: 0x060033BB RID: 13243 RVA: 0x00125E30 File Offset: 0x00124030
		public static Toil GotoPrisoner(Pawn pawn, Pawn talkee, PrisonerInteractionModeDef mode)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				pawn.pather.StartPath(talkee, PathEndMode.Touch);
			};
			toil.AddFailCondition(() => talkee.DestroyedOrNull() || (mode != PrisonerInteractionModeDefOf.Execution && !talkee.Awake()) || !talkee.IsPrisonerOfColony || (talkee.guest == null || talkee.guest.interactionMode != mode));
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			return toil;
		}

		// Token: 0x060033BC RID: 13244 RVA: 0x00125E90 File Offset: 0x00124090
		public static Toil GotoGuiltyColonist(Pawn pawn, Pawn talkee)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				pawn.pather.StartPath(talkee, PathEndMode.Touch);
			};
			toil.AddFailCondition(() => talkee.DestroyedOrNull() || !talkee.guilt.IsGuilty);
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			return toil;
		}

		// Token: 0x060033BD RID: 13245 RVA: 0x00125EE8 File Offset: 0x001240E8
		public static Toil GotoSlave(Pawn pawn, Pawn talkee)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				pawn.pather.StartPath(talkee, PathEndMode.Touch);
			};
			toil.AddFailCondition(() => talkee.DestroyedOrNull() || !talkee.IsSlaveOfColony);
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			return toil;
		}

		// Token: 0x060033BE RID: 13246 RVA: 0x00125F40 File Offset: 0x00124140
		public static Toil WaitToBeAbleToInteract(Pawn pawn)
		{
			return new Toil
			{
				initAction = delegate()
				{
					if (!pawn.interactions.InteractedTooRecentlyToInteract())
					{
						pawn.jobs.curDriver.ReadyForNextToil();
					}
				},
				tickAction = delegate()
				{
					if (!pawn.interactions.InteractedTooRecentlyToInteract())
					{
						pawn.jobs.curDriver.ReadyForNextToil();
					}
				},
				socialMode = RandomSocialMode.Off,
				defaultCompleteMode = ToilCompleteMode.Never
			};
		}

		// Token: 0x060033BF RID: 13247 RVA: 0x00125F94 File Offset: 0x00124194
		public static Toil ConvinceRecruitee(Pawn pawn, Pawn talkee, InteractionDef interactionDef = null)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				if (!pawn.interactions.TryInteractWith(talkee, interactionDef ?? InteractionDefOf.BuildRapport))
				{
					pawn.jobs.curDriver.ReadyForNextToil();
					return;
				}
				pawn.records.Increment(RecordDefOf.PrisonersChatted);
			};
			toil.FailOn(() => !talkee.guest.ScheduledForInteraction);
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = 350;
			toil.activeSkill = (() => SkillDefOf.Social);
			return toil;
		}

		// Token: 0x060033C0 RID: 13248 RVA: 0x00126024 File Offset: 0x00124224
		public static Toil ReduceWill(Pawn pawn, Pawn talkee)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				if (!pawn.interactions.TryInteractWith(talkee, InteractionDefOf.ReduceWill))
				{
					pawn.jobs.curDriver.ReadyForNextToil();
					return;
				}
				pawn.records.Increment(RecordDefOf.PrisonersChatted);
			};
			toil.FailOn(() => !talkee.guest.ScheduledForInteraction);
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = 350;
			toil.activeSkill = (() => SkillDefOf.Social);
			return toil;
		}

		// Token: 0x060033C1 RID: 13249 RVA: 0x001260B0 File Offset: 0x001242B0
		public static Toil SetLastInteractTime(TargetIndex targetInd)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn pawn = (Pawn)toil.actor.jobs.curJob.GetTarget(targetInd).Thing;
				pawn.mindState.lastAssignedInteractTime = Find.TickManager.TicksGame;
				pawn.mindState.interactionsToday++;
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		// Token: 0x060033C2 RID: 13250 RVA: 0x00126100 File Offset: 0x00124300
		public static Toil TryRecruit(TargetIndex recruiteeInd)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Pawn pawn = (Pawn)actor.jobs.curJob.GetTarget(recruiteeInd).Thing;
				if (!pawn.Spawned || !pawn.Awake())
				{
					return;
				}
				InteractionDef intDef = pawn.AnimalOrWildMan() ? InteractionDefOf.TameAttempt : InteractionDefOf.RecruitAttempt;
				actor.interactions.TryInteractWith(pawn, intDef);
			};
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = 350;
			toil.activeSkill = delegate()
			{
				if (!((Pawn)toil.actor.jobs.curJob.GetTarget(recruiteeInd).Thing).RaceProps.Animal)
				{
					return SkillDefOf.Social;
				}
				return SkillDefOf.Animals;
			};
			return toil;
		}

		// Token: 0x060033C3 RID: 13251 RVA: 0x00126184 File Offset: 0x00124384
		public static Toil TryTrain(TargetIndex traineeInd)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Pawn pawn = (Pawn)actor.jobs.curJob.GetTarget(traineeInd).Thing;
				if (pawn.Spawned && pawn.Awake() && actor.interactions.TryInteractWith(pawn, InteractionDefOf.TrainAttempt))
				{
					float num = actor.GetStatValue(StatDefOf.TrainAnimalChance, true);
					num *= GenMath.LerpDouble(0f, 1f, 1.5f, 0.5f, pawn.RaceProps.wildness);
					if (actor.relations.DirectRelationExists(PawnRelationDefOf.Bond, pawn))
					{
						num *= 5f;
					}
					num = Mathf.Clamp01(num);
					TrainableDef trainableDef = pawn.training.NextTrainableToTrain();
					if (trainableDef == null)
					{
						Log.ErrorOnce("Attempted to train untrainable animal", 7842936);
						return;
					}
					string text;
					if (Rand.Value < num)
					{
						pawn.training.Train(trainableDef, actor, false);
						if (pawn.caller != null)
						{
							pawn.caller.DoCall();
						}
						text = "TextMote_TrainSuccess".Translate(trainableDef.LabelCap, num.ToStringPercent());
						RelationsUtility.TryDevelopBondRelation(actor, pawn, 0.007f);
						TaleRecorder.RecordTale(TaleDefOf.TrainedAnimal, new object[]
						{
							actor,
							pawn,
							trainableDef
						});
					}
					else
					{
						text = "TextMote_TrainFail".Translate(trainableDef.LabelCap, num.ToStringPercent());
					}
					text = string.Concat(new object[]
					{
						text,
						"\n",
						pawn.training.GetSteps(trainableDef),
						" / ",
						trainableDef.steps
					});
					MoteMaker.ThrowText((actor.DrawPos + pawn.DrawPos) / 2f, actor.Map, text, 5f);
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = 100;
			toil.activeSkill = (() => SkillDefOf.Animals);
			return toil;
		}

		// Token: 0x060033C4 RID: 13252 RVA: 0x0012620C File Offset: 0x0012440C
		public static Toil Interact(TargetIndex otherPawnInd, InteractionDef interaction)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Pawn pawn = (Pawn)actor.jobs.curJob.GetTarget(otherPawnInd).Thing;
				if (!pawn.Spawned)
				{
					return;
				}
				actor.interactions.TryInteractWith(pawn, interaction);
			};
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = 60;
			return toil;
		}

		// Token: 0x060033C5 RID: 13253 RVA: 0x0012627C File Offset: 0x0012447C
		public static Toil TryEnslave(TargetIndex prisonerInd)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Pawn pawn = (Pawn)actor.jobs.curJob.GetTarget(prisonerInd).Thing;
				if (!pawn.Spawned || !pawn.Awake())
				{
					return;
				}
				actor.interactions.TryInteractWith(pawn, InteractionDefOf.EnslaveAttempt);
			};
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = 350;
			toil.activeSkill = (() => SkillDefOf.Social);
			return toil;
		}

		// Token: 0x060033C6 RID: 13254 RVA: 0x00126310 File Offset: 0x00124510
		public static Toil TryConvert(TargetIndex prisonerInd)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Pawn recipient = (Pawn)actor.jobs.curJob.GetTarget(prisonerInd).Thing;
				actor.interactions.TryInteractWith(recipient, InteractionDefOf.ConvertIdeoAttempt);
			};
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = 350;
			toil.activeSkill = (() => SkillDefOf.Social);
			return toil;
		}
	}
}
