
Imports System.Windows.Media.Animation

<TemplatePart(Name:="PART_PaintArea", Type:=GetType(Shape))>
<TemplatePart(Name:="PART_MainContent", Type:=GetType(ContentPresenter))>
Public Class AnimatedContentControl
    Inherits ContentControl




    Shared Sub New()
        DefaultStyleKeyProperty.OverrideMetadata(GetType(AnimatedContentControl), New FrameworkPropertyMetadata(GetType(AnimatedContentControl)))
    End Sub

    Private _paintArea As Shape
    Private _mainContent As ContentPresenter



    Public Property AnimationType As AnimType
        Get
            Return CType(GetValue(AnimationTypeProperty), AnimType)
        End Get

        Set(ByVal value As AnimType)
            SetValue(AnimationTypeProperty, value)
        End Set
    End Property

    Public Shared ReadOnly AnimationTypeProperty As DependencyProperty =
                           DependencyProperty.Register("AnimationType",
                           GetType(AnimType), GetType(AnimatedContentControl),
                           New PropertyMetadata(AnimType.SlideLeft))



    Public Property AnimationDuration As TimeSpan
        Get
            Return CType(GetValue(AnimationDurationProperty), TimeSpan)
        End Get

        Set(ByVal value As TimeSpan)
            SetValue(AnimationDurationProperty, value)
        End Set
    End Property

    Public Shared ReadOnly AnimationDurationProperty As DependencyProperty =
                           DependencyProperty.Register("AnimationDuration",
                           GetType(TimeSpan), GetType(AnimatedContentControl),
                           New PropertyMetadata(TimeSpan.FromSeconds(0.5)))



    Public Property EaseAnimation As Boolean
        Get
            Return CBool(GetValue(EaseAnimationProperty))
        End Get

        Set(ByVal value As Boolean)
            SetValue(EaseAnimationProperty, value)
        End Set
    End Property

    Public Shared ReadOnly EaseAnimationProperty As DependencyProperty =
                           DependencyProperty.Register("EaseAnimation",
                           GetType(Boolean), GetType(AnimatedContentControl),
                           New PropertyMetadata(True))



    Public Overrides Sub OnApplyTemplate()
        _paintArea = CType(Template.FindName("PART_PaintArea", Me), Shape)
        _mainContent = CType(Template.FindName("PART_MainContent", Me), ContentPresenter)
        MyBase.OnApplyTemplate()
    End Sub

    Protected Overrides Sub OnContentChanged(oldContent As Object, newContent As Object)
        If _paintArea IsNot Nothing AndAlso _mainContent IsNot Nothing AndAlso AnimationType <> AnimType.Switch Then
            _paintArea.Fill = CreateBrushFromVisual(_mainContent)
            BeginAnimation()
        End If

        MyBase.OnContentChanged(oldContent, newContent)
    End Sub

    Private Overloads Sub BeginAnimation()
        Dim newContentTransform = New TranslateTransform
        Dim oldContentTransform = New TranslateTransform

        _paintArea.RenderTransform = oldContentTransform
        _mainContent.RenderTransform = newContentTransform
        _paintArea.Visibility = Visibility.Visible

        Select Case DirectCast(GetValue(AnimationTypeProperty), AnimType)
            Case AnimType.SlideLeft
                newContentTransform.BeginAnimation(TranslateTransform.XProperty, CreateAnimation(Me.ActualWidth, 0))
                oldContentTransform.BeginAnimation(TranslateTransform.XProperty, CreateAnimation(0, -Me.ActualWidth, Sub(s, e) _paintArea.Visibility = Visibility.Hidden))
            Case AnimType.SlideRight
                newContentTransform.BeginAnimation(TranslateTransform.XProperty, CreateAnimation(-Me.ActualWidth, 0))
                oldContentTransform.BeginAnimation(TranslateTransform.XProperty, CreateAnimation(0, Me.ActualWidth, Sub(s, e) _paintArea.Visibility = Visibility.Hidden))
            Case AnimType.SlideUp
                newContentTransform.BeginAnimation(TranslateTransform.YProperty, CreateAnimation(Me.ActualHeight, 0))
                oldContentTransform.BeginAnimation(TranslateTransform.YProperty, CreateAnimation(0, -Me.ActualHeight, Sub(s, e) _paintArea.Visibility = Visibility.Hidden))
            Case AnimType.SlideDown
                newContentTransform.BeginAnimation(TranslateTransform.YProperty, CreateAnimation(-Me.ActualHeight, 0))
                oldContentTransform.BeginAnimation(TranslateTransform.YProperty, CreateAnimation(0, Me.ActualHeight, Sub(s, e) _paintArea.Visibility = Visibility.Hidden))
            Case AnimType.Switch

        End Select


    End Sub
    Private Function CreateAnimation(ByVal fromValue As Double, ByVal toValue As Double, Optional ByVal whenDone As EventHandler = Nothing) As AnimationTimeline
        Dim ease As IEasingFunction = New BackEase() With {.Amplitude = 0.5, .EasingMode = EasingMode.EaseInOut}
        Dim duration = New Duration(AnimationDuration)
        Dim anim = New DoubleAnimation(fromValue, toValue, duration)
        If EaseAnimation Then anim.EasingFunction = ease
        If whenDone IsNot Nothing Then AddHandler anim.Completed, whenDone
        anim.Freeze()
        Return anim
    End Function
    Private Function CreateBrushFromVisual(mainContent As Visual) As Brush
        If mainContent Is Nothing Then Throw New ArgumentNullException(NameOf(mainContent))
        Dim target = New RenderTargetBitmap(CType(ActualWidth, Integer), CType(ActualHeight, Integer), 96, 96, PixelFormats.Pbgra32)
        target.Render(mainContent)
        Dim brush = New ImageBrush(target)
        brush.Freeze()
        Return brush
    End Function
End Class


Public Enum AnimType
    SlideLeft = 0
    SlideRight = 1
    SlideUp = 2
    SlideDown = 3
    Switch = 4

End Enum

