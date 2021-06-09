using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001992 RID: 6546
	public class Designator_AreaIgnoreRoof : Designator_Area
	{
		// Token: 0x170016E6 RID: 5862
		// (get) Token: 0x060090B1 RID: 37041 RVA: 0x0001B6B4 File Offset: 0x000198B4
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170016E7 RID: 5863
		// (get) Token: 0x060090B2 RID: 37042 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060090B3 RID: 37043 RVA: 0x0029AB84 File Offset: 0x00298D84
		public Designator_AreaIgnoreRoof()
		{
			this.defaultLabel = "DesignatorAreaIgnoreRoofExpand".Translate();
			this.defaultDesc = "DesignatorAreaIgnoreRoofExpandDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/IgnoreRoofArea", true);
			this.hotKey = KeyBindingDefOf.Misc11;
			this.soundDragSustain = SoundDefOf.Designate_DragAreaAdd;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_ZoneAdd;
			this.useMouseIcon = true;
		}

		// Token: 0x060090B4 RID: 37044 RVA: 0x0029AC04 File Offset: 0x00298E04
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
			return base.Map.areaManager.BuildRoof[c] || base.Map.areaManager.NoRoof[c];
		}

		// Token: 0x060090B5 RID: 37045 RVA: 0x000611CA File Offset: 0x0005F3CA
		public override void DesignateSingleCell(IntVec3 c)
		{
			base.Map.areaManager.BuildRoof[c] = false;
			base.Map.areaManager.NoRoof[c] = false;
		}

		// Token: 0x060090B6 RID: 37046 RVA: 0x000610FA File Offset: 0x0005F2FA
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			base.Map.areaManager.NoRoof.MarkForDraw();
			base.Map.areaManager.BuildRoof.MarkForDraw();
		}
	}
}
