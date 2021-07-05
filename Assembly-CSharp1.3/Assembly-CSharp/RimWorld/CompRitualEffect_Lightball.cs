using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FC4 RID: 4036
	public class CompRitualEffect_Lightball : CompRitualEffect_IntervalSpawn
	{
		// Token: 0x17001055 RID: 4181
		// (get) Token: 0x06005F12 RID: 24338 RVA: 0x00208646 File Offset: 0x00206846
		public new CompProperties_RitualEffectLightball Props
		{
			get
			{
				return (CompProperties_RitualEffectLightball)this.props;
			}
		}

		// Token: 0x06005F13 RID: 24339 RVA: 0x000FE248 File Offset: 0x000FC448
		protected override Vector3 SpawnPos(LordJob_Ritual ritual)
		{
			return Vector3.zero;
		}

		// Token: 0x06005F14 RID: 24340 RVA: 0x00208654 File Offset: 0x00206854
		public override void SpawnFleck(LordJob_Ritual ritual, Vector3? forcedPos = null, float? exactRotation = null)
		{
			CompPowerTrader compPowerTrader = ritual.selectedTarget.Thing.TryGetComp<CompPowerTrader>();
			if (compPowerTrader == null || !compPowerTrader.PowerOn)
			{
				return;
			}
			float num = Rand.Range(0f, 360f);
			float num2 = num + 180f;
			float num3 = (num + num2) / 2f + (float)Rand.Range(-55, 55);
			Vector3 a = this.parent.ritual.selectedTarget.Cell.ToVector3Shifted();
			Vector3 b = Quaternion.AngleAxis(num, Vector3.up) * Vector3.forward * this.Props.radius;
			Vector3 b2 = Quaternion.AngleAxis(num2, Vector3.up) * Vector3.forward * this.Props.radius;
			Vector3 b3 = Quaternion.AngleAxis(num3, Vector3.up) * Vector3.forward * this.Props.radius;
			base.SpawnFleck(this.parent.ritual, new Vector3?(a + b), new float?(num - 45f));
			base.SpawnFleck(this.parent.ritual, new Vector3?(a + b2), new float?(num2 - 45f));
			base.SpawnFleck(this.parent.ritual, new Vector3?(a + b3), new float?(num3 - 45f));
			this.lastSpawnTick = GenTicks.TicksGame;
		}
	}
}
