using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013C7 RID: 5063
	public class Instruction_DownRaider : Lesson_Instruction
	{
		// Token: 0x06007B21 RID: 31521 RVA: 0x002B78CA File Offset: 0x002B5ACA
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<IntVec3>(ref this.coverCells, "coverCells", LookMode.Undefined, Array.Empty<object>());
		}

		// Token: 0x06007B22 RID: 31522 RVA: 0x002B78E8 File Offset: 0x002B5AE8
		public override void OnActivated()
		{
			base.OnActivated();
			CellRect cellRect = Find.TutorialState.sandbagsRect.ContractedBy(1);
			this.coverCells = new List<IntVec3>();
			foreach (IntVec3 intVec in cellRect.EdgeCells)
			{
				if (intVec.x != cellRect.CenterCell.x && intVec.z != cellRect.CenterCell.z)
				{
					this.coverCells.Add(intVec);
				}
			}
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.target = base.Map;
			incidentParms.points = PawnKindDefOf.Drifter.combatPower;
			incidentParms.raidArrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn;
			incidentParms.raidStrategy = RaidStrategyDefOf.ImmediateAttack;
			incidentParms.raidForceOneIncap = true;
			incidentParms.raidNeverFleeIndividual = true;
			IncidentDefOf.RaidEnemy.Worker.TryExecute(incidentParms);
		}

		// Token: 0x06007B23 RID: 31523 RVA: 0x002B79DC File Offset: 0x002B5BDC
		private bool AllColonistsInCover()
		{
			foreach (Pawn pawn in base.Map.mapPawns.FreeColonistsSpawned)
			{
				if (!this.coverCells.Contains(pawn.Position))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06007B24 RID: 31524 RVA: 0x002B7A4C File Offset: 0x002B5C4C
		public override void LessonOnGUI()
		{
			if (!this.AllColonistsInCover())
			{
				TutorUtility.DrawCellRectOnGUI(Find.TutorialState.sandbagsRect, this.def.onMapInstruction);
			}
			base.LessonOnGUI();
		}

		// Token: 0x06007B25 RID: 31525 RVA: 0x002B7A78 File Offset: 0x002B5C78
		public override void LessonUpdate()
		{
			if (!this.AllColonistsInCover())
			{
				for (int i = 0; i < this.coverCells.Count; i++)
				{
					Vector3 position = this.coverCells[i].ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays);
					Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, GenDraw.InteractionCellMaterial, 0);
				}
			}
			if (base.Map.mapPawns.PawnsInFaction(Faction.OfPlayer).Any((Pawn p) => p.Downed))
			{
				foreach (Pawn pawn in base.Map.mapPawns.AllPawns)
				{
					if (pawn.HostileTo(Faction.OfPlayer))
					{
						HealthUtility.DamageUntilDowned(pawn, true);
					}
				}
			}
			if ((from p in base.Map.mapPawns.AllPawnsSpawned
			where p.HostileTo(Faction.OfPlayer) && !p.Downed
			select p).Count<Pawn>() == 0)
			{
				Find.ActiveLesson.Deactivate();
			}
		}

		// Token: 0x0400443E RID: 17470
		private List<IntVec3> coverCells;
	}
}
