using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012AF RID: 4783
	public class Designator_Haul : Designator
	{
		// Token: 0x170013F2 RID: 5106
		// (get) Token: 0x06007240 RID: 29248 RVA: 0x0009007E File Offset: 0x0008E27E
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170013F3 RID: 5107
		// (get) Token: 0x06007241 RID: 29249 RVA: 0x002628F4 File Offset: 0x00260AF4
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Haul;
			}
		}

		// Token: 0x06007242 RID: 29250 RVA: 0x002628FC File Offset: 0x00260AFC
		public Designator_Haul()
		{
			this.defaultLabel = "DesignatorHaulThings".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Haul", true);
			this.defaultDesc = "DesignatorHaulThingsDesc".Translate();
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Haul;
			this.hotKey = KeyBindingDefOf.Misc12;
		}

		// Token: 0x06007243 RID: 29251 RVA: 0x00262980 File Offset: 0x00260B80
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map) || c.Fogged(base.Map))
			{
				return false;
			}
			Thing firstHaulable = c.GetFirstHaulable(base.Map);
			if (firstHaulable == null)
			{
				return "MessageMustDesignateHaulable".Translate();
			}
			AcceptanceReport result = this.CanDesignateThing(firstHaulable);
			if (!result.Accepted)
			{
				return result;
			}
			return true;
		}

		// Token: 0x06007244 RID: 29252 RVA: 0x002629E9 File Offset: 0x00260BE9
		public override void DesignateSingleCell(IntVec3 c)
		{
			this.DesignateThing(c.GetFirstHaulable(base.Map));
		}

		// Token: 0x06007245 RID: 29253 RVA: 0x00262A00 File Offset: 0x00260C00
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			if (!t.def.designateHaulable)
			{
				return false;
			}
			if (base.Map.designationManager.DesignationOn(t, this.Designation) != null)
			{
				return false;
			}
			if (t.IsInValidStorage())
			{
				return "MessageAlreadyInStorage".Translate();
			}
			return true;
		}

		// Token: 0x06007246 RID: 29254 RVA: 0x00262A5F File Offset: 0x00260C5F
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.AddDesignation(new Designation(t, this.Designation));
		}

		// Token: 0x06007247 RID: 29255 RVA: 0x00261B2D File Offset: 0x0025FD2D
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}
	}
}
