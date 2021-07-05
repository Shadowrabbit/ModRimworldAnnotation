using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012C6 RID: 4806
	public class Designator_Install : Designator_Place
	{
		// Token: 0x1700141B RID: 5147
		// (get) Token: 0x060072E5 RID: 29413 RVA: 0x00265EF4 File Offset: 0x002640F4
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

		// Token: 0x1700141C RID: 5148
		// (get) Token: 0x060072E6 RID: 29414 RVA: 0x00265F30 File Offset: 0x00264130
		private Thing ThingToInstall
		{
			get
			{
				return this.MiniToInstallOrBuildingToReinstall.GetInnerIfMinified();
			}
		}

		// Token: 0x1700141D RID: 5149
		// (get) Token: 0x060072E7 RID: 29415 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool DoTooltip
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700141E RID: 5150
		// (get) Token: 0x060072E8 RID: 29416 RVA: 0x00265F3D File Offset: 0x0026413D
		public override BuildableDef PlacingDef
		{
			get
			{
				return this.ThingToInstall.def;
			}
		}

		// Token: 0x1700141F RID: 5151
		// (get) Token: 0x060072E9 RID: 29417 RVA: 0x00265F4A File Offset: 0x0026414A
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

		// Token: 0x17001420 RID: 5152
		// (get) Token: 0x060072EA RID: 29418 RVA: 0x00265F78 File Offset: 0x00264178
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

		// Token: 0x17001421 RID: 5153
		// (get) Token: 0x060072EB RID: 29419 RVA: 0x0001A4C7 File Offset: 0x000186C7
		public override Color IconDrawColor
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x17001422 RID: 5154
		// (get) Token: 0x060072EC RID: 29420 RVA: 0x00265FA6 File Offset: 0x002641A6
		public override bool Visible
		{
			get
			{
				return Find.Selector.SingleSelectedThing != null && base.Visible;
			}
		}

		// Token: 0x17001423 RID: 5155
		// (get) Token: 0x060072ED RID: 29421 RVA: 0x00002688 File Offset: 0x00000888
		public override ThingDef StuffDef
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001424 RID: 5156
		// (get) Token: 0x060072EE RID: 29422 RVA: 0x00265FBC File Offset: 0x002641BC
		public override ThingStyleDef ThingStyleDefForPreview
		{
			get
			{
				Thing thingToInstall = this.ThingToInstall;
				if (thingToInstall == null)
				{
					return null;
				}
				return thingToInstall.StyleDef;
			}
		}

		// Token: 0x060072EF RID: 29423 RVA: 0x00265FCF File Offset: 0x002641CF
		public Designator_Install()
		{
			this.icon = TexCommand.Install;
			this.iconProportions = new Vector2(1f, 1f);
			this.order = -10f;
		}

		// Token: 0x060072F0 RID: 29424 RVA: 0x00266002 File Offset: 0x00264202
		public override bool CanRemainSelected()
		{
			return this.MiniToInstallOrBuildingToReinstall != null;
		}

		// Token: 0x060072F1 RID: 29425 RVA: 0x00266010 File Offset: 0x00264210
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

		// Token: 0x060072F2 RID: 29426 RVA: 0x00266054 File Offset: 0x00264254
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

		// Token: 0x060072F3 RID: 29427 RVA: 0x00266100 File Offset: 0x00264300
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
			FleckMaker.ThrowMetaPuffs(GenAdj.OccupiedRect(c, this.placingRot, this.PlacingDef.Size), base.Map);
			Find.DesignatorManager.Deselect();
		}

		// Token: 0x060072F4 RID: 29428 RVA: 0x002661A4 File Offset: 0x002643A4
		protected override void DrawGhost(Color ghostCol)
		{
			ThingDef def;
			if ((def = (this.PlacingDef as ThingDef)) != null)
			{
				MeditationUtility.DrawMeditationFociAffectedByBuildingOverlay(base.Map, def, Faction.OfPlayer, UI.MouseCell(), this.placingRot);
			}
			Graphic baseGraphic = this.ThingToInstall.Graphic.ExtractInnerGraphicFor(this.ThingToInstall);
			GhostDrawer.DrawGhostThing(UI.MouseCell(), this.placingRot, (ThingDef)this.PlacingDef, baseGraphic, ghostCol, AltitudeLayer.Blueprint, this.ThingToInstall, true, this.StuffDef);
		}

		// Token: 0x060072F5 RID: 29429 RVA: 0x0026621F File Offset: 0x0026441F
		protected override bool CanDrawNumbersBetween(Thing thing, ThingDef def, IntVec3 a, IntVec3 b, Map map)
		{
			return this.ThingToInstall != thing && !GenThing.CloserThingBetween(def, a, b, map, this.ThingToInstall);
		}

		// Token: 0x060072F6 RID: 29430 RVA: 0x00266240 File Offset: 0x00264440
		public override void SelectedUpdate()
		{
			base.SelectedUpdate();
			BuildDesignatorUtility.TryDrawPowerGridAndAnticipatedConnection(this.PlacingDef, this.placingRot);
		}
	}
}
