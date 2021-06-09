using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001996 RID: 6550
	public class Designator_AreaSnowClearClear : Designator_AreaSnowClear
	{
		// Token: 0x060090C6 RID: 37062 RVA: 0x0029AEDC File Offset: 0x002990DC
		public Designator_AreaSnowClearClear() : base(DesignateMode.Remove)
		{
			this.defaultLabel = "DesignatorAreaSnowClearClear".Translate();
			this.defaultDesc = "DesignatorAreaSnowClearClearDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/SnowClearAreaOff", true);
			this.soundDragSustain = SoundDefOf.Designate_DragAreaDelete;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_ZoneDelete;
		}
	}
}
