using System;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001D89 RID: 7561
	public class Verb_MechCluster : Verb_CastBase
	{
		// Token: 0x0600A44A RID: 42058 RVA: 0x002FD28C File Offset: 0x002FB48C
		protected override bool TryCastShot()
		{
			if (this.currentTarget.HasThing && this.currentTarget.Thing.Map != this.caster.Map)
			{
				return false;
			}
			MechClusterUtility.SpawnCluster(this.currentTarget.Cell, this.caster.Map, MechClusterGenerator.GenerateClusterSketch_NewTemp(2500f, this.caster.Map, true, true), true, false, null);
			CompReloadable reloadableCompSource = base.ReloadableCompSource;
			if (reloadableCompSource != null)
			{
				reloadableCompSource.UsedOnce();
			}
			return true;
		}

		// Token: 0x0600A44B RID: 42059 RVA: 0x0006CDE8 File Offset: 0x0006AFE8
		public override float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
		{
			needLOSToCenter = false;
			return 23f;
		}

		// Token: 0x04006F5C RID: 28508
		public const float Points = 2500f;
	}
}
