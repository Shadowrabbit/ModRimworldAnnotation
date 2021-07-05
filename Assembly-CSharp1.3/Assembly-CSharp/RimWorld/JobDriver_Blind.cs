using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020006E6 RID: 1766
	public class JobDriver_Blind : JobDriver
	{
		// Token: 0x17000930 RID: 2352
		// (get) Token: 0x06003135 RID: 12597 RVA: 0x0011F534 File Offset: 0x0011D734
		protected Pawn Target
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06003136 RID: 12598 RVA: 0x0011F55C File Offset: 0x0011D75C
		public static void Blind(Pawn pawn, Pawn doer)
		{
			Lord lord = pawn.GetLord();
			IEnumerable<BodyPartRecord> enumerable = from p in pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null)
			where p.def == BodyPartDefOf.Eye
			select p;
			LordJob_Ritual_Mutilation lordJob_Ritual_Mutilation;
			if (lord != null && (lordJob_Ritual_Mutilation = (lord.LordJob as LordJob_Ritual_Mutilation)) != null && enumerable.Count<BodyPartRecord>() == 1)
			{
				lordJob_Ritual_Mutilation.mutilatedPawns.Add(pawn);
			}
			foreach (BodyPartRecord bodyPartRecord in enumerable)
			{
				if (bodyPartRecord.def == BodyPartDefOf.Eye)
				{
					pawn.TakeDamage(new DamageInfo(DamageDefOf.SurgicalCut, 99999f, 999f, -1f, null, bodyPartRecord, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true));
					break;
				}
			}
			if (pawn.Dead)
			{
				ThoughtUtility.GiveThoughtsForPawnExecuted(pawn, doer, PawnExecutionKind.GenericBrutal);
			}
		}

		// Token: 0x06003137 RID: 12599 RVA: 0x0011F650 File Offset: 0x0011D850
		public static void CreateHistoryEventDef(Pawn pawn)
		{
			Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.GotBlinded, pawn.Named(HistoryEventArgsNames.Doer)), true);
		}

		// Token: 0x06003138 RID: 12600 RVA: 0x0011F672 File Offset: 0x0011D872
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Target, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06003139 RID: 12601 RVA: 0x0011F694 File Offset: 0x0011D894
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.CheckIdeology("Scarify"))
			{
				yield break;
			}
			this.FailOnDespawnedOrNull(TargetIndex.A);
			Pawn target = this.Target;
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch);
			yield return Toils_General.Wait(35, TargetIndex.None);
			Toil scarify = new Toil();
			scarify.initAction = delegate()
			{
				JobDriver_Blind.Blind(target, this.pawn);
				JobDriver_Blind.CreateHistoryEventDef(target);
				SoundDefOf.Execute_Cut.PlayOneShot(target);
				if (target.RaceProps.BloodDef != null)
				{
					CellRect cellRect = new CellRect(target.PositionHeld.x - 1, target.PositionHeld.z - 1, 3, 3);
					for (int i = 0; i < 3; i++)
					{
						IntVec3 randomCell = cellRect.RandomCell;
						if (randomCell.InBounds(this.Map) && GenSight.LineOfSight(randomCell, target.PositionHeld, this.Map, false, null, 0, 0))
						{
							FilthMaker.TryMakeFilth(randomCell, target.MapHeld, target.RaceProps.BloodDef, this.pawn.LabelIndefinite(), 1, FilthSourceFlags.None);
						}
					}
				}
			};
			scarify.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return Toils_General.Wait(120, TargetIndex.None);
			yield return scarify;
			yield break;
		}

		// Token: 0x04001D6C RID: 7532
		private const TargetIndex TargetPawnIndex = TargetIndex.A;
	}
}
