using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200129D RID: 4765
	public class Designator_AreaAllowedClear : Designator_AreaAllowed
	{
		// Token: 0x060071D9 RID: 29145 RVA: 0x00260EB8 File Offset: 0x0025F0B8
		public Designator_AreaAllowedClear() : base(DesignateMode.Remove)
		{
			this.defaultLabel = "DesignatorClearAreaAllowed".Translate();
			this.defaultDesc = "DesignatorClearAreaAllowedDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/AreaAllowedClear", true);
			this.soundDragSustain = SoundDefOf.Designate_DragAreaDelete;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_ZoneDelete;
			this.hotKey = KeyBindingDefOf.Misc10;
			this.tutorTag = "AreaAllowedClear";
		}

		// Token: 0x060071DA RID: 29146 RVA: 0x00260F3A File Offset: 0x0025F13A
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			return c.InBounds(base.Map) && Designator_AreaAllowed.SelectedArea != null && Designator_AreaAllowed.SelectedArea[c];
		}

		// Token: 0x060071DB RID: 29147 RVA: 0x00260F64 File Offset: 0x0025F164
		public override void DesignateSingleCell(IntVec3 c)
		{
			Designator_AreaAllowed.SelectedArea[c] = false;
		}
	}
}
