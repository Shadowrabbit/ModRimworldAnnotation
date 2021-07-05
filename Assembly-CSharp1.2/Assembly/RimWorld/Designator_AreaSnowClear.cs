using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001994 RID: 6548
	public abstract class Designator_AreaSnowClear : Designator_Area
	{
		// Token: 0x170016EA RID: 5866
		// (get) Token: 0x060090BF RID: 37055 RVA: 0x0001B6B4 File Offset: 0x000198B4
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170016EB RID: 5867
		// (get) Token: 0x060090C0 RID: 37056 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060090C1 RID: 37057 RVA: 0x0029ADCC File Offset: 0x00298FCC
		public Designator_AreaSnowClear(DesignateMode mode)
		{
			this.mode = mode;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.hotKey = KeyBindingDefOf.Misc7;
			this.tutorTag = "AreaSnowClear";
		}

		// Token: 0x060090C2 RID: 37058 RVA: 0x0029AE1C File Offset: 0x0029901C
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

		// Token: 0x060090C3 RID: 37059 RVA: 0x0006122A File Offset: 0x0005F42A
		public override void DesignateSingleCell(IntVec3 c)
		{
			if (this.mode == DesignateMode.Add)
			{
				base.Map.areaManager.SnowClear[c] = true;
				return;
			}
			base.Map.areaManager.SnowClear[c] = false;
		}

		// Token: 0x060090C4 RID: 37060 RVA: 0x00061263 File Offset: 0x0005F463
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			base.Map.areaManager.SnowClear.MarkForDraw();
		}

		// Token: 0x04005C05 RID: 23557
		private DesignateMode mode;
	}
}
