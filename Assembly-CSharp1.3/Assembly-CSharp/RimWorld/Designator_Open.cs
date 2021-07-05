using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012B2 RID: 4786
	public class Designator_Open : Designator
	{
		// Token: 0x170013F9 RID: 5113
		// (get) Token: 0x0600725F RID: 29279 RVA: 0x0009007E File Offset: 0x0008E27E
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170013FA RID: 5114
		// (get) Token: 0x06007260 RID: 29280 RVA: 0x00263064 File Offset: 0x00261264
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Open;
			}
		}

		// Token: 0x06007261 RID: 29281 RVA: 0x0026306C File Offset: 0x0026126C
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

		// Token: 0x06007262 RID: 29282 RVA: 0x002630ED File Offset: 0x002612ED
		protected override void FinalizeDesignationFailed()
		{
			base.FinalizeDesignationFailed();
			Messages.Message("MessageMustDesignateOpenable".Translate(), MessageTypeDefOf.RejectInput, false);
		}

		// Token: 0x06007263 RID: 29283 RVA: 0x0026310F File Offset: 0x0026130F
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

		// Token: 0x06007264 RID: 29284 RVA: 0x00263144 File Offset: 0x00261344
		public override void DesignateSingleCell(IntVec3 c)
		{
			foreach (Thing t in this.OpenablesInCell(c))
			{
				this.DesignateThing(t);
			}
		}

		// Token: 0x06007265 RID: 29285 RVA: 0x00263194 File Offset: 0x00261394
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			IOpenable openable = t as IOpenable;
			if (openable == null || !openable.CanOpen || base.Map.designationManager.DesignationOn(t, this.Designation) != null)
			{
				return false;
			}
			return true;
		}

		// Token: 0x06007266 RID: 29286 RVA: 0x00262A5F File Offset: 0x00260C5F
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.AddDesignation(new Designation(t, this.Designation));
		}

		// Token: 0x06007267 RID: 29287 RVA: 0x002631D9 File Offset: 0x002613D9
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
