using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x02000F43 RID: 3907
	public class RitualObligationTargetWorker_ConsumableBuilding : RitualObligationTargetWorker_ThingDef
	{
		// Token: 0x06005CDF RID: 23775 RVA: 0x001FE9E5 File Offset: 0x001FCBE5
		public RitualObligationTargetWorker_ConsumableBuilding()
		{
		}

		// Token: 0x06005CE0 RID: 23776 RVA: 0x001FE9ED File Offset: 0x001FCBED
		public RitualObligationTargetWorker_ConsumableBuilding(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x06005CE1 RID: 23777 RVA: 0x001FEE04 File Offset: 0x001FD004
		public override List<string> MissingTargetBuilding(Ideo ideo)
		{
			List<string> list = new List<string>();
			for (int i = 0; i < this.def.thingDefs.Count; i++)
			{
				if (ideo.HasPreceptForBuilding(this.def.thingDefs[i]))
				{
					return base.MissingTargetBuilding(ideo);
				}
				list.Add(this.def.thingDefs[i].label);
			}
			return list;
		}
	}
}
