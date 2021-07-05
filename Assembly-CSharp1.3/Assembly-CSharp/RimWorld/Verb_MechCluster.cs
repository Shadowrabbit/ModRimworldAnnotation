using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001513 RID: 5395
	public class Verb_MechCluster : Verb_CastBase
	{
		// Token: 0x06008079 RID: 32889 RVA: 0x002D8304 File Offset: 0x002D6504
		protected override bool TryCastShot()
		{
			if (Faction.OfMechanoids == null)
			{
				Messages.Message("MessageNoFactionForVerbMechCluster".Translate(), this.caster, MessageTypeDefOf.RejectInput, null, false);
				return false;
			}
			if (this.currentTarget.HasThing && this.currentTarget.Thing.Map != this.caster.Map)
			{
				return false;
			}
			MechClusterUtility.SpawnCluster(this.currentTarget.Cell, this.caster.Map, MechClusterGenerator.GenerateClusterSketch(2500f, this.caster.Map, true, true), true, false, null);
			CompReloadable reloadableCompSource = base.ReloadableCompSource;
			if (reloadableCompSource != null)
			{
				reloadableCompSource.UsedOnce();
			}
			return true;
		}

		// Token: 0x0600807A RID: 32890 RVA: 0x002D7E6B File Offset: 0x002D606B
		public override float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
		{
			needLOSToCenter = false;
			return 23f;
		}

		// Token: 0x04004FFD RID: 20477
		public const float Points = 2500f;
	}
}
