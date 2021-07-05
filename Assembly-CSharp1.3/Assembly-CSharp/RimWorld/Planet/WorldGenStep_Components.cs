using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001786 RID: 6022
	public class WorldGenStep_Components : WorldGenStep
	{
		// Token: 0x170016A5 RID: 5797
		// (get) Token: 0x06008AE7 RID: 35559 RVA: 0x0031DF28 File Offset: 0x0031C128
		public override int SeedPart
		{
			get
			{
				return 508565678;
			}
		}

		// Token: 0x06008AE8 RID: 35560 RVA: 0x0031DF2F File Offset: 0x0031C12F
		public override void GenerateFresh(string seed)
		{
			Find.World.ConstructComponents();
		}

		// Token: 0x06008AE9 RID: 35561 RVA: 0x0031DF3B File Offset: 0x0031C13B
		public override void GenerateWithoutWorldData(string seed)
		{
			this.GenerateFromScribe(seed);
		}

		// Token: 0x06008AEA RID: 35562 RVA: 0x0031DF44 File Offset: 0x0031C144
		public override void GenerateFromScribe(string seed)
		{
			Find.World.ConstructComponents();
			Find.World.ExposeComponents();
		}
	}
}
