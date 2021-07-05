using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CEF RID: 3311
	public class RoomStatWorker_FromStatByCurve : RoomStatWorker
	{
		// Token: 0x06004D0D RID: 19725 RVA: 0x0019B234 File Offset: 0x00199434
		public override float GetScore(Room room)
		{
			return this.def.curve.Evaluate(room.GetStat(this.def.inputStat));
		}
	}
}
