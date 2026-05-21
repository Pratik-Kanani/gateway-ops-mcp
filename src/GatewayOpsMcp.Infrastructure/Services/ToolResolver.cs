using GatewayOpsMcp.Core.Interfaces;
using GatewayOpsMcp.Core.Models;

namespace GatewayOpsMcp.Infrastructure.Services;

public class ToolResolver : IToolResolver
{
    private readonly IToolRegistry _registry;
    private readonly ISemanticDictionary _dictionary;
    private const int MinimumScore = 15;

    public ToolResolver(
        IToolRegistry registry,
        ISemanticDictionary dictionary)
    {
        _registry = registry;
        _dictionary = dictionary;
    }

    public ToolResolutionResult Resolve(
    string input)
    {
        input = input.ToLowerInvariant();

        var scoredTools =
            new List<(IMcpTool Tool, int Score)>();

        foreach (var tool in _registry.GetAll())
        {
            var score =
                CalculateScore(input, tool);

            foreach (var capability in tool.Capabilities)
            {
                if (input.Contains(
                        capability.Name.Replace("_", " "),
                        StringComparison.OrdinalIgnoreCase))
                {
                    score += 15;
                }

                if (input.Contains(
                        capability.Category,
                        StringComparison.OrdinalIgnoreCase))
                {
                    score += 5;
                }
            }

            scoredTools.Add((tool, score));
        }

        var ordered =
            scoredTools
                .OrderByDescending(x => x.Score)
                .ToList();

        var (Tool, Score) = ordered.FirstOrDefault();

        // no confident match
        if (Tool == null ||
            Score < MinimumScore)
        {
            return new ToolResolutionResult
            {
                Tool = null,
                Score = Score
            };
        }

        // ambiguity detection
        var ambiguous =
            ordered
                .Where(x =>
                    Score - x.Score <= 5)
                .ToList();

        if (ambiguous.Count > 1)
        {
            return new ToolResolutionResult
            {
                IsAmbiguous = true,
                Score = Score,
                CandidateTools =
                    [.. ambiguous.Select(x => x.Tool.Name)]
            };
        }

        return new ToolResolutionResult
        {
            Tool = Tool,
            Score = Score
        };
    }

    private int CalculateScore(
        string input,
        IMcpTool tool)
    {
        var score = 0;

        // keyword matches
        foreach (var keyword in tool.Metadata.Keywords)
        {
            if (input.Contains(
                    keyword,
                    StringComparison.OrdinalIgnoreCase))
            {
                score += 10;
            }
        }

        // example similarity
        foreach (var example in tool.Metadata.Examples)
        {
            var words = example
                .ToLowerInvariant()
                .Split(' ');

            foreach (var word in words)
            {
                if (input.Contains(word))
                {
                    score += 1;
                }
            }
        }
        var semanticMatches = 0;
        foreach (var entity in _dictionary.GetEntities())
        {
            foreach (var synonym in entity.Synonyms)
            {
                if (input.Contains(
                        synonym,
                        StringComparison.OrdinalIgnoreCase))
                {
                    semanticMatches++;
                    if (entity.CanonicalValue
                        == tool.Name)
                    {
                        score += 25;
                        score += semanticMatches * 5;
                    }
                }
            }
        }

        return score;
    }
}