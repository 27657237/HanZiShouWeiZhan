using System.Collections.Generic;
using System.Xml;
using UnityGameFramework.Runtime;

/// <summary>
/// 构建报告解析器 - 封装XML解析逻辑
/// </summary>
public static class BuildReportParser
{
    public static BuildReport Parse(string xmlContent)
    {
        if (xmlContent.StartsWith("\uFEFF"))
        {
            xmlContent = xmlContent.Substring(1);
        }

        var doc = new XmlDocument();
        doc.LoadXml(xmlContent);

        // 定位到根节点下的BuildReport
        var buildReportNode = doc.DocumentElement.SelectSingleNode("BuildReport");
        if (buildReportNode == null)
        {
            Log.Error("XML中未找到BuildReport节点");
            return new BuildReport();
        }

        return new BuildReport
        {
            InternalResourceVersion = ParseVersion(buildReportNode), // 传递BuildReport节点
            ApplicableGameVersion = ParseGameVersion(buildReportNode),
            Resources = ParseResources(buildReportNode) // 传递BuildReport节点
        };
    }

    private static int ParseVersion(XmlNode buildReportNode)
    {
        // 在BuildReport节点下查找Summary/InternalResourceVersion
        var node = buildReportNode.SelectSingleNode("Summary/InternalResourceVersion");
        return node != null && int.TryParse(node.InnerText, out int version) ? version : 0;
    }

    private static string ParseGameVersion(XmlNode buildReportNode)
    {
        var node = buildReportNode.SelectSingleNode("Summary/ApplicableGameVersion");
        return node?.InnerText ?? "1.0";
    }

    private static List<ResourceInfo> ParseResources(XmlNode buildReportNode)
    {
        var resources = new List<ResourceInfo>();
        var resourcesNode = buildReportNode.SelectSingleNode("Resources");
        if (resourcesNode == null) return resources;

        var resourceNodes = resourcesNode.SelectNodes("Resource");
        foreach (XmlNode node in resourceNodes)
        {
            var resource = new ResourceInfo
            {
                Name = node.Attributes["Name"]?.Value,
                Extension = node.Attributes["Extension"]?.Value,
                LoadType = GetAttributeInt(node, "LoadType"),
                Packed = GetAttributeBool(node, "Packed")
            };

            var webGLNode = node.SelectSingleNode("Codes/WebGL");
            if (webGLNode != null)
            {
                resource.HashCode = webGLNode.Attributes?["HashCode"]?.Value;
                if (webGLNode.Attributes?["Length"] != null &&
                    long.TryParse(webGLNode.Attributes["Length"].Value, out long length))
                {
                    resource.Length = length;
                }
            }

            if (!string.IsNullOrEmpty(resource.Name) &&
                !string.IsNullOrEmpty(resource.HashCode) &&
                resource.Length > 0)
            {
                resources.Add(resource);
            }
        }
        return resources;
    }

    private static int GetAttributeInt(XmlNode node, string attrName, int defaultValue = 0)
    {
        return node.Attributes[attrName] != null &&
               int.TryParse(node.Attributes[attrName].Value, out int result)
               ? result : defaultValue;
    }

    private static bool GetAttributeBool(XmlNode node, string attrName, bool defaultValue = false)
    {
        return node.Attributes[attrName] != null &&
               bool.TryParse(node.Attributes[attrName].Value, out bool result)
               ? result : defaultValue;
    }
}