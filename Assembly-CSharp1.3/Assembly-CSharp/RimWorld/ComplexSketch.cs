using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C68 RID: 3176
	public class ComplexSketch : IExposable
	{
		// Token: 0x06004A34 RID: 18996 RVA: 0x0018837C File Offset: 0x0018657C
		public void ExposeData()
		{
			Scribe_Deep.Look<Sketch>(ref this.structure, "structure", Array.Empty<object>());
			Scribe_Deep.Look<ComplexLayout>(ref this.layout, "layout", Array.Empty<object>());
			Scribe_Collections.Look<Thing>(ref this.thingsToSpawn, "thingsToSpawn", LookMode.Deep, Array.Empty<object>());
			Scribe_Defs.Look<ComplexDef>(ref this.complexDef, "complexDef");
			Scribe_Values.Look<string>(ref this.thingDiscoveredMessage, "thingDiscoveredMessage", null, false);
		}

		// Token: 0x04002D19 RID: 11545
		public Sketch structure;

		// Token: 0x04002D1A RID: 11546
		public ComplexLayout layout;

		// Token: 0x04002D1B RID: 11547
		public List<Thing> thingsToSpawn = new List<Thing>();

		// Token: 0x04002D1C RID: 11548
		public string thingDiscoveredMessage;

		// Token: 0x04002D1D RID: 11549
		public ComplexDef complexDef;
	}
}
