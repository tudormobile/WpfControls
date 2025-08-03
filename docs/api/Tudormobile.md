## WpfControls Library Namespaces
The namespaces available in the *Tudormobile.WpfControls* library are described below. In several cases, including the namespace is required to expose extension methods provided.

### Tudormobile.WpfControls.dll

```
using Tudormobile.WpfControls;
using Tudormobile.WpfControls.Converters;
using Tudormobile.WpfControls.Commands;
using Tudormobile.WpfCongtrols.Services;
```

- [Tudormobile.Wpf.Controls](Tudormobile.Wpf.Controls.yml)
    - Root namespace for the WpfApp library.  
This is where you will find all of the custom control provided by the library.
- [Tudormobile.Wpf.Converters](Tudormobile.Wpf.Converters.yml)
    - Value converters.  
A number of value converters used by these controls are found here. They may be useful in other scenarios within your applications..
- [Tudormobile.Wpf.Commands](Tudormobile.Wpf.Commands.yml)
    - Built-in library commands.  
    Commands are available for working with the proided controls. Includes support for automatically associating methods in your classes to ICommand properties on view models without having to write boilerplate code.
    
# Release [!include[version](../../src/WpfControls/bin/release/ver.txt)]
Latest unit testing results are shown below.
[!include[summary](../../output/SummaryGithub.md)]