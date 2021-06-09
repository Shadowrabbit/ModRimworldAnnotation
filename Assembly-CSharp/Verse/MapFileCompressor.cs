using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000242 RID: 578
	public class MapFileCompressor : IExposable
	{
		// Token: 0x06000EC6 RID: 3782 RVA: 0x000111B4 File Offset: 0x0000F3B4
		public MapFileCompressor(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000EC7 RID: 3783 RVA: 0x000111C3 File Offset: 0x0000F3C3
		public void ExposeData()
		{
			DataExposeUtility.ByteArray(ref this.compressedData, "compressedThingMap");
		}

		// Token: 0x06000EC8 RID: 3784 RVA: 0x000111D5 File Offset: 0x0000F3D5
		public void BuildCompressedString()
		{
			this.compressibilityDecider = new CompressibilityDecider(this.map);
			this.compressibilityDecider.DetermineReferences();
			this.compressedData = MapSerializeUtility.SerializeUshort(this.map, new Func<IntVec3, ushort>(this.HashValueForSquare));
		}

		// Token: 0x06000EC9 RID: 3785 RVA: 0x000B415C File Offset: 0x000B235C
		private ushort HashValueForSquare(IntVec3 curSq)
		{
			ushort num = 0;
			foreach (Thing thing in this.map.thingGrid.ThingsAt(curSq))
			{
				if (thing.IsSaveCompressible())
				{
					if (num != 0)
					{
						Log.Error(string.Concat(new object[]
						{
							"Found two compressible things in ",
							curSq,
							". The last was ",
							thing
						}), false);
					}
					num = thing.def.shortHash;
				}
			}
			return num;
		}

		// Token: 0x06000ECA RID: 3786 RVA: 0x000B41F8 File Offset: 0x000B23F8
		public IEnumerable<Thing> ThingsToSpawnAfterLoad()
		{
			Dictionary<ushort, ThingDef> thingDefsByShortHash = new Dictionary<ushort, ThingDef>();
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDefsByShortHash.ContainsKey(thingDef.shortHash))
				{
					Log.Error(string.Concat(new object[]
					{
						"Hash collision between ",
						thingDef,
						" and  ",
						thingDefsByShortHash[thingDef.shortHash],
						": both have short hash ",
						thingDef.shortHash
					}), false);
				}
				else
				{
					thingDefsByShortHash.Add(thingDef.shortHash, thingDef);
				}
			}
			int major = VersionControl.MajorFromVersionString(ScribeMetaHeaderUtility.loadedGameVersion);
			int minor = VersionControl.MinorFromVersionString(ScribeMetaHeaderUtility.loadedGameVersion);
			List<Thing> loadables = new List<Thing>();
			MapSerializeUtility.LoadUshort(this.compressedData, this.map, delegate(IntVec3 c, ushort val)
			{
				if (val == 0)
				{
					return;
				}
				ThingDef thingDef2 = BackCompatibility.BackCompatibleThingDefWithShortHash_Force(val, major, minor);
				if (thingDef2 == null)
				{
					try
					{
						thingDef2 = thingDefsByShortHash[val];
					}
					catch (KeyNotFoundException)
					{
						ThingDef thingDef3 = BackCompatibility.BackCompatibleThingDefWithShortHash(val);
						if (thingDef3 != null)
						{
							thingDef2 = thingDef3;
							thingDefsByShortHash.Add(val, thingDef3);
						}
						else
						{
							Log.Error("Map compressor decompression error: No thingDef with short hash " + val + ". Adding as null to dictionary.", false);
							thingDefsByShortHash.Add(val, null);
						}
					}
				}
				if (thingDef2 != null)
				{
					try
					{
						Thing thing = ThingMaker.MakeThing(thingDef2, null);
						thing.SetPositionDirect(c);
						loadables.Add(thing);
					}
					catch (Exception arg)
					{
						Log.Error("Could not instantiate compressed thing: " + arg, false);
					}
				}
			});
			return loadables;
		}

		// Token: 0x04000C28 RID: 3112
		private Map map;

		// Token: 0x04000C29 RID: 3113
		private byte[] compressedData;

		// Token: 0x04000C2A RID: 3114
		public CompressibilityDecider compressibilityDecider;
	}
}
