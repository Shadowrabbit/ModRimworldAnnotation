using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200129C RID: 4764
	public class Designator_AreaAllowedExpand : Designator_AreaAllowed
	{
		// Token: 0x060071D6 RID: 29142 RVA: 0x00260DF8 File Offset: 0x0025EFF8
		public Designator_AreaAllowedExpand() : base(DesignateMode.Add)
		{
			this.defaultLabel = "DesignatorExpandAreaAllowed".Translate();
			this.defaultDesc = "DesignatorExpandAreaAllowedDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/AreaAllowedExpand", true);
			this.soundDragSustain = SoundDefOf.Designate_DragAreaAdd;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_ZoneAdd;
			this.hotKey = KeyBindingDefOf.Misc8;
			this.tutorTag = "AreaAllowedExpand";
		}

		// Token: 0x060071D7 RID: 29143 RVA: 0x00260E7A File Offset: 0x0025F07A
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			return c.InBounds(base.Map) && Designator_AreaAllowed.SelectedArea != null && !Designator_AreaAllowed.SelectedArea[c];
		}

		// Token: 0x060071D8 RID: 29144 RVA: 0x00260EA7 File Offset: 0x0025F0A7
		public override void DesignateSingleCell(IntVec3 c)
		{
			Designator_AreaAllowed.SelectedArea[c] = true;
		}
	}
}
