using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009D2 RID: 2514
	public class Thought_WeaponTrait : Thought_Memory
	{
		// Token: 0x17000AE2 RID: 2786
		// (get) Token: 0x06003E4A RID: 15946 RVA: 0x00154E67 File Offset: 0x00153067
		public override bool ShouldDiscard
		{
			get
			{
				return base.ShouldDiscard || !this.HasWeapon;
			}
		}

		// Token: 0x17000AE3 RID: 2787
		// (get) Token: 0x06003E4B RID: 15947 RVA: 0x00154E7C File Offset: 0x0015307C
		public override string LabelCap
		{
			get
			{
				return base.CurStage.label.Formatted(this.Name.Named("WEAPON")).CapitalizeFirst();
			}
		}

		// Token: 0x17000AE4 RID: 2788
		// (get) Token: 0x06003E4C RID: 15948 RVA: 0x00154EB8 File Offset: 0x001530B8
		public override string Description
		{
			get
			{
				return base.CurStage.description.Formatted(this.Name.Named("WEAPON")).CapitalizeFirst();
			}
		}

		// Token: 0x17000AE5 RID: 2789
		// (get) Token: 0x06003E4D RID: 15949 RVA: 0x00154EF2 File Offset: 0x001530F2
		protected bool HasWeapon
		{
			get
			{
				return this.weapon != null && !this.weapon.Destroyed;
			}
		}

		// Token: 0x17000AE6 RID: 2790
		// (get) Token: 0x06003E4E RID: 15950 RVA: 0x00154F0C File Offset: 0x0015310C
		private string Name
		{
			get
			{
				CompGeneratedNames compGeneratedNames = this.weapon.TryGetComp<CompGeneratedNames>();
				if (compGeneratedNames != null)
				{
					return compGeneratedNames.Name;
				}
				return this.weapon.LabelNoCount;
			}
		}

		// Token: 0x06003E4F RID: 15951 RVA: 0x00154F3A File Offset: 0x0015313A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<ThingWithComps>(ref this.weapon, "weapon", false);
		}

		// Token: 0x040020EA RID: 8426
		public ThingWithComps weapon;
	}
}
