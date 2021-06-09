using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020019A8 RID: 6568
	public class Designator_Open : Designator
	{
		// Token: 0x17001700 RID: 5888
		// (get) Token: 0x06009136 RID: 37174 RVA: 0x0001B6B4 File Offset: 0x000198B4
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17001701 RID: 5889
		// (get) Token: 0x06009137 RID: 37175 RVA: 0x000615A7 File Offset: 0x0005F7A7
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Open;
			}
		}

		// Token: 0x06009138 RID: 37176 RVA: 0x0029C344 File Offset: 0x0029A544
		public Designator_Open()
		{
			this.defaultLabel = "DesignatorOpen".Translate();
			this.defaultDesc = "DesignatorOpenDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Open", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.hotKey = KeyBindingDefOf.Misc5;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Claim;
		}

		// Token: 0x06009139 RID: 37177 RVA: 0x000615AE File Offset: 0x0005F7AE
		protected override void FinalizeDesignationFailed()
		{
			base.FinalizeDesignationFailed();
			Messages.Message("MessageMustDesignateOpenable".Translate(), MessageTypeDefOf.RejectInput, false);
		}

		// Token: 0x0600913A RID: 37178 RVA: 0x000615D0 File Offset: 0x0005F7D0
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (!this.OpenablesInCell(c).Any<Thing>())
			{
				return false;
			}
			return true;
		}

		// Token: 0x0600913B RID: 37179 RVA: 0x0029C3C8 File Offset: 0x0029A5C8
		public override void DesignateSingleCell(IntVec3 c)
		{
			foreach (Thing t in this.OpenablesInCell(c))
			{
				this.DesignateThing(t);
			}
		}

		// Token: 0x0600913C RID: 37180 RVA: 0x0029C418 File Offset: 0x0029A618
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			IOpenable openable = t as IOpenable;
			if (openable == null || !openable.CanOpen || base.Map.designationManager.DesignationOn(t, this.Designation) != null)
			{
				return false;
			}
			return true;
		}

		// Token: 0x0600913D RID: 37181 RVA: 0x0006147D File Offset: 0x0005F67D
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.AddDesignation(new Designation(t, this.Designation));
		}

		// Token: 0x0600913E RID: 37182 RVA: 0x00061602 File Offset: 0x0005F802
		private IEnumerable<Thing> OpenablesInCell(IntVec3 c)
		{
			if (c.Fogged(base.Map))
			{
				yield break;
			}
			List<Thing> thingList = c.GetThingList(base.Map);
			int num;
			for (int i = 0; i < thingList.Count; i = num + 1)
			{
				if (this.CanDesignateThing(thingList[i]).Accepted)
				{
					yield return thingList[i];
				}
				num = i;
			}
			yield break;
		}
	}
}
