using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001995 RID: 6549
	public class Designator_AreaSnowClearExpand : Designator_AreaSnowClear
	{
		// Token: 0x060090C5 RID: 37061 RVA: 0x0029AE70 File Offset: 0x00299070
		public Designator_AreaSnowClearExpand() : base(DesignateMode.Add)
		{
			this.defaultLabel = "DesignatorAreaSnowClearExpand".Translate();
			this.defaultDesc = "DesignatorAreaSnowClearExpandDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/SnowClearAreaOn", true);
			this.soundDragSustain = SoundDefOf.Designate_DragAreaAdd;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_ZoneAdd;
		}
	}
}
