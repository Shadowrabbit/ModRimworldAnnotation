using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012A6 RID: 4774
	public class Designator_AreaSnowClearExpand : Designator_AreaSnowClear
	{
		// Token: 0x06007200 RID: 29184 RVA: 0x002616DC File Offset: 0x0025F8DC
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
