using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000719 RID: 1817
	public class JobDriver_HaulCorpseToPublicPlace : JobDriver
	{
		// Token: 0x17000963 RID: 2403
		// (get) Token: 0x06003271 RID: 12913 RVA: 0x001229EC File Offset: 0x00120BEC
		private Corpse Corpse
		{
			get
			{
				return (Corpse)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000964 RID: 2404
		// (get) Token: 0x06003272 RID: 12914 RVA: 0x00122A14 File Offset: 0x00120C14
		private Building_Grave Grave
		{
			get
			{
				return (Building_Grave)this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x17000965 RID: 2405
		// (get) Token: 0x06003273 RID: 12915 RVA: 0x00122A3A File Offset: 0x00120C3A
		private bool InGrave
		{
			get
			{
				return this.Grave != null;
			}
		}

		// Token: 0x17000966 RID: 2406
		// (get) Token: 0x06003274 RID: 12916 RVA: 0x00122A45 File Offset: 0x00120C45
		private Thing Target
		{
			get
			{
				return this.Grave ?? this.Corpse;
			}
		}

		// Token: 0x06003275 RID: 12917 RVA: 0x00122A57 File Offset: 0x00120C57
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Target, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06003276 RID: 12918 RVA: 0x00122A79 File Offset: 0x00120C79
		public override string GetReport()
		{
			if (this.InGrave && this.Grave.def == ThingDefOf.Grave)
			{
				return "ReportDiggingUpCorpse".Translate();
			}
			return base.GetReport();
		}

		// Token: 0x06003277 RID: 12919 RVA: 0x00122AAB File Offset: 0x00120CAB
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			Toil gotoCorpse = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Jump.JumpIfTargetInvalid(TargetIndex.B, gotoCorpse);
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.InteractionCell).FailOnDespawnedOrNull(TargetIndex.B);
			yield return Toils_General.Wait(this.Grave.OpenTicks, TargetIndex.None).WithProgressBarToilDelay(TargetIndex.B, false, -0.5f).FailOnDespawnedOrNull(TargetIndex.B).FailOnCannotTouch(TargetIndex.B, PathEndMode.InteractionCell);
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

		// Token: 0x06003278 RID: 12920 RVA: 0x00122ABB File Offset: 0x00120CBB
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

		// Token: 0x06003279 RID: 12921 RVA: 0x00122ADB File Offset: 0x00120CDB
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

		// Token: 0x0600327A RID: 12922 RVA: 0x00122AFC File Offset: 0x00120CFC
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

		// Token: 0x04001DC3 RID: 7619
		private const TargetIndex CorpseInd = TargetIndex.A;

		// Token: 0x04001DC4 RID: 7620
		private const TargetIndex GraveInd = TargetIndex.B;

		// Token: 0x04001DC5 RID: 7621
		private const TargetIndex CellInd = TargetIndex.C;

		// Token: 0x04001DC6 RID: 7622
		private static List<IntVec3> tmpCells = new List<IntVec3>();
	}
}
