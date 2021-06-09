using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EDF RID: 3807
	public class Thought_WeaponTrait : Thought_Memory
	{
		// Token: 0x17000CD0 RID: 3280
		// (get) Token: 0x06005438 RID: 21560 RVA: 0x0003A79D File Offset: 0x0003899D
		public override bool ShouldDiscard
		{
			get
			{
				return base.ShouldDiscard || !this.HasWeapon;
			}
		}

		// Token: 0x17000CD1 RID: 3281
		// (get) Token: 0x06005439 RID: 21561 RVA: 0x001C3028 File Offset: 0x001C1228
		public override string LabelCap
		{
			get
			{
				return base.CurStage.label.Formatted(this.Name.Named("WEAPON")).CapitalizeFirst();
			}
		}

		// Token: 0x17000CD2 RID: 3282
		// (get) Token: 0x0600543A RID: 21562 RVA: 0x001C3064 File Offset: 0x001C1264
		public override string Description
		{
			get
			{
				return base.CurStage.description.Formatted(this.Name.Named("WEAPON")).CapitalizeFirst();
			}
		}

		// Token: 0x17000CD3 RID: 3283
		// (get) Token: 0x0600543B RID: 21563 RVA: 0x0003A7B2 File Offset: 0x000389B2
		protected bool HasWeapon
		{
			get
			{
				return this.weapon != null && !this.weapon.Destroyed;
			}
		}

		// Token: 0x17000CD4 RID: 3284
		// (get) Token: 0x0600543C RID: 21564 RVA: 0x001C30A0 File Offset: 0x001C12A0
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

		// Token: 0x0600543D RID: 21565 RVA: 0x0003A7CC File Offset: 0x000389CC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<ThingWithComps>(ref this.weapon, "weapon", false);
		}

		// Token: 0x04003526 RID: 13606
		public ThingWithComps weapon;
	}
}
