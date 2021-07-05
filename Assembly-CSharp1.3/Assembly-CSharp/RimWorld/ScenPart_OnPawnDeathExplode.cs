using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FFF RID: 4095
	public class ScenPart_OnPawnDeathExplode : ScenPart
	{
		// Token: 0x06006073 RID: 24691 RVA: 0x0020DC17 File Offset: 0x0020BE17
		public override void Randomize()
		{
			this.radius = (float)Rand.RangeInclusive(3, 8) - 0.1f;
			this.damage = this.PossibleDamageDefs().RandomElement<DamageDef>();
		}

		// Token: 0x06006074 RID: 24692 RVA: 0x0020DC3E File Offset: 0x0020BE3E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.radius, "radius", 0f, false);
			Scribe_Defs.Look<DamageDef>(ref this.damage, "damage");
		}

		// Token: 0x06006075 RID: 24693 RVA: 0x0020DC6C File Offset: 0x0020BE6C
		public override string Summary(Scenario scen)
		{
			return "ScenPart_OnPawnDeathExplode".Translate(this.damage.label, this.radius.ToString());
		}

		// Token: 0x06006076 RID: 24694 RVA: 0x0020DCA0 File Offset: 0x0020BEA0
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

		// Token: 0x06006077 RID: 24695 RVA: 0x0020DD44 File Offset: 0x0020BF44
		public override void Notify_PawnDied(Corpse corpse)
		{
			if (corpse.Spawned)
			{
				GenExplosion.DoExplosion(corpse.Position, corpse.Map, this.radius, this.damage, null, -1, -1f, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false, null, null);
			}
		}

		// Token: 0x06006078 RID: 24696 RVA: 0x0020DDA0 File Offset: 0x0020BFA0
		private IEnumerable<DamageDef> PossibleDamageDefs()
		{
			yield return DamageDefOf.Bomb;
			yield return DamageDefOf.Flame;
			yield break;
		}

		// Token: 0x04003730 RID: 14128
		private float radius = 5.9f;

		// Token: 0x04003731 RID: 14129
		private DamageDef damage;

		// Token: 0x04003732 RID: 14130
		private string radiusBuf;
	}
}
