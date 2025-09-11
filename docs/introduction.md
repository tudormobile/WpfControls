# Introduction
**Tudormobile.WpfControls** provides a collection of controls for use in applications utilizing the WPF framework.

```
using Tudormobile.Wpf.Controls;
```

## Tudormobile.Wpf.Controls
This namespace contains a number of additional controls not included in the base library. Most of the controls are designed to be compatible with the original WPF themes as well as the more modern dark/light Fluent Design schemes found in framework 9 and later. You can also provide your own control templates to match the look and feel of your application.

### Tudormobile.Wpf.Converters
A collection of useful converters (*IValueConverter*) and multi-value converters (*IMultiValueConverter*) for Wpf applications typically used in data binding scenarios.

### Tudormobile.Wpf.Behaviors
In addition to a set of new controls, a number of *behaviors* are provided to attach and extend existing controls. For example, you can add popup content to a button, effectively creating a *SplitButton* or *MenuButton* by extending the existing *Button* control (using existing or your own templates). Most behaviors are non-visible; the ones that include a visible element are stylized in a compatible manner as much as possible.

### Tudormobile.Wpf.Adorners
A set of *adorners* are provided for use with behaviors that include interactive visuals. They may aso be useful in your own applications.

## Gallery Application
A *Gallery* sample application is provided to showcase some of the controls and behaviors, and also to provide code examples using the library.
> [!NOTE]
> The Gallery application is currently in early development.
