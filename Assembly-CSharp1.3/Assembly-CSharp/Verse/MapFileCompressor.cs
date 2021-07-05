using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000186 RID: 390
	public class MapFileCompressor : IExposable
	{
		// Token: 0x06000B18 RID: 2840 RVA: 0x0003C5B4 File Offset: 0x0003A7B4
		public MapFileCompressor(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000B19 RID: 2841 RVA: 0x0003C5C3 File Offset: 0x0003A7C3
		public void ExposeData()
		{
			DataExposeUtility.ByteArray(ref this.compressedData, "compressedThingMap");
		}

		// Token: 0x06000B1A RID: 2842 RVA: 0x0003C5D5 File Offset: 0x0003A7D5
		public void BuildCompressedString()
		{
			this.compressibilityDecider = new CompressibilityDecider(this.map);
			this.compressibilityDecider.DetermineReferences();
			this.compressedData = MapSerializeUtility.SerializeUshort(this.map, new Func<IntVec3, ushort>(this.HashValueForSquare));
		}

		// Token: 0x06000B1B RID: 2843 RVA: 0x0003C610 File Offset: 0x0003A810
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
						}));
					}
					num = thing.def.shortHash;
				}
			}
			return num;
		}

		// Token: 0x06000B1C RID: 2844 RVA: 0x0003C6A8 File Offset: 0x0003A8A8
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
					}));
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
							Log.Error("Map compressor decompression error: No thingDef with short hash " + val + ". Adding as null to dictionary.");
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
						Log.Error("Could not instantiate compressed thing: " + arg);
					}
				}
			});
			return loadables;
		}

		// Token: 0x0400093C RID: 2364
		private Map map;

		// Token: 0x0400093D RID: 2365
		private byte[] compressedData;

		// Token: 0x0400093E RID: 2366
		public CompressibilityDecider compressibilityDecider;
	}
}
