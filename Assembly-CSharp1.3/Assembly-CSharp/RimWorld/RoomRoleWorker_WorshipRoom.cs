using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CEC RID: 3308
	public class RoomRoleWorker_WorshipRoom : RoomRoleWorker
	{
		// Token: 0x06004D05 RID: 19717 RVA: 0x0019AEE9 File Offset: 0x001990E9
		public override string PostProcessedLabel(string baseLabel)
		{
			if (this.firstAltarIdeo == null || this.firstAltarIdeo.WorshipRoomLabel.NullOrEmpty())
			{
				return base.PostProcessedLabel(baseLabel);
			}
			return this.firstAltarIdeo.WorshipRoomLabel;
		}

		// Token: 0x06004D06 RID: 19718 RVA: 0x0019AF18 File Offset: 0x00199118
		public override float GetScore(Room room)
		{
			if (!ModsConfig.IdeologyActive)
			{
				return -1f;
			}
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			this.firstAltarIdeo = null;
			int num = 0;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				if (containedAndAdjacentThings[i].def.isAltar)
				{
					CompStyleable compStyleable = containedAndAdjacentThings[i].TryGetComp<CompStyleable>();
					if (compStyleable != null)
					{
						Precept_ThingStyle sourcePrecept = compStyleable.SourcePrecept;
						if (((sourcePrecept != null) ? sourcePrecept.ideo.StructureMeme : null) != null)
						{
							if (this.firstAltarIdeo == null)
							{
								this.firstAltarIdeo = compStyleable.SourcePrecept.ideo;
							}
							num++;
						}
					}
				}
			}
			return (float)((num == 0) ? 0 : Mathf.Max(2000, num * 75));
		}

		// Token: 0x04002E89 RID: 11913
		private const int MinScore = 2000;

		// Token: 0x04002E8A RID: 11914
		private Ideo firstAltarIdeo;
	}
}
