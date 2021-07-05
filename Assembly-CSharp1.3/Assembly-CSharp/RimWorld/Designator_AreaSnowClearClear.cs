using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012A7 RID: 4775
	public class Designator_AreaSnowClearClear : Designator_AreaSnowClear
	{
		// Token: 0x06007201 RID: 29185 RVA: 0x00261748 File Offset: 0x0025F948
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
