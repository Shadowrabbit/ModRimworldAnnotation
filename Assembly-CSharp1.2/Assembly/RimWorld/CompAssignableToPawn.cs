using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001796 RID: 6038
	public class CompAssignableToPawn : ThingComp
	{
		// Token: 0x1700149D RID: 5277
		// (get) Token: 0x0600855B RID: 34139 RVA: 0x0005965D File Offset: 0x0005785D
		public CompProperties_AssignableToPawn Props
		{
			get
			{
				return (CompProperties_AssignableToPawn)this.props;
			}
		}

		// Token: 0x1700149E RID: 5278
		// (get) Token: 0x0600855C RID: 34140 RVA: 0x0005966A File Offset: 0x0005786A
		public int MaxAssignedPawnsCount
		{
			get
			{
				return this.Props.maxAssignedPawnsCount;
			}
		}

		// Token: 0x1700149F RID: 5279
		// (get) Token: 0x0600855D RID: 34141 RVA: 0x002760E8 File Offset: 0x002742E8
		public bool PlayerCanSeeAssignments
		{
			get
			{
				if (this.parent.Faction == Faction.OfPlayer)
				{
					return true;
				}
				for (int i = 0; i < this.assignedPawns.Count; i++)
				{
					if (this.assignedPawns[i].Faction == Faction.OfPlayer || this.assignedPawns[i].HostFaction == Faction.OfPlayer)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x170014A0 RID: 5280
		// (get) Token: 0x0600855E RID: 34142 RVA: 0x00059677 File Offset: 0x00057877
		public virtual IEnumerable<Pawn> AssigningCandidates
		{
			get
			{
				if (!this.parent.Spawned)
				{
					return Enumerable.Empty<Pawn>();
				}
				return this.parent.Map.mapPawns.FreeColonists;
			}
		}

		// Token: 0x170014A1 RID: 5281
		// (get) Token: 0x0600855F RID: 34143 RVA: 0x000596A1 File Offset: 0x000578A1
		public List<Pawn> AssignedPawnsForReading
		{
			get
			{
				return this.assignedPawns;
			}
		}

		// Token: 0x170014A2 RID: 5282
		// (get) Token: 0x06008560 RID: 34144 RVA: 0x000596A1 File Offset: 0x000578A1
		public IEnumerable<Pawn> AssignedPawns
		{
			get
			{
				return this.assignedPawns;
			}
		}

		// Token: 0x170014A3 RID: 5283
		// (get) Token: 0x06008561 RID: 34145 RVA: 0x000596A9 File Offset: 0x000578A9
		public bool HasFreeSlot
		{
			get
			{
				return this.assignedPawns.Count < this.Props.maxAssignedPawnsCount;
			}
		}

		// Token: 0x06008562 RID: 34146 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected virtual bool CanDrawOverlayForPawn(Pawn pawn)
		{
			return true;
		}

		// Token: 0x06008563 RID: 34147 RVA: 0x00276154 File Offset: 0x00274354
		public override void DrawGUIOverlay()
		{
			if (!this.Props.drawAssignmentOverlay || (!this.Props.drawUnownedAssignmentOverlay && !this.assignedPawns.Any<Pawn>()))
			{
				return;
			}
			if (Find.CameraDriver.CurrentZoom == CameraZoomRange.Closest && this.PlayerCanSeeAssignments)
			{
				Color defaultThingLabelColor = GenMapUI.DefaultThingLabelColor;
				if (!this.assignedPawns.Any<Pawn>())
				{
					GenMapUI.DrawThingLabel(this.parent, "Unowned".Translate(), defaultThingLabelColor);
				}
				if (this.assignedPawns.Count == 1)
				{
					if (!this.CanDrawOverlayForPawn(this.assignedPawns[0]))
					{
						return;
					}
					GenMapUI.DrawThingLabel(this.parent, this.assignedPawns[0].LabelShort, defaultThingLabelColor);
				}
			}
		}

		// Token: 0x06008564 RID: 34148 RVA: 0x000596C3 File Offset: 0x000578C3
		protected virtual void SortAssignedPawns()
		{
			this.assignedPawns.SortBy((Pawn x) => x.thingIDNumber);
		}

		// Token: 0x06008565 RID: 34149 RVA: 0x000596EF File Offset: 0x000578EF
		public virtual void ForceAddPawn(Pawn pawn)
		{
			if (!this.assignedPawns.Contains(pawn))
			{
				this.assignedPawns.Add(pawn);
			}
			this.SortAssignedPawns();
		}

		// Token: 0x06008566 RID: 34150 RVA: 0x00059711 File Offset: 0x00057911
		public virtual void ForceRemovePawn(Pawn pawn)
		{
			if (this.assignedPawns.Contains(pawn))
			{
				this.assignedPawns.Remove(pawn);
			}
			this.SortAssignedPawns();
		}

		// Token: 0x06008567 RID: 34151 RVA: 0x00012DFE File Offset: 0x00010FFE
		public virtual AcceptanceReport CanAssignTo(Pawn pawn)
		{
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x06008568 RID: 34152 RVA: 0x00059734 File Offset: 0x00057934
		public virtual void TryAssignPawn(Pawn pawn)
		{
			if (this.assignedPawns.Contains(pawn))
			{
				return;
			}
			this.assignedPawns.Add(pawn);
			this.SortAssignedPawns();
		}

		// Token: 0x06008569 RID: 34153 RVA: 0x00059757 File Offset: 0x00057957
		public virtual void TryUnassignPawn(Pawn pawn, bool sort = true)
		{
			if (!this.assignedPawns.Contains(pawn))
			{
				return;
			}
			this.assignedPawns.Remove(pawn);
			if (sort)
			{
				this.SortAssignedPawns();
			}
		}

		// Token: 0x0600856A RID: 34154 RVA: 0x0005977E File Offset: 0x0005797E
		public virtual bool AssignedAnything(Pawn pawn)
		{
			return this.assignedPawns.Contains(pawn);
		}

		// Token: 0x0600856B RID: 34155 RVA: 0x0005978C File Offset: 0x0005798C
		protected virtual bool ShouldShowAssignmentGizmo()
		{
			return this.parent.Faction == Faction.OfPlayer;
		}

		// Token: 0x0600856C RID: 34156 RVA: 0x000597A0 File Offset: 0x000579A0
		protected virtual string GetAssignmentGizmoLabel()
		{
			return "CommandThingSetOwnerLabel".Translate();
		}

		// Token: 0x0600856D RID: 34157 RVA: 0x0000A713 File Offset: 0x00008913
		protected virtual string GetAssignmentGizmoDesc()
		{
			return "";
		}

		// Token: 0x0600856E RID: 34158 RVA: 0x000597B1 File Offset: 0x000579B1
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (this.ShouldShowAssignmentGizmo())
			{
				yield return new Command_Action
				{
					defaultLabel = this.GetAssignmentGizmoLabel(),
					icon = ContentFinder<Texture2D>.Get("UI/Commands/AssignOwner", true),
					defaultDesc = this.GetAssignmentGizmoDesc(),
					action = delegate()
					{
						Find.WindowStack.Add(new Dialog_AssignBuildingOwner(this));
					},
					hotKey = KeyBindingDefOf.Misc4
				};
			}
			yield break;
		}

		// Token: 0x0600856F RID: 34159 RVA: 0x0027620C File Offset: 0x0027440C
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Collections.Look<Pawn>(ref this.assignedPawns, "assignedPawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.assignedPawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x06008570 RID: 34160 RVA: 0x00276268 File Offset: 0x00274468
		public override void PostDeSpawn(Map map)
		{
			for (int i = this.assignedPawns.Count - 1; i >= 0; i--)
			{
				this.TryUnassignPawn(this.assignedPawns[i], false);
			}
		}

		// Token: 0x04005624 RID: 22052
		protected List<Pawn> assignedPawns = new List<Pawn>();
	}
}
