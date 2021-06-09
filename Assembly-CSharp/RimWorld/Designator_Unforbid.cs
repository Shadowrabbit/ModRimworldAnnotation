using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020019BD RID: 6589
	public class Designator_Unforbid : Designator
	{
		// Token: 0x17001717 RID: 5911
		// (get) Token: 0x060091A8 RID: 37288 RVA: 0x0001B6B4 File Offset: 0x000198B4
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x060091A9 RID: 37289 RVA: 0x0029D638 File Offset: 0x0029B838
		public Designator_Unforbid()
		{
			this.defaultLabel = "DesignatorUnforbid".Translate();
			this.defaultDesc = "DesignatorUnforbidDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/ForbidOff", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Claim;
			this.hotKey = KeyBindingDefOf.Misc6;
			this.hasDesignateAllFloatMenuOption = true;
			this.designateAllLabel = "UnforbidAllItems".Translate();
		}

		// Token: 0x060091AA RID: 37290 RVA: 0x0029D6D8 File Offset: 0x0029B8D8
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map) || c.Fogged(base.Map))
			{
				return false;
			}
			if (!c.GetThingList(base.Map).Any((Thing t) => this.CanDesignateThing(t).Accepted))
			{
				return "MessageMustDesignateUnforbiddable".Translate();
			}
			return true;
		}

		// Token: 0x060091AB RID: 37291 RVA: 0x0029B4E4 File Offset: 0x002996E4
		public override void DesignateSingleCell(IntVec3 c)
		{
			List<Thing> thingList = c.GetThingList(base.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (this.CanDesignateThing(thingList[i]).Accepted)
				{
					this.DesignateThing(thingList[i]);
				}
			}
		}

		// Token: 0x060091AC RID: 37292 RVA: 0x0029D740 File Offset: 0x0029B940
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			if (t.def.category != ThingCategory.Item)
			{
				return false;
			}
			CompForbiddable compForbiddable = t.TryGetComp<CompForbiddable>();
			return compForbiddable != null && compForbiddable.Forbidden;
		}

		// Token: 0x060091AD RID: 37293 RVA: 0x000619CD File Offset: 0x0005FBCD
		public override void DesignateThing(Thing t)
		{
			t.SetForbidden(false, false);
		}
	}
}
