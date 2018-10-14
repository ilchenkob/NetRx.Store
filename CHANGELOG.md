## Changelog
All notable changes to this project will be documented in this file.

#### 1.4 - 2018-10-15
##### Added
* Support of the pure function as a reducer. Previously, ```WithState``` method supported only the class inherited from ```NetRx.Store.Reducer``` as a reducer. But now there is one more override of this method that supports ```Func<TState, Action, TState>``` as a reducer.

#### 1.3.1 - 2018-10-10
##### Fixed
* Ability to create a string collection as a state field.

#### 1.3.0 - 2018-10-08
##### Added
* Internal wrappers for Reducer, Subscription and Effect. It slightly slows down the store initialization time. But at the same time gives significant improvement in Reducer and Effect invocations and their execution takes much less time.

#### 1.2.1 - 2018-10-04
##### Added
* Restriction for collections changed: all types from ```System.Collections.Immutable``` namespace are allowed.

#### 1.2.0 - 2018-10-03
##### Added
* Restriction for the state fields: only ```ImmutableArray``` type is allowed for collections.

#### 1.1.0 - 2018-10-01
##### Added
* Few optimization to the internal class ```StateWrapper```.

#### 1.0.0 - 2018-09-30
* Project has been published to Nuget and uploaded to GitHub.