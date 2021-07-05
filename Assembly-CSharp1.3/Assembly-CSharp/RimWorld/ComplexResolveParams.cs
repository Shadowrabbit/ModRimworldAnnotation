using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C67 RID: 3175
	public struct ComplexResolveParams
	{
		// Token: 0x04002D0F RID: 11535
		public List<Thing> spawnedThings;

		// Token: 0x04002D10 RID: 11536
		public List<CellRect> room;

		// Token: 0x04002D11 RID: 11537
		public List<List<CellRect>> allRooms;

		// Token: 0x04002D12 RID: 11538
		public float points;

		// Token: 0x04002D13 RID: 11539
		public Map map;

		// Token: 0x04002D14 RID: 11540
		public CellRect complexRect;

		// Token: 0x04002D15 RID: 11541
		public Faction hostileFaction;

		// Token: 0x04002D16 RID: 11542
		public string triggerSignal;

		// Token: 0x04002D17 RID: 11543
		public int? delayTicks;

		// Token: 0x04002D18 RID: 11544
		public bool passive;
	}
}
