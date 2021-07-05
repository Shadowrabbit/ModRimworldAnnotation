using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012A4 RID: 4772
	public class Designator_AreaNoRoof : Designator_Area
	{
		// Token: 0x170013E2 RID: 5090
		// (get) Token: 0x060071F2 RID: 29170 RVA: 0x0009007E File Offset: 0x0008E27E
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170013E3 RID: 5091
		// (get) Token: 0x060071F3 RID: 29171 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060071F4 RID: 29172 RVA: 0x0026145C File Offset: 0x0025F65C
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

		// Token: 0x060071F5 RID: 29173 RVA: 0x002614DC File Offset: 0x0025F6DC
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

		// Token: 0x060071F6 RID: 29174 RVA: 0x0026155E File Offset: 0x0025F75E
		public override void DesignateSingleCell(IntVec3 c)
		{
			base.Map.areaManager.NoRoof[c] = true;
			Designator_AreaNoRoof.justAddedCells.Add(c);
		}

		// Token: 0x060071F7 RID: 29175 RVA: 0x00261584 File Offset: 0x0025F784
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			for (int i = 0; i < Designator_AreaNoRoof.justAddedCells.Count; i++)
			{
				base.Map.areaManager.BuildRoof[Designator_AreaNoRoof.justAddedCells[i]] = false;
			}
			Designator_AreaNoRoof.justAddedCells.Clear();
		}

		// Token: 0x060071F8 RID: 29176 RVA: 0x0026111C File Offset: 0x0025F31C
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			base.Map.areaManager.NoRoof.MarkForDraw();
			base.Map.areaManager.BuildRoof.MarkForDraw();
		}

		// Token: 0x04003EB8 RID: 16056
		private static List<IntVec3> justAddedCells = new List<IntVec3>();
	}
}
