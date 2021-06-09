using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200198F RID: 6543
	public abstract class Designator_AreaHome : Designator_Area
	{
		// Token: 0x170016E4 RID: 5860
		// (get) Token: 0x060090A8 RID: 37032 RVA: 0x0001B6B4 File Offset: 0x000198B4
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170016E5 RID: 5861
		// (get) Token: 0x060090A9 RID: 37033 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060090AA RID: 37034 RVA: 0x0006112B File Offset: 0x0005F32B
		public Designator_AreaHome(DesignateMode mode)
		{
			this.mode = mode;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.hotKey = KeyBindingDefOf.Misc7;
		}

		// Token: 0x060090AB RID: 37035 RVA: 0x0029AA34 File Offset: 0x00298C34
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			bool flag = base.Map.areaManager.Home[c];
			if (this.mode == DesignateMode.Add)
			{
				return !flag;
			}
			return flag;
		}

		// Token: 0x060090AC RID: 37036 RVA: 0x00061162 File Offset: 0x0005F362
		public override void DesignateSingleCell(IntVec3 c)
		{
			if (this.mode == DesignateMode.Add)
			{
				base.Map.areaManager.Home[c] = true;
				return;
			}
			base.Map.areaManager.Home[c] = false;
		}

		// Token: 0x060090AD RID: 37037 RVA: 0x0006119B File Offset: 0x0005F39B
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.HomeArea, KnowledgeAmount.Total);
		}

		// Token: 0x060090AE RID: 37038 RVA: 0x000611AE File Offset: 0x0005F3AE
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			base.Map.areaManager.Home.MarkForDraw();
		}

		// Token: 0x04005C03 RID: 23555
		private DesignateMode mode;
	}
}
