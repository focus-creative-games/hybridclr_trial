using HybridCLR.Editor.LinkGenerator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace HybridCLR.Editor.LinkGenerator
{

    public static class LinkGeneratorCommand
    {

        [MenuItem("HybridCLR/GenerateLinkXml")]
        public static void GenerateLinkXml()
        {
            var ls = SettingsUtil.LinkSettings;

            var allAssByNames = new Dictionary<string, Assembly>();
            foreach (var ass in AppDomain.CurrentDomain.GetAssemblies())
            {
                allAssByNames[ass.GetName().Name] = ass;
            }

            var hotfixAssembles = new List<Assembly>();
            foreach(var assName in SettingsUtil.HotUpdateAssemblies)
            {
                string assNameWithoutExt = Path.GetFileNameWithoutExtension(assName);
                if (allAssByNames.TryGetValue(assNameWithoutExt, out var ass))
                {
                    hotfixAssembles.Add(ass);
                }
                else
                {
                    throw new Exception($"assembly:{assName} 不存在");
                }
            }

            var analyzer = new Analyzer();
            var refTypes = analyzer.CollectRefs(hotfixAssembles);

            Debug.Log($"hotfix assembly count:{hotfixAssembles.Count}, ref type count:{refTypes.Count}");
            var linkXmlWriter = new LinkXmlWriter();
            linkXmlWriter.Write($"{Application.dataPath}/{ls.outputLinkFile}", refTypes);
        }
    }
}
