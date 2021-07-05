using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020008EC RID: 2284
	public static class ShipJobMaker
	{
		// Token: 0x06003BE5 RID: 15333 RVA: 0x0014DED1 File Offset: 0x0014C0D1
		public static ShipJob MakeShipJob(ShipJobDef def)
		{
			ShipJob shipJob = (ShipJob)Activator.CreateInstance(def.jobClass);
			shipJob.def = def;
			shipJob.loadID = Find.UniqueIDsManager.GetNextShipJobID();
			return shipJob;
		}
	}
}
