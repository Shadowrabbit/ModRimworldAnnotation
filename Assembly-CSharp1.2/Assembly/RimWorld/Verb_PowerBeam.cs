using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001D8F RID: 7567
	public class Verb_PowerBeam : Verb_CastBase
	{
		// Token: 0x0600A46D RID: 42093 RVA: 0x002FE11C File Offset: 0x002FC31C
		protected override bool TryCastShot()
		{
			if (this.currentTarget.HasThing && this.currentTarget.Thing.Map != this.caster.Map)
			{
				return false;
			}
			PowerBeam powerBeam = (PowerBeam)GenSpawn.Spawn(ThingDefOf.PowerBeam, this.currentTarget.Cell, this.caster.Map, WipeMode.Vanish);
			powerBeam.duration = 600;
			powerBeam.instigator = this.caster;
			powerBeam.weaponDef = ((base.EquipmentSource != null) ? base.EquipmentSource.def : null);
			powerBeam.StartStrike();
			CompReloadable reloadableCompSource = base.ReloadableCompSource;
			if (reloadableCompSource != null)
			{
				reloadableCompSource.UsedOnce();
			}
			return true;
		}

		// Token: 0x0600A46E RID: 42094 RVA: 0x0006D031 File Offset: 0x0006B231
		public override float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
		{
			needLOSToCenter = false;
			return 15f;
		}

		// Token: 0x04006F70 RID: 28528
		private const int DurationTicks = 600;
	}
}
