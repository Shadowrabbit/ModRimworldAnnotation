using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012A2 RID: 4770
	public class Designator_AreaHomeClear : Designator_AreaHome
	{
		// Token: 0x060071EB RID: 29163 RVA: 0x002612C4 File Offset: 0x0025F4C4
		public Designator_AreaHomeClear() : base(DesignateMode.Remove)
		{
			this.defaultLabel = "DesignatorAreaHomeClear".Translate();
			this.defaultDesc = "DesignatorAreaHomeClearDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/HomeAreaOff", true);
			this.soundDragSustain = SoundDefOf.Designate_DragAreaDelete;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_ZoneDelete;
			this.hotKey = KeyBindingDefOf.Misc7;
		}
	}
}
