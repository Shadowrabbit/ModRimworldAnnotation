using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012B6 RID: 4790
	public class Designator_ReleaseAnimalToWild : Designator
	{
		// Token: 0x170013FF RID: 5119
		// (get) Token: 0x06007273 RID: 29299 RVA: 0x0009007E File Offset: 0x0008E27E
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x06007274 RID: 29300 RVA: 0x00263400 File Offset: 0x00261600
		public Designator_ReleaseAnimalToWild()
		{
			this.defaultLabel = "DesignatorReleaseAnimalToWild".Translate();
			this.defaultDesc = "DesignatorReleaseAnimalToWildDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/ReleaseToTheWild", true);
			this.useMouseIcon = true;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.soundSucceeded = SoundDefOf.Designate_ReleaseToWild;
			this.hotKey = KeyBindingDefOf.Misc3;
			this.tutorTag = "ReleaseAnimalToWild";
		}

		// Token: 0x06007275 RID: 29301 RVA: 0x0026348C File Offset: 0x0026168C
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			foreach (Thing t in c.GetThingList(base.Map))
			{
				if (this.CanDesignateThing(t).Accepted)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007276 RID: 29302 RVA: 0x00263518 File Offset: 0x00261718
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			Pawn pawn = t as Pawn;
			if (pawn != null && pawn.RaceProps.Animal && pawn.Faction == Faction.OfPlayer && base.Map.designationManager.DesignationOn(t, DesignationDefOf.ReleaseAnimalToWild) == null && !pawn.Dead && pawn.RaceProps.canReleaseToWild)
			{
				return true;
			}
			return false;
		}

		// Token: 0x06007277 RID: 29303 RVA: 0x00263584 File Offset: 0x00261784
		public override void DesignateSingleCell(IntVec3 c)
		{
			foreach (Thing t in c.GetThingList(base.Map))
			{
				if (this.CanDesignateThing(t).Accepted)
				{
					this.DesignateThing(t);
				}
			}
		}

		// Token: 0x06007278 RID: 29304 RVA: 0x002635F0 File Offset: 0x002617F0
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.AddDesignation(new Designation((Pawn)t, DesignationDefOf.ReleaseAnimalToWild));
			Designation designation = base.Map.designationManager.DesignationOn(t, DesignationDefOf.Slaughter);
			if (designation != null)
			{
				base.Map.designationManager.RemoveDesignation(designation);
			}
			ReleaseAnimalToWildUtility.CheckWarnAboutBondedAnimal((Pawn)t);
		}
	}
}
