using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001312 RID: 4882
	public class RoomStatWorker_FromStatByCurve : RoomStatWorker
	{
		// Token: 0x060069BB RID: 27067 RVA: 0x00048097 File Offset: 0x00046297
		public override float GetScore(Room room)
		{
			return this.def.curve.Evaluate(room.GetStat(this.def.inputStat));
		}
	}
}
