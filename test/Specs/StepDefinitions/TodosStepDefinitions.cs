using MediatR;

namespace TodoApp.Specs.StepDefinitions;

[Binding]
public sealed class TodosStepDefinitions
{
    private readonly ITestService testService;

    public TodosStepDefinitions(ITestService testService)
    {
        this.testService = testService;
    }

    // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

    [Given("that you define a task to do")]
    public void GivenThatYouDefineATaskToDo()
    {
        
    }

    [When("you post it")]
    public void WhenYouPostIt()
    {
        
    }

    [Then("then an item should have been created")]
    public void ThenAnItemShouldHaveBeenCreated()
    {
        
    }
}