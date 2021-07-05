using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012A1 RID: 4769
	public class Designator_AreaHomeExpand : Designator_AreaHome
	{
		// Token: 0x060071EA RID: 29162 RVA: 0x00261240 File Offset: 0x0025F440
		public Designator_AreaHomeExpand() : base(DesignateMode.Add)
		{
			this.defaultLabel = "DesignatorAreaHomeExpand".Translate();
			this.defaultDesc = "DesignatorAreaHomeExpandDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/HomeAreaOn", true);
			this.soundDragSustain = SoundDefOf.Designate_DragAreaAdd;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_ZoneAdd;
			this.tutorTag = "AreaHomeExpand";
			this.hotKey = KeyBindingDefOf.Misc4;
		}
	}
}
