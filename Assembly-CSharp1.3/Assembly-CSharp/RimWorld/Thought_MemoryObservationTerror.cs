using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E9D RID: 3741
	public class Thought_MemoryObservationTerror : Thought_MemoryObservation
	{
		// Token: 0x17000F58 RID: 3928
		// (get) Token: 0x060057EB RID: 22507 RVA: 0x001DE434 File Offset: 0x001DC634
		public override string LabelCap
		{
			get
			{
				return base.CurStage.label.Formatted(this.target.Named("THING"), this.pawn.Named("PAWN"));
			}
		}

		// Token: 0x17000F59 RID: 3929
		// (get) Token: 0x060057EC RID: 22508 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool Save
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000F5A RID: 3930
		// (set) Token: 0x060057ED RID: 22509 RVA: 0x001DE46B File Offset: 0x001DC66B
		public override Thing Target
		{
			set
			{
				this.targetThingID = value.thingIDNumber;
				this.target = value;
			}
		}

		// Token: 0x040033D6 RID: 13270
		public int intensity;

		// Token: 0x040033D7 RID: 13271
		private Thing target;
	}
}
