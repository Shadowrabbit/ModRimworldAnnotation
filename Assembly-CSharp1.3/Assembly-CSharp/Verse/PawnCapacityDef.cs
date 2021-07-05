using System;

namespace Verse
{
	// Token: 0x020000E4 RID: 228
	public class PawnCapacityDef : Def
	{
		// Token: 0x17000113 RID: 275
		// (get) Token: 0x0600063D RID: 1597 RVA: 0x0001F03E File Offset: 0x0001D23E
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

		// Token: 0x0600063E RID: 1598 RVA: 0x0001F064 File Offset: 0x0001D264
		public string GetLabelFor(Pawn pawn)
		{
			return this.GetLabelFor(pawn.RaceProps.IsFlesh, pawn.RaceProps.Humanlike);
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x0001F084 File Offset: 0x0001D284
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

		// Token: 0x06000640 RID: 1600 RVA: 0x0001F0D3 File Offset: 0x0001D2D3
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

		// Token: 0x0400050C RID: 1292
		public int listOrder;

		// Token: 0x0400050D RID: 1293
		public Type workerClass = typeof(PawnCapacityWorker);

		// Token: 0x0400050E RID: 1294
		[MustTranslate]
		public string labelMechanoids = "";

		// Token: 0x0400050F RID: 1295
		[MustTranslate]
		public string labelAnimals = "";

		// Token: 0x04000510 RID: 1296
		public bool showOnHumanlikes = true;

		// Token: 0x04000511 RID: 1297
		public bool showOnAnimals = true;

		// Token: 0x04000512 RID: 1298
		public bool showOnMechanoids = true;

		// Token: 0x04000513 RID: 1299
		public bool lethalFlesh;

		// Token: 0x04000514 RID: 1300
		public bool lethalMechanoids;

		// Token: 0x04000515 RID: 1301
		public float minForCapable;

		// Token: 0x04000516 RID: 1302
		public float minValue;

		// Token: 0x04000517 RID: 1303
		public bool zeroIfCannotBeAwake;

		// Token: 0x04000518 RID: 1304
		public bool showOnCaravanHealthTab;

		// Token: 0x04000519 RID: 1305
		[Unsaved(false)]
		private PawnCapacityWorker workerInt;
	}
}
