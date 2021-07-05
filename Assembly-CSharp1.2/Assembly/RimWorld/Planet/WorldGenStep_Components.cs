using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200209A RID: 8346
	public class WorldGenStep_Components : WorldGenStep
	{
		// Token: 0x17001A24 RID: 6692
		// (get) Token: 0x0600B0E3 RID: 45283 RVA: 0x00072F70 File Offset: 0x00071170
		public override int SeedPart
		{
			get
			{
				return 508565678;
			}
		}

		// Token: 0x0600B0E4 RID: 45284 RVA: 0x00072F77 File Offset: 0x00071177
		public override void GenerateFresh(string seed)
		{
			Find.World.ConstructComponents();
		}

		// Token: 0x0600B0E5 RID: 45285 RVA: 0x00072F83 File Offset: 0x00071183
		public override void GenerateWithoutWorldData(string seed)
		{
			this.GenerateFromScribe(seed);
		}

		// Token: 0x0600B0E6 RID: 45286 RVA: 0x00072F8C File Offset: 0x0007118C
		public override void GenerateFromScribe(string seed)
		{
			Find.World.ConstructComponents();
			Find.World.ExposeComponents();
		}
	}
}
