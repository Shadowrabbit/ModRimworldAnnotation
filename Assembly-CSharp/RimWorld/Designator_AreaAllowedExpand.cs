using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200198B RID: 6539
	public class Designator_AreaAllowedExpand : Designator_AreaAllowed
	{
		// Token: 0x0600909B RID: 37019 RVA: 0x0029A7B4 File Offset: 0x002989B4
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

		// Token: 0x0600909C RID: 37020 RVA: 0x00061057 File Offset: 0x0005F257
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			return c.InBounds(base.Map) && Designator_AreaAllowed.SelectedArea != null && !Designator_AreaAllowed.SelectedArea[c];
		}

		// Token: 0x0600909D RID: 37021 RVA: 0x00061084 File Offset: 0x0005F284
		public override void DesignateSingleCell(IntVec3 c)
		{
			Designator_AreaAllowed.SelectedArea[c] = true;
		}
	}
}
