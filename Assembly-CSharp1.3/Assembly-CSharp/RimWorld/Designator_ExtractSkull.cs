using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012AD RID: 4781
	public class Designator_ExtractSkull : Designator
	{
		// Token: 0x170013F0 RID: 5104
		// (get) Token: 0x06007233 RID: 29235 RVA: 0x0009007E File Offset: 0x0008E27E
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x06007234 RID: 29236 RVA: 0x002624E4 File Offset: 0x002606E4
		public Designator_ExtractSkull()
		{
			if (!ModLister.CheckIdeology("Skull extraction"))
			{
				return;
			}
			this.defaultLabel = "DesignatorExtractSkull".Translate();
			this.defaultDesc = "DesignatorExtractSkullDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/ExtractSkull", true);
			this.useMouseIcon = true;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.soundSucceeded = SoundDefOf.Designate_ExtractSkull;
			this.hotKey = KeyBindingDefOf.Misc3;
			this.tutorTag = "ExtractSkull";
		}

		// Token: 0x06007235 RID: 29237 RVA: 0x00262580 File Offset: 0x00260780
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

		// Token: 0x06007236 RID: 29238 RVA: 0x0026260C File Offset: 0x0026080C
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			if (!WorkGiver_ExtractSkull.CanPlayerExtractSkull())
			{
				return false;
			}
			Corpse corpse = t as Corpse;
			if (corpse != null && corpse.InnerPawn.RaceProps.Humanlike)
			{
				if (corpse.InnerPawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).Any((BodyPartRecord p) => p.def == BodyPartDefOf.Head) && base.Map.designationManager.DesignationOn(t, DesignationDefOf.ExtractSkull) == null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007237 RID: 29239 RVA: 0x002626A8 File Offset: 0x002608A8
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

		// Token: 0x06007238 RID: 29240 RVA: 0x00262714 File Offset: 0x00260914
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.AddDesignation(new Designation(t, DesignationDefOf.ExtractSkull));
		}
	}
}
