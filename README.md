# Release Retention
## Octopus Deploy take home assessment.

### Features
* Base Retention library that can be used for other types.
* Highly extendable policy system with built in logical operators
* Hierarchically applied policies
* Policy references (Named policies)
* DI Integration
* Logging
* Highly detailed return result allowing the caller to analyze what policies failed

### TODO:
* Use builders to create policies to make neater, can remove passing RetentionContext down the policy chain
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
