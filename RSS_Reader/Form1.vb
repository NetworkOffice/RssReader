Public Class Form1
    Dim aa As New RssItem
    Dim arr As ArrayList
    Dim deshtml
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If (tRssAddress.Text = "") Then
            MsgBox("The RSS Address cannot be empty!", , "Warning")
        Else
            Dim rss As RssItem
            rss = New RssItem()
            Dim rssCh As RSSChannel
            rssCh = New RSSChannel(tRssAddress.Text)
            'rssCh.GetChannelInfo()
            If Not rssCh.m_Error Then
                Label2.Visible = True
                Label3.Visible = True
                LinkLabel3.Visible = True
                Label2.Text = rssCh.m_Title
                Label3.Text = rssCh.m_Description
                LinkLabel3.Text = rssCh.m_Link
                arr = rssCh.GetChannelItems()
                For Each Me.aa In arr
                    ListBox1.Items.Add(aa.m_Title)
                Next
            End If
        End If
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBox1.SelectedIndexChanged
        LinkLabel1.Visible = True
        Dim i As Integer = ListBox1.SelectedIndex
        aa = arr.Item(i)
        LinkLabel1.Text = aa.m_link
        WebBrowser1.Document.Body.InnerHtml = aa.m_Description
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Label2.Visible = False
        Label3.Visible = False
        LinkLabel1.Visible = False
        LinkLabel3.Visible = False
        WebBrowser1.Navigate("about:blank")
    End Sub

    Private Sub LinkLabel1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkLabel1.Click
        Shell("C:\Program Files\Internet Explorer\iexplore.exe" + " " + LinkLabel1.Text, AppWinStyle.NormalFocus)
    End Sub

    Private Sub LinkLabel3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkLabel3.Click
        Shell("C:\Program Files\Internet Explorer\iexplore.exe" + " " + LinkLabel3.Text, AppWinStyle.NormalFocus)
    End Sub
End Class
Class RssItem
    Public m_Title As String
    Public m_link As String
    Public m_Description As String
    Public Sub New()
        m_Title = ""
        m_link = ""
        m_Description = ""
    End Sub
End Class
Class RSSChannel
    Public m_FeedURL As String
    Public m_Title As String
    Public m_Link As String
    Public m_Description As String
    Public m_Error As Boolean
    Public Sub New(ByVal url As String)
        m_Title = ""
        m_Link = ""
        m_Description = ""
        If CheckRSSUrlAvailable(url) Then
            m_Error = False
            m_FeedURL = url
            GetChannelInfo()
        Else
            MsgBox("Thr URL that you type is not available")
            m_Error = True
        End If
    End Sub
    Public Sub GetChannelInfo()
        Dim rss As Xml.XmlNodeList = GetXMLDoc("rss/channel")
        m_Title = rss(0).SelectSingleNode("title").InnerText
        m_Link = rss(0).SelectSingleNode("link").InnerText
        m_Description = rss(0).SelectSingleNode("description").InnerText
    End Sub
    Private Function GetXMLDoc(ByVal node As String) As Xml.XmlNodeList
        Dim tempNodeList As System.Xml.XmlNodeList = Nothing
        Dim request As System.Net.WebRequest = System.Net.WebRequest.Create(Me.m_FeedURL)
        Dim response As System.Net.WebResponse = request.GetResponse()
        Dim rssStream As System.IO.Stream = response.GetResponseStream()
        Dim rssDoc As Xml.XmlDocument = New Xml.XmlDocument()
        rssDoc.Load(rssStream)
        tempNodeList = rssDoc.SelectNodes(node)
        Return tempNodeList
    End Function
    Public Function GetChannelItems() As ArrayList
        Dim tempArrayList As New ArrayList
        Dim rssItems As Xml.XmlNodeList = GetXMLDoc("rss/channel/item")
        Dim item As Xml.XmlNode
        For Each item In rssItems
            Dim newItem As New RSSItem
            With newItem
                .m_Title = item.SelectSingleNode("title").InnerText
                .m_link = item.SelectSingleNode("link").InnerText
                .m_Description = item.SelectSingleNode("description").InnerText
            End With
            tempArrayList.Add(newItem)
        Next

        Return tempArrayList
    End Function
    Public Function CheckRSSUrlAvailable(ByVal Url As String) As Boolean
        Dim request As System.Net.WebRequest
        Try
            request = System.Net.WebRequest.Create(Url)
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function
End Class