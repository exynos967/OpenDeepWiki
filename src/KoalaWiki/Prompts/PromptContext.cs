namespace KoalaWiki.Prompts;

public class PromptContext
{
    private static readonly string PromptPath = Path.Combine("./", "Prompts");


    /// <summary>
    /// 仓库提示词
    /// </summary>
    private static string WarehousePrompt => Path.Combine(PromptPath, "Warehouse");

    private static string ChatPrompt => Path.Combine(PromptPath, "Chat");

    private static string Mem0Prompt => Path.Combine(PromptPath, "Mem0");

    public static async Task<string> Chat(
        string name,
        KernelArguments args, string model)
    {
        var fileName = name + ".md";

        // 将model插入fileName到.md前面，如果文件不存在则删除model
        if (model != null && !string.IsNullOrEmpty(model))
        {
            var modelFileName = $"{model}_{fileName}";
            if (!File.Exists(Path.Combine(ChatPrompt, modelFileName)))
            {
                // 如果文件不存在，则删除model
                fileName = name + ".md";
                Log.Logger.Debug(
                    "Model-specific chat prompt not found: {ModelFileName}. Falling back to: {FileName}",
                    modelFileName,
                    fileName);
            }
            else
            {
                fileName = modelFileName;
            }
        }

        if (!File.Exists(Path.Combine(ChatPrompt, fileName)))
        {
            throw new FileNotFoundException($"Chat prompt not found name:{fileName}");
        }

        var values = await File.ReadAllTextAsync(Path.Combine(ChatPrompt, fileName));

        return args.Aggregate(values,
            (current, value) =>
                current.Replace("{{$" + value.Key + "}}", value.Value?.ToString(),
                    StringComparison.CurrentCultureIgnoreCase)) + Prompt.Language;
    }

    public static async Task<string> Mem0(string name,
        KernelArguments args, string model)
    {
        var fileName = name + ".md";

        // 将model插入fileName到.md前面，如果文件不存在则删除model
        if (model != null && !string.IsNullOrEmpty(model))
        {
            var modelFileName = $"{model}_{fileName}";
            if (!File.Exists(Path.Combine(Mem0Prompt, modelFileName)))
            {
                // 如果文件不存在，则删除model
                fileName = name + ".md";
                Log.Logger.Debug(
                    "Model-specific Mem0 prompt not found: {ModelFileName}. Falling back to: {FileName}",
                    modelFileName,
                    fileName);
            }
            else
            {
                fileName = modelFileName;
            }
        }

        if (!File.Exists(Path.Combine(Mem0Prompt, fileName)))
        {
            throw new FileNotFoundException($"Chat prompt not found name:{fileName}");
        }

        var values = await File.ReadAllTextAsync(Path.Combine(Mem0Prompt, fileName));

        return args.Aggregate(values,
            (current, value) =>
                current.Replace("{{$" + value.Key + "}}", value.Value?.ToString(),
                    StringComparison.CurrentCultureIgnoreCase)) + Prompt.Language;
    }

    // 创建一个默认的索引
    public static async Task<string> Warehouse(
        string name,
        KernelArguments args, string model)
    {
        var fileName = name + ".md";

        // 将model插入fileName到.md前面，如果文件不存在则删除model
        if (model != null && !string.IsNullOrEmpty(model))
        {
            var modelFileName = $"{model}_{fileName}";
            if (!File.Exists(Path.Combine(WarehousePrompt, modelFileName)))
            {
                // 如果文件不存在，则删除model
                fileName = name + ".md";
                Log.Logger.Debug(
                    "Model-specific warehouse prompt not found: {ModelFileName}. Falling back to: {FileName}",
                    modelFileName,
                    fileName);
            }
            else
            {
                fileName = modelFileName;
            }
        }

        if (!File.Exists(Path.Combine(WarehousePrompt, fileName)))
        {
            throw new FileNotFoundException($"Warehouse prompt not found name:{fileName}");
        }

        var values = await File.ReadAllTextAsync(Path.Combine(WarehousePrompt, fileName));

        return args.Aggregate(values,
            (current, value) =>
                current.Replace("{{$" + value.Key + "}}", value.Value?.ToString(),
                    StringComparison.CurrentCultureIgnoreCase)) + Prompt.Language;
    }
}
