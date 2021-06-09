using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020019C4 RID: 6596
	public class Designator_Install : Designator_Place
	{
		// Token: 0x17001723 RID: 5923
		// (get) Token: 0x060091DA RID: 37338 RVA: 0x0029E7F8 File Offset: 0x0029C9F8
		private Thing MiniToInstallOrBuildingToReinstall
		{
			get
			{
				Thing singleSelectedThing = Find.Selector.SingleSelectedThing;
				if (singleSelectedThing is MinifiedThing)
				{
					return singleSelectedThing;
				}
				Building building = singleSelectedThing as Building;
				if (building != null && building.def.Minifiable)
				{
					return singleSelectedThing;
				}
				return null;
			}
		}

		// Token: 0x17001724 RID: 5924
		// (get) Token: 0x060091DB RID: 37339 RVA: 0x00061AF7 File Offset: 0x0005FCF7
		private Thing ThingToInstall
		{
			get
			{
				return this.MiniToInstallOrBuildingToReinstall.GetInnerIfMinified();
			}
		}

		// Token: 0x17001725 RID: 5925
		// (get) Token: 0x060091DC RID: 37340 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool DoTooltip
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001726 RID: 5926
		// (get) Token: 0x060091DD RID: 37341 RVA: 0x00061B04 File Offset: 0x0005FD04
		public override BuildableDef PlacingDef
		{
			get
			{
				return this.ThingToInstall.def;
			}
		}

		// Token: 0x17001727 RID: 5927
		// (get) Token: 0x060091DE RID: 37342 RVA: 0x00061B11 File Offset: 0x0005FD11
		public override string Label
		{
			get
			{
				if (this.MiniToInstallOrBuildingToReinstall is MinifiedThing)
				{
					return "CommandInstall".Translate();
				}
				return "CommandReinstall".Translate();
			}
		}

		// Token: 0x17001728 RID: 5928
		// (get) Token: 0x060091DF RID: 37343 RVA: 0x00061B3F File Offset: 0x0005FD3F
		public override string Desc
		{
			get
			{
				if (this.MiniToInstallOrBuildingToReinstall is MinifiedThing)
				{
					return "CommandInstallDesc".Translate();
				}
				return "CommandReinstallDesc".Translate();
			}
		}

		// Token: 0x17001729 RID: 5929
		// (get) Token: 0x060091E0 RID: 37344 RVA: 0x0000BBC0 File Offset: 0x00009DC0
		public override Color IconDrawColor
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x1700172A RID: 5930
		// (get) Token: 0x060091E1 RID: 37345 RVA: 0x00061B6D File Offset: 0x0005FD6D
		public override bool Visible
		{
			get
			{
				return Find.Selector.SingleSelectedThing != null && base.Visible;
			}
		}

		// Token: 0x060091E2 RID: 37346 RVA: 0x00061B83 File Offset: 0x0005FD83
		public Designator_Install()
		{
			this.icon = TexCommand.Install;
			this.iconProportions = new Vector2(1f, 1f);
			this.order = -10f;
		}

		// Token: 0x060091E3 RID: 37347 RVA: 0x00061BB6 File Offset: 0x0005FDB6
		public override bool CanRemainSelected()
		{
			return this.MiniToInstallOrBuildingToReinstall != null;
		}

		// Token: 0x060091E4 RID: 37348 RVA: 0x0029E834 File Offset: 0x0029CA34
		public override void ProcessInput(Event ev)
		{
			Thing miniToInstallOrBuildingToReinstall = this.MiniToInstallOrBuildingToReinstall;
			if (miniToInstallOrBuildingToReinstall != null)
			{
				InstallBlueprintUtility.CancelBlueprintsFor(miniToInstallOrBuildingToReinstall);
				if (!((ThingDef)this.PlacingDef).rotatable)
				{
					this.placingRot = Rot4.North;
				}
			}
			base.ProcessInput(ev);
		}

		// Token: 0x060091E5 RID: 37349 RVA: 0x0029E878 File Offset: 0x0029CA78
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (!(this.MiniToInstallOrBuildingToReinstall is MinifiedThing) && c.GetThingList(base.Map).Find((Thing x) => x.Position == c && x.Rotation == this.placingRot && x.def == this.PlacingDef) != null)
			{
				return new AcceptanceReport("IdenticalThingExists".Translate());
			}
			return GenConstruct.CanPlaceBlueprintAt(this.PlacingDef, c, this.placingRot, base.Map, false, this.MiniToInstallOrBuildingToReinstall, this.ThingToInstall, null);
		}

		// Token: 0x060091E6 RID: 37350 RVA: 0x0029E924 File Offset: 0x0029CB24
		public override void DesignateSingleCell(IntVec3 c)
		{
			GenSpawn.WipeExistingThings(c, this.placingRot, this.PlacingDef.installBlueprintDef, base.Map, DestroyMode.Deconstruct);
			MinifiedThing minifiedThing = this.MiniToInstallOrBuildingToReinstall as MinifiedThing;
			if (minifiedThing != null)
			{
				GenConstruct.PlaceBlueprintForInstall(minifiedThing, c, base.Map, this.placingRot, Faction.OfPlayer);
			}
			else
			{
				GenConstruct.PlaceBlueprintForReinstall((Building)this.MiniToInstallOrBuildingToReinstall, c, base.Map, this.placingRot, Faction.OfPlayer);
			}
			MoteMaker.ThrowMetaPuffs(GenAdj.OccupiedRect(c, this.placingRot, this.PlacingDef.Size), base.Map);
			Find.DesignatorManager.Deselect();
		}

		// Token: 0x060091E7 RID: 37351 RVA: 0x0029E9C8 File Offset: 0x0029CBC8
		protected override void DrawGhost(Color ghostCol)
		{
			ThingDef def;
			if ((def = (this.PlacingDef as ThingDef)) != null)
			{
				MeditationUtility.DrawMeditationFociAffectedByBuildingOverlay(base.Map, def, Faction.OfPlayer, UI.MouseCell(), this.placingRot);
			}
			Graphic baseGraphic = this.ThingToInstall.Graphic.ExtractInnerGraphicFor(this.ThingToInstall);
			GhostDrawer.DrawGhostThing_NewTmp(UI.MouseCell(), this.placingRot, (ThingDef)this.PlacingDef, baseGraphic, ghostCol, AltitudeLayer.Blueprint, this.ThingToInstall, true);
		}

		// Token: 0x060091E8 RID: 37352 RVA: 0x00061BC1 File Offset: 0x0005FDC1
		protected override bool CanDrawNumbersBetween(Thing thing, ThingDef def, IntVec3 a, IntVec3 b, Map map)
		{
			return this.ThingToInstall != thing && !GenThing.CloserThingBetween(def, a, b, map, this.ThingToInstall);
		}

		// Token: 0x060091E9 RID: 37353 RVA: 0x00061BE2 File Offset: 0x0005FDE2
		public override void SelectedUpdate()
		{
			base.SelectedUpdate();
			BuildDesignatorUtility.TryDrawPowerGridAndAnticipatedConnection(this.PlacingDef, this.placingRot);
		}
	}
}
