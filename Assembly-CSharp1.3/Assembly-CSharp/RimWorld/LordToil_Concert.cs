using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020008CE RID: 2254
	public class LordToil_Concert : LordToil_Party
	{
		// Token: 0x17000AAC RID: 2732
		// (get) Token: 0x06003B4D RID: 15181 RVA: 0x0014B714 File Offset: 0x00149914
		public Pawn Organizer
		{
			get
			{
				return this.organizer;
			}
		}

		// Token: 0x06003B4E RID: 15182 RVA: 0x0014B71C File Offset: 0x0014991C
		public LordToil_Concert(IntVec3 spot, Pawn organizer, GatheringDef gatheringDef, float joyPerTick = 3.5E-05f) : base(spot, gatheringDef, joyPerTick)
		{
			this.organizer = organizer;
		}

		// Token: 0x04002052 RID: 8274
		private Pawn organizer;
	}
}
