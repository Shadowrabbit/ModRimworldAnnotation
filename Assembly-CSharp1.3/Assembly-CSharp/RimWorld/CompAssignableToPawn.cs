using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001101 RID: 4353
	public class CompAssignableToPawn : ThingComp
	{
		// Token: 0x170011D3 RID: 4563
		// (get) Token: 0x06006874 RID: 26740 RVA: 0x00234B2B File Offset: 0x00232D2B
		public CompProperties_AssignableToPawn Props
		{
			get
			{
				return (CompProperties_AssignableToPawn)this.props;
			}
		}

		// Token: 0x170011D4 RID: 4564
		// (get) Token: 0x06006875 RID: 26741 RVA: 0x00234B38 File Offset: 0x00232D38
		public int MaxAssignedPawnsCount
		{
			get
			{
				return this.Props.maxAssignedPawnsCount;
			}
		}

		// Token: 0x170011D5 RID: 4565
		// (get) Token: 0x06006876 RID: 26742 RVA: 0x00234B48 File Offset: 0x00232D48
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

		// Token: 0x170011D6 RID: 4566
		// (get) Token: 0x06006877 RID: 26743 RVA: 0x00234BB2 File Offset: 0x00232DB2
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

		// Token: 0x170011D7 RID: 4567
		// (get) Token: 0x06006878 RID: 26744 RVA: 0x00234BDC File Offset: 0x00232DDC
		public List<Pawn> AssignedPawnsForReading
		{
			get
			{
				return this.assignedPawns;
			}
		}

		// Token: 0x170011D8 RID: 4568
		// (get) Token: 0x06006879 RID: 26745 RVA: 0x00234BDC File Offset: 0x00232DDC
		public IEnumerable<Pawn> AssignedPawns
		{
			get
			{
				return this.assignedPawns;
			}
		}

		// Token: 0x170011D9 RID: 4569
		// (get) Token: 0x0600687A RID: 26746 RVA: 0x00234BE4 File Offset: 0x00232DE4
		public bool HasFreeSlot
		{
			get
			{
				return this.assignedPawns.Count < this.Props.maxAssignedPawnsCount;
			}
		}

		// Token: 0x170011DA RID: 4570
		// (get) Token: 0x0600687B RID: 26747 RVA: 0x00234B38 File Offset: 0x00232D38
		public int TotalSlots
		{
			get
			{
				return this.Props.maxAssignedPawnsCount;
			}
		}

		// Token: 0x0600687C RID: 26748 RVA: 0x000126F5 File Offset: 0x000108F5
		protected virtual bool CanDrawOverlayForPawn(Pawn pawn)
		{
			return true;
		}

		// Token: 0x0600687D RID: 26749 RVA: 0x00234C00 File Offset: 0x00232E00
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

		// Token: 0x0600687E RID: 26750 RVA: 0x00234CB7 File Offset: 0x00232EB7
		protected virtual void SortAssignedPawns()
		{
			this.assignedPawns.SortBy((Pawn x) => x.thingIDNumber);
		}

		// Token: 0x0600687F RID: 26751 RVA: 0x00234CE3 File Offset: 0x00232EE3
		public virtual void ForceAddPawn(Pawn pawn)
		{
			if (!this.assignedPawns.Contains(pawn))
			{
				this.assignedPawns.Add(pawn);
			}
			this.SortAssignedPawns();
		}

		// Token: 0x06006880 RID: 26752 RVA: 0x00234D05 File Offset: 0x00232F05
		public virtual void ForceRemovePawn(Pawn pawn)
		{
			if (this.assignedPawns.Contains(pawn))
			{
				this.assignedPawns.Remove(pawn);
			}
			this.SortAssignedPawns();
		}

		// Token: 0x06006881 RID: 26753 RVA: 0x0004EE58 File Offset: 0x0004D058
		public virtual AcceptanceReport CanAssignTo(Pawn pawn)
		{
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x06006882 RID: 26754 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool IdeoligionForbids(Pawn pawn)
		{
			return false;
		}

		// Token: 0x06006883 RID: 26755 RVA: 0x00234D28 File Offset: 0x00232F28
		public virtual void TryAssignPawn(Pawn pawn)
		{
			if (this.assignedPawns.Contains(pawn))
			{
				return;
			}
			this.assignedPawns.Add(pawn);
			this.SortAssignedPawns();
		}

		// Token: 0x06006884 RID: 26756 RVA: 0x00234D4B File Offset: 0x00232F4B
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

		// Token: 0x06006885 RID: 26757 RVA: 0x00234D72 File Offset: 0x00232F72
		public virtual bool AssignedAnything(Pawn pawn)
		{
			return this.assignedPawns.Contains(pawn);
		}

		// Token: 0x06006886 RID: 26758 RVA: 0x00234D80 File Offset: 0x00232F80
		protected virtual bool ShouldShowAssignmentGizmo()
		{
			return this.parent.Faction == Faction.OfPlayer;
		}

		// Token: 0x06006887 RID: 26759 RVA: 0x00234D94 File Offset: 0x00232F94
		protected virtual string GetAssignmentGizmoLabel()
		{
			return "CommandThingSetOwnerLabel".Translate();
		}

		// Token: 0x06006888 RID: 26760 RVA: 0x00014F75 File Offset: 0x00013175
		protected virtual string GetAssignmentGizmoDesc()
		{
			return "";
		}

		// Token: 0x06006889 RID: 26761 RVA: 0x00234DA5 File Offset: 0x00232FA5
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

		// Token: 0x0600688A RID: 26762 RVA: 0x00234DB8 File Offset: 0x00232FB8
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Collections.Look<Pawn>(ref this.assignedPawns, "assignedPawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.assignedPawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x0600688B RID: 26763 RVA: 0x00234E14 File Offset: 0x00233014
		public override void PostDeSpawn(Map map)
		{
			for (int i = this.assignedPawns.Count - 1; i >= 0; i--)
			{
				this.TryUnassignPawn(this.assignedPawns[i], false);
			}
		}

		// Token: 0x04003A97 RID: 14999
		protected List<Pawn> assignedPawns = new List<Pawn>();
	}
}
