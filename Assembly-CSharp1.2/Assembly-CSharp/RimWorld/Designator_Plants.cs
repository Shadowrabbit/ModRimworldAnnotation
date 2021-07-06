using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020019D2 RID: 6610
	public abstract class Designator_Plants : Designator
	{
		// Token: 0x17001732 RID: 5938
		// (get) Token: 0x0600921C RID: 37404 RVA: 0x0001B6B4 File Offset: 0x000198B4
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17001733 RID: 5939
		// (get) Token: 0x0600921D RID: 37405 RVA: 0x00061E0C File Offset: 0x0006000C
		protected override DesignationDef Designation
		{
			get
			{
				return this.designationDef;
			}
		}

		// Token: 0x0600921E RID: 37406 RVA: 0x00060FC2 File Offset: 0x0005F1C2
		public Designator_Plants()
		{
		}

		// Token: 0x0600921F RID: 37407 RVA: 0x00061E14 File Offset: 0x00060014
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

		// Token: 0x06009220 RID: 37408 RVA: 0x0029F8A0 File Offset: 0x0029DAA0
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

		// Token: 0x06009221 RID: 37409 RVA: 0x00061E50 File Offset: 0x00060050
		public override void DesignateSingleCell(IntVec3 c)
		{
			this.DesignateThing(c.GetPlant(base.Map));
		}

		// Token: 0x06009222 RID: 37410 RVA: 0x00061E64 File Offset: 0x00060064
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.RemoveAllDesignationsOn(t, false);
			base.Map.designationManager.AddDesignation(new Designation(t, this.designationDef));
		}

		// Token: 0x06009223 RID: 37411 RVA: 0x0006127F File Offset: 0x0005F47F
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}

		// Token: 0x04005C68 RID: 23656
		protected DesignationDef designationDef;
	}
}
