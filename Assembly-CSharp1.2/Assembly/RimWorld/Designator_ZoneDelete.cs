using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020019D0 RID: 6608
	public class Designator_ZoneDelete : Designator_Zone
	{
		// Token: 0x06009217 RID: 37399 RVA: 0x0029F738 File Offset: 0x0029D938
		public Designator_ZoneDelete()
		{
			this.defaultLabel = "DesignatorZoneDelete".Translate();
			this.defaultDesc = "DesignatorZoneDeleteDesc".Translate();
			this.soundDragSustain = SoundDefOf.Designate_DragAreaDelete;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_ZoneDelete;
			this.useMouseIcon = true;
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneDelete", true);
			this.hotKey = KeyBindingDefOf.Misc3;
		}

		// Token: 0x06009218 RID: 37400 RVA: 0x0029F7C0 File Offset: 0x0029D9C0
		public override AcceptanceReport CanDesignateCell(IntVec3 sq)
		{
			if (!sq.InBounds(base.Map))
			{
				return false;
			}
			if (sq.Fogged(base.Map))
			{
				return false;
			}
			if (base.Map.zoneManager.ZoneAt(sq) == null)
			{
				return false;
			}
			return true;
		}

		// Token: 0x06009219 RID: 37401 RVA: 0x0029F818 File Offset: 0x0029DA18
		public override void DesignateSingleCell(IntVec3 c)
		{
			Zone zone = base.Map.zoneManager.ZoneAt(c);
			zone.RemoveCell(c);
			if (!this.justDesignated.Contains(zone))
			{
				this.justDesignated.Add(zone);
			}
		}

		// Token: 0x0600921A RID: 37402 RVA: 0x0029F858 File Offset: 0x0029DA58
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			for (int i = 0; i < this.justDesignated.Count; i++)
			{
				this.justDesignated[i].CheckContiguous();
			}
			this.justDesignated.Clear();
		}

		// Token: 0x04005C67 RID: 23655
		private List<Zone> justDesignated = new List<Zone>();
	}
}
