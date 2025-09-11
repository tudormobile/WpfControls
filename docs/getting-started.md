# Getting Started

### Install the package
The full package release includes all of the controls, adorners, and behaviors. These features are installed
using the *Tudormobile.WpfControls* package and contained in a single library asembly. Alternatively,
if you are only interested in adding additional functionality to existing controls via *behaviors* (i.e., 
attached properties), you can choose the install the *Tudormobile.Wpf.Behaviors* package. 
```
dotnet add package Tudormobile.WpfControls
```
-or-
```
dotnet add package Tudormobile.Wpf.Behaviors
dotnet add package Tudormobile.Wpf.Converters
```

### Prerequisites
**NONE**

### Dependencies
**NONE**

#### Inlcuding library provided services:
```
using Tudormobile.Wpf.Controls;
using Tudormobnile.Wpf.Converters;
using Tudormobile.Wpf.Behaviors;
using Tudormobile.Wpf.Adorners;
```

```
xmlns:controls="clr-namespace:Tudormobile.Wpf.Controls;assembly=Tudormobile.WpfControls"
xmlns:behaviors="clr-namespace:Tudormobile.Wpf.Behaviors;assembly=Tudormobile.WpfControls"
xmlns:adorners="clr-namespace:Tudormobile.Wpf.Adorners;assembly=Tudormobile.WpfControls"
```
-or-
```
xmlns:behaviors="clr-namespace:Tudormobile.Wpf.Behaviors;assembly=Tudormobile.Wpf.Behaviors"
xmlns:adorners="clr-namespace:Tudormobile.Wpf.Adorners;assembly=Tudormobile.Wpf.Behaviors"
xmlns:converters="clr-namespace:Tudormobile.Wpf.Converters;assembly=Tudormobile.Wpf.Converters"
```
