using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017FF RID: 6143
	public static class WorldObjectMaker
	{
		// Token: 0x06008F73 RID: 36723 RVA: 0x00335A2D File Offset: 0x00333C2D
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
