# NetRx.Store Monitor

*Inspired by [Redux DevTools Extension](https://github.com/zalmoxisus/redux-devtools-extension)*

This extension helps to debug an application which uses [NetRx.Store](https://www.nuget.org/packages/NetRx.Store) for state management.
After installation you can find it in "Debug" -> "Windows" menu:

![menu](https://user-images.githubusercontent.com/2301026/66945984-48be6d00-f050-11e9-804d-d5e5125c5988.png)

It helps to track dispatched actions and state changes. Its main window is presented on the screenshot below.

![main_window](https://user-images.githubusercontent.com/2301026/66946338-05183300-f051-11e9-9d79-ea5ebf343518.PNG)

#### How to use it:
1. Make sure that your project uses [NetRx.Store](https://www.nuget.org/packages/NetRx.Store) of version 2.0 or above.
2. Run your application in "Debug" or attach the debugger to it.
3. Open NetRx.Store Monitor and it will be attached to the application automatically. So it will show the state changes in a real-time.