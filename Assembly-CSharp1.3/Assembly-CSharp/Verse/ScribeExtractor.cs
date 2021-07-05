using System;
using System.Xml;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000329 RID: 809
	public static class ScribeExtractor
	{
		// Token: 0x06001705 RID: 5893 RVA: 0x00087B14 File Offset: 0x00085D14
		public static T ValueFromNode<T>(XmlNode subNode, T defaultValue)
		{
			if (subNode == null)
			{
				return defaultValue;
			}
			XmlAttribute xmlAttribute = subNode.Attributes["IsNull"];
			T result;
			if (xmlAttribute != null && xmlAttribute.Value.ToLower() == "true")
			{
				result = default(T);
				return result;
			}
			try
			{
				try
				{
					return ParseHelper.FromString<T>(subNode.InnerText);
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Exception parsing node ",
						subNode.OuterXml,
						" into a ",
						typeof(T),
						":\n",
						ex.ToString()
					}));
				}
				result = default(T);
			}
			catch (Exception arg)
			{
				Log.Error("Exception loading XML: " + arg);
				result = defaultValue;
			}
			return result;
		}

		// Token: 0x06001706 RID: 5894 RVA: 0x00087BF4 File Offset: 0x00085DF4
		public static T DefFromNode<T>(XmlNode subNode) where T : Def, new()
		{
			if (subNode == null || subNode.InnerText == null || subNode.InnerText == "null")
			{
				return default(T);
			}
			string text = BackCompatibility.BackCompatibleDefName(typeof(T), subNode.InnerText, false, subNode);
			T namedSilentFail = DefDatabase<T>.GetNamedSilentFail(text);
			if (namedSilentFail == null && !BackCompatibility.WasDefRemoved(subNode.InnerText, typeof(T)))
			{
				if (text == subNode.InnerText)
				{
					Log.Error(string.Concat(new object[]
					{
						"Could not load reference to ",
						typeof(T),
						" named ",
						subNode.InnerText
					}));
				}
				else
				{
					Log.Error(string.Concat(new object[]
					{
						"Could not load reference to ",
						typeof(T),
						" named ",
						subNode.InnerText,
						" after compatibility-conversion to ",
						text
					}));
				}
				BackCompatibility.PostCouldntLoadDef(subNode.InnerText);
			}
			return namedSilentFail;
		}

		// Token: 0x06001707 RID: 5895 RVA: 0x00087D00 File Offset: 0x00085F00
		public static T DefFromNodeUnsafe<T>(XmlNode subNode)
		{
			return (T)((object)GenGeneric.InvokeStaticGenericMethod(typeof(ScribeExtractor), typeof(T), "DefFromNode", new object[]
			{
				subNode
			}));
		}

		// Token: 0x06001708 RID: 5896 RVA: 0x00087D30 File Offset: 0x00085F30
		public static T SaveableFromNode<T>(XmlNode subNode, object[] ctorArgs)
		{
			if (Scribe.mode != LoadSaveMode.LoadingVars)
			{
				Log.Error("Called SaveableFromNode(), but mode is " + Scribe.mode);
				return default(T);
			}
			if (subNode == null)
			{
				return default(T);
			}
			XmlAttribute xmlAttribute = subNode.Attributes["IsNull"];
			T result;
			if (xmlAttribute != null && xmlAttribute.Value.ToLower() == "true")
			{
				result = default(T);
			}
			else
			{
				try
				{
					XmlAttribute xmlAttribute2 = subNode.Attributes["Class"];
					string text = (xmlAttribute2 != null) ? xmlAttribute2.Value : typeof(T).FullName;
					Type type = BackCompatibility.GetBackCompatibleType(typeof(T), text, subNode);
					if (type == null)
					{
						Type bestFallbackType = ScribeExtractor.GetBestFallbackType<T>(subNode);
						Log.Error(string.Concat(new object[]
						{
							"Could not find class ",
							text,
							" while resolving node ",
							subNode.Name,
							". Trying to use ",
							bestFallbackType,
							" instead. Full node: ",
							subNode.OuterXml
						}));
						type = bestFallbackType;
					}
					if (type.IsAbstract)
					{
						throw new ArgumentException("Can't load abstract class " + type);
					}
					IExposable exposable = (IExposable)Activator.CreateInstance(type, ctorArgs);
					bool flag = typeof(T).IsValueType || typeof(Name).IsAssignableFrom(typeof(T));
					if (!flag)
					{
						Scribe.loader.crossRefs.RegisterForCrossRefResolve(exposable);
					}
					XmlNode curXmlParent = Scribe.loader.curXmlParent;
					IExposable curParent = Scribe.loader.curParent;
					string curPathRelToParent = Scribe.loader.curPathRelToParent;
					Scribe.loader.curXmlParent = subNode;
					Scribe.loader.curParent = exposable;
					Scribe.loader.curPathRelToParent = null;
					try
					{
						exposable.ExposeData();
					}
					finally
					{
						Scribe.loader.curXmlParent = curXmlParent;
						Scribe.loader.curParent = curParent;
						Scribe.loader.curPathRelToParent = curPathRelToParent;
					}
					if (!flag)
					{
						Scribe.loader.initer.RegisterForPostLoadInit(exposable);
					}
					result = (T)((object)exposable);
				}
				catch (Exception ex)
				{
					result = default(T);
					Log.Error(string.Concat(new object[]
					{
						"SaveableFromNode exception: ",
						ex,
						"\nSubnode:\n",
						subNode.OuterXml
					}));
				}
			}
			return result;
		}

		// Token: 0x06001709 RID: 5897 RVA: 0x00087FC4 File Offset: 0x000861C4
		private static Type GetBestFallbackType<T>(XmlNode node)
		{
			if (typeof(Thing).IsAssignableFrom(typeof(T)))
			{
				ThingDef thingDef = ScribeExtractor.TryFindDef<ThingDef>(node, "def");
				if (thingDef != null)
				{
					return thingDef.thingClass;
				}
			}
			else if (typeof(Hediff).IsAssignableFrom(typeof(T)))
			{
				HediffDef hediffDef = ScribeExtractor.TryFindDef<HediffDef>(node, "def");
				if (hediffDef != null)
				{
					return hediffDef.hediffClass;
				}
			}
			else if (typeof(Ability).IsAssignableFrom(typeof(T)))
			{
				AbilityDef abilityDef = ScribeExtractor.TryFindDef<AbilityDef>(node, "def");
				if (abilityDef != null)
				{
					return abilityDef.abilityClass;
				}
			}
			else if (typeof(Thought).IsAssignableFrom(typeof(T)))
			{
				ThoughtDef thoughtDef = ScribeExtractor.TryFindDef<ThoughtDef>(node, "def");
				if (thoughtDef != null)
				{
					return thoughtDef.thoughtClass;
				}
			}
			return typeof(T);
		}

		// Token: 0x0600170A RID: 5898 RVA: 0x000880A4 File Offset: 0x000862A4
		private static TDef TryFindDef<TDef>(XmlNode node, string defNodeName) where TDef : Def, new()
		{
			XmlElement xmlElement = node[defNodeName];
			if (xmlElement == null)
			{
				return default(TDef);
			}
			return DefDatabase<TDef>.GetNamedSilentFail(BackCompatibility.BackCompatibleDefName(typeof(TDef), xmlElement.InnerText, false, null));
		}

		// Token: 0x0600170B RID: 5899 RVA: 0x000880E4 File Offset: 0x000862E4
		public static LocalTargetInfo LocalTargetInfoFromNode(XmlNode node, string label, LocalTargetInfo defaultValue)
		{
			LoadIDsWantedBank loadIDs = Scribe.loader.crossRefs.loadIDs;
			if (node != null && Scribe.EnterNode(label))
			{
				try
				{
					string innerText = node.InnerText;
					if (innerText.Length != 0 && innerText[0] == '(')
					{
						loadIDs.RegisterLoadIDReadFromXml(null, typeof(Thing), "thing");
						return new LocalTargetInfo(IntVec3.FromString(innerText));
					}
					loadIDs.RegisterLoadIDReadFromXml(innerText, typeof(Thing), "thing");
					return LocalTargetInfo.Invalid;
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
			loadIDs.RegisterLoadIDReadFromXml(null, typeof(Thing), label + "/thing");
			return defaultValue;
		}

		// Token: 0x0600170C RID: 5900 RVA: 0x000881A0 File Offset: 0x000863A0
		public static TargetInfo TargetInfoFromNode(XmlNode node, string label, TargetInfo defaultValue)
		{
			LoadIDsWantedBank loadIDs = Scribe.loader.crossRefs.loadIDs;
			if (node != null && Scribe.EnterNode(label))
			{
				try
				{
					string innerText = node.InnerText;
					if (innerText.Length != 0 && innerText[0] == '(')
					{
						string str;
						string targetLoadID;
						ScribeExtractor.ExtractCellAndMapPairFromTargetInfo(innerText, out str, out targetLoadID);
						loadIDs.RegisterLoadIDReadFromXml(null, typeof(Thing), "thing");
						loadIDs.RegisterLoadIDReadFromXml(targetLoadID, typeof(Map), "map");
						return new TargetInfo(IntVec3.FromString(str), null, true);
					}
					loadIDs.RegisterLoadIDReadFromXml(innerText, typeof(Thing), "thing");
					loadIDs.RegisterLoadIDReadFromXml(null, typeof(Map), "map");
					return TargetInfo.Invalid;
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
			loadIDs.RegisterLoadIDReadFromXml(null, typeof(Thing), label + "/thing");
			loadIDs.RegisterLoadIDReadFromXml(null, typeof(Map), label + "/map");
			return defaultValue;
		}

		// Token: 0x0600170D RID: 5901 RVA: 0x000882B8 File Offset: 0x000864B8
		public static GlobalTargetInfo GlobalTargetInfoFromNode(XmlNode node, string label, GlobalTargetInfo defaultValue)
		{
			LoadIDsWantedBank loadIDs = Scribe.loader.crossRefs.loadIDs;
			if (node != null && Scribe.EnterNode(label))
			{
				try
				{
					string innerText = node.InnerText;
					if (innerText.Length != 0 && innerText[0] == '(')
					{
						string str;
						string targetLoadID;
						ScribeExtractor.ExtractCellAndMapPairFromTargetInfo(innerText, out str, out targetLoadID);
						loadIDs.RegisterLoadIDReadFromXml(null, typeof(Thing), "thing");
						loadIDs.RegisterLoadIDReadFromXml(targetLoadID, typeof(Map), "map");
						loadIDs.RegisterLoadIDReadFromXml(null, typeof(WorldObject), "worldObject");
						return new GlobalTargetInfo(IntVec3.FromString(str), null, true);
					}
					int tile;
					if (int.TryParse(innerText, out tile))
					{
						loadIDs.RegisterLoadIDReadFromXml(null, typeof(Thing), "thing");
						loadIDs.RegisterLoadIDReadFromXml(null, typeof(Map), "map");
						loadIDs.RegisterLoadIDReadFromXml(null, typeof(WorldObject), "worldObject");
						return new GlobalTargetInfo(tile);
					}
					if (innerText.Length != 0 && innerText[0] == '@')
					{
						loadIDs.RegisterLoadIDReadFromXml(null, typeof(Thing), "thing");
						loadIDs.RegisterLoadIDReadFromXml(null, typeof(Map), "map");
						loadIDs.RegisterLoadIDReadFromXml(innerText.Substring(1), typeof(WorldObject), "worldObject");
						return GlobalTargetInfo.Invalid;
					}
					loadIDs.RegisterLoadIDReadFromXml(innerText, typeof(Thing), "thing");
					loadIDs.RegisterLoadIDReadFromXml(null, typeof(Map), "map");
					loadIDs.RegisterLoadIDReadFromXml(null, typeof(WorldObject), "worldObject");
					return GlobalTargetInfo.Invalid;
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
			loadIDs.RegisterLoadIDReadFromXml(null, typeof(Thing), label + "/thing");
			loadIDs.RegisterLoadIDReadFromXml(null, typeof(Map), label + "/map");
			loadIDs.RegisterLoadIDReadFromXml(null, typeof(WorldObject), label + "/worldObject");
			return defaultValue;
		}

		// Token: 0x0600170E RID: 5902 RVA: 0x000884E8 File Offset: 0x000866E8
		public static LocalTargetInfo ResolveLocalTargetInfo(LocalTargetInfo loaded, string label)
		{
			if (Scribe.EnterNode(label))
			{
				try
				{
					Thing thing = Scribe.loader.crossRefs.TakeResolvedRef<Thing>("thing");
					IntVec3 cell = loaded.Cell;
					if (thing != null)
					{
						return new LocalTargetInfo(thing);
					}
					return new LocalTargetInfo(cell);
				}
				finally
				{
					Scribe.ExitNode();
				}
				return loaded;
			}
			return loaded;
		}

		// Token: 0x0600170F RID: 5903 RVA: 0x00088548 File Offset: 0x00086748
		public static TargetInfo ResolveTargetInfo(TargetInfo loaded, string label)
		{
			if (Scribe.EnterNode(label))
			{
				try
				{
					Thing thing = Scribe.loader.crossRefs.TakeResolvedRef<Thing>("thing");
					Map map = Scribe.loader.crossRefs.TakeResolvedRef<Map>("map");
					IntVec3 cell = loaded.Cell;
					if (thing != null)
					{
						return new TargetInfo(thing);
					}
					if (cell.IsValid && map != null)
					{
						return new TargetInfo(cell, map, false);
					}
					return TargetInfo.Invalid;
				}
				finally
				{
					Scribe.ExitNode();
				}
				return loaded;
			}
			return loaded;
		}

		// Token: 0x06001710 RID: 5904 RVA: 0x000885D4 File Offset: 0x000867D4
		public static GlobalTargetInfo ResolveGlobalTargetInfo(GlobalTargetInfo loaded, string label)
		{
			if (Scribe.EnterNode(label))
			{
				try
				{
					Thing thing = Scribe.loader.crossRefs.TakeResolvedRef<Thing>("thing");
					Map map = Scribe.loader.crossRefs.TakeResolvedRef<Map>("map");
					WorldObject worldObject = Scribe.loader.crossRefs.TakeResolvedRef<WorldObject>("worldObject");
					IntVec3 cell = loaded.Cell;
					int tile = loaded.Tile;
					if (thing != null)
					{
						return new GlobalTargetInfo(thing);
					}
					if (worldObject != null)
					{
						return new GlobalTargetInfo(worldObject);
					}
					if (cell.IsValid)
					{
						if (map != null)
						{
							return new GlobalTargetInfo(cell, map, false);
						}
						return GlobalTargetInfo.Invalid;
					}
					else
					{
						if (tile >= 0)
						{
							return new GlobalTargetInfo(tile);
						}
						return GlobalTargetInfo.Invalid;
					}
				}
				finally
				{
					Scribe.ExitNode();
				}
				return loaded;
			}
			return loaded;
		}

		// Token: 0x06001711 RID: 5905 RVA: 0x000886AC File Offset: 0x000868AC
		public static BodyPartRecord BodyPartFromNode(XmlNode node, string label, BodyPartRecord defaultValue)
		{
			if (node != null && Scribe.EnterNode(label))
			{
				try
				{
					XmlAttribute xmlAttribute = node.Attributes["IsNull"];
					if (xmlAttribute != null && xmlAttribute.Value.ToLower() == "true")
					{
						return null;
					}
					BodyDef bodyDef = ScribeExtractor.DefFromNode<BodyDef>(Scribe.loader.curXmlParent["body"]);
					if (bodyDef == null)
					{
						return null;
					}
					XmlElement xmlElement = Scribe.loader.curXmlParent["index"];
					int index = (xmlElement != null) ? int.Parse(xmlElement.InnerText) : -1;
					index = BackCompatibility.GetBackCompatibleBodyPartIndex(bodyDef, index);
					return bodyDef.GetPartAtIndex(index);
				}
				finally
				{
					Scribe.ExitNode();
				}
				return defaultValue;
			}
			return defaultValue;
		}

		// Token: 0x06001712 RID: 5906 RVA: 0x00088770 File Offset: 0x00086970
		private static void ExtractCellAndMapPairFromTargetInfo(string str, out string cell, out string map)
		{
			int num = str.IndexOf(')');
			cell = str.Substring(0, num + 1);
			int num2 = str.IndexOf(',', num + 1);
			map = str.Substring(num2 + 1);
			map = map.TrimStart(new char[]
			{
				' '
			});
		}
	}
}
