using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012A3 RID: 4771
	public class Designator_AreaIgnoreRoof : Designator_Area
	{
		// Token: 0x170013E0 RID: 5088
		// (get) Token: 0x060071EC RID: 29164 RVA: 0x0009007E File Offset: 0x0008E27E
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170013E1 RID: 5089
		// (get) Token: 0x060071ED RID: 29165 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060071EE RID: 29166 RVA: 0x0026133C File Offset: 0x0025F53C
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

		// Token: 0x060071EF RID: 29167 RVA: 0x002613BC File Offset: 0x0025F5BC
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

		// Token: 0x060071F0 RID: 29168 RVA: 0x00261429 File Offset: 0x0025F629
		public override void DesignateSingleCell(IntVec3 c)
		{
			base.Map.areaManager.BuildRoof[c] = false;
			base.Map.areaManager.NoRoof[c] = false;
		}

		// Token: 0x060071F1 RID: 29169 RVA: 0x0026111C File Offset: 0x0025F31C
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			base.Map.areaManager.NoRoof.MarkForDraw();
			base.Map.areaManager.BuildRoof.MarkForDraw();
		}
	}
}
