using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200150F RID: 5391
	public class Verb_Bombardment : Verb_CastBase
	{
		// Token: 0x06008065 RID: 32869 RVA: 0x002D7DC8 File Offset: 0x002D5FC8
		protected override bool TryCastShot()
		{
			if (this.currentTarget.HasThing && this.currentTarget.Thing.Map != this.caster.Map)
			{
				return false;
			}
			Bombardment bombardment = (Bombardment)GenSpawn.Spawn(ThingDefOf.Bombardment, this.currentTarget.Cell, this.caster.Map, WipeMode.Vanish);
			bombardment.duration = 540;
			bombardment.instigator = this.caster;
			bombardment.weaponDef = ((base.EquipmentSource != null) ? base.EquipmentSource.def : null);
			CompReloadable reloadableCompSource = base.ReloadableCompSource;
			if (reloadableCompSource != null)
			{
				reloadableCompSource.UsedOnce();
			}
			return true;
		}

		// Token: 0x06008066 RID: 32870 RVA: 0x002D7E6B File Offset: 0x002D606B
		public override float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
		{
			needLOSToCenter = false;
			return 23f;
		}

		// Token: 0x04004FFB RID: 20475
		public const int DurationTicks = 540;
	}
}
