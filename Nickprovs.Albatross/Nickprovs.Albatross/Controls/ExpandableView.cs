using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Nickprovs.Albatross.Controls
{
    public class ExpandableView : StackLayout
    {
        public const string ExpandAnimationName = nameof(ExpandAnimationName);

        public event EventHandler<StatusChangedEventArgs> StatusChanged;

        public event Action Tapped;

        public static readonly BindableProperty PrimaryViewProperty = BindableProperty.Create(nameof(PrimaryView), typeof(View), typeof(ExpandableView), null, propertyChanged: (bindable, oldValue, newValue) =>
        {
            (bindable as ExpandableView).SetPrimaryView(oldValue as View);
            (bindable as ExpandableView).OnTouchHandlerViewChanged();
        });

        public static readonly BindableProperty SecondaryViewTemplateProperty = BindableProperty.Create(nameof(SecondaryViewTemplate), typeof(DataTemplate), typeof(ExpandableView), null, propertyChanged: (bindable, oldValue, newValue) =>
        {
            (bindable as ExpandableView).SetSecondaryView(true);
            (bindable as ExpandableView).OnIsExpandedChanged();
        });

        public static readonly BindableProperty IsExpandedProperty = BindableProperty.Create(nameof(IsExpanded), typeof(bool), typeof(ExpandableView), default(bool), BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) =>
        {
            (bindable as ExpandableView).SetSecondaryView();
            (bindable as ExpandableView).OnIsExpandedChanged();
        });

        public static readonly BindableProperty TouchHandlerViewProperty = BindableProperty.Create(nameof(TouchHandlerView), typeof(View), typeof(ExpandableView), null, propertyChanged: (bindable, oldValue, newValue) =>
        {
            (bindable as ExpandableView).OnTouchHandlerViewChanged();
        });

        public static readonly BindableProperty ContainingScrollViewerProperty = BindableProperty.Create(nameof(ContainingScrollViewer), typeof(ScrollView), typeof(ExpandableView), null);

        public static readonly BindableProperty IsTouchToExpandEnabledProperty = BindableProperty.Create(nameof(IsTouchToExpandEnabled), typeof(bool), typeof(ExpandableView), true);

        public static readonly BindableProperty SecondaryViewHeightRequestProperty = BindableProperty.Create(nameof(SecondaryViewHeightRequest), typeof(double), typeof(ExpandableView), -1.0);

        public static readonly BindableProperty ExpandAnimationLengthProperty = BindableProperty.Create(nameof(ExpandAnimationLength), typeof(uint), typeof(ExpandableView), 250u);

        public static readonly BindableProperty CollapseAnimationLengthProperty = BindableProperty.Create(nameof(CollapseAnimationLength), typeof(uint), typeof(ExpandableView), 250u);

        public static readonly BindableProperty ExpandAnimationEasingProperty = BindableProperty.Create(nameof(ExpandAnimationEasing), typeof(Easing), typeof(ExpandableView), Easing.SinOut);

        public static readonly BindableProperty CollapseAnimationEasingProperty = BindableProperty.Create(nameof(CollapseAnimationEasing), typeof(Easing), typeof(ExpandableView), Easing.SinIn);

        public static readonly BindableProperty StatusProperty = BindableProperty.Create(nameof(Status), typeof(ExpandStatus), typeof(ExpandableView), default(ExpandStatus), BindingMode.OneWayToSource);

        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ExpandableView), default(ICommand));

        public static readonly BindableProperty ForceUpdateSizeCommandProperty = BindableProperty.Create(nameof(ForceUpdateSizeCommand), typeof(ICommand), typeof(ExpandableView), default(ICommand), BindingMode.OneWayToSource);

        private readonly TapGestureRecognizer _defaultTapGesture;
        private bool _shouldIgnoreAnimation;
        private double _lastVisibleHeight = -1;
        private double _startHeight;
        private double _endHeight;
        private View _secondaryView;

        public ExpandableView()
        {
            _defaultTapGesture = new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    Command?.Execute(null);
                    Tapped?.Invoke();
                    if (!IsTouchToExpandEnabled)
                    {
                        return;
                    }
                    IsExpanded = !IsExpanded;
                })
            };

            ForceUpdateSizeCommand = new Command(ForceUpdateSize);
        }

        public View PrimaryView
        {
            get => GetValue(PrimaryViewProperty) as View;
            set => SetValue(PrimaryViewProperty, value);
        }

        public DataTemplate SecondaryViewTemplate
        {
            get => GetValue(SecondaryViewTemplateProperty) as DataTemplate;
            set => SetValue(SecondaryViewTemplateProperty, value);
        }

        public bool IsExpanded
        {
            get => (bool)GetValue(IsExpandedProperty);
            set => SetValue(IsExpandedProperty, value);
        }

        public View TouchHandlerView
        {
            get => GetValue(TouchHandlerViewProperty) as View;
            set => SetValue(TouchHandlerViewProperty, value);
        }

        public ScrollView ContainingScrollViewer
        {
            get => GetValue(ContainingScrollViewerProperty) as ScrollView;
            set => SetValue(ContainingScrollViewerProperty, value);
        }

        public bool IsTouchToExpandEnabled
        {
            get => (bool)GetValue(IsTouchToExpandEnabledProperty);
            set => SetValue(IsTouchToExpandEnabledProperty, value);
        }

        public double SecondaryViewHeightRequest
        {
            get => (double)GetValue(SecondaryViewHeightRequestProperty);
            set => SetValue(SecondaryViewHeightRequestProperty, value);
        }

        public uint ExpandAnimationLength
        {
            get => (uint)GetValue(ExpandAnimationLengthProperty);
            set => SetValue(ExpandAnimationLengthProperty, value);
        }

        public uint CollapseAnimationLength
        {
            get => (uint)GetValue(CollapseAnimationLengthProperty);
            set => SetValue(CollapseAnimationLengthProperty, value);
        }

        public Easing ExpandAnimationEasing
        {
            get => (Easing)GetValue(ExpandAnimationEasingProperty);
            set => SetValue(ExpandAnimationEasingProperty, value);
        }

        public Easing CollapseAnimationEasing
        {
            get => (Easing)GetValue(CollapseAnimationEasingProperty);
            set => SetValue(CollapseAnimationEasingProperty, value);
        }

        public ExpandStatus Status
        {
            get => (ExpandStatus)GetValue(StatusProperty);
            set => SetValue(StatusProperty, value);
        }

        public ICommand Command
        {
            get => GetValue(CommandProperty) as ICommand;
            set => SetValue(CommandProperty, value);
        }

        public ICommand ForceUpdateSizeCommand
        {
            get => GetValue(ForceUpdateSizeCommandProperty) as ICommand;
            set => SetValue(ForceUpdateSizeCommandProperty, value);
        }

        public View SecondaryView
        {
            get => _secondaryView;
            private set
            {
                if (_secondaryView != null)
                {
                    _secondaryView.SizeChanged -= OnSecondaryViewSizeChanged;
                    Children.Remove(_secondaryView);
                }
                if (value != null)
                {
                    if (value is Layout layout)
                    {
                        layout.IsClippedToBounds = true;
                    }
                    value.HeightRequest = 0;
                    value.IsVisible = false;
                    Children.Add(_secondaryView = value);
                }
            }
        }

        public void ForceUpdateSize()
        {
            _lastVisibleHeight = -1;

            if (SecondaryView == null)
            {
                return;
            }

            OnIsExpandedChanged();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            _lastVisibleHeight = -1;
        }

        private void OnIsExpandedChanged()
        {
            if (SecondaryView == null)
            {
                return;
            }

            SecondaryView.SizeChanged -= OnSecondaryViewSizeChanged;

            var isExpanding = SecondaryView.AnimationIsRunning(ExpandAnimationName);
            SecondaryView.AbortAnimation(ExpandAnimationName);


            _startHeight = SecondaryView.IsVisible
                ? Math.Max(SecondaryView.Height - (SecondaryView is Layout l
                                    ? l.Padding.Top + l.Padding.Bottom
                                    : 0), 0)
                : 0;

            if (IsExpanded)
            {
                SecondaryView.IsVisible = true;
            }

            _endHeight = SecondaryViewHeightRequest >= 0
                ? SecondaryViewHeightRequest
                : _lastVisibleHeight;

            var shouldInvokeAnimation = true;

            if (IsExpanded)
            {
                if (_endHeight <= 0)
                {
                    shouldInvokeAnimation = false;
                    SecondaryView.HeightRequest = -1;
                    SecondaryView.SizeChanged += OnSecondaryViewSizeChanged;
                }
            }
            else
            {
                _lastVisibleHeight = _startHeight = SecondaryViewHeightRequest >= 0
                        ? SecondaryViewHeightRequest
                            : !isExpanding
                                 ? SecondaryView.Height - (SecondaryView is Layout lay
                                    ? lay.Padding.Top + lay.Padding.Bottom
                                    : 0)
                                  : _lastVisibleHeight;
                _endHeight = 0;
            }

            _shouldIgnoreAnimation = Height < 0;

            if (shouldInvokeAnimation)
            {
                InvokeAnimation();
            }
        }

        private void OnTouchHandlerViewChanged()
        {
            var gesturesList = (TouchHandlerView ?? PrimaryView)?.GestureRecognizers;
            gesturesList?.Remove(_defaultTapGesture);
            PrimaryView?.GestureRecognizers.Remove(_defaultTapGesture);
            gesturesList?.Add(_defaultTapGesture);
        }

        private void SetPrimaryView(View oldView)
        {
            if (oldView != null)
            {
                Children.Remove(oldView);
            }
            Children.Insert(0, PrimaryView);
        }

        private void SetSecondaryView(bool forceUpdate = false)
        {
            if (IsExpanded && (SecondaryView == null || forceUpdate))
            {
                SecondaryView = CreateSecondaryView();
            }
        }

        private View CreateSecondaryView()
        {
            var template = SecondaryViewTemplate;
            if (template is DataTemplateSelector selector)
            {
                template = selector.SelectTemplate(BindingContext, this);
            }
            return template?.CreateContent() as View;
        }

        private void OnSecondaryViewSizeChanged(object sender, EventArgs e)
        {
            if (SecondaryView.Height <= 0) return;
            SecondaryView.SizeChanged -= OnSecondaryViewSizeChanged;
            SecondaryView.HeightRequest = 0;
            _endHeight = SecondaryView.Height;
            InvokeAnimation();
        }

        private void InvokeAnimation()
        {
            RaiseStatusChanged(IsExpanded ? ExpandStatus.Expanding : ExpandStatus.Collapsing);

            if (_shouldIgnoreAnimation)
            {
                RaiseStatusChanged(IsExpanded ? ExpandStatus.Expanded : ExpandStatus.Collapsed);
                SecondaryView.HeightRequest = _endHeight;
                SecondaryView.IsVisible = IsExpanded;
                return;
            }

            var length = ExpandAnimationLength;
            var easing = ExpandAnimationEasing;
            if (!IsExpanded)
            {
                length = CollapseAnimationLength;
                easing = CollapseAnimationEasing;
            }

            if (_lastVisibleHeight > 0)
            {
                length = Math.Max((uint)(length * (Math.Abs(_endHeight - _startHeight) / _lastVisibleHeight)), 1);
            }

            //If the consumer linked a scrollviewer
            if (this.ContainingScrollViewer != null)
            {
                //Check to see if the secondary view + it's to-be height will be below the scrollviewer...
                //If so... skip the animation (because it doesn't scroll) and just give the secondary view a height now and scroll to it.
                if(this.IsExpanded && this.GetScreenCoordinates(this.ContainingScrollViewer).Y + this.ContainingScrollViewer.Height < this.GetScreenCoordinates(this._secondaryView).Y + this._endHeight)
                {
                    this._secondaryView.HeightRequest = _endHeight;
                    this.ContainingScrollViewer.ScrollToAsync(this.SecondaryView, ScrollToPosition.Start, true);
                    RaiseStatusChanged(ExpandStatus.Expanded);
                    return;
                }
            }

            new Animation(v => SecondaryView.HeightRequest = v, _startHeight, _endHeight)
                .Commit(SecondaryView, ExpandAnimationName, 16, length, easing, (value, interrupted) =>
                {
                    if (interrupted)
                    {
                        return;
                    }
                    if (!IsExpanded)
                    {
                        SecondaryView.IsVisible = false;
                        RaiseStatusChanged(ExpandStatus.Collapsed);
                        return;
                    }
                    RaiseStatusChanged(ExpandStatus.Expanded);
                });
        }

        private void RaiseStatusChanged(ExpandStatus status)
        {
            Status = status;
            StatusChanged?.Invoke(this, new StatusChangedEventArgs(status));
        }

        //Helper
        public (double X, double Y) GetScreenCoordinates(VisualElement view)
        {
            // A view's default X- and Y-coordinates are LOCAL with respect to the boundaries of its parent,
            // and NOT with respect to the screen. This method calculates the SCREEN coordinates of a view.
            // The coordinates returned refer to the top left corner of the view.

            // Initialize with the view's "local" coordinates with respect to its parent
            double screenCoordinateX = view.X;
            double screenCoordinateY = view.Y;

            // Get the view's parent (if it has one...)
            if (view.Parent.GetType() != typeof(App))
            {
                VisualElement parent = (VisualElement)view.Parent;


                // Loop through all parents
                while (parent != null)
                {
                    // Add in the coordinates of the parent with respect to ITS parent
                    screenCoordinateX += parent.X;
                    screenCoordinateY += parent.Y;

                    // If the parent of this parent isn't the app itself, get the parent's parent.
                    if (parent.Parent.GetType() == typeof(App))
                        parent = null;
                    else
                        parent = (VisualElement)parent.Parent;
                }
            }

            // Return the final coordinates...which are the global SCREEN coordinates of the view
            return (screenCoordinateX, screenCoordinateY);
        }
    }
}
