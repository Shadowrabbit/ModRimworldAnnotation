using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001990 RID: 6544
	public class Designator_AreaHomeExpand : Designator_AreaHome
	{
		// Token: 0x060090AF RID: 37039 RVA: 0x0029AA88 File Offset: 0x00298C88
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
