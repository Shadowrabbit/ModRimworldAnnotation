using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012BE RID: 4798
	public class Designator_Strip : Designator
	{
		// Token: 0x17001406 RID: 5126
		// (get) Token: 0x0600729C RID: 29340 RVA: 0x0009007E File Offset: 0x0008E27E
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17001407 RID: 5127
		// (get) Token: 0x0600729D RID: 29341 RVA: 0x002640D0 File Offset: 0x002622D0
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Strip;
			}
		}

		// Token: 0x0600729E RID: 29342 RVA: 0x002640D8 File Offset: 0x002622D8
		public Designator_Strip()
		{
			this.defaultLabel = "DesignatorStrip".Translate();
			this.defaultDesc = "DesignatorStripDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Strip", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Claim;
			this.hotKey = KeyBindingDefOf.Misc11;
		}

		// Token: 0x0600729F RID: 29343 RVA: 0x00264159 File Offset: 0x00262359
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (!this.StrippablesInCell(c).Any<Thing>())
			{
				return "MessageMustDesignateStrippable".Translate();
			}
			return true;
		}

		// Token: 0x060072A0 RID: 29344 RVA: 0x00264194 File Offset: 0x00262394
		public override void DesignateSingleCell(IntVec3 c)
		{
			foreach (Thing t in this.StrippablesInCell(c))
			{
				this.DesignateThing(t);
			}
		}

		// Token: 0x060072A1 RID: 29345 RVA: 0x002641E4 File Offset: 0x002623E4
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			if (base.Map.designationManager.DesignationOn(t, this.Designation) != null)
			{
				return false;
			}
			return StrippableUtility.CanBeStrippedByColony(t);
		}

		// Token: 0x060072A2 RID: 29346 RVA: 0x00264211 File Offset: 0x00262411
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.AddDesignation(new Designation(t, this.Designation));
			StrippableUtility.CheckSendStrippingImpactsGoodwillMessage(t);
		}

		// Token: 0x060072A3 RID: 29347 RVA: 0x0026423A File Offset: 0x0026243A
		private IEnumerable<Thing> StrippablesInCell(IntVec3 c)
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
