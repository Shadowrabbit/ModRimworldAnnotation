using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012C2 RID: 4802
	public class Designator_Unforbid : Designator
	{
		// Token: 0x1700140C RID: 5132
		// (get) Token: 0x060072B8 RID: 29368 RVA: 0x0009007E File Offset: 0x0008E27E
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x060072B9 RID: 29369 RVA: 0x0026482C File Offset: 0x00262A2C
		public Designator_Unforbid()
		{
			this.defaultLabel = "DesignatorUnforbid".Translate();
			this.defaultDesc = "DesignatorUnforbidDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/ForbidOff", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Checkbox_TurnedOn;
			this.hotKey = KeyBindingDefOf.Misc6;
			this.hasDesignateAllFloatMenuOption = true;
			this.designateAllLabel = "UnforbidAllItems".Translate();
			this.tutorTag = "Unforbid";
		}

		// Token: 0x060072BA RID: 29370 RVA: 0x002648D4 File Offset: 0x00262AD4
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

		// Token: 0x060072BB RID: 29371 RVA: 0x0026493C File Offset: 0x00262B3C
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

		// Token: 0x060072BC RID: 29372 RVA: 0x0026498C File Offset: 0x00262B8C
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			if (t.def.category != ThingCategory.Item)
			{
				return false;
			}
			CompForbiddable compForbiddable = t.TryGetComp<CompForbiddable>();
			return compForbiddable != null && compForbiddable.Forbidden;
		}

		// Token: 0x060072BD RID: 29373 RVA: 0x002649C6 File Offset: 0x00262BC6
		public override void DesignateThing(Thing t)
		{
			t.SetForbidden(false, false);
		}
	}
}
