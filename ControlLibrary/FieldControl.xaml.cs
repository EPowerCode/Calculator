using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ControlLibrary;
/// <summary>
/// Interaction logic for FieldControl.xaml
/// </summary>
public partial class FieldControl : UserControl
{
    public string Label
    {
        get { return (string)GetValue(LabelProperty); }
        set { SetValue(LabelProperty, value); }
    }

    public static readonly DependencyProperty LabelProperty =
        DependencyProperty.Register("Label", typeof(string), typeof(FieldControl), new PropertyMetadata(""));

    public string Value
    {
        get { return (string)GetValue(ValueProperty); }
        set { SetValue(ValueProperty, value); }
    }

    public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register("Value", typeof(string), typeof(FieldControl), new PropertyMetadata(""));


    public bool IsReadOnly
    {
        get { return (bool)GetValue(IsReadOnlyProperty); }
        set { SetValue(IsReadOnlyProperty, value); }
    }

    public static readonly DependencyProperty IsReadOnlyProperty =
        DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(FieldControl), new PropertyMetadata(false, OnIsReadOnlyChanged));

    // SharedSizeGroup names for label and value columns
    public string LabelSharedGroup
    {
        get => (string)GetValue(LabelSharedGroupProperty);
        set => SetValue(LabelSharedGroupProperty, value);
    }
    public static readonly DependencyProperty LabelSharedGroupProperty =
        DependencyProperty.Register("LabelSharedGroup", typeof(string), typeof(FieldControl), new PropertyMetadata(string.Empty, OnSharedGroupChanged));

    public string ValueSharedGroup
    {
        get => (string)GetValue(ValueSharedGroupProperty);
        set => SetValue(ValueSharedGroupProperty, value);
    }
    public static readonly DependencyProperty ValueSharedGroupProperty =
        DependencyProperty.Register("ValueSharedGroup", typeof(string), typeof(FieldControl), new PropertyMetadata(string.Empty, OnSharedGroupChanged));

    public FieldControl()
    {
        InitializeComponent();
        // set DataContext on the root element so internal bindings refer to the control, but external bindings on the control still resolve to the parent's DataContext
        RootGrid.DataContext = this;
        UpdateVisualState();
    }

    private static void OnIsReadOnlyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is FieldControl fc)
        {
            fc.UpdateVisualState();
        }
    }

    private static void OnSharedGroupChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is FieldControl fc)
        {
            fc.UpdateSharedGroups();
        }
    }

    private void UpdateVisualState()
    {
        var editable = FindName("EditablePanel") as FrameworkElement;
        var readOnly = FindName("ReadOnlyGrid") as FrameworkElement;

        if (IsReadOnly)
        {
            if (editable != null) editable.Visibility = Visibility.Collapsed;
            if (readOnly != null) readOnly.Visibility = Visibility.Visible;

            // enable shared size scope on nearest parent Grid so groups work across siblings (only if groups are specified)
            if (!string.IsNullOrEmpty(LabelSharedGroup) || !string.IsNullOrEmpty(ValueSharedGroup))
            {
                DependencyObject parent = VisualTreeHelper.GetParent(this);
                while (parent != null && !(parent is Grid))
                {
                    parent = VisualTreeHelper.GetParent(parent);
                }
                if (parent is Grid g)
                {
                    Grid.SetIsSharedSizeScope(g, true);
                }
            }
        }
        else
        {
            if (editable != null) editable.Visibility = Visibility.Visible;
            if (readOnly != null) readOnly.Visibility = Visibility.Collapsed;
        }
        UpdateSharedGroups();
    }

    private void UpdateSharedGroups()
    {
        var readOnly = FindName("ReadOnlyGrid") as Grid;
        if (readOnly != null)
        {
            // only set SharedSizeGroup when property provided
            if (!string.IsNullOrEmpty(LabelSharedGroup))
                readOnly.ColumnDefinitions[0].SharedSizeGroup = LabelSharedGroup;
            else
                readOnly.ColumnDefinitions[0].SharedSizeGroup = null;

            if (!string.IsNullOrEmpty(ValueSharedGroup))
                readOnly.ColumnDefinitions[1].SharedSizeGroup = ValueSharedGroup;
            else
                readOnly.ColumnDefinitions[1].SharedSizeGroup = null;
        }
    }
}
