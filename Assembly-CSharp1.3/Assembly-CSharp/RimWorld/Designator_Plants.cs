using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020012D2 RID: 4818
	public abstract class Designator_Plants : Designator
	{
		// Token: 0x1700142E RID: 5166
		// (get) Token: 0x06007329 RID: 29481 RVA: 0x0009007E File Offset: 0x0008E27E
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x1700142F RID: 5167
		// (get) Token: 0x0600732A RID: 29482 RVA: 0x0026730B File Offset: 0x0026550B
		protected override DesignationDef Designation
		{
			get
			{
				return this.designationDef;
			}
		}

		// Token: 0x0600732B RID: 29483 RVA: 0x00260D22 File Offset: 0x0025EF22
		public Designator_Plants()
		{
		}

		// Token: 0x0600732C RID: 29484 RVA: 0x00267313 File Offset: 0x00265513
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			if (t.def.plant == null)
			{
				return false;
			}
			if (base.Map.designationManager.DesignationOn(t, this.designationDef) != null)
			{
				return false;
			}
			return true;
		}

		// Token: 0x0600732D RID: 29485 RVA: 0x00267350 File Offset: 0x00265550
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map) || c.Fogged(base.Map))
			{
				return false;
			}
			Plant plant = c.GetPlant(base.Map);
			if (plant == null)
			{
				return "MessageMustDesignatePlants".Translate();
			}
			AcceptanceReport result = this.CanDesignateThing(plant);
			if (!result.Accepted)
			{
				return result;
			}
			return true;
		}

		// Token: 0x0600732E RID: 29486 RVA: 0x002673B9 File Offset: 0x002655B9
		public override void DesignateSingleCell(IntVec3 c)
		{
			this.DesignateThing(c.GetPlant(base.Map));
		}

		// Token: 0x0600732F RID: 29487 RVA: 0x002673CD File Offset: 0x002655CD
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.RemoveAllDesignationsOn(t, false);
			base.Map.designationManager.AddDesignation(new Designation(t, this.designationDef));
		}

		// Token: 0x06007330 RID: 29488 RVA: 0x00261B2D File Offset: 0x0025FD2D
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}

		// Token: 0x04003ED7 RID: 16087
		protected DesignationDef designationDef;
	}
}
