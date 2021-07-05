using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020012A5 RID: 4773
	public abstract class Designator_AreaSnowClear : Designator_Area
	{
		// Token: 0x170013E4 RID: 5092
		// (get) Token: 0x060071FA RID: 29178 RVA: 0x0009007E File Offset: 0x0008E27E
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170013E5 RID: 5093
		// (get) Token: 0x060071FB RID: 29179 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060071FC RID: 29180 RVA: 0x002615E4 File Offset: 0x0025F7E4
		public Designator_AreaSnowClear(DesignateMode mode)
		{
			this.mode = mode;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.hotKey = KeyBindingDefOf.Misc7;
			this.tutorTag = "AreaSnowClear";
		}

		// Token: 0x060071FD RID: 29181 RVA: 0x00261634 File Offset: 0x0025F834
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			bool flag = base.Map.areaManager.SnowClear[c];
			if (this.mode == DesignateMode.Add)
			{
				return !flag;
			}
			return flag;
		}

		// Token: 0x060071FE RID: 29182 RVA: 0x00261685 File Offset: 0x0025F885
		public override void DesignateSingleCell(IntVec3 c)
		{
			if (this.mode == DesignateMode.Add)
			{
				base.Map.areaManager.SnowClear[c] = true;
				return;
			}
			base.Map.areaManager.SnowClear[c] = false;
		}

		// Token: 0x060071FF RID: 29183 RVA: 0x002616BE File Offset: 0x0025F8BE
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			base.Map.areaManager.SnowClear.MarkForDraw();
		}

		// Token: 0x04003EB9 RID: 16057
		private DesignateMode mode;
	}
}
