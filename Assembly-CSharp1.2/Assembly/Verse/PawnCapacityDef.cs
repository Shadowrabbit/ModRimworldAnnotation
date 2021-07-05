using System;

namespace Verse
{
	// Token: 0x02000155 RID: 341
	public class PawnCapacityDef : Def
	{
		// Token: 0x17000196 RID: 406
		// (get) Token: 0x060008B0 RID: 2224 RVA: 0x0000CDEE File Offset: 0x0000AFEE
		public PawnCapacityWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (PawnCapacityWorker)Activator.CreateInstance(this.workerClass);
				}
				return this.workerInt;
			}
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x0000CE14 File Offset: 0x0000B014
		public string GetLabelFor(Pawn pawn)
		{
			return this.GetLabelFor(pawn.RaceProps.IsFlesh, pawn.RaceProps.Humanlike);
		}

		// Token: 0x060008B2 RID: 2226 RVA: 0x00096F64 File Offset: 0x00095164
		public string GetLabelFor(bool isFlesh, bool isHumanlike)
		{
			if (isHumanlike)
			{
				return this.label;
			}
			if (isFlesh)
			{
				if (!this.labelAnimals.NullOrEmpty())
				{
					return this.labelAnimals;
				}
				return this.label;
			}
			else
			{
				if (!this.labelMechanoids.NullOrEmpty())
				{
					return this.labelMechanoids;
				}
				return this.label;
			}
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x0000CE32 File Offset: 0x0000B032
		public bool CanShowOnPawn(Pawn p)
		{
			if (p.def.race.Humanlike)
			{
				return this.showOnHumanlikes;
			}
			if (p.def.race.Animal)
			{
				return this.showOnAnimals;
			}
			return this.showOnMechanoids;
		}

		// Token: 0x0400070A RID: 1802
		public int listOrder;

		// Token: 0x0400070B RID: 1803
		public Type workerClass = typeof(PawnCapacityWorker);

		// Token: 0x0400070C RID: 1804
		[MustTranslate]
		public string labelMechanoids = "";

		// Token: 0x0400070D RID: 1805
		[MustTranslate]
		public string labelAnimals = "";

		// Token: 0x0400070E RID: 1806
		public bool showOnHumanlikes = true;

		// Token: 0x0400070F RID: 1807
		public bool showOnAnimals = true;

		// Token: 0x04000710 RID: 1808
		public bool showOnMechanoids = true;

		// Token: 0x04000711 RID: 1809
		public bool lethalFlesh;

		// Token: 0x04000712 RID: 1810
		public bool lethalMechanoids;

		// Token: 0x04000713 RID: 1811
		public float minForCapable;

		// Token: 0x04000714 RID: 1812
		public float minValue;

		// Token: 0x04000715 RID: 1813
		public bool zeroIfCannotBeAwake;

		// Token: 0x04000716 RID: 1814
		public bool showOnCaravanHealthTab;

		// Token: 0x04000717 RID: 1815
		[Unsaved(false)]
		private PawnCapacityWorker workerInt;
	}
}
