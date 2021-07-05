using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012A9 RID: 4777
	public class Designator_Claim : Designator
	{
		// Token: 0x170013E7 RID: 5095
		// (get) Token: 0x0600720C RID: 29196 RVA: 0x0009007E File Offset: 0x0008E27E
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x0600720D RID: 29197 RVA: 0x00261CA0 File Offset: 0x0025FEA0
		public Designator_Claim()
		{
			this.defaultLabel = "DesignatorClaim".Translate();
			this.defaultDesc = "DesignatorClaimDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Claim", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Claim;
			this.hotKey = KeyBindingDefOf.Misc4;
		}

		// Token: 0x0600720E RID: 29198 RVA: 0x00261D24 File Offset: 0x0025FF24
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (c.Fogged(base.Map))
			{
				return false;
			}
			if (!(from t in c.GetThingList(base.Map)
			where this.CanDesignateThing(t).Accepted
			select t).Any<Thing>())
			{
				return "MessageMustDesignateClaimable".Translate();
			}
			return true;
		}

		// Token: 0x0600720F RID: 29199 RVA: 0x00261D98 File Offset: 0x0025FF98
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

		// Token: 0x06007210 RID: 29200 RVA: 0x00261DE8 File Offset: 0x0025FFE8
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			Building building = t as Building;
			return building != null && building.Faction != Faction.OfPlayer && building.ClaimableBy(Faction.OfPlayer);
		}

		// Token: 0x06007211 RID: 29201 RVA: 0x00261E20 File Offset: 0x00260020
		public override void DesignateThing(Thing t)
		{
			t.SetFaction(Faction.OfPlayer, null);
			foreach (IntVec3 cell in t.OccupiedRect())
			{
				FleckMaker.ThrowMetaPuffs(new TargetInfo(cell, base.Map, false));
			}
		}
	}
}
