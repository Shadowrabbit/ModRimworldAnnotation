using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200198C RID: 6540
	public class Designator_AreaAllowedClear : Designator_AreaAllowed
	{
		// Token: 0x0600909E RID: 37022 RVA: 0x0029A838 File Offset: 0x00298A38
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

		// Token: 0x0600909F RID: 37023 RVA: 0x00061092 File Offset: 0x0005F292
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			return c.InBounds(base.Map) && Designator_AreaAllowed.SelectedArea != null && Designator_AreaAllowed.SelectedArea[c];
		}

		// Token: 0x060090A0 RID: 37024 RVA: 0x000610BC File Offset: 0x0005F2BC
		public override void DesignateSingleCell(IntVec3 c)
		{
			Designator_AreaAllowed.SelectedArea[c] = false;
		}
	}
}
