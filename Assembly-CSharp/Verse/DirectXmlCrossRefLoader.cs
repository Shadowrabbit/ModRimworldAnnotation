using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;

namespace Verse
{
	// Token: 0x0200047F RID: 1151
	public static class DirectXmlCrossRefLoader
	{
		// Token: 0x17000584 RID: 1412
		// (get) Token: 0x06001D0D RID: 7437 RVA: 0x0001A33D File Offset: 0x0001853D
		public static bool LoadingInProgress
		{
			get
			{
				return DirectXmlCrossRefLoader.wantedRefs.Count > 0;
			}
		}

		// Token: 0x06001D0E RID: 7438 RVA: 0x000F2F6C File Offset: 0x000F116C
		public static void RegisterObjectWantsCrossRef(object wanter, FieldInfo fi, string targetDefName, string mayRequireMod = null, Type assumeFieldType = null)
		{
			DeepProfiler.Start("RegisterObjectWantsCrossRef (object, FieldInfo, string)");
			try
			{
				DirectXmlCrossRefLoader.WantedRefForObject item = new DirectXmlCrossRefLoader.WantedRefForObject(wanter, fi, targetDefName, mayRequireMod, assumeFieldType);
				DirectXmlCrossRefLoader.wantedRefs.Add(item);
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x06001D0F RID: 7439 RVA: 0x000F2FB4 File Offset: 0x000F11B4
		public static void RegisterObjectWantsCrossRef(object wanter, string fieldName, string targetDefName, string mayRequireMod = null, Type overrideFieldType = null)
		{
			DeepProfiler.Start("RegisterObjectWantsCrossRef (object,string,string)");
			try
			{
				DirectXmlCrossRefLoader.WantedRefForObject item = new DirectXmlCrossRefLoader.WantedRefForObject(wanter, wanter.GetType().GetField(fieldName), targetDefName, mayRequireMod, overrideFieldType);
				DirectXmlCrossRefLoader.wantedRefs.Add(item);
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x06001D10 RID: 7440 RVA: 0x000F3008 File Offset: 0x000F1208
		public static void RegisterObjectWantsCrossRef(object wanter, string fieldName, XmlNode parentNode, string mayRequireMod = null, Type overrideFieldType = null)
		{
			DeepProfiler.Start("RegisterObjectWantsCrossRef (object,string,XmlNode)");
			try
			{
				string text = mayRequireMod;
				if (mayRequireMod == null)
				{
					XmlAttributeCollection attributes = parentNode.Attributes;
					if (attributes == null)
					{
						text = null;
					}
					else
					{
						XmlAttribute xmlAttribute = attributes["MayRequire"];
						text = ((xmlAttribute != null) ? xmlAttribute.Value.ToLower() : null);
					}
				}
				string mayRequireMod2 = text;
				DirectXmlCrossRefLoader.WantedRefForObject item = new DirectXmlCrossRefLoader.WantedRefForObject(wanter, wanter.GetType().GetField(fieldName), parentNode.Name, mayRequireMod2, overrideFieldType);
				DirectXmlCrossRefLoader.wantedRefs.Add(item);
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x06001D11 RID: 7441 RVA: 0x000F308C File Offset: 0x000F128C
		public static void RegisterListWantsCrossRef<T>(List<T> wanterList, string targetDefName, object debugWanterInfo = null, string mayRequireMod = null)
		{
			DeepProfiler.Start("RegisterListWantsCrossRef");
			try
			{
				DirectXmlCrossRefLoader.WantedRef wantedRef;
				DirectXmlCrossRefLoader.WantedRefForList<T> wantedRefForList;
				if (!DirectXmlCrossRefLoader.wantedListDictRefs.TryGetValue(wanterList, out wantedRef))
				{
					wantedRefForList = new DirectXmlCrossRefLoader.WantedRefForList<T>(wanterList, debugWanterInfo);
					DirectXmlCrossRefLoader.wantedListDictRefs.Add(wanterList, wantedRefForList);
					DirectXmlCrossRefLoader.wantedRefs.Add(wantedRefForList);
				}
				else
				{
					wantedRefForList = (DirectXmlCrossRefLoader.WantedRefForList<T>)wantedRef;
				}
				wantedRefForList.AddWantedListEntry(targetDefName, mayRequireMod);
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x06001D12 RID: 7442 RVA: 0x000F30FC File Offset: 0x000F12FC
		public static void RegisterDictionaryWantsCrossRef<K, V>(Dictionary<K, V> wanterDict, XmlNode entryNode, object debugWanterInfo = null)
		{
			DeepProfiler.Start("RegisterDictionaryWantsCrossRef");
			try
			{
				DirectXmlCrossRefLoader.WantedRef wantedRef;
				DirectXmlCrossRefLoader.WantedRefForDictionary<K, V> wantedRefForDictionary;
				if (!DirectXmlCrossRefLoader.wantedListDictRefs.TryGetValue(wanterDict, out wantedRef))
				{
					wantedRefForDictionary = new DirectXmlCrossRefLoader.WantedRefForDictionary<K, V>(wanterDict, debugWanterInfo);
					DirectXmlCrossRefLoader.wantedRefs.Add(wantedRefForDictionary);
					DirectXmlCrossRefLoader.wantedListDictRefs.Add(wanterDict, wantedRefForDictionary);
				}
				else
				{
					wantedRefForDictionary = (DirectXmlCrossRefLoader.WantedRefForDictionary<K, V>)wantedRef;
				}
				wantedRefForDictionary.AddWantedDictEntry(entryNode);
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x06001D13 RID: 7443 RVA: 0x000F316C File Offset: 0x000F136C
		public static T TryResolveDef<T>(string defName, FailMode failReportMode, object debugWanterInfo = null)
		{
			DeepProfiler.Start("TryResolveDef");
			T result;
			try
			{
				T t = (T)((object)GenDefDatabase.GetDefSilentFail(typeof(T), defName, true));
				if (t != null)
				{
					result = t;
				}
				else
				{
					if (failReportMode == FailMode.LogErrors)
					{
						string text = string.Concat(new object[]
						{
							"Could not resolve cross-reference to ",
							typeof(T),
							" named ",
							defName
						});
						if (debugWanterInfo != null)
						{
							text = text + " (wanter=" + debugWanterInfo.ToStringSafe<object>() + ")";
						}
						Log.Error(text, false);
					}
					result = default(T);
				}
			}
			finally
			{
				DeepProfiler.End();
			}
			return result;
		}

		// Token: 0x06001D14 RID: 7444 RVA: 0x000F321C File Offset: 0x000F141C
		public static void Clear()
		{
			DeepProfiler.Start("Clear");
			try
			{
				DirectXmlCrossRefLoader.wantedRefs.Clear();
				DirectXmlCrossRefLoader.wantedListDictRefs.Clear();
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x06001D15 RID: 7445 RVA: 0x000F3260 File Offset: 0x000F1460
		public static void ResolveAllWantedCrossReferences(FailMode failReportMode)
		{
			DeepProfiler.Start("ResolveAllWantedCrossReferences");
			try
			{
				HashSet<DirectXmlCrossRefLoader.WantedRef> resolvedRefs = new HashSet<DirectXmlCrossRefLoader.WantedRef>();
				object resolvedRefsLock = new object();
				DeepProfiler.enabled = false;
				GenThreading.ParallelForEach<DirectXmlCrossRefLoader.WantedRef>(DirectXmlCrossRefLoader.wantedRefs, delegate(DirectXmlCrossRefLoader.WantedRef wantedRef)
				{
					if (wantedRef.TryResolve(failReportMode))
					{
						object resolvedRefsLock = resolvedRefsLock;
						lock (resolvedRefsLock)
						{
							resolvedRefs.Add(wantedRef);
						}
					}
				}, -1);
				foreach (DirectXmlCrossRefLoader.WantedRef wantedRef2 in resolvedRefs)
				{
					wantedRef2.Apply();
				}
				DirectXmlCrossRefLoader.wantedRefs.RemoveAll((DirectXmlCrossRefLoader.WantedRef x) => resolvedRefs.Contains(x));
				DeepProfiler.enabled = true;
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x040014C3 RID: 5315
		private static List<DirectXmlCrossRefLoader.WantedRef> wantedRefs = new List<DirectXmlCrossRefLoader.WantedRef>();

		// Token: 0x040014C4 RID: 5316
		private static Dictionary<object, DirectXmlCrossRefLoader.WantedRef> wantedListDictRefs = new Dictionary<object, DirectXmlCrossRefLoader.WantedRef>();

		// Token: 0x02000480 RID: 1152
		private abstract class WantedRef
		{
			// Token: 0x06001D17 RID: 7447
			public abstract bool TryResolve(FailMode failReportMode);

			// Token: 0x06001D18 RID: 7448 RVA: 0x00006A05 File Offset: 0x00004C05
			public virtual void Apply()
			{
			}

			// Token: 0x040014C5 RID: 5317
			public object wanter;
		}

		// Token: 0x02000481 RID: 1153
		private class WantedRefForObject : DirectXmlCrossRefLoader.WantedRef
		{
			// Token: 0x17000585 RID: 1413
			// (get) Token: 0x06001D1A RID: 7450 RVA: 0x0001A362 File Offset: 0x00018562
			private bool BadCrossRefAllowed
			{
				get
				{
					return !this.mayRequireMod.NullOrEmpty() && !ModsConfig.IsActive(this.mayRequireMod);
				}
			}

			// Token: 0x06001D1B RID: 7451 RVA: 0x0001A381 File Offset: 0x00018581
			public WantedRefForObject(object wanter, FieldInfo fi, string targetDefName, string mayRequireMod = null, Type overrideFieldType = null)
			{
				this.wanter = wanter;
				this.fi = fi;
				this.defName = targetDefName;
				this.mayRequireMod = mayRequireMod;
				this.overrideFieldType = overrideFieldType;
			}

			// Token: 0x06001D1C RID: 7452 RVA: 0x000F333C File Offset: 0x000F153C
			public override bool TryResolve(FailMode failReportMode)
			{
				if (this.fi == null)
				{
					Log.Error("Trying to resolve null field for def named " + this.defName.ToStringSafe<string>(), false);
					return false;
				}
				Type type = this.overrideFieldType ?? this.fi.FieldType;
				this.resolvedDef = GenDefDatabase.GetDefSilentFail(type, this.defName, true);
				if (this.resolvedDef == null)
				{
					if (failReportMode == FailMode.LogErrors && !this.BadCrossRefAllowed)
					{
						Log.Error(string.Concat(new object[]
						{
							"Could not resolve cross-reference: No ",
							type,
							" named ",
							this.defName.ToStringSafe<string>(),
							" found to give to ",
							this.wanter.GetType(),
							" ",
							this.wanter.ToStringSafe<object>()
						}), false);
					}
					return false;
				}
				SoundDef soundDef = this.resolvedDef as SoundDef;
				if (soundDef != null && soundDef.isUndefined)
				{
					Log.Warning(string.Concat(new object[]
					{
						"Could not resolve cross-reference: No ",
						type,
						" named ",
						this.defName.ToStringSafe<string>(),
						" found to give to ",
						this.wanter.GetType(),
						" ",
						this.wanter.ToStringSafe<object>(),
						" (using undefined sound instead)"
					}), false);
				}
				this.fi.SetValue(this.wanter, this.resolvedDef);
				return true;
			}

			// Token: 0x040014C6 RID: 5318
			public FieldInfo fi;

			// Token: 0x040014C7 RID: 5319
			public string defName;

			// Token: 0x040014C8 RID: 5320
			public Def resolvedDef;

			// Token: 0x040014C9 RID: 5321
			public string mayRequireMod;

			// Token: 0x040014CA RID: 5322
			public Type overrideFieldType;
		}

		// Token: 0x02000482 RID: 1154
		private class WantedRefForList<T> : DirectXmlCrossRefLoader.WantedRef
		{
			// Token: 0x06001D1D RID: 7453 RVA: 0x0001A3AE File Offset: 0x000185AE
			public WantedRefForList(object wanter, object debugWanterInfo)
			{
				this.wanter = wanter;
				this.debugWanterInfo = debugWanterInfo;
			}

			// Token: 0x06001D1E RID: 7454 RVA: 0x000F34AC File Offset: 0x000F16AC
			public void AddWantedListEntry(string newTargetDefName, string mayRequireMod = null)
			{
				if (!mayRequireMod.NullOrEmpty() && this.mayRequireMods == null)
				{
					this.mayRequireMods = new List<string>();
					for (int i = 0; i < this.defNames.Count; i++)
					{
						this.mayRequireMods.Add(null);
					}
				}
				this.defNames.Add(newTargetDefName);
				if (this.mayRequireMods != null)
				{
					this.mayRequireMods.Add(mayRequireMod);
				}
			}

			// Token: 0x06001D1F RID: 7455 RVA: 0x000F3518 File Offset: 0x000F1718
			public override bool TryResolve(FailMode failReportMode)
			{
				bool flag = false;
				for (int i = 0; i < this.defNames.Count; i++)
				{
					bool flag2 = this.mayRequireMods != null && i < this.mayRequireMods.Count && !this.mayRequireMods[i].NullOrEmpty() && !ModsConfig.IsActive(this.mayRequireMods[i]);
					T t = DirectXmlCrossRefLoader.TryResolveDef<T>(this.defNames[i], flag2 ? FailMode.Silent : failReportMode, this.debugWanterInfo);
					if (t != null)
					{
						((List<T>)this.wanter).Add(t);
						this.defNames.RemoveAt(i);
						if (this.mayRequireMods != null && i < this.mayRequireMods.Count)
						{
							this.mayRequireMods.RemoveAt(i);
						}
						i--;
					}
					else
					{
						flag = true;
					}
				}
				return !flag;
			}

			// Token: 0x040014CB RID: 5323
			private List<string> defNames = new List<string>();

			// Token: 0x040014CC RID: 5324
			private List<string> mayRequireMods;

			// Token: 0x040014CD RID: 5325
			private object debugWanterInfo;
		}

		// Token: 0x02000483 RID: 1155
		private class WantedRefForDictionary<K, V> : DirectXmlCrossRefLoader.WantedRef
		{
			// Token: 0x06001D20 RID: 7456 RVA: 0x0001A3CF File Offset: 0x000185CF
			public WantedRefForDictionary(object wanter, object debugWanterInfo)
			{
				this.wanter = wanter;
				this.debugWanterInfo = debugWanterInfo;
			}

			// Token: 0x06001D21 RID: 7457 RVA: 0x0001A3FB File Offset: 0x000185FB
			public void AddWantedDictEntry(XmlNode entryNode)
			{
				this.wantedDictRefs.Add(entryNode);
			}

			// Token: 0x06001D22 RID: 7458 RVA: 0x000F35F8 File Offset: 0x000F17F8
			public override bool TryResolve(FailMode failReportMode)
			{
				failReportMode = FailMode.LogErrors;
				bool flag = typeof(Def).IsAssignableFrom(typeof(K));
				bool flag2 = typeof(Def).IsAssignableFrom(typeof(V));
				foreach (XmlNode xmlNode in this.wantedDictRefs)
				{
					XmlNode xmlNode2 = xmlNode["key"];
					XmlNode xmlNode3 = xmlNode["value"];
					object first;
					if (flag)
					{
						first = DirectXmlCrossRefLoader.TryResolveDef<K>(xmlNode2.InnerText, failReportMode, this.debugWanterInfo);
					}
					else
					{
						first = xmlNode2;
					}
					object second;
					if (flag2)
					{
						second = DirectXmlCrossRefLoader.TryResolveDef<V>(xmlNode3.InnerText, failReportMode, this.debugWanterInfo);
					}
					else
					{
						second = xmlNode3;
					}
					this.makingData.Add(new Pair<object, object>(first, second));
				}
				return true;
			}

			// Token: 0x06001D23 RID: 7459 RVA: 0x000F36EC File Offset: 0x000F18EC
			public override void Apply()
			{
				Dictionary<K, V> dictionary = (Dictionary<K, V>)this.wanter;
				dictionary.Clear();
				foreach (Pair<object, object> pair in this.makingData)
				{
					try
					{
						object obj = pair.First;
						object obj2 = pair.Second;
						if (obj is XmlNode)
						{
							obj = DirectXmlToObject.ObjectFromXml<K>(obj as XmlNode, true);
						}
						if (obj2 is XmlNode)
						{
							obj2 = DirectXmlToObject.ObjectFromXml<V>(obj2 as XmlNode, true);
						}
						dictionary.Add((K)((object)obj), (V)((object)obj2));
					}
					catch
					{
						Log.Error(string.Concat(new object[]
						{
							"Failed to load key/value pair: ",
							pair.First,
							", ",
							pair.Second
						}), false);
					}
				}
			}

			// Token: 0x040014CE RID: 5326
			private List<XmlNode> wantedDictRefs = new List<XmlNode>();

			// Token: 0x040014CF RID: 5327
			private object debugWanterInfo;

			// Token: 0x040014D0 RID: 5328
			private List<Pair<object, object>> makingData = new List<Pair<object, object>>();
		}
	}
}
