using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Navigation;
using Tudormobile.Wpf.Helpers;

namespace Tudormobile.Wpf.Controls;

/// <summary>
/// Represents a wizard-style navigation window that allows users to navigate through a sequence of pages.
/// </summary>
/// <remarks>The <see cref="Wizard"/> class provides a framework for creating multi-step workflows or wizards.
/// Pages can be added to the <see cref="Pages"/> collection, and the wizard will handle navigation between them. The
/// wizard supports forward navigation, resizing, and a finish button to complete the workflow.  The control uses a
/// custom style and template, which can be overridden. Template parts include: <list type="bullet">
/// <item><term>PART_ForwardButton</term>: A <see cref="Button"/> for navigating to the next page.</item>
/// <item><term>PART_FinishButton</term>: A <see cref="Button"/> for completing or canceling the wizard.</item>
/// <item><term>PART_ResizeGrip</term>: A <see cref="ResizeGrip"/> for resizing the window.</item> </list></remarks>
public partial class Wizard : NavigationWindow
{
    private Button? _forwardButton;
    private Button? _finishButton;
    private ResizeGrip? _resizeGrip;

    static Wizard()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(Wizard), new FrameworkPropertyMetadata(typeof(Wizard)));
    }

    /// <summary>
    /// True if UI should allow forward navigation.
    /// </summary>
    public bool IsForwardButtonEnabled
    {
        get { return (bool)GetValue(IsForwardButtonEnabledProperty); }
        set { SetValue(IsForwardButtonEnabledProperty, value); }
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public static readonly DependencyProperty IsForwardButtonEnabledProperty = DependencyProperty
        .Register(nameof(IsForwardButtonEnabled), typeof(bool), typeof(Wizard), new PropertyMetadata(true));

    /// <summary>
    /// Gets or sets the collection of pages associated with this instance.
    /// </summary>
    /// <remarks>Use this property to access or modify the pages. Changes to the collection will not
    /// automatically trigger updates unless explicitly handled.</remarks>
    public IList<Object> Pages
    {
        get { return (IList<Object>)GetValue(PagesProperty); }
        set { SetValue(PagesProperty, value); }
    }

    /// <inheritdoc/>
    public static readonly DependencyProperty PagesProperty = DependencyProperty
        .Register(nameof(Pages), typeof(IList<Object>), typeof(Wizard), new PropertyMetadata(null));

    /// <summary>
    /// Initializes a new instance of the <see cref="Wizard"/> class.
    /// </summary>
    /// <remarks>This constructor sets up the default style for the wizard by retrieving the style resource 
    /// associated with the <see cref="Wizard"/> type. It also initializes the <see cref="Pages"/>  collection and
    /// subscribes to the Loaded event to handle additional setup when  the wizard is loaded.</remarks>
    public Wizard()
    {
        Loaded += wizard_Loaded;
        Pages = new List<Object>();
        Style = (Style)Application.Current.FindResource(typeof(Wizard));
    }

    /// <inheritdoc/>
    public override void OnApplyTemplate()
    {
        _forwardButton = GetTemplateChild("PART_ForwardButton") as Button;
        _finishButton = GetTemplateChild("PART_FinishButton") as Button;
        _resizeGrip = GetTemplateChild("PART_ResizeGrip") as ResizeGrip;
    }

    private void wizard_Loaded(object sender, RoutedEventArgs e)
    {
        this.Navigated += wizard_Navigated;
        if (_forwardButton != null)
        {
            _forwardButton.IsEnabled = false;
            _forwardButton.Click += forwardButton_Click;
        }
        if (_finishButton != null)
        {
            _finishButton.Click += (s, args) =>
            {
                this.DialogResult = (this.NavigationService.Content == Pages?[Pages.Count - 1]);
                this.Close();
            };
        }
        if (_resizeGrip != null)
        {
            _resizeGrip.MouseLeftButtonDown += (s, args) =>
            {
                WindowHelpers.DragResize(this);
            };
        }
        if (Pages != null && Pages.Count > 0)
        {
            // Try to set the window size to the first page's size
            this.Width = this.MinWidth;
            this.Height = this.MinHeight;
            if (Pages[0] is Page firstPage)
            {
                this.Width = firstPage.Width > 0 ? firstPage.Width + 20 : this.Width;
                this.Height = firstPage.Height > 0 ? firstPage.Height + 60 : this.Height;
            }
            else if (Pages[0] is FrameworkElement element)
            {
                this.Width = element.Width > 0 ? element.Width + 20 : this.Width;
                this.Height = element.Height > 0 ? element.Height + 60 : this.Height;
            }
            Navigate(Pages[0]);
        }
    }

    private void wizard_Navigated(object sender, NavigationEventArgs e)
    {
        // Enable the forward button if there are more pages to navigate to
        int currentIndex = Pages?.IndexOf(e.Content) ?? -1;
        int totalPages = Pages?.Count ?? 0;
        if (_forwardButton != null && currentIndex >= 0)
        {
            _forwardButton.IsEnabled = currentIndex < totalPages - 1;
        }
        // Enable the finish button if we are on the last page
        if (_finishButton != null)
        {
            _finishButton.Content = (currentIndex >= 0 && currentIndex >= totalPages - 1) ? "Finish" : "Cancel";
        }
    }

    private void forwardButton_Click(object sender, RoutedEventArgs e)
    {
        if (Pages is not null && IsForwardButtonEnabled)
        {
            int currentIndex = Pages?.IndexOf(NavigationService.Content) ?? -1;
            int totalPages = Pages?.Count ?? 0;
            if (currentIndex >= 0 && currentIndex < totalPages - 1)
            {
                Navigate(Pages![currentIndex + 1]);
            }
        }
    }
}
