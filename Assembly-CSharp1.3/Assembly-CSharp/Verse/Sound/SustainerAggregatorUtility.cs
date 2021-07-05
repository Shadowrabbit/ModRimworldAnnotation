using System;

namespace Verse.Sound
{
	// Token: 0x0200057E RID: 1406
	public static class SustainerAggregatorUtility
	{
		// Token: 0x06002948 RID: 10568 RVA: 0x000F9D98 File Offset: 0x000F7F98
		public static Sustainer AggregateOrSpawnSustainerFor(ISizeReporter reporter, SoundDef def, SoundInfo info)
		{
			Sustainer sustainer = null;
			foreach (Sustainer sustainer2 in Find.SoundRoot.sustainerManager.AllSustainers)
			{
				if (sustainer2.def == def && sustainer2.info.Maker.Map == info.Maker.Map && sustainer2.info.Maker.Cell.InHorDistOf(info.Maker.Cell, SustainerAggregatorUtility.AggregateRadius))
				{
					sustainer = sustainer2;
					break;
				}
			}
			if (sustainer == null)
			{
				sustainer = def.TrySpawnSustainer(info);
			}
			else
			{
				sustainer.Maintain();
			}
			if (sustainer.externalParams.sizeAggregator == null)
			{
				sustainer.externalParams.sizeAggregator = new SoundSizeAggregator();
			}
			sustainer.externalParams.sizeAggregator.RegisterReporter(reporter);
			return sustainer;
		}

		// Token: 0x0400199A RID: 6554
		private static float AggregateRadius = 12f;
	}
}
