using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012AE RID: 4782
	public class Designator_Forbid : Designator
	{
		// Token: 0x170013F1 RID: 5105
		// (get) Token: 0x06007239 RID: 29241 RVA: 0x0009007E File Offset: 0x0008E27E
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x0600723A RID: 29242 RVA: 0x00262738 File Offset: 0x00260938
		public Designator_Forbid()
		{
			this.defaultLabel = "DesignatorForbid".Translate();
			this.defaultDesc = "DesignatorForbidDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/ForbidOn", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Checkbox_TurnedOff;
			this.hotKey = KeyBindingDefOf.Command_ItemForbid;
			this.hasDesignateAllFloatMenuOption = true;
			this.designateAllLabel = "ForbidAllItems".Translate();
		}

		// Token: 0x0600723B RID: 29243 RVA: 0x002627D8 File Offset: 0x002609D8
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map) || c.Fogged(base.Map))
			{
				return false;
			}
			if (!c.GetThingList(base.Map).Any((Thing t) => this.CanDesignateThing(t).Accepted))
			{
				return "MessageMustDesignateForbiddable".Translate();
			}
			return true;
		}

		// Token: 0x0600723C RID: 29244 RVA: 0x00262840 File Offset: 0x00260A40
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

		// Token: 0x0600723D RID: 29245 RVA: 0x00262890 File Offset: 0x00260A90
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			if (t.def.category != ThingCategory.Item)
			{
				return false;
			}
			CompForbiddable compForbiddable = t.TryGetComp<CompForbiddable>();
			return compForbiddable != null && !compForbiddable.Forbidden;
		}

		// Token: 0x0600723E RID: 29246 RVA: 0x002628CD File Offset: 0x00260ACD
		public override void DesignateThing(Thing t)
		{
			t.SetForbidden(true, false);
		}
	}
}
