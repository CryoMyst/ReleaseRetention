# Release Retention
## Octopus Deploy take home assessment.

# Result: Rejection for role Junior Developer!
## Feedback
What we are looking for in an ideal solution is one that has the code required to solve the problem, and not much more. The solution presented has a lot of additional code which does not directly contribute to solving the problem. The LastReleasedPolicy class contains most of the code that solves the problem at hand. The extra layers of abstraction and structure don't add value given the context and use case, and make the solution much harder to reason about. When we add code to a solution for problems we don't currently face, we are forced to think about and maintain that code "just in case" - a cost which can build up significantly over time in larger codebases.

When building a component like the one we ask for in this exercise, it can be useful to think about how it will be consumed. The use of interface abstractions instead of concrete models in the implementation makes it hard for consumers to use the component - they would need to build their own concrete models to pass into the component, rather than using models provided by the component. This overhead doesn't provide much value to the consumer.

When writing tests, it is important that others who read our tests can understand what scenario or behaviour is being tested. The tests in this submission do not make it clear what scenarios they cover in many cases, simply referring to "success" or "fails". It would be better if the tests had names that explained what scenario they were covering, and the scenarios related to a specific behaviour under test.

I would recommend the candidate focus on simplicity and justifying complexity and abstraction when building solutions - sometimes it is worth it if the problem warrants it, but often it is not and a simpler solution is better.

## Feedback Review
I believe it was overengineered, this is due to me having the requirements:
1. The initial task must work.
2. Must be able to support hierarchical/complex policies since they are shown on the Octopus Deploy Release Retention page + Teamcity experience.
3. Must be able to add more policy types without issue in the future.
4. Must have a verbose recording of actions to give back to the caller/handler.

Interfaces were used as an unknown type for the internal models, this makes sense in my eyes as the underlying Retention container is generic and can be easily be swapped for a concrete model.

I could improve on tests documentation and I missed out some use cases.

## UPDATE 23/9/21
* New Extension methods for adding to ServiceCollection
* New builder type for creating your ReleaseRetentionPolicyManager
* General moving of types around for improvements

### Features
* Base Retention library that can be used for other types.
* Highly extendable policy system with built in logical operators
* Hierarchically applied policies
* Policy references (Named policies)
* DI Integration
* Logging
* Highly detailed return result allowing the caller to analyze what policies failed

### TODO:
* Caching to speed up lookup for LastReleasedPolicy

### Exmaple of the power of policies with conditions
```cs
var basepolicy = new RetentionOrPolicy<IRelease>
(
    new RetentionAndPolicy<IRelease>
    (
        new RetentionStatePolicy<IRelease>("LastReleased"),
        new RetentionStatePolicy<IRelease>("TimeSinceReleased")
    ),
    new ReleasePinnedPolicy()
);

manager.SetBasePolicy(basepolicy);
manager.SetBaseStatePolicy("LastReleased", new LastReleasedPolicy(5));
manager.SetBaseStatePolicy("TimeSinceReleased", new TimeSinceReleasedPolicy(TimeSpan.FromDays(30)));
```
