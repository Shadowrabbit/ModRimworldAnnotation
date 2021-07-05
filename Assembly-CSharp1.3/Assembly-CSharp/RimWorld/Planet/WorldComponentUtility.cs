using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200177D RID: 6013
	public static class WorldComponentUtility
	{
		// Token: 0x06008ABE RID: 35518 RVA: 0x0031C738 File Offset: 0x0031A938
		public static void WorldComponentUpdate(World world)
		{
			List<WorldComponent> components = world.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].WorldComponentUpdate();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString());
				}
			}
		}

		// Token: 0x06008ABF RID: 35519 RVA: 0x0031C788 File Offset: 0x0031A988
		public static void WorldComponentTick(World world)
		{
			List<WorldComponent> components = world.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].WorldComponentTick();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString());
				}
			}
		}

		// Token: 0x06008AC0 RID: 35520 RVA: 0x0031C7D8 File Offset: 0x0031A9D8
		public static void FinalizeInit(World world)
		{
			List<WorldComponent> components = world.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].FinalizeInit();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString());
				}
			}
		}
	}
}
