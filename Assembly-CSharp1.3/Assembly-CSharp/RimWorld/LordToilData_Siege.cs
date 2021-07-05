using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008BE RID: 2238
	public class LordToilData_Siege : LordToilData
	{
		// Token: 0x06003B14 RID: 15124 RVA: 0x0014A5F4 File Offset: 0x001487F4
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.siegeCenter, "siegeCenter", default(IntVec3), false);
			Scribe_Values.Look<float>(ref this.baseRadius, "baseRadius", 16f, false);
			Scribe_Values.Look<float>(ref this.blueprintPoints, "blueprintPoints", 0f, false);
			Scribe_Values.Look<float>(ref this.desiredBuilderFraction, "desiredBuilderFraction", 0.5f, false);
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.blueprints.RemoveAll((Blueprint blue) => blue.Destroyed);
			}
			Scribe_Collections.Look<Blueprint>(ref this.blueprints, "blueprints", LookMode.Reference, Array.Empty<object>());
		}

		// Token: 0x04002031 RID: 8241
		public IntVec3 siegeCenter;

		// Token: 0x04002032 RID: 8242
		public float baseRadius = 16f;

		// Token: 0x04002033 RID: 8243
		public float blueprintPoints;

		// Token: 0x04002034 RID: 8244
		public float desiredBuilderFraction = 0.5f;

		// Token: 0x04002035 RID: 8245
		public List<Blueprint> blueprints = new List<Blueprint>();
	}
}
