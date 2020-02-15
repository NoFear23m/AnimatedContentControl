Imports WpfTestContentControl.Instrastructure

Public Class SwitchContentViewModel
    Inherits ViewModelBase


    Public Sub New()
        AviableContents = New List(Of Object)
        AviableContents.Add(New Objects.PersonViewModel() With {.Fullname = "Bill Gates",
                            .Birthday = New Date(1955, 10, 28),
                            .Job = "US-amerikanischer Unternehmer, Programmierer und Mäzen",
                            .ObjectImageUri = "https://upload.wikimedia.org/wikipedia/commons/thumb/2/2d/Bill_Gates_2014.jpg/800px-Bill_Gates_2014.jpg"})
        AviableContents.Add(New Objects.PersonViewModel() With {.Fullname = "Steve Jobs",
                            .Birthday = New Date(1955, 2, 24),
                            .Job = "US-amerikanischer Unternehmer",
                            .ObjectImageUri = "https://upload.wikimedia.org/wikipedia/commons/thumb/f/f5/Steve_Jobs_Headshot_2010-CROP2.jpg/800px-Steve_Jobs_Headshot_2010-CROP2.jpg"})
        CurrentContent = AviableContents.First
    End Sub

    Private _aviableContents As List(Of Object)
    Public Property AviableContents As List(Of Object)
        Get
            Return _aviableContents
        End Get
        Set(ByVal value As List(Of Object))
            _aviableContents = Value
            RaisePropertyChanged()
        End Set
    End Property

    Private _currentContent As Object
    Public Property CurrentContent As Object
        Get
            Return _currentContent
        End Get
        Set(ByVal value As Object)
            _currentContent = Value
            RaisePropertyChanged()
        End Set
    End Property



End Class
