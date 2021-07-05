using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001999 RID: 6553
	public class Designator_Claim : Designator
	{
		// Token: 0x170016ED RID: 5869
		// (get) Token: 0x060090D4 RID: 37076 RVA: 0x0001B6B4 File Offset: 0x000198B4
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x060090D5 RID: 37077 RVA: 0x0029B3EC File Offset: 0x002995EC
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

		// Token: 0x060090D6 RID: 37078 RVA: 0x0029B470 File Offset: 0x00299670
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

		// Token: 0x060090D7 RID: 37079 RVA: 0x0029B4E4 File Offset: 0x002996E4
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

		// Token: 0x060090D8 RID: 37080 RVA: 0x0029B534 File Offset: 0x00299734
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			Building building = t as Building;
			return building != null && building.Faction != Faction.OfPlayer && building.ClaimableBy(Faction.OfPlayer);
		}

		// Token: 0x060090D9 RID: 37081 RVA: 0x0029B56C File Offset: 0x0029976C
		public override void DesignateThing(Thing t)
		{
			t.SetFaction(Faction.OfPlayer, null);
			foreach (IntVec3 cell in t.OccupiedRect())
			{
				MoteMaker.ThrowMetaPuffs(new TargetInfo(cell, base.Map, false));
			}
		}
	}
}
