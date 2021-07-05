using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E8A RID: 3722
	public class DirectPawnRelation : IExposable
	{
		// Token: 0x06005735 RID: 22325 RVA: 0x000033AC File Offset: 0x000015AC
		public DirectPawnRelation()
		{
		}

		// Token: 0x06005736 RID: 22326 RVA: 0x001D9D8C File Offset: 0x001D7F8C
		public DirectPawnRelation(PawnRelationDef def, Pawn otherPawn, int startTicks)
		{
			this.def = def;
			this.otherPawn = otherPawn;
			this.startTicks = startTicks;
		}

		// Token: 0x06005737 RID: 22327 RVA: 0x001D9DA9 File Offset: 0x001D7FA9
		public void ExposeData()
		{
			Scribe_Defs.Look<PawnRelationDef>(ref this.def, "def");
			Scribe_References.Look<Pawn>(ref this.otherPawn, "otherPawn", true);
			Scribe_Values.Look<int>(ref this.startTicks, "startTicks", 0, false);
		}

		// Token: 0x0400339A RID: 13210
		public PawnRelationDef def;

		// Token: 0x0400339B RID: 13211
		public Pawn otherPawn;

		// Token: 0x0400339C RID: 13212
		public int startTicks;
	}
}
