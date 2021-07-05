using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02001E23 RID: 7715
	public static class BaseGen
	{
		// Token: 0x1700196C RID: 6508
		// (get) Token: 0x0600A6E2 RID: 42722 RVA: 0x0006E51D File Offset: 0x0006C71D
		public static string CurrentSymbolPath
		{
			get
			{
				return BaseGen.currentSymbolPath;
			}
		}

		// Token: 0x0600A6E3 RID: 42723 RVA: 0x003080F8 File Offset: 0x003062F8
		public static void Reset()
		{
			BaseGen.rulesBySymbol.Clear();
			List<RuleDef> allDefsListForReading = DefDatabase<RuleDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				List<RuleDef> list;
				if (!BaseGen.rulesBySymbol.TryGetValue(allDefsListForReading[i].symbol, out list))
				{
					list = new List<RuleDef>();
					BaseGen.rulesBySymbol.Add(allDefsListForReading[i].symbol, list);
				}
				list.Add(allDefsListForReading[i]);
			}
		}

		// Token: 0x0600A6E4 RID: 42724 RVA: 0x0030816C File Offset: 0x0030636C
		public static void Generate()
		{
			if (BaseGen.working)
			{
				Log.Error("Cannot call Generate() while already generating. Nested calls are not allowed.", false);
				return;
			}
			BaseGen.working = true;
			BaseGen.currentSymbolPath = "";
			BaseGen.globalSettings.ClearResult();
			try
			{
				if (BaseGen.symbolStack.Empty)
				{
					Log.Warning("Symbol stack is empty.", false);
				}
				else if (BaseGen.globalSettings.map == null)
				{
					Log.Error("Called BaseGen.Resolve() with null map.", false);
				}
				else
				{
					int num = BaseGen.symbolStack.Count - 1;
					int num2 = 0;
					while (!BaseGen.symbolStack.Empty)
					{
						num2++;
						if (num2 > 100000)
						{
							Log.Error("Error in BaseGen: Too many iterations. Infinite loop?", false);
							break;
						}
						SymbolStack.Element element = BaseGen.symbolStack.Pop();
						BaseGen.currentSymbolPath = element.symbolPath;
						if (BaseGen.symbolStack.Count == num)
						{
							BaseGen.globalSettings.mainRect = element.resolveParams.rect;
							num--;
						}
						try
						{
							BaseGen.Resolve(element);
						}
						catch (Exception ex)
						{
							Log.Error(string.Concat(new object[]
							{
								"Error while resolving symbol \"",
								element.symbol,
								"\" with params=",
								element.resolveParams,
								"\n\nException: ",
								ex
							}), false);
						}
					}
				}
			}
			catch (Exception arg)
			{
				Log.Error("Error in BaseGen: " + arg, false);
			}
			finally
			{
				BaseGen.globalSettings.landingPadsGenerated = BaseGen.globalSettings.basePart_landingPadsResolved;
				BaseGen.working = false;
				BaseGen.symbolStack.Clear();
				BaseGen.globalSettings.Clear();
			}
		}

		// Token: 0x0600A6E5 RID: 42725 RVA: 0x0030833C File Offset: 0x0030653C
		private static void Resolve(SymbolStack.Element toResolve)
		{
			string symbol = toResolve.symbol;
			ResolveParams resolveParams = toResolve.resolveParams;
			BaseGen.tmpResolvers.Clear();
			List<RuleDef> list;
			if (BaseGen.rulesBySymbol.TryGetValue(symbol, out list))
			{
				for (int i = 0; i < list.Count; i++)
				{
					RuleDef ruleDef = list[i];
					for (int j = 0; j < ruleDef.resolvers.Count; j++)
					{
						SymbolResolver symbolResolver = ruleDef.resolvers[j];
						if (symbolResolver.CanResolve(resolveParams))
						{
							BaseGen.tmpResolvers.Add(symbolResolver);
						}
					}
				}
			}
			if (!BaseGen.tmpResolvers.Any<SymbolResolver>())
			{
				Log.Warning(string.Concat(new object[]
				{
					"Could not find any RuleDef for symbol \"",
					symbol,
					"\" with any resolver that could resolve ",
					resolveParams
				}), false);
				return;
			}
			BaseGen.tmpResolvers.RandomElementByWeight((SymbolResolver x) => x.selectionWeight).Resolve(resolveParams);
		}

		// Token: 0x04007130 RID: 28976
		public static GlobalSettings globalSettings = new GlobalSettings();

		// Token: 0x04007131 RID: 28977
		public static SymbolStack symbolStack = new SymbolStack();

		// Token: 0x04007132 RID: 28978
		private static Dictionary<string, List<RuleDef>> rulesBySymbol = new Dictionary<string, List<RuleDef>>();

		// Token: 0x04007133 RID: 28979
		private static bool working;

		// Token: 0x04007134 RID: 28980
		private static string currentSymbolPath;

		// Token: 0x04007135 RID: 28981
		private const int MaxResolvedSymbols = 100000;

		// Token: 0x04007136 RID: 28982
		private static List<SymbolResolver> tmpResolvers = new List<SymbolResolver>();
	}
}
