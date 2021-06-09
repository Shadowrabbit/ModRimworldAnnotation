using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020019A2 RID: 6562
	public class Designator_Haul : Designator
	{
		// Token: 0x170016F7 RID: 5879
		// (get) Token: 0x0600910C RID: 37132 RVA: 0x0001B6B4 File Offset: 0x000198B4
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170016F8 RID: 5880
		// (get) Token: 0x0600910D RID: 37133 RVA: 0x00061462 File Offset: 0x0005F662
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Haul;
			}
		}

		// Token: 0x0600910E RID: 37134 RVA: 0x0029BC90 File Offset: 0x00299E90
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

		// Token: 0x0600910F RID: 37135 RVA: 0x0029BD14 File Offset: 0x00299F14
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

		// Token: 0x06009110 RID: 37136 RVA: 0x00061469 File Offset: 0x0005F669
		public override void DesignateSingleCell(IntVec3 c)
		{
			this.DesignateThing(c.GetFirstHaulable(base.Map));
		}

		// Token: 0x06009111 RID: 37137 RVA: 0x0029BD80 File Offset: 0x00299F80
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

		// Token: 0x06009112 RID: 37138 RVA: 0x0006147D File Offset: 0x0005F67D
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.AddDesignation(new Designation(t, this.Designation));
		}

		// Token: 0x06009113 RID: 37139 RVA: 0x0006127F File Offset: 0x0005F47F
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}
	}
}
