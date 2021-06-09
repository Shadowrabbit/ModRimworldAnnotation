using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BC8 RID: 3016
	public class JobDriver_HaulCorpseToPublicPlace : JobDriver
	{
		// Token: 0x17000B1E RID: 2846
		// (get) Token: 0x060046E0 RID: 18144 RVA: 0x00196508 File Offset: 0x00194708
		private Corpse Corpse
		{
			get
			{
				return (Corpse)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000B1F RID: 2847
		// (get) Token: 0x060046E1 RID: 18145 RVA: 0x00196530 File Offset: 0x00194730
		private Building_Grave Grave
		{
			get
			{
				return (Building_Grave)this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x17000B20 RID: 2848
		// (get) Token: 0x060046E2 RID: 18146 RVA: 0x00033AF5 File Offset: 0x00031CF5
		private bool InGrave
		{
			get
			{
				return this.Grave != null;
			}
		}

		// Token: 0x17000B21 RID: 2849
		// (get) Token: 0x060046E3 RID: 18147 RVA: 0x00033B00 File Offset: 0x00031D00
		private Thing Target
		{
			get
			{
				return this.Grave ?? this.Corpse;
			}
		}

		// Token: 0x060046E4 RID: 18148 RVA: 0x00033B12 File Offset: 0x00031D12
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Target, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x060046E5 RID: 18149 RVA: 0x00033B34 File Offset: 0x00031D34
		public override string GetReport()
		{
			if (this.InGrave && this.Grave.def == ThingDefOf.Grave)
			{
				return "ReportDiggingUpCorpse".Translate();
			}
			return base.GetReport();
		}

		// Token: 0x060046E6 RID: 18150 RVA: 0x00033B66 File Offset: 0x00031D66
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			Toil gotoCorpse = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Jump.JumpIfTargetInvalid(TargetIndex.B, gotoCorpse);
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.InteractionCell).FailOnDespawnedOrNull(TargetIndex.B);
			yield return Toils_General.Wait(300, TargetIndex.None).WithProgressBarToilDelay(TargetIndex.B, false, -0.5f).FailOnDespawnedOrNull(TargetIndex.B).FailOnCannotTouch(TargetIndex.B, PathEndMode.InteractionCell);
			yield return Toils_General.Open(TargetIndex.B);
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			yield return gotoCorpse;
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false);
			yield return this.FindCellToDropCorpseToil();
			yield return Toils_Reserve.Reserve(TargetIndex.C, 1, -1, null);
			yield return Toils_Goto.GotoCell(TargetIndex.C, PathEndMode.Touch);
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.C, null, false, false);
			yield return this.ForbidAndNotifyMentalStateToil();
			yield break;
		}

		// Token: 0x060046E7 RID: 18151 RVA: 0x00033B76 File Offset: 0x00031D76
		private Toil FindCellToDropCorpseToil()
		{
			return new Toil
			{
				initAction = delegate()
				{
					IntVec3 c = IntVec3.Invalid;
					if (!Rand.Chance(0.8f) || !this.TryFindTableCell(out c))
					{
						bool flag = false;
						IntVec3 root;
						if (RCellFinder.TryFindRandomSpotJustOutsideColony(this.pawn, out root) && CellFinder.TryRandomClosewalkCellNear(root, this.pawn.Map, 5, out c, (IntVec3 x) => this.pawn.CanReserve(x, 1, -1, null, false) && x.GetFirstItem(this.pawn.Map) == null))
						{
							flag = true;
						}
						if (!flag)
						{
							c = CellFinder.RandomClosewalkCellNear(this.pawn.Position, this.pawn.Map, 10, (IntVec3 x) => this.pawn.CanReserve(x, 1, -1, null, false) && x.GetFirstItem(this.pawn.Map) == null);
						}
					}
					this.job.SetTarget(TargetIndex.C, c);
				},
				atomicWithPrevious = true
			};
		}

		// Token: 0x060046E8 RID: 18152 RVA: 0x00033B96 File Offset: 0x00031D96
		private Toil ForbidAndNotifyMentalStateToil()
		{
			return new Toil
			{
				initAction = delegate()
				{
					Corpse corpse = this.Corpse;
					if (corpse != null)
					{
						corpse.SetForbidden(true, true);
					}
					MentalState_CorpseObsession mentalState_CorpseObsession = this.pawn.MentalState as MentalState_CorpseObsession;
					if (mentalState_CorpseObsession != null)
					{
						mentalState_CorpseObsession.Notify_CorpseHauled();
					}
				},
				atomicWithPrevious = true
			};
		}

		// Token: 0x060046E9 RID: 18153 RVA: 0x00196558 File Offset: 0x00194758
		private bool TryFindTableCell(out IntVec3 cell)
		{
			JobDriver_HaulCorpseToPublicPlace.tmpCells.Clear();
			List<Building> allBuildingsColonist = this.pawn.Map.listerBuildings.allBuildingsColonist;
			for (int i = 0; i < allBuildingsColonist.Count; i++)
			{
				Building building = allBuildingsColonist[i];
				if (building.def.IsTable)
				{
					foreach (IntVec3 intVec in building.OccupiedRect())
					{
						if (this.pawn.CanReserveAndReach(intVec, PathEndMode.OnCell, Danger.Deadly, 1, -1, null, false) && intVec.GetFirstItem(this.pawn.Map) == null)
						{
							JobDriver_HaulCorpseToPublicPlace.tmpCells.Add(intVec);
						}
					}
				}
			}
			if (!JobDriver_HaulCorpseToPublicPlace.tmpCells.Any<IntVec3>())
			{
				cell = IntVec3.Invalid;
				return false;
			}
			cell = JobDriver_HaulCorpseToPublicPlace.tmpCells.RandomElement<IntVec3>();
			return true;
		}

		// Token: 0x04002F92 RID: 12178
		private const TargetIndex CorpseInd = TargetIndex.A;

		// Token: 0x04002F93 RID: 12179
		private const TargetIndex GraveInd = TargetIndex.B;

		// Token: 0x04002F94 RID: 12180
		private const TargetIndex CellInd = TargetIndex.C;

		// Token: 0x04002F95 RID: 12181
		private static List<IntVec3> tmpCells = new List<IntVec3>();
	}
}
