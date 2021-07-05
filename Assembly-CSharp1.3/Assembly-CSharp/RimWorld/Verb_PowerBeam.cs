using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001517 RID: 5399
	public class Verb_PowerBeam : Verb_CastBase
	{
		// Token: 0x0600808D RID: 32909 RVA: 0x002D8E90 File Offset: 0x002D7090
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

		// Token: 0x0600808E RID: 32910 RVA: 0x002D8F39 File Offset: 0x002D7139
		public override float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
		{
			needLOSToCenter = false;
			return 15f;
		}

		// Token: 0x04005001 RID: 20481
		private const int DurationTicks = 600;
	}
}
