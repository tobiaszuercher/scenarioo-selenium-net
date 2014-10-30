using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scenarioo.Api;
using Scenarioo.Model.Docu.Entities;

namespace Scenarioo.SeleniumNetIntegration
{
    /// <summary>
    /// Reads Test information from the TestContext and creates for each TestClass a UseCase (name of the test class is the use case name). A scenario is 
    /// created for each test method.
    /// </summary>
    public class ScenariooMsTestIntegration
    {
        private string BranchName { get; set; }
        private string BuildName { get; set; }
        private string ScenarioName { get; set; }
        private string UseCaseName { get; set; }

        private ScenarioDocuWriter DocuWriter { get; set; }
        private ScenarioDocuReader DocuReader { get; set; }

        private static bool _allTestsGreen = true;
        private readonly TestContext _testContext;

        /// <summary>
        /// Branch and build informations are usually not known during the test execution. A default value is used which can be changed later.
        /// </summary>
        public ScenariooMsTestIntegration(TestContext testContext, string dirName = "scenarioo", string branchName = "master", string buildName = "build")
        {
            _testContext = testContext;

            // branch & build name not always required. probably add this information later when copying to destination directory.
            BranchName = branchName;
            BuildName = buildName;

            var basePath = new DirectoryInfo(dirName).FullName;

            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }

            DocuWriter = new ScenarioDocuWriter(basePath, BranchName, BuildName);
            DocuReader = new ScenarioDocuReader(basePath);

            DocuWriter.SaveBuildDescription(new Build() { Name = "build", Status = "Success", Date = DateTime.Now });
            DocuWriter.SaveBranchDescription(new Branch() { Name = "master" });
        }

        public void ScenarioStart()
        {
            UseCaseName = _testContext.FullyQualifiedTestClassName.Substring(_testContext.FullyQualifiedTestClassName.LastIndexOf('.') + 1);
            ScenarioName = _testContext.TestName;

            var u = new UseCase { Name = UseCaseName };
            DocuWriter.SaveUseCase(u);

            var scenario = new Scenario { Name = _testContext.TestName };
            DocuWriter.SaveScenario(u.Name, scenario);
        }

        public void ScenarioEnd()
        {
            var scenarioo = DocuReader.LoadScenario(BranchName, BuildName, UseCaseName, ScenarioName);
            scenarioo.Status = _testContext.CurrentTestOutcome == UnitTestOutcome.Passed ? "success" : "failed";
            DocuWriter.SaveScenario(UseCaseName, scenarioo);

            _allTestsGreen &= _testContext.CurrentTestOutcome == UnitTestOutcome.Passed;

            var useCase = DocuReader.LoadUseCase(BranchName, BuildName, UseCaseName);

            var curentTestOk = _testContext.CurrentTestOutcome == UnitTestOutcome.Passed;
            var useCaseStatus = useCase.Status == "success";
            
            useCase.Status = useCaseStatus && curentTestOk ? "success" : "failed";

            DocuWriter.SaveUseCase(useCase);
        }

        public void SaveStep(Step step, byte[] screenshot)
        {
            DocuWriter.SaveStep(UseCaseName, ScenarioName, step);
            DocuWriter.SaveScreenshot(UseCaseName, ScenarioName, step, screenshot);
        }
    }
}
