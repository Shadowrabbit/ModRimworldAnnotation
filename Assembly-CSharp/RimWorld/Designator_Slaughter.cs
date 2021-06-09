using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020019AF RID: 6575
	public class Designator_Slaughter : Designator
	{
		// Token: 0x17001709 RID: 5897
		// (get) Token: 0x0600915A RID: 37210 RVA: 0x0001B6B4 File Offset: 0x000198B4
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x1700170A RID: 5898
		// (get) Token: 0x0600915B RID: 37211 RVA: 0x000616FA File Offset: 0x0005F8FA
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Slaughter;
			}
		}

		// Token: 0x0600915C RID: 37212 RVA: 0x0029C8E0 File Offset: 0x0029AAE0
		public Designator_Slaughter()
		{
			this.defaultLabel = "DesignatorSlaughter".Translate();
			this.defaultDesc = "DesignatorSlaughterDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Slaughter", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Hunt;
			this.hotKey = KeyBindingDefOf.Misc7;
		}

		// Token: 0x0600915D RID: 37213 RVA: 0x00061701 File Offset: 0x0005F901
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (!this.SlaughterablesInCell(c).Any<Pawn>())
			{
				return "MessageMustDesignateSlaughterable".Translate();
			}
			return true;
		}

		// Token: 0x0600915E RID: 37214 RVA: 0x0029C96C File Offset: 0x0029AB6C
		public override void DesignateSingleCell(IntVec3 loc)
		{
			foreach (Pawn t in this.SlaughterablesInCell(loc))
			{
				this.DesignateThing(t);
			}
		}

		// Token: 0x0600915F RID: 37215 RVA: 0x0029C9BC File Offset: 0x0029ABBC
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			Pawn pawn = t as Pawn;
			if (pawn != null && pawn.def.race.Animal && pawn.Faction == Faction.OfPlayer && base.Map.designationManager.DesignationOn(pawn, this.Designation) == null && !pawn.InAggroMentalState)
			{
				return true;
			}
			return false;
		}

		// Token: 0x06009160 RID: 37216 RVA: 0x0006173C File Offset: 0x0005F93C
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.AddDesignation(new Designation(t, this.Designation));
			this.justDesignated.Add((Pawn)t);
		}

		// Token: 0x06009161 RID: 37217 RVA: 0x0029CA20 File Offset: 0x0029AC20
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			for (int i = 0; i < this.justDesignated.Count; i++)
			{
				SlaughterDesignatorUtility.CheckWarnAboutBondedAnimal(this.justDesignated[i]);
			}
			this.justDesignated.Clear();
		}

		// Token: 0x06009162 RID: 37218 RVA: 0x00061770 File Offset: 0x0005F970
		private IEnumerable<Pawn> SlaughterablesInCell(IntVec3 c)
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
					yield return (Pawn)thingList[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x04005C2C RID: 23596
		private List<Pawn> justDesignated = new List<Pawn>();
	}
}
