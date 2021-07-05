using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020008D3 RID: 2259
	public class LordToil_Party : LordToil_Gathering
	{
		// Token: 0x06003B63 RID: 15203 RVA: 0x0014BC39 File Offset: 0x00149E39
		public LordToil_Party(IntVec3 spot, GatheringDef gatheringDef, float joyPerTick = 3.5E-05f) : base(spot, gatheringDef)
		{
			this.joyPerTick = joyPerTick;
		}

		// Token: 0x06003B64 RID: 15204 RVA: 0x0014BC58 File Offset: 0x00149E58
		public override void LordToilTick()
		{
			base.LordToilTick();
			List<Pawn> ownedPawns = this.lord.ownedPawns;
			for (int i = 0; i < ownedPawns.Count; i++)
			{
				if (GatheringsUtility.InGatheringArea(ownedPawns[i].Position, this.spot, base.Map))
				{
					Need_Joy joy = ownedPawns[i].needs.joy;
					if (joy != null)
					{
						joy.GainJoy(this.joyPerTick, JoyKindDefOf.Social);
					}
				}
			}
		}

		// Token: 0x0400205E RID: 8286
		private float joyPerTick = 3.5E-05f;

		// Token: 0x0400205F RID: 8287
		public const float DefaultJoyPerTick = 3.5E-05f;
	}
}
