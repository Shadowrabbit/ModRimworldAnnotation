using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001991 RID: 6545
	public class Designator_AreaHomeClear : Designator_AreaHome
	{
		// Token: 0x060090B0 RID: 37040 RVA: 0x0029AB0C File Offset: 0x00298D0C
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
