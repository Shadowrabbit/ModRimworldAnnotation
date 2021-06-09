using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020015E2 RID: 5602
	public class ScenPart_OnPawnDeathExplode : ScenPart
	{
		// Token: 0x060079C0 RID: 31168 RVA: 0x00051EF1 File Offset: 0x000500F1
		public override void Randomize()
		{
			this.radius = (float)Rand.RangeInclusive(3, 8) - 0.1f;
			this.damage = this.PossibleDamageDefs().RandomElement<DamageDef>();
		}

		// Token: 0x060079C1 RID: 31169 RVA: 0x00051F18 File Offset: 0x00050118
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.radius, "radius", 0f, false);
			Scribe_Defs.Look<DamageDef>(ref this.damage, "damage");
		}

		// Token: 0x060079C2 RID: 31170 RVA: 0x00051F46 File Offset: 0x00050146
		public override string Summary(Scenario scen)
		{
			return "ScenPart_OnPawnDeathExplode".Translate(this.damage.label, this.radius.ToString());
		}

		// Token: 0x060079C3 RID: 31171 RVA: 0x0024DAC4 File Offset: 0x0024BCC4
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 2f);
			Widgets.TextFieldNumericLabeled<float>(scenPartRect.TopHalf(), "radius".Translate(), ref this.radius, ref this.radiusBuf, 0f, 1E+09f);
			if (Widgets.ButtonText(scenPartRect.BottomHalf(), this.damage.LabelCap, true, true, true))
			{
				FloatMenuUtility.MakeMenu<DamageDef>(this.PossibleDamageDefs(), (DamageDef d) => d.LabelCap, (DamageDef d) => delegate()
				{
					this.damage = d;
				});
			}
		}

		// Token: 0x060079C4 RID: 31172 RVA: 0x0024DB68 File Offset: 0x0024BD68
		public override void Notify_PawnDied(Corpse corpse)
		{
			if (corpse.Spawned)
			{
				GenExplosion.DoExplosion(corpse.Position, corpse.Map, this.radius, this.damage, null, -1, -1f, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false, null, null);
			}
		}

		// Token: 0x060079C5 RID: 31173 RVA: 0x00051F77 File Offset: 0x00050177
		private IEnumerable<DamageDef> PossibleDamageDefs()
		{
			yield return DamageDefOf.Bomb;
			yield return DamageDefOf.Flame;
			yield break;
		}

		// Token: 0x04004FFE RID: 20478
		private float radius = 5.9f;

		// Token: 0x04004FFF RID: 20479
		private DamageDef damage;

		// Token: 0x04005000 RID: 20480
		private string radiusBuf;
	}
}
