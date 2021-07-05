using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012BA RID: 4794
	public class Designator_Slaughter : Designator
	{
		// Token: 0x17001402 RID: 5122
		// (get) Token: 0x06007284 RID: 29316 RVA: 0x0009007E File Offset: 0x0008E27E
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17001403 RID: 5123
		// (get) Token: 0x06007285 RID: 29317 RVA: 0x0026399F File Offset: 0x00261B9F
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Slaughter;
			}
		}

		// Token: 0x06007286 RID: 29318 RVA: 0x002639A8 File Offset: 0x00261BA8
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

		// Token: 0x06007287 RID: 29319 RVA: 0x00263A34 File Offset: 0x00261C34
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

		// Token: 0x06007288 RID: 29320 RVA: 0x00263A70 File Offset: 0x00261C70
		public override void DesignateSingleCell(IntVec3 loc)
		{
			foreach (Pawn t in this.SlaughterablesInCell(loc))
			{
				this.DesignateThing(t);
			}
		}

		// Token: 0x06007289 RID: 29321 RVA: 0x00263AC0 File Offset: 0x00261CC0
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			Pawn pawn = t as Pawn;
			if (pawn != null && pawn.def.race.Animal && pawn.Faction == Faction.OfPlayer && base.Map.designationManager.DesignationOn(pawn, this.Designation) == null && !pawn.InAggroMentalState)
			{
				return true;
			}
			return false;
		}

		// Token: 0x0600728A RID: 29322 RVA: 0x00263B24 File Offset: 0x00261D24
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.AddDesignation(new Designation(t, this.Designation));
			this.justDesignated.Add((Pawn)t);
			Designation designation = base.Map.designationManager.DesignationOn(t, DesignationDefOf.ReleaseAnimalToWild);
			if (designation != null)
			{
				base.Map.designationManager.RemoveDesignation(designation);
			}
		}

		// Token: 0x0600728B RID: 29323 RVA: 0x00263B90 File Offset: 0x00261D90
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			for (int i = 0; i < this.justDesignated.Count; i++)
			{
				this.ShowDesignationWarnings(this.justDesignated[i]);
			}
			this.justDesignated.Clear();
		}

		// Token: 0x0600728C RID: 29324 RVA: 0x00263BD6 File Offset: 0x00261DD6
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

		// Token: 0x0600728D RID: 29325 RVA: 0x00263BED File Offset: 0x00261DED
		private void ShowDesignationWarnings(Pawn pawn)
		{
			SlaughterDesignatorUtility.CheckWarnAboutBondedAnimal(pawn);
			SlaughterDesignatorUtility.CheckWarnAboutVeneratedAnimal(pawn);
		}

		// Token: 0x04003EC4 RID: 16068
		private List<Pawn> justDesignated = new List<Pawn>();
	}
}
