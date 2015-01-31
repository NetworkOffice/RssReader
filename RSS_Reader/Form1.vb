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
            rssCh.GetChannelInfo()
            Label2.Text = rssCh.m_Title
            Label3.Text = rssCh.m_Description
            LinkLabel3.Text = rssCh.m_Link
            arr = rssCh.GetChannelItems()
            For Each Me.aa In arr
                ListBox1.Items.Add(aa.m_Title)
            Next
        End If
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBox1.SelectedIndexChanged
        Dim i As Integer = ListBox1.SelectedIndex
        aa = arr.Item(i)
        LinkLabel1.Text = aa.m_link
        'WebBrowser1.Refresh()
        'WebBrowser1.Document.clear()
        WebBrowser1.Document.Body.InnerHtml = aa.m_Description
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        WebBrowser1.Navigate("about:blank")
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
    Public Sub New(ByVal url As String)
        m_FeedURL = url
        m_Title = ""
        m_Link = ""
        m_Description = ""
        GetChannelInfo()
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
End Class