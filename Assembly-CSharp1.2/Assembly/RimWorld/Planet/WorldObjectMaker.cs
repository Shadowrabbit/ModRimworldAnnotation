using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020021B0 RID: 8624
	public static class WorldObjectMaker
	{
		// Token: 0x0600B85C RID: 47196 RVA: 0x00077878 File Offset: 0x00075A78
		public static WorldObject MakeWorldObject(WorldObjectDef def)
		{
			WorldObject worldObject = (WorldObject)Activator.CreateInstance(def.worldObjectClass);
			worldObject.def = def;
			worldObject.ID = Find.UniqueIDsManager.GetNextWorldObjectID();
			worldObject.creationGameTicks = Find.TickManager.TicksGame;
			worldObject.PostMake();
			return worldObject;
		}
	}
}
