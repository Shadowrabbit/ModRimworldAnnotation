using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000E0C RID: 3596
	public class LordToilData_Siege : LordToilData
	{
		// Token: 0x060051C9 RID: 20937 RVA: 0x001BCC84 File Offset: 0x001BAE84
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

		// Token: 0x04003467 RID: 13415
		public IntVec3 siegeCenter;

		// Token: 0x04003468 RID: 13416
		public float baseRadius = 16f;

		// Token: 0x04003469 RID: 13417
		public float blueprintPoints;

		// Token: 0x0400346A RID: 13418
		public float desiredBuilderFraction = 0.5f;

		// Token: 0x0400346B RID: 13419
		public List<Blueprint> blueprints = new List<Blueprint>();
	}
}
