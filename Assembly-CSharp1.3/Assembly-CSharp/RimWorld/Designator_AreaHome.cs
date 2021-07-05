using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020012A0 RID: 4768
	public abstract class Designator_AreaHome : Designator_Area
	{
		// Token: 0x170013DE RID: 5086
		// (get) Token: 0x060071E3 RID: 29155 RVA: 0x0009007E File Offset: 0x0008E27E
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170013DF RID: 5087
		// (get) Token: 0x060071E4 RID: 29156 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060071E5 RID: 29157 RVA: 0x0026114D File Offset: 0x0025F34D
		public Designator_AreaHome(DesignateMode mode)
		{
			this.mode = mode;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.hotKey = KeyBindingDefOf.Misc7;
		}

		// Token: 0x060071E6 RID: 29158 RVA: 0x00261184 File Offset: 0x0025F384
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

		// Token: 0x060071E7 RID: 29159 RVA: 0x002611D5 File Offset: 0x0025F3D5
		public override void DesignateSingleCell(IntVec3 c)
		{
			if (this.mode == DesignateMode.Add)
			{
				base.Map.areaManager.Home[c] = true;
				return;
			}
			base.Map.areaManager.Home[c] = false;
		}

		// Token: 0x060071E8 RID: 29160 RVA: 0x0026120E File Offset: 0x0025F40E
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.HomeArea, KnowledgeAmount.Total);
		}

		// Token: 0x060071E9 RID: 29161 RVA: 0x00261221 File Offset: 0x0025F421
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			base.Map.areaManager.Home.MarkForDraw();
		}

		// Token: 0x04003EB7 RID: 16055
		private DesignateMode mode;
	}
}
