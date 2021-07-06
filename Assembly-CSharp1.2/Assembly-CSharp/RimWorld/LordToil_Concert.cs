using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E15 RID: 3605
	public class LordToil_Concert : LordToil_Party
	{
		// Token: 0x17000CA6 RID: 3238
		// (get) Token: 0x060051E5 RID: 20965 RVA: 0x000393ED File Offset: 0x000375ED
		public Pawn Organizer
		{
			get
			{
				return this.organizer;
			}
		}

		// Token: 0x060051E6 RID: 20966 RVA: 0x000393F5 File Offset: 0x000375F5
		public LordToil_Concert(IntVec3 spot, Pawn organizer, GatheringDef gatheringDef, float joyPerTick = 3.5E-05f) : base(spot, gatheringDef, joyPerTick)
		{
			this.organizer = organizer;
		}

		// Token: 0x04003470 RID: 13424
		private Pawn organizer;
	}
}
