using System;
using System.Xml;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x020004AE RID: 1198
	public static class ScribeExtractor
	{
		// Token: 0x06001DCD RID: 7629 RVA: 0x000F7AC8 File Offset: 0x000F5CC8
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
					}), false);
				}
				result = default(T);
			}
			catch (Exception arg)
			{
				Log.Error("Exception loading XML: " + arg, false);
				result = defaultValue;
			}
			return result;
		}

		// Token: 0x06001DCE RID: 7630 RVA: 0x000F7BAC File Offset: 0x000F5DAC
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
					}), false);
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
					}), false);
				}
				BackCompatibility.PostCouldntLoadDef(subNode.InnerText);
			}
			return namedSilentFail;
		}

		// Token: 0x06001DCF RID: 7631 RVA: 0x0001AA35 File Offset: 0x00018C35
		public static T DefFromNodeUnsafe<T>(XmlNode subNode)
		{
			return (T)((object)GenGeneric.InvokeStaticGenericMethod(typeof(ScribeExtractor), typeof(T), "DefFromNode", new object[]
			{
				subNode
			}));
		}

		// Token: 0x06001DD0 RID: 7632 RVA: 0x000F7CBC File Offset: 0x000F5EBC
		public static T SaveableFromNode<T>(XmlNode subNode, object[] ctorArgs)
		{
			if (Scribe.mode != LoadSaveMode.LoadingVars)
			{
				Log.Error("Called SaveableFromNode(), but mode is " + Scribe.mode, false);
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
						}), false);
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
					}), false);
				}
			}
			return result;
		}

		// Token: 0x06001DD1 RID: 7633 RVA: 0x000F7F50 File Offset: 0x000F6150
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

		// Token: 0x06001DD2 RID: 7634 RVA: 0x000F8030 File Offset: 0x000F6230
		private static TDef TryFindDef<TDef>(XmlNode node, string defNodeName) where TDef : Def, new()
		{
			XmlElement xmlElement = node[defNodeName];
			if (xmlElement == null)
			{
				return default(TDef);
			}
			return DefDatabase<TDef>.GetNamedSilentFail(BackCompatibility.BackCompatibleDefName(typeof(TDef), xmlElement.InnerText, false, null));
		}

		// Token: 0x06001DD3 RID: 7635 RVA: 0x000F8070 File Offset: 0x000F6270
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

		// Token: 0x06001DD4 RID: 7636 RVA: 0x000F812C File Offset: 0x000F632C
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

		// Token: 0x06001DD5 RID: 7637 RVA: 0x000F8244 File Offset: 0x000F6444
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

		// Token: 0x06001DD6 RID: 7638 RVA: 0x000F8474 File Offset: 0x000F6674
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

		// Token: 0x06001DD7 RID: 7639 RVA: 0x000F84D4 File Offset: 0x000F66D4
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

		// Token: 0x06001DD8 RID: 7640 RVA: 0x000F8560 File Offset: 0x000F6760
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

		// Token: 0x06001DD9 RID: 7641 RVA: 0x000F8638 File Offset: 0x000F6838
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

		// Token: 0x06001DDA RID: 7642 RVA: 0x000F86FC File Offset: 0x000F68FC
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
