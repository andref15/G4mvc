using Microsoft.CodeAnalysis;

namespace G4mvc.Test.Utils;

internal static class SolutionTransforms
{
    extension(List<Func<Solution, ProjectId, Solution>> solutionTransforms)
    {
        public void EnableNullable()
        {
            solutionTransforms.Add((solution, projectId) =>
            {
                var project = solution.GetProject(projectId)!;
                project.ParseOptions!.WithFeatures([KeyValuePair.Create("nullable", "enable")]);

                return solution;
            });
        }
    }
}
