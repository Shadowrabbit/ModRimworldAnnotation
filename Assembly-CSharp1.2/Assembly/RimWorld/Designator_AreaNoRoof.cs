using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001993 RID: 6547
	public class Designator_AreaNoRoof : Designator_Area
	{
		// Token: 0x170016E8 RID: 5864
		// (get) Token: 0x060090B7 RID: 37047 RVA: 0x0001B6B4 File Offset: 0x000198B4
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170016E9 RID: 5865
		// (get) Token: 0x060090B8 RID: 37048 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060090B9 RID: 37049 RVA: 0x0029AC74 File Offset: 0x00298E74
		public Designator_AreaNoRoof()
		{
			this.defaultLabel = "DesignatorAreaNoRoofExpand".Translate();
			this.defaultDesc = "DesignatorAreaNoRoofExpandDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/NoRoofArea", true);
			this.hotKey = KeyBindingDefOf.Misc5;
			this.soundDragSustain = SoundDefOf.Designate_DragAreaAdd;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_ZoneAdd;
			this.useMouseIcon = true;
		}

		// Token: 0x060090BA RID: 37050 RVA: 0x0029ACF4 File Offset: 0x00298EF4
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (c.Fogged(base.Map))
			{
				return false;
			}
			RoofDef roofDef = base.Map.roofGrid.RoofAt(c);
			if (roofDef != null && roofDef.isThickRoof)
			{
				return "MessageNothingCanRemoveThickRoofs".Translate();
			}
			return !base.Map.areaManager.NoRoof[c];
		}

		// Token: 0x060090BB RID: 37051 RVA: 0x000611FA File Offset: 0x0005F3FA
		public override void DesignateSingleCell(IntVec3 c)
		{
			base.Map.areaManager.NoRoof[c] = true;
			Designator_AreaNoRoof.justAddedCells.Add(c);
		}

		// Token: 0x060090BC RID: 37052 RVA: 0x0029AD78 File Offset: 0x00298F78
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			for (int i = 0; i < Designator_AreaNoRoof.justAddedCells.Count; i++)
			{
				base.Map.areaManager.BuildRoof[Designator_AreaNoRoof.justAddedCells[i]] = false;
			}
			Designator_AreaNoRoof.justAddedCells.Clear();
		}

		// Token: 0x060090BD RID: 37053 RVA: 0x000610FA File Offset: 0x0005F2FA
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			base.Map.areaManager.NoRoof.MarkForDraw();
			base.Map.areaManager.BuildRoof.MarkForDraw();
		}

		// Token: 0x04005C04 RID: 23556
		private static List<IntVec3> justAddedCells = new List<IntVec3>();
	}
}
